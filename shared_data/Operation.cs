using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared_data
{
    [Serializable]
    public class Operation
    {
        public bool is_last { get; set; }
        public string digits { get; set; }
        public string registered_at { get; set; }
        public int model_year { get; set; }
        public string vendor { get; set; }
        public string vendor_slug { get; set; }
        public string model { get; set; }
        public string department { get; set; }
        public Color color { get; set; }
        public bool is_registered_to_company { get; set; }
        public string address { get; set; }
        public long koatuu { get; set; }
        public int displacement { get; set; }
        public Kind kind { get; set; }
        public OperationGroup operation_group { get; set; }
        public Fuel fuel { get; set; } 
        public int own_weight { get; set; }
        public int total_weight { get; set; }
    }

}
