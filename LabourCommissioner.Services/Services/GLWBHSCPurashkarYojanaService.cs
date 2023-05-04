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
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissioner.Services.Services
{
    public class GLWBHSCPurashkarYojanaService : IGLWBHSCPurashkarYojanaService
    {
        private readonly IGLWBHSCPurashkarYojanaRepository _iglwbhscpurashkaryojanarepository;

        public GLWBHSCPurashkarYojanaService(IGLWBHSCPurashkarYojanaRepository iglwbhscpurashkaryojanarepository)
        {
            _iglwbhscpurashkaryojanarepository = iglwbhscpurashkaryojanarepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetTabSequenceByApplicationId(ApplicationId,id,schemaname,tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetAllStates();
            return res;
        }

        public async Task<GLWBHSC_PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _iglwbhscpurashkaryojanarepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<GLWBHSC_PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _iglwbhscpurashkaryojanarepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<GLWBHSCSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _iglwbhscpurashkaryojanarepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }
        public async Task<GLWBHSCSchemeDetails> GetTotalsahayByServiceID(int serviceId)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetTotalsahayByServiceID(serviceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, string schemaname, string tablename, long serviceId)
        {
            var res = _iglwbhscpurashkaryojanarepository.GetUploadedDocuments(ApplicationId,schemaname,tablename,serviceId);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(int subjectId)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId)
        {
            var res = await _iglwbhscpurashkaryojanarepository.GetDocumentFileDetails(ServiceId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(GLWBHSC_PersonalDetailsModel personalDetailsModel)
        {
            return await _iglwbhscpurashkaryojanarepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(GLWBHSCSchemeDetails schemeDetails)
        {
            return await _iglwbhscpurashkaryojanarepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _iglwbhscpurashkaryojanarepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _iglwbhscpurashkaryojanarepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _iglwbhscpurashkaryojanarepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }

        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _iglwbhscpurashkaryojanarepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _iglwbhscpurashkaryojanarepository.FinalSubmit(finalSubmitModel);
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
