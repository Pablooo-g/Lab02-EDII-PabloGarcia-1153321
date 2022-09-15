using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public class Persona
    {
        public string name { get; set; }
        public string dpi { get; set; }
        public DateTime datebirth { get; set; }
        public string address { get; set; }
        public string[] companies { get; set; }

        public List<Dictionary<char, Letra>> dictionaries { get; set; }
        
        public List<double> codes { get; set; }
        
    }
}
