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
using LabourCommissioner.DataRepository.Repositories;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissioner.Services.Services
{
    public class BOCWVishishtaCoachingYojanaService : IBOCWVishishtaCoachingYojanaService
    {
        private readonly IBOCWVishishtaCoachingYojanaRepository _bocwVishishtaCoachingYojanaRepository;

        public BOCWVishishtaCoachingYojanaService(IBOCWVishishtaCoachingYojanaRepository ibocwVishishtaCoachingYojanaRepository)
        {
            _bocwVishishtaCoachingYojanaRepository = ibocwVishishtaCoachingYojanaRepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetTabSequenceByApplicationId(ApplicationId,id,schemaname,tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetAllStates();
            return res;
        }

        public async Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _bocwVishishtaCoachingYojanaRepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _bocwVishishtaCoachingYojanaRepository.GetApplicationDetailsByAppId(ApplicationId, schemaname,tablename);
            return await res;
        }

        public async Task<BOCWVCYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _bocwVishishtaCoachingYojanaRepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }
        public async Task<BOCWVCYSchemeDetails> GetTotalsahayByServiceID(int serviceId)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetTotalsahayByServiceID(serviceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _bocwVishishtaCoachingYojanaRepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(string subjectId)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSemesterbyCourseId(int courseid)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetSemesterbyCourseId(courseid);
            return res;
        }
        public async Task<IEnumerable> GetBenifitByCourseId(int courseId, string semesteryear)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetBenifitByCourseId(courseId,semesteryear);
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId)
        {
            var res = await _bocwVishishtaCoachingYojanaRepository.GetDocumentFileDetails(ServiceId,ApplicationId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(PersonalDetailsModel personalDetailsModel)
        {
            return await _bocwVishishtaCoachingYojanaRepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(BOCWVCYSchemeDetails schemeDetails)
        {
            return await _bocwVishishtaCoachingYojanaRepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _bocwVishishtaCoachingYojanaRepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _bocwVishishtaCoachingYojanaRepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _bocwVishishtaCoachingYojanaRepository.FinalSubmit(finalSubmitModel);
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _bocwVishishtaCoachingYojanaRepository.GetSmsContentForService(serviceId, ApplicationId, SMSType,schemaname, tablename);
            return await res;
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _bocwVishishtaCoachingYojanaRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
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
