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
    public class AadeshPaymentDetailsModel
    {
        public long srno { get; set; }
        public long payinfoid { get; set; }
        public long serviceid { get; set; }
        public long applicationid { get; set; }
        public long aadeshid { get; set; }
        public string? uniqueid { get; set; }
        //public Guid? uniqueid { get; set; }
        public string? applicantname { get; set; }
        public string? districtname { get; set; }
        public string? fwdistrictid { get; set; }

        public string? filename { get; set; }
        public string? applicationno { get; set; }
        public string? beneficiaryaccountno { get; set; }
        public string? ifsccode { get; set; }
        public decimal? amount { get; set; }
        public string? accounttype { get; set; }
        public string? schemaname { get; set; }
        public string? boardname { get; set; }
        public string? boardaccountno { get; set; }
        public string? corporateid { get; set; }
    }
}
