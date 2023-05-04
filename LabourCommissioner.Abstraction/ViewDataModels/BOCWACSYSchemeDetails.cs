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
    public class BOCWACSYSchemeDetails : BankDetails
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


        //[Required(ErrorMessage = "લાભાર્થીનો શ્રમિક સાથેનો સબંધ પસંદ કરો")]
        public string? relation { get; set; }

        [Required(ErrorMessage = "મૃત્યુનુ સ્થળ લખો.")]
        public string? deathplace { get; set; }

        [Required(ErrorMessage = "અકસ્માતની તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime dateofaccident { get; set; }

    //    [Required(ErrorMessage = "વારસદારનુ નામ લખો.")]
        public string nomineename { get; set; }

        //[Required(ErrorMessage = "વારસદારનુ આધાર કાર્ડ નં લખો.")]
        //[RegularExpression(@"^[0-9]{12}$", ErrorMessage = "ફક્ત નંબર અને ૧૨ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        public string nomineeaadharcardno { get; set; }

        [Required(ErrorMessage = "દિવ્યાંગતાની ટકાવારી લખો")]
        public decimal disabilitypercentage { get; set; }

        public string? disabilitytypes { get; set; }

        [Required(ErrorMessage = "મરણ ની તારીખ લખો")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime deathdate { get; set; }
        public string deathtimeage { get; set; }

        [Required(ErrorMessage = "વારસદારનુ સરનામું લખો .")]
        public string address { get; set; }

        [Required(ErrorMessage = "જિલ્લો પસંદ કરો")]
        public int district { get; set; }
        public string districts { get; set; }
        public string pdistrictnameineng { get; set; }

        [Required(ErrorMessage = "રાજ્ય પસંદ કરો")]
        public int state { get; set; }
        public string states { get; set; }
        [Required(ErrorMessage = "તાલુકો પસંદ કરો")]
        public int taluka { get; set; }
        public string talukas { get; set; }
        public string ptalukanameineng { get; set; }
        [Required(ErrorMessage = "ગામ પસંદ કરો")]
        public int village { get; set; }
        public string villages { get; set; }
        public string pvillagenameineng { get; set; }

        public int deathdisability { get; set; }


        [Required(ErrorMessage = "પિનકોડ લખો.")]
        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "હાલ નું પિનકોડ બરાબર નથી.")]
        public long? pincode { get; set; }

        public int totalsahay { get; set; }


    }
   
}
