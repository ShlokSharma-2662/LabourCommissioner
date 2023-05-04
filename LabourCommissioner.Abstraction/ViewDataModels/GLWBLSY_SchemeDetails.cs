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
    public class GLWBLSY_SchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }

        [Required(ErrorMessage = " લેબર વેલ્ફેર ફંડ એકાઉન્ટ નંબર નાખો")]
        public string? ENirmanCardNo { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        [Required(ErrorMessage = "સહાયની વિગત પસંદ કરો")]
        public string Schmename { get; set; }

        [Required(ErrorMessage = "પરીક્ષા નો વરાશ પસંદ કરો ")]
        public string? ExamYear { get; set; }

        [Required(ErrorMessage = "પરીક્ષા નો મહિનો પસંદ કરો ")]
        public string? ExamMonth { get; set; }

        [Required(ErrorMessage = "વિધ્યાર્થી નો આધાર કાર્ડ નાખો ")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "ફક્ત નંબર અને ૧૨ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        public string? StudentAadharCardNo { get; set; }
        [Required]
        public string? Standard { get; set; }

        [Required(ErrorMessage = "પરીક્ષા માં આવેલ પર્સેંટેજ નાખો")]
        [Range(70, 100, ErrorMessage = "આ યોજના માત્ર 70 થી વધુ ટકાવારી ધરાવતા વિદ્યાર્થીઓ માટે છે")]
        //[Range(70, 100, ErrorMessage = "This scheme is only for students with percentage more than 70")]
        public decimal Percentage { get; set; }
        [Required(ErrorMessage = "પ્રોફેશનલ/ડડઝાઈનીંગ પસંદ કરો")]
        public long courseid { get; set; }

        [Required(ErrorMessage = "અભ્યાસક્રમનું નામ નાખો")]
        public string? coursename { get; set; }

        [Required(ErrorMessage = "લેપટોપ ખરીદ તારીખ નાખો ")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? buydate { get; set; }

        [Required(ErrorMessage = "લેપટોપ બીલ નંબરનાખો ")]
        public string? billno { get; set; }

        [Required(ErrorMessage = "લેપટોપની કિંમત નાખો ")]
        public long laptopamount { get; set; }
        [Required(ErrorMessage = "લેપટોપ સીરીયલ નંબર")]
        public string? laptopserialno { get; set; }

        [Required(ErrorMessage = "લેપટોપની કુંપનીનું નામ")]
        public string? laptopbrand { get; set; }

        [Required(ErrorMessage = "યુનિવર્સિટીનું નામ")]
        public string? university { get; set; }

        public DateTime SubmittedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string Benifitsrs { get; set; }
        public string buydates { get; set; }
        public int totalsahay { get; set; }
    }
}
