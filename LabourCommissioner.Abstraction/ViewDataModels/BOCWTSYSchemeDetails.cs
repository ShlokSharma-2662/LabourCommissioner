using LabourCommissioner.Abstraction.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class BOCWTSYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }

        [Required(ErrorMessage ="અભ્યાસ ની વિગત લખો")]
        public string studydetails { get; set; }

        [Required(ErrorMessage = "વર્ષ અને સેમેસ્ટર લખો")]
        public string Yearofs { get; set; }

        [Required(ErrorMessage = "ટેબ્લેટની પહોંચ નંબર લખો")]
        public string tabletno { get; set; }

        [Required(ErrorMessage = "સ્કૂલ અને કોલેજ ના નામ લખો")]
        public string schoolorcollagename { get; set; }



    }

}
