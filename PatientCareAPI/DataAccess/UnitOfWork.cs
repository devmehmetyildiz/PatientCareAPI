using PatientCareAPI.DataAccess.Repositories.Abstract.Auth;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.DataAccess.Repositories.Concrete.Auth;
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
            DepartmentRepository = new DepartmentRepository(_dBContext);
            StationsRepository = new StationsRepository(_dBContext);
            UsertoStationRepository = new UsertoStationRepository(_dBContext);
            UsertoDepartmentRepository = new UsertoDepartmentRepository(_dBContext);
            DepartmenttoStationRepository = new DepartmenttoStationRepository(_dBContext);
        }

        public IAuthoryRepository AuthoryRepository { get; private set; }

        public IUsersRepository UsersRepository { get; private set; }

        public IUsertoRoleRepository UsertoRoleRepository { get; private set; }

        public IUsertoSaltRepository UsertoSaltRepository { get; private set; }
      
        public ICaseRepository CaseRepository { get; private set; }

        public IUnitRepository UnitRepository { get; private set; }

        public IRoletoAuthoryRepository RoletoAuthoryRepository { get; private set; }

        public IRoleRepository RoleRepository { get; private set; }

        public IDepartmentRepository DepartmentRepository { get; private set; }

        public IStationsRepository StationsRepository { get; private set; }

        public IUsertoStationRepository UsertoStationRepository { get; private set; }

        public IUsertoDepartmentRepository UsertoDepartmentRepository { get; private set; }

        public IDepartmenttoStationRepository DepartmenttoStationRepository { get; private set; }

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
