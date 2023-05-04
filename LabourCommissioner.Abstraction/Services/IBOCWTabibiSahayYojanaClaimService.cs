using System;
using System.Collections;
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
    public interface IBOCWTabibiSahayYojanaClaimService : IBaseService<TabModel>
    {
        Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId);
        Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename);
        Task<List<SelectListItem>> GetAllStates();
        Task<BOCW_TBSYClaim_personalDetails> GetPersonalDetailsByRegNo(string RegistrationNo);
        Task<BOCW_TBSYClaim_personalDetails> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename);
        Task<BOCW_TBSYClaim_personalDetails> GetCompanyDetailsByUserName(string UserName);
        Task<BOCWTBSYSchemeDetails> GetTotalsahayByServiceID(int serviceId);
        Task<BOCWTBSYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId);
        
        
        Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename);
        Task<IEnumerable<SelectListItem>> GetDistrict();
        Task<IEnumerable<SelectListItem>> GetSubject(string subjectId);
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId);
        Task<IEnumerable<SelectListItem>> GetSemesterbyCourseId(int courseid);
        Task<IEnumerable> GetBenifitByCourseId(int courseId, string semesteryear);
        Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId);
        Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType);
        Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId);
        Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId);
        Task<ResponseMessage> AddSchemeDetails(BOCWTBSYSchemeDetails schemeDetails);
        Task<ResponseMessage> AddSchemeDetails(BOCW_TBSYClaim_personalDetails schemeDetails);
        Task<ResponseMessage> AddUpdateApplication(BOCW_TBSYClaim_personalDetails personalDetailsModel);

        Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails);
        Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData);
        Task<ResponseMessage> AddSchemeDetails(DataTable dtData);
        Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel);
        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
        Task<IEnumerable<BOCW_TBSYClaim_personalDetails>> GetCheckedUPApplicationDetailsByServiceId(long serviceId, string schemaname, string tablename);
    }
}
