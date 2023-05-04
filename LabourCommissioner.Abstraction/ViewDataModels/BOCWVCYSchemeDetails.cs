using LabourCommissioner.Abstraction.DataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class BOCWVCYSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
       
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        [Required(ErrorMessage = "વિદ્યાર્થીનું નામ લખો.")]
        public string studentname { get; set; }

        [Required(ErrorMessage = "જન્મ તારીખ પસંદ કરો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime? dateofbirth { get; set; }

        [Required(ErrorMessage = "સ્પધાત્મક કોચિંગ વર્ગની વિગત લખો.")]
        public string compcoachingdetails { get; set; }

        [Required(ErrorMessage = "અભ્યાસનું વર્ષ/સેમ લખો.")]
        public string acadmicyearsem { get; set; }

        [Required(ErrorMessage = "એડમીશન મળ્યા/સત્ર શરુ થયા તારીખ પસંદ કરો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]       
        public DateTime? admissionstartdate { get; set; }

        [Required(ErrorMessage = "યુનિવર્સીટી /ઇન્સ્ટીટયુટ ઓફ ચાર્ટર્ડ એકાઉન્ટ ઓફ ઇન્ડિયાની સંસ્થાનું નામ લખો.")]
        public string institutename { get; set; }

        [Required(ErrorMessage = "રજીસ્ટ્રેશન ફી(રૂપિયામાં) નાખો")]
        public int registrationfee { get; set; }

        [Required(ErrorMessage = "કોચિંગ ફી/ટુશન ફી.(રૂપિયામાં) નાખો")]
        public int coachingfee { get; set; }

        [Required(ErrorMessage = "તાલીમ ફી(રૂપિયામાં) નાખો")]
        public int trainingfee { get; set; }

        [Required(ErrorMessage = "પરીક્ષા ફી(રૂપિયામાં) નાખો")]
        public int examfee { get; set; }
        public int totalsahay { get; set; }
        public int identitycardrenewal { get; set; }

        [Required(ErrorMessage = "રિન્યુઅલ તારીખ પસંદ કરો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime? renewaldate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; } 
    }
   
}
