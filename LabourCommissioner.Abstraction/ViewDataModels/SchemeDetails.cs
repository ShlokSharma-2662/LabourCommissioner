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
    public class SchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public string? ENirmanCardNo { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; } 
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        [Required(ErrorMessage = "સહાયની વિગત પસંદ કરો")]
        public string Schmename { get; set; }

        [Required(ErrorMessage = "અભ્યાસક્રમ પસંદ કરો")]
        public int Syllabus { get; set; }

        public string syllabusname { get; set; }

        [Required(ErrorMessage = "ડીગ્રી/ધોરણ પસંદ કરો")]
        public string Course { get; set; }
        //public string Degree { get; set; }

        [Required(ErrorMessage = "અભ્યાસનું વર્ષ/સેમ પસંદ કરો")]
        public string AcadmicYearSem { get; set; }

        [Required(ErrorMessage = "એડમીશન મળ્યા/સત્ર શરુ થયા તારીખ પસંદ કરો.")]        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        
        
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "શાળા/કોલેજનું નામ લખો.")]
        public string SchoolCollageName { get; set; }
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

        [Required(ErrorMessage = "સહાય (રૂપિયામાં) નાખો")]
        public long benifitsrs { get; set; }

        [Required(ErrorMessage = "હોસ્ટેલ સહાય (રૂપિયામાં) નાખો")]
        public long hostelbenifits { get; set; }

        [Required(ErrorMessage = "પુસ્તક સહાય (રૂપિયામાં) નાખો")]
        public long booksbenifits { get; set; }

        [Required(ErrorMessage = "સ્કૂલ ફી (રૂપિયામાં) નાખો")]
        public long ubenifitsrs { get; set; }

        [Required(ErrorMessage = " હોસ્ટેલ ફી (રૂપિયામાં) નાખો")]
        public long uhostelbenifits { get; set; }

        [Required(ErrorMessage = "પુસ્તક સહાય (રૂપિયામાં) નાખો")]
        public long ubooksbenifits { get; set; }

        [Required(ErrorMessage = "મળવા પાત્ર (રૂપિયામાં) નાખો")]
        public long fbenifitsrs { get; set; }

        [Required(ErrorMessage = "મળવા પાત્ર હોસ્ટેલ સહાય (રૂપિયામાં) નાખો")]
        public long fhostelbenifits { get; set; }

        [Required(ErrorMessage = "મળવા પાત્ર પુસ્તક સહાય (રૂપિયામાં) નાખો")]
        public long fbooksbenifits { get; set; }

        [Required(ErrorMessage = "કુલ મળવા પાત્ર સહાય (રૂપિયામાં) નાખો")]
        public long totalsahay { get; set; }
        public string StartDates { get; set; }
    }
}
