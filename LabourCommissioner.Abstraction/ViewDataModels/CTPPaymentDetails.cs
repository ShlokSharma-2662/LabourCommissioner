using LabourCommissioner.Abstraction.ViewDataModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.DataModels
{
    public partial class CTPPaymentDetails : BaseDataTableEntity
    {

        public long paymentinfotransid { get; set; }
        public long registrationid { get; set; }
        public DateTime? paymentinitdate { get; set; }
        public string transactionid { get; set; }
        public string taxtype { get; set; }
        public string merchantid { get; set; }
        public string registrationno { get; set; }
        public string name { get; set; }
        public string emailid { get; set; }
        public string phoneno { get; set; }
        public long tokenno { get; set; }
        public long totalamount { get; set; }
        public DateTime? taxperiodfrom { get; set; }
        public DateTime? taxperiodto { get; set; }
        public long createdby { get; set; }
        public DateTime? createddate { get; set; }
        public long modifiedby { get; set; }
        public DateTime? modifieddate { get; set; }
        public bool isdeleted { get; set; }
        public long deletedby { get; set; }
        public DateTime? deleteddate { get; set; }
        public string ipaddress { get; set; }
        public string hostname { get; set; }
        
    }
    
}
