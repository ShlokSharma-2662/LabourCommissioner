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
    public class CMDashboardRepository : BaseRepository<CCApplicationDetails>, ICMDashboardRepository
    {
        public IConfiguration appConfig;

        public CMDashboardRepository(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }
        public async Task<IEnumerable<SelectListItem>> GetServiceMasterByBeneficiaryIdforCMD(int beneficiarytypeid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_beneficiarytypeid", beneficiarytypeid);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetServiceMasterByBeneficiaryIdforCMD, queryParameters, commandType: CommandType.StoredProcedure));
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
        public async Task<IEnumerable<SelectListItem>> GetYear()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetYear, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetMonth()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetMonth, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<CMDApplicationDetails>> GetCMDApplicationDetailslist(long appYear, long appMonth, long beneficiarytypeid, int statusId)
        {
            try
            {
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_appyear", appYear);
                    queryParameters.Add("in_appmonth", appMonth);
                    queryParameters.Add("in_beneficiarytypeid", beneficiarytypeid);
                    queryParameters.Add("in_status", statusId);
                    var result = (await conn.QueryAsync<CMDApplicationDetails>(Procedures.GetCMDApplicationDetailslist, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CMDApplicationDetails> GetCMDApplicationDetailsForInsert(long applicationId, long appYear, long appMonth, long serviceId)
        {
            try
            {
                CMDApplicationDetails objCMDApplicationDetails = new CMDApplicationDetails();
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", applicationId);
                    queryParameters.Add("in_appyear", appYear);
                    queryParameters.Add("in_appmonth", appMonth);
                    queryParameters.Add("in_serviceid", serviceId);
                    var result = (await conn.QueryAsync<CMDApplicationDetailsList>(Procedures.GetCMDApplicationDetailsForInsert, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    if (result != null && result.Count() > 0)
                    {
                        objCMDApplicationDetails.appyear = result[0].appyear;
                        objCMDApplicationDetails.appmonth = result[0].appmonth;
                        objCMDApplicationDetails.serviceid = result[0].serviceid;
                        objCMDApplicationDetails.lstCMDApplicationDetails = result.Select(m => new CMDApplicationDetailsList
                        {
                            srno = m.srno,
                            totalpagecount = m.totalpagecount,
                            districtname = m.districtname,
                            districtnameguj = m.districtnameguj,
                            //serviceid = m.serviceid,
                            applicationid = m.applicationid,
                            appyear = m.appyear,
                            appmonth = m.appmonth,
                            districtid = m.districtid,
                            fintarget = m.fintarget,
                            phytarget = m.phytarget,
                            phyachievement = m.phyachievement,
                            finachievement = m.finachievement,
                            appreceived = m.appreceived,
                            appsanction = m.appsanction,
                            appreject = m.appreject,
                            apppending = m.apppending,
                            appdaypending = m.appdaypending,
                            dcode = m.dcode,
                            asondate = m.asondate,
                            issubmitted = m.issubmitted
                        }).ToList();

                    }
                    else
                    {
                        return null;
                    }
                    return objCMDApplicationDetails;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CMDAPIApplicationDetails> GetBOCWCMDApplicationDetails(long appYear, long appMonth, long serviceId)
        {
            try
            {
                CMDAPIApplicationDetails objCMDApplicationDetails = new CMDAPIApplicationDetails();
                using (var conn = GetConnection())
                {

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_appyear", appYear);
                    queryParameters.Add("in_appmonth", appMonth);
                    queryParameters.Add("in_serviceid", serviceId);
                    var result = (await conn.QueryAsync<CMDAPIApplicationDetailsList>(Procedures.GetBOCWCMDApplicationDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    if (result != null && result.Count() > 0)
                    {
                        objCMDApplicationDetails.appyear = result[0].appyear;
                        objCMDApplicationDetails.appmonth = result[0].appmonth;
                        objCMDApplicationDetails.serviceid = result[0].serviceid;
                        objCMDApplicationDetails.servicename = result[0].servicename;
                        objCMDApplicationDetails.lstCMDAPIApplicationDetails = result.Select(m => new CMDAPIApplicationDetailsList
                        {
                            srno = m.srno,
                            appyear = m.appyear,
                            appmonth = m.appmonth,
                            districtid = m.districtid,
                            districtname = m.districtname,
                            districtnameguj = m.districtnameguj,
                            serviceid = m.serviceid,
                            servicename = m.servicename,
                            fintarget = m.fintarget,
                            phytarget = m.phytarget,
                            phyachievement = m.phyachievement,
                            finachievement = m.finachievement,
                            appreceived = m.appreceived,
                            appsanction = m.appsanction,
                            appreject = m.appreject,
                            apppending = m.apppending,
                            appdaypending = m.appdaypending,
                            dcode = m.dcode,
                            asondate = m.asondate,
                            issubmitted = m.issubmitted
                        }).ToList();

                    }
                    else
                    {
                        return null;
                    }
                    return objCMDApplicationDetails;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> AddUpdateCMDApplication(DataTable dtData, CMDApplicationDetails cmdApplicationDetails)
        {

            try
            {
                var dsdsd = EnumLookup.DataTableToJson(dtData);
                using (var conn = GetConnection())
                {
                    var procName = "CALL cmdashboard.addupdatecmdapplication(@in_applicationtbl,@in_userid,@in_appyear,@in_appmonth,@in_serviceid,@in_ipaddress,@in_hostname,@in_applicationfrom," +
                        "@out_error,@out_msg,@out_applicationid,@out_id)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_applicationtbl", new JsonParameter(JsonConvert.SerializeObject(dtData)));
                    queryParameters.Add("@in_userid", cmdApplicationDetails.userId);
                    queryParameters.Add("@in_appyear", cmdApplicationDetails.appyear);
                    queryParameters.Add("@in_appmonth", cmdApplicationDetails.appmonth);
                    queryParameters.Add("@in_serviceid", cmdApplicationDetails.serviceid);
                    queryParameters.Add("@in_ipaddress", cmdApplicationDetails.ipaddress);
                    queryParameters.Add("@in_hostname", cmdApplicationDetails.hostname);
                    queryParameters.Add("@in_applicationfrom", cmdApplicationDetails.applicationfrom);
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

            #region OLD
            //try
            //{
            //    using (var conn = GetConnection())
            //    {
            //        conn.Open();
            //        var queryParameters = new DynamicParameters();
            //        conn.TypeMapper.MapComposite<p_in_document>("ty_documentdetails");
            //        var result = await conn.QueryFirstOrDefaultAsync<ResponseMessage>("bocw_ssy_insertdocumentdetails_withtable", new
            //        {
            //            p_in_document = new p_in_document
            //            {
            //                couchdbdocid = Convert.ToString(dtData.Rows[0]["couchdbdocid"])
            //            }
            //        }, commandType: System.Data.CommandType.StoredProcedure);
            //        return result;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
        }

        public async Task<ResponseMessage> CMDSubmitApplication(long appYear, long appMonth, long serviceId, long userId, string ipAddress, string hostName)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL cmdashboard.cmdsubmitapplication(@in_appyear,@in_appmonth,@in_serviceid,@in_userid,@in_ipaddress,@in_hostname,@out_error,@out_msg,@out_data)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_appyear", appYear);
                    queryParameters.Add("@in_appmonth", appMonth);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_userid", userId);
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
    }
}
