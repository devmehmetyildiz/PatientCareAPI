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

namespace PatientCareAPI.Controllers.Application
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<ProcessController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public ProcessController(IConfiguration configuration, ILogger<ProcessController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [Route("GetAll")]
        [Authorize(Roles = UserAuthory.Process_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ProcessModel> Data = new List<ProcessModel>();
            if (Utilities.CheckAuth(UserAuthory.Process_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.ProcessRepository.GetAll().Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    List<string> Activestocks = unitOfWork.ProcesstoActivestocksRepostiyory.GetAll().Where(u => u.ProcessID == item.ConcurrencyStamp).Select(u => u.ActivestocksID).ToList();
                    foreach (var activestock in Activestocks)
                    {
                        item.Activestocks.Add(unitOfWork.ActivestockRepository.GetStockByGuid(activestock));
                    }
                    List<string> Users = unitOfWork.ProcesstoUsersRepository.GetAll().Where(u => u.ProcessID == item.ConcurrencyStamp).Select(u => u.UserID).ToList();
                    foreach (var user in Users)
                    {
                        item.Users.Add(unitOfWork.UsersRepository.GetUsertByGuid(user));
                    }
                    List<string> Files = unitOfWork.ProcesstoFilesRepostiyory.GetAll().Where(u => u.ProcessID == item.ConcurrencyStamp).Select(u => u.FilesID).ToList();
                    foreach (var file in Files)
                    {
                        item.Files.Add(unitOfWork.FileRepository.GetFilebyGuid(file));
                    }
                }
            }
            else
            {
                Data = unitOfWork.ProcessRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    List<string> Activestocks = unitOfWork.ProcesstoActivestocksRepostiyory.GetAll().Where(u => u.ProcessID == item.ConcurrencyStamp).Select(u => u.ActivestocksID).ToList();
                    foreach (var activestock in Activestocks)
                    {
                        item.Activestocks.Add(unitOfWork.ActivestockRepository.GetStockByGuid(activestock));
                    }
                    List<string> Users = unitOfWork.ProcesstoUsersRepository.GetAll().Where(u => u.ProcessID == item.ConcurrencyStamp).Select(u => u.UserID).ToList();
                    foreach (var user in Users)
                    {
                        item.Users.Add(unitOfWork.UsersRepository.GetUsertByGuid(user));
                    }
                    List<string> Files = unitOfWork.ProcesstoFilesRepostiyory.GetAll().Where(u => u.ProcessID == item.ConcurrencyStamp).Select(u => u.FilesID).ToList();
                    foreach (var file in Files)
                    {
                        item.Files.Add(unitOfWork.FileRepository.GetFilebyGuid(file));
                    }
                }
            }
            if (Data.Count == 0)
                return NotFound();
            return Ok(Data);
        }

        [Authorize(Roles = (UserAuthory.Department_Screen + "," + UserAuthory.Department_Update))]
        [Route("GetSelectedDepartment")]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            ProcessModel Data = unitOfWork.ProcessRepository.Getbyid(ID);
            List<string> Activestocks = unitOfWork.ProcesstoActivestocksRepostiyory.GetAll().Where(u => u.ProcessID == Data.ConcurrencyStamp).Select(u => u.ActivestocksID).ToList();
            foreach (var activestock in Activestocks)
            {
                Data.Activestocks.Add(unitOfWork.ActivestockRepository.GetStockByGuid(activestock));
            }
            List<string> Users = unitOfWork.ProcesstoUsersRepository.GetAll().Where(u => u.ProcessID == Data.ConcurrencyStamp).Select(u => u.UserID).ToList();
            foreach (var user in Users)
            {
                Data.Users.Add(unitOfWork.UsersRepository.GetUsertByGuid(user));
            }
            List<string> Files = unitOfWork.ProcesstoFilesRepostiyory.GetAll().Where(u => u.ProcessID == Data.ConcurrencyStamp).Select(u => u.FilesID).ToList();
            foreach (var file in Files)
            {
                Data.Files.Add(unitOfWork.FileRepository.GetFilebyGuid(file));
            }
            if (Utilities.CheckAuth(UserAuthory.Process_ManageAll, this.User.Identity))
            {
                if (Data.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }



        [Route("Add")]
        [Authorize(Roles = UserAuthory.Process_Add)]
        [HttpPost]
        public IActionResult Add(ProcessModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.ProcessRepository.Add(model);
            foreach (var item in model.Files)
            {
                unitOfWork.ProcesstoFilesRepostiyory.Add(new ProcesstoFilesModel { Id = 0, ProcessID = model.ConcurrencyStamp, FilesID = item.ConcurrencyStamp });
            }
            foreach (var item in model.Users)
            {
                unitOfWork.ProcesstoUsersRepository.Add(new ProcesstoUsersModel { Id = 0, ProcessID = model.ConcurrencyStamp, UserID = item.ConcurrencyStamp });
            }
            foreach (var item in model.Activestocks)
            {
                unitOfWork.ProcesstoActivestocksRepostiyory.Add(new ProcesstoActiveStocksModel { Id = 0, ProcessID = model.ConcurrencyStamp, ActivestocksID = item.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Process_Update)]
        [HttpPost]
        public IActionResult Update(ProcessModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            if (!Utilities.CheckAuth(UserAuthory.Process_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            unitOfWork.ProcessRepository.update(unitOfWork.ProcessRepository.Getbyid(model.Id), model);
            unitOfWork.ProcesstoActivestocksRepostiyory.DeleteActiveStocksByProcess(model.ConcurrencyStamp);
            unitOfWork.ProcesstoFilesRepostiyory.DeleteFilesByProcess(model.ConcurrencyStamp);
            unitOfWork.ProcesstoUsersRepository.DeleteUserByProcess(model.ConcurrencyStamp);
            foreach (var item in model.Files)
            {
                unitOfWork.ProcesstoFilesRepostiyory.Add(new ProcesstoFilesModel { Id = 0, ProcessID = model.ConcurrencyStamp, FilesID = item.ConcurrencyStamp });
            }
            foreach (var item in model.Users)
            {
                unitOfWork.ProcesstoUsersRepository.Add(new ProcesstoUsersModel { Id = 0, ProcessID = model.ConcurrencyStamp, UserID = item.ConcurrencyStamp });
            }
            foreach (var item in model.Activestocks)
            {
                unitOfWork.ProcesstoActivestocksRepostiyory.Add(new ProcesstoActiveStocksModel { Id = 0, ProcessID = model.ConcurrencyStamp, ActivestocksID = item.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Process_Delete)]
        [HttpDelete]
        public IActionResult Delete(ProcessModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            if (!Utilities.CheckAuth(UserAuthory.Process_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            unitOfWork.ProcessRepository.update(unitOfWork.ProcessRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles = UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(ProcessModel model)
        {
            unitOfWork.ProcessRepository.Remove(model.Id);
            unitOfWork.ProcesstoActivestocksRepostiyory.DeleteActiveStocksByProcess(model.ConcurrencyStamp);
            unitOfWork.ProcesstoFilesRepostiyory.DeleteFilesByProcess(model.ConcurrencyStamp);
            unitOfWork.ProcesstoUsersRepository.DeleteUserByProcess(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
