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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelBinderAttribute = Microsoft.AspNetCore.Mvc.ModelBinderAttribute;

namespace LabourCommissioner.Abstraction.ViewDataModels
{


    public class WorkerPersonalDetailsModel
    {
        public string RegistrationNo { get; set; }
        //public string? ENirmanCardNo { get; set; }   //0 reference

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ICardFromDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ICardToDate { get; set; }



        [Required(ErrorMessage = "પૂરું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string RegistrationName { get; set; }



    }
}
