using Dapper;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.DataRepository.Repositories
{
    public class CCApplicationRepository : BaseRepository<CCApplicationDetails>, ICCApplicationRepository
    {
        public IConfiguration appConfig;

        public CCApplicationRepository(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IEnumerable<CCApplicationDetails>> GetCCApplicationDetails(int? pageNo, int pageSize, long districtId, long talukaId, long villageId, DateTime? fromDate,
           DateTime? toDate, int statusId, string? search)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_districtid", districtId);
                    queryParameters.Add("in_talukaid", talukaId);
                    queryParameters.Add("in_villageid", villageId);
                    queryParameters.Add("in_fromdate", fromDate);
                    queryParameters.Add("in_todate", toDate);
                    queryParameters.Add("in_status", statusId);
                    queryParameters.Add("in_pageno", pageNo);
                    queryParameters.Add("in_pagesize", pageSize);
                    queryParameters.Add("in_search", search);
                    var result = (await conn.QueryAsync<CCApplicationDetails>(Procedures.GetCCApplicationDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<IEnumerable<CCApplicationDetails>> GetCCCompletedAppForPayment(int? pageNo, int pageSize, DateTime? fromDate,DateTime? toDate, int statusId, string? search)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_fromdate", fromDate);
                    queryParameters.Add("in_todate", toDate);
                    queryParameters.Add("in_status", statusId);
                    queryParameters.Add("in_pageno", pageNo);
                    queryParameters.Add("in_pagesize", pageSize);
                    queryParameters.Add("in_search", search);
                    var result = (await conn.QueryAsync<CCApplicationDetails>(Procedures.GetCCCompletedAppForPayment, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            try
            {

                using (var conn = GetConnection())
                {

                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetAllStates, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        public async Task<ResponseMessage> AddUpdateCCApplication(CCApplicationDetails ccApplicationDetails)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var procName = "CALL cesscollection.addupdateccapplication(@in_applicationid,@in_applicationno,@in_registrationid,@in_serviceid," +
                    "@in_cesspayername,@in_dateofcollection,@in_costofconstruction,@in_cesspercentage,@in_totalcess," +
                    "@in_stateid,@in_districtid,@in_talukaid,@in_villageid,@in_pincode,@in_addressineng,@in_addressinguj,@in_ipaddress,@in_hostname," +
                    "@in_applicationfrom,@out_error,@out_msg,@out_id,@out_applicatinno)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationid", ccApplicationDetails.applicationid);
                    queryParameters.Add("@in_applicationno", ccApplicationDetails.applicationno);
                    queryParameters.Add("@in_registrationid", ccApplicationDetails.registrationid);
                    queryParameters.Add("@in_serviceid", ccApplicationDetails.serviceid);
                    queryParameters.Add("@in_cesspayername", ccApplicationDetails.cesspayername);
                    queryParameters.Add("@in_dateofcollection", ccApplicationDetails.dateofcollection);
                    queryParameters.Add("@in_costofconstruction", ccApplicationDetails.costofconstruction);
                    queryParameters.Add("@in_cesspercentage", ccApplicationDetails.cesspercentage);
                    queryParameters.Add("@in_totalcess", ccApplicationDetails.totalcess);
                    queryParameters.Add("@in_stateid", ccApplicationDetails.stateid);
                    queryParameters.Add("@in_districtid", ccApplicationDetails.districtid);
                    queryParameters.Add("@in_talukaid", ccApplicationDetails.talukaid);
                    queryParameters.Add("@in_villageid", ccApplicationDetails.villageid);
                    queryParameters.Add("@in_pincode", ccApplicationDetails.pincode);
                    queryParameters.Add("@in_addressineng", ccApplicationDetails.addressineng);
                    queryParameters.Add("@in_addressinguj", ccApplicationDetails.addressinguj);
                    queryParameters.Add("@in_ipaddress", ccApplicationDetails.ipaddress);
                    queryParameters.Add("@in_hostname", ccApplicationDetails.hostname);
                    queryParameters.Add("@in_applicationfrom", ccApplicationDetails.applicationfrom);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", " ", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<long>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Id = queryParameters.Get<long>("@out_id");
                    res.ApplicationNo= queryParameters.Get<long>("@out_applicatinno");

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CCApplicationDetails> GetTotalsahayByServiceID(int serviceId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", serviceId);
                    queryParameters.Add("_relation", 0);
                    queryParameters.Add("_gender", 0);
                    queryParameters.Add("_other1", 0);
                    queryParameters.Add("_tharavid", 0);
                    var result = (await conn.QueryAsync<CCApplicationDetails>(Procedures.GetTotalsahayByServiceID, queryParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CTPPaymentDetails>> InsertPaymentTransactionInfo(DataTable dtApplicationIds, long registrationId, DateTime? fromDate, DateTime? toDate, string ipAddress, string hostName)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", new JsonParameter(JsonConvert.SerializeObject(dtApplicationIds)));
                    queryParameters.Add("in_registrationid", registrationId);
                    queryParameters.Add("in_fromdate", fromDate);
                    queryParameters.Add("in_todate", toDate);
                    queryParameters.Add("in_ipaddress", ipAddress);
                    queryParameters.Add("in_hostname", hostName);
                    
                    var result = (await conn.QueryAsync<CTPPaymentDetails>(Procedures.InsertPaymentTransactionInfo, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public async Task<IEnumerable<CTPPaymentDetails>> CheckTransactionTokenExistorNot(long registrationId, long tokenNo, string transactionId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_registrationid", registrationId);
                    queryParameters.Add("in_tokenno", tokenNo);
                    queryParameters.Add("in_transactionid", transactionId);

                    var result = (await conn.QueryAsync<CTPPaymentDetails>(Procedures.CheckTransactionTokenExistorNot, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
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
        public async Task<CCApplicationDetails> GetApplicationDetailsByAppId(long applicationId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", applicationId);
                    var result = (await conn.QueryAsync<CCApplicationDetails>(Procedures.GetCCApplicationDetailsByAppId, queryParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<CTPPaymentDetails>> UpdatePaymentTransactionInfo(long userId, long transactionId, string regno, string bankrefno, string bankname, long dlrrefno, string cin, string amount, DateTime? paymentdate, string status, string statusdesc)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_userid", userId);
                    queryParameters.Add("in_transactionid", userId);
                    queryParameters.Add("in_regno", userId);
                    queryParameters.Add("in_bankrefno", userId);
                    queryParameters.Add("in_bankname", userId);
                    queryParameters.Add("in_dlrrefno", userId);
                    queryParameters.Add("in_cin", userId);
                    queryParameters.Add("in_amount", userId);
                    queryParameters.Add("in_paymentdate", userId);
                    queryParameters.Add("in_status", userId);
                    queryParameters.Add("in_statusdesc", userId);
                    var result = (await conn.QueryAsync<CTPPaymentDetails>(Procedures.UpdatePaymentTransactionInfo, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<CTPPaymentDetails>> GetDataForCTPMakePayment(long paymentinfoTransId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_paymentinfotransid", paymentinfoTransId);
                    var result = (await conn.QueryAsync<CTPPaymentDetails>(Procedures.GetDataForCTPMakePayment, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
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
