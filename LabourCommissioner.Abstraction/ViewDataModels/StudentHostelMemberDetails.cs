using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class StudentHostelMemberDetails
    {
        public long familydetailsid { get; set; }
        //public string removeList { get; set; }
        [Required(ErrorMessage = "વિદ્યાર્થિ નુ નામ નાખો  ")]
        public string sname { get; set; }
        [Required(ErrorMessage = "હોસ્ટેલ મા રહે છે કે કેમ? નાખો  ")]
        public int isstayhostel { get; set; }
        [Required(ErrorMessage = "હોસ્ટેલનું નામ નાખો  ")]
        public string hostelname { get; set; }
        [Required(ErrorMessage = "સરનામુ નાખો  ")]
        public string hosteladdress { get; set; }
        [Required(ErrorMessage = "હોસ્ટેલ નો પ્રકાર: સરકાર/ખાનગી/ગ્રાન્ટ-ઇન-એડ/જ્ઞા તિ ધ્વારા સંચાલિત છે  નાખો  ")]
        public string hosteltype { get; set; }
        public long CreatedBy { get; set; }
        public bool isDeleted { get; set; }

    }
}
