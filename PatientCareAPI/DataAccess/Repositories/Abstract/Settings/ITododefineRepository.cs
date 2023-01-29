using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface ITododefineRepository : IRepository<TododefineModel>
    {
        List<TododefineModel> GetTodosbyGuids(List<string> todos);
    }
}
