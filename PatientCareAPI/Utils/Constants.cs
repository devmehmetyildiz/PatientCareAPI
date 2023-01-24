using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Utils
{
    public static class Constants
    {
        public enum Movementtypes
        {
           income = 1,
           outcome = -1,
           transfer = 0
        }

        public enum Casetypes
        {
            Complated = 0,
            
        }
        public enum Patienttypes
        {
            KurumdanCıkıs = 0,
            KurumaGiriş = 1,
            İlkKayıt = 2,
            HastaneCıkıs = 3,
            HastaneGiris=4,
            Ölüm=5,
            Kontrol=6
        }
        public enum Stockstatus
        {
            IsActive = 0,
            IsCompleted = 1,
            IsTransfered = 2,
            IsDeactivated = 3
        }
    }
}
