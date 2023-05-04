using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.ViewDataModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using LabourCommissioner.Abstraction;
using DNTCaptcha.Core;

namespace LabourCommissioner.Controllers
{
    //tEsT
    /// <summary>
    /// /
    /// </summary>

    public class AccountController : Controller
    {
        private readonly IAccountService _iaccountService;
        private IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly string _forgotpasswordOTPTemplateId;
        private readonly string _forgotpasswordChangedTemplateId;
        private readonly string _changePasswordTemplateId;
        private readonly string _domainName;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _claimPincipal;
        public AccountController(IAccountService accountService, IHostingEnvironment env, IConfiguration config, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            //Test: 1
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
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "home");
        }

        [HttpPost, AllowAnonymous]
        [ValidateDNTCaptcha(
            ErrorMessage = "કૃપા કરીને સાચો Captcha નાખો.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> Login(Usermaster model)
        {
            //int a = 10;int b = 0; int c = a / b;

            string returnUrl = String.Empty;
            if (ModelState.IsValid)
            {
                Usermaster user = await _iaccountService.AthenticateUser(model);
                if (user == null || (user != null && user.UserId == 0))
                {
                    ViewBag.Message = "Invalid Credential";
                    return View("~/Views/Home/Index.cshtml", model);
                }
                else
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.CitizenRoleId.ToString()),
                        new Claim("RegistrationId", user.RegistrationId.ToString()),
                        new Claim("UserType", user.UserType.ToString()),
                        new Claim("DisplayName", Convert.ToString(user.DisplayName)),
                        new Claim("MobileNo", Convert.ToString(user.MobileNo)),
                        new Claim("EmailId", String.IsNullOrEmpty(user.EmailId) ? "" : Convert.ToString(user.EmailId)),
                        new Claim("BeneficiaryType", Convert.ToString(user.BeneficiaryType)),
                        new Claim("PostId", user.PostId.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTime.UtcNow.AddHours(2)
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                    return RedirectToAction("Home", "Home");
                }
            }
            else
            {
                //ModelState.AddModelError("DNTCaptchaInputText", "કૃપા કરીને સાચો Captcha નાખો.");
                return View("~/Views/Home/Index.cshtml", model);
            }
        }


        [HttpPost, AllowAnonymous]
        [ValidateDNTCaptcha(
            ErrorMessage = "કૃપા કરીને સાચો Captcha નાખો.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> EmployeeLogin(Employeemaster model)
        {
            //int a = 10;int b = 0; int c = a / b;
            string returnUrl = String.Empty;
            if (ModelState.IsValid)
            {
                Employeemaster user = await _iaccountService.AthenticateEmployee(model);
                if (user == null || (user != null && user.PostId == 0))
                {
                    ViewBag.Message = "Invalid Credential";
                    return View("~/Views/Home/EmployeeLogin.cshtml", model);
                }
                else
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.EmployeeId)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                        new Claim("RegistrationId", user.RegistrationId.ToString()),
                        new Claim("BeneficiaryType", user.EmployeeType.ToString()),
                        new Claim("PostName", Convert.ToString(user.postname)),
                        new Claim("MobileNo", Convert.ToString(user.contactno)),
                        new Claim("EmailId", Convert.ToString(user.EmailId)),
                        new Claim("DistrictId", Convert.ToString(user.DistrictId)),
                        new Claim("VillageId", Convert.ToString(user.VillageId)),
                        new Claim("TalukaId", Convert.ToString(user.TalukaId)),
                        new Claim("UserType", Convert.ToString(user.UserType)),
                        new Claim("PostId", user.PostId.ToString()),
                        new Claim("IsUrban", user.IsUrban.ToString()),
                        new Claim("OrderBy", user.OrderBy.ToString()),
                        new Claim("ServiceId", "0")
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTime.UtcNow.AddHours(2)
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                    return RedirectToAction("Index", "EmployeeHome");
                }
            }
            else
            {
                return View("~/Views/Home/EmployeeLogin.cshtml", model);
            }
        }


        [HttpPost, AllowAnonymous]  
        [ValidateDNTCaptcha(
            ErrorMessage = "કૃપા કરીને સાચો Captcha નાખો.",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> OtherLogin(Employeemaster model)
        {
            //int a = 10;int b = 0; int c = a / b;
            string returnUrl = String.Empty;
            if (ModelState.IsValid)
            {
                var claims = new List<Claim>();
                Employeemaster user = await _iaccountService.AthenticateCompany(model);
                if (user == null)
                {
                    ViewBag.Message = "Invalid Credential";
                    return View("~/Views/Home/OtherLogin.cshtml", model);
                }
                else if (user.UserType == 3)
                {
                    claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.companyuserid)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                        new Claim("RegistrationId", user.RegistrationId.ToString()),
                        new Claim("DisplayName", user.name.ToString()),
                        new Claim("EmailId", Convert.ToString(user.EmailId)),
                        new Claim("UserType", Convert.ToString(user.UserType)),
                        new Claim("BeneficiaryType", "2"),
                        new Claim("ServiceId", "0"),
                        new Claim("PostId","0"),
                    };


                }
                else if (user.UserType == 4)
                {
                    claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.hospitaluserid)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                        new Claim("RegistrationId", user.RegistrationId.ToString()),
                        new Claim("DisplayName", user.name.ToString()),
                        new Claim("EmailId", Convert.ToString(user.EmailId)),
                        new Claim("UserType", Convert.ToString(user.UserType)),
                         new Claim("BeneficiaryType", "2"),
                        new Claim("ServiceId", "0"),
                        new Claim("PostId","0"),
                    };
                }
                else if (user.UserType == 5)
                {
                    claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.companyuserid)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                        new Claim("RegistrationId", user.RegistrationId.ToString()),
                        new Claim("DisplayName", user.name.ToString()),
                        new Claim("EmailId", Convert.ToString(user.EmailId)),
                        new Claim("UserType", Convert.ToString(user.UserType)),
                        new Claim("BeneficiaryType", "7"),
                        new Claim("ServiceId", "33"),
                        new Claim("isFilledAuthority", user.isfilledauthority==false?"0":"1"),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                        new Claim("PostId","0"),
                    };


                }
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2)
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                //return RedirectToAction("OtherHome", "Home");
                return RedirectToAction("Home", "Home");

            }
            else
            {
                return View("~/Views/Home/EmployeeLogin.cshtml", model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddHours(2)
            };
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authProperties);
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeLogout()
        {
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddHours(2)
            };
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, authProperties);
            return RedirectToAction("EmployeeLogin", "home");
        }

        //[HttpPost]
        //public string BrowserClose()
        //{
        //    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    var prop = new AuthenticationProperties();
        //    return "Success";
        //}


        [AllowAnonymous, HttpPost("MailTemplate/ForgetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SentEmailForForgetPassword(Usermaster emailModel)
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
            TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
            return RedirectToAction("Index", "Home", res);
        }


        // Method for Forget Password Reset.
        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> ForgetPasswordReset(ForgetPassword resetpassword)
        {
            resetpassword.URL = (_domainName + "Home/ForgetPasswordReset?" + resetpassword.UserId);
            resetpassword.HostName = CommonUtils.GetHostName();
            resetpassword.IpAddress = CommonUtils.GetLocalIPAddress();

            var result = await _iaccountService.ResetPassword(resetpassword);
            if (result.UserId != -1)
            {
                string errorMsg = "Password Change Successfully....!";

                CommonUtils commonFunction = new CommonUtils(_config);
                var msg = "સન્માન પોર્ટલ પર User ID: " + result.username + " નવો Password: " + resetpassword.Password + "- LSEDEPT";
                bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(msg, result.MobileNo, _forgotpasswordChangedTemplateId);
                if (isSendSMS)
                {
                    await _iaccountService.AddSMSLogs(result.MobileNo, 0, msg, Convert.ToInt32(result.registrationid));
                }
                TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
            }
            else
            {
                string errorMsg = "Password Does Not Changed...!";
                TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            string beneficiarytype = "";
            changePasswordModel.UserType = Convert.ToInt32(_claimPincipal.FindFirstValue("UserType"));
            if (changePasswordModel.UserType != null && changePasswordModel.UserType == Convert.ToInt32(EnumLookup.UserType.Citizen))
            {
                changePasswordModel.UserId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                beneficiarytype = _claimPincipal.FindFirst("BeneficiaryType").Value;
            }
            else if (changePasswordModel.UserType != null && changePasswordModel.UserType == Convert.ToInt32(EnumLookup.UserType.Employee))
            {
                changePasswordModel.UserId = Convert.ToInt64(_claimPincipal.FindFirstValue("PostId"));
                beneficiarytype = _claimPincipal.FindFirst("BeneficiaryType").Value;
            }
            ChangePasswordModel user = await _iaccountService.ChangePassword(changePasswordModel);




            if (user != null && user.errorcode == 0)
            {
                string errorMsg = "સન્માન પોર્ટલ તમારો પાસવર્ડ સફળતાપૂર્વક બદલાઈ ગયો છે.!";

                if (changePasswordModel.UserType == Convert.ToInt32(EnumLookup.UserType.Citizen))
                {

                    CommonUtils emailFunction = new CommonUtils(_config);
                    string webRootPath = _env.WebRootPath ?? throw new InvalidOperationException();
                    var res = emailFunction.SendChangePasswordMail(user.name, user.emailid, user.username, changePasswordModel.NewPassword, webRootPath, beneficiarytype);

                    CommonUtils commonFunction = new CommonUtils(_config);
                    var msg = "સન્માન પોર્ટલ તમારો પાસવર્ડ સફળતાપૂર્વક બદલાઈ ગયો છે.(User ID:" + user.username + ",Password:" + changePasswordModel.NewPassword + " - LSEDEPT";
                    bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(msg, user.mobileno, _changePasswordTemplateId);
                    if (isSendSMS)
                    {
                        await _iaccountService.AddSMSLogs(user.mobileno, 0, msg, Convert.ToInt32(user.registrationid));
                    }
                }

                TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
            }
            else
            {
                string errorMsg = user.errormsg;
                TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (changePasswordModel.UserType != null && changePasswordModel.UserType == Convert.ToInt32(EnumLookup.UserType.Citizen))
                return RedirectToAction("LogOut", "Account");
            else
                return RedirectToAction("EmployeeLogout", "Account");
        }



    }
}
