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
    public class BOCWPIPSchemeDetails : BankDetails
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
        public string schmename { get; set; }
        public string tablename { get; set; }

        //[Required(ErrorMessage = "વ્યવસાય નો પ્રકાર પસંદ કરો.")]
        public string worktype { get; set; }

        [Required(ErrorMessage = "યોજનાનુ નામ પસંદ કરો.")]
        public string schemetype { get; set; }

        [Required(ErrorMessage = "યોજનામાં જોડાવાની તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime joindate { get; set; }

        [Required(ErrorMessage = "ચાલુ વષનું પ્રીમિયમ કપાયા તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime premiumdate { get; set; }


        // [Required(ErrorMessage = "અન્ય કોઈ સામાન્ય વીમા યોજનામાં જોડાયેલ છો કે નથી લખો.")]

        public string insuranceNo { get; set; }
       // [Required(ErrorMessage = "સામાન્ય વીમા યોજનાની વિગત લખો")]
        public string insurancedetails { get; set; }

        [Required(ErrorMessage = "કુલ કપાયેલ પ્રિમ્યમની રકમ")]
        public long totalsahay { get; set; }



    }
}
