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

namespace LabourCommissioner.Abstraction.Services
{
    public interface IEmployeeHomeService /*: IBaseService<Usermaster>*/
    {
        Task<IEnumerable<ServiceMaster>> BindServicesEmployeeWiseFilter(int usertype, int benificiarytypeid, int postid, int roleid);
        Task <long>GetLevelNoFromPostId(long postid);
        
        Task<IEnumerable<ApplicationCountForDashBoard>> BindAppcountDistrictTalukaWiseForDashboard(long finYear, long districtId, long postId, string schemaName);
        IEnumerable<AdminMenu> BindMenuRoleWise(int usertypeid,long serviceid, long parentmenuid, string roleid, long postid);
        Task<IEnumerable<SelectListItem>> GetDistrict();
        Task<IEnumerable<SelectListItem>> GetFinancialYearList();
        Task<IEnumerable<SelectListItem>> GetDivision();
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId);
        Task<IEnumerable<SelectListItem>> GetSubServiceByServiceId(long serviceId);
        Task<IEnumerable<SelectListItem>> GetDistrictNamebyDivisionId(int districtId);
        Task<ServiceMaster> GetSchemeByServiceId(long ServiceId);
        //Task<ApplicationFilterModel> GetApplicationDetails(long registrationId, long serviceId, string schemaName, string tableName);
        Task<IEnumerable<EmpApplicationDetailsModel>> BOCW_SSY_GetApplicationDetailsList(int? pageNo, int pageSize,long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long postId, long serviceid, string? action, string? schemaName, long subServiceId, long isbpsyservice);

        Task<IEnumerable<EmpApplicationDetailsModel>> GLWB_GetApplicationDetailsList(int? pageNo, int pageSize, long divisionId, long talukaId, long districtId, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long postId, long serviceid, string? action, string? schemaName);

        Task<IEnumerable<EmpApplicationDetailsModel>> GLWB_Hospital_GetApplicationDetailsList(int? pageNo, int pageSize, long divisionId,   DateTime? fromDate, DateTime? toDate, int statusId, string? search, long postId, long serviceid, string? action, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GLWBHospitalApplicationDetailsClaim(int? pageNo, int pageSize, long divisionId,   DateTime? fromDate, DateTime? toDate, int statusId, string? search, long postId, long serviceid, string? action, string? schemaName);

        Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetUploadedDocumentForEmployee(int applicationId, int serviceId, string schemaName, string tableName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetGlwb_TsyUploadedDocumentForEmployee(int applicationId, int serviceId, string schemaName, string tableName);
        
        Task<IEnumerable<SelectListItem>> GetNextRole(int serviceid, int postid, long districtid, long talukaid);
        Task<EmpApplicationDetailsModel> ApproveApplication(string applicationId, string approvalstate, int toroleid, string approvalstatus, string remarks, string fromPostId, string serviceId, string topostid, string schemaname, string tablename, int beneficiarytype);
        Task<ResponseMessage> TimeScheduleHospital(HospitalScheduleTime hospitalScheduleTime);
        Task<ResponseMessage> UploadUserApplicationReport(HospitalScheduleTime hospitalScheduleTime);
        Task<IEnumerable<EmpApplicationDetailsModel>> ApproveMultipleApplication(string applicationId, string approvalstate, int toroleid, string approvalstatus, string remarks, string fromPostId, string serviceId, string topostid, string schemaname, string tablename, int beneficiarytype,DataTable dtApplicationIds, string ipAddress, string hostName);

        Task<EmpApplicationDetailsModel> IsLastLevel(int serviceId, int postId, int applicationId);
        Task<IEnumerable<SelectListItem>> GetSendBackRole(int serviceid, int postid, long districtid, long talukaid, long aplicationid, string schemaName,int beneficiarytype);
        Task<IEnumerable<SelectListItem>> GetSendBackRoleGLWb_TSY(int serviceid, int postid, long districtid, long aplicationid, string schemaName,int beneficiarytype);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetApplicationHistory(long applicationid,string schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetApplicationHistory_Glwb_Tsy_claim(long applicationid,string schemaName);
        Task<IEnumerable<HospitalScheduleTime>> GetHospitalScheduleTime(long applicationid, long serviceid);

        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
        Task<IEnumerable<CountForDashBoard>> BindCountForDashboard(long finYear, long districtId, long postId, long talukaId,long serviceId);
        Task<IEnumerable<CountForDashBoard>> BindCountForDashboardGLWB_tsy(long finYear, long districtId, long postId, long serviceId);
        Task<IEnumerable<CountForDashBoard>> BindAdminDashBoardData(long hodid, long districtId, long postId, long talukaId, long finYear);
        Task<IEnumerable<CountForDashBoard>> BindAdminGridData(long finYear, long hodid, long districtId, long postId, long talukaId);
        Task<IEnumerable<RegisteredApplicant>> ViewRegisteredApplicant(long hodId, DateTime? fromDate, DateTime? toDate, long districtId, int? pageNo, int pageSize, string? search);
        Task<ResponseMessage> ResetPassword(long registrationId, long userId, string password, string ipAddress, string hostName);
        Task<ResponseMessage> DeleteRegistration(long registrationId, long userId, long beneficiaryTypeId, string ipAddress, string hostName);
        Task<ResponseMessage> UpdateCitizenMobileNo(long registrationId, long userId, string mobileNo);
        Task<ApplicationStatusModel> ViewApplicationStatus(string applicationno, DateTime? dob);
        Task<IEnumerable<ApplicationStatusDetails>> GetRoleForApplicationStatus(string applicationno, DateTime? dob);
        Task<IEnumerable<CompletedApplicationDetailsModel>> ViewTotalApplicationDetails(long hodId, DateTime? fromDate, DateTime? toDate, long serviceId,long isApproved, int? pageNo, int pageSize, string? search);
        Task<IEnumerable<SelectListItem>> GetServiceMasterByBeneficiaryId(int beneficiarytypeid);
        Task<ApplicationStatusModel> ViewApplicationEmployeeSearch(string applicationno, int hodid);
        Task<IEnumerable<ApplicationStatusDetails>> GetRoleForApplicationStatusForEmployee(string applicationno, int hodid);
        Task<IEnumerable<EmpApplicationDetailsModel>> BOCWCompletedApplicationForPayment(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate, string? search, long postId, long serviceid, string? action, string? schemaName);

        Task<IEnumerable<EmpApplicationDetailsModel>> GetAppDetailsByAppIdForViewAadesh(string? applicationids, string? schemaName);

        Task<IEnumerable<EmpApplicationDetailsModel>> BOCWViewAadeshDetails(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long serviceid, string? schemaName);

        Task<IEnumerable<EmpApplicationDetailsModel>> GetAadeshDetailsbyAadeshid(long aadeshId, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> BOCWSendForPayment(DataTable dtAadeshIds,long serviceId, string schemaName,long postId, string ipAddress, string hostName);

        Task<IEnumerable<EmpApplicationDetailsModel>> GetAppDetailsByAppIdForGLWBViewAadesh(string? applicationids, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GLWBViewAadeshDetails(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long serviceid, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> Glwb_Tsy_ViewAadeshDetails(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long serviceid, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetGLWBAadeshDetailsbyAadeshid(long aadeshId, string? schemaName,long applicationId);
        Task<IEnumerable<EmpApplicationDetailsModel>> GetGlwbTSYAadeshDetailsbyAadeshid(long aadeshId, string? schemaName,long applicationId);
       // Task<IEnumerable<PaymentDetailsModel>> GetBOCWPaymentHistory(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate, string? search, long serviceid);
        Task<ResponseMessage> AddDocumentForBOCW_ACSY(DocumentDetails.DocumentFileDetails model);
        Task <DocumentDetails.DocumentFileDetails>getemployeeuploadeddocumentsbyappid(long applicationid);
        Task <HospitalScheduleTime> gethospitaluploadeddocumentsbyappid(long applicationid);
        Task<IEnumerable<PaymentDetailsModel>> GetBOCWPaymentHistory(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate, string? search, long serviceid, int statusId);

        Task<IEnumerable<EmpApplicationDetailsModel>> GLWBSendForPayment(DataTable dtAadeshIds, long serviceId, string schemaName, long postId, string ipAddress, string hostName);

        Task<IEnumerable<PaymentDetailsModel>> GetGLWBPaymentHistory(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, string? search, long serviceid, int statusId);
        Task<ResponseMessage> UpdateBOCWBankDetails(long applicationId, long serviceId, string? bankaccountNo, string? IFSCCode,string Remarks, long userId,string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> BOCWSendForPaymentFailedRecord(DataTable dtPayInfoIds, long serviceId, string schemaName, long postId, string ipAddress, string hostName);

        Task<ResponseMessage> UpdateGLWBBankDetails(long applicationId, long serviceId, string? bankaccountNo, string? IFSCCode, string Remarks, long userId, string? schemaName);
        Task<IEnumerable<EmpApplicationDetailsModel>> GLWBSendForPaymentFailedRecord(DataTable dtPayInfoIds, long serviceId, string schemaName, long postId, string ipAddress, string hostName);
    }
}
