using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Abstraction.Repositories
{
    public interface ICCApplicationRepository : IBaseRepository<CCApplicationDetails>
    {
        Task<IEnumerable<CCApplicationDetails>> GetCCApplicationDetails(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate, int statusId, string? search);
        Task<IEnumerable<CCApplicationDetails>> GetCCCompletedAppForPayment(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, int statusId, string? search);
        Task<List<SelectListItem>> GetAllStates();
        Task<IEnumerable<SelectListItem>> GetDistrict();
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId);
        Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId);
        Task<ResponseMessage> AddUpdateCCApplication(CCApplicationDetails ccApplicationDetails);
        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
        Task<CCApplicationDetails> GetTotalsahayByServiceID(int serviceId);
        Task<CCApplicationDetails> GetApplicationDetailsByAppId(long applicationId);
        Task<IEnumerable<CTPPaymentDetails>> InsertPaymentTransactionInfo(DataTable dtApplicationIds, long registrationId, DateTime? fromDate, DateTime? toDate, string ipAddress, string hostName);
        Task<IEnumerable<CTPPaymentDetails>> CheckTransactionTokenExistorNot(long registrationId, long tokenNo, string transactionId);
        Task<IEnumerable<CTPPaymentDetails>> UpdatePaymentTransactionInfo(long userId, long transactionId, string regno, string bankrefno, string bankname, long dlrrefno, string cin, string amount, DateTime? paymentdate, string status, string statusdesc);
        Task<IEnumerable<CTPPaymentDetails>> GetDataForCTPMakePayment(long paymentinfoTransId);
    }
}
