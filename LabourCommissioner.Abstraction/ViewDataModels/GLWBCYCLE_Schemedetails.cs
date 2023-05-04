using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class GLWBCYCLE_Schemedetails : BankDetails
    {
        public int SchemeId { get; set; }
        public string? ENirmanCardNo { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }


        [Required(ErrorMessage = "સાયકલની કિંમત with GST લખો.")]
        public long cyclers { get; set; }
        [Required(ErrorMessage = "બીલ નંબર લખો.")]
        public string? billno { get; set; }
        [Required(ErrorMessage = "ચેસીસ નંબર લખો.")]
        public string? chasisno { get; set; }
        [Required(ErrorMessage = "સાયકલની સાઈઝ લખો.")]
        [Range(22, 9999999, ErrorMessage = "સાયકલ 22 કે તેથી વધુ ઇંચ ની હોવી જોઇએ")]
        public long cyclesize { get; set; }
       
        [Required(ErrorMessage = "બિલનો જી.એસ.ટી નંબર લખો.")]        
        public string? gstno { get; set; }
        
        [Required(ErrorMessage = "બીલ તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime billdate { get; set; } 







        


        public DateTime SubmittedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string Benifitsrs { get; set; }
        public string billdates { get; set; }
        public decimal totalsahay { get; set; }
    }
}
