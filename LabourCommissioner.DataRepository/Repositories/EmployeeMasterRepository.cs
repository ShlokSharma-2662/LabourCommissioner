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
    public class EmployeeMasterRepository : BaseRepository<Registration>, IEmployeeMasterRepository
    {
        public IConfiguration appConfig;
        private readonly UserCookies cookies;
        private readonly ClaimsPrincipal _claimPincipal;
        public EmployeeMasterRepository(IConfiguration config, IHttpContextAccessor _httpContextAccessor) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.cookies = new UserCookies(_httpContextAccessor);
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
        }

        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetDistrictForMaster, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetRole(long districtid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtid);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetRole, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<PostMaster>> GetPostMasterData(long postId, long districtId, long talukaid, bool isactive)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_postid", postId);
                    queryParameters.Add("_isactive", isactive);
                    queryParameters.Add("_districtid", districtId);
                    queryParameters.Add("_talukaid", talukaid);
                    queryParameters.Add("_isurban", 0);
                    queryParameters.Add("_userpostid", 0);
                    var result = (await conn.QueryAsync<PostMaster>(Procedures.getpostmasterdata, queryParameters, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseMessage> AddUpdateDeletePost(long districtId, long postid, long roleId, string postshortname, string postname, string password, string emailid, string contactno, bool isActive, string action)
        {
            string ipAddress = CommonUtils.GetLocalIPAddress();
            string hostName = CommonUtils.GetHostName();
            try
            {

                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addupdatedeletepost(@in_postid,@in_districtid,@in_postname,@in_username,@in_password,@in_roleid,@in_emailid,@in_contactno,@in_isactive,@in_action,@in_hostname,@in_ipaddress,@out_error,@out_msg,@out_id,@out_applicatinno)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_postid", postid);
                    queryParameters.Add("@in_districtid", districtId);
                    queryParameters.Add("@in_postname", postname);
                    queryParameters.Add("@in_username", postshortname);
                    queryParameters.Add("@in_password", password);
                    queryParameters.Add("@in_roleid", roleId);
                    queryParameters.Add("@in_emailid", emailid);
                    queryParameters.Add("@in_contactno", contactno);
                    queryParameters.Add("@in_isactive", isActive);
                    queryParameters.Add("@in_action", action);
                    queryParameters.Add("@in_hostname", hostName);
                    queryParameters.Add("@in_ipaddress", ipAddress);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_data", "", direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Error = queryParameters.Get<Int64>("@out_error");
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PostMaster> GetPostData(long postid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_postid", postid);
                    var result = (await conn.QueryFirstOrDefaultAsync<PostMaster>(Procedures.GetPostData, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Menumaster>> GetMenuMasterData(Menumaster menumaster)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_isactive", menumaster.IsActive);
                    queryParameters.Add("_menuid", menumaster.MenuId);
                    queryParameters.Add("_serviceid", menumaster.ServiceId);
                    queryParameters.Add("_usertypeid", menumaster.UserTypeId);
                    var result = (await conn.QueryAsync<Menumaster>(Procedures.getmenumasterdata, queryParameters, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SelectListItem>> bindservicemaster(int beneficiarytypeid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_beneficiarytypeid", beneficiarytypeid);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetServiceMasterByBeneficiaryId, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}


