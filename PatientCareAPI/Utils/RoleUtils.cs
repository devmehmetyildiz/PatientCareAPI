using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.Utils
{
    public class RoleUtils
    {
        UnitOfWork unitOfWork;
        ApplicationDBContext context;
        public RoleUtils()
        {         
            unitOfWork = new UnitOfWork(context);
        }

       
    }
}
