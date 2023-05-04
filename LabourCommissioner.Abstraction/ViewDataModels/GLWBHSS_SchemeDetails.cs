using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.DataModels;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class GLWBHSS_SchemeDetails : BankDetails
    {
        public long SchemeId { get; set; }
        [Required(ErrorMessage = " લેબર વેલ્ફેર ફંડ એકાઉન્ટ નંબર નાખો ")]
        public string? ENirmanCardNo { get; set; }
        public long ApplicationId { get; set; }
        public long ServiceId { get; set; }
        public long UserId { get; set; }
        public long TabSequenceNo { get; set; }

        [Required(ErrorMessage = "સહાયની વિગત પસંદ કરો")]
        public string Schmename { get; set; }

        [Required(ErrorMessage = "પરીક્ષા નો વરાશ પસંદ કરો ")]
        public string? ExamYear12th { get; set; }

        [Required(ErrorMessage = "પરીક્ષા નો મહિનો પસંદ કરો ")]
        public string? ExamMonth12th { get; set; }


        [Required(ErrorMessage = "પરીક્ષા નો વરાશ પસંદ કરો ")]
        public string? ExamYearEntrance { get; set; }

        [Required(ErrorMessage = "પરીક્ષા નો મહિનો પસંદ કરો ")]
        public string? ExamMonthEntrance { get; set; }


        [Required(ErrorMessage = "વિધ્યાર્થી નો આધાર કાર્ડ નાખો ")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "ફક્ત નંબર અને ૧૨ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        public string? StudentAadharCardNo { get; set; }
        [Required]
        public string? Standard { get; set; }

        [Required(ErrorMessage = "પરીક્ષા માં આવેલ પર્સેંટેજ નાખો")]
        [Range(70, 100, ErrorMessage = "આ યોજના નો માત્ર ૭૦ ક એના થી વધારે પર્સેંટેજ / પર્સન્ટાઇલ મેળવેલ વિદ્યાર્થી જ લાભ લે સકે છે.")]
        public decimal Percentage { get; set; }

        [Required(ErrorMessage = "કોર્સ પસંદ કરો.")]
        public long courseid { get; set; }


        public DateTime SubmittedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string Benifitsrs { get; set; }
        public long totalsahay { get; set; }


    }
}
