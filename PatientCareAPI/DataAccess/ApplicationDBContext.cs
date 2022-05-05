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

        public DbSet<RolesModel> Tbl_Roles { get; set; }
        public DbSet<UsersModel> Tbl_Users { get; set; }
        public DbSet<UsertoRoleModel> Users_to_Role { get; set; }
        public DbSet<UsertoSaltModel> Users_to_Salt { get; set; }
        public DbSet<CaseModel> Tbl_Case { get; set; }
        public DbSet<UnitModel> Tbl_Unit { get; set; }
    }
}
