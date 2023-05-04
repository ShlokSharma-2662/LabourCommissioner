using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class ApplicationStatusModel
    {

        public string ApplicationNo { get; set; }
        public string UserId { get; set; }
        public string ServiceName { get; set; }
        public int ServiceId { get; set; }
        public string ApplicantName { get; set; }

        public string ApplicationDate { get; set; }
        public DateTime? dateofbirth { get; set; }
        public string StatusName { get; set; }
        public string RolePending { get; set; }
        public string OfficeName { get; set; }
        public string ContactPersonPost { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactNo { get; set; }
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        
        public List<ApplicationStatusDetails>  applicationStatusDetails { get; set; }


    }

    public class ApplicationStatusDetails
    {
        public string role_name { get; set; }
        public int orderby { get; set; }
        public int TimelineStatus { get; set; }
        public string description { get; set; }

        public long ApplicationId { get; set; }
    }
}
