using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class APIResponse
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public string StackTrace { get; set; }
    }
    public class BOCWRegAPIResult
    {
        public string ApplicationNo { get; set; }
        public string Name { get; set; }
        public string FirstCardIssuedDate { get; set; }
        public int CDistrictId { get; set; }
        public string ICardFromDate { get; set; }
        public string ICardToDate { get; set; }
        public int Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int RegistrationId { get; set; }
        public string UserName { get; set; }
    }

    public class RootBOCWRegAPIResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<BOCWRegAPIResult> Result { get; set; }
    }
}
