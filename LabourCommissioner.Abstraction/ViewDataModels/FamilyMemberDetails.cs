using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class FamilyMemberDetails
    {
        public long familydetailsid { get; set; }
        //public string removeList { get; set; }

        [Required(ErrorMessage = "નામ નાખો  ")]
        public string? name { get; set; }

        [Required(ErrorMessage = "ઉંમર નાખો ")]
        public string? age { get; set; }

        [Required(ErrorMessage = "અરજદાર સાથે સંબઘ પસંદ કરો ")]
        public string? relation { get; set; }
        [Required(ErrorMessage = "નોકરી/ઘંઘો કરે છે કે કેમ")]
        public string? joborcor { get; set; }


        [RequiredIf("IsFilled", false, ErrorMessage = "ઓળખપત્રોની સ્વપ્રમાણીત નકલ બીડવી")]
        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "અરજદારના આશ્રિત છે કે કેમ")]
        public string islabour { get; set; }
        [Required(ErrorMessage = "આધારકાર્ડ નંબર નાખો")]
        public string aadharcard { get; set; }
        public string CouchDBDocId { get; set; }
        public string CouchDBDocRevId { get; set; }
        public string? DocumentName { get; set; }
        public long CreatedBy { get; set; }
        public bool isDeleted { get; set; }

        public bool IsFilled { get; set; }
    }
}
