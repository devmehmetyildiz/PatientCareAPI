﻿using PatientCareAPI.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IProcesstoFilesRepostiyory : IRepository<ProcesstoFilesModel>
    {
        void DeleteFilesByProcess(string ProcessGuid);
    }
}
