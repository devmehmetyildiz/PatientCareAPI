using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Models.Warehouse;

namespace PatientCareAPI.Controllers.Application
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatientController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public PatientController(IConfiguration configuration, ILogger<PatientController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        private string GetSessionUser()
        {
            return (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
        }

        private List<PatientModel> FetchList(bool isactivated)
        {
            var List = unitOfWork.PatientRepository.GetRecords<PatientModel>(u => u.IsActive && isactivated? !u.Iswaitingactivation : u.Iswaitingactivation);
            foreach (var item in List)
            {
                item.Case = unitOfWork.CaseRepository.GetSingleRecord<CaseModel>(u => u.ConcurrencyStamp == item.CaseId);
                item.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
                item.Patientdefine = unitOfWork.PatientdefineRepository.GetSingleRecord<PatientdefineModel>(u => u.ConcurrencyStamp == item.PatientdefineID);
                item.Files = unitOfWork.FileRepository.GetRecords<FileModel>(u => u.Parentid == item.ConcurrencyStamp);
                item.Stocks = unitOfWork.PatientstocksRepository.GetRecords<PatientstocksModel>(u => u.IsActive && u.PatientID == item.ConcurrencyStamp);
                foreach (var stock in item.Stocks)
                {
                    double amount = 0;
                    var movements = unitOfWork.PatientstocksmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.StockID == item.ConcurrencyStamp);
                    foreach (var movement in movements)
                    {
                        amount += (movement.Amount * movement.Movementtype);
                    }
                    stock.Amount = amount;
                    stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == stock.StockdefineID);
                    stock.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == stock.Departmentid);
                    stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == stock.Stockdefine.Unitid);
                }
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList(true));
        }

        [Route("GetActivationlist")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [HttpGet]
        public IActionResult GetActivationlist()
        {
            return Ok(FetchList(false));
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            var model = unitOfWork.PatientRepository.GetSingleRecord<PatientModel>(u => u.IsActive && u.ConcurrencyStamp == guid);
            model.Case = unitOfWork.CaseRepository.GetSingleRecord<CaseModel>(u => u.ConcurrencyStamp == model.CaseId);
            model.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == model.Departmentid);
            model.Patientdefine = unitOfWork.PatientdefineRepository.GetSingleRecord<PatientdefineModel>(u => u.ConcurrencyStamp == model.PatientdefineID);
            model.Files = unitOfWork.FileRepository.GetRecords<FileModel>(u => u.Parentid == model.ConcurrencyStamp);
            model.Stocks = unitOfWork.PatientstocksRepository.GetRecords<PatientstocksModel>(u => u.IsActive && u.PatientID == model.ConcurrencyStamp);
            foreach (var stock in model.Stocks)
            {
                double amount = 0;
                var movements = unitOfWork.PatientstocksmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.StockID == stock.ConcurrencyStamp);
                foreach (var movement in movements)
                {
                    amount += (movement.Amount * movement.Movementtype);
                }
                stock.Amount = amount;
                stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == stock.StockdefineID);
                stock.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == stock.Departmentid);
                stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == stock.Stockdefine.Unitid);
            }
            return Ok(model);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Add)]
        [HttpPost]
        public IActionResult Add(PatientModel model)
        {
            var username = GetSessionUser();
            if (string.IsNullOrWhiteSpace(model.Patientdefine.ConcurrencyStamp))
            {
                string patientguid = Guid.NewGuid().ToString();
                model.Patientdefine.ConcurrencyStamp = patientguid;
                model.Patientdefine.CreatedUser = username;
                model.Patientdefine.IsActive = true;
                model.Patientdefine.CreateTime = DateTime.Now;
                model.PatientdefineID = patientguid;
                unitOfWork.PatientdefineRepository.Add(model.Patientdefine);
            }
            string guid = Guid.NewGuid().ToString();
            model.ConcurrencyStamp = guid;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            unitOfWork.PatientRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList(false));
        }

        //[Route("GetUserImage")]
        //[AllowAnonymous]
        //[HttpGet]
        //public IActionResult GetUserImage(string Guid)
        //{
        //    FileModel Data = unitOfWork.FileRepository.GetFilebyGuid(unitOfWork.ActivepatientRepository.FindByGuid(Guid).ImageID);
        //    if (Data != null)
        //        return File(Utilities.GetFile(Data), Data.Filetype);
        //    else
        //        return NotFound();
        //}

        //[Route("AddImage")]
        //[AuthorizeMultiplePolicy(UserAuthory.File_Add)]
        //[RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        //[HttpPost]
        //public IActionResult AddImage([FromForm] FileModel model)
        //{
        //    var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
        //    if (string.IsNullOrWhiteSpace(model.Filefolder))
        //    {
        //        model.Filefolder = Guid.NewGuid().ToString();
        //    }
        //    string imageguid = Guid.NewGuid().ToString();
        //    PatientModel patientmodel = unitOfWork.ActivepatientRepository.FindByGuid(model.ConcurrencyStamp);
        //    patientmodel.ImageID = imageguid;
        //    PatientdefineModel patient = unitOfWork.PatientdefineRepository.GetPatientByGuid(patientmodel.PatientID);
        //    unitOfWork.ActivepatientRepository.update(unitOfWork.ActivepatientRepository.FindByGuid(model.ConcurrencyStamp), patientmodel);
        //    model.CreatedUser = username;
        //    model.IsActive = true;
        //    model.CreateTime = DateTime.Now;
        //    model.ConcurrencyStamp = imageguid;
        //    model.Name = patient.Firstname+patient.Lastname;
        //    model.Filetype = model.File.ContentType;
        //    model.Filename = model.File.FileName;
        //    if (Utilities.UploadFile(model))
        //    {
        //        unitOfWork.FileRepository.Add(model);
        //        unitOfWork.Complate();
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}


        [Route("Preparestocks")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult Updatestocks(PatientModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            foreach (var stock in model.Stocks)
            {
                if (stock.ConcurrencyStamp != null && stock.ConcurrencyStamp != "")
                {
                    StockModel oldmodel = unitOfWork.StockRepository.GetSingleRecord<StockModel>(u => u.ConcurrencyStamp == stock.ConcurrencyStamp);
                    stock.UpdatedUser = username;
                    stock.UpdateTime = DateTime.Now;
                    unitOfWork.StockRepository.update(oldmodel, stock);
                }
                else
                {
                    string stockguid = Guid.NewGuid().ToString();
                    unitOfWork.PatientToStockRepostiyory.Add(new PatientToStockModel { PatientID = model.ConcurrencyStamp, StockID = stockguid });
                    stock.CreatedUser = username;
                    stock.CreateTime = DateTime.Now;
                    stock.IsActive = true;
                    stock.ConcurrencyStamp = stockguid;
                    unitOfWork.StockRepository.Add(stock);
                    unitOfWork.StockmovementRepository.Add(new StockmovementModel
                    {
                        Activestockid = stockguid,
                        Amount = stock.Amount,
                        Movementdate = DateTime.Now,
                        Movementtype = ((int)Constants.Movementtypes.Create),
                        Prevvalue = 0,
                        Newvalue = stock.Amount,
                        UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp,
                    });
                }
            }

            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Delete)]
        [HttpPost]
        public IActionResult Delete(PatientModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.PatientRepository.Remove(model.Id);
            unitOfWork.FileRepository.Removefilesbyguid(model.ConcurrencyStamp);
            var definesstocks = unitOfWork.PatientToStockRepostiyory.GetRecords<PatientToStockModel>(u => u.PatientID == model.ConcurrencyStamp).Select(u=>u.StockID).ToList();
            if (definesstocks.Count > 0)
            {
                 unitOfWork.StockRepository.RemoveStocksbyGuids(definesstocks);
            }
            unitOfWork.Complate();
            return Ok(FetchList(!model.Iswaitingactivation));
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PatientModel model)
        {
            unitOfWork.PatientRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
