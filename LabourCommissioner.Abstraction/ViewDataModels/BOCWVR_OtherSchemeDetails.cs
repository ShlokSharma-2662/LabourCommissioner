using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class BOCWVR_OtherSchemeDetails
    {


        public int otherschemedetailsid { get; set; }
        //public string removeList { get; set; }

        [Required(ErrorMessage = "યોજનાનું નામ નાખો  ")]
        public string? otherschemename { get; set; }

        [Required(ErrorMessage = "સહાય ની રકમ નાખો ")]
        [RegularExpression(@"^[0-9]{1,11}(?:\.[0-9]{1,3})?$", ErrorMessage = "સહાય માન્ય નથી.")]
        public string? otherschemesahay { get; set; }

        [Required(ErrorMessage = "યોજનાનું નામ નાખો  ")]
        public string restschemename { get; set; }


        [Required(ErrorMessage = "સહાય ની રકમ નાખો ")]
        [RegularExpression(@"^[0-9]{1,11}(?:\.[0-9]{1,3})?$", ErrorMessage = "સહાય માન્ય નથી.")]
        public string restschemesahay { get; set; }

        public long CreatedBy { get; set; }
        public long istakeanyotherassistance { get; set; }
        public bool isDeleted { get; set; }



    }
}
