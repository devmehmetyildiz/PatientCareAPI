﻿using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientRepository : IRepository<PatientModel>
    {
        PatientModel FindByGuid(string guid);
    }
}
