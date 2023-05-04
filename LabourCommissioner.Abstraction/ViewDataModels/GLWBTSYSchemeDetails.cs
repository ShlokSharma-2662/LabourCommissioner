using LabourCommissioner.Abstraction.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class GLWBTSYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public string? ENirmanCardNo { get; set; }
        public int ApplicationId { get; set; }
        public long ClaimApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }

        public int employeeid { get; set; }

        public List<CompanyWorkerDetails> lstCompanyWorkerDetails { get; set; }

        public long fg_workerdetailsid { get; set; }
        public long fg_registrationid { get; set; }

        public string? fg_workername { get; set; }
        //public string? fg_MobileNo { get; set; }
        public string? fg_ageyear { get; set; }
        public string? fg_gender { get; set; }
        public DateTime? fg_DateOfBirth { get; set; }
        //public string fg_aadharcard { get; set; }
        public string fg_sanmanregistrationno { get; set; }
        public bool fg_ischeckup { get; set; }
        public int totalsahay { get; set; }
        public int finaltotalsahay { get; set; }

        public string lwbaccountno { get; set; }

        public int totalemployeesforcheckup { get; set; }

        public string remarks { get; set; }

    }
   
}
