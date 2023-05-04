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
    public class VillageMaster
    {
        public long villageid { get; set; }
    public long talukaid { get; set; }
    public long districtid { get; set; }
    public int postid { get; set; }
    public int villagecode { get; set; }
    public string villagename { get; set; }
    public string talukanames { get; set; }
    public string districtnames { get; set; }
    public string villagenameguj { get; set; }
    public string DistrictHeadOfficeName { get; set; }
    public string DistrictHeadOfficeNameGuj { get; set; }
    public bool iscorporation { get; set; }
    public bool IsActive { get; set; }
    public bool isfilled { get; set; }
    public long createdby { get; set; }
    public long modifiedby { get; set; }
    public bool isdeleted { get; set; }
    public string actionid { get; set; }
    public string action { get; set; }
    public string ipaddress { get; set; }
    public string hostname { get; set; }
    public DateTime createddate { get; set; }
    public DateTime modifieddate { get; set; }



    //public long districtid { get; set; }
    //public long postid { get; set; }
    //public int districtcode { get; set; }
    //public int lgdistrictcode { get; set; }
    //public int fwdistrictid { get; set; }

    //public int iscorporation { get; set; }
    public bool isactive { get; set; }
    //public int displayseq { get; set; }
    //public string? districtname { get; set; }
    //public string? action { get; set; }
    //public string? ipaddress { get; set; }
    //public string? districtnameguj { get; set; }
    //public string? districtheadofficename { get; set; }
    //public string? districtheadofficenameguj { get; set; }
    //public string? hostname { get; set; }
    //public string? actionid { get; set; }

    //public DateTime? createddate { get; set; }

    //public DateTime? modifieddate { get; set; }

    //public string dateofbirth1 { get; set; }
    //public string registrationname { get; set; }
    //public long createdby { get; set; }
    //public long modifiedby { get; set; }
    //public bool isdeleted { get; set; }


}
}
