using PatientCareAPI.DataAccess.Repositories.Abstract;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.DataAccess.Repositories.Concrete;
using PatientCareAPI.DataAccess.Repositories.Concrete.Settings;
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
            AuthoryRepository = new AuthoryRepository(_dBContext);
            UsersRepository = new UsersRepository(_dBContext);
            RoleRepository = new RoleRepository(_dBContext);
            UsertoRoleRepository = new UsertoRoleRepository(_dBContext);
            RoletoAuthoryRepository = new RoletoAuthoryRepository(_dBContext);
            UsertoSaltRepository = new UsertoSaltRepository(_dBContext);
            CaseRepository = new CaseRepository(_dBContext);
            UnitRepository = new UnitRepository(_dBContext);
        }

        public IAuthoryRepository AuthoryRepository { get; private set; }

        public IUsersRepository UsersRepository { get; private set; }

        public IUsertoRoleRepository UsertoRoleRepository { get; private set; }

        public IUsertoSaltRepository UsertoSaltRepository { get; private set; }
      
        public ICaseRepository CaseRepository { get; private set; }

        public IUnitRepository UnitRepository { get; private set; }

        public IRoletoAuthoryRepository RoletoAuthoryRepository { get; private set; }

        public IRoleRepository RoleRepository { get; private set; }

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
