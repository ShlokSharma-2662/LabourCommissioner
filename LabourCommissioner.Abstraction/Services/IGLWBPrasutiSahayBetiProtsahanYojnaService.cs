using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissioner.Abstraction.Services
{
    public interface IGLWBPrasutiSahayBetiProtsahanYojnaService : IBaseService<TabModel>
    {
        Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId);
        Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename);
        Task<List<SelectListItem>> GetAllStates();
        Task<GLWBPSY_PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo);
        Task<GLWBPSY_PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename);
        Task<ResponseMessage> getaadharcardcountbyaadharnoandserviceid(string aadharcardno, long serviceId);
        Task<GLWBPSY_SchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId);
        Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename);
        Task<IEnumerable<SelectListItem>> GetDistrict();
        Task<IEnumerable<SelectListItem>> GetSubject(int subjectId);
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId);
        Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId);
        Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType);
        Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId);
        Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId);
        Task<ResponseMessage> AddSchemeDetails(GLWBPSY_SchemeDetails schemeDetails);

        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddUpdateApplication(GLWBPSY_PersonalDetailsModel personalDetailsModel);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
        Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails);
        Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData);
        Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel);
        Task<GLWBPSY_SchemeDetails> GetTotalsahayByServiceID(int serviceId, int male, int female);
    }
}