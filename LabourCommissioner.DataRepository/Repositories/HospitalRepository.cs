using Dapper;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using LabourCommissioner.Abstraction;
using Newtonsoft.Json;
using Dapper;


namespace LabourCommissioner.DataRepository.Repositories
{
    public class HospitalRepository : BaseRepository<Registration>, IHospitalRepository
    {
        public IConfiguration appConfig;
        private readonly UserCookies cookies;
        private readonly ClaimsPrincipal _claimPincipal;
        public HospitalRepository(IConfiguration config, IHttpContextAccessor _httpContextAccessor) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.cookies = new UserCookies(_httpContextAccessor);
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
        }
        public async Task<IEnumerable<ServiceMaster>> BindServicesEmployeeWiseFilter(int usertype, int benificiarytypeid, int postid, int roleid)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("_usertype", usertype);
                queryParameters.Add("_benificiarytypeid", benificiarytypeid);
                queryParameters.Add("_postid", postid);
                queryParameters.Add("_roleids", Convert.ToString(roleid));
                queryParameters.Add("_servicesubtypeid", 0);
                var result = (await conn.QueryAsync<ServiceMaster>(Procedures.BindServicesUserWiseFilter, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                return result;
            }
        }

        public async Task<long> GetLevelNoFromPostId(long postid)
        {
            using (var conn = GetConnection())
            {
                try
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_postid", postid);
                    var result = (await conn.QueryFirstOrDefaultAsync<long>(Procedures.GetLevelNoFromPostId, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                    return result;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public async Task<IEnumerable<ApplicationCountForDashBoard>> BindAppcountDistrictTalukaWiseForDashboard(long finYear, long districtId, long postId, string schemaName)
        {
            try
            {


                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_finyear", finYear);
                    queryParameters.Add("in_districtid", districtId);
                    queryParameters.Add("in_postid", postId);
                    queryParameters.Add("in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<ApplicationCountForDashBoard>(Procedures.BindAppcountDistrictTalukaWiseForDashboard, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetDistrict, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetFinancialYearList()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetFinancialYearList, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetDivision()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetDivision, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtId);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetTalukaByDistrictId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetSubServiceByServiceId(long serviceId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", serviceId);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetSubServiceByServiceId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrictNamebyDivisionId(int districtId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_divisionid", districtId);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetDistrictNamebyDivisionId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ServiceMaster> GetSchemeByServiceId(long ServiceId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", ServiceId);
                    queryParameters.Add("_registrationid", 0);
                    var result = await conn.QueryFirstOrDefaultAsync<ServiceMaster>(Procedures.GetSchemeByServiceId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCW_SSY_GetApplicationDetailsList(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate,
            DateTime? toDate, int statusId, string? search, long postId, long serviceid, string? action, string? schemaName, long subServiceId)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_districtid", districtId);
                    queryParameters.Add("@in_talukaid", talukaId);
                    queryParameters.Add("@in_villageid", villageId);
                    queryParameters.Add("@in_fromdate", fromDate);
                    queryParameters.Add("@in_todate", toDate);
                    queryParameters.Add("@in_status", statusId);
                    queryParameters.Add("@in_postid", postId);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    queryParameters.Add("@in_serviceid", serviceid);
                    queryParameters.Add("@in_action", action);
                    queryParameters.Add("@in_schemaname", schemaName);
                    queryParameters.Add("@in_subserviceid", subServiceId);
                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.BOCW_GetApplicationDetailsList, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    // var result = (await conn.QueryFirstOrDefaultAsync<ApplicationFilterModel>(Procedures.BOCW_SSY_GetApplicationDetailsList, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWB_GetApplicationDetailsList(int? pageNo, int pageSize, long divisionId, long talukaId, long districtId, DateTime? fromDate,
            DateTime? toDate, int statusId, string? search, long postId, long serviceid, string? action, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_divisionid", divisionId);
                    queryParameters.Add("@in_talukaid", talukaId);
                    queryParameters.Add("@in_districtid", districtId);
                    queryParameters.Add("@in_fromdate", fromDate);
                    queryParameters.Add("@in_todate", toDate);
                    queryParameters.Add("@in_status", statusId);
                    queryParameters.Add("@in_postid", postId);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    queryParameters.Add("@in_serviceid", serviceid);
                    queryParameters.Add("@in_action", action);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GLWB_GetApplicationDetailsList, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    // var result = (await conn.QueryFirstOrDefaultAsync<ApplicationFilterModel>(Procedures.BOCW_SSY_GetApplicationDetailsList, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWB_Hospital_GetApplicationDetailsList(int? pageNo, int pageSize, long divisionId, long talukaId, long districtId, DateTime? fromDate,
            DateTime? toDate, int statusId, string? search, long postId, long serviceid, string? action, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_divisionid", divisionId);
                    queryParameters.Add("@in_talukaid", talukaId);
                    queryParameters.Add("@in_districtid", districtId);
                    queryParameters.Add("@in_fromdate", fromDate);
                    queryParameters.Add("@in_todate", toDate);
                    queryParameters.Add("@in_status", statusId);
                    queryParameters.Add("@in_postid", postId);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    queryParameters.Add("@in_serviceid", serviceid);
                    queryParameters.Add("@in_action", action);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GLWB_Hospital_GetApplicationDetailsList, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    // var result = (await conn.QueryFirstOrDefaultAsync<ApplicationFilterModel>(Procedures.BOCW_SSY_GetApplicationDetailsList, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtId);
                    queryParameters.Add("_talukaid", talukaId);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetVillageByDistrictIdAndTalukaId, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetNextRole(int serviceid, int postid, long districtid, long talukaid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", serviceid);
                    queryParameters.Add("_postid", postid);
                    queryParameters.Add("_districtid", districtid);
                    queryParameters.Add("_talukaid", talukaid);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetEmployeeRoleForApprove, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetUploadedDocumentForEmployee(int applicationId, int serviceId, string schemaName, string tableName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationid", applicationId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_schemaname", schemaName);
                    queryParameters.Add("@in_tablename", tableName);
                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GetUploadedDocumentForEmployee, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<EmpApplicationDetailsModel> ApproveApplication(string applicationId, string approvalstate, int toroleid, string approvalstatus, string remarks, string fromPostId, string serviceId, string topostid, string schemaname, string tablename, int beneficiarytype)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addapprovaldetails(@in_applicationid,@in_approvalstate,@in_toroleid,@in_applicationstatus,@in_remarks," +
                        "@in_createdbypostid,@in_createdbyempid,@in_serviceid,@in_topostid,@in_schemaname,@in_tablename,@in_beneficiarytype," +
                        "@out_error,@out_issendmail,@out_applicantname,@out_msg,@out_approvalid,@out_email,@out_mobileno)";

                    EmpApplicationDetailsModel res = new EmpApplicationDetailsModel();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_applicationid", Convert.ToInt64(applicationId));
                    queryParameters.Add("@in_approvalstate", Convert.ToInt32(approvalstate));
                    queryParameters.Add("@in_toroleid", Convert.ToInt32(toroleid));
                    queryParameters.Add("@in_applicationstatus", Convert.ToInt32(approvalstatus));
                    queryParameters.Add("@in_remarks", remarks);
                    queryParameters.Add("@in_createdbypostid", Convert.ToInt64(fromPostId));
                    queryParameters.Add("@in_createdbyempid", Convert.ToInt32(topostid));
                    queryParameters.Add("@in_serviceid", Convert.ToInt64(serviceId));
                    queryParameters.Add("@in_topostid", Convert.ToInt64(topostid));
                    queryParameters.Add("@in_schemaname", schemaname);
                    queryParameters.Add("@in_tablename", tablename);
                    queryParameters.Add("@in_beneficiarytype", beneficiarytype);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_issendmail", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicantname", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_approvalid", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_email", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_mobileno", "", direction: ParameterDirection.InputOutput);

                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<long>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.applicantname = queryParameters.Get<string>("@out_applicantname");
                    res.email = queryParameters.Get<string>("@out_email");
                    res.mobileno = queryParameters.Get<string>("@out_mobileno");
                    res.Id = queryParameters.Get<long>("@out_approvalid");
                    //res.ApplicationNo = queryParameters.Get<long>("@out_applicatinno");


                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> ApproveMultipleApplication(string applicationId, string approvalstate, int toroleid, string approvalstatus, string remarks, string fromPostId, string serviceId, string topostid, string schemaname, string tablename, int beneficiarytype, DataTable dtApplicationIds, string ipAddress, string hostName)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    //var procName = "CALL public.addmultipleapprovaldetails(@in_approveapplicationtbl,@in_applicationid,@in_approvalstate,@in_toroleid,@in_applicationstatus,@in_remarks," +
                    //    "@in_createdbypostid,@in_createdbyempid,@in_serviceid,@in_topostid,@in_schemaname,@in_tablename,@in_beneficiarytype," +
                    //    "@out_error,@out_issendmail,@out_applicantname,@out_msg,@out_approvalid,@out_email,@out_mobileno)";

                    EmpApplicationDetailsModel res = new EmpApplicationDetailsModel();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_approveapplicationtbl", new JsonParameter(JsonConvert.SerializeObject(dtApplicationIds)));
                    queryParameters.Add("@in_applicationid", Convert.ToInt64(applicationId));
                    queryParameters.Add("@in_approvalstate", Convert.ToInt32(approvalstate));
                    queryParameters.Add("@in_toroleid", Convert.ToInt32(toroleid));
                    queryParameters.Add("@in_applicationstatus", Convert.ToInt32(approvalstatus));
                    queryParameters.Add("@in_remarks", remarks);
                    queryParameters.Add("@in_createdbypostid", Convert.ToInt64(fromPostId));
                    queryParameters.Add("@in_createdbyempid", Convert.ToInt32(topostid));
                    queryParameters.Add("@in_serviceid", Convert.ToInt64(serviceId));
                    queryParameters.Add("@in_topostid", Convert.ToInt64(topostid));
                    queryParameters.Add("@in_schemaname", schemaname);
                    queryParameters.Add("@in_tablename", tablename);
                    queryParameters.Add("@in_beneficiarytype", beneficiarytype);
                    queryParameters.Add("@in_ipaddress", ipAddress);
                    queryParameters.Add("@in_hostname", hostName);
                    //queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    //queryParameters.Add("@out_issendmail", 0, direction: ParameterDirection.InputOutput);
                    //queryParameters.Add("@out_applicantname", "", direction: ParameterDirection.InputOutput);
                    //queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    //queryParameters.Add("@out_approvalid", 0, direction: ParameterDirection.InputOutput);
                    //queryParameters.Add("@out_email", "", direction: ParameterDirection.InputOutput);
                    //queryParameters.Add("@out_mobileno", "", direction: ParameterDirection.InputOutput);

                    //var result = conn.Execute(procName, queryParameters);
                    //res.Error = queryParameters.Get<long>("@out_error");
                    //res.Msg = queryParameters.Get<string>("@out_msg");
                    //res.applicantname = queryParameters.Get<string>("@out_applicantname");
                    //res.email = queryParameters.Get<string>("@out_email");
                    //res.mobileno = queryParameters.Get<string>("@out_mobileno");
                    //res.Id = queryParameters.Get<long>("@out_approvalid");
                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.AddMultipleApprovalDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EmpApplicationDetailsModel> IsLastLevel(int serviceid, int postid, int applicationid)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_serviceid", serviceid);
                    queryParameters.Add("@in_postid", postid);
                    queryParameters.Add("@in_applicationid", applicationid);
                    queryParameters.Add("@in_schemaname", nameof(EnumLookup.schemaname.bocw_ssy));
                    queryParameters.Add("@in_tablename", nameof(EnumLookup.tablename.personaldetails));
                    var result = (await conn.QueryFirstOrDefaultAsync<EmpApplicationDetailsModel>(Procedures.GetLastLevel, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetSendBackRole(int serviceid, int postid, long districtid, long talukaid, long aplicationid, string schemaName, int beneficiarytype)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", serviceid);
                    queryParameters.Add("_postid", postid);
                    queryParameters.Add("_districtid", districtid);
                    queryParameters.Add("_talukaid", talukaid);
                    queryParameters.Add("_applicationid", aplicationid);
                    queryParameters.Add("_schemaname", schemaName);
                    queryParameters.Add("_beneficiarytype", beneficiarytype);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetEmployeeRoleForSendBack, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetApplicationHistory(long applicationid, string schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationid", applicationid);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GetApplicationHistory, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_serviceid", serviceId);
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_smstype", SMSType);
                    queryParameters.Add("in_schemaname", schemaname);
                    queryParameters.Add("in_tablename", tablename);

                    var result = await conn.QueryFirstOrDefaultAsync<SMSModel>(Procedures.GetSmsContentForService, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addsmslogs(@in_mobileno,@in_serviceid,@in_smscontent,@in_userid,@out_msg)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_mobileno", mobileNo);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_smscontent", smsContent);
                    queryParameters.Add("@in_userid", userId);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CountForDashBoard>> BindCountForDashboard(long finYear, long districtId, long talukaId, long postId, long serviceId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_finyear", finYear);
                    queryParameters.Add("in_districtid", districtId);
                    queryParameters.Add("in_talukaid", talukaId);
                    queryParameters.Add("in_postid", postId);
                    queryParameters.Add("in_serviceid", serviceId);
                    var result = (await conn.QueryAsync<CountForDashBoard>(Procedures.BindAppCountForDashboard, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CountForDashBoard>> BindAdminDashBoardData(long hodid, long districtId, long talukaId, long postId, long finYear)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_hodid", hodid);
                    queryParameters.Add("in_districtid", districtId);
                    queryParameters.Add("in_talukaid", talukaId);
                    queryParameters.Add("in_postid", postId);
                    queryParameters.Add("in_finyear", finYear);
                    var result = (await conn.QueryAsync<CountForDashBoard>(Procedures.BindAdminDashBoardData, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CountForDashBoard>> BindAdminGridData(long finYear, long hodid, long districtId, long talukaId, long postId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_hodid", hodid);
                    queryParameters.Add("in_finyear", finYear);
                    queryParameters.Add("in_districtid", districtId);
                    queryParameters.Add("in_talukaid", talukaId);
                    queryParameters.Add("in_postid", postId);
                    var result = (await conn.QueryAsync<CountForDashBoard>(Procedures.BindAppCountSchemeWiseForDashboard, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<AdminMenu> BindMenuRoleWise(int usertypeId, long serviceId, long parentmenuId, string roleId, long postId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_usertypeid", usertypeId);
                    queryParameters.Add("_serviceid", serviceId);
                    queryParameters.Add("_parentmenuid", parentmenuId);
                    queryParameters.Add("_roleids", roleId);
                    queryParameters.Add("_postid", postId);
                    var result = (conn.Query<AdminMenu>(Procedures.BindMenuRoleWise, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<RegisteredApplicant>> ViewRegisteredApplicant(long hodId, DateTime? fromDate, DateTime? toDate, long districtId, int? pageNo, int pageSize, string? search)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_hodid", hodId);
                    queryParameters.Add("@in_fromdate", fromDate == null ? DateTime.MinValue : fromDate);
                    queryParameters.Add("@in_todate", toDate == null ? DateTime.Now : toDate);
                    queryParameters.Add("@in_districtid", districtId);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    var result = (await conn.QueryAsync<RegisteredApplicant>(Procedures.GetRegisteredApplicantDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public async Task<ResponseMessage> ResetPassword(long registrationId, long userId, string password, string ipAddress, string hostName)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.citizenresetpassword(@in_registrationid,@in_userid,@in_password,@in_ipaddress,@in_hostname,@out_error,@out_msg,@out_data)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_userid", userId);
                    queryParameters.Add("@in_password", password);
                    queryParameters.Add("@in_ipaddress", ipAddress);
                    queryParameters.Add("@in_hostname", hostName);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_data", "", direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Error = queryParameters.Get<Int64>("@out_error");
                    res.email = queryParameters.Get<string>("@out_data");

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> UpdateCitizenMobileNo(long registrationId, long userId, string mobileNo)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.updatecitizenmobileno(@in_registrationid,@in_userid,@in_mobileno,@out_error,@out_msg,@out_data)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_userid", userId);
                    queryParameters.Add("@in_mobileno", mobileNo);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_data", "", direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Error = queryParameters.Get<Int64>("@out_error");
                    res.email = queryParameters.Get<string>("@out_data");

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApplicationStatusModel> ViewApplicationStatus(string applicationno, DateTime? dob)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationno", applicationno);
                    queryParameters.Add("@in_dateofbirth", dob);
                    var result = await conn.QueryFirstOrDefaultAsync<ApplicationStatusModel>(Procedures.ViewApplicationStatusNew, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationStatusDetails>> GetRoleForApplicationStatus(string applicationno, DateTime? dob)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationno", applicationno);
                    queryParameters.Add("@in_dateofbirth", dob);

                    var result = (await conn.QueryAsync<ApplicationStatusDetails>(Procedures.GetRoleForApplicationStatus, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<CompletedApplicationDetailsModel>> ViewTotalApplicationDetails(long hodId, DateTime? fromDate, DateTime? toDate, long serviceId, long isApproved, int? pageNo, int pageSize, string? search)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_hodid", hodId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_isapproved", isApproved);
                    queryParameters.Add("@in_fromdate", fromDate == null ? DateTime.MinValue : fromDate);
                    queryParameters.Add("@in_todate", toDate == null ? DateTime.Now : toDate);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    var result = (await conn.QueryAsync<CompletedApplicationDetailsModel>(Procedures.GetTotalApplicationDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public async Task<IEnumerable<SelectListItem>> GetServiceMasterByBeneficiaryId(int beneficiarytypeid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_beneficiarytypeid", beneficiarytypeid);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetServiceMasterByBeneficiaryId, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ApplicationStatusModel> ViewApplicationEmployeeSearch(string applicationno, int hodid)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationno", applicationno);
                    queryParameters.Add("@in_hodid", hodid);
                    var result = await conn.QueryFirstOrDefaultAsync<ApplicationStatusModel>(Procedures.ViewApplicationEmployeeSearch, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<ApplicationStatusDetails>> GetRoleForApplicationStatusForEmployee(string applicationno, int hodid)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationno", applicationno);
                    queryParameters.Add("@in_hodid", hodid);

                    var result = (await conn.QueryAsync<ApplicationStatusDetails>(Procedures.GetRoleForApplicationStatusForEmployee, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCWCompletedApplicationForPayment(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate, DateTime? toDate, string? search, long postId, long serviceid, string? action, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_districtid", districtId);
                    queryParameters.Add("@in_talukaid", talukaId);
                    queryParameters.Add("@in_villageid", villageId);
                    queryParameters.Add("@in_fromdate", fromDate);
                    queryParameters.Add("@in_todate", toDate);
                    queryParameters.Add("@in_postid", postId);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    queryParameters.Add("@in_serviceid", serviceid);
                    queryParameters.Add("@in_action", action);
                    queryParameters.Add("@in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.BOCWGetCompletedApplicationForPayment, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetAppDetailsByAppIdForViewAadesh(string? applicationids, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationids", applicationids);
                    queryParameters.Add("@in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GetAppDetailsByAppIdForViewAadesh, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCWViewAadeshDetails(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long serviceid, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_districtid", districtId);
                    queryParameters.Add("@in_fromdate", fromDate);
                    queryParameters.Add("@in_todate", toDate);
                    queryParameters.Add("@in_status", statusId);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    queryParameters.Add("@in_serviceid", serviceid);
                    queryParameters.Add("@in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.BOCWViewAadeshDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetAadeshDetailsbyAadeshid(long aadeshId, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_aadeshid", aadeshId);
                    queryParameters.Add("@in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GetAadeshDetailsbyAadeshid, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<EmpApplicationDetailsModel>> BOCWSendForPayment(DataTable dtAadeshIds, long serviceId, string schemaName, long postId, string ipAddress, string hostName)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    //var procName = "CALL public.addmultipleapprovaldetails(@in_approveapplicationtbl,@in_applicationid,@in_approvalstate,@in_toroleid,@in_applicationstatus,@in_remarks," +
                    //    "@in_createdbypostid,@in_createdbyempid,@in_serviceid,@in_topostid,@in_schemaname,@in_tablename,@in_beneficiarytype," +
                    //    "@out_error,@out_issendmail,@out_applicantname,@out_msg,@out_approvalid,@out_email,@out_mobileno)";

                    EmpApplicationDetailsModel res = new EmpApplicationDetailsModel();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_aadeshtbl", new JsonParameter(JsonConvert.SerializeObject(dtAadeshIds)));
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_schemaname", Convert.ToString(schemaName));
                    queryParameters.Add("@in_postid", Convert.ToInt64(postId));
                    queryParameters.Add("@in_ipaddress", ipAddress);
                    queryParameters.Add("@in_hostname", hostName);
                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.BOCWSendForPayment, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetAppDetailsByAppIdForGLWBViewAadesh(string? applicationids, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationids", applicationids);
                    queryParameters.Add("@in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GetAppDetailsByAppIdForGLWBViewAadesh, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GLWBViewAadeshDetails(int? pageNo, int pageSize, DateTime? fromDate, DateTime? toDate, int statusId, string? search, long serviceid, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_fromdate", fromDate);
                    queryParameters.Add("@in_todate", toDate);
                    queryParameters.Add("@in_status", statusId);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    queryParameters.Add("@in_serviceid", serviceid);
                    queryParameters.Add("@in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GLWBViewAadeshDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<EmpApplicationDetailsModel>> GetGLWBAadeshDetailsbyAadeshid(long aadeshId, string? schemaName, long applicationId)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_aadeshid", aadeshId);
                    queryParameters.Add("@in_schemaname", schemaName);
                    queryParameters.Add("@in_applicationid", applicationId);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GetGLWBAadeshDetailsbyAadeshid, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<PaymentDetailsModel>> GetBOCWPaymentHistory(int? pageNo, int pageSize, long districtId, DateTime? fromDate, DateTime? toDate, string? search, long serviceid, int statusId)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_districtid", districtId);
                    queryParameters.Add("@in_fromdate", fromDate);
                    queryParameters.Add("@in_todate", toDate);
                    queryParameters.Add("@in_pageno", pageNo);
                    queryParameters.Add("@in_pagesize", pageSize);
                    queryParameters.Add("@in_search", search);
                    queryParameters.Add("@in_serviceid", serviceid);
                    queryParameters.Add("@in_statusid", statusId);

                    var result = (await conn.QueryAsync<PaymentDetailsModel>(Procedures.GetBOCWPaymentHistory, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //  
        public async Task<ResponseMessage> AddDocumentForBOCW_ACSY(DocumentDetails.DocumentFileDetails model)
        {

            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.bocw_acsy_insertemployeedocumentdetails(@in_applicationid,@in_documentmasterid,@in_documentnamename,@in_createdby," +
                        "@in_ipaddress,@in_hostname,@in_couchdbdocid,@in_couchdbdocrevid," +
                        "@in_schemaname,@in_tablename,@out_error,@out_msg,@out_id)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_applicationid", model.ApplicationId);
                    queryParameters.Add("@in_documentmasterid", model.DocumentMasterId);
                    queryParameters.Add("@in_documentnamename", model.DocumentName);
                    queryParameters.Add("@in_createdby", model.CreatedBy);
                    queryParameters.Add("@in_ipaddress", model.IpAddress);
                    queryParameters.Add("@in_hostname", model.HostName);
                    queryParameters.Add("@in_couchdbdocid", model.CouchDBDocId);
                    queryParameters.Add("@in_couchdbdocrevid", model.CouchDBDocRevId);
                    queryParameters.Add("@in_schemaname", model.schemaname);
                    queryParameters.Add("@in_tablename", model.tablename);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    //queryParameters.Add("@out_applicationid", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);

                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<Int32>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    //res.ApplicationNo = queryParameters.Get<long>("@out_applicationid");
                    res.Id = queryParameters.Get<Int32>("@out_id");

                    return res;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DocumentDetails.DocumentFileDetails> getemployeeuploadeddocumentsbyappid(long applicationid)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationid", applicationid);
                    var result = await conn.QueryFirstOrDefaultAsync<DocumentDetails.DocumentFileDetails>(Procedures.getemployeeuploadeddocumentsbyappid, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}


