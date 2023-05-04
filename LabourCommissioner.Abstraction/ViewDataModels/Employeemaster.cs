using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public partial class Employeemaster
    {
       
        public long EmployeeId { get; set; }
        public long RegistrationId { get; set; }

        //[Required(ErrorMessage = "કર્મચારીનો વિભાગ પસંદ કરો.")]
        public long? EmployeeType { get; set; }
        public int RoleId { get; set; }
        
        [Required(ErrorMessage = "યુઝરનેમ નાખવું જરૂરી છે")]
        [Display(Name = "Username")]
        [StringLength(100, ErrorMessage = "કૃપા કરીને સાચું યુઝરનેમ નાખો.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "પાસવર્ડ નાખવો જરૂરી છે.")]
        [RegularExpression("^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{8,}$", ErrorMessage = "The Password must have minimum 8 characters and at least one numeric and one special character")]
        [StringLength(15, ErrorMessage = "કૃપા કરીને સાચો પાસવર્ડ નાખો.")]
        public string Password { get; set; } = null!;

        
        public string? postname { get; set; }

        [Required(ErrorMessage = "યુઝર ટાઇપ જરૂરી છે.")]
        public int UserType { get; set; }
        public int DistrictId { get; set; } 
        public int VillageId { get; set; }
        public int TalukaId { get; set; }
        public int OrderBy { get; set; }
        public int OEMId { get; set; }
        public int DealerId { get; set; }
        public bool IsUrban { get; set; }
       
        public string? contactno { get; set; }
        public string? EmailId { get; set; }
        
        public int BeneficiaryType { get; set; }
        public long PostId { get; set; }
        public long companyuserid { get; set; }
        public string? companyname { get; set; }

        public long hospitaluserid { get; set; }
        public string? name { get; set; }
        public bool isfilledauthority { get; set; }

    }
}
