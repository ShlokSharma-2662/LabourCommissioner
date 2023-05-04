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
    public class GLWBSSCPurashkarYojanaService : IGLWBSSCPurashkarYojanaService
    {
        private readonly IGLWBSSCPurashkarYojanaRepository _iglwbsscpurashkaryojanarepository;

        public GLWBSSCPurashkarYojanaService(IGLWBSSCPurashkarYojanaRepository iglwbsscpurashkaryojanarepository)
        {
            _iglwbsscpurashkaryojanarepository = iglwbsscpurashkaryojanarepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetTabSequenceByApplicationId(ApplicationId,id,schemaname,tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetAllStates();
            return res;
        }

        public async Task<GLWBSSC_PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _iglwbsscpurashkaryojanarepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<GLWBSSC_PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _iglwbsscpurashkaryojanarepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }       

        public async Task<GLWBSSCSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _iglwbsscpurashkaryojanarepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }
        public async Task<GLWBSSCSchemeDetails> GetTotalsahayByServiceID(int serviceId)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetTotalsahayByServiceID(serviceId);
            return res;
        }

        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, string schemaname, string tablename, long serviceId)
        {
            var res = _iglwbsscpurashkaryojanarepository.GetUploadedDocuments(ApplicationId,schemaname,tablename,serviceId);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(int subjectId)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId)
        {
            var res = await _iglwbsscpurashkaryojanarepository.GetDocumentFileDetails(ServiceId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(GLWBSSC_PersonalDetailsModel personalDetailsModel)
        {
            return await _iglwbsscpurashkaryojanarepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(GLWBSSCSchemeDetails schemeDetails)
        {
            return await _iglwbsscpurashkaryojanarepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _iglwbsscpurashkaryojanarepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _iglwbsscpurashkaryojanarepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _iglwbsscpurashkaryojanarepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }

        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _iglwbsscpurashkaryojanarepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _iglwbsscpurashkaryojanarepository.FinalSubmit(finalSubmitModel);
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
