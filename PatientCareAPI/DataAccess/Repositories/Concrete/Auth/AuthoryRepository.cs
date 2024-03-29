﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract.Auth;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Auth
{
    public class AuthoryRepository : Repository<AuthoryModel>, IAuthoryRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<AuthoryModel> _dbSet;
        public AuthoryRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<AuthoryModel>();
        }

       
        public AuthoryModel FindAuthoryByName(string yetkiName)
        {
            return _dbSet.FirstOrDefault(u => u.Name== yetkiName);
        }

        public AuthoryModel FindAuthoryBuGuid(string Guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == Guid);
        }

        public List<AuthoryModel> GetAuthoriesbyGuids(List<string> authoryguids)
        {
            if (authoryguids.Count == 0)
            {
                return new List<AuthoryModel>();
            }
            string query = "";
            query += "select * from authories  where ConcurrencyStamp IN (";
            for (int i = 0; i < authoryguids.Count; i++)
            {
                query += $"'{authoryguids[i]}'";
                if (i != authoryguids.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList(); ;
        }

        public bool CheckAuthByUsername(string username, string authoryname)
        {
            string query = "";
            query += "select authories.* from authories ";
            query += "left join roletoauthories on authories.ConcurrencyStamp = roletoauthories.AuthoryID ";
            query += "left join roles on roles.ConcurrencyStamp = roletoauthories.RoleID ";
            query += "left join usertoroles on usertoroles.RoleID  = roles.ConcurrencyStamp ";
            query += "left join users on users.ConcurrencyStamp = usertoroles.UserID ";
            query += $"where users.Username = '{username}' and authories.Name = '{authoryname}'";
           
            var Response = dbcontext.Authories.FromSqlRaw(query).ToList();
            if (Response.Count == 0)
                return false;
            else
                return true;
        }
    }
}
