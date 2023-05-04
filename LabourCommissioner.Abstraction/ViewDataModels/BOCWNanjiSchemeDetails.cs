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
    public class BOCWNanjiSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }       
        public int ApplicationId { get; set; }
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

        //[Required(ErrorMessage = "કુટુંબ ના કુલ સભ્યો લખો.")]
        public int familymemberno { get; set; }

        [Required(ErrorMessage = "મકાન ફાળવનાર સંસ્થાનુ નામ લખો.")]
        public string schemeorg { get; set; }

        [Required(ErrorMessage = "સ્કીમ પસંદ કરો.")]
        public int housingscheme { get; set; }

        [Required(ErrorMessage = "પોતાની માલિકીનું મકાન/ફ્લેટ છે કે કેમ તે પસંદ કરો.")]
        public int houseowner { get; set; }

        [Required(ErrorMessage = "મકાન/ફ્લેટ ની વિગત લખો")]
        public string housedetails { get; set; }

        [Required(ErrorMessage = "અન્ય સભ્યના નામે મકાન/ફ્લેટ છે તે પસંદ કરો.")]
        public int otherhouse { get; set; }

        [Required(ErrorMessage = "અન્ય સભ્યના નામે મકાન/ફ્લેટ ની વિગત લખો")]
        public string otherhousedetails { get; set; }

        [Required(ErrorMessage = "ફાળવેલ મકાન પર અન્ય કોઈ લોન લીધેલ છે તે પસંદ કરો.")]
        public int houseloan { get; set; }

        [Required(ErrorMessage = "લોનની વિગત લખો")]
        public string loandetails { get; set; }

        [Required(ErrorMessage = "મકાન ફાળવનારની તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime housegetdate { get; set; }


        [Required(ErrorMessage = "જીલ્લા કચેરીમાં અરજી કર્યાની તારીખ લખો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        

        public DateTime districtapplydate { get; set; }


        [Required(ErrorMessage = "અન્ય કોઈ સરકારી /સ્થાનિક સ્વરાજયની સંસ્થાની આવાસ યોજના હેઠળ લાભ મેળવેલ છે કે કેમ? તે પસંદ કરો.")]
        public int otherschemuse { get; set; }

        [Required(ErrorMessage = "યોજનાનું નામ લખો.")]
        public string otherschemename { get; set; }

        [Required(ErrorMessage = "મકાન ફાળવણી થયા બાદ કેટલા નાણાં ભરેલ છે. તે નાખો.")]
        public int money { get; set; }


        [Required(ErrorMessage = "મકાન ફાળવણી થયા બાદ કેટલા હપ્તા ભરેલ છે. તે નાખો.")]
        public int instalments { get; set; }

        [Required(ErrorMessage = "સભ્યો ના નામ લખો.")]
        public string familymembersname { get; set; }
        public int totalsahay { get; set; }

        public List<NanajiFamilyMemberDetails> lstFamilyMemberDetails { get; set; }

        public long fg_familydetailsid { get; set; }

        public string? fg_familymembersname { get; set; }
        public string? fg_relation { get; set; }
        public string housegetdates { get; set; }
        public string districtapplydates { get; set; }
        
        


    }
   
}
