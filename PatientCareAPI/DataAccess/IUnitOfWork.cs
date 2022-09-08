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
        IPatientapplicantRepository PatientapplicantRepository { get; }
        IPatientbodycontrolformRepository PatientbodycontrolformRepository { get; }
        IPatientdiagnosisRepository PatientdiagnosisRepository { get; }
        IPatientdisabilitypermitformRepository PatientdisabilitypermitformRepository { get; }
        IPatientdisabledhealthboardreportRepository PatientdisabledhealthboardreportRepository { get; }
        IPatientfirstadmissionformRepository PatientfirstadmissionformRepository { get; }
        IPatientfirstapproachreportRepository PatientfirstapproachreportRepository { get; }
        IPatientownershiprecieveRepository PatientownershiprecieveRepository { get; }
        IPatientrecieveformRepository PatientrecieveformRepository { get; }
        IPatientsubmittingformRepository PatientsubmittingformRepository { get; }
        IActivepatienttoActivestocksRepostiyory ActivepatienttoActivestocksRepostiyory { get; }
        IActivepatienttoFilesRepostiyory ActivepatienttoFilesRepostiyory { get; }
        IActivestockRepository ActivestockRepository { get; }
        IDeactivestockRepository DeactivestockRepository { get; }
        IPatientactivestockRepository PatientactivestockRepository { get; }
        IPatientmovementRepository PatientmovementRepository { get; }
        IStockmovementRepository StockmovementRepository { get; }
        IPatientRepository PatientRepository { get; }

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
        ICostumertypeRepository CostumertypeRepository { get; }
        ICostumertypetoDepartmentRepository CostumertypetoDepartmentRepository { get; }
        IDatatableRepository DatatableRepository { get; }
        int Complate();
    }
}
