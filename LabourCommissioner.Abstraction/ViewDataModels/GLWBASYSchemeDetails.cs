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
    public class GLWBASYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public string? ENirmanCardNo { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; } 
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        public string? relation { get; set; }

        public string nomineename { get; set; }

        public string nomineeaadharcardno { get; set; }

        public string deathtimeage { get; set; }

        [Required(ErrorMessage = "મૃત્યુનુ સ્થળ લખો.")]
        public string? deathplace { get; set; }

        [Required(ErrorMessage = "મરણ ની તારીખ લખો")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime deathdate { get; set; }

        [Required(ErrorMessage = "FIR નં લખો.")]
        public decimal firnum { get; set; }

        [Required(ErrorMessage = "FIR તારીખ લખો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime firdate { get; set; }

        public string address { get; set; }

        public int district { get; set; }
        public string pdistrictnameineng { get; set; }
        public int state { get; set; }
        public int taluka { get; set; }
        public string ptalukanameineng { get; set; }
        public int village { get; set; }
        public string pvillagenameineng { get; set; }


        public long? pincode { get; set; }

        //public int totalsahay { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string talukas { get; set; }
        public string villages { get; set; }
        public string states { get; set; }
        public string districts { get; set; }
        public long totalsahay { get; set; }
    }
}
