using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.DataAccess.Repositories.Concrete.Application;
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
            //Application
            ActivepatientRepository = new ActivepatientRepository(_dBContext);
            PatientapplicantRepository = new PatientapplicantRepository(_dBContext);
            PatientbodycontrolformRepository = new PatientbodycontrolformRepository(_dBContext);
            PatientdiagnosisRepository = new PatientdiagnosisRepository(_dBContext);
            PatientdisabilitypermitformRepository = new PatientdisabilitypermitformRepository(_dBContext);
            PatientdisabledhealthboardreportRepository = new PatientdisabledhealthboardreportRepository(_dBContext);
            PatientfirstadmissionformRepository = new PatientfirstadmissionformRepository(_dBContext);
            PatientfirstapproachreportRepository = new PatientfirstapproachreportRepository(_dBContext);
            PatientownershiprecieveRepository = new PatientownershiprecieveRepository(_dBContext);
            PatientrecieveformRepository = new PatientrecieveformRepository(_dBContext);
            PatientsubmittingformRepository = new PatientsubmittingformRepository(_dBContext);

            ProcesstoActivestocksRepostiyory = new ProcesstoActivestockRepository(_dBContext);
            ProcesstoFilesRepostiyory = new ProcesstoFilesRepository(_dBContext);
            ProcesstoUsersRepository = new ProcesstoUsersRepository(_dBContext);
            ActivestockRepository = new ActivestockRepository(_dBContext);
            ProcessRepository = new ProcessRepository(_dBContext);
            DeactivestockRepository = new DeactivestockRepository(_dBContext);
            PatientactivestockRepository = new PatientactivestockRepository(_dBContext);
            PatientmovementRepository = new PatientmovementRepository(_dBContext);
            StockmovementRepository = new StockmovementRepository(_dBContext);
            PatientRepository = new PatientRepository(_dBContext);
            //Auth
            AuthoryRepository = new AuthoryRepository(_dBContext);
            UsersRepository = new UsersRepository(_dBContext);
            RoleRepository = new RoleRepository(_dBContext);
            UsertoRoleRepository = new UsertoRoleRepository(_dBContext);
            RoletoAuthoryRepository = new RoletoAuthoryRepository(_dBContext);
            UsertoSaltRepository = new UsertoSaltRepository(_dBContext);
            //Settings
            CaseRepository = new CaseRepository(_dBContext);
            CasetodepartmentRepository = new CasetodepartmentRepository(dBContext);
            UnitRepository = new UnitRepository(_dBContext);
            DepartmentRepository = new DepartmentRepository(_dBContext);
            StationsRepository = new StationsRepository(_dBContext);
            UsertoStationRepository = new UsertoStationRepository(_dBContext);
            UsertoDepartmentRepository = new UsertoDepartmentRepository(_dBContext);
            DepartmenttoStationRepository = new DepartmenttoStationRepository(_dBContext);
            FileRepository = new FileRepository(_dBContext);
            RemindingRepository = new RemindingRepository(_dBContext);
            StockRepository = new StockRepository(_dBContext);
            UnittodepartmentRepository = new UnittodepartmentRepository(_dBContext);
            PatienttypeRepository = new PatienttypeRepository(_dBContext);
            CostumertypeRepository = new CostumertypeRepository(_dBContext);
            CostumertypetoDepartmentRepository = new CostumertypetoDepartmentRepository(_dBContext);
            DatatableRepository = new DatatableRepository(_dBContext);
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

        public IActivepatientRepository ActivepatientRepository { get; private set; }

        public IProcesstoActivestocksRepostiyory ProcesstoActivestocksRepostiyory { get; private set; }

        public IProcesstoFilesRepostiyory ProcesstoFilesRepostiyory { get; private set; }

        public IProcesstoUsersRepository ProcesstoUsersRepository { get; private set; }

        public IActivestockRepository ActivestockRepository { get; private set; }

        public IProcessRepository ProcessRepository { get; private set; }

        public ICasetodepartmentRepository CasetodepartmentRepository { get; private set; }

        public IFileRepository FileRepository { get; private set; }

        public IPatientRepository PatientRepository { get; private set; }

        public IRemindingRepository RemindingRepository { get; private set; }

        public IStockRepository StockRepository { get; private set; }

        public IUnittodepartmentRepository UnittodepartmentRepository { get; private set; }

        public IPatienttypeRepository PatienttypeRepository { get; private set; }

        public ICostumertypeRepository CostumertypeRepository { get; private set; }

        public ICostumertypetoDepartmentRepository CostumertypetoDepartmentRepository { get; private set; }

        public IDeactivestockRepository DeactivestockRepository { get; private set; }

        public IPatientactivestockRepository PatientactivestockRepository { get; private set; }

        public IPatientmovementRepository PatientmovementRepository { get; private set; }

        public IStockmovementRepository StockmovementRepository { get; private set; }

        public IDatatableRepository DatatableRepository { get; private set; }

        public IPatientapplicantRepository PatientapplicantRepository { get; private set; }

        public IPatientbodycontrolformRepository PatientbodycontrolformRepository { get; private set; }

        public IPatientdiagnosisRepository PatientdiagnosisRepository { get; private set; }

        public IPatientdisabilitypermitformRepository PatientdisabilitypermitformRepository { get; private set; }

        public IPatientdisabledhealthboardreportRepository PatientdisabledhealthboardreportRepository { get; private set; }

        public IPatientfirstadmissionformRepository PatientfirstadmissionformRepository { get; private set; }

        public IPatientfirstapproachreportRepository PatientfirstapproachreportRepository { get; private set; }

        public IPatientownershiprecieveRepository PatientownershiprecieveRepository { get; private set; }

        public IPatientrecieveformRepository PatientrecieveformRepository { get; private set; }

        public IPatientsubmittingformRepository PatientsubmittingformRepository { get; private set; }

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
