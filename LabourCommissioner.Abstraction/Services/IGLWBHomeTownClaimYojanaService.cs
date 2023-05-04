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
    public interface IGLWBHomeTownClaimYojanaService : IBaseService<TabModel>
    {
        Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId);
        Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename);
        Task<List<SelectListItem>> GetAllStates();
        Task<GLWBhty_PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo);
        Task<GLWBhty_PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename);
        Task<GLWBhtySchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId);
        Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename);
        Task<IEnumerable<SelectListItem>> GetDistrict();
        Task<IEnumerable<SelectListItem>> GetSubject(int subjectId);
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId);
        Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId);
        Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType);
        Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId);
        Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId);
        Task<ResponseMessage> AddSchemeDetails(GLWBhtySchemeDetails schemeDetails,DataTable dt);

        Task<ResponseMessage> AddUpdateApplication(GLWBhty_PersonalDetailsModel personalDetailsModel);

        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails);
        Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData);

        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);

        Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel);

       // Task<IEnumerable<GLWBhtySchemeDetails>> GetApplicationDetails(long serviceId, string schemaName, string tableName);

    }
}