using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RadartonaOnibus.Models
{
    public class Alerta
    {
        public string tipo { get; set; }
        public decimal lat { get; set; }
        public decimal lon { get; set; }
    }
}
