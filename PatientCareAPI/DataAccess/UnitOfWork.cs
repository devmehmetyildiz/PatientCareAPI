﻿using PatientCareAPI.DataAccess.Repositories.Abstract;
using PatientCareAPI.DataAccess.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _dBContext;
        public UnitOfWork(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
            RolesRepository = new RolesRepository(_dBContext);
            UsersRepository = new UsersRepository(_dBContext);
            UsertoRoleRepository = new UsertoRoleRepository(_dBContext);
            UsertoSaltRepository = new UsertoSaltRepository(_dBContext);
        }

        public IRolesRepository RolesRepository { get; private set; }

        public IUsersRepository UsersRepository { get; private set; }

        public IUsertoRoleRepository UsertoRoleRepository { get; private set; }

        public IUsertoSaltRepository UsertoSaltRepository { get; private set; }

        public int Complate()
        {
            return _dBContext.SaveChanges();
        }

        public void Dispose()
        {
            _dBContext.Dispose();
        }
    }
}
