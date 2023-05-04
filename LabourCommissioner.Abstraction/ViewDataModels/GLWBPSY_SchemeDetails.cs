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
    public class GLWBPSY_SchemeDetails : BankDetails
    {
        public int SchemeId { get; set; }
        public int ApplicationId { get; set; }
        public int ServiceId { get; set; }
        public long UserId { get; set; }
        public int TabSequenceNo { get; set; }

        //[Required(ErrorMessage = "પ્રસુતિ ક્રમાંક પસંદ કરો.")]
        public int? prasutino { get; set; }

        [Required(ErrorMessage = "બાળકની જાતિ પસંદ કરો.")]
        public int gender { get; set; }

        [Required(ErrorMessage = "બાળકની જન્મ તારીખ લખો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateofbirth { get; set; }
        public string dateofbirth2 { get; set; }

        //[Range(0, 10, ErrorMessage = "ફક્ત શૂન્ય અને દસ ની વચે આંકડા લખી સકો છો")]
        //[Required(ErrorMessage = "નવા જન્મેલ બાળક સહિત હયાત બાળકની સંખ્યા લખો.")]
        public int? noofchild { get; set; }
        //public int? noofchildFemale { get; set; }

        //[Required(ErrorMessage = "માસિક પગાર પગાર લખો.")]
        public long? monthlysalary { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string IpAddress { get; set; }
        public string HostName { get; set; }
        public string schemaname { get; set; }
        public string tablename { get; set; }
        public int noofchildemale { get; set; }
        public int noofchildfemale { get; set; }
        public decimal totalsahay { get; set; }
        public decimal betisahay { get; set; }
        public decimal prasutisahay { get; set; }
    }
}
