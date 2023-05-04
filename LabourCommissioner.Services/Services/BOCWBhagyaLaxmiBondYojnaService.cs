using System;
using System.Collections;
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
    public class BOCWBhagyaLaxmiBondYojnaService : IBOCWBhagyaLaxmiBondYojnaService
    {
        private readonly IBOCWBhagyaLaxmiBondYojnaRepository _bocwBhagyaLaxmiBondYojnaRepository;

        public BOCWBhagyaLaxmiBondYojnaService(IBOCWBhagyaLaxmiBondYojnaRepository ibocwBhagyaLaxmiBondYojnaRepository)
        {
            _bocwBhagyaLaxmiBondYojnaRepository = ibocwBhagyaLaxmiBondYojnaRepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetTabSequenceByApplicationId(ApplicationId, id, schemaname, tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetAllStates();
            return res;
        }

        public async Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _bocwBhagyaLaxmiBondYojnaRepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _bocwBhagyaLaxmiBondYojnaRepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<BOCWBPSYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _bocwBhagyaLaxmiBondYojnaRepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }

        public async Task<BOCWBPSYSchemeDetails> GettotalsahaybysahayId(int serviceid, int relation, int gender, int isApplied, int tharavId)
        {
            var res = _bocwBhagyaLaxmiBondYojnaRepository.GettotalsahaybysahayId(serviceid, relation, gender,isApplied,tharavId);
            return await res;
        }

        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _bocwBhagyaLaxmiBondYojnaRepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(int subjectId)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable> GetBenifitByCourseId(int courseId)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetBenifitByCourseId(courseId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId , int ApplicationId)
        {
            var res = await _bocwBhagyaLaxmiBondYojnaRepository.GetDocumentFileDetails(ServiceId, ApplicationId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(PersonalDetailsModel personalDetailsModel)
        {
            return await _bocwBhagyaLaxmiBondYojnaRepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(BOCWBPSYSchemeDetails schemeDetails)
        {
            return await _bocwBhagyaLaxmiBondYojnaRepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _bocwBhagyaLaxmiBondYojnaRepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _bocwBhagyaLaxmiBondYojnaRepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _bocwBhagyaLaxmiBondYojnaRepository.FinalSubmit(finalSubmitModel);
        }
        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _bocwBhagyaLaxmiBondYojnaRepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _bocwBhagyaLaxmiBondYojnaRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
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
