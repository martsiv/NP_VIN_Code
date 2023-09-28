using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared_data
{
    [Serializable]
    public class Kind
    {
        public int id { get; set; }
        public string ru { get; set; }
        public string ua { get; set; }
        public string slug { get; set; }
    }
}
