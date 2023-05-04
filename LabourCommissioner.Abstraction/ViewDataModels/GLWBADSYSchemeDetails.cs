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
    public class GLWBADSYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public string? ENirmanCardNo { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        [Required(ErrorMessage = "અકસ્માતની તારીખ લખો")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateofaccident { get; set; }

        [Range(1, 100, ErrorMessage = "કૃપા કરીને 1 કરતા વધારે અથવા તેના બરાબર મૂલ્ય દાખલ કરો")]
        [Required(ErrorMessage = "દિવ્યાંગતાની ટકાવારી")]
        public decimal disabilitypercentage { get; set; }

        public string? disabilitytypes { get; set; }

        [Required(ErrorMessage = "FIR નં લખો.")]
        public decimal firnum { get; set; }

        [Required(ErrorMessage = "FIR તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime firdate { get; set; }
        [Required(ErrorMessage = "અકસ્માતની વિગત (ટુંકમાં) લખો.")]
        public string accidentdetails { get; set; }

        [Required(ErrorMessage = "વારસદારનુ સરનામું લખો .")]
        public string address { get; set; }

        [Required(ErrorMessage = "જિલ્લો પસંદ કરો")]
        public int district { get; set; }
        public string pdistrictnameineng { get; set; }
        [Required(ErrorMessage = "રાજ્ય પસંદ કરો")]
        public int state { get; set; }
        [Required(ErrorMessage = "તાલુકો પસંદ કરો")]
        public int taluka { get; set; }
        public string ptalukanameineng { get; set; }
        [Required(ErrorMessage = "ગામ પસંદ કરો")]
        public int village { get; set; }
        public string pvillagenameineng { get; set; }


        [Required(ErrorMessage = "પિનકોડ લખો.")]
        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "હાલ નું પિનકોડ બરાબર નથી.")]
        public long? pincode { get; set; }

        public int totalsahay { get; set; }
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


        [Required(ErrorMessage = "રકમ લખો.")]
        public int amount { get; set; }

        [Required(ErrorMessage = "બેંક નુ નામ લખો.")]
        public string banknames { get; set; }

        [Required(ErrorMessage = " બ્રાન્ચ નુ નામ લખો.")]
        public string branchname { get; set; }

        [Required(ErrorMessage = " તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BDate { get; set; }

        [Required(ErrorMessage = " ચેક નંબર લખો.")]
        public int checknumber { get; set; }

        public string BDates { get; set; }
        public string dateofaccidents { get; set; }
    }
}
