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
    public class CMDashboardService : ICMDashboardService
    {
        private readonly ICMDashboardRepository _cmDashboardServiceRepository;

        public CMDashboardService(ICMDashboardRepository cmDashboardServiceRepository)
        {
            _cmDashboardServiceRepository = cmDashboardServiceRepository ?? throw new ArgumentNullException(nameof(cmDashboardServiceRepository));
        }
        public async Task<IEnumerable<SelectListItem>> GetServiceMasterByBeneficiaryIdforCMD(int beneficiarytypeid)
        {
            var res = await _cmDashboardServiceRepository.GetServiceMasterByBeneficiaryIdforCMD(beneficiarytypeid);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _cmDashboardServiceRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetYear()
        {
            var res = await _cmDashboardServiceRepository.GetYear();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetMonth()
        {
            var res = await _cmDashboardServiceRepository.GetMonth();
            return res;
        }
        public async Task<IEnumerable<CMDApplicationDetails>> GetCMDApplicationDetailslist(long appYear, long appMonth, long beneficiarytypeid, int statusId)
        {
            var res = _cmDashboardServiceRepository.GetCMDApplicationDetailslist(appYear, appMonth, beneficiarytypeid, statusId);
            return await res;
        }
        public async Task<CMDApplicationDetails> GetCMDApplicationDetailsForInsert(long applicationId, long appYear, long appMonth, long serviceId)
        {
            var res = _cmDashboardServiceRepository.GetCMDApplicationDetailsForInsert(applicationId, appYear, appMonth, serviceId);
            return await res;
        }
        public async Task<ResponseMessage> AddUpdateCMDApplication(DataTable dtData, CMDApplicationDetails cmdApplicationDetails)
        {
            return await _cmDashboardServiceRepository.AddUpdateCMDApplication(dtData, cmdApplicationDetails);
        }
        public async Task<ResponseMessage> CMDSubmitApplication(long appYear, long appMonth, long serviceId, long userId, string ipAddress, string hostName)
        {
            return await _cmDashboardServiceRepository.CMDSubmitApplication(appYear, appMonth, serviceId, userId, ipAddress, hostName);
        }
        public async Task<CMDAPIApplicationDetails> GetBOCWCMDApplicationDetails(long appYear, long appMonth, long serviceId)
        {
            var res = _cmDashboardServiceRepository.GetBOCWCMDApplicationDetails(appYear, appMonth, serviceId);
            return await res;
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
