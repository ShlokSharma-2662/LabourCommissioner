using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Abstraction.Repositories
{
    public interface IHospitalRepository
    {
        Task<IEnumerable<ServiceMaster>> BindServicesEmployeeWiseFilter(int usertype, int benificiarytypeid, int postid, int roleid);
        Task<long> GetLevelNoFromPostId(long postid);
        Task<IEnumerable<ApplicationCountForDashBoard>> BindAppcountDistrictTalukaWiseForDashboard(long finYear, long districtId, long postId, string schemaName);
        Task<IEnumerable<SelectListItem>> GetDistrict();
        Task<IEnumerable<SelectListItem>> GetFinancialYearList();
        Task<IEnumerable<SelectListItem>> GetDivision();
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId);
        Task<IEnumerable<SelectListItem>> GetSubServiceByServiceId(long serviceId);
        Task<IEnumerable<SelectListItem>> GetDistrictNamebyDivisionId(int districtId);
        Task<ServiceMaster> GetSchemeByServiceId(long serviceId);
        //Task<ApplicationFilterModel> GetApplicationDetails(long registrationId, long serviceId, string schemaName, string tableName);
        Task<IEnumerable<EmpApplicationDetailsModel>> BOCW_SSY_GetApplicationDetailsList(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate,
            int statusId, string? search, long postId, long serviceid, string? action, string? schemaName, long subServiceId);

        Task<IEnumerable<EmpApplicationDetailsModel>> GLWB_GetApplicationDetailsList(int? pageNo, int pageSize, long divisionId, long talukaId, long districtId, DateTime? fromDate, DateTime? toDate,
            int statusId, string? search, long postId, long serviceid, string? action, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GLWB_Hospital_GetApplicationDetailsList(int? pageNo, int pageSize, long divisionId, long talukaId, long districtId, DateTime? fromDate, DateTime? toDate,
           int statusId, string? search, long postId, long serviceid, string? action, string? schemaName);

        Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId);
        Task<IEnumerable<SelectListItem>> GetNextRole(int serviceid, int postid, long districtid, long talukaid);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetUploadedDocumentForEmployee(int applicationId, int serviceId, string schemaName, string tableName);
        Task<EmpApplicationDetailsModel> ApproveApplication(string applicationId, string approvalstate, int toroleid, string approvalstatus, string remarks, string fromPostId, string serviceId, string topostid, string schemaname, string tablename, int beneficiarytype);
        Task<IEnumerable<EmpApplicationDetailsModel>> ApproveMultipleApplication(string applicationId, string approvalstate, int toroleid, string approvalstatus, string remarks, string fromPostId, string serviceId, string topostid, string schemaname, string tablename, int beneficiarytype, DataTable dtApplicationIds, string ipAddress, string hostName);
        Task<EmpApplicationDetailsModel> IsLastLevel(int serviceid, int postid, int applicationid);
        Task<IEnumerable<SelectListItem>> GetSendBackRole(int serviceid, int postid, long districtid, long talukaid, long aplicationid, string schemaName, int beneficiarytype);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetApplicationHistory(long applicationid, string schemaName);

        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
        Task<IEnumerable<CountForDashBoard>> BindCountForDashboard(long finYear, long districtId, long talukaId, long postId, long serviceId);
        Task<IEnumerable<CountForDashBoard>> BindAdminDashBoardData(long hodid, long districtId, long talukaId, long postId, long finYear);
        Task<IEnumerable<CountForDashBoard>> BindAdminGridData(long finYear, long hodid, long districtId, long talukaId, long postId);
        IEnumerable<AdminMenu> BindMenuRoleWise(int usertypeId, long serviceId, long parentmenuId, string roleId, long postId);
        Task<IEnumerable<RegisteredApplicant>> ViewRegisteredApplicant(long hodId, DateTime? fromDate, DateTime? toDate, long districtId, int? pageNo, int pageSize, string? search);
        Task<ResponseMessage> ResetPassword(long registrationId, long userId, string password, string ipAddress, string hostName);
        Task<ResponseMessage> UpdateCitizenMobileNo(long registrationId, long userId, string mobileNo);
        Task<ApplicationStatusModel> ViewApplicationStatus(string applicationno, DateTime? dob);
        Task<IEnumerable<ApplicationStatusDetails>> GetRoleForApplicationStatus(string applicationno, DateTime? dob);

        Task<ApplicationStatusModel> ViewApplicationEmployeeSearch(string applicationno, int hodid);
        Task<IEnumerable<ApplicationStatusDetails>> GetRoleForApplicationStatusForEmployee(string applicationno, int hodid);

        Task<IEnumerable<CompletedApplicationDetailsModel>> ViewTotalApplicationDetails(long hodId, DateTime? fromDate, DateTime? toDate, long serviceId, long isApproved, int? pageNo, int pageSize, string? search);
        Task<IEnumerable<SelectListItem>> GetServiceMasterByBeneficiaryId(int beneficiarytypeid);
        Task<IEnumerable<EmpApplicationDetailsModel>> BOCWCompletedApplicationForPayment(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate, string? search, long postId, long serviceid, string? action, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetAppDetailsByAppIdForViewAadesh(string? applicationids, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> BOCWViewAadeshDetails(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate,int statusId, string? search, long serviceid, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetAadeshDetailsbyAadeshid(long aadeshId, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> BOCWSendForPayment(DataTable dtAadeshIds, long serviceId, string schemaName, long postId, string ipAddress, string hostName);

        Task<IEnumerable<EmpApplicationDetailsModel>> GetAppDetailsByAppIdForGLWBViewAadesh(string? applicationids, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GLWBViewAadeshDetails(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long serviceid, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetGLWBAadeshDetailsbyAadeshid(long aadeshId, string? schemaName, long applicationId);
       // Task<IEnumerable<PaymentDetailsModel>> GetBOCWPaymentHistory(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate, string? search, long serviceid);
        Task <ResponseMessage>AddDocumentForBOCW_ACSY(DocumentDetails.DocumentFileDetails model);
        Task <DocumentDetails.DocumentFileDetails>getemployeeuploadeddocumentsbyappid(long applicationid);
        Task<IEnumerable<PaymentDetailsModel>> GetBOCWPaymentHistory(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate, string? search, long serviceid, int statusId);
    }
}
