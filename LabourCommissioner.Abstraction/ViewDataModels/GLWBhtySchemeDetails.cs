using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class GLWBhtySchemeDetails : BankDetails
    {

        public int SchemeId { get; set; }
        public int i = 1;
        public string? ENirmanCardNo { get; set; }
        public string? schmename { get; set; }
       
        public string couchdbdocid { get; set; }
        public string couchdbdocrevid { get; set; }
        public int ApplicationId { get; set; }
        public long ClaimApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }



        [Required(ErrorMessage = "ઓળખપત્રોની સ્વપ્રમાણીત નકલ બીડવી")]
        public string startlocation { get; set; }

        [Required(ErrorMessage = "યોજનામાં જોડાવાની તારીખ લખો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime journeystartdate { get; set; }
        public string journeystartdates { get; set; }


        [Required(ErrorMessage = "યોજના પૂર્ણ થવાની તારીખ લખો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime journeyenddate { get; set; }
        public string journeyenddates { get; set; }
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
        public long TotalFamilyMemberCount { get; set; }
        public decimal totalsahay { get; set; }

        public List<FamilyMemberDetails> lstFamilyMemberDetails { get; set; }
        public List<FamilyMemberTravelDetails> lstFamilyMemberTravelDetails { get; set; }


        public long fg_familydetailsid { get; set; }

        public string? fg_name { get; set; }

        public string? fg_age { get; set; }

        public string? fg_relation { get; set; }
        public string? fg_joborcor { get; set; }

        public IFormFile? fg_File { get; set; }

        public string fg_islabour { get; set; }
        public string fg_aadharcard { get; set; }
        public string fg_CouchDBDocId { get; set; }
        public string fg_CouchDBDocRevId { get; set; }
       // public string? fg_DocumentName { get; set; }

        [Required(ErrorMessage = "વતન નું સ્‍થળ લખો.")]
        public string? homelandlocation { get; set; }

        public long fg_travellingdetailsid { get; set; }

        public long? fg_travelledby { get; set; }

        public long? fg_travelstatus { get; set; }


        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fg_travelldate { get; set; }
        public string? fg_fromplace { get; set; }

        public string? fg_toplace { get; set; }
        public string? fg_ticketno { get; set; }
        public string? fg_documentname { get; set; }
        public decimal fg_ticketamount { get; set; }
       

    }
}
