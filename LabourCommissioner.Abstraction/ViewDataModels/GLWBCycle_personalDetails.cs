using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.WebPages.Html;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class GLWBCycle_personalDetails
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
        public string? ApplicationNo { get; set; }
        public int RegistrationId { get; set; }
        public int UserProfileId { get; set; }


        //[Required(ErrorMessage = "પૂરું નામ લખો.")]
        //[StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        //[RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "આધાર કાર્ડ નંબર  લખો.")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "ફક્ત નંબર અને ૧૨ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        public string AadharCardNo { get; set; }
        public string? MaskedAadharCardNo { get; set; }
        
        public string? NameinGujarati { get; set; }

        //[Required(ErrorMessage = "ગુજરાતીમાં પિતા નું નામ લખો.")]
        //[StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        public string? FatherOrHusbandNameinGujarati { get; set; }

        //[Required(ErrorMessage = "પિતા નું નામ લખો.")]
        //[StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        //[RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? FatherOrHusbandName { get; set; }

        [Required(ErrorMessage = "જન્મ તારીખ પસંદ કરો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirth1 { get; set; }
        [Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(10)]
        public string MobileNo { get; set; }

        //[Required(ErrorMessage = "ફોને નંબર લખો."), MaxLength(10)]
        public string? PhoneNo { get; set; }

        //[RequiredIf("isfilled", "false")]
        //[Required(ErrorMessage = "તમારો ફોટો અપલોડ કરવો જરૂરી છે.")]
        [RequiredIf("IsFilled", false, ErrorMessage = "તમારો ફોટો અપલોડ કરવો જરૂરી છે.")]
        public IFormFile Photo { get; set; }

        //[Required(ErrorMessage = "ઈમેલ આઈડી નાખો")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "ઈ-મેઈલ આઈડી બરાબર નથી.")]
        [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "ઈ-મેઈલ આઈડી બરાબર નથી.")]
        public string? EmailId { get; set; }

        public string castename { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public int CasteId { get; set; }
        [Required(ErrorMessage = "લિંગ પસંદ કરો")]
        public int? Gender { get; set; }

        [Required(ErrorMessage = "અંગ્રેજી માં હાલનું સરનામું લખો.")]
        public string CAddressInEng { get; set; }

        [Required(ErrorMessage = "ગુજરાતી માં હાલનું સરનામું લખો.")]
        public string CAddressInGuj { get; set; }

        [Required(ErrorMessage = "હાલ નો જિલ્લો પસંદ કરો")]
        public int CDistrictId { get; set; }

        [Required(ErrorMessage = "હાલ નો રાજ્ય પસંદ કરો")]
        public int CStateId { get; set; }
        [Required(ErrorMessage = "હાલ નું તાલુકો પસંદ કરો")]
        public int CTalukaId { get; set; }
        [Required(ErrorMessage = "હાલ નો ગામ પસંદ કરો")]
        public int CVillageId { get; set; }
        [Required(ErrorMessage = "હાલ નું પિનકોડ લખો.")]
        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "હાલ નું પિનકોડ બરાબર નથી.")]
        public long? CPinCode { get; set; }

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
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string GreenSoldierReferralcode { get; set; }
        public int BeneficiaryType { get; set; }
        public string DOutWordNo { get; set; }
        public string ROutWordNo { get; set; }
        public DateTime DOutWordNoGenerateDate { get; set; }
        public DateTime ROutWordNoGenerateDate { get; set; }
        public int DOutWordFinYear { get; set; }
        public int ROutWordFinYear { get; set; }
        public string AOutWordNo { get; set; }
        public DateTime AOutWordNoGenerateDate { get; set; }
        public int AOutWordFinYear { get; set; }
        //public IFormFile FormFile { get; set; }
        public string? FileName { get; set; }

        public string? CouchDBDocId { get; set; }
        public string? CouchDBDocRevId { get; set; }
        public int ServiceId { get; set; }
        public int TabSequenceNo { get; set; }
        public int Error { get; set; }
        public string? ResponseMsg { get; set; }
        public string? OrganizationName { get; set; }
        public string? OrganizationEmail { get; set; }
        public string? OrganizationAddress { get; set; }
        public string? OrganizationCity { get; set; }
        public string? OrganizationDistrict { get; set; }
        public string? OrganizationTaluka { get; set; }
        public long OrganizationPincode { get; set; }

        [Required(ErrorMessage = "કંપનીમાં જોડાવાની તારીખ પસંદ કરો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrganizationJoiningDate { get; set; }

        [Required(ErrorMessage = "સંપર્કકર્તા વ્યક્તિનું નામ નાખો")]
        public string? OContactPersonName { get; set; }
        [Required(ErrorMessage = "સંપર્કકર્તા વ્યક્તિનો મોબાઇલ નંબર નાખો"), MaxLength(10)]
        public string? OContactPersonMobile { get; set; }
        [Required(ErrorMessage = "સંપર્કકર્તા વ્યક્તિનો ઈમેલ નાખો")]
        [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "ઈ-મેઈલ આઈડી બરાબર નથી.")]
        public string? OContactPersonEmail { get; set; }


        [Required(ErrorMessage = "શ્રમયોગી સાથેનો સબંધ પસંદ કરો")]
        public string? Relation { get; set; }

        [Required(ErrorMessage = "અરજદારની ઉમર નાખો.")]
        public string? Age { get; set; }



        public string cdistrictname { get; set; }
        public string ctalukaname { get; set; }
        public string cvillagename { get; set; }
        public string pdistrictname { get; set; }
        public string ptalukaname { get; set; }
        public string pvillagename { get; set; }
        public int applicationfrom { get; set; }


    }
}
