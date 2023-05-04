using DNTCaptcha.Core;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using LabourCommissionerAPI.ResponseModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;

namespace LabourCommissionerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _iaccountService;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly string _forgotpasswordOTPTemplateId;
        private readonly string _forgotpasswordChangedTemplateId;
        private readonly string _changePasswordTemplateId;
        private readonly string _domainName;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _claimPincipal;
        ApiResponse apiResponse = new ApiResponse();


        public AccountController(IAccountService accountService, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, IConfiguration config, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)

        {
            _iaccountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _env = env;
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _forgotpasswordOTPTemplateId = _config["SMSTemplateId:TForgotpasswordOTP"];
            _forgotpasswordChangedTemplateId = _config["SMSTemplateId:TForgotpasswordChanged"];
            _changePasswordTemplateId = _config["SMSTemplateId:TChangePassword"];
            _domainName = _config["DomainName:Name"];
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
        }


        [HttpGet("Login")]
        public async Task<IActionResult> Login(Usermaster model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usermaster user = await _iaccountService.AthenticateUser(model);

                    if (user != null)
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                        apiResponse.Result = user;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Successfull);
                        apiResponse.StackTrace = null;
                    }
                    else
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                        apiResponse.Result = null;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Not_Successfull);
                        apiResponse.StackTrace = null;
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }

            return Ok(apiResponse);
        }

        //SentEmailForForgetPassword API

        [HttpGet("SentEmailForForgetPassword")]
        [AllowAnonymous, HttpPost("MailTemplate/ForgetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SentEmailForForgetPassword(Usermaster emailModel)
        {
            try
            {

                string errorMsg = "OTP has been successfully sent to your Email...!";
                string webRootPath = _env.WebRootPath ?? throw new InvalidOperationException();

                var URL = (_domainName + "Home/ForgetPasswordReset?UserId = " + emailModel.UserId);
                emailModel.URL = URL;
                emailModel.HostName = CommonUtils.GetHostName();
                emailModel.IpAddress = CommonUtils.GetLocalIPAddress();

                var result = await _iaccountService.MethodForGetData(emailModel);

                CommonUtils emailFunction = new CommonUtils(_config);
                string beneficiarytype = "";

                var res = emailFunction.SendPasswordMail(result.DisplayName, result.EmailId, result.OTP_Code, result.DisplayName, result.Password, 1, webRootPath, URL, beneficiarytype);

                CommonUtils commonFunction = new CommonUtils(_config);
                var msg = "સન્માન પોર્ટલ પર પાસવર્ડ બદલવા OTP: " + result.OTP_Code + ". 30 મિનિટ સુધી માન્ય રેહશે - LSEDEPT";
                bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(msg, result.MobileNo, _forgotpasswordOTPTemplateId);
                if (isSendSMS)
                {
                    await _iaccountService.AddSMSLogs(result.MobileNo, 0, msg, Convert.ToInt32(result.RegistrationId));
                }

                if (res != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = res;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Successfull);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Not_Successfull);
                    apiResponse.StackTrace = null;
                }
            }

            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }

            return Ok(apiResponse);
        }


        //ForgetPasswordReset API

        [HttpPost("ForgetPasswordReset")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPasswordReset(ForgetPassword resetpassword)
        {
            try
            {
                    var result = await _iaccountService.ResetPassword(resetpassword);
                    if (result != null)
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                        apiResponse.Result = result;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Password_Change_Successfull);
                        apiResponse.StackTrace = null;
                    }
                    else
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                        apiResponse.Result = null;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Password_Does_Not_Changed);
                        apiResponse.StackTrace = null;
                    }
                
            }

            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }

            return Ok(apiResponse);
        }

        //EmployeeLogin API

        [HttpGet("EmployeeLogin")]
        [HttpPost, AllowAnonymous]
        [ValidateDNTCaptcha(
            ErrorMessage = "કૃપા કરીને સાચો Captcha નાખો.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> EmployeeLogin(Employeemaster model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Employeemaster user = await _iaccountService.AthenticateEmployee(model);

                    if (user != null)
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                        apiResponse.Result = user;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Successfull);
                        apiResponse.StackTrace = null;
                    }
                    else
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                        apiResponse.Result = null;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Not_Successfull);
                        apiResponse.StackTrace = null;
                    }
                }
            }

            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }

            return Ok(apiResponse);
        }

        //Emolyee logout

        [HttpGet("EmployeeLogout")]
        public async Task<IActionResult> EmployeeLogout()
        {
            try
            {
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2)
                };
                //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authProperties);
                var model = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authProperties);

                if (model != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Successfull);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Not_Successfull);
                    apiResponse.StackTrace = null;
                }

            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }

            return Ok(apiResponse);
        }

    }
}

