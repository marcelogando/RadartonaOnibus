using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RadartonaOnibus.Models
{
    public class Onibus
    {
        public string nome { get; set; }
        public int p { get; set; }
        public bool a { get; set; }
        public string ta { get; set; }
        public decimal py { get; set; }
        public decimal px { get; set; }
        public bool ocorrencia { get; set; }
    }
}
