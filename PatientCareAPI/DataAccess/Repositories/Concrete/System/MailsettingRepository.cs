using PatientCareAPI.DataAccess.Repositories.Abstract.System;
using PatientCareAPI.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.System
{
    public class MailsettingRepository : Repository<MailsettingModel> , IMailsettingRepository
    {
        public MailsettingRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
