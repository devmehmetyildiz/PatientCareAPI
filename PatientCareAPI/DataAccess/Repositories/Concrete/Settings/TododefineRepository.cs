using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class TododefineRepository : Repository<TododefineModel>, ITododefineRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<TododefineModel> _dbSet;
        public TododefineRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<TododefineModel>();
        }

        public List<TododefineModel> GetTodosbyGuids(List<string> todos)
        {
            if (todos.Count == 0)
            {
                return new List<TododefineModel>();
            }
            string query = "";
            query += "select * from Tododefines  where ConcurrencyStamp IN (";
            for (int i = 0; i < todos.Count; i++)
            {
                query += $"'{todos[i]}'";
                if (i != todos.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList();
        }
    }
}
