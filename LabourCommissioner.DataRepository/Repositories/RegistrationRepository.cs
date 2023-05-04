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

namespace LabourCommissioner.DataRepository.Repositories
{
    public class RegistrationRepository : BaseRepository<Registration>, IRegistrationRepository
    {
        public IConfiguration appConfig;

        public RegistrationRepository(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }
        public async Task<ResponseMessage> AddUpdateRegistration(Registration registration)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addregistration(@in_registrationno,@in_lwbaccountno,@in_enirmancardno,@in_name,@in_dateofbirth,@in_gender,@in_mobileno,@in_emailid,@in_password," +
                       "@in_uniqueidnumber,@in_beneficiarytype,@in_hostname,@in_ipaddress,@in_userid," +
                       "@in_firstcardissueddate,@in_cdistrictid,@in_icardfromdate,@in_icardtodate,@in_organizationname,@in_organizationemail,@in_organizationaddress,@in_organizationcity,@in_organizationdistrict,@in_organizationdistrict_Id,@in_organizationtaluka,@in_organizationpinCode," +
                       "@out_msg,@out_registrationno,@out_error,@out_registrationid)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    //queryParameters.Add("@RegistrationNo", registration.RegistrationNo);
                    queryParameters.Add("@in_registrationno", registration.ENirmanCardNo == null ? "" : registration.ENirmanCardNo);
                    queryParameters.Add("@in_lwbaccountno", registration.LWBAccountNo == null ? "" : registration.LWBAccountNo);
                    queryParameters.Add("@in_enirmancardno", registration.ENirmanCardNo == null ? "" : registration.ENirmanCardNo);
                    queryParameters.Add("@in_name", registration.Name == null ? registration.OtherUserName : registration.Name);
                    queryParameters.Add("@in_dateofbirth", registration.DateOfBirth);
                    queryParameters.Add("@in_gender", registration.Gender == null ? registration.OtherUserGender : registration.Gender);
                    queryParameters.Add("@in_mobileno", registration.MobileNo == null ? registration.OtherUserMobileNo : registration.MobileNo);
                    queryParameters.Add("@in_emailid", registration.EmailId == null ? registration.OtherUserEmailId : registration.EmailId);
                    queryParameters.Add("@in_password", registration.Password == null ? registration.OtherUserPassword : registration.Password);
                    queryParameters.Add("@in_uniqueidnumber", registration.regunique.UniqueIdnumber);
                    queryParameters.Add("@in_beneficiarytype", registration.BeneficiaryType);
                    queryParameters.Add("@in_hostname", registration.hostname);//registration.regunique.HostName);
                    queryParameters.Add("@in_ipaddress", registration.ipaddress);//registration.regunique.IpAddress);
                    queryParameters.Add("@in_userid", 0);
                    queryParameters.Add("@in_firstcardissueddate", registration.FirstCardIssuedDate);
                    queryParameters.Add("@in_cdistrictid", registration.CDistrictId);
                    queryParameters.Add("@in_icardfromdate", registration.ICardFromDate);
                    queryParameters.Add("@in_icardtodate", registration.ICardToDate);
                    queryParameters.Add("@in_organizationname", registration.OrganizationName);
                    queryParameters.Add("@in_organizationemail", registration.OrganizationEmail);
                    queryParameters.Add("@in_organizationaddress", registration.OrganizationAddress);
                    queryParameters.Add("@in_organizationcity", registration.OrganizationCity);
                    queryParameters.Add("@in_organizationdistrict", registration.OrganizationDistrict);
                    queryParameters.Add("@in_organizationdistrict_Id", registration.district_id);
                    queryParameters.Add("@in_organizationtaluka", registration.OrganizationTaluka);
                    queryParameters.Add("@in_organizationpinCode", registration.OrganizationPinCode);
                    queryParameters.Add("@out_msg", " ", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_registrationno", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_registrationid", 0, direction: ParameterDirection.InputOutput);
                    //var result = await conn.QueryFirstOrDefaultAsync<Registration>(Procedures.AddUpdateRegistration, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
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
