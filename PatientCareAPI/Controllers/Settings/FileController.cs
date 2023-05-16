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
using PatientCareAPI.Models.Application;

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
        private string GetSessionUser()
        {
            return (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
        }

        private List<FileModel> FetchList()
        {
            return unitOfWork.FileRepository.GetRecords<FileModel>(u => u.IsActive);
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Files_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Files_Getselected))]
        [HttpGet]
        public IActionResult GetSelectedFile(string guid)
        {
            return Ok(unitOfWork.FileRepository.GetFilebyGuid(guid));
        }

        [Route("GetFile")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetFile(string guid)
        {
            FileModel Data = unitOfWork.FileRepository.GetFilebyGuid(guid);
            if (Data != null)
                return File(Utilities.GetFile(Data), Data.Filetype, Data.Filename);
            else
                return NotFound();
        }

        [AllowAnonymous]
        [Route("GetImage")]
        [HttpGet]
        public IActionResult GetImage(string guid)
        {
            var files = unitOfWork.FileRepository.GetRecords<FileModel>(u => u.Parentid== guid);
            FileModel Data = files.FirstOrDefault(u => u.Usagetype == "PP");
            if (Data != null)
                return File(Utilities.GetFile(Data), Data.Filetype);
            else
                return NotFound();
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Files_Add)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost]
        public IActionResult Add([FromForm] List<FileModel> list)
        {
            var username = GetSessionUser();
            foreach (var model in list)
            {
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
                }
                else
                {
                    return BadRequest();
                }
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Files_Edit)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost]
        public IActionResult Update([FromForm]List<FileModel> list)
        {
            var username = GetSessionUser();
            foreach (var model in list)
            {
                if (string.IsNullOrWhiteSpace(model.ConcurrencyStamp))
                {
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
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    if (model.WillDelete)
                    {
                        model.DeleteUser = username;
                        model.IsActive = false;
                        model.DeleteTime = DateTime.Now;
                        if (Utilities.DeleteFile(model))
                        {
                            unitOfWork.FileRepository.update(unitOfWork.FileRepository.GetFilebyGuid(model.ConcurrencyStamp), model);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        model.UpdatedUser = username;
                        model.UpdateTime = DateTime.Now;
                        FileModel oldData = unitOfWork.FileRepository.GetFilebyGuid(model.ConcurrencyStamp);
                        if (model.File != null)
                        {
                            if (Utilities.DeleteFile(oldData))
                            {
                                model.Filename = model.File.FileName;
                                if (Utilities.UploadFile(model))
                                {
                                    unitOfWork.FileRepository.update(unitOfWork.FileRepository.Getbyid(model.Id), model);
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                        }
                        else
                        {
                            unitOfWork.FileRepository.update(unitOfWork.FileRepository.Getbyid(model.Id), model);
                        }
                    }
                }
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Files_Delete)]
        [HttpDelete]
        public IActionResult Delete(FileModel model)
        {
            var username = GetSessionUser();
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
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(FileModel model)
        {
            unitOfWork.FileRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }

       
    }
}
