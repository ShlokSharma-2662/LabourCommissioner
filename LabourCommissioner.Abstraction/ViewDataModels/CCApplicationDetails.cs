using LabourCommissioner.Abstraction.ViewDataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public partial class CCApplicationDetails : BaseDataTableEntity
    {

        public long srno { get; set; }
        public long applicationid { get; set; }
        public string? applicationno { get; set; }
        public long registrationid { get; set; }
        public long serviceid { get; set; }
        [Required(ErrorMessage = "CESS payer fullname is required")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? cesspayername { get; set; }

        [Required(ErrorMessage = "CESS collection date is required")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dateofcollection { get; set; }

        [Required(ErrorMessage = "Cost of Construction is required")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{0,2})?$", ErrorMessage = "Please enter value in proper format")]
        public decimal costofconstruction { get; set; }

        [Required(ErrorMessage = "CESS percentage is required")]
        public decimal cesspercentage { get; set; }

        [Required(ErrorMessage = "Total CESS is required")]
        public decimal totalcess { get; set; }

        [Required(ErrorMessage = "Please select state")]
        public long stateid { get; set; }
        [Required(ErrorMessage = "Please select district")]
        public long districtid { get; set; }
        [Required(ErrorMessage = "Please select taluka")]
        public long talukaid { get; set; }
        [Required(ErrorMessage = "Please select village")]
        public long villageid { get; set; }

        [Required(ErrorMessage = "Pincode  is required")]
        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "Please enter valid pincode")]
        public long pincode { get; set; }

        [Required(ErrorMessage = "Please enter address in english")]
        [StringLength(100, ErrorMessage = "Maximum 500 Characters Allowed")]
        public string? addressineng { get; set; }
        [Required(ErrorMessage = "Please enter address in gujarati")]
        [StringLength(100, ErrorMessage = "Maximum 500 Characters Allowed")]
        public string? addressinguj { get; set; }
        public string? issubmitted { get; set; }
        public string? submitteddate { get; set; }
        public string? createddate { get; set; }
        public string? DistrictName { get; set; }
        public string? TalukaName { get; set; }
        public string? VillageName { get; set; }
        public string? ipaddress { get; set; }
        public string? hostname { get; set; }
        public int applicationfrom { get; set; }
        public decimal totalsahay { get; set; }
        public bool isfilled { get; set; }
        public bool? isinitforpayment { get; set; }




        public long? paymentid { get; set; }
        public long? paymentinfotransid { get; set; }
        public DateTime? paymentinitdate { get; set; }
        public string? transactionid { get; set; }
        public string? taxtype { get; set; }
        public string? merchantid { get; set; }
        public string? registrationno { get; set; }
        public string? name { get; set; }
        public string? emailid { get; set; }
        public string? phoneno { get; set; }
        public long? tokenno { get; set; }
        public decimal? totalamount { get; set; }
        public DateTime? taxperiodfrom { get; set; }
        public DateTime? taxperiodto { get; set; }
        public long? resuserid { get; set; }
        public long? restransactionid { get; set; }
        public string? resregno { get; set; }
        public string? bankreferenceno { get; set; }
        public string? bankname { get; set; }
        public string? dlrreferenceno { get; set; }
        public string? cin { get; set; }
        public decimal? resamount { get; set; }
        public DateTime? paymentdate { get; set; }
        public string? resstatus { get; set; }
        public string? resstatusdesc { get; set; }
    }
    
}
