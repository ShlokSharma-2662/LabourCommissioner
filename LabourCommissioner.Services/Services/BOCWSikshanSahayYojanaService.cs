﻿using System;
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
    public class BOCWSikshanSahayYojanaService : IBOCWSikshanSahayYojanaService
    {
        private readonly IBOCWSikshanSahayYojanaRepository _bocwSikshanSahayYojanaRepository;

        public BOCWSikshanSahayYojanaService(IBOCWSikshanSahayYojanaRepository ibocwSikshanSahayYojanaRepository)
        {
            _bocwSikshanSahayYojanaRepository = ibocwSikshanSahayYojanaRepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetTabSequenceByApplicationId(ApplicationId,id,schemaname,tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetAllStates();
            return res;
        }

        public async Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _bocwSikshanSahayYojanaRepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }
        public async Task<PersonalDetailsModelAPI> GetPersonalDetailsByRegNoAPI(string RegistrationNo)
        {
            var res = _bocwSikshanSahayYojanaRepository.GetPersonalDetailsByRegNoAPI(RegistrationNo);
            return await res;
        }
       
        public async Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _bocwSikshanSahayYojanaRepository.GetApplicationDetailsByAppId(ApplicationId, schemaname,tablename);
            return await res;
        }
        public async Task<PersonalDetailsModelAPI> GetApplicationDetailsByAppIdAPI(long ApplicationId, string schemaname, string tablename)
        {
            var res = _bocwSikshanSahayYojanaRepository.GetApplicationDetailsByAppIdAPI(ApplicationId, schemaname, tablename);
            return await res;
        }

        public async Task<SchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _bocwSikshanSahayYojanaRepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }

        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _bocwSikshanSahayYojanaRepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(string subjectId)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSemesterbyCourseId(int courseid)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetSemesterbyCourseId(courseid);
            return res;
        }
        public async Task<IEnumerable> GetBenifitByCourseId(int courseId, string semesteryear)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetBenifitByCourseId(courseId,semesteryear);
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId)
        {
            var res = await _bocwSikshanSahayYojanaRepository.GetDocumentFileDetails(ServiceId,ApplicationId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(PersonalDetailsModel personalDetailsModel)
        {
            return await _bocwSikshanSahayYojanaRepository.AddUpdateApplication(personalDetailsModel);
        }
        public async Task<ResponseMessage> AddUpdateApplicationAPI(PersonalDetailsModelAPI personalDetailsModel)
        {
            return await _bocwSikshanSahayYojanaRepository.AddUpdateApplicationAPI(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(SchemeDetails schemeDetails)
        {
            return await _bocwSikshanSahayYojanaRepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _bocwSikshanSahayYojanaRepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _bocwSikshanSahayYojanaRepository.AddUpdateDocumentDetailsNew(dtData);
        }

        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _bocwSikshanSahayYojanaRepository.FinalSubmit(finalSubmitModel);
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _bocwSikshanSahayYojanaRepository.GetSmsContentForService(serviceId, ApplicationId, SMSType,schemaname, tablename);
            return await res;
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _bocwSikshanSahayYojanaRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
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
