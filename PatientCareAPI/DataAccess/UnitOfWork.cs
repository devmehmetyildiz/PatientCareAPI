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
            RolesRepository = new RolesRepository(_dBContext);
            UsersRepository = new UsersRepository(_dBContext);
            AuthoryRepository = new AuthoryRepository(_dBContext);
            UsertoAuthoryRepository = new UsertoAuthoryRepository(_dBContext);
            AuthorytoRolesRepository = new AuthorytoRolesRepository(_dBContext);
            UsertoSaltRepository = new UsertoSaltRepository(_dBContext);
            CaseRepository = new CaseRepository(_dBContext);
            UnitRepository = new UnitRepository(_dBContext);
        }

        public IRolesRepository RolesRepository { get; private set; }

        public IUsersRepository UsersRepository { get; private set; }

        public IUsertoAuthoryRepository UsertoAuthoryRepository { get; private set; }

        public IUsertoSaltRepository UsertoSaltRepository { get; private set; }
      
        public ICaseRepository CaseRepository { get; private set; }

        public IUnitRepository UnitRepository { get; private set; }

        public IAuthorytoRolesRepository AuthorytoRolesRepository { get; private set; }

        public IAuthoryRepository AuthoryRepository { get; private set; }

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
