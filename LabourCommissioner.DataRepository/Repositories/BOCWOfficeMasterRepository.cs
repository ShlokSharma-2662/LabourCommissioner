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
using System.Web.Mvc;

namespace LabourCommissioner.DataRepository.Repositories
{
    public class BOCWOfficeMasterRepository : BaseRepository<Registration>, IBOCWMasterRepository
    {
        public IConfiguration appConfig;
        private readonly UserCookies cookies;
        public BOCWOfficeMasterRepository(IConfiguration config, IHttpContextAccessor _httpContextAccessor) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.cookies = new UserCookies(_httpContextAccessor);
        }



        public async Task<ResponseMessage> AddOfficeDetails(OfficeDetailsModel officeDetails)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL bocw_master.insertofficedetails(@in_officeid,@in_officename,@in_officedistrictid,@in_contactpersonname,@in_contactpersonpost," +
                        "@in_contactpersonno,@in_fax,@in_officeaddress,@in_officevillageid,@in_officetalukaid,@in_officestate,@in_officepincode,@in_createdby,@in_ipaddress," +
                        "@in_hostname,@out_error,@out_msg,@out_id,@out_applicatinno)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_officeid", officeDetails.officeid);
                    queryParameters.Add("@in_officename", officeDetails.officeName);
                    queryParameters.Add("@in_officedistrictid", officeDetails.officedistrictid);
                    queryParameters.Add("@in_contactpersonname", officeDetails.contactpersonname);
                    queryParameters.Add("@in_contactpersonpost", officeDetails.contactpersonpost);
                    queryParameters.Add("@in_contactpersonno", officeDetails.contactpersoncontactno);
                    queryParameters.Add("@in_fax", officeDetails.faxno);
                    queryParameters.Add("@in_officeaddress", officeDetails.officeaddress);
                    queryParameters.Add("@in_officevillageid", officeDetails.OfficeVillageId);
                    queryParameters.Add("@in_officetalukaid", officeDetails.OfficeTalukaId);
                    queryParameters.Add("@in_officestate", officeDetails.OfficeStateId);
                    queryParameters.Add("@in_officepincode", officeDetails.OfficePinCode);
                    queryParameters.Add("@in_createdby", officeDetails.createdby);
                    queryParameters.Add("@in_ipaddress", officeDetails.ipaddress);
                    queryParameters.Add("@in_hostname", officeDetails.hostname);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<long>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Id = queryParameters.Get<long>("@out_id");
                    res.ApplicationNo = queryParameters.Get<long>("@out_applicatinno");


                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<OfficeDetailsModel>> GetOfficeDetails()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var result = (await conn.QueryAsync<OfficeDetailsModel>(Procedures.GetApplication, commandType: System.Data.CommandType.StoredProcedure)).ToList();
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

