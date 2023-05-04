using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.WebPages.Html;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class GLWB_TSYClaim_personalDetails : BankDetails
    {
        public string RegistrationNo { get; set; }
        public string registrationname { get; set; }
        public string? ENirmanCardNo { get; set; }
        public string? LwbAccountNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ICardFromDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ICardToDate { get; set; }
        public int ApplicationId { get; set; }
        public int ClaimApplicationId { get; set; }
        public string? ApplicationNo { get; set; }
        public int RegistrationId { get; set; }
        public int UserProfileId { get; set; }

        [Required(ErrorMessage = "હોસ્પિટલનું નામ લખો.")]
        public string? hospitalname { get; set; }

        [Required(ErrorMessage = " હોસ્પિટલનો ઈમેલ લખો.")]
        public string? hospitalemailid { get; set; }

        [Required(ErrorMessage = "હોસ્પિટલનું સરનામું લખો.")]
        public string? hospitaladdress { get; set; }

        [Required(ErrorMessage = " હોસ્પિટલનો મોબાઇલ નંબર લખો.")]
        public string? hospitalmobile { get; set; }

        [Required(ErrorMessage = "હોસ્પિટલનો પીનકોડ લખો.")]
        public int hospitalpincode { get; set; }

        public string schemaname { get; set; }
        public string tablename { get; set; }
        public int IsAgree { get; set; }
        public int IsApproved { get; set; }
        public int LevelNo { get; set; }
        public int ToPostId { get; set; }
        public string Remarks { get; set; }
        public bool IsSubmitted { get; set; }
        public bool IsReverted { get; set; }
        public bool IsFilled { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        //public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
       // public string GreenSoldierReferralcode { get; set; }
        public int BeneficiaryType { get; set; }

        //public string DOutWordNo { get; set; }
        //public string ROutWordNo { get; set; }
        public DateTime DOutWordNoGenerateDate { get; set; }
        public DateTime ROutWordNoGenerateDate { get; set; }
        public int DOutWordFinYear { get; set; }
        public int ROutWordFinYear { get; set; }

        //public string AOutWordNo { get; set; }
        public DateTime AOutWordNoGenerateDate { get; set; }
        public int AOutWordFinYear { get; set; }
        //public IFormFile FormFile { get; set; }
        //public string? FileName { get; set; }

        //public string? CouchDBDocId { get; set; }
        //public string? CouchDBDocRevId { get; set; }
        public int ServiceId { get; set; }
        public int TabSequenceNo { get; set; }
        public int Error { get; set; }
        public string? ResponseMsg { get; set; }
        
        public int districtid { get; set; }

        public string? companyname { get; set; }
        public string? emailid { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? district { get; set; }
        public string? taluka { get; set; }
        public long pincode { get; set; }
        public string cdistrictname { get; set; }
        public string ctalukaname { get; set; }
        public string cvillagename { get; set; }
        public string pdistrictname { get; set; }
        public string ptalukaname { get; set; }
        public string pvillagename { get; set; }
        public int applicationfrom { get; set; }

    }
}
