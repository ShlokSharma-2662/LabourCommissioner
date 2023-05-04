using LabourCommissioner.Abstraction.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class BOCWTBSYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public int ApplicationId { get; set; }
        public long ClaimApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        [Required(ErrorMessage = "ટેસ્ટના માન (૧૭ પ્રકરના ટેસ્ટ )લખો")]
        [StringLength(100, ErrorMessage = "Maximum 100 Characters Allowed")]
        [RegularExpression(@"[A-Za-z ]+", ErrorMessage = "Allows only alphabates and spaces")]
        public string? testname { get; set; }

        [Required(ErrorMessage = "રોગ/બીમારી જો હોઈ તો તેમની વિગત")]
        public int testno { get; set; }
        [Required(ErrorMessage = "રોગ/બીમારી ની વિગત લખો.")]
        public string testdetails { get; set; }

        //[Required(ErrorMessage = "મરણ નું પ્રમાણપત્ર નંબર લખો.")]
        //public string? deathcertino { get; set; }

        //[Phone]
        //[Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(10)]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "મોબાઇલ નંબર બરાબર નથી.")]
        //public string? mobileno { get; set; }
        //[Required(ErrorMessage = "ઈમેલ આઈડી નાખો")]
        //[RegularExpression("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "ઈ-મેઈલ આઈડી બરાબર નથી.")]

        //public string? email { get; set; }
        [Required(ErrorMessage = "હોસ્પિટલનું નામ લખો.")]
        public string? hospitalname { get; set; }

        [Required(ErrorMessage = " હોસ્પિટલનો ઈમેલ લખો.")]
        public string? hospitalemailid { get; set; }

        [Required(ErrorMessage = "હોસ્પિટલનું સરનામું લખો.")]
        public string? hospitaladdress { get; set; }

        [Required(ErrorMessage = " હોસ્પિટલનો મોબાઇલ નંબર લખો.")]
        public string? hospitalmobile { get; set; }

        [Required(ErrorMessage = "હોસ્પિટલનો પીનકોડ લખો.")]
        public int hospitalpincode { get; set; }
        public int totalsahay { get; set; }
        public string? fromdate { get; set; }
        public string? todate { get; set; }
        public TimeSpan? fromtime { get; set; }
        public TimeSpan? totime { get; set; }


    }
   
}
