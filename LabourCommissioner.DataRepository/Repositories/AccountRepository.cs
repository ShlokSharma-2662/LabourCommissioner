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
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Abstraction;

namespace LabourCommissioner.DataRepository.Repositories
{
    public class AccountRepository : BaseRepository<Registration>, IAccountRepository
    {
        public IConfiguration appConfig;

        public AccountRepository(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }
        public async Task<Usermaster> AthenticateUser(Usermaster usermaster)
        {
            try
            {


                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    //queryParameters.Add("@RegistrationNo", registration.RegistrationNo);
                    queryParameters.Add("_username", usermaster.UserName);
                    queryParameters.Add("_password", usermaster.Password);
                    queryParameters.Add("_usertype", usermaster.UserType);
                    var result = await conn.QueryFirstOrDefaultAsync<Usermaster>(Procedures.AthenticateUser, queryParameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Employeemaster> AthenticateEmployee(Employeemaster employeemaster)

        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    //queryParameters.Add("@RegistrationNo", registration.RegistrationNo);
                    queryParameters.Add("_username", employeemaster.UserName);
                    queryParameters.Add("_password", employeemaster.Password);
                    queryParameters.Add("_employeetype", employeemaster.EmployeeType);
                    var result = await conn.QueryFirstOrDefaultAsync<Employeemaster>(Procedures.AthenticateEmployee, queryParameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Employeemaster> AthenticateCompany(Employeemaster employeemaster)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    //queryParameters.Add("@RegistrationNo", registration.RegistrationNo);
                    queryParameters.Add("_username", employeemaster.UserName);
                    queryParameters.Add("_password", employeemaster.Password);
                    queryParameters.Add("_usertype", employeemaster.UserType);
                    var result = await conn.QueryFirstOrDefaultAsync<Employeemaster>(Procedures.authenticateotheruser, queryParameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<Usermaster> MethodForGetData(Usermaster emailModel)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    //queryParameters.Add("@RegistrationNo", registration.RegistrationNo);
                    queryParameters.Add("in_username", Convert.ToString(emailModel.UserId));
                    queryParameters.Add("in_password", emailModel.Password);
                    queryParameters.Add("in_purpose", Convert.ToString(EnumLookup.EmailPurpose.V));
                    queryParameters.Add("in_url", emailModel.URL);
                    queryParameters.Add("in_host_name", emailModel.HostName);
                    queryParameters.Add("in_ipaddr", emailModel.IpAddress);
                    queryParameters.Add("in_otp_code1", emailModel.OTP_Code);

                    var result = await conn.QueryFirstOrDefaultAsync<Usermaster>(Procedures.CitizenPasswordRecovery, queryParameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Method for reset password of Forget password.
        public async Task<ForgetPassword> ResetPassword(ForgetPassword resetpassword)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    //queryParameters.Add("@RegistrationNo", registration.RegistrationNo);
                    queryParameters.Add("in_username", Convert.ToString(resetpassword.UserId));
                    queryParameters.Add("in_password", resetpassword.Password);
                    queryParameters.Add("in_purpose", Convert.ToString(EnumLookup.EmailPurpose.R));
                    queryParameters.Add("in_url", resetpassword.URL);
                    queryParameters.Add("in_host_name", resetpassword.HostName);
                    queryParameters.Add("in_ipaddr", resetpassword.IpAddress);
                    queryParameters.Add("in_otp_code1", Convert.ToInt32(resetpassword.OTP_Code));

                    var result = await conn.QueryFirstOrDefaultAsync<ForgetPassword>(Procedures.CitizenPasswordRecovery, queryParameters, commandType: CommandType.StoredProcedure);
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

        public async Task<ChangePasswordModel> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_usertypeid", changePasswordModel.UserType);
                    queryParameters.Add("in_userid", changePasswordModel.UserId);
                    queryParameters.Add("in_oldpassword", changePasswordModel.CurrentPassword);
                    queryParameters.Add("in_newpassword", changePasswordModel.NewPassword);
                    var result = await conn.QueryFirstOrDefaultAsync<ChangePasswordModel>(Procedures.ChangePassword, queryParameters, commandType: CommandType.StoredProcedure);
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
