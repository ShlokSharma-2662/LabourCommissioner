using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class NanajiFamilyMemberDetails
    {
        public long familydetailsid { get; set; }
        //public string removeList { get; set; }

        [Required(ErrorMessage = "નામ નાખો")]
        public string? familymembersname { get; set; }

        [Required(ErrorMessage = "સંબંધ નાખો  ")]
        public string? relation { get; set; }     
         
        public long CreatedBy { get; set; }
        public bool isDeleted { get; set; }

    }
}
