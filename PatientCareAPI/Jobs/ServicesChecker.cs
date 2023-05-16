using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Jobs
{
    [DisallowConcurrentExecution]
    public class ServicesChecker : IJob
    {
        private readonly ILogger<ServicesChecker> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ServicesChecker(ILogger<ServicesChecker> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var triggerDate = DateTime.Now;
            var triggerTime = triggerDate.ToString("HH:mm");
            _logger.LogInformation("Hello, world -> " + DateTime.Now);
            var patients = _unitOfWork.PatientRepository.GetRecords<PatientModel>(u => u.IsActive && !u.Iswaitingactivation);
            foreach (var patient in patients)
            {
                if(!string.IsNullOrWhiteSpace(patient.CheckperiodID))
                {
                    var patientcontrolperiodguids = _unitOfWork.CheckperiodtoPeriodRepository.GetRecords<CheckperiodtoPeriodModel>(u => u.CheckperiodID == patient.CheckperiodID).Select(u => u.PeriodID).ToList();
                    var patientControlperiods = _unitOfWork.PeriodRepository.GetPeriodsbyGuids(patientcontrolperiodguids);
                    if (patientControlperiods.FirstOrDefault(u => u.Occuredtime == triggerTime) != null)
                    {
                        var patientmovement = _unitOfWork.PatientmovementRepository.GetRecord<PatientmovementModel>(u => u.Movementdate == triggerDate);
                        if (patientmovement == null)
                        {
                            PatientmovementModel model = new PatientmovementModel();
                            string movementguid = Guid.NewGuid().ToString();
                            model.ConcurrencyStamp = movementguid;
                            model.CreatedUser = "ServiceChecker";
                            model.IsActive = true;
                            model.CreateTime = DateTime.Now;
                            model.IsTodoneed = !string.IsNullOrWhiteSpace(patient.TodogroupdefineID);
                            model.Movementdate = triggerDate;
                            model.Patientmovementtype = (int)Constants.Patienttypes.Kontrol;
                            model.PatientID = patient.ConcurrencyStamp;
                            _unitOfWork.PatientmovementRepository.Add(model);
                            if (!string.IsNullOrWhiteSpace(patient.TodogroupdefineID))
                            {
                                var patienttodoguids = _unitOfWork.TodogrouptoTodoRepository.GetRecords<TodogrouptoTodoModel>(u => u.GroupID == patient.TodogroupdefineID).Select(u=>u.TodoID).ToList();
                                var patienttodos = _unitOfWork.TododefineRepository.GetTodosbyGuids(patienttodoguids);
                                int counter = 0;
                                foreach (var tododefine in patienttodos)
                                {
                                    TodoModel todomodel = new TodoModel();
                                    string todoguid = Guid.NewGuid().ToString();
                                    todomodel.ConcurrencyStamp = todoguid;
                                    todomodel.CreatedUser = "ServiceChecker";
                                    todomodel.IsActive = true;
                                    todomodel.CreateTime = DateTime.Now;
                                    todomodel.MovementID = movementguid;
                                    todomodel.Order = counter;
                                    todomodel.TododefineID = tododefine.ConcurrencyStamp;
                                    counter++;
                                    _unitOfWork.TodoRepository.Add(todomodel);
                                _logger.LogInformation("Yapılacak Eklendi -> "+todomodel.ConcurrencyStamp );
                                }
                            }
                        }
                    }
                }
            }
            _unitOfWork.Complate();
            return Task.CompletedTask;
        }



    }
}
