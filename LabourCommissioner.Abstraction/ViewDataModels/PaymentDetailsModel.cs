using LabourCommissioner.Abstraction.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class PaymentDetailsModel
    {
        public long srno { get; set; }
        public string? aadeshno { get; set; }
        public string? accountholdername { get; set; }
        public string? place { get; set; }
        public string? applicationno { get; set; }

        [Required(ErrorMessage = "બેંક નો એકાઉન્ટ નંબર લખો.")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"^\d{9,18}$", ErrorMessage = "બેંક એકાઉન્ટ નંબર અમાન્ય છે.")]
        public string? beneficiaryaccountno { get; set; }

        [Required(ErrorMessage = "બેંક નો આઇ.એફ.એસ.સી નંબર લખો.")]
        [StringLength(15, ErrorMessage = "Maximum 15 Characters Allowed")]
        [RegularExpression("^[A-Z]{4}0[A-Z0-9]{6}$" +
           "", ErrorMessage = "આઇ.એફ.એસ.સી કોડ અમાન્ય છે.")]
        public string? ifsccode { get; set; }
        public string? amount { get; set; }
        public string? boardname { get; set; }
        public string? boarddebitaccountnumber { get; set; }
        public string? paymenttype { get; set; }
        public DateTime? transactiondate { get; set; }
        public string? transactionstatus { get; set; }
        public int transactionstatusval { get; set; }
        public string? utrno { get; set; }
        public string? returnreason { get; set; }
        public long payinfoid { get; set; }
        public long serviceid { get; set; }
        public long applicationid { get; set; }

        [Required(ErrorMessage = "રિમાર્ક નાખવું અનિવાર્ય છે.")]
        public string? remarks { get; set; }
        public int resendforpayment { get; set; }
        public DateTime? resendforpaymentdate { get; set; }

    }
}
