using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NpgsqlTypes;
using static Dapper.SqlMapper;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissioner.DataRepository.Repositories
{
    public class ReportsRepository : BaseRepository<TabModel>, IReportsRepository
    {
        public IConfiguration appConfig;
        public ReportsRepository(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }
        public async Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);

                    var result = await conn.QueryFirstOrDefaultAsync<PersonalDetailsModel>(Procedures.GetApplicationDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PersonalDetailsModel> GetReportPersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<PersonalDetailsModel>(Procedures.GetReportPersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWBSSC_PersonalDetailsModel> GetReportGLWB_SSC_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBSSC_PersonalDetailsModel>(Procedures.GetReportGLWB_SSC_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWBHSC_PersonalDetailsModel> GetReportGLWB_Hsc_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBHSC_PersonalDetailsModel>(Procedures.GetReportGLWB_Hsc_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBADSY_PersonalDetailsModel> GetReportGLWB_adsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBADSY_PersonalDetailsModel>(Procedures.GetReportGLWB_adsy_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<GLWBADSYSchemeDetails> GetReportGLWB_adsy_SchemeDetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    //queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBADSYSchemeDetails>(Procedures.GetReportGLWB_adsy_SchemeDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWBPSY_PersonalDetailsModel> GetReportGLWB_PSY_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBPSY_PersonalDetailsModel>(Procedures.GetReportGLWB_PSY_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBASY_PersonalDetailsModel> GetReportGLWB_aSY_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBASY_PersonalDetailsModel>(Procedures.getrpt_glwb_asy_personaldetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBCycle_personalDetails> GetReportGLWB_CSY_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBCycle_personalDetails>(Procedures.GetReportGLWB_CSY_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBMSL_Personaldetails> GetReportGLWB_MSL_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBMSL_Personaldetails>(Procedures.GetReportGLWB_MSL_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWB_TSY_personalDetails> GetReportGLWB_tsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWB_TSY_personalDetails>(Procedures.GetReportGLWB_tsy_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBhty_PersonalDetailsModel> GetReportGLWB_HTY_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBhty_PersonalDetailsModel>(Procedures.GetReportGLWB_HTY_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ServiceMaster> GetSchemeByServiceIdgetscheme(long _serviceid)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", _serviceid);
                    queryParameters.Add("_registrationid", 0);
                    var result = await conn.QueryFirstOrDefaultAsync<ServiceMaster>(Procedures.GetSchemeByServiceId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SchemeDetails> GetSchemeDetailsByAppId(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<SchemeDetails>(Procedures.GetSchemeDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 5 schemss manually added 
        public async Task<BOCWTBSYSchemeDetails> getrptbocwTbsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWTBSYSchemeDetails>(Procedures.getrpt_bocw_tbsy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BOCWVRSchemeDetails> getrptbocwvrschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWVRSchemeDetails>(Procedures.getrptbocwvrschemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BOCWBPSYSchemeDetails> getrptbocwbpsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWBPSYSchemeDetails>(Procedures.getrpt_bocw_bpsy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BOCWASYSchemeDetails> getrptbocwasyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWASYSchemeDetails>(Procedures.getrpt_bocw_asy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BOCWTSYSchemeDetails> getrptbocwtsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWTSYSchemeDetails>(Procedures.getrpt_bocw_tsy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BOCWVCYSchemeDetails> getrptbocwvcyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWVCYSchemeDetails>(Procedures.getrpt_bocw_vcy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BOCWPSYSchemeDetails> getrptbocwpsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWPSYSchemeDetails>(Procedures.getrpt_bocw_psy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWBHSCSchemeDetails> getrptglwbhscschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBHSCSchemeDetails>(Procedures.getrpt_glwb_hsc_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWBPSY_SchemeDetails> getrptglwbpsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<GLWBPSY_SchemeDetails>(Procedures.getrpt_glwb_psy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWBSSCSchemeDetails> getrptglwbsscschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<GLWBSSCSchemeDetails>(Procedures.getrpt_glwb_ssc_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BOCWPIPSchemeDetails> getrptbocwpipschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWPIPSchemeDetails>(Procedures.getrpt_bocw_pip_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBMSL_SchemeDetails> getrptglwb_msl_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<GLWBMSL_SchemeDetails>(Procedures.getrptglwb_msl_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBCYCLE_Schemedetails> getrptglwb_csy_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<GLWBCYCLE_Schemedetails>(Procedures.getrptglwb_csy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBhtySchemeDetails> getrptglwb_hty_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var result = new GLWBhtySchemeDetails();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);
                    if (schemaName == "glwb_hty")
                    {
                        result = await conn.QueryFirstOrDefaultAsync<GLWBhtySchemeDetails>(Procedures.getrptglwb_hty_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    }
                    else if (schemaName == "glwb_hty_claim")
                    {
                        result = await conn.QueryFirstOrDefaultAsync<GLWBhtySchemeDetails>(Procedures.getrptglwb_hty_claim_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBASYSchemeDetails> getrptglwb_asy_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<GLWBASYSchemeDetails>(Procedures.getrpt_glwb_asy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BOCWACSYSchemeDetails> getrptbocwacsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWACSYSchemeDetails>(Procedures.getrpt_bocw_acsy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        // end extara schemes

        public async Task<List<DocumentFileDetails>> GetdocumentDetailsByAppId(long ApplicationId, string schemaName, string tableName, long servicesId)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);
                    queryParameters.Add("in_serviceid", servicesId);

                    var result = (await conn.QueryAsync<DocumentFileDetails>(Procedures.GetdocumentDetailsByAppId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
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




        public async Task<GLWBHLS_PersonalDetailsModel> GetReportGLWB_HLS_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBHLS_PersonalDetailsModel>(Procedures.getrpt_glwb_hls_personaldetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBHLS_SchemeDetails> GetReportGLWB_HLS_SchemeDetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBHLS_SchemeDetails>(Procedures.getrpt_glwb_hls_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<PendencyReportDetails>> GetRPTDistrictWisePendencyData(long serviceId, DateTime? dtFromDate, DateTime? dtToDate, long districtId, long talukaId, int beneficiaryType, string schemaName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_serviceid", serviceId);
                    queryParameters.Add("in_fromdate", dtFromDate);
                    queryParameters.Add("in_todate", dtToDate);
                    queryParameters.Add("in_districtid", districtId);
                    queryParameters.Add("in_talukaid", talukaId);
                    queryParameters.Add("in_beneficiarytype", beneficiaryType);
                    queryParameters.Add("in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<PendencyReportDetails>(Procedures.GetRPTDistrictWisePendencyData, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //hss
        public async Task<GLWBHSS_PersonalDetails> GetReportGLWB_Hss_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBHSS_PersonalDetails>(Procedures.GetReportGLWB_Hss_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWBHSS_SchemeDetails> getrptglwbhssschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBHSS_SchemeDetails>(Procedures.getrpt_glwb_hss_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<EmpApplicationDetailsModel>> GetRPTBOCWApplicationDetailsList(long districtId, long talukaId, DateTime? dtFromDate, DateTime? dtToDate, int status, long postId, int beneficiaryType, string? schemaName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_districtid", districtId);
                    queryParameters.Add("in_talukaid", talukaId);
                    queryParameters.Add("in_fromdate", dtFromDate);
                    queryParameters.Add("in_todate", dtToDate);
                    queryParameters.Add("in_status", status);
                    queryParameters.Add("in_postid", postId);
                    queryParameters.Add("in_beneficiaryty", beneficiaryType);
                    queryParameters.Add("in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.RPTBOCWGETApplicationDetailsList, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EmpApplicationDetailsModel>> GetRPTBOCWApplicationDetailsListGLWB_TSY(long districtId, DateTime? dtFromDate, DateTime? dtToDate, int status, long postId, int beneficiaryType, string? schemaName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_districtid", districtId);
                    queryParameters.Add("in_fromdate", dtFromDate);
                    queryParameters.Add("in_todate", dtToDate);
                    queryParameters.Add("in_status", status);
                    queryParameters.Add("in_postid", postId);
                    queryParameters.Add("in_beneficiaryty", beneficiaryType);
                    queryParameters.Add("in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.GetRPTBOCWApplicationDetailsListGLWB_TSY, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EmpApplicationDetailsModel>> GetRPTBOCWPendencyDaysList(long serviceId, long applicationid, int applicationstatus, string? schemaName)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_serviceid", serviceId);
                    queryParameters.Add("in_applicationid", applicationid);
                    queryParameters.Add("in_status", applicationstatus);
                    queryParameters.Add("in_schemaname", schemaName);

                    var result = (await conn.QueryAsync<EmpApplicationDetailsModel>(Procedures.RPTBOCWGETPendencyDaysList, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //lsy
        public async Task<GLWBLSY_PersonalDetails> GetReportGLWB_Lsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBLSY_PersonalDetails>(Procedures.GetReportGLWB_Lsy_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<GLWBLSY_SchemeDetails> getrptglwblsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWBLSY_SchemeDetails>(Procedures.getrpt_glwb_lsy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // GLWB_TSY

        public async Task<GLWB_TSY_personalDetails> GetReportGLWB_Tsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWB_TSY_personalDetails>(Procedures.GetReportGLWB_Tsy_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBTSYSchemeDetails> getrptglwb_tsy_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<GLWBTSYSchemeDetails>(Procedures.getrptglwb_tsy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CompanyWorkerDetails>> getrptglwb_tsy_companyWorkerDetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<CompanyWorkerDetails>(Procedures.getrptglwb_tsy_companyWorkerDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CompanyWorkerDetails>> getrpt_glwb_tsy_getagecount(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<CompanyWorkerDetails>(Procedures.getrpt_glwb_tsy_getagecount, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //GLWb_TSY_Claim
        public async Task<GLWB_TSYClaim_personalDetails> GetReportGLWB_Tsy_Claim_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", servicesId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = await conn.QueryFirstOrDefaultAsync<GLWB_TSYClaim_personalDetails>(Procedures.GetReportGLWB_Tsy_Claim_PersonalDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GLWBTSYSchemeDetails> getrptglwb_tsy_claim_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<GLWBTSYSchemeDetails>(Procedures.getrptglwb_tsy_claim_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CompanyWorkerDetails>> getrptglwb_tsy_claim_companyWorkerDetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<CompanyWorkerDetails>(Procedures.getrptglwb_tsy_claim_companyWorkerDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CompanyWorkerDetails>> getrpt_glwb_tsy_claim_getagecount(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<CompanyWorkerDetails>(Procedures.getrpt_glwb_tsy_claim_getagecount, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //bocw_hssy

        public async Task<BOCWHssySchemeDetails> getrptbocw_hssy_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWHssySchemeDetails>(Procedures.getrptbocw_hssy_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BOCWNanjiSchemeDetails> getrpt_bocw_nanaji_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWNanjiSchemeDetails>(Procedures.getrpt_bocw_nanaji_schemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<NanajiFamilyMemberDetails>> getrpt_bocw_nanaji_Familymembersdetailsschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<NanajiFamilyMemberDetails>(Procedures.getrpt_bocw_nanaji_Familymembersdetailsschemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<familymember>> getrptbocw_hssy_familyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<familymember>(Procedures.getrpt_bocw_hssy_familyschemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<StudentMemberDetails>> getrpt_bocw_hssy_childrenfamilyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<StudentMemberDetails>(Procedures.getrpt_bocw_hssy_childrenfamilyschemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<StudentHostelMemberDetails>> getrpt_bocw_hssy_childrenhostelschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<StudentHostelMemberDetails>(Procedures.getrpt_bocw_hssy_childrenhostelschemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FamilyMemberDetails>> getrptbocw_hty_familymembersschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<FamilyMemberDetails>(Procedures.getrpt_glwb_hty_Familyssschemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<FamilyMemberTravelDetails>> getrptbocw_hty_claim_familymembersschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<FamilyMemberTravelDetails>(Procedures.getrpt_glwb_hty_claim_Familyssschemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<BOCWVR_OtherSchemeDetails>> getrpt_bocw_vr_otherschemedetails(long applicationId, string schemaName, string tableName)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", applicationId);
                    queryParameters.Add("in_schemaname", schemaName);
                    queryParameters.Add("in_tablename", tableName);


                    var result = (await conn.QueryAsync<BOCWVR_OtherSchemeDetails>(Procedures.getrpt_bocw_vr_otherschemedetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();

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
