using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class EmpApproveApplication
    {
        
        public string applicationid { get; set; }
        public string? applicationno { get; set; }

        public string? isapproved { get; set; }

        public string? isapprovestatus { get; set; }

        public string serviceid { get; set; }
        public string[]? applicationidlist { get; set; }
        public string subserviceid { get; set; }
        public bool isIndividual { get; set; }


    }
}
