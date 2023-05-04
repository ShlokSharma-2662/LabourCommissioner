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
    public class GLWBHigherStudyService : IGLWBHigherStudyService
    {
        private readonly IGLWBHigherStudyRepository _iGLWBHigherStudyrepository;

        public GLWBHigherStudyService(IGLWBHigherStudyRepository iGLWBHigherStudyrepository)
        {
            _iGLWBHigherStudyrepository = iGLWBHigherStudyrepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _iGLWBHigherStudyrepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _iGLWBHigherStudyrepository.GetTabSequenceByApplicationId(ApplicationId, id, schemaname, tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _iGLWBHigherStudyrepository.GetAllStates();
            return res;
        }

        public async Task<GLWBHSS_PersonalDetails> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _iGLWBHigherStudyrepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<GLWBHSS_PersonalDetails> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _iGLWBHigherStudyrepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<GLWBHSS_SchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _iGLWBHigherStudyrepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }


        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _iGLWBHigherStudyrepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _iGLWBHigherStudyrepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(int subjectId)
        {
            var res = await _iGLWBHigherStudyrepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _iGLWBHigherStudyrepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _iGLWBHigherStudyrepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _iGLWBHigherStudyrepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _iGLWBHigherStudyrepository.GetFileDocuments(ServiceId);
            return res;
        }

        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId)
        {
            var res = await _iGLWBHigherStudyrepository.GetDocumentFileDetails(ServiceId, ApplicationId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(GLWBHSS_PersonalDetails personalDetailsModel)
        {
            return await _iGLWBHigherStudyrepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(GLWBHSS_SchemeDetails schemeDetails)
        {
            return await _iGLWBHigherStudyrepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _iGLWBHigherStudyrepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _iGLWBHigherStudyrepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _iGLWBHigherStudyrepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }

        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _iGLWBHigherStudyrepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _iGLWBHigherStudyrepository.FinalSubmit(finalSubmitModel);
        }

        public async Task<GLWBHSS_SchemeDetails> GetTotalsahayByServiceID(int serviceId, int course)
        {
            return await _iGLWBHigherStudyrepository.GetTotalsahayByServiceID(serviceId,course);
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
