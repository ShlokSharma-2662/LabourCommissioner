using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.ViewDataModels;
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
    public interface IMastersRepository
    {
        Task<IEnumerable<SelectListItem>> GetDistrict();

        Task<ResponseMessage> AddUpdateDistrictMaster(DistrictMaster addDistrictMasters);
        Task<ResponseMessage> AddUpdateTalukaMaster(TalukaMaster addTalukaMaster);
        Task<ResponseMessage> AddDocumentsMasters(DocumentMaster addDocumentMaster);
        Task<ResponseMessage> AddResourceMaster(ResourceMaster addResourceMaster);
        Task<ResponseMessage> AddVillageMaster(VillageMaster addVillageMaster);
        Task<ResponseMessage> AddUpdateDeleteServiceSchedular(ServiceSchedular addServiceSchedular);
        Task<IEnumerable<DistrictMaster>> DistrictMaster();
        Task<IEnumerable<DocumentMaster>> DocumentMaster();
        Task<IEnumerable<VillageMaster>> VillageMaster();
        Task<IEnumerable<ServiceSchedular>> ServiceScheduler();

        Task<DistrictMaster> getdistrictdatabyid(long districtid);
        Task<ServiceSchedular> getserviceschedulerbyid(long serviceschedulerid);
        Task<DocumentMaster> getdocumentbyid(long documentmasterid);
        Task<ResourceMaster> getresourcebyid(long resourceid);
        Task<VillageMaster> getvillagedatabyid(long districtid, long villageid, long talukaid);

        Task<IEnumerable<TalukaMaster>> TalukaMaster();
        Task<IEnumerable<ResourceMaster>> ResourceMaster();
        Task<IEnumerable<ResourceMaster>> bindservicemaster();

        Task<TalukaMaster> gettalukabyId(long talukaid);
        Task<IEnumerable<VillageMaster>> getvillagebyDistrictTalukaId(int districtid,int talukaid);

        Task<IEnumerable<SelectListItem>> bindservicemaster(int beneficiarytypeid);
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtid);
    }
}
