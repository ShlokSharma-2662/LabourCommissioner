using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class StudentMemberDetails
    {
       

        public int familydetailsid { get; set; }
        //public string removeList { get; set; }
        [Required(ErrorMessage = "નામ નાખો  ")]
        public string? name { get; set; }
        [Required(ErrorMessage = "ઉંમર નાખો ")]
        public string? age { get; set; }
        [Required(ErrorMessage = "ધોરણ/કોર્સ નાખો  ")]
        public string stdorcourse { get; set; }
        [Required(ErrorMessage = " શાળા/કોલેજ/સંસ્થાનું નામ નાખો  ")]
        public string schoolcollagename { get; set; }

        public long CreatedBy { get; set; }
        public bool isDeleted { get; set; }

    }
}
