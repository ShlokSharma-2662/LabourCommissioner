using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.ViewDataModels
{
    public class CompanyWorkerDetails
    {
        public long workerdetailsid { get; set; }

        [Required(ErrorMessage = "કૃપા કરીને કોઈપણ કામદારોની વિગતો પસંદ કરો.")]
        public long registrationid { get; set; }
        //public string removeList { get; set; }

        //[Required(ErrorMessage = "શ્રમયોગીનું નામ નાખો.  ")]
        public string? workername { get; set; }

        //[Required(ErrorMessage = "મોબાઇલ નંબર લખો."), MaxLength(10)]
        //public string MobileNo { get; set; }

        //[Required(ErrorMessage = "ઉંમર નાખો. ")]
        public string? ageyear { get; set; }

        //[Required(ErrorMessage = "લિંગ નાખો.  ")]
        public string? gender { get; set; }

        //[Required(ErrorMessage = "જન્મ તારીખ પસંદ કરો.")]        
        [ModelBinder(BinderType = typeof(CustomDateTimeModelBinder))]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        public string dateofbirth1 { get; set; }
        public string registrationname { get; set; }

        //[Required(ErrorMessage = "આધાર કાર્ડ નંબર  લખો.")]
        //[RegularExpression(@"^[0-9]{12}$", ErrorMessage = "ફક્ત નંબર અને ૧૨ આંકડા સુધી જ સ્વીકાર્ય છે.")]
        //public string AadharCardNo { get; set; }        
        public long CreatedBy { get; set; }
        public bool isDeleted { get; set; }

        public int aboveagemale { get; set; }
        public int belowagemale { get; set; }
        public int aboveagefemale { get; set; }
        public int belowagefemale { get; set; }


        public string? sanmanregistrationno { get; set; }

        public bool ischeckeup { get; set; }

    }
}
