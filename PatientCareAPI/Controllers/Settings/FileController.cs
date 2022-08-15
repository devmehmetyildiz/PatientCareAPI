﻿using Microsoft.AspNetCore.Authorization;
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
    // [Authorize]
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

        //[Route("GetSelectedFile")]
        //[Authorize(Roles = (UserAuthory.File_Screen + "," + UserAuthory.File_Update))]
        //[HttpGet]
        //public IActionResult GetSelectedFile(int ID)
        //{
        //    FileModel Data = unitOfWork.FileRepository.Getbyid(ID);
        //    if (!Utilities.CheckAuth(UserAuthory.File_ManageAll, this.User.Identity))
        //    {
        //        if (Data.CreatedUser != this.User.Identity.Name)
        //        {
        //            return StatusCode(403);
        //        }
        //    }
        //    if (Data == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(Data);
        //}

        // [Route("Add")]
        //// [Authorize(Roles = UserAuthory.File_Add)]
        // [HttpPost]
        // public IActionResult Add(FileModel model)
        // {
        //     var claimsIdentity = this.User.Identity as ClaimsIdentity;
        //     var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
        //     model.CreatedUser = username;
        //     model.IsActive = true;
        //     model.CreateTime = DateTime.Now;
        //     model.ConcurrencyStamp = Guid.NewGuid().ToString();
        //     unitOfWork.FileRepository.Add(model);
        //     unitOfWork.Complate();
        //     return Ok();
        // }

        [Route("GetSelectedFile")]
        //   [Authorize(Roles = (UserAuthory.File_Screen + "," + UserAuthory.File_Update))]
        [HttpGet]
        public IActionResult GetSelectedFile()
        {
            //string uploadUrl = String.Format("ftp://{0}/{1}/{2}", "interpolapi.armsteknoloji.com", "interpol", "pp.jpeg");
            //using (WebClient request = new WebClient())
            //{
            //    request.Credentials = new NetworkCredential("u0584616", "5^k30nbC");
            //    byte[] fileData = request.DownloadData(uploadUrl);
            //    var stream = new MemoryStream(fileData);
            //    IFormFile file = new FormFile(stream, 0, fileData.Length, "pp", "pp.jpeg");
            //    return Ok(file);
            //}

            string ftphost = "ftp://interpolapi.armsteknoloji.com/interpol/";
            string ftpfilepath = "pp.jpeg";

            string ftpfullpath = ftphost + ftpfilepath;

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("u0584616", "5^k30nbC");
                byte[] fileData = request.DownloadData(ftpfullpath);
                var stream = new MemoryStream(fileData);

                IFormFile file = new FormFile(stream, 0, fileData.Length, "pp", "pp.jpeg")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };

                return Ok(file);
            }
        }


        [Route("Add")]
        [HttpPost]
        public IActionResult Add([FromForm] testmodel model)
        {
            try
            {
                string uploadUrl = String.Format("ftp://{0}/{1}/{2}", "interpolapi.armsteknoloji.com", "Patientcare", model.ImageFile.FileName);
                var request = (FtpWebRequest)WebRequest.Create(uploadUrl);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential("u0584616", "5^k30nbC");
                byte[] buffer = new byte[1024];
                var stream = model.ImageFile.OpenReadStream();
                byte[] fileContents;
                using (var ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    fileContents = ms.ToArray();
                }
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                return Ok("Upload Successfuly.");
            }
            catch (Exception ex)
            {
                return BadRequest("Upload Failed: " + ex.Message);
            }
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.File_Update)]
        [HttpPost]
        public IActionResult Update(FileModel model)
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
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.FileRepository.update(unitOfWork.FileRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
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
            unitOfWork.FileRepository.update(unitOfWork.FileRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
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
