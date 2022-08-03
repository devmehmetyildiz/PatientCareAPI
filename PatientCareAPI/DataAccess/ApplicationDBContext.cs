﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        //Application
        public DbSet<ActivepatientModel> Activepatients { get; set; }
        public DbSet<ProcesstoActiveStocksModel> ProcesstoActivestocks { get; set; }
        public DbSet<ProcesstoFilesModel> ProcesstoFiles { get; set; }
        public DbSet<ProcesstoUsersModel> ProcesstoUsers { get; set; }
        public DbSet<ActivestockModel> Activestock { get; set; }
        public DbSet<DeactivestockModel> Deactivestocks { get; set; }
        public DbSet<PatientactivestockModel> Patientactivestocks { get; set; }
        public DbSet<PatientmovementModel> Patientmovements { get; set; }
        public DbSet<StockmovementModel> Stockmovements { get; set; }

        public DbSet<ProcessModel> Process{ get; set; }
        //Auth
        public DbSet<AuthoryModel> Authories { get; set; }
        public DbSet<UsersModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UsertoRoleModel> UsertoRoles { get; set; }
        public DbSet<RoletoAuthory> RoletoAuthories { get; set; }
        public DbSet<UsertoSaltModel> UsertoSalt { get; set; }

        //Settings
        public DbSet<CaseModel> Cases { get; set; }
        public DbSet<CasetoDepartmentModel> CasetoDepartments { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<PatientModel> Patients { get; set; }
        public DbSet<RemindingModel> Remindings { get; set; }
        public DbSet<StockModel> Stocks { get; set; }
        public DbSet<UnitModel> Units { get; set; }
        public DbSet<UnittoDepartmentModel> UnittoDepartments { get; set; }
        public DbSet<StationsModel> Stations { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<UsertoDepartmentModel> UsertoDepartment { get; set; }
        public DbSet<UsertoStationsModel> UsertoStations { get; set; }
        public DbSet<DepartmenttoStationModel>  DepartmenttoStation { get; set; }
        public DbSet<PatienttypeModel> Patienttypes { get; set; }
        public DbSet<CostumertypeModel> Costumertypes { get; set; }
        public DbSet<CostumertypetoDepartmentModel> CostumertypetoDepartments{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

    }

}
