using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.DataModels;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class ExceptionLogger
    {
        public long UserId { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? ExceptionMessage { get; set; } 
        public string? ExceptionStackTrace { get; set; }
       
        public string? RequestURI { get; set; }
        public long LineNumber { get; set; }
        public long CreatedBy { get; set; }
        public int LogFrom { get; set; }
        public string? IpAddress { get; set; }
        public string? HostName { get; set; }
    }
}
