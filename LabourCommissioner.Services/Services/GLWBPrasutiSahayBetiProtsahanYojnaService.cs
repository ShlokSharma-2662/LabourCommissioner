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
    public class GLWBPrasutiSahayBetiProtsahanYojnaService : IGLWBPrasutiSahayBetiProtsahanYojnaService
    {
        private readonly IGLWBPrasutiSahayBetiProtsahanYojnaRepository _iglwbprasutisahaybetiprotsahanyojnarepository;

        public GLWBPrasutiSahayBetiProtsahanYojnaService(IGLWBPrasutiSahayBetiProtsahanYojnaRepository iglwbprasutisahaybetiprotsahanyojnarepository)
        {
            _iglwbprasutisahaybetiprotsahanyojnarepository = iglwbprasutisahaybetiprotsahanyojnarepository;
        }

        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetServiceTabByServiceId(ServiceId);
            return res;
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetTabSequenceByApplicationId(ApplicationId,id,schemaname,tablename);
            return res;
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetAllStates();
            return res;
        }

        public async Task<GLWBPSY_PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            var res = _iglwbprasutisahaybetiprotsahanyojnarepository.GetPersonalDetailsByRegNo(RegistrationNo);
            return await res;
        }

        public async Task<GLWBPSY_PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            var res = _iglwbprasutisahaybetiprotsahanyojnarepository.GetApplicationDetailsByAppId(ApplicationId, schemaname, tablename);
            return await res;
        }
        public async Task<ResponseMessage> getaadharcardcountbyaadharnoandserviceid(string aadharcardno, long serviceId)
        {
            var res = _iglwbprasutisahaybetiprotsahanyojnarepository.getaadharcardcountbyaadharnoandserviceid(aadharcardno, serviceId);
            return await res;
        }

        public async Task<GLWBPSY_SchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            var res = _iglwbprasutisahaybetiprotsahanyojnarepository.GetApplicationSchemeDetailsByAppId(ApplicationId);
            return await res;
        }

        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            var res = _iglwbprasutisahaybetiprotsahanyojnarepository.GetUploadedDocuments(ApplicationId, serviceId, schemaname, tablename);
            return await res;
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubject(int subjectId)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetSubject(subjectId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetEducation(ResourceType);
            return res;
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetFileDocuments(ServiceId);
            return res;
        }
        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetDocumentFileDetails(ServiceId);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateApplication(GLWBPSY_PersonalDetailsModel personalDetailsModel)
        {
            return await _iglwbprasutisahaybetiprotsahanyojnarepository.AddUpdateApplication(personalDetailsModel);
        }

        public async Task<ResponseMessage> AddSchemeDetails(GLWBPSY_SchemeDetails schemeDetails)
        {
            return await _iglwbprasutisahaybetiprotsahanyojnarepository.AddSchemeDetails(schemeDetails);
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            return await _iglwbprasutisahaybetiprotsahanyojnarepository.AddUpdateDocumentDetails(lstdocumentFileDetails);
        }

        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)
        {
            return await _iglwbprasutisahaybetiprotsahanyojnarepository.AddUpdateDocumentDetailsNew(dtData);
        }
        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _iglwbprasutisahaybetiprotsahanyojnarepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }

        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _iglwbprasutisahaybetiprotsahanyojnarepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }
        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            return await _iglwbprasutisahaybetiprotsahanyojnarepository.FinalSubmit(finalSubmitModel);
        }

        public async Task<GLWBPSY_SchemeDetails> GetTotalsahayByServiceID(int serviceId, int male, int female)
        {
            var res = await _iglwbprasutisahaybetiprotsahanyojnarepository.GetTotalsahayByServiceID(serviceId, male, female);
            return res;
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
