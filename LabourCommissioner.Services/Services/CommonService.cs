using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Services.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _CommonRepository;
        public CommonService(ICommonRepository CommonRepository)
        {
            _CommonRepository = CommonRepository ?? throw new ArgumentNullException(nameof(CommonRepository));
        }
        
        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _CommonRepository.GetAllStates();
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _CommonRepository.GetDistrict();
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _CommonRepository.GetTalukaByDistrictId(districtId);
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _CommonRepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<WorkerPersonalDetailsModel> GetWorkerDetailsByRegNo(string RegistrationNo)
        {
            var res = _CommonRepository.GetWorkerDetailsByRegNo(RegistrationNo);
            return await res;
        }

    }
}
