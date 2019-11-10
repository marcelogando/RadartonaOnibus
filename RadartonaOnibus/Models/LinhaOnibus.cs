using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RadartonaOnibus.Models
{
    public class LinhaOnibus
    {
        public string c { get; set; }
        public int cl { get; set; }
        public int sl { get; set; }
        public string lt0 { get; set; }
        public string lt1 { get; set; }
        public int qv { get; set; }
        public List<Onibus> vs { get; set; }
    }
}
