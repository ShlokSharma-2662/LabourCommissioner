using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
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
    public class MastersService : IMastersService
    {
        private readonly IMastersRepository _homeRepository;
        public MastersService(IMastersRepository homeRepository)
        {
            _homeRepository = homeRepository ?? throw new ArgumentNullException(nameof(homeRepository));
        }
        //public async Task<IEnumerable<ServiceMaster>> BindServicesUserWiseFilter()
        //{
        //    return await _homeRepository.BindServicesUserWiseFilter();


        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _homeRepository.GetDistrict();
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtid)
        {
            var res = await _homeRepository.GetTalukaByDistrictId(districtid);
            return res;
        }
        public async Task<ResponseMessage> AddUpdateDistrictMaster(DistrictMaster addDistrictMasters)
        {
            var res = await _homeRepository.AddUpdateDistrictMaster(addDistrictMasters);
            return res;
        }
        public async Task<ResponseMessage> AddUpdateTalukaMaster(TalukaMaster addTalukaMaster)
        {
            var res = await _homeRepository.AddUpdateTalukaMaster(addTalukaMaster);
            return res;
        }
         public async Task<ResponseMessage> AddDocumentsMasters(DocumentMaster addDocumentMaster)
        {
            var res = await _homeRepository.AddDocumentsMasters(addDocumentMaster);
            return res;
        } 
        public async Task<ResponseMessage> AddResourceMaster(ResourceMaster addResourceMaster)
        {
            var res = await _homeRepository.AddResourceMaster(addResourceMaster);
            return res;
        }
        public async Task<ResponseMessage> AddUpdateDeleteServiceSchedular(ServiceSchedular addServiceSchedular)
        {
            var res = await _homeRepository.AddUpdateDeleteServiceSchedular(addServiceSchedular);
            return res;
        }
        public async Task<ResponseMessage> AddVillageMaster(VillageMaster addVillageMaster)
        {
            var res = await _homeRepository.AddVillageMaster(addVillageMaster);
            return res;
        }

        public async Task<IEnumerable<DistrictMaster>> DistrictMaster()
        {
            var res = await _homeRepository.DistrictMaster();
            return res;
        }
        public async Task<IEnumerable<ServiceSchedular>> ServiceScheduler()
        {
            var res = await _homeRepository.ServiceScheduler();
            return res;
        }
        public async Task<IEnumerable<VillageMaster>> VillageMaster()
        {
            var res = await _homeRepository.VillageMaster();
            return res;
        }
        public async Task<IEnumerable<DocumentMaster>> DocumentMaster()
        {
            var res = await _homeRepository.DocumentMaster();
            return res;
        }

        public async Task<DistrictMaster> getdistrictdatabyid(long districtid)
        {
            return await _homeRepository.getdistrictdatabyid(districtid);
        }
        public async Task<ServiceSchedular> getserviceschedulerbyid(long serviceschedulerid)
        {
            return await _homeRepository.getserviceschedulerbyid(serviceschedulerid);
        }
        
        public async Task<VillageMaster> getvillagedatabyid(long districtid, long villageid, long talukaid)
        {
            return await _homeRepository.getvillagedatabyid(districtid, villageid, talukaid);
        }
         public async Task<DocumentMaster> getdocumentbyid(long documentmasterid)
        {
            return await _homeRepository.getdocumentbyid(documentmasterid);
        }
        public async Task<ResourceMaster> getresourcebyid(long resourceid)
        {
            return await _homeRepository.getresourcebyid(resourceid);
        }

        public async Task<IEnumerable<SelectListItem>> bindservicemaster(int beneficiarytypeid)
        {
            var res = await _homeRepository.bindservicemaster(beneficiarytypeid);
            return res;
        }

        public async Task<IEnumerable<TalukaMaster>> TalukaMaster()
        {
            var res = await _homeRepository.TalukaMaster();
            return res;
        }
        public async Task<IEnumerable<ResourceMaster>> ResourceMaster()
        {
            var res = await _homeRepository.ResourceMaster();
            return res;
        }
        public async Task<IEnumerable<ResourceMaster>> bindservicemaster()
        {
            var res = await _homeRepository.bindservicemaster();
            return res;
        }
        public async Task<TalukaMaster> gettalukabyId(long talukaid)
        {
            return await _homeRepository.gettalukabyId(talukaid);
        }
        public async Task<IEnumerable<VillageMaster>> getvillagebyDistrictTalukaId(int districtid, int talukaid)
        {
            return await _homeRepository.getvillagebyDistrictTalukaId(districtid, talukaid);
        }

    }
}
