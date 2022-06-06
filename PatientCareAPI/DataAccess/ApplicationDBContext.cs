using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<AuthoryModel> Authories { get; set; }
        public DbSet<UsersModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UsertoRoleModel> UsertoRoles { get; set; }
        public DbSet<RoletoAuthory> RoletoAuthories { get; set; }
        public DbSet<UsertoSaltModel> UsertoSalt { get; set; }
        public DbSet<CaseModel> Cases { get; set; }
        public DbSet<UnitModel> Units { get; set; }
        public DbSet<StationsModel> Stations { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<UsertoDepartmentModel> UsertoDepartment { get; set; }
        public DbSet<UsertoStationsModel> UsertoStations { get; set; }

        public DbSet<DepartmenttoStationModel>  DepartmenttoStation { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UsertoRoleModel>(
                    eb =>
                    {
                        eb.HasNoKey();
                    });

            modelBuilder
               .Entity<RoletoAuthory>(
                   eb =>
                   {
                       eb.HasNoKey();
                   });
            modelBuilder
              .Entity<UsertoDepartmentModel>(
                  eb =>
                  {
                      eb.HasNoKey();
                  });
            modelBuilder
            .Entity<UsertoStationsModel>(
                eb =>
                {
                    eb.HasNoKey();
                });
            modelBuilder
           .Entity<DepartmenttoStationModel>(
               eb =>
               {
                   eb.HasNoKey();
               });
        }

    }

}
