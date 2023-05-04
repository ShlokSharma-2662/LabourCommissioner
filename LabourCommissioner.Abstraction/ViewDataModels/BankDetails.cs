using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public partial class BankDetails
    {

        [Required(ErrorMessage = "બેંક નું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        public string? BankName { get; set; }

        [Required(ErrorMessage = "બેંક ની શાખાનું નામ લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        public string? BankBranch { get; set; }

        [Required(ErrorMessage = "બેંક નો આઇ.એફ.એસ.સી નંબર લખો.")]
        [StringLength(15, ErrorMessage = "Maximum 15 Characters Allowed")]
        [RegularExpression("^[A-Z]{4}0[A-Z0-9]{6}$" +
            "", ErrorMessage = "આઇ.એફ.એસ.સી કોડ અમાન્ય છે.")]
        public string? IFSCCode { get; set; }

        [Required(ErrorMessage = "બેંક નો એકાઉન્ટ નંબર લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"^\d{9,18}$", ErrorMessage = "બઁક અકાઉંટ નંબર અમાન્ય છે.")]
        public string? BankAccountNo { get; set; }


        [Compare("BankAccountNo", ErrorMessage = "બેંક એકાઉન્ટ નંબર અમાન્ય છે.")]
        public string? ConfirmBankAccountNo { get; set; }

        [Required(ErrorMessage = "બેંક અકાઉંટ હોલ્ડર નું નામ લખો.")]
        [StringLength(200, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? AccountHolderName { get; set; }
    }
}
