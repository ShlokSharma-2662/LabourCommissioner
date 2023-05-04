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
    public class BOCWPSYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; } 
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        [Required(ErrorMessage = "પ્રસુતિની સંભવિત તારીખ લખો")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime expecteddeliverydate { get; set; }

        [Required(ErrorMessage = "પ્રસુતિ ક્રમાંક પસંદ કરો.")]
        public int prasutino { get; set; }
        [Required(ErrorMessage = "પ્રસુતિ વખતે ઉંમર લખો.")]
        public int age { get; set; }
        [Required(ErrorMessage = "જન્મનુ પ્રમાણપત્ર નંબર લખો.")]
        
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public long totalsahay { get; set; }
        public int diffdays { get; set; }
        public string strexpecteddeliverydate { get; set; }

    }
}
