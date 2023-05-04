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
    public class GLWBTabibiSahayService : IGLWBTabibiSahayService
    {
        private readonly IGLWBTabibiSahayRepository _GLWBTabibiSahayRepository;

        public GLWBTabibiSahayService(IGLWBTabibiSahayRepository iGLWBTabibiSahayRepository)
        {
            _GLWBTabibiSahayRepository = iGLWBTabibiSahayRepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _GLWBTabibiSahayRepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _GLWBTabibiSahayRepository.GetTabSequenceByApplicationId(ApplicationId, id, schemaname, tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _GLWBTabibiSahayRepository.GetAllStates();
            return res;
        }

        public async Task<GLWB_TSY_personalDetails> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _GLWBTabibiSahayRepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }
        public async Task<GLWB_TSY_personalDetails> GetCompanyDetailsByUserName(string UserName)
        {
            var res = _GLWBTabibiSahayRepository.GetCompanyDetailsByUserName(UserName);
            return await res;
        }

        public async Task<GLWB_TSY_personalDetails> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _GLWBTabibiSahayRepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<GLWBTSYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _GLWBTabibiSahayRepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }
        public async Task<GLWBTSYSchemeDetails> GetTotalsahayByServiceID(int serviceId)
        {
            var res = await _GLWBTabibiSahayRepository.GetTotalsahayByServiceID(serviceId);
            return res;
        }

        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _GLWBTabibiSahayRepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _GLWBTabibiSahayRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> BindEmployee(string lwbaccountno)
        {
            var res = await _GLWBTabibiSahayRepository.BindEmployee(lwbaccountno);
            return res;
        }       
        public async Task<IEnumerable> GetGLWBEmployeeDetailsbyRegistrationid(int registrationid)
        {
            var res = await _GLWBTabibiSahayRepository.GetGLWBEmployeeDetailsbyRegistrationid(registrationid);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(string subjectId)
        {
            var res = await _GLWBTabibiSahayRepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _GLWBTabibiSahayRepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSemesterbyCourseId(int courseid)
        {
            var res = await _GLWBTabibiSahayRepository.GetSemesterbyCourseId(courseid);
            return res;
        }
        public async Task<IEnumerable> GetBenifitByCourseId(int courseId, string semesteryear)
        {
            var res = await _GLWBTabibiSahayRepository.GetBenifitByCourseId(courseId, semesteryear);
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _GLWBTabibiSahayRepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _GLWBTabibiSahayRepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _GLWBTabibiSahayRepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId)
        {
            var res = await _GLWBTabibiSahayRepository.GetDocumentFileDetails(ServiceId, ApplicationId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(GLWB_TSY_personalDetails personalDetailsModel)
        {
            return await _GLWBTabibiSahayRepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(GLWBTSYSchemeDetails schemeDetails, DataTable dt)
        {
            return await _GLWBTabibiSahayRepository.AddSchemeDetails(schemeDetails,dt);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _GLWBTabibiSahayRepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _GLWBTabibiSahayRepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _GLWBTabibiSahayRepository.FinalSubmit(finalSubmitModel);
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _GLWBTabibiSahayRepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _GLWBTabibiSahayRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
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
