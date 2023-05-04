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
    public class GLWBAccidentalSahayYojanaService : IGLWBAccidentalSahayYojanaService
    {
        private readonly IGLWBAccidentalSahayYojanaRepository _iglwbAccidentalSahayYojanaServicerepository;

        public GLWBAccidentalSahayYojanaService(IGLWBAccidentalSahayYojanaRepository iglwbAccidentalSahayYojanaServicerepository)
        {
            _iglwbAccidentalSahayYojanaServicerepository = iglwbAccidentalSahayYojanaServicerepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetTabSequenceByApplicationId(ApplicationId,id,schemaname,tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetAllStates();
            return res;
        }

        public async Task<GLWBASY_PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _iglwbAccidentalSahayYojanaServicerepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<GLWBASY_PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _iglwbAccidentalSahayYojanaServicerepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<GLWBASYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _iglwbAccidentalSahayYojanaServicerepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }

        public async Task<GLWBASYSchemeDetails> GetTotalsahayByServiceID(int serviceId)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetTotalsahayByServiceID(serviceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _iglwbAccidentalSahayYojanaServicerepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(int subjectId)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId)
        {
            var res = await _iglwbAccidentalSahayYojanaServicerepository.GetDocumentFileDetails(ServiceId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(GLWBASY_PersonalDetailsModel personalDetailsModel)
        {
            return await _iglwbAccidentalSahayYojanaServicerepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(GLWBASYSchemeDetails schemeDetails)
        {
            return await _iglwbAccidentalSahayYojanaServicerepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _iglwbAccidentalSahayYojanaServicerepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _iglwbAccidentalSahayYojanaServicerepository.AddUpdateDocumentDetailsNew(dtData);
        }
        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _iglwbAccidentalSahayYojanaServicerepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }

        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _iglwbAccidentalSahayYojanaServicerepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }
        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _iglwbAccidentalSahayYojanaServicerepository.FinalSubmit(finalSubmitModel);
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
