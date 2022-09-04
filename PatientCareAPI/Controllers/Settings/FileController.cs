using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PatientCareAPI.Utils;
using PatientCareAPI.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PatientCareAPI.Controllers.Settings
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<FileController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public FileController(IConfiguration configuration, ILogger<FileController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        [Authorize(Roles = UserAuthory.File_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<FileModel> Data = new List<FileModel>();
            if (Utilities.CheckAuth(UserAuthory.File_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.FileRepository.GetAll().Where(u => u.IsActive).ToList();
            }
            else
            {
                Data = unitOfWork.FileRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("GetSelectedFile")]
        [Authorize(Roles = (UserAuthory.File_Screen + "," + UserAuthory.File_Update))]
        [HttpGet]
        public IActionResult GetSelectedFile(string ID)
        {
            FileModel Data = unitOfWork.FileRepository.GetFilebyGuid(ID);
            if (!Utilities.CheckAuth(UserAuthory.File_ManageAll, this.User.Identity))
            {
                if (Data.CreatedUser != this.User.Identity.Name)
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

        [Route("GetSelectedFileByPatientGuid")]
        [Authorize(Roles = (UserAuthory.File_Screen + "," + UserAuthory.File_Update))]
        [HttpGet]
        public IActionResult GetSelectedFileByPatientGuid(string Guid)
        {
            FileModel Data = unitOfWork.FileRepository.GetFilebyGuid(unitOfWork.ActivepatientRepository.FindByGuid(Guid).ImageID);
            if (!Utilities.CheckAuth(UserAuthory.File_ManageAll, this.User.Identity))
            {
                if (Data.CreatedUser != this.User.Identity.Name)
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

        [Route("GetFile")]
        [Authorize(Roles = (UserAuthory.File_Screen + "," + UserAuthory.File_Update))]
        [HttpGet]
        public IActionResult GetFile(string ID)
        {
            FileModel Data = unitOfWork.FileRepository.GetFilebyGuid(ID);
            if (Data != null)
                return File(Utilities.GetFile(Data), Data.Filetype);
            else
                return NotFound();
        }

        [Route("Add")]
        [Authorize(Roles = UserAuthory.File_Add)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost]
        public IActionResult Add([FromForm] FileModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(model.Filefolder))
            {
                model.Filefolder = Guid.NewGuid().ToString();
            }
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            model.Filename = model.File.FileName;
            model.Filetype = model.File.ContentType;
            if (Utilities.UploadFile(model))
            {
                unitOfWork.FileRepository.Add(model);
                unitOfWork.Complate();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.File_Update)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost]
        public IActionResult Update([FromForm]FileModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.File_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            FileModel oldData = unitOfWork.FileRepository.GetFilebyGuid(model.ConcurrencyStamp);
            if (Utilities.DeleteFile(oldData))
            {
                model.Filename = model.File.FileName;
                if (Utilities.UploadFile(model))
                {
                    unitOfWork.FileRepository.update(unitOfWork.FileRepository.Getbyid(model.Id), model);
                    unitOfWork.Complate();
                }
            }
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.File_Delete)]
        [HttpDelete]
        public IActionResult Delete(FileModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.File_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            if (Utilities.DeleteFile(model))
            {
                unitOfWork.FileRepository.update(unitOfWork.FileRepository.GetFilebyGuid(model.ConcurrencyStamp), model);
                unitOfWork.Complate();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles = UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(FileModel model)
        {
            unitOfWork.FileRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }

       
    }
}
