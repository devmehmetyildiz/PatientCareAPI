﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;

namespace PatientCareAPI.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IRolesRepository RolesRepository { get; }

        IUsersRepository UsersRepository { get; }

        IUsertoRoleRepository UsertoRoleRepository { get; }

        IUsertoSaltRepository UsertoSaltRepository { get; }
        ICaseRepository CaseRepository { get; }
        IUnitRepository UnitRepository { get; }

        int Complate();
    }
}
