using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public partial class ChangePasswordModel
    {

        [Required(ErrorMessage = "પાસવર્ડ નાખવો જરૂરી છે.")]
        [RegularExpression("^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{8,}$", ErrorMessage = "The Password must have minimum 8 characters and at least one numeric and one special character")]
        [StringLength(15, ErrorMessage = "કૃપા કરીને સાચો પાસવર્ડ નાખો.")]
        public string? CurrentPassword { get; set; }


        [Required(ErrorMessage = "પાસવર્ડ નાખવો જરૂરી છે.")]
        [RegularExpression("^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{8,}$", ErrorMessage = "The Password must have minimum 8 characters and at least one numeric and one special character")]
        [StringLength(15, ErrorMessage = "કૃપા કરીને સાચો પાસવર્ડ નાખો.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "કન્ફર્મ પાસવર્ડ લખો.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.CompareAttribute("NewPassword", ErrorMessage = "The NewPassword and ConfirmPassword password do not match.")]
        public string? ConfirmPassword { get; set; }
        public long UserType { get; set; }
        public long UserId { get; set; }
        public long registrationid { get; set; }
        public string? name { get; set; }
        public string? emailid { get; set; }
        public string? mobileno { get; set; }
        public string? username { get; set; }
        public long errorcode { get; set; }
        public string? errormsg { get; set; }

    }
}
