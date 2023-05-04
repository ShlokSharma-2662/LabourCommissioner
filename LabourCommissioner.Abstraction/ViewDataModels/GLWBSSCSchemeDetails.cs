using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.DataModels;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class GLWBSSCSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public string? ENirmanCardNo { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; } 
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        //[Required(ErrorMessage = "સહાયની વિગત પસંદ કરો")]
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
        public decimal Percentage { get; set; }
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
        public int totalsahay { get; set; }
    }
}
