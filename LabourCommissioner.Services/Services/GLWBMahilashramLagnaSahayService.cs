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
    public class GLWBMahilashramLagnaSahayService : IGLWBMahilashramLagnaSahayService
    {
        private readonly IGLWBMahilashramLagnaSahayRepository _iglwbMahilashramLagnaSahayrepository;

        public GLWBMahilashramLagnaSahayService(IGLWBMahilashramLagnaSahayRepository iglwbMahilashramLagnaSahayrepository)
        {
            _iglwbMahilashramLagnaSahayrepository = iglwbMahilashramLagnaSahayrepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetTabSequenceByApplicationId(ApplicationId,id,schemaname,tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetAllStates();
            return res;
        }

        public async Task<GLWBMSL_Personaldetails> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _iglwbMahilashramLagnaSahayrepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<GLWBMSL_Personaldetails> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _iglwbMahilashramLagnaSahayrepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<GLWBMSL_SchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _iglwbMahilashramLagnaSahayrepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }

      
        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _iglwbMahilashramLagnaSahayrepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(int subjectId)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetFileDocuments(ServiceId);
            return res;
        }
       
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetDocumentFileDetails(ServiceId, ApplicationId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(GLWBMSL_Personaldetails personalDetailsModel)
        {
            return await _iglwbMahilashramLagnaSahayrepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(GLWBMSL_SchemeDetails schemeDetails)
        {
            return await _iglwbMahilashramLagnaSahayrepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _iglwbMahilashramLagnaSahayrepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _iglwbMahilashramLagnaSahayrepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _iglwbMahilashramLagnaSahayrepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }

        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _iglwbMahilashramLagnaSahayrepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }

        public async Task<GLWBMSL_SchemeDetails> GetTotalsahayByServiceID(int serviceId)
        {
            var res = await _iglwbMahilashramLagnaSahayrepository.GetTotalsahayByServiceID(serviceId);
            return res;
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _iglwbMahilashramLagnaSahayrepository.FinalSubmit(finalSubmitModel);
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
