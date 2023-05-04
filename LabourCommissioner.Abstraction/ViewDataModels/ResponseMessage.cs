using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Common
{
    public class ResponseMessage
    {
        public long Error { get; set; }
        public string? Msg { get; set; }
        public string? email { get; set; }
        public int ApplicationName { get; set; }
        public long? ApplicationNo { get; set; }
        public int res { get; set; }
        public long Id { get; set; }
        public string ApplicantName { get; set; }
        public long registrationId { get; set; }
        public long ErrorCode { get; set; }
        public DateTime? iCardToDate { get; set; }
        public long applicationcount { get; set; }




    }
}
