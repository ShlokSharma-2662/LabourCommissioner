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
    public class CompletedApplicationDetailsModel
    {
        public long srno { get; set; }
        public string? applicationno { get; set; }
        public string? name { get; set; }
        public DateTime? dateofbirth { get; set; }
        public DateTime? applicationdate { get; set; }
        public DateTime? submitteddate { get; set; }
        public string? mobileno { get; set; }
        public string? castename { get; set; }
        public string? districtname { get; set; }
        public string? talukaname { get; set; }
        public string? villagename { get; set; }
        public string? servicename { get; set; }
        public string? appstatus { get; set; }
        public string? hodname { get; set; }
    }
}
