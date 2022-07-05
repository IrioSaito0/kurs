using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Botcode
{
    public class publicAPIData
    {
        string next { get; set; }
        string previous { get; set; }
        public List<data> results { get; set; }
        public class data
        { 
        
            public string title { get; set; }
            public string description { get; set; }
            public string category { get; set; }
            public List<string> labels { get; set; }
            public int rank { get; set; }
            public DateTime start { get; set; }
            public DateTime end { get; set; }
            public string country { get; set; }
        }
    }
}
