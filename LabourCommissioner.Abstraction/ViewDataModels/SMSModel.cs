using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public class SMSModel 
    {
        public string? SmsContent { get; set; }
        public string? MobileNo { get; set; }
        public string? TemplateId { get; set; }
    }
}
