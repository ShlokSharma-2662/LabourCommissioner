using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ModelBinderAttribute = Microsoft.AspNetCore.Mvc.ModelBinderAttribute;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class BOCWHssySchemeDetails : BankDetails
    {
        
        public int SchemeId { get; set; }
        public int familydetailsid { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }
        public int familymemberno { get; set; }

        public List<familymember> lstfamilymemberDetails { get; set; }
        public List<StudentMemberDetails> lstStudentMemberDetails { get; set; }
       
        public List<StudentHostelMemberDetails> lstStudentHostelMemberDetails { get; set; }

        public int famillytotalno { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }

        public string fg_name { get; set; }
        public string fg_age { get; set; }
        public string fg_stdorcourse { get; set; }
        public string fg_schoolcollagename { get; set; }
        public string fg_famillymembernames { get; set; }
        public string fg_sname { get; set; }
        public int fg_isstayhostel { get; set; }
        public string fg_hostelname { get; set; }
        public string fg_hosteladdress { get; set; }
        public string fg_hosteltype { get; set; }

    }
}
