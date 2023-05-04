using LabourCommissioner.Abstraction.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LabourCommissioner.Abstraction.ViewDataModels;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using LabourCommissioner.Common;
using System.Data;

namespace LabourCommissioner.Abstraction.Repositories
{
    public interface IGLWBAccidentalSahayYojanaRepository : IBaseRepository<TabModel>
    {
        Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId);
        Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename);
        Task<List<SelectListItem>> GetAllStates();
        Task<GLWBASY_PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo);
        Task<GLWBASY_PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename);
        Task<GLWBASYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId);
        Task<GLWBASYSchemeDetails> GetTotalsahayByServiceID(int serviceId);
        Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename);
        Task<IEnumerable<SelectListItem>> GetDistrict();

        Task<IEnumerable<SelectListItem>> GetSubject(int subjectId);
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId);
        Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId);
        Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType);
        Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId);

        Task<ResponseMessage> AddUpdateApplication(GLWBASY_PersonalDetailsModel personalDetailsModel);
        Task<ResponseMessage> AddSchemeDetails(GLWBASYSchemeDetails schemeDetails);
        Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId);
        Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails);

        Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData);

        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
        Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel);
    }
}