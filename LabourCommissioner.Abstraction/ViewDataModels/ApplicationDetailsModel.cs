using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class ApplicationDetailsModel
    {

        public int i = 1;
        public string ApplicationNo { get; set; }
        public long ApplicationId { get; set; }
        public long claim_applicationid { get; set; }
        public long count { get; set; }
        public int ServiceId { get; set; }
        public string ApplicationDate { get; set; }
        public string Name { get; set; }
        public string NameinGujarati { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameGuj { get; set; }
        public string TalukaName { get; set; }
        public string TalukaNameGuj { get; set; }
        public string CasteName { get; set; }
        public string CasteNameInGujarati { get; set; }
        public string CAddressInEng { get; set; }
        public string CAddressInGuj { get; set; }
        public string MobileNo { get; set; }
        public string ApplicationStatus { get; set; }
        public string Remarks { get; set; }
        public int EditVisible { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int districtId { get; set; }
        public int talukaId { get; set; }
        public bool isclaimsubmitted { get; set; }
        public bool isemployeeclaimed { get; set; }


        //Company Login

        public string CompanyName { get; set; }
        public string district { get; set; }
        public string lwbaccountno { get; set; }
        public string totalemployee { get; set; }
        public string totalemployeesforcheckup { get; set; }


        //Hospital


        public string fromdate { get; set; }
        public string todate { get; set; }
        //public bool isclaimsubmitted { get; set; }
        public TimeSpan? fromtime { get; set; }
        public TimeSpan? totime { get; set; }

        //Hospital Claim

        public string? hospitalname { get; set; }
        public string? hospitalemailid { get; set; }
        public string? hospitaladdress { get; set; }
        public string? hospitalmobile { get; set; }
        public string? hospitalpincode { get; set; }
        public long claimapplicationid { get; set; }
    }
}
