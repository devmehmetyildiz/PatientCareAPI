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
            Create = 0,
            Add = 1,
            Reduce = 2,
            Update = 3,
            Transfer = 4,
            Kill = 5,
            Delete = 6
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
    }
}
