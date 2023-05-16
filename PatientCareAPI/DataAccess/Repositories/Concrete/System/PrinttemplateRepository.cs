using PatientCareAPI.DataAccess.Repositories.Abstract.System;
using PatientCareAPI.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.System
{
    public class PrinttemplateRepository : Repository<PrinttemplateModel> , IPrinttemplateRepository
    {
        public PrinttemplateRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
