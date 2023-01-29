using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class TodogrouptoTodoRepository : Repository<TodogrouptoTodoModel>, ITodogrouptoTodoRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<TodogrouptoTodoModel> _dbSet;
        public TodogrouptoTodoRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<TodogrouptoTodoModel>();
        }

        public void RemoveTodosfromTodogroup(string groupID)
        {
            string query = $"DELETE FROM `TodogrouptoTodos` WHERE `GroupID` = '{groupID}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

       
    }
}
