using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class DistrictMaster
    {



        public long postid { get; set; }
        public long districtid { get; set; }
        public int districtcode { get; set; }
        public int lgdistrictcode { get; set; }
        public int fwdistrictid { get; set; }
        public int isheadoffice { get; set; }
        public int iscorporation { get; set; }
        public bool isactive { get; set; }
        public int displayseq { get; set; }
        public string? districtname { get; set; }
        public string? action { get; set; }
        public string? ipaddress { get; set; }
        public string? districtnameguj { get; set; }
        public string? districtheadofficename { get; set; }
        public string? districtheadofficenameguj { get; set; }
        public string? hostname { get; set; }

        public DateTime? createddate { get; set; }

        public DateTime? modifieddate { get; set; }

        public string dateofbirth1 { get; set; }
        public string registrationname { get; set; }    
        public long createdby { get; set; }
        public long modifiedby { get; set; }
        public bool isdeleted { get; set; }

       

             

    }
}
