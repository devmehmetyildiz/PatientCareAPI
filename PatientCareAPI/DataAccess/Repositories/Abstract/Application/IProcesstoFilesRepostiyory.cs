﻿using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IProcesstoFilesRepostiyory : IRepository<ProcesstoFilesModel>
    {
        void DeleteFilesByProcess(string ProcessGuid);
    }
}
