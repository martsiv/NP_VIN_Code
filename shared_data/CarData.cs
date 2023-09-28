using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared_data
{
    [Serializable]
    public class CarData
    {
        public string digits { get; set; }
        public string vin { get; set; }
        public Region region { get; set; }
        public string vendor { get; set; }
        public string model { get; set; }
        public int model_year { get; set; }
        public string photo_url { get; set; }
        public bool is_stolen { get; set; }
        public object? stolen_details { get; set; }
        public List<Operation> operations { get; set; } = new List<Operation>();
    }
}
