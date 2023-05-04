using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class PendencyReportDetails
    {
        
        public string districtname { get; set; }
        public string rolename { get; set; }

        public long districtid { get; set; }

        public long talukaid { get; set; }

        public string fwdistrict { get; set; }
        public string totalapplication { get; set; }
        public string sendback { get; set; }
        public string totalpending { get; set; }
        public string pending { get; set; }
        public string approved { get; set; }
        public long rejected { get; set; }
        public long orderby { get; set; }
        public string url { get; set; }
        public long rownumber { get; set; }
    }
}
