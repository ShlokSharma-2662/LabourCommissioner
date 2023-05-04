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
    public class BOCWTabletSahayYojanaService : IBOCWTabletSahayYojanaService
    {
        private readonly IBOCWTabletSahayYojanaRepository _bocwTabletSahayYojanaRepository;

        public BOCWTabletSahayYojanaService(IBOCWTabletSahayYojanaRepository ibocwTabletSahayYojanaRepository)
        {
            _bocwTabletSahayYojanaRepository = ibocwTabletSahayYojanaRepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetTabSequenceByApplicationId(ApplicationId, id, schemaname, tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _bocwTabletSahayYojanaRepository.GetAllStates();
            return res;
        }

        public async Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _bocwTabletSahayYojanaRepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _bocwTabletSahayYojanaRepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<BOCWTSYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _bocwTabletSahayYojanaRepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }

        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _bocwTabletSahayYojanaRepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _bocwTabletSahayYojanaRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(string subjectId)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSemesterbyCourseId(int courseid)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetSemesterbyCourseId(courseid);
            return res;
        }
        public async Task<IEnumerable> GetBenifitByCourseId(int courseId, string semesteryear)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetBenifitByCourseId(courseId, semesteryear);
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId)
        {
            var res = await _bocwTabletSahayYojanaRepository.GetDocumentFileDetails(ServiceId, ApplicationId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(PersonalDetailsModel personalDetailsModel)
        {
            return await _bocwTabletSahayYojanaRepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(BOCWTSYSchemeDetails schemeDetails)
        {
            return await _bocwTabletSahayYojanaRepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _bocwTabletSahayYojanaRepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _bocwTabletSahayYojanaRepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _bocwTabletSahayYojanaRepository.FinalSubmit(finalSubmitModel);
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _bocwTabletSahayYojanaRepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _bocwTabletSahayYojanaRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
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
