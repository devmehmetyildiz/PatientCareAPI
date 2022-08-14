﻿using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IProcesstoUsersRepository : IRepository<ProcesstoUsersModel>
    {
        void DeleteUserByProcess(string ProcessGuid);
    }
}
