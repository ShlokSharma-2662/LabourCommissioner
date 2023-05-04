using Dapper;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.DataRepository.Repositories
{
    public class CCRegistrationRepository : BaseRepository<CCRegistration>, ICCRegistrationRepository
    {
        public IConfiguration appConfig;

        public CCRegistrationRepository(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IEnumerable<SelectListItem>> GetCCUserType(string ResourceType)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_resourcetype", ResourceType);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.BindResourceValue, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UserAlreadyExist(string? PANTANNo)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL cesscollection.cessuseralreadyexist(@in_pantanno,@out_msg)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_pantanno", PANTANNo);
                    queryParameters.Add("@out_msg", false, direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    bool isExist = queryParameters.Get<bool>("@out_msg");
                    return isExist;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> AddUpdateRegistration(CCRegistration registration)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var procName = "CALL cesscollection.addregistration(@in_usertype,@in_name,@in_mobileno,@in_emailid,@in_pantanno,@in_password," +
                       "@in_ipaddress,@in_hostname,@out_msg,@out_registrationno,@out_error,@out_registrationid)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_usertype", registration.UserType);
                    queryParameters.Add("@in_name", registration.Name);
                    queryParameters.Add("@in_mobileno", registration.MobileNo);
                    queryParameters.Add("@in_emailid", registration.EmailId);
                    queryParameters.Add("@in_pantanno", registration.PANTANNo);
                    queryParameters.Add("@in_password", registration.Password);
                    queryParameters.Add("@in_ipaddress", registration.ipaddress);
                    queryParameters.Add("@in_hostname", registration.hostname);
                    queryParameters.Add("@out_msg", " ", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_registrationno", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_registrationid", 0, direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Error = queryParameters.Get<long>("@out_error");
                    res.Id = queryParameters.Get<long>("@out_registrationno");
                    res.registrationId = queryParameters.Get<long>("@out_registrationid");

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseMessage> AddUpdateAuthorityDetails(CCRegistration registration)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var procName = "CALL cesscollection.addauthoritydetails(@in_registrationid,@in_resname,@in_resdesignation,@in_resmobileno," +
                       "@in_locname,@in_locdesignation,@in_locmobileno,@in_ipaddress,@in_hostname,@out_msg,@out_error,@out_registrationid)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registration.RegistrationId);
                    queryParameters.Add("@in_resname", registration.resname);
                    queryParameters.Add("@in_resdesignation", registration.resdesignation);
                    queryParameters.Add("@in_resmobileno", registration.resmobileno);
                    queryParameters.Add("@in_locname", registration.locname);
                    queryParameters.Add("@in_locdesignation", registration.locdesignation);
                    queryParameters.Add("@in_locmobileno", registration.locmobileno);
                    queryParameters.Add("@in_ipaddress", registration.ipaddress);
                    queryParameters.Add("@in_hostname", registration.hostname);
                    queryParameters.Add("@out_msg", " ", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_registrationid", 0, direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Error = queryParameters.Get<long>("@out_error");
                    res.registrationId = queryParameters.Get<long>("@out_registrationid");

                    return res;
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
    }
}
