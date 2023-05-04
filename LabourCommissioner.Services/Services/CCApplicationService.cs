using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Services.Services
{
    public class CCApplicationService : ICCApplicationService
    {
        private readonly ICCApplicationRepository _ccapplicationRepository;

        public CCApplicationService(ICCApplicationRepository ccapplicationRepository)
        {
            _ccapplicationRepository = ccapplicationRepository ?? throw new ArgumentNullException(nameof(ccapplicationRepository));
        }
        public async Task<IEnumerable<CCApplicationDetails>> GetCCApplicationDetails(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate, int statusId, string? search)
        {
            var res = await _ccapplicationRepository.GetCCApplicationDetails(pageNo,pageSize, districtId, talukaId, villageId, fromDate,toDate, statusId, search);
            return res;
        }
        public async Task<IEnumerable<CCApplicationDetails>> GetCCCompletedAppForPayment(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, int statusId, string? search)
        {
            var res = await _ccapplicationRepository.GetCCCompletedAppForPayment(pageNo, pageSize, fromDate, toDate, statusId, search);
            return res;
        }
        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _ccapplicationRepository.GetAllStates();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _ccapplicationRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _ccapplicationRepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _ccapplicationRepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<ResponseMessage> AddUpdateCCApplication(CCApplicationDetails ccApplicationDetails)
        {
            return await _ccapplicationRepository.AddUpdateCCApplication(ccApplicationDetails);
        }

        public async Task<CCApplicationDetails> GetTotalsahayByServiceID(int serviceId)
        {
            var res = await _ccapplicationRepository.GetTotalsahayByServiceID(serviceId);
            return res;
        }
        public async Task<CCApplicationDetails> GetApplicationDetailsByAppId(long applicationId)
        {
            var res = await _ccapplicationRepository.GetApplicationDetailsByAppId(applicationId);
            return res;
        }
        public async Task<IEnumerable<CTPPaymentDetails>> InsertPaymentTransactionInfo(DataTable dtApplicationIds, long registrationId, DateTime? fromDate, DateTime? toDate, string ipAddress, string hostName)
        {
            var res = await _ccapplicationRepository.InsertPaymentTransactionInfo(dtApplicationIds, registrationId, fromDate, toDate, ipAddress, hostName);
            return res;
        }
        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _ccapplicationRepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _ccapplicationRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }
        public async Task<IEnumerable<CTPPaymentDetails>> CheckTransactionTokenExistorNot(long registrationId, long tokenNo, string transactionId)
        {
            var res = await _ccapplicationRepository.CheckTransactionTokenExistorNot(registrationId, tokenNo, transactionId);
            return res;
        }
        public async Task<IEnumerable<CTPPaymentDetails>> UpdatePaymentTransactionInfo(long userId, long transactionId, string regno, string bankrefno, string bankname, long dlrrefno, string cin, string amount, DateTime? paymentdate, string status, string statusdesc)
        {
            var res = await _ccapplicationRepository.UpdatePaymentTransactionInfo(userId, transactionId, regno, bankrefno, bankname, dlrrefno, cin, amount, paymentdate, status, statusdesc);
            return res;
        }
        public async Task<IEnumerable<CTPPaymentDetails>> GetDataForCTPMakePayment(long paymentinfoTransId)
        {
            var res = await _ccapplicationRepository.GetDataForCTPMakePayment(paymentinfoTransId);
            return res;
        }

        #region Not Implemented Methods
        public Task<CCApplicationDetails> GetASync(long entityID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CCApplicationDetails>> GetAllASync()
        {
            throw new NotImplementedException();
        }

        public Task<long> AddAsync(CCApplicationDetails entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CCApplicationDetails entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(CCApplicationDetails entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CCApplicationDetails>> GetListAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
