using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class CountForDashBoard
    {
        public long totalapplicationcount { get; set; }
        public long totalapplication { get; set; }
        public long sendback { get; set; }
        public long approved { get; set; }
        public long pending { get; set; }
        public long rejected { get; set; }
        public long totalcount { get; set; }
        public long serviceid { get; set; }
        public string countyype { get; set; }
        public string backgroundcolor { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public string servicename { get; set; }
        public decimal apppercentage { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
    }
}
