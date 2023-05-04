using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Services.Services
{
    public class CCRegistrationService : ICCRegistrationService
    {
        private readonly ICCRegistrationRepository _ccregistrationRepository;

        public CCRegistrationService(ICCRegistrationRepository ccregistrationRepository)
        {
            _ccregistrationRepository = ccregistrationRepository ?? throw new ArgumentNullException(nameof(ccregistrationRepository));
        }
        public async Task<IEnumerable<SelectListItem>> GetCCUserType(string ResourceType)
        {
            var res = await _ccregistrationRepository.GetCCUserType(ResourceType);
            return res;
        }
        public async Task<bool> UserAlreadyExist(string? PANTANNo)
        {
            return await _ccregistrationRepository.UserAlreadyExist(PANTANNo);
        }
        public async Task<ResponseMessage> AddUpdateRegistration(CCRegistration registration)
        {
            return await _ccregistrationRepository.AddUpdateRegistration(registration);
        }
        public async Task<ResponseMessage> AddUpdateAuthorityDetails(CCRegistration registration)
        {
            return await _ccregistrationRepository.AddUpdateAuthorityDetails(registration);
        }

        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _ccregistrationRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }

        #region Not Implemented Methods
        public Task<CCRegistration> GetASync(long entityID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CCRegistration>> GetAllASync()
        {
            throw new NotImplementedException();
        }

        public Task<long> AddAsync(CCRegistration entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(CCRegistration entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(CCRegistration entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CCRegistration>> GetListAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
