using LabourCommissioner.Abstraction.ViewDataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public partial class CMDApplicationDetails : BaseDataTableEntity
    {
        public long srno { get; set; }
        public string servicename { get; set; }
        public string servicenamegujarati { get; set; }
        public long serviceid { get; set; }
        public long appyear { get; set; }
        public long appmonth { get; set; }
        public bool issubmitted { get; set; }
        public int status { get; set; }
        public long userId { get; set; }
        public string? ipaddress { get; set; }
        public string? hostname { get; set; }
        public int applicationfrom { get; set; }
        public List<CMDApplicationDetailsList> lstCMDApplicationDetails { get; set; }
    }

    public class CMDApplicationDetailsList
    {
        public long srno { get; set; }
        public long totalpagecount { get; set; }
        public string? districtname { get; set; }
        public string? districtnameguj { get; set; }
        public long districtid { get; set; }
        public long serviceid { get; set; }
        public long applicationid { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long appyear { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long appmonth { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long fintarget { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long phytarget { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long phyachievement { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long finachievement { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long appreceived { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long appsanction { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long appreject { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long apppending { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long appdaypending { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only number is allowed")]
        public long dcode { get; set; }

        [Required(ErrorMessage = "Please select As on Date")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? asondate { get; set; }
        public bool issubmitted { get; set; }



    }

}
