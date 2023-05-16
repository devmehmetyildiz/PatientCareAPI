using PatientCareAPI.DataAccess.Repositories.Abstract.Auth;
using PatientCareAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Auth
{
    public class ResetpasswordrequestRepository : Repository<ResetpasswordrequestModel>, IResetpasswordrequestRepository
    {
        public ResetpasswordrequestRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
