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

namespace LabourCommissioner.DataRepository.Repositories
{
    public class ServiceRoutineRepository : BaseRepository<Registration>, IServiceRoutineRepository
    {
        public IConfiguration appConfig;
        private readonly UserCookies cookies;
        private readonly ClaimsPrincipal _claimPincipal;
        public ServiceRoutineRepository(IConfiguration config, IHttpContextAccessor _httpContextAccessor) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.cookies = new UserCookies(_httpContextAccessor);
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> BOCWGetAadeshDataForRoutine(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_fromdate", fromDate);
                    queryParameters.Add("@in_todate", toDate);
                    var result = (await conn.QueryAsync<AadeshPaymentDetailsModel>(Procedures.BOCWGetAadeshDataForRoutine, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> UpdateBOCWPaymentInfo(string payinfoids, string filename, int confirmuploadedstatus, int verifiedstatus)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_payinfoids", payinfoids);
                    queryParameters.Add("@in_filename", filename);
                    queryParameters.Add("@in_confirmuploadedstatus", confirmuploadedstatus);
                    queryParameters.Add("@in_verifiedstatus", verifiedstatus);

                    var result = (await conn.QueryAsync<AadeshPaymentDetailsModel>(Procedures.UpdateBOCWPaymentInfo, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> BOCWGetAadeshDataForFetchReturnCSVFile()
        {
            using (var conn = GetConnection())
            {
                var result = (await conn.QueryAsync<AadeshPaymentDetailsModel>(Procedures.BOCWGetAadeshDataForFetchReturnCSVFile, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                return result;
            }
        }
        public async Task<ResponseMessage> SaveBOCWPaymentResponse(DataTable dtData, string? IpAddress, string? HostName)
        {
            try
            {
                var dsdsd = EnumLookup.DataTableToJson(dtData);
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.savebocwpaymentresponse(@in_paymentresponsetbl,@in_ipaddress,@in_hostname,@out_error,@out_msg,@out_applicationid,@out_id)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_paymentresponsetbl", new JsonParameter(JsonConvert.SerializeObject(dtData)));
                    queryParameters.Add("@in_ipaddress", IpAddress);
                    queryParameters.Add("@in_hostname", HostName);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicationid", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);

                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<Int64>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.ApplicationNo = queryParameters.Get<long>("@out_applicationid");
                    res.Id = queryParameters.Get<long>("@out_id");

                    return res;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<IEnumerable<AadeshPaymentDetailsModel>> GLWBGetAadeshDataForRoutine(DateTime? fromDate, DateTime? toDate)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new Dapper.DynamicParameters();
                queryParameters.Add("@in_fromdate", fromDate);
                queryParameters.Add("@in_todate", toDate);
                var result = (await conn.QueryAsync<AadeshPaymentDetailsModel>(Procedures.GLWBGetAadeshDataForRoutine, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                return result;
            }
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> UpdateGLWBPaymentInfo(string payinfoids, string filename, int confirmuploadedstatus, int verifiedstatus)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_payinfoids", payinfoids);
                    queryParameters.Add("@in_filename", filename);
                    queryParameters.Add("@in_confirmuploadedstatus", confirmuploadedstatus);
                    queryParameters.Add("@in_verifiedstatus", verifiedstatus);

                    var result = (await conn.QueryAsync<AadeshPaymentDetailsModel>(Procedures.UpdateGLWBPaymentInfo, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> GLWBGetAadeshDataForFetchReturnCSVFile()
        {
            using (var conn = GetConnection())
            {
                var result = (await conn.QueryAsync<AadeshPaymentDetailsModel>(Procedures.GLWBGetAadeshDataForFetchReturnCSVFile, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                return result;
            }
        }
        public async Task<ResponseMessage> SaveGLWBPaymentResponse(DataTable dtData, string? IpAddress, string? HostName)
        {
            try
            {
                var dsdsd = EnumLookup.DataTableToJson(dtData);
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.saveglwbpaymentresponse(@in_paymentresponsetbl,@in_ipaddress,@in_hostname,@out_error,@out_msg,@out_applicationid,@out_id)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_paymentresponsetbl", new JsonParameter(JsonConvert.SerializeObject(dtData)));
                    queryParameters.Add("@in_ipaddress", IpAddress);
                    queryParameters.Add("@in_hostname", HostName);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicationid", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);

                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<Int64>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.ApplicationNo = queryParameters.Get<long>("@out_applicationid");
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


