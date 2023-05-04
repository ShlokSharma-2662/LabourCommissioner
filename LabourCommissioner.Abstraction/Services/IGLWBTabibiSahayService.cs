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
    public interface IGLWBTabibiSahayService : IBaseService<TabModel>
    {
        Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId);
        Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename);
        Task<List<SelectListItem>> GetAllStates();
        Task<GLWB_TSY_personalDetails> GetPersonalDetailsByRegNo(string RegistrationNo);
        Task<GLWB_TSY_personalDetails> GetCompanyDetailsByUserName(string UserName);
        Task<GLWB_TSY_personalDetails> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename);

        Task<GLWBTSYSchemeDetails> GetTotalsahayByServiceID(int serviceId);
        Task<GLWBTSYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId);


        Task<IEnumerable<SelectListItem>> BindEmployee(string lwbaccountno);
        Task<IEnumerable> GetGLWBEmployeeDetailsbyRegistrationid(int registrationid);
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
        Task<ResponseMessage> AddSchemeDetails(GLWBTSYSchemeDetails schemeDetails, DataTable dt);
        Task<ResponseMessage> AddUpdateApplication(GLWB_TSY_personalDetails personalDetailsModel);

        Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails);
        Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData);
        Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel);
        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
    }
}
