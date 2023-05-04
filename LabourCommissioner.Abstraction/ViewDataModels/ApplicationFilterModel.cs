using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class ApplicationFilterModel
    {
        public long EDistrictId { get; set; }
        public long EVillageId { get; set; }
        public long ETalukaId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public int ApplicationStatus { get; set; }
        public string? MobileNo { get; set; }
        public int UserType { get; set; }
        public long PostId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public long ServiceId { get; set; }
        public string? Action { get; set; }
    }
}
