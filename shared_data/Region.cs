using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared_data
{
    [Serializable]
    public class Region
    {
        public string name { get; set; }
        public string name_ua { get; set; }
        public string slug { get; set; }
        public string old_code { get; set; }
        public string new_code { get; set; }
        public string new_code_1 { get; set; }
        public string new_code_2 { get; set; }
    }
}
