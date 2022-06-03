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

        public DbSet<YetkiModel> Tbl_yetkis { get; set; }
        public DbSet<UsersModel> Tbl_Users { get; set; }
        public DbSet<RoleModel> Tbl_Authory { get; set; }
        public DbSet<UsertoRoleModel> User_to_Authory { get; set; }
        public DbSet<RoletoYetki> Authory_to_yetkis { get; set; }
        public DbSet<UsertoSaltModel> Users_to_Salt { get; set; }
        public DbSet<CaseModel> Tbl_Case { get; set; }
        public DbSet<UnitModel> Tbl_Unit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UsertoRoleModel>(
                    eb =>
                    {
                        eb.HasNoKey();
                    });

            modelBuilder
               .Entity<RoletoYetki>(
                   eb =>
                   {
                       eb.HasNoKey();
                   });
        }

    }

}
