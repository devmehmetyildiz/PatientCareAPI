using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.Utils
{
    public class Utilities
    {
        UnitOfWork unitOfWork;
        private const string FTP_URL = "ftp://interpolapi.armsteknoloji.com";
        private const string FTP_USERNAME = "u0584616";
        private const string FTP_PASSWORD = "5^k30nbC";
        private const string FTP_FOLDERNAME = "Patientcare";
        private readonly ApplicationDBContext _context;

        public Utilities(ApplicationDBContext context)
        {
            _context = context;
            unitOfWork = new UnitOfWork(_context);
        }
        private List<string> GetUserRoles(object userid)
        {
            var claimsIdentity = userid as ClaimsIdentity;
            var user = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            UsersModel Activeuser = unitOfWork.UsersRepository.FindUserByName(user.ToString());
            List<string> Userroles = unitOfWork.UsertoRoleRepository.GetRolesbyUser(Activeuser.ConcurrencyStamp);
            List<string> Userauthories = new List<string>();
            foreach (var roles in Userroles)
            {
                foreach (var authories in unitOfWork.RoletoAuthoryRepository.GetAuthoriesByRole(roles))
                {
                    Userauthories.Add(unitOfWork.AuthoryRepository.FindAuthoryBuGuid(authories).Name);
                }
            }
            return Userauthories;
        }

        public bool CheckAuth(string Role,object userid)
        {
            return unitOfWork.AuthoryRepository.CheckAuthByUsername((userid as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value, Role);
        }

        public bool UploadFile(FileModel model)
        {
            try
            {
                if (!FtpDirectoryExists(model.Filefolder))
                {
                    Makefolder(model.Filefolder);
                }
                string URL = $"{FTP_URL}/{FTP_FOLDERNAME}/{model.Filefolder}/{model.File.FileName}";
                var request = (FtpWebRequest)WebRequest.Create(URL);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                byte[] buffer = new byte[1024];
                var stream = model.File.OpenReadStream();
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IFormFile GetFile(FileModel model)
        {
            string URL = $"{FTP_URL}/{FTP_FOLDERNAME}/{model.Filefolder}/{model.Filename}";

            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("username", "password");
            client.DownloadFile(
                "ftp://ftp.example.com/remote/path/file.zip", @"C:\local\path\file.zip");

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                byte[] fileData = request.DownloadData(URL);
                var stream = new MemoryStream(fileData);

                IFormFile file = new FormFile(stream, 0, fileData.Length, model.Filename, model.Filename)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = model.Filetype
                };
                return file;
            }
        }

        public bool DeleteFile(FileModel model)
        {
            try
            {
                string URL = $"{FTP_URL}/{FTP_FOLDERNAME}/{model.Filefolder}/{model.Filefolder}";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(URL);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool FtpDirectoryExists(string directory)
        {
            try
            {
                List<string> directroys = new List<string>();
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"{FTP_URL}/{FTP_FOLDERNAME}/{directory}");
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();
                reader.Close();
                response.Close();
                directroys = names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool Makefolder(string folder)
        {
            bool iscreated = false;
            try
            {
                WebRequest request = WebRequest.Create($"{FTP_URL}/{FTP_FOLDERNAME}/{folder}");
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(FTP_USERNAME, FTP_PASSWORD);
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    iscreated = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return iscreated;

        }
    }
}
