using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.DataAccess.Repositories.Abstract.Auth;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Warehouse;
using PatientCareAPI.DataAccess.Repositories.Abstract.System;

namespace PatientCareAPI.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDBContext Context { get; }
        //Application
        IPatientRepository PatientRepository { get; }
        IPatientmovementRepository PatientmovementRepository { get; }
        IPatientdefineRepository PatientdefineRepository { get; }
        ITodoRepository TodoRepository { get; }

        //Warehouse
        IDeactivestockRepository DeactivestockRepository { get; }
        IPatientstocksRepository PatientstocksRepository { get; }
        IPatientstocksmovementRepository PatientstocksmovementRepository { get; }
        IPurchaseorderRepository PurchaseorderRepository { get; }
        IPurchaseorderstocksmovementRepository PurchaseorderstocksmovementRepository { get; }
        IPurchaseorderstocksRepository PurchaseorderstocksRepository { get; }
        IStockdefineRepository StockdefineRepository { get; }
        IStockmovementRepository StockmovementRepository { get; }
        IStockRepository StockRepository { get; }
        IWarehouseRepository WarehouseRepository { get; }

        //Auth
        IAuthoryRepository AuthoryRepository { get; }
        IUsersRepository UsersRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUsertoRoleRepository UsertoRoleRepository { get; }
        IRoletoAuthoryRepository RoletoAuthoryRepository { get; }
        IUsertoSaltRepository UsertoSaltRepository { get; }
        IResetpasswordrequestRepository ResetpasswordrequestRepository { get; }

        //Settings
        ICaseRepository CaseRepository { get; }
        ICasetodepartmentRepository CasetodepartmentRepository { get; }
        IFileRepository FileRepository { get; }
        IRemindingRepository RemindingRepository { get; }
        IUnittodepartmentRepository UnittodepartmentRepository { get; }
        IUnitRepository UnitRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IStationsRepository StationsRepository { get; }
        IUsertoStationRepository UsertoStationRepository { get; }
        IUsertoDepartmentRepository UsertoDepartmentRepository { get; }
        IDepartmenttoStationRepository DepartmenttoStationRepository { get; }
        IPatienttypeRepository PatienttypeRepository { get; }
        ICostumertypeRepository CostumertypeRepository { get; }
        ICostumertypetoDepartmentRepository CostumertypetoDepartmentRepository { get; }
        ITablemetaconfigRepository TablemetaconfigRepository { get; }
        ITododefineRepository TododefineRepository { get; }
        ITodogroupdefineRepository TodogroupdefineRepository { get; }
        ITodogrouptoTodoRepository TodogrouptoTodoRepository { get; }
        ICheckperiodRepository CheckperiodRepository { get; }
        ICheckperiodtoPeriodRepository CheckperiodtoPeriodRepository { get; }
        IPeriodRepository PeriodRepository { get; }
        ITododefinetoPeriodRepository TododefinetoPeriodRepository { get; }

        //System
        IMailsettingRepository MailsettingRepository { get; }
        IPrinttemplateRepository PrinttemplateRepository { get; }
        int Complate();
    }
}
