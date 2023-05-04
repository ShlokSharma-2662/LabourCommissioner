using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class EmpApplicationDetailsModel : BankDetails
    {
        public long srno { get; set; }
        public long totalpagecount { get; set; }
        public long applicationid { get; set; }
        public long pendingdays { get; set; }
        public string? applicationno { get; set; }
        public string? name { get; set; }
        public string? nameingujarati { get; set; }
        public string? fatherorhusbandname { get; set; }


        public DateTime? dateofbirth { get; set; }
        public string? mobileno { get; set; }
        public string? phoneno { get; set; }
        public string? emailid { get; set; }
        public string? castename { get; set; }
        public string? gender { get; set; }
        public string? caddressinguj { get; set; }
        public string? caddressineng { get; set; }
        public string? cDistrictName { get; set; }
        public string? ctalukaname { get; set; }
        public string? cvillagename { get; set; }
        public long? cpincode { get; set; }
        public long? cdistrictid { get; set; }
        public long? ctalukaid { get; set; }
        public long? cvillageid { get; set; }
        public string? paddressineng { get; set; }
        public string? pDistrictName { get; set; }
        public string? ptalukaname { get; set; }
        public string? pvillagename { get; set; }
        public long? ppincode { get; set; }
        public long? pdistrictid { get; set; }
        public long? ptalukaid { get; set; }
        public long? pvillageid { get; set; }

        public DateTime? applicationdate { get; set; }
        public DateTime? submitteddate { get; set; }
        public int isapproved { get; set; }
        public int? levelno { get; set; }
        public long? emplevelno { get; set; }
        public string? isapprovestatus { get; set; }
        public DateTime? approveddate { get; set; }
        public int serviceid { get; set; }
        public string? servicename { get; set; }
        public string? age { get; set; }

        //public string? bankname { get; set; }
        //public string? bankbranch { get; set; }
        //public string? ifsccode { get; set; }

        public string? aadharcardno { get; set; }

        public string? servicenamegujarati { get; set; }



        //public string? districtname { get; set; }
        //public string? talukaname { get; set; }
        public string? rolename { get; set; }
        //public string? villagename { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? todate { get; set; }



        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fromdate { get; set; }



        //public string? pdistrictname { get; set; }

        int ToRole { get; set; }
        string Remarks { get; set; }


        public int documentmasterid { get; set; }
        public string? documentname { get; set; }
        public string? documentnameguj { get; set; }
        public bool iscompulsary { get; set; }
        public string? filename { get; set; }
        public string? couchdbdocid { get; set; }

        public string? applicantname { get; set; }
        public string? applicantnameinguj { get; set; }

        public int? isLastLevel { get; set; }


        //RESPONSE
        public long Error { get; set; }
        public string? Msg { get; set; }
        public string? email { get; set; }
        public int ApplicationName { get; set; }

        //public long? ApplicationNo { get; set; }
        public int res { get; set; }
        public long Id { get; set; }
        //public string ApplicantName { get; set; }
        public long registrationId { get; set; }

        //View History

        public long approvalid { get; set; }
        public int fromroleid { get; set; }
        public int toroleid { get; set; }
        public string fromlevelname { get; set; }
        public string tolevelname { get; set; }
        public long createdbypostid { get; set; }
        public long topostid { get; set; }
        public string applicationstatus { get; set; }
        public string appstatus { get; set; }
        public string remarks { get; set; }
        public string? districtname { get; set; }
        public IList<SelectListItem> applicationlist { get; set; }
        public decimal? totalsahay { get; set; }
        public decimal? prasutisahay { get; set; }
        public decimal? betisahay { get; set; }
        public string? enirmancardno { get; set; }
        public decimal? schoolbenifits { get; set; }
        public decimal? hostelbenifits { get; set; }
        public decimal? booksbenifits { get; set; }
        public long? aadeshid { get; set; }
        public string? aadeshno { get; set; }
        public DateTime? aadeshcreateddate { get; set; }
        public string? financialyear { get; set; }
        public int sendforpaymentstatus { get; set; }
        public DateTime? sendforpaymentdate { get; set; }

        public int? alreadyapplied { get; set; }
        public int? diffdays { get; set; }
        public string? stralreadyapplied { get; set; }
        public DateTime? psdateofbirth { get; set; }
        public Microsoft.AspNetCore.Mvc.ContentResult? aadeshreport { get; set; }

        public FormFileWrapper IdImage { get; set; }

        public string approvalremarks { get; set; }
        public DateTime? transactiondate { get; set; }
        public DateTime? confirmuploadeddate { get; set; }

        //GLWB_TabibiSahay_Approval Flow for Hospital

        public int totalemployeesforcheckup { get; set; }
        public string? companyname { get; set; }
        //public string? emailid { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? district { get; set; }
        public string? taluka { get; set; }
        public long pincode { get; set; }
        public string? hrManagerMobile { get; set; }
        public string? hrmanageremail { get; set; }
        public string? childdateofbirth{ get; set; }
        public int? noofchildemale { get; set; }
        public int? noofchildfemale { get; set; }
        public string? billdate { get; set; }
        public string? billno { get; set; }
        public string? marriagedate { get; set; }
        
        public string? hospitalname { get; set; }
        public string? hospitalemailid { get; set; }
        public string? hospitaladdress { get; set; }
        public string? hospitalmobile { get; set; }
        public int hospitalpincode { get; set; }
        public int finaltotalsahay { get; set; }
        public string? tharavname { get; set; }
        public string? strdegree { get; set; }
        public string? course { get; set; }
        public string? childgender { get; set; }
        public string? organizationname { get; set; }
        public string? lwbaccountno { get; set; }

    }
    public class FormFileWrapper
    {

        //[DataType(DataType.Upload)]
        //[MaxFileSize(5 * 1024 * 1024, ErrorMessage = "This photo extension is not allowed!")]
        //[AllowedExtensions(new string[] { ".jpg", ".png" }, ErrorMessage = "This photo extension is not allowed!")]

        //[Required(ErrorMessage = "Please select a file.")]
        public IFormFile File { get; set; }
    }

}
