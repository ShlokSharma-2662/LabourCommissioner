using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Services.Services
{
    public class EmployeeHomeService : IEmployeeHomeService
    {
        private readonly IEmployeeHomeRepository _homeRepository;
        public EmployeeHomeService(IEmployeeHomeRepository homeRepository)
        {
            _homeRepository = homeRepository ?? throw new ArgumentNullException(nameof(homeRepository));
        }
        //public async Task<IEnumerable<ServiceMaster>> BindServicesUserWiseFilter()
        //{
        //    return await _homeRepository.BindServicesUserWiseFilter();
        //}
        public async Task<IEnumerable<ServiceMaster>> BindServicesEmployeeWiseFilter(int usertype, int benificiarytypeid, int postid, int roleid)
        {
            return await _homeRepository.BindServicesEmployeeWiseFilter(usertype, benificiarytypeid, postid, roleid);
        }

        public async Task<long> GetLevelNoFromPostId(long postid)
        {
            return await _homeRepository.GetLevelNoFromPostId(postid);
        }

        public async Task<IEnumerable<ApplicationCountForDashBoard>> BindAppcountDistrictTalukaWiseForDashboard(long finYear, long districtId, long postId, string schemaName)
        {
            return await _homeRepository.BindAppcountDistrictTalukaWiseForDashboard(finYear, districtId, postId, schemaName);
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _homeRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetFinancialYearList()
        {
            var res = await _homeRepository.GetFinancialYearList();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetDivision()
        {
            var res = await _homeRepository.GetDivision();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            var res = await _homeRepository.GetTalukaByDistrictId(districtId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetSubServiceByServiceId(long serviceId)
        {
            var res = await _homeRepository.GetSubServiceByServiceId(serviceId);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetDistrictNamebyDivisionId(int districtId)
        {
            var res = await _homeRepository.GetDistrictNamebyDivisionId(districtId);
            return res;
        }

        public async Task<ServiceMaster> GetSchemeByServiceId(long ServiceId)
        {
            return await _homeRepository.GetSchemeByServiceId(ServiceId);
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCW_SSY_GetApplicationDetailsList(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate, int statusId, string? search,
            long postId, long serviceid, string? action, string? schemaName, long subServiceId,long isbpsyservice)
        {
            return await _homeRepository.BOCW_SSY_GetApplicationDetailsList(pageNo, pageSize, districtId, talukaId, villageId, fromDate, toDate,
            statusId, search, postId, serviceid, action,schemaName, subServiceId, isbpsyservice);
        }


        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWB_GetApplicationDetailsList(int? pageNo, int pageSize, long divisionId, long talukaId, long districtId, DateTime? fromDate, DateTime? toDate, int statusId, string? search,
           long postId, long serviceid, string? action, string? schemaName)
        {
            return await _homeRepository.GLWB_GetApplicationDetailsList(pageNo, pageSize, divisionId, talukaId, districtId, fromDate, toDate,
            statusId, search, postId, serviceid, action, schemaName);
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWB_Hospital_GetApplicationDetailsList(int? pageNo, int pageSize, long divisionId,  DateTime? fromDate, DateTime? toDate, int statusId, string? search,
           long postId, long serviceid, string? action, string? schemaName)
        {
            return await _homeRepository.GLWB_Hospital_GetApplicationDetailsList(pageNo, pageSize, divisionId, fromDate, toDate,
            statusId, search, postId, serviceid, action, schemaName);
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWBHospitalApplicationDetailsClaim(int? pageNo, int pageSize, long divisionId, DateTime? fromDate, DateTime? toDate, int statusId, string? search,
          long postId, long serviceid, string? action, string? schemaName)
        {
            return await _homeRepository.GLWBHospitalApplicationDetailsClaim(pageNo, pageSize, divisionId, fromDate, toDate,
            statusId, search, postId, serviceid, action, schemaName);
        }

        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var res = await _homeRepository.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetUploadedDocumentForEmployee(int applicationId, int serviceId, string schemaName, string tableName)
        {
            var res = await _homeRepository.GetUploadedDocumentForEmployee(applicationId, serviceId, schemaName, tableName);
            return res;
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetGlwb_TsyUploadedDocumentForEmployee(int applicationId, int serviceId, string schemaName, string tableName)
        {
            var res = await _homeRepository.GetGlwb_TsyUploadedDocumentForEmployee(applicationId, serviceId, schemaName, tableName);
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetNextRole(int serviceid, int postid, long districtid, long talukaid)
        {
            var res = await _homeRepository.GetNextRole(serviceid, postid, districtid, talukaid);
            return res;
        }

        public async Task<EmpApplicationDetailsModel> IsLastLevel(int serviceid, int postid, int applicationid)
        {
            var res = await _homeRepository.IsLastLevel(serviceid, postid, applicationid);
            return res;
        }
        public async Task<ResponseMessage> TimeScheduleHospital(HospitalScheduleTime hospitalScheduleTime)
        {
            return await _homeRepository.TimeScheduleHospital(hospitalScheduleTime);
        }
        public async Task<ResponseMessage> UploadUserApplicationReport(HospitalScheduleTime hospitalScheduleTime)
        {
            return await _homeRepository.UploadUserApplicationReport(hospitalScheduleTime);
        }
        public async Task<EmpApplicationDetailsModel> ApproveApplication(string applicationId, string approvalstate, int toroleid, string approvalstatus, string remarks, string fromPostId, string serviceId, string topostid, string schemaname, string tablename, int beneficiarytype)
        {
            return await _homeRepository.ApproveApplication(applicationId, approvalstate, toroleid, approvalstatus, remarks, fromPostId, serviceId, topostid, schemaname, tablename,beneficiarytype);
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> ApproveMultipleApplication(string applicationId, string approvalstate, int toroleid, string approvalstatus, string remarks, string fromPostId, string serviceId, string topostid, string schemaname, string tablename, int beneficiarytype, DataTable dtApplicationIds, string ipAddress, string hostName)
        {
            return await _homeRepository.ApproveMultipleApplication(applicationId, approvalstate, toroleid, approvalstatus, remarks, fromPostId, serviceId, topostid, schemaname, tablename, beneficiarytype,dtApplicationIds,ipAddress,hostName);
        }


        //public async Task<ApplicationFilterModel> GetApplicationDetails(long registrationId, long serviceId, string schemaName, string tableName)
        //{
        //    return await _homeRepository.GetApplicationDetails(registrationId, serviceId, schemaName, tableName);
        //}

        public async Task<IEnumerable<SelectListItem>> GetSendBackRole(int serviceid, int postid, long districtid, long talukaid, long aplicationid,string schemaName, int beneficiarytype)
        {
            var res = await _homeRepository.GetSendBackRole(serviceid, postid, districtid, talukaid, aplicationid,schemaName,beneficiarytype);
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> GetSendBackRoleGLWb_TSY(int serviceid, int postid, long districtid, long aplicationid, string schemaName, int beneficiarytype)
        {
            var res = await _homeRepository.GetSendBackRoleGLWb_TSY(serviceid, postid, districtid, aplicationid, schemaName, beneficiarytype);
            return res;
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>>GetApplicationHistory(long applicationid, string schemaName)
        {
            var res = await _homeRepository.GetApplicationHistory(applicationid,schemaName);
            return res;
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetApplicationHistory_Glwb_Tsy_claim(long applicationid, string schemaName)
        {
            var res = await _homeRepository.GetApplicationHistory_Glwb_Tsy_claim(applicationid, schemaName);
            return res;
        }

        public async Task<IEnumerable<HospitalScheduleTime>> GetHospitalScheduleTime(long applicationid, long serviceid)
        {
            var res = await _homeRepository.GetHospitalScheduleTime(applicationid, serviceid);
            return res;
        }


        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            var res = _homeRepository.GetSmsContentForService(serviceId, ApplicationId, SMSType, schemaname, tablename);
            return await res;
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _homeRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }
        public async Task<IEnumerable<CountForDashBoard>> BindCountForDashboard(long finYear, long districtId, long postId, long talukaId, long serviceId)
        {
            var res = await _homeRepository.BindCountForDashboard(finYear,districtId,talukaId,postId,serviceId);
            return res;
        }
        public async Task<IEnumerable<CountForDashBoard>> BindCountForDashboardGLWB_tsy(long finYear, long districtId, long postId, long serviceId)
        {
            var res = await _homeRepository.BindCountForDashboardGLWB_tsy(finYear, districtId, postId, serviceId);
            return res;
        }

        public async Task<IEnumerable<CountForDashBoard>> BindAdminDashBoardData(long hodid, long districtId, long postId, long talukaId, long finYear)
        {
            var res = await _homeRepository.BindAdminDashBoardData(hodid, districtId, talukaId, postId, finYear);
            return res;
        }

        public async Task<IEnumerable<CountForDashBoard>> BindAdminGridData(long finYear, long hodid, long districtId, long postId, long talukaId)
        {
            var res = await _homeRepository.BindAdminGridData(finYear,hodid, districtId, talukaId, postId);
            return res;
        }


        public IEnumerable<AdminMenu> BindMenuRoleWise(int usertypeId, long serviceId, long parentmenuId, string roleId, long postId)
        {
            var res = _homeRepository.BindMenuRoleWise(usertypeId,serviceId,parentmenuId,roleId,postId);
            return res;
        }
        public Task<IEnumerable<RegisteredApplicant>> ViewRegisteredApplicant(long hodId, DateTime? fromDate, DateTime? toDate, long districtId, int? pageNo, int pageSize, string? search)
        {
            var res = _homeRepository.ViewRegisteredApplicant(hodId, fromDate, toDate,districtId,pageNo,pageSize, search);
            return res;
        }
        public Task<ResponseMessage> ResetPassword(long registrationId, long userId, string password, string ipAddress, string hostName)
        {
            var res = _homeRepository.ResetPassword(registrationId, userId, password, ipAddress, hostName);
            return res;
        }
        public Task<ResponseMessage> DeleteRegistration(long registrationId, long userId, long beneficiaryTypeId, string ipAddress, string hostName)
        {
            var res = _homeRepository.DeleteRegistration(registrationId, userId, beneficiaryTypeId, ipAddress, hostName);
            return res;
        }
        public Task<ResponseMessage> UpdateCitizenMobileNo(long registrationId, long userId, string mobileNo)
        {
            var res = _homeRepository.UpdateCitizenMobileNo(registrationId, userId, mobileNo);
            return res;
        }

        public Task<ApplicationStatusModel> ViewApplicationStatus(string applicationno, DateTime? dob)
        {
            var res = _homeRepository.ViewApplicationStatus(applicationno,dob);
            return res;
        }

        public Task<IEnumerable<ApplicationStatusDetails>> GetRoleForApplicationStatus(string applicationno, DateTime? dob)
        {
            var res = _homeRepository.GetRoleForApplicationStatus(applicationno, dob);
            return res;
        }

        public Task<ApplicationStatusModel> ViewApplicationEmployeeSearch(string applicationno, int hodid)
        {
            var res = _homeRepository.ViewApplicationEmployeeSearch(applicationno, hodid);
            return res;
        }

        public Task<IEnumerable<ApplicationStatusDetails>> GetRoleForApplicationStatusForEmployee(string applicationno, int hodid)
        {
            var res = _homeRepository.GetRoleForApplicationStatusForEmployee(applicationno, hodid);
            return res;
        }

        public Task<IEnumerable<CompletedApplicationDetailsModel>> ViewTotalApplicationDetails(long hodId, DateTime? fromDate, DateTime? toDate, long serviceId, long isApproved, int? pageNo, int pageSize, string? search)
        {
            var res = _homeRepository.ViewTotalApplicationDetails(hodId, fromDate, toDate, serviceId, isApproved, pageNo, pageSize, search);
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetServiceMasterByBeneficiaryId(int beneficiarytypeid)
        {
            var res = await _homeRepository.GetServiceMasterByBeneficiaryId(beneficiarytypeid);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCWCompletedApplicationForPayment(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate, string? search, long postId, long serviceid, string? action, string? schemaName)
        {
            var res = await _homeRepository.BOCWCompletedApplicationForPayment(pageNo, pageSize, districtId, talukaId, villageId, fromDate, toDate, search, postId, serviceid, action, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetAppDetailsByAppIdForViewAadesh(string? applicationids, string? schemaName)
        {
            var res = await _homeRepository.GetAppDetailsByAppIdForViewAadesh(applicationids, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetAppDetailsByAppIdForGLWBViewAadesh(string? applicationids, string? schemaName)
        {
            var res = await _homeRepository.GetAppDetailsByAppIdForGLWBViewAadesh(applicationids, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCWViewAadeshDetails(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate,int statusId, string? search, long serviceid, string? schemaName)
        {
            var res = await _homeRepository.BOCWViewAadeshDetails(pageNo, pageSize, districtId, fromDate, toDate, statusId, search, serviceid, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetAadeshDetailsbyAadeshid(long aadeshId, string? schemaName)
        {
            var res = await _homeRepository.GetAadeshDetailsbyAadeshid(aadeshId, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCWSendForPayment(DataTable dtAadeshIds,long serviceId, string schemaName, long postId, string ipAddress, string hostName)
        {
            var res = await _homeRepository.BOCWSendForPayment(dtAadeshIds, serviceId, schemaName, postId, ipAddress, hostName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWBViewAadeshDetails(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long serviceid, string? schemaName)
        {
            var res = await _homeRepository.GLWBViewAadeshDetails(pageNo, pageSize, fromDate, toDate, statusId, search, serviceid, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> Glwb_Tsy_ViewAadeshDetails(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long serviceid, string? schemaName)
        {
            var res = await _homeRepository.Glwb_Tsy_ViewAadeshDetails(pageNo, pageSize, fromDate, toDate, statusId, search, serviceid, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetGLWBAadeshDetailsbyAadeshid(long aadeshId, string? schemaName, long applicationId)
        {
            var res = await _homeRepository.GetGLWBAadeshDetailsbyAadeshid(aadeshId, schemaName,applicationId);
            return res;

        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetGlwbTSYAadeshDetailsbyAadeshid(long aadeshId, string? schemaName, long applicationId)
        {
            var res = await _homeRepository.GetGlwbTSYAadeshDetailsbyAadeshid(aadeshId, schemaName, applicationId);
            return res;
        }

        public async Task<IEnumerable<PaymentDetailsModel>> GetBOCWPaymentHistory(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate, string? search, long serviceid, int statusId)
        {
            var res = await _homeRepository.GetBOCWPaymentHistory(pageNo, pageSize,districtId,fromDate, toDate, search, serviceid,statusId);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWBSendForPayment(DataTable dtAadeshIds, long serviceId, string schemaName, long postId, string ipAddress, string hostName)
        {
            var res = await _homeRepository.GLWBSendForPayment(dtAadeshIds, serviceId, schemaName, postId, ipAddress, hostName);
            return res;
        }
        public async Task<ResponseMessage> AddDocumentForBOCW_ACSY(DocumentDetails.DocumentFileDetails model)
        {
            var res = await _homeRepository.AddDocumentForBOCW_ACSY(model);
            return res;
        }

        public async Task<DocumentDetails.DocumentFileDetails> getemployeeuploadeddocumentsbyappid(long applicationid)
        {
            var res = await _homeRepository.getemployeeuploadeddocumentsbyappid(applicationid);
            return res;
        }
        public async Task<HospitalScheduleTime> gethospitaluploadeddocumentsbyappid(long applicationid)
        {
            var res = await _homeRepository.gethospitaluploadeddocumentsbyappid(applicationid);
            return res;
        }

        public async Task<IEnumerable<PaymentDetailsModel>> GetGLWBPaymentHistory(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, string? search, long serviceid, int statusId)
        {
            var res = await _homeRepository.GetGLWBPaymentHistory(pageNo, pageSize, fromDate, toDate, search, serviceid, statusId);
            return res;
        }
        public Task<ResponseMessage> UpdateBOCWBankDetails(long applicationId, long serviceId, string? bankaccountNo, string? IFSCCode, string Remarks, long userId, string? schemaName)
        {
            var res = _homeRepository.UpdateBOCWBankDetails(applicationId, serviceId, bankaccountNo, IFSCCode,Remarks, userId, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCWSendForPaymentFailedRecord(DataTable dtApplicationIds, long serviceId, string schemaName, long postId, string ipAddress, string hostName)
        {
            var res = await _homeRepository.BOCWSendForPaymentFailedRecord(dtApplicationIds, serviceId, schemaName, postId, ipAddress, hostName);
            return res;
        }
        public Task<ResponseMessage> UpdateGLWBBankDetails(long applicationId, long serviceId, string? bankaccountNo, string? IFSCCode, string Remarks, long userId, string? schemaName)
        {
            var res = _homeRepository.UpdateGLWBBankDetails(applicationId, serviceId, bankaccountNo, IFSCCode, Remarks, userId, schemaName);
            return res;
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWBSendForPaymentFailedRecord(DataTable dtApplicationIds, long serviceId, string schemaName, long postId, string ipAddress, string hostName)
        {
            var res = await _homeRepository.GLWBSendForPaymentFailedRecord(dtApplicationIds, serviceId, schemaName, postId, ipAddress, hostName);
            return res;
        }

        #region Not Implemented Methods
        public Task<long> AddAsync(Registration entity)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(Registration entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetAllASync()
        {
            throw new NotImplementedException();
        }

        public Task<Registration> GetASync(long entityID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Registration entity)
        {
            throw new NotImplementedException();
        }

        //Task<Usermaster> IBaseService<Usermaster>.GetASync(long entityID)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<IEnumerable<Usermaster>> IBaseService<Usermaster>.GetAllASync()
        //{
        //    throw new NotImplementedException();
        //}

        public Task<long> AddAsync(Usermaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Usermaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Usermaster entity)
        {
            throw new NotImplementedException();
        }

       









        //Task<IEnumerable<Usermaster>> IBaseService<Usermaster>.GetListAsync()
        //{
        //    throw new NotImplementedException();
        //}


        #endregion
    }
}
