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
using PatientCareAPI.DataAccess.Repositories.Abstract.Warehouse;
using PatientCareAPI.DataAccess.Repositories.Concrete.Warehouse;

namespace PatientCareAPI.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _dBContext;
        public UnitOfWork(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
            //Application
            PatientRepository = new PatientRepository(_dBContext);
            PatientdefineRepository = new PatientdefineRepository(_dBContext);
            PatientmovementRepository = new PatientmovementRepository(_dBContext);
            //Warehouse
            DeactivestockRepository = new DeactivestockRepository(_dBContext);
            PatientstocksRepository = new PatientstocksRepository(_dBContext);
            PatientstocksmovementRepository = new PatientstocksmovementRepository(_dBContext);
            PurchaseorderRepository = new PurchaseorderRepository(_dBContext);
            PurchaseorderstocksmovementRepository = new PurchaseorderstocksmovementRepository(_dBContext);
            PurchaseorderstocksRepository = new PurchaseorderstocksRepository(_dBContext);
            StockdefineRepository = new StockdefineRepository(_dBContext);
            StockmovementRepository = new StockmovementRepository(_dBContext);
            StockRepository = new StockRepository(_dBContext);
            WarehouseRepository = new WarehouseRepository(_dBContext);
            //Auth
            AuthoryRepository = new AuthoryRepository(_dBContext);
            UsersRepository = new UsersRepository(_dBContext);
            RoleRepository = new RoleRepository(_dBContext);
            UsertoRoleRepository = new UsertoRoleRepository(_dBContext);
            RoletoAuthoryRepository = new RoletoAuthoryRepository(_dBContext);
            UsertoSaltRepository = new UsertoSaltRepository(_dBContext);
            //Settings
            CaseRepository = new CaseRepository(_dBContext);
            CasetodepartmentRepository = new CasetodepartmentRepository(_dBContext);
            UnitRepository = new UnitRepository(_dBContext);
            DepartmentRepository = new DepartmentRepository(_dBContext);
            StationsRepository = new StationsRepository(_dBContext);
            UsertoStationRepository = new UsertoStationRepository(_dBContext);
            UsertoDepartmentRepository = new UsertoDepartmentRepository(_dBContext);
            DepartmenttoStationRepository = new DepartmenttoStationRepository(_dBContext);
            FileRepository = new FileRepository(_dBContext);
            RemindingRepository = new RemindingRepository(_dBContext);
            UnittodepartmentRepository = new UnittodepartmentRepository(_dBContext);
            PatienttypeRepository = new PatienttypeRepository(_dBContext);
            CostumertypeRepository = new CostumertypeRepository(_dBContext);
            CostumertypetoDepartmentRepository = new CostumertypetoDepartmentRepository(_dBContext);
            TablemetaconfigRepository = new TablemetaconfigRepository(_dBContext);
            TododefineRepository = new TododefineRepository(_dBContext);
            TodogroupdefineRepository = new TodogroupdefineRepository(_dBContext);
            TodogrouptoTodoRepository = new TodogrouptoTodoRepository(_dBContext);
            CheckperiodRepository = new CheckperiodRepository(_dBContext);
            CheckperiodtoPeriodRepository = new CheckperiodtoPeriodRepository(_dBContext);
            PeriodRepository = new PeriodRepository(_dBContext);
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

        public IPatientRepository PatientRepository { get; private set; }

        public ICasetodepartmentRepository CasetodepartmentRepository { get; private set; }

        public IFileRepository FileRepository { get; private set; }

        public IPatientdefineRepository PatientdefineRepository { get; private set; }

        public IRemindingRepository RemindingRepository { get; private set; }

        public IUnittodepartmentRepository UnittodepartmentRepository { get; private set; }

        public IPatienttypeRepository PatienttypeRepository { get; private set; }

        public ICostumertypeRepository CostumertypeRepository { get; private set; }

        public ICostumertypetoDepartmentRepository CostumertypetoDepartmentRepository { get; private set; }

        public IPatientmovementRepository PatientmovementRepository { get; private set; }

        public IDeactivestockRepository DeactivestockRepository { get; private set; }

        public IPatientstocksRepository PatientstocksRepository { get; private set; }

        public IPatientstocksmovementRepository PatientstocksmovementRepository { get; private set; }

        public IPurchaseorderRepository PurchaseorderRepository { get; private set; }

        public IPurchaseorderstocksmovementRepository PurchaseorderstocksmovementRepository { get; private set; }

        public IPurchaseorderstocksRepository PurchaseorderstocksRepository { get; private set; }

        public IStockdefineRepository StockdefineRepository { get; private set; }

        public IStockmovementRepository StockmovementRepository { get; private set; }

        public IStockRepository StockRepository { get; private set; }

        public IWarehouseRepository WarehouseRepository { get; private set; }

        public ITablemetaconfigRepository TablemetaconfigRepository { get; private set; }

        public ITododefineRepository TododefineRepository { get; private set; }

        public ITodogroupdefineRepository TodogroupdefineRepository { get; private set; }

        public ITodogrouptoTodoRepository TodogrouptoTodoRepository { get; private set; }

        public ICheckperiodRepository CheckperiodRepository { get; private set; }

        public ICheckperiodtoPeriodRepository CheckperiodtoPeriodRepository { get; private set; }

        public IPeriodRepository PeriodRepository { get; private set; }

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
