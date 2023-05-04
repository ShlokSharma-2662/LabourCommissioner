using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class ApplicationCountForDashBoard
    {
        public long totalcount { get; set; }
        public string districtname { get; set; }
        public long pending { get; set; }
        public long approved { get; set; }
        public long rejected { get; set; }
    }
}
