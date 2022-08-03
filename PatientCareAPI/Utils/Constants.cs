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
    }
}
