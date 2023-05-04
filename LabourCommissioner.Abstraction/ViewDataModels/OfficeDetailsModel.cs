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


    public class OfficeDetailsModel
    {
        public long officeid { get; set; }
        //public string? ENirmanCardNo { get; set; }   //0 reference


        [Required(ErrorMessage = "ઓફિસ નું નામ લખો")]
        public string? officeName { get; set; }

        [Required(ErrorMessage = "ઓફિસ નો જિલ્લો પસંદ કરો")]
        public long? officedistrictid { get; set; }

        [Required(ErrorMessage = "સંપર્ક કરતાં વ્યક્તિ નું નામ")]
        public string? contactpersonname { get; set; }

        [Required(ErrorMessage = "સંપર્ક કરતાં વ્યક્તિ ની પોસ્ટ")]

        public string? contactpersonpost { get; set; }

        [Required(ErrorMessage = "સંપર્ક કરતાં વ્યક્તિ નો નંબર")]
        public string? contactpersoncontactno { get; set; }


        public string? faxno { get; set; }


        [Required(ErrorMessage = "પૂરું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? officeaddress { get; set; }

        public int? OfficeStateId { get; set; }

        [Required(ErrorMessage = "હાલ નું તાલુકો પસંદ કરો")]
        public long? OfficeTalukaId { get; set; }

        [Required(ErrorMessage = "હાલ નો ગામ પસંદ કરો")]
        public long? OfficeVillageId { get; set; }

        [Required(ErrorMessage = "હાલ નું પિનકોડ લખો.")]
        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "હાલ નું પિનકોડ બરાબર નથી.")]
        public long? OfficePinCode { get; set; }
        public int? createdby { get; set; }
        public string? ipaddress { get; set; }
        public string? hostname { get; set; }

    }


}
