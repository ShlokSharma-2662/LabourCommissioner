using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class PaymentDetails
    {
        public string serviceid { get; set; }
        public string[]? aadeshidlist { get; set; }
        public string[]? payinfoidlist { get; set; }
        public string[]? applicationidlist { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string amount { get; set; }

    }
}
