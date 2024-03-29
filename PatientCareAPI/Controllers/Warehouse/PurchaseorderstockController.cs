﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Models.Warehouse;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Warehouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseorderstockController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PurchaseorderstockController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public PurchaseorderstockController(IConfiguration configuration, ILogger<PurchaseorderstockController> logger, ApplicationDBContext context)
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

        private List<PurchaseorderstocksModel> FetchList()
        {
            var List = unitOfWork.PurchaseorderstocksRepository.GetRecords<PurchaseorderstocksModel>(u => u.IsActive);
            foreach (var item in List)
            {
                double amount = 0;
                var movements = unitOfWork.PurchaseorderstocksmovementRepository.GetRecords<PurchaseorderstocksmovementModel>(u => u.StockID == item.ConcurrencyStamp && u.IsActive);
                foreach (var movement in movements)
                {
                    amount += (movement.Amount * movement.Movementtype);
                }
                item.Amount = amount;
                item.Purchaseorder = unitOfWork.PurchaseorderRepository.GetRecord<PurchaseorderModel>(u => u.ConcurrencyStamp == item.PurchaseorderID);
                item.Stockdefine = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.StockdefineID);
                item.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
                item.Stockdefine.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stockdefine.Unitid);
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Purchaseorderstock_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Purchaseorderstock_Getselected))]
        [HttpGet]
        public IActionResult GetSelectedActivestock(string guid)
        {
            PurchaseorderstocksModel Data = unitOfWork.PurchaseorderstocksRepository.GetRecord<PurchaseorderstocksModel>(u => u.ConcurrencyStamp == guid);
            double amount = 0;
            var movements = unitOfWork.PurchaseorderstocksmovementRepository.GetRecords<PurchaseorderstocksmovementModel>(u => u.StockID == Data.ConcurrencyStamp && u.IsActive);
            foreach (var movement in movements)
            {
                amount += (movement.Amount * movement.Movementtype);
            }
            Data.Amount = amount;
            Data.Purchaseorder = unitOfWork.PurchaseorderRepository.GetRecord<PurchaseorderModel>(u => u.ConcurrencyStamp == Data.PurchaseorderID);
            Data.Stockdefine = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.StockdefineID);
            Data.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == Data.Departmentid);
            Data.Stockdefine.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == Data.Stockdefine.Unitid);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Purchaseorderstock_Add)]
        [HttpPost]
        public IActionResult Add(PurchaseorderstocksModel model)
        {
            string stockguid = Guid.NewGuid().ToString();
            model.CreatedUser = GetSessionUser();
            model.CreateTime = DateTime.Now;
            model.IsActive = true;
            model.ConcurrencyStamp = stockguid;
            unitOfWork.PurchaseorderstocksRepository.Add(model);
            unitOfWork.PurchaseorderstocksmovementRepository.Add(new PurchaseorderstocksmovementModel
            {
                StockID = stockguid,
                Amount = model.Amount,
                Movementdate = DateTime.Now,
                Movementtype = ((int)Constants.Movementtypes.income),
                Prevvalue = 0,
                Newvalue = model.Amount,
                CreatedUser = GetSessionUser(),
                CreateTime = DateTime.Now,
                IsActive = true,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Purchaseorderstock_Edit)]
        [HttpPost]
        public IActionResult Update(PurchaseorderstocksModel model)
        {
            var username = GetSessionUser();
            PurchaseorderstocksModel oldmodel = unitOfWork.PurchaseorderstocksRepository.GetRecord<PurchaseorderstocksModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp);
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PurchaseorderstocksRepository.update(oldmodel, model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PurchaseorderstocksModel model)
        {
            unitOfWork.PurchaseorderstocksRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
