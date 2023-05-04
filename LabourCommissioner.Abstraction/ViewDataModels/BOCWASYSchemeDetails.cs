using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class BOCWASYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        [Required(ErrorMessage = "પૂરું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? name { get; set; }

        [Required(ErrorMessage = "લાભાર્થીનો શ્રમિક સાથેનો સબંધ પસંદ કરો")]
        public string? Relation { get; set; }
        [Required(ErrorMessage = "મરણ ની તારીખ લખો")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public DateTime deathdate { get; set; }
        public string deathtimeage { get; set; }

        [Required(ErrorMessage = "મરણ નું પ્રમાણપત્ર નંબર લખો.")]
        public string? deathcertino { get; set; }
       
        [Phone]
        [Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "મોબાઇલ નંબર બરાબર નથી.")]
        public string? mobileno { get; set; }
        [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "ઈ-મેઈલ આઈડી બરાબર નથી.")]

        public string? email { get; set; }
        public int totalsahay { get; set; }


    }
   
}
