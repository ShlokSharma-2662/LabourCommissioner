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
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.DataRepository.Repositories
{
    public class HomeRepository : BaseRepository<Registration>, IHomeRepository
    {
        public IConfiguration appConfig;
        private readonly UserCookies cookies;
        public HomeRepository(IConfiguration config, IHttpContextAccessor _httpContextAccessor) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.cookies = new UserCookies(_httpContextAccessor);
        }
        public async Task<IEnumerable<ServiceMaster>> BindServicesUserWiseFilter(int userType, int beneficiaryType, int postId, int roleIds, int servicesubtypeid)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("_usertype", userType);
                queryParameters.Add("_benificiarytypeid", beneficiaryType);
                queryParameters.Add("_postid", postId);
                queryParameters.Add("_roleids", Convert.ToString(roleIds));
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

        public async Task<ServiceMaster> GetSchemeByServiceId(long ServiceId,long registrationid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", ServiceId);
                    queryParameters.Add("_registrationid", registrationid);
                    var result = await conn.QueryFirstOrDefaultAsync<ServiceMaster>(Procedures.GetSchemeByServiceId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IEnumerable<ServiceMaster>> GetSchemeByBeneficiaryTypeId(long beneficiaryTypeId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_beneficiarytypeid", beneficiaryTypeId);
                    var result = (await conn.QueryAsync<ServiceMaster>(Procedures.GetSchemeByBeneficiaryId, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ApplicationDetailsModel> GetServiceId()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = await conn.QueryFirstOrDefaultAsync<ApplicationDetailsModel>(Procedures.GetServiceId, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<bocw_ssy_personaldetails>> GetCitizen(int ApplicationId)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@ApplicationId", ApplicationId);
                var result = await conn.QueryAsync<bocw_ssy_personaldetails>(Procedures.GetCitizen, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> GetApplicationDetails(long registrationId, long serviceId, string schemaName, string tableName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_tblname", tableName);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<ApplicationDetailsModel>(Procedures.GetApplication, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_Claim_GetApplication(long registrationId, long serviceId, string schemaName, string tableName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_tblname", tableName);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<ApplicationDetailsModel>(Procedures.Glwb_TSY_Claim_GetApplication, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> GetBocw_TBSYApplication(long registrationId, long serviceId, string schemaName, string tableName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_tblname", tableName);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<ApplicationDetailsModel>(Procedures.GetBocw_TBSYApplication, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> GetGLWB_HTYApplicationDetailsForClaim(long registrationId, long serviceId, string schemaName, string tableName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_tblname", tableName);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<ApplicationDetailsModel>(Procedures.GetGLWB_HTYApplication, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ApplicationDetailsModel>> Bocw_Tbsy_GetApplication(long registrationId, long serviceId, string schemaName, string tableName,long usertype)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_tblname", tableName);
                    queryParameters.Add("@in_schemaname", schemaName);
                    queryParameters.Add("@in_usertype", usertype);
                    var result = (await conn.QueryAsync<ApplicationDetailsModel>(Procedures.Bocw_Tbsy_GetApplication, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_GetApplication(long registrationId, long serviceId, string schemaName, string tableName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_tblname", tableName);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<ApplicationDetailsModel>(Procedures.Glwb_TSY_GetApplication, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_Hospital_GetApplication(long registrationId, long serviceId, string schemaName, string tableName)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", registrationId);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_tblname", tableName);
                    queryParameters.Add("@in_schemaname", schemaName);
                    var result = (await conn.QueryAsync<ApplicationDetailsModel>(Procedures.Glwb_TSY_Hospital_GetApplication, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ResponseMessage> GetApplicationCountByRegistrationIdAndServiceId(long registrationid, long serviceId)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@in_registrationid", Convert.ToInt64(registrationid));
                queryParameters.Add("@in_serviceid", Convert.ToInt64(serviceId));
                var result = (await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.GetApplicationCountByRegIdAndServiceId, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
        }
        public async Task<ResponseMessage> AddExceptionLog(ExceptionLogger exceptionLogger)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addexceptionlogs(@in_userid,@in_actionname,@in_controllername,@in_exceptionmessage,@in_exceptionstacktrace,@in_requesturi,@in_ipaddress,@in_hostname,@in_lineno,@in_logfrom," +
                        "@out_msg,@out_error)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_userid", exceptionLogger.UserId);
                    queryParameters.Add("@in_actionname", exceptionLogger.ActionName);
                    queryParameters.Add("@in_controllername", exceptionLogger.ControllerName);
                    queryParameters.Add("@in_exceptionmessage", exceptionLogger.ExceptionMessage);
                    queryParameters.Add("@in_exceptionstacktrace", exceptionLogger.ExceptionStackTrace);
                    queryParameters.Add("@in_requesturi", exceptionLogger.RequestURI);
                    queryParameters.Add("@in_ipaddress", exceptionLogger.IpAddress);
                    queryParameters.Add("@in_hostname", exceptionLogger.HostName);
                    queryParameters.Add("@in_lineno", exceptionLogger.LineNumber);
                    queryParameters.Add("@in_logfrom", exceptionLogger.LogFrom);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Error = queryParameters.Get<long>("@out_error");
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Usermaster> GetUserId(Usermaster usermaster)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@in_aadharcardno", usermaster.aadharcardno);
                queryParameters.Add("@in_dob", usermaster.DateofBirth);
                var result = (await conn.QueryFirstOrDefaultAsync<Usermaster>(Procedures.GetUserId, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
        }

        //public async Task<IEnumerable<ApplicationDetailsModel>> GLWB_HSC_GetApplicationDetails(long registrationId, long serviceId, string schemaName, string tableName)
        //{
        //    using (var conn = GetConnection())
        //    {

        //        var queryParameters = new DynamicParameters();
        //        queryParameters.Add("@in_registrationid", registrationId);
        //        queryParameters.Add("@in_serviceid", serviceId);
        //        queryParameters.Add("@in_tblname", tableName);
        //        queryParameters.Add("@in_schemaname", schemaName);
        //        var result = (await conn.QueryAsync<ApplicationDetailsModel>(Procedures.GLWB_HSC_GetApplication, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
        //        return result;
        //    }
        //}


        //public async Task<ServiceMaster> GetServicesByHodId(int HodId)
        //{
        //    using (var conn = GetConnection())
        //    {
        //        var queryParameters = new DynamicParameters();
        //        queryParameters.Add("@HodId", HodId);
        //        var result = await conn.QueryFirstOrDefaultAsync<ServiceMaster>(Procedures.GetServicesByHodId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
        //        return result;
        //    }
        //}
        public async Task<ResponseMessage> CheckENirmanCardExpiry(long registrationId)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@in_registrationid", Convert.ToInt64(registrationId));
                var result = (await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.CheckENirmanCardExpiry, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
        }
        public async Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@_regno", RegistrationNo);
                var result = (await conn.QueryFirstOrDefaultAsync<PersonalDetailsModel>(Procedures.GetPersonalDetailsByRegNo, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
        }
        public async Task<ResponseMessage> UpdateeNirmanCardxpiryDate(long registrationId, DateTime? iCardToDateOld, DateTime? iCardToDateNew, DateTime? iCardFromDateOld, DateTime? iCardFromDateNew)
        {
            using (var conn = GetConnection())
            {
                try
                {


                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", Convert.ToInt64(registrationId));
                    queryParameters.Add("@in_icardtodateold", iCardToDateOld);
                    queryParameters.Add("@in_icardtodatenew", iCardToDateNew);
                    queryParameters.Add("@in_icardfromdateold", iCardFromDateOld);
                    queryParameters.Add("@in_icardfromdatenew", iCardFromDateNew);
                    var result = (await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.UpdateeNirmanCardxpiryDate, queryParameters, commandType: System.Data.CommandType.StoredProcedure));
                    return result;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public async Task<ResponseMessage> getaadharcardcountbyaadharnoandserviceid(string aadharcardno, long serviceId)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_aadharno", CommonUtils.EncryptCRY(aadharcardno));
                    queryParameters.Add("in_decaadharno", aadharcardno);
                    queryParameters.Add("in_serviceid", serviceId);

                    var result = await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.getaadharcardcountbyaadharnoandserviceid, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseMessage> UpdateGLWBUserCompany(PersonalDetailsModel personalDetailsModel)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.updateglwbusercompany(@in_registrationid,@in_lwbaccountno,@in_organizationname,@in_organizationemail,@in_organizationaddress,@in_organizationcity,@in_organizationdistrict,@in_organizationtaluka,@in_organizationpincode,@in_organizationdistrictid," +
                        "@out_msg,@out_error,@out_id)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_registrationid", personalDetailsModel.RegistrationId);
                    queryParameters.Add("@in_lwbaccountno", personalDetailsModel.ULwbAccountNo);
                    queryParameters.Add("@in_organizationname", personalDetailsModel.UOrganizationName);
                    queryParameters.Add("@in_organizationemail", personalDetailsModel.UOrganizationEmail);
                    queryParameters.Add("@in_organizationaddress", personalDetailsModel.UOrganizationAddress);
                    queryParameters.Add("@in_organizationcity", personalDetailsModel.UOrganizationCity);
                    queryParameters.Add("@in_organizationdistrict", personalDetailsModel.UOrganizationDistrict);
                    queryParameters.Add("@in_organizationtaluka", personalDetailsModel.UOrganizationTaluka);
                    queryParameters.Add("@in_organizationpincode", personalDetailsModel.UOrganizationPincode);
                    queryParameters.Add("@in_organizationdistrictid", personalDetailsModel.Udistrict_id);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Error = queryParameters.Get<long>("@out_error");
                    res.Id = queryParameters.Get<long>("@out_id");
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

