using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class familymember
    {

        public int familydetailsid { get; set; }
        [Required(ErrorMessage = "નામ નાખો  ")]
        public string famillymembernames { get; set; }
      
        public long CreatedBy { get; set; }
        public bool isDeleted { get; set; }

    }
}
