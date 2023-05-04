using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class FamilyMemberTravelDetails
    {
        public long travellingdetailsid { get; set; }
        //public string removeList { get; set; }

        [Required(ErrorMessage = "સફર નો પ્રકાર પસંદ કરો.")]
        public long? travelledby { get; set; }

        [Required(ErrorMessage = "આવાનું/જવાનું પસંદ કરો.")]
        public long? travelstatus { get; set; }

        [Required(ErrorMessage = "સફર ની તારીખ પસંદ કરો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? travelldate { get; set; }
        public string? strtravelldate { get; set; }

        [Required(ErrorMessage = "સફર શરૂ થવાનો સ્થળ લખો")]
        public string? fromplace { get; set; }

        [Required(ErrorMessage = "સફરની ટિકિટ નાખો.")]
        public IFormFile Ticket { get; set; }

        [Required(ErrorMessage = "સફર પૂરો થયાનો સ્થળ નાખો.")]
        public string toplace { get; set; }

        [Required(ErrorMessage = "ટિકિટ નંબર નાખો.")]
        public string ticketno { get; set; }

        [Required(ErrorMessage = "ટિકિટની રકમ નાખો.")]
        public decimal ticketamount { get; set; }
        public string CouchDBDocId { get; set; }
        public string CouchDBDocRevId { get; set; }
        public string? DocumentName { get; set; }
        public long CreatedBy { get; set; }
        public bool isDeleted { get; set; }

    }
}
