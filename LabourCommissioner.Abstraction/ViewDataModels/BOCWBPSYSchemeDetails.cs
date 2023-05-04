using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ModelBinderAttribute = Microsoft.AspNetCore.Mvc.ModelBinderAttribute;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class BOCWBPSYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }
        public int diffdays { get; set; }

        [Required(ErrorMessage = "પ્રસુતિની સંભવિત તારીખ લખો")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpectedDeliveryDate { get; set; }

        [Required(ErrorMessage = "પ્રસુતિ ક્રમાંક પસંદ કરો.")]
        public int? PrasutiNo { get; set; }

        [Required(ErrorMessage = "પસંદ કરો.")]
        public int? AlreadyApplied { get; set; }

        //[Required(ErrorMessage = "પ્રસુતિ વખતે ઉંમર લખો.")]
        //[Range(19,100, ErrorMessage = "પ્રસુતિ વખતે ઉંમર વર્ષ મર્યાદા ૧૯ હોવી ફરજિયાત છે")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "જન્મ તારીખ પસંદ કરો.")]
        //[Microsoft.AspNetCore.Mvc.Route("IsAlreadySigned", "Register", HttpMethod = "POST", ErrorMessage = "EmailId already exists in database.")]
        //[Microsoft.AspNetCore.Mvc.Remote(action: "DateValidation", controller: "BOCWSikshanSahayYojana")]
        //[Microsoft.AspNetCore.Mvc.Remote(action: "DateValidation", controller: "BOCWSikshanSahayYojana")]
        //[Remote(action: "DateValidation", controller: "BOCWSikshanSahayYojana")]
        //[Remote(action:"DateValidation", controller: "BOCWSikshanSahayYojana")]
        //[RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "કૃપા કરીને dd/MM/yyyy ફોર્મેટમાં તારીખ દાખલ કરો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PsDateOfBirth { get; set; }
        public int totalsahay { get; set; }
        [Required(ErrorMessage = "જન્મનુ પ્રમાણપત્ર નંબર લખો.")]
        public string BirthCertificateNo { get; set; }

        [Required(ErrorMessage = "જન્મના પ્રમાણપત્ર ઈશ્યૂ કરનાર સંસ્થાનું નામ લખો.")]
        public string BirthCertificateIssueCompany { get; set; }

        [Required(ErrorMessage = "બાળકની જાતિ પસંદ કરો.")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "જન્મનુ પ્રમાણપત્રની તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$", ErrorMessage = "કૃપા કરીને dd/MM/yyyy ફોર્મેટમાં તારીખ દાખલ કરો.")]
        public DateTime BirthCertificateIssueDate { get; set; }

        [Required(ErrorMessage = "દિકરી / દીકરા નું નામ લખો.")]
        public string KidsName { get; set; }
        [Required(ErrorMessage = "નોમીનીઝનું નામ લખો.")]
        public string NomineeName { get; set; }

        [Required(ErrorMessage = "અરાજદાર સાથેનો સબંધ પસંદ કરો")]
        public int Relation { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsBPSY { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public string expecteddeliverydates { get; set; }
        public string strpsdateofbirth { get; set; }

    }
}
