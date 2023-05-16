using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class TodoRepository : Repository<TodoModel> , ITodoRepository
    {
        public TodoRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
