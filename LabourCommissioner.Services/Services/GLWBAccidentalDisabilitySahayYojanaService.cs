using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.DataRepository.Repositories;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissioner.Services.Services
{
    public class GLWBAccidentalDisabilitySahayYojanaService : IGLWBAccidentalDisabilitySahayYojanaService
    {
        private readonly IGLWBAccidentalDisabilitySahayYojanaRepository _iglwbAccidentalDisabilitySahayYojanaServicerepository;

        public GLWBAccidentalDisabilitySahayYojanaService(IGLWBAccidentalDisabilitySahayYojanaRepository iglwbAccidentalDisabilitySahayYojanaServicerepository)
        {
            _iglwbAccidentalDisabilitySahayYojanaServicerepository = iglwbAccidentalDisabilitySahayYojanaServicerepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetTabSequenceByApplicationId(ApplicationId,id,schemaname,tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetAllStates();
            return res;
        }

        public async Task<GLWBADSY_PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<GLWBADSY_PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<GLWBADSYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }

        public async Task<GLWBADSYSchemeDetails> GetTotalsahayByServiceID(int serviceId)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetTotalsahayByServiceID(serviceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(int subjectId)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId)
        {
            var res = await _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetDocumentFileDetails(ServiceId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(GLWBADSY_PersonalDetailsModel personalDetailsModel)
        {
            return await _iglwbAccidentalDisabilitySahayYojanaServicerepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(GLWBADSYSchemeDetails schemeDetails)
        {
            return await _iglwbAccidentalDisabilitySahayYojanaServicerepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _iglwbAccidentalDisabilitySahayYojanaServicerepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _iglwbAccidentalDisabilitySahayYojanaServicerepository.AddUpdateDocumentDetailsNew(dtData);
        }
        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _iglwbAccidentalDisabilitySahayYojanaServicerepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }

        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _iglwbAccidentalDisabilitySahayYojanaServicerepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }
        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _iglwbAccidentalDisabilitySahayYojanaServicerepository.FinalSubmit(finalSubmitModel);
        }

        #region Not Implemented
        public Task<TabModel> GetASync(long entityID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TabModel>> GetAllASync()
        {
            throw new NotImplementedException();
        }

        public Task<long> AddAsync(TabModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TabModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TabModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TabModel>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
