using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class HospitalScheduleTime
    {
        
        public long ApplicationId { get; set; }
        public string? ApplicationNo { get; set; }

        public string? isapproved { get; set; }

        public string? isapprovestatus { get; set; }

        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ServiceId { get; set; }


        [Required(ErrorMessage = "FromDate પસંદ કરો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime fromdate { get; set; }

        [Required(ErrorMessage = "ToDate પસંદ કરો.")]
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime todate { get; set; }

        [Required(ErrorMessage = "FromTime પસંદ કરો.")]
        public TimeSpan fromtime { get; set; }


        [Required(ErrorMessage = "ToTime પસંદ કરો.")]
        public TimeSpan totime { get; set; }
        public bool IsDeleted { get; set; }
        public string? Remarks { get; set; }
        //public bool IsDeleted { get; set; }

        public string CouchDBDocId { get; set; }
        public string DocumentName { get; set; }
        public string CouchDBDocRevId { get; set; }


    }
}
