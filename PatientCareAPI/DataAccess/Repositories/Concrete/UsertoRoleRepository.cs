﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class UsertoRoleRepository : Repository<UsertoRoleModel>, IUsertoRoleRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<UsertoRoleModel> _dbSet;
        public UsertoRoleRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<UsertoRoleModel>();
        }

        public List<string> GetRolesForUser(string UserID)
        {
            return _dbSet.Where(u => u.UserID == UserID).Select(u => u.RoleID).ToList();
        }
    }
}