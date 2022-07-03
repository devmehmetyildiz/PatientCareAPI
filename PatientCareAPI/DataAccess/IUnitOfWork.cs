using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.DataAccess.Repositories.Abstract.Auth;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
namespace PatientCareAPI.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        //Application
        IActivepatientRepository ActivepatientRepository { get; }
        IProcesstoActivestocksRepostiyory ProcesstoActivestocksRepostiyory { get; }
        IProcesstoFilesRepostiyory ProcesstoFilesRepostiyory { get; }
        IProcesstoUsersRepository ProcesstoUsersRepository { get; }
        IActivestockRepository ActivestockRepository { get; }
        IProcessRepository ProcessRepository { get; }

        //Auth
        IAuthoryRepository AuthoryRepository { get; }
        IUsersRepository UsersRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUsertoRoleRepository UsertoRoleRepository { get; }
        IRoletoAuthoryRepository RoletoAuthoryRepository { get; }
        IUsertoSaltRepository UsertoSaltRepository { get; }

        //Settings
        ICaseRepository CaseRepository { get; }
        ICasetodepartmentRepository CasetodepartmentRepository { get; }
        IFileRepository FileRepository { get; }
        IPatientRepository PatientRepository { get; }
        IRemindingRepository RemindingRepository { get; }
        IStockRepository StockRepository { get; }
        IUnittodepartmentRepository UnittodepartmentRepository { get; }
        IUnitRepository UnitRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IStationsRepository StationsRepository { get; }
        IUsertoStationRepository UsertoStationRepository { get; }
        IUsertoDepartmentRepository UsertoDepartmentRepository { get; }
        IDepartmenttoStationRepository DepartmenttoStationRepository { get; }
        IPatienttypeRepository PatienttypeRepository { get; }
        int Complate();
    }
}
