﻿using Microsoft.AspNetCore.Authorization;
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
                item.Case = unitOfWork.CaseRepository.GetRecord<CaseModel>(u => u.ConcurrencyStamp == item.CaseId);
                item.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
                item.Patientdefine = unitOfWork.PatientdefineRepository.GetRecord<PatientdefineModel>(u => u.ConcurrencyStamp == item.PatientdefineID);
                if (item.Patientdefine != null)
                {
                    item.Patientdefine.Costumertype = unitOfWork.CostumertypeRepository.GetRecord<CostumertypeModel>(u => u.ConcurrencyStamp == item.Patientdefine.Costumertypeid);
                    item.Patientdefine.Patienttype = unitOfWork.PatienttypeRepository.GetRecord<PatienttypeModel>(u => u.ConcurrencyStamp == item.Patientdefine.Patienttypeid);
                }
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
                    stock.Stockdefine = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == stock.StockdefineID);
                    stock.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == stock.Departmentid);
                    stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == stock.Stockdefine.Unitid);
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
        [AuthorizeMultiplePolicy(UserAuthory.Preregistration_Screen)]
        [HttpGet]
        public IActionResult GetActivationlist()
        {
            return Ok(FetchList(false));
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Getselected))]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            var model = unitOfWork.PatientRepository.GetRecord<PatientModel>(u => u.IsActive && u.ConcurrencyStamp == guid);
            model.Case = unitOfWork.CaseRepository.GetRecord<CaseModel>(u => u.ConcurrencyStamp == model.CaseId);
            model.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == model.Departmentid);
            model.Patientdefine = unitOfWork.PatientdefineRepository.GetRecord<PatientdefineModel>(u => u.ConcurrencyStamp == model.PatientdefineID);
            if (model.Patientdefine != null)
            {
                model.Patientdefine.Costumertype = unitOfWork.CostumertypeRepository.GetRecord<CostumertypeModel>(u => u.ConcurrencyStamp == model.Patientdefine.Costumertypeid);
                model.Patientdefine.Patienttype = unitOfWork.PatienttypeRepository.GetRecord<PatienttypeModel>(u => u.ConcurrencyStamp == model.Patientdefine.Patienttypeid);
            }
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
                stock.Stockdefine = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == stock.StockdefineID);
                stock.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == stock.Departmentid);
                stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == stock.Stockdefine.Unitid);
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
           
            PatientmovementModel movementmodel = new PatientmovementModel();
            string movementguid = Guid.NewGuid().ToString();
            movementmodel.ConcurrencyStamp = movementguid;
            movementmodel.CreatedUser = "ServiceChecker";
            movementmodel.IsActive = true;
            movementmodel.CreateTime = DateTime.Now;
            movementmodel.IsTodoneed = false;
            movementmodel.Movementdate = DateTime.Now;
            movementmodel.PatientID = model.ConcurrencyStamp;
            movementmodel.Patientmovementtype = (int)Constants.Patienttypes.İlkKayıt;
            unitOfWork.PatientmovementRepository.Add(movementmodel);
            
            unitOfWork.Complate();
            return Ok(FetchList(false));
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Edit)]
        [HttpPost]
        public IActionResult Update(PatientModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatientRepository.update(unitOfWork.PatientRepository.Getbyid(model.Id),model);
            unitOfWork.Complate();
            return Ok(model.Iswaitingactivation? FetchList(false):FetchList(true));
        }

        [Route("Updatecheckperiod")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Edit)]
        [HttpPost]
        public IActionResult Updatecheckperiod(PatientModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatientRepository.update(unitOfWork.PatientRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList(true));
        }

        [Route("Updatetodogroupdefine")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Edit)]
        [HttpPost]
        public IActionResult Updatetodogroupdefine(PatientModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatientRepository.update(unitOfWork.PatientRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList(true));
        }

        [Route("Completeprepatient")]
        [AuthorizeMultiplePolicy(UserAuthory.Preregistration_Edit)]
        [HttpPost]
        public IActionResult Completeprepatient(PatientModel model)
        {
            var username = GetSessionUser();
            model.Iswaitingactivation = false;
            model.UpdatedUser = "System";
            model.UpdateTime = DateTime.Now;
            var patientstocks = unitOfWork.PatientstocksRepository.GetRecords<PatientstocksModel>(u => u.PatientID == model.ConcurrencyStamp);
            var stocks = new List<StockModel>();
            foreach (var patientstock in patientstocks)
            {
                var foundedstock = unitOfWork.StockRepository.GetRecord<StockModel>(u =>
                u.Skt == patientstock.Skt &&
                u.Barcodeno.Trim() == patientstock.Barcodeno.Trim() &&
                u.StockdefineID == patientstock.StockdefineID &&
                u.Departmentid == patientstock.Departmentid &&
                u.WarehouseID == model.WarehouseID
                );
                double amount = 0;
                var movements = unitOfWork.PatientstocksmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.StockID == patientstock.ConcurrencyStamp && u.IsActive);
                foreach (var movement in movements)
                {
                    amount += (movement.Amount * movement.Movementtype);
                }
                if (foundedstock == null)
                {
                    string newStockguid = Guid.NewGuid().ToString();
                    unitOfWork.StockRepository.Add(new StockModel
                    {
                        CreatedUser = GetSessionUser(),
                        CreateTime = DateTime.Now,
                        IsActive = true,
                        Barcodeno = patientstock.Barcodeno,
                        ConcurrencyStamp = newStockguid,
                        Departmentid = patientstock.Departmentid,
                        Info = patientstock.Info,
                        Skt = patientstock.Skt,
                        StockdefineID = patientstock.StockdefineID,
                        WarehouseID = model.WarehouseID,
                    });
                    unitOfWork.StockmovementRepository.Add(new StockmovementModel
                    {
                        StockID = newStockguid,
                        Amount = amount,
                        Movementdate = DateTime.Now,
                        Movementtype = ((int)Constants.Movementtypes.income),
                        Prevvalue = 0,
                        Newvalue = amount,
                        CreatedUser = GetSessionUser(),
                        CreateTime = DateTime.Now,
                        IsActive = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });
                }
                else
                {
                    double previousamount = 0;
                    var oldmovements = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.StockID == foundedstock.ConcurrencyStamp);
                    foreach (var movement in oldmovements)
                    {
                        previousamount += (movement.Amount * movement.Movementtype);
                    }
                    unitOfWork.StockmovementRepository.Add(new StockmovementModel
                    {
                        StockID = foundedstock.ConcurrencyStamp,
                        Amount = amount,
                        Movementdate = DateTime.Now,
                        Movementtype = ((int)Constants.Movementtypes.income),
                        Prevvalue = previousamount,
                        Newvalue = previousamount + amount,
                        CreatedUser = GetSessionUser(),
                        CreateTime = DateTime.Now,
                        IsActive = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });
                }

                patientstock.Status = (int)Constants.Stockstatus.IsCompleted;
                patientstock.UpdateTime = DateTime.Now;
                patientstock.UpdatedUser = "System";
                foreach (var movement in movements)
                {
                    movement.Status = (int)Constants.Stockstatus.IsCompleted;
                    movement.UpdatedUser = "System";
                    movement.UpdateTime = DateTime.Now;
                }
            }
            PatientmovementModel movementmodel = new PatientmovementModel();
            string movementguid = Guid.NewGuid().ToString();
            movementmodel.PatientID = model.ConcurrencyStamp;
            movementmodel.ConcurrencyStamp = movementguid;
            movementmodel.CreatedUser = "ServiceChecker";
            movementmodel.IsActive = true;
            movementmodel.CreateTime = DateTime.Now;
            movementmodel.IsTodoneed = false;
            movementmodel.Movementdate = DateTime.Now;
            movementmodel.Patientmovementtype = (int)Constants.Patienttypes.KurumaGiriş;
            unitOfWork.PatientmovementRepository.Add(movementmodel);
            unitOfWork.PatientRepository.update(unitOfWork.PatientRepository.GetRecord<PatientModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok(FetchList(false));
        }

        [Route("Preparestocks")]
        [AuthorizeMultiplePolicy(UserAuthory.Preregistration_Edit)]
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
                    if (!stock.Willdelete)
                    {
                        PatientstocksModel oldmodel = unitOfWork.PatientstocksRepository.GetRecord<PatientstocksModel>(u => u.ConcurrencyStamp == stock.ConcurrencyStamp);
                        stock.UpdatedUser = username;
                        stock.UpdateTime = DateTime.Now;
                        unitOfWork.PatientstocksRepository.update(oldmodel, stock);
                    }
                    else
                    {
                        PatientstocksModel oldmodel = unitOfWork.PatientstocksRepository.GetRecord<PatientstocksModel>(u => u.ConcurrencyStamp == stock.ConcurrencyStamp);
                        unitOfWork.PatientstocksRepository.Remove(oldmodel.Id);
                        var movements = unitOfWork.PatientmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.StockID == stock.ConcurrencyStamp);
                        foreach (var item in movements)
                        {
                            unitOfWork.PatientstocksmovementRepository.Remove(item.Id);
                        }
                    }
                }
                else
                {
                    string stockguid = Guid.NewGuid().ToString();
                    stock.CreatedUser = GetSessionUser();
                    stock.CreateTime = DateTime.Now;
                    stock.IsActive = true;
                    stock.ConcurrencyStamp = stockguid;
                    stock.PatientID = model.ConcurrencyStamp;
                    unitOfWork.PatientstocksRepository.Add(stock);
                    unitOfWork.PatientstocksmovementRepository.Add(new PatientstocksmovementModel
                    {
                        StockID = stockguid,
                        Amount = stock.Amount,
                        Movementdate = DateTime.Now,
                        Movementtype = ((int)Constants.Movementtypes.income),
                        Prevvalue = 0,
                        Newvalue = stock.Amount,
                        CreatedUser = GetSessionUser(),
                        CreateTime = DateTime.Now,
                        IsActive = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    }); ;
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
            var stocks = unitOfWork.PatientstocksRepository.GetRecords<PatientstocksModel>(u => u.PatientID == model.ConcurrencyStamp);
            foreach (var stock in stocks)
            {
                var oldmodel = unitOfWork.PatientstocksRepository.GetRecord<PatientstocksModel>(u => u.ConcurrencyStamp == stock.ConcurrencyStamp);
                unitOfWork.PatientstocksRepository.Remove(oldmodel.Id);
                var movements = unitOfWork.PatientmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.StockID == stock.ConcurrencyStamp);
                foreach (var item in movements)
                {
                    unitOfWork.PatientstocksmovementRepository.Remove(item.Id);
                }
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
