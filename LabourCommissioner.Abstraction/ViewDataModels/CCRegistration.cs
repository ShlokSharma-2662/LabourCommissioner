using LabourCommissioner.Abstraction.ViewDataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public partial class CCRegistration : BaseDataTableEntity
    {

        public long RegistrationId { get; set; }
        public string? RegistrationNo { get; set; }

        [Required(ErrorMessage = "યુઝરનો પ્રકાર પસંદ કરો.")]
        public int UserType { get; set; }

        [Required(ErrorMessage = "પૂરું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(12)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "ફક્ત નંબર અને ૧૦ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        public string? MobileNo { get; set; }


        [Required(ErrorMessage = "ઈ-મેઈલ આઈડી લખો."), MaxLength(100)]
        [RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "ઈ-મેઈલ આઈડી બરાબર નથી.")]
        public string? EmailId { get; set; }

        [Required(ErrorMessage = "પાસવર્ડ લખો."), DataType(DataType.Password)]
        [RegularExpression("^(?=.*[0-9])(?=.*[!@#$%^&*])[0-9a-zA-Z!@#$%^&*0-9]{8,}$", ErrorMessage = "The Password must have minimum 8 characters and at least one numeric and one special character")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "કન્ફર્મ પાસવર્ડ લખો.")]
        [DataType(DataType.Password), Compare(nameof(Password),ErrorMessage = "પાસવર્ડ અને કન્ફર્મ પાસવર્ડ મેચ થતાં નથી.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "પાન નંબર / ટાન નંબર લખો.")]
        //[RegularExpression("([A-Z]{5}[0-9]{4}[A-Z]{1}|[A-Z]{4}[0-9]{5}[A-Z]{1})", ErrorMessage = "પાન નંબર / ટાન નંબર બરાબર નથી.")]
        [Remote(action: "UserAlreadyExist", controller: "CCRegistration", HttpMethod = "POST", ErrorMessage = "PAN No./TAN No. already exists in database.")]
        //[Remote(action: "UserAlreadyExist", controller: "CCRegistration", HttpMethod = "POST")]
        public string? PANTANNo { get; set; }
        public string? ipaddress { get; set; }
        public string? hostname { get; set; }

        [Required(ErrorMessage = "પૂરું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? resname { get; set; }

        [Required(ErrorMessage = "હોદ્દો લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? resdesignation { get; set; }

        [Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(12)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "ફક્ત નંબર અને ૧૦ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        public string? resmobileno { get; set; }

        [Required(ErrorMessage = "પૂરું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? locname { get; set; }

        [Required(ErrorMessage = "હોદ્દો લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? locdesignation { get; set; }

        [Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(12)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "ફક્ત નંબર અને ૧૦ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        public string? locmobileno { get; set; }

    }
    
}
