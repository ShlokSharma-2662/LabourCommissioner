using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public partial class RegisteredApplicant
    {
       
        public long srno { get; set; }
        public long registrationid { get; set; }
        public string? registrationno { get; set; }
        public string? name { get; set; }
        public string? emailid { get; set; }
        public DateTime? registrationdate { get; set; }
        public DateTime? dateofbirth { get; set; }

        [Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(10)]
        [RegularExpression("^[0-9]*$",ErrorMessage ="મોબાઈલ નંબર અમાન્ય છે.")]
        public string? mobileno { get; set; }
        public string? aadharno { get; set; }
        public string? gender { get; set; }
        public long cdistrictid { get; set; }
    }
}
