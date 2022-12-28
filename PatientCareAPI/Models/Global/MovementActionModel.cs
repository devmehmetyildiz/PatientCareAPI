using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Global
{
    public class MovementActionModel
    {
        public string Stockid { get; set; }
        public int Movementtype { get; set; }
        public string User { get; set; }
        public double Movementvalue { get; set; }
    }
}
