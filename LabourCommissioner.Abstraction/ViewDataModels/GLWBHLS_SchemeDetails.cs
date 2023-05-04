using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class GLWBHLS_SchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        //[Range(1, 35000, ErrorMessage = "માસિક પગાર ૩૫૦૦૦ થી ઓછો હોવો જોઈએ.")]
        //[Required(ErrorMessage = "માસિક પગાર પગાર લખો.")]
        public decimal monthlyincome { get; set; }

        [Required(ErrorMessage = "દસ્તાવેજ મુજબ મકાનની કિંમત લખો.")]
        [Range(1, 3000000, ErrorMessage = "મકાન ની કિમત ૩૦,૦૦,૦૦૦ થી વધારે ના હોવી જોઈએ.")]
        public decimal houseprice { get; set; }

        [Required(ErrorMessage = "દસ્તાવેજ નંબર લખો.")]
        public string doucumentnumber { get; set; }

        [Required(ErrorMessage = "દસ્તાવેજની તારીખ લખો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime doucumentdate { get; set; }
        public string rptdoucumentdate { get; set; }

        [Required(ErrorMessage = "લોનની રકમ લખો.")]
        
        public decimal loanamount { get; set; }

        [Required(ErrorMessage = "લોનની સમય મર્યાદા લખો.")]
        [Range(15, 999, ErrorMessage = "લોનની સમય મર્યાદા ૧૫ વર્ષ થી ઓછો ના હોવી જોઈએ.")]
        public decimal loanduration { get; set; }

        [Required(ErrorMessage = "માસિક હપ્તો લખો.")]
        
        public decimal installmentamount { get; set; }

        [Required(ErrorMessage = "વ્યાજનો દર લખો.")]
        [Range(0, 99.99, ErrorMessage = "વ્યાજનો દર અમાન્ય છે.")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "મહત્તમ 2 દશાંશ સ્થાનો સાથે માન્ય દશાંશ સંખ્યા નાખો.")]
        public decimal intrestrate { get; set; }

        [Required(ErrorMessage = "લોન મંજુર થયા તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime loanapprovaldate { get; set; }
        public string rptloanapprovaldate { get; set; }

        [Required(ErrorMessage = " હોમ લોન એકાઉન્ટ નં લખો.")]
        public string loanaccountno { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public long totalsahay { get; set; }

    }
}
