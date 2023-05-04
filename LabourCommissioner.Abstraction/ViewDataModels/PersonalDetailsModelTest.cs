using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.Mvc;
using System.Web.WebPages.Html;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.AspNetCore.Mvc;
//using ModelBinderAttribute = Microsoft.AspNetCore.Mvc.ModelBinderAttribute;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelBinderAttribute = Microsoft.AspNetCore.Mvc.ModelBinderAttribute;
using Microsoft.AspNetCore.Routing;

namespace LabourCommissioner.Abstraction.ViewDataModels
{


    public class PersonalDetailsModelTest
    {
        public string RegistrationNo { get; set; }
        //public string? ENirmanCardNo { get; set; }   //0 reference

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ICardFromDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ICardToDate { get; set; }

        public string RegisteredUserName { get; set; }
        public int ApplicationId { get; set; }
        public string ApplicationNo { get; set; }
        public int RegistrationId { get; set; }
        public int UserProfileId { get; set; }

        [Required(ErrorMessage = "ઉમર લખો."), MaxLength(3)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter only Numaric")]  //Translate
        [Range(18, 100, ErrorMessage = "ઉંમર વર્ષ મર્યાદા ૧૮ હોવી ફરજિયાત છે")]
        public string Age { get; set; }

        //[RequiredIf("isfilled", "false")]
        public string Phototest { get; set; }
        public IFormFile? Photo { get; set; }

        [Required(ErrorMessage = "પૂરું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string Name { get; set; }



        [Required(ErrorMessage = "આધાર કાર્ડ નંબર  લખો.")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "ફક્ત નંબર અને ૧૨ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        public string AadharCardNo { get; set; }

        public string MaskedAadharCardNo { get; set; }

        [Required(ErrorMessage = "ગુજરાતીમાં પૂરું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        public string NameinGujarati { get; set; }

        [Required(ErrorMessage = "ગુજરાતીમાં પિતા/પતિ નું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        public string FatherOrHusbandNameinGujarati { get; set; }

        [Required(ErrorMessage = "પિતા/પતિ નું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string FatherOrHusbandName { get; set; }



        [Required(ErrorMessage = "જન્મ તારીખ પસંદ કરો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]


        public DateTime? DateOfBirth { get; set; }



        public string DateOfBirth1 { get; set; }

        [Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(10)]
        public string MobileNo { get; set; }

        //[Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(10)]
        //public string StudentMobileNo { get; set; }    //0 reference

        public string? PhoneNo { get; set; }

        //[Required(ErrorMessage = "ઈમેલ આઈડી નાખો")]
        //[DataType(DataType.EmailAddress, ErrorMessage = "ઈ-મેઈલ આઈડી બરાબર નથી.")]
        [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "ઈ-મેઈલ આઈડી બરાબર નથી.")]
        public string? EmailId { get; set; }
        public string Caste { get; set; }
        public string castename { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }



        [Required(ErrorMessage = "જાતિ પસંદ કરો")]
        public int CasteId { get; set; }
        [Required(ErrorMessage = "લિંગ પસંદ કરો")]
        public int? Gender { get; set; }
        public int? isDisabled { get; set; }

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
        [Required(ErrorMessage = "અંગ્રેજી માં કાયમીનું સરનામું લખો.")]

        public string PAddressInEng { get; set; }
        [Required(ErrorMessage = "ગુજરાતી માં કાયમીનું સરનામું લખો.")]

        public string PAddressInGuj { get; set; }
        [Required(ErrorMessage = "કાયમી નો રાજ્ય પસંદ કરો")]

        public int PStateId { get; set; }
        [Required(ErrorMessage = "કાયમી નો જિલ્લો પસંદ કરો")]
        public int PDistrictId { get; set; }
        [Required(ErrorMessage = "કાયમી નો જિલ્લો અંગ્રેજી માં પસંદ કરો")]
        public string PDistrictNameInEng { get; set; }

        //[Required(ErrorMessage = "કાયમી નો જિલ્લો ગુજરાતી માં પસંદ કરો")]
        //public string PDistrictNameInGuj { get; set; }  //0 reference

        [Required(ErrorMessage = "કાયમી નો તાલુકો પસંદ કરો")]
        public int PTalukaId { get; set; }
        [Required(ErrorMessage = "કાયમી નો તાલુકો પસંદ કરો")]

        public string PTalukaNameInEng { get; set; }
        //[Required(ErrorMessage = "કાયમી નો તાલુકો અંગ્રેજી માં પસંદ કરો")]

        //public string PTalukaNameInGuj { get; set; } //0 reference

        [Required(ErrorMessage = "કાયમીનું ગામ ગુજરાતી માં પસંદ કરો")]
        public int PVillageId { get; set; }
        [Required(ErrorMessage = "કાયમીનું ગામ અંગ્રેજી માં પસંદ કરો")]
        public string PVillageNameInEng { get; set; }

        //[Required(ErrorMessage = "કાયમીનું ગામ ગુજરાતી માં પસંદ કરો")]
        //public string PVillageNameInGuj { get; set; } //0 reference

        [Required(ErrorMessage = "કાયમીનું પિનકોડ પસંદ કરો")]
        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "કાયમીનું પિનકોડ બરાબર નથી.")]
        public long? PPinCode { get; set; }
        public int IsAgree { get; set; }
        public int IsApproved { get; set; }
        public int LevelNo { get; set; }
        public int ToPostId { get; set; }
        //public string Remarks { get; set; } //0 reference
        public bool IsSubmitted { get; set; }
        public bool IsReverted { get; set; }
        public bool IsFilled { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        //public string ModifiedBy { get; set; } //0 reference
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }

        //public string GreenSoldierReferralcode { get; set; }  //0 reference
        //public int BeneficiaryType { get; set; }      //0 reference
        //public string DOutWordNo { get; set; }        //0 reference
        //public string ROutWordNo { get; set; }        //0 reference
        //public DateTime DOutWordNoGenerateDate { get; set; }  //0 reference
        //public DateTime ROutWordNoGenerateDate { get; set; }  //0 reference   
        //public int DOutWordFinYear { get; set; }      //0 reference
        //public int ROutWordFinYear { get; set; }      //0 reference
        //public string AOutWordNo { get; set; }        //0 reference
        //public DateTime AOutWordNoGenerateDate { get; set; }     //0 reference
        //public int AOutWordFinYear { get; set; }      //0 reference
        //public IFormFile FormFile { get; set; }       //0 reference
        public string? FileName { get; set; }

        public string? CouchDBDocId { get; set; }
        public string? CouchDBDocRevId { get; set; }
        public int ServiceId { get; set; }
        public int TabSequenceNo { get; set; }
        public int Error { get; set; }
        public string? ResponseMsg { get; set; }

        public string cdistrictname { get; set; }
        public string ctalukaname { get; set; }
        public string cvillagename { get; set; }
        public string pdistrictname { get; set; }
        public string ptalukaname { get; set; }
        public string pvillagename { get; set; }

    }

    
}
