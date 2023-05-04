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
    public class BOCWVRSchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public string? ENirmanCardNo { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "પસંદ કરો")]
        public long diseasestype { get; set; } 
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }


        [Required(ErrorMessage = "વ્યવસાયિક રોગ/ બીમારી નુ નામ લખો.")]
        public string diseasename { get; set; }

        [Required(ErrorMessage = "કેટલા સમયથી રોગ/બીમારી છે તે લખો")]
        public string diseaseduration { get; set; }

        [Required(ErrorMessage = "સરકાર દ્વારા એમ્પેનલ થયેલ હોસ્પિટલ નુ નામ લખો")]
        public string hospitalname { get; set; }

        [Required(ErrorMessage = "ડૉક્ટરનુ નામ લખો.")]
        public string doctorname { get; set; }

        [Required(ErrorMessage = "અશક્તતા ની ટકાવારી(%) લખો.")]
        public long strength { get; set; }

        [Required(ErrorMessage = "આરામ કરવાનો સમયગાળો લખો.")]
        public long restduration { get; set; }

        [Required(ErrorMessage = "અત્યાર સુધીમાં ચૂકવેલ સહાય લખો.")]
        public long assistancepaid { get; set; }
        public long istakeanyotherassistance { get; set; }
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
        public string strdiseasestype { get; set; }

        [Required(ErrorMessage = "કુલ મળવા પાત્ર સહાય (રૂપિયામાં) નાખો")]
        public long totalsahay { get; set; }
        public int diffdays { get; set; }

        public List<BOCWVR_OtherSchemeDetails> lstOtherSchemeDetails { get; set; }

        public string? fg_otherschemename { get; set; }
        public string? fg_otherschemesahay { get; set; }
        public string  fg_restschemename { get; set; }
        public string  fg_restschemesahay { get; set; }


    }
}
