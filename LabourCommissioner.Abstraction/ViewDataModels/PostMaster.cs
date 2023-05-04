using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class PostMaster
    {

        public long postid { get; set; }
        public long SrNo { get; set; }
        public long districtid { get; set; }
        public string districtname { get; set; }
        public long talukaid { get; set; }
        public string talukaname { get; set; }
        public string postname { get; set; }
        public string username { get; set; }
        public string emailid { get; set; }
        public string password { get; set; }
        public string contactno { get; set; }
        public bool isactive { get; set; }
        public string roleid { get; set; }
        public bool isurban { get; set; }
        public string rolename { get; set; }
        public long officeid { get; set; }

    }
}
