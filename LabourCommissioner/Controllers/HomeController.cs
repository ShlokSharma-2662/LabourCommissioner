using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common.Utility;
using LabourCommissioner.Models;
//using LabourCommissioner.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;
using LabourCommissioner.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Text.RegularExpressions;
using LabourCommissioner.CustomAuthorization;
using System.Globalization;
using Newtonsoft.Json;
using LabourCommissioner.Common;

namespace LabourCommissioner.Controllers
{
    [Authorize]
    //[ServiceFilter(typeof(PermissionRequirementFilter))]
    public class HomeController : Controller
    {
        private readonly IHomeService _ihomeService;
        private readonly IHomeRepository _homeRepository;
        private readonly ISchemeService _iscchemeService;
        private readonly ISchemeUserServices _schemeUserServices;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _isexceptionmailrequired;
        private readonly string _bocwRegistrationAPI;
        private readonly string _glwbRegistrationAPI;

        public HomeController(IStringLocalizer<HomeController> localizer, IConfiguration config, IWebHostEnvironment webHostEnvironment, IHtmlLocalizer<HomeController> htmlLocalizer, IHomeService homeService, IHomeRepository homeRepository, ISchemeService schemeService, ISchemeUserServices schemeUserServices,
            IHttpContextAccessor httpContextAccessor)

        {
            _ihomeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
            _homeRepository = homeRepository;
            _iscchemeService = schemeService ?? throw new ArgumentNullException(nameof(schemeService));
            _schemeUserServices = schemeUserServices;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _localizer = localizer;
            _htmlLocalizer = htmlLocalizer;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _isexceptionmailrequired = _config["SMTPConfig:_IsExceptionMailRequired"];
            _bocwRegistrationAPI = _config["RegistrationAPI:BOCW"];
            _glwbRegistrationAPI = _config["RegistrationAPI:GLWB"];
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //Usermaster model = new Usermaster();
            ViewBag.showLoginButtons = true;
            ViewBag.hideHomeButton = true;
            ViewBag.showSchemeMenu = true;
            ViewBag.hideAppStatus = true;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(7) }
            );
            return LocalRedirect(returnUrl);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GLWBScheme()
        {
            long beneficiaryTypeId = Convert.ToInt32(EnumLookup.BeneficiaryType.GLWB);
            IEnumerable<ServiceMaster> model = await _ihomeService.GetSchemeByBeneficiaryTypeId(beneficiaryTypeId);
            return View(model);

        }

        [AllowAnonymous]
        //[PermissionRequirement(PermissionConstant.IsView)]
        public async Task<IActionResult> BOCWScheme()
        {
            long beneficiaryTypeId = Convert.ToInt32(EnumLookup.BeneficiaryType.BOCW);
            IEnumerable<ServiceMaster> model = await _ihomeService.GetSchemeByBeneficiaryTypeId(beneficiaryTypeId);
            return View(model);

        }


        [AllowAnonymous]
        public async Task<IActionResult> ListDetails(int ApplicationId)
        {
            IEnumerable<bocw_ssy_personaldetails> model = await _ihomeService.GetCitizen(ApplicationId);
            return View(model);
        }


        public async Task<IActionResult> ApplicationDetails(string strserviceId,bool isschemeopen, bool isreverted)
        {
            int serviceId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strserviceId)));
            long registrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            int usertype = Convert.ToInt32(_claimPincipal.FindFirstValue("UserType"));
            ServiceMaster objServiceMaster = await _ihomeService.GetSchemeByServiceId(serviceId,registrationId);
            string schemaName = "";
            string tableName = "personaldetails";

            ViewBag.isschemeopen = isschemeopen;
            ViewBag.isreverted = isreverted;

            #region Check E-NirmanCard Expiry
            var eNirmanCardExpiryData = await _ihomeService.CheckENirmanCardExpiry(registrationId);
            if (eNirmanCardExpiryData != null && eNirmanCardExpiryData.ErrorCode == 1)
            {
                ViewBag.eNExpMessage = eNirmanCardExpiryData.Msg;
            }
            #endregion

            if (serviceId == 7)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 2)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 3)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 4)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }

            if (serviceId == 23)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }

            if (serviceId == 20)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 22)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 19)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 16)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 30)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 21)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 14)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 12)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 6)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 5)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (serviceId == 13)
            {
                ResponseMessage model = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
                ViewBag.ErrorCode = model.ErrorCode;
                ViewBag.Message = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }
            if (objServiceMaster != null)
            {
                ViewBag.ControllerName = objServiceMaster.ControllerName;
                ViewBag.ActionName = objServiceMaster.ActionName;
                schemaName = objServiceMaster.SchemaName;
                ViewBag.SchemeName = objServiceMaster.ServiceNameGujarati;
            }
            ViewBag.serviceId = serviceId;

            if (serviceId == 24 && usertype == 4)
            {
                IEnumerable<ApplicationDetailsModel> model1 = await _ihomeService.Glwb_TSY_Hospital_GetApplication(registrationId, serviceId, schemaName, tableName);
                return View(model1);
            }
            else if (serviceId == 24)
            {
                IEnumerable<ApplicationDetailsModel> model1 = await _ihomeService.Glwb_TSY_GetApplication(registrationId, serviceId, schemaName, tableName);
                return View(model1);
            }
            else if (serviceId == 12 && usertype == 4)
            {
                IEnumerable<ApplicationDetailsModel> model1 = await _ihomeService.Bocw_Tbsy_GetApplication(registrationId, serviceId, schemaName, tableName, usertype);
                return View(model1);
            }
            else if (serviceId == 12)
            {
                IEnumerable<ApplicationDetailsModel> model1 = await _ihomeService.Bocw_Tbsy_GetApplication(registrationId, serviceId, schemaName, tableName, usertype);
                return View(model1);
            }
            else
            {
                IEnumerable<ApplicationDetailsModel> model;
                if (serviceId == 32)
                {
                    var orgServiceId = serviceId;
                    //serviceId = 16;
                    model = await _ihomeService.GetGLWB_HTYApplicationDetailsForClaim(registrationId, serviceId, schemaName, tableName);
                    ViewBag.OrgServiceId = orgServiceId;
                    return View(model);

                }
                else if (serviceId == 34)
                {
                    var orgServiceId = serviceId;
                    IEnumerable<ApplicationDetailsModel> model1 = await _ihomeService.Glwb_TSY_Claim_GetApplication(registrationId, serviceId, schemaName, tableName);
                    ViewBag.OrgServiceId = orgServiceId;
                    return View(model1);
                }
                else if (serviceId == 36)
                {
                    var orgServiceId = serviceId;
                    IEnumerable<ApplicationDetailsModel> model1 = await _ihomeService.GetBocw_TBSYApplication(registrationId, serviceId, schemaName, tableName);
                    ViewBag.OrgServiceId = orgServiceId;
                    return View(model1);
                }
                else
                {
                    model = await _ihomeService.GetApplicationDetails(registrationId, serviceId, schemaName, tableName);
                    return View(model);
                }

            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateeNirmanCardExpiryDate()
        {

            Task<APIResponse> APIResponse = null;
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            var model = await _ihomeService.GetPersonalDetailsByRegNo(RegistrationNo);

            if (model != null)
            {
                var dataObject = new
                {
                    ApplicationNo = model.RegistrationNo,
                    UniqueIDNumber = model.AadharCardNo,
                };
                CommonUtils emailFunction = new CommonUtils(_config);
                APIResponse = emailFunction.CallWebAPI(dataObject);

                if (APIResponse != null && APIResponse.Result != null)
                {

                    RootBOCWRegAPIResult rootBOCWRegAPIResult = JsonConvert.DeserializeObject<RootBOCWRegAPIResult>(APIResponse.Result.Result.ToString());

                    string iCardToDateOLD = model.ICardToDate.ToString("dd/MM/yyyy");
                    string iCardFromDateOLD = model.ICardFromDate.ToString("dd/MM/yyyy");


                    string iCardToDateNew = Convert.ToString(rootBOCWRegAPIResult.Result[0].ICardToDate);
                    string iCardFromDateNew = Convert.ToString(rootBOCWRegAPIResult.Result[0].ICardFromDate);

                    DateTime? dtiCardFromDateOld = null;
                    DateTime? dtiCardFromDateNew = null;
                    DateTime? dtiCardToDateOld = null;
                    DateTime? dtiCardToDateNew = null;

                    if (string.IsNullOrEmpty(iCardToDateOLD))
                    {
                        DateTime now = DateTime.Now;
                        var startDate = new DateTime(now.Year, now.Month, 1);
                        var formatStartDate = String.Format("{0:dd/MM/yyyy}", startDate);
                        dtiCardToDateOld = DateTime.ParseExact(formatStartDate, "dd/MM/yyyy", null);
                    }
                    else
                    {
                        var finalDateTime = DateTime.ParseExact(iCardToDateOLD, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var demo = String.Format("{0:dd/MM/yyyy}", finalDateTime);
                        dtiCardToDateOld = DateTime.ParseExact(demo, "dd/MM/yyyy", null);
                    }
                    if (string.IsNullOrEmpty(iCardToDateNew))
                    {
                        DateTime endDate = DateTime.Now;
                        var formatEndDate = String.Format("{0:dd/MM/yyyy}", endDate);
                        dtiCardToDateNew = DateTime.ParseExact(formatEndDate, "dd/MM/yyyy", null);
                    }
                    else
                    {
                        var finalDateTime1 = DateTime.ParseExact(iCardToDateNew, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var demo1 = String.Format("{0:dd/MM/yyyy}", finalDateTime1);
                        dtiCardToDateNew = DateTime.ParseExact(demo1, "dd/MM/yyyy", null);
                    }



                    if (string.IsNullOrEmpty(iCardFromDateOLD))
                    {
                        DateTime now = DateTime.Now;
                        var startDate = new DateTime(now.Year, now.Month, 1);
                        var formatStartDate = String.Format("{0:dd/MM/yyyy}", startDate);
                        dtiCardFromDateOld = DateTime.ParseExact(formatStartDate, "dd/MM/yyyy", null);
                    }
                    else
                    {
                        var finalDateTime = DateTime.ParseExact(iCardFromDateOLD, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var demo = String.Format("{0:dd/MM/yyyy}", finalDateTime);
                        dtiCardFromDateOld = DateTime.ParseExact(demo, "dd/MM/yyyy", null);
                    }
                    if (string.IsNullOrEmpty(iCardFromDateNew))
                    {
                        DateTime endDate = DateTime.Now;
                        var formatEndDate = String.Format("{0:dd/MM/yyyy}", endDate);
                        dtiCardFromDateNew = DateTime.ParseExact(formatEndDate, "dd/MM/yyyy", null);
                    }
                    else
                    {
                        var finalDateTime1 = DateTime.ParseExact(iCardFromDateNew, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var demo1 = String.Format("{0:dd/MM/yyyy}", finalDateTime1);
                        dtiCardFromDateNew = DateTime.ParseExact(demo1, "dd/MM/yyyy", null);
                    }


                    var responseModel = await _ihomeService.UpdateeNirmanCardxpiryDate(Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")), dtiCardToDateOld, dtiCardToDateNew, dtiCardFromDateOld, dtiCardFromDateNew);
                    return Json(new { data = responseModel });
                }
                else
                {
                    return Json(new { data = "Registration details not found in API." });
                }
            }
            else
            {
                return Json(new { data = "Registration details not found." });
            }



            //long userId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //string password = CommonUtils.GenerateRandomStrongPassword(8);
            //string ipAddress = CommonUtils.GetLocalIPAddress();
            //string hostName = CommonUtils.GetHostName();
            //var model = await _ihomeService.ResetPassword(Convert.ToInt64(RegistrationId), userId, password, ipAddress, hostName);

        }



        public IActionResult GLWBSchemeDetails()
        {
            return View();
        }


        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Form()
        {
            return View();
        }
        public IActionResult Grid()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Form2()
        {
            return View();
        }
        public IActionResult Form3()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult EmployeeLogin()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult OtherLogin()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registration()
        {
            var vm = new Registration()
            {
                //    Gender = new List<Gender>
                //    {
                //        new Gender {Value = 1, Text = "Male"},
                //        new Gender {Value = 2, Text = "Female"}
                //}
            };

            ViewBag.BOCWRegistrationAPI = _bocwRegistrationAPI;
            ViewBag.GLWBRegistrationAPI = _glwbRegistrationAPI;
            return View(vm);
            //return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Registration(Registration registration)
        {
            return View(registration);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var ajaxRequest = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionDetails != null)
            {
                ExceptionLogger exceptionLogger = new ExceptionLogger();
                exceptionLogger.UserId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
                exceptionLogger.ControllerName = Convert.ToString(((object[])((ExceptionHandlerFeature)exceptionDetails).RouteValues.Values)[0]);
                exceptionLogger.ActionName = Convert.ToString(((object[])((ExceptionHandlerFeature)exceptionDetails).RouteValues.Values)[1]);
                exceptionLogger.ExceptionMessage = Convert.ToString(exceptionDetails.Error.Message);
                exceptionLogger.ExceptionStackTrace = Convert.ToString(exceptionDetails.Error.StackTrace);
                exceptionLogger.RequestURI = Convert.ToString(exceptionDetails.Path);
                string LineNumber = Convert.ToString(exceptionDetails.Error.StackTrace.Substring(exceptionDetails.Error.StackTrace.IndexOf(' ')));
                LineNumber = Regex.Match(LineNumber, @"\d+").Value;
                exceptionLogger.LineNumber = LineNumber == "" ? 0 : Convert.ToInt32(LineNumber);
                exceptionLogger.LogFrom = Convert.ToInt32((int)EnumLookup.exceptionlogfrom.WEB);
                exceptionLogger.IpAddress = CommonUtils.GetLocalIPAddress();
                exceptionLogger.HostName = CommonUtils.GetHostName();

                var model = _ihomeService.AddExceptionLog(exceptionLogger);
                if (_isexceptionmailrequired == "1")
                {
                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendExceptionMail(exceptionLogger, rootPath, "ExceptionMail.html");
                }
            }


            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult NotFound()
        {

            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //[PermissionRequirement(PermissionConstant.IsView)]
        //[Route("View/{id?}")]

        public async Task<IActionResult> Home()
        {
            int userType = Convert.ToInt32(_claimPincipal.FindFirstValue("UserType"));
            int beneficiaryType = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            int postId = Convert.ToInt32(_claimPincipal.FindFirstValue("PostId"));
            int roleIds = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            int servicesubtypeid = 0;

            IEnumerable<ServiceMaster> model = await _ihomeService.BindServicesUserWiseFilter(userType, beneficiaryType, postId, roleIds, servicesubtypeid);
            var endrole = model.ToList()[0].ApprovalEndRoleId;
            var levelno = await _ihomeService.GetLevelNoFromPostId(Convert.ToInt64(postId));
            ViewBag.LastLevel = false;
            if (levelno != null)
            {
                if (Convert.ToInt32(levelno) == endrole)
                {
                    ViewBag.LastLevel = true;
                }
            }
            return View(model);
        }


        public async Task<IActionResult> OtherHome()
        {
            int userType = Convert.ToInt32(_claimPincipal.FindFirstValue("UserType"));
            int beneficiaryType = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            int postId = Convert.ToInt32(_claimPincipal.FindFirstValue("PostId"));
            int roleIds = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            int servicesubtypeid = 0;

            IEnumerable<ServiceMaster> model = await _ihomeService.BindServicesUserWiseFilter(userType, beneficiaryType, postId, roleIds, servicesubtypeid);
            var endrole = model.ToList()[0].ApprovalEndRoleId;
            var levelno = await _ihomeService.GetLevelNoFromPostId(Convert.ToInt64(postId));
            ViewBag.LastLevel = false;
            if (levelno != null)
            {
                if (Convert.ToInt32(levelno) == endrole)
                {
                    ViewBag.LastLevel = true;
                }
            }
            return View(model);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetSchemeDescription(string ServiceId)
        {
            var registrationid = Convert.ToInt64(_claimPincipal.FindFirstValue("RegistrationId"));
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(ServiceId),registrationid);
            // model.ServiceId = Convert.ToInt64(HttpUtility.UrlEncode(CommonUtils.Encrypt(ServiceId)));
            return PartialView("_SchemeDescription", model);
        }



        [HttpGet]
        public async Task<IActionResult> GetLocalAuthorityDetailsPopup(EmpApproveApplication empApproveApplication)
        {
            return PartialView("_LocalAuthorityDetailsPopup");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SchemeUsers(SchemeUserModel schemeUserModel)
        {
            List<SchemeUserModel> schemeUserModel1 = await _schemeUserServices.GetSchemeUser(schemeUserModel);
            return View(schemeUserModel1);
        }


        //public async Task<IActionResult> ForgetPasswordReset(int UserId)
        //{
        //    return View();
        //}
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPasswordReset(ForgetPassword uForgetPassword)
        {
            ModelState.Clear();
            return View(uForgetPassword);
        }


        [HttpGet]
        public async Task<IActionResult> Getaadharcardcountbyaadharnoandserviceid(string aadharcardno, long serviceId)
        {
            var model = await _ihomeService.getaadharcardcountbyaadharnoandserviceid(aadharcardno, serviceId);

            return Json(new { data = model });
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetUserId(Usermaster usermaster)
        {
            var model = await _ihomeService.GetUserId(usermaster);
            ModelState.Clear();
            if (model != null)
            {
                var msg = "Your User ID is : " + model.UserName;
                TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var msg = "Aadharcard number or birth date is incorrect";
                TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                return RedirectToAction("Index", "Home");
            }
        }

        [AllowAnonymous]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCompany()
        {
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            var model = await _ihomeService.GetPersonalDetailsByRegNo(RegistrationNo);
            ViewBag.GLWBRegistrationAPI = _glwbRegistrationAPI;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCompany(PersonalDetailsModel personalDetailsModel)
        {

            try
            {

                personalDetailsModel.RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));

                ResponseMessage regResponse = await _ihomeService.UpdateGLWBUserCompany(personalDetailsModel);
                if (regResponse != null)
                {
                    string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                    if (regResponse != null && regResponse.Error == 1)
                    {
                        TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                        ModelState.Clear();
                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {
                        TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                        ModelState.Clear();
                        Registration empEmpty = new Registration();
                        return RedirectToAction("UpdateCompany", "Home");
                    }
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                    return RedirectToAction("UpdateCompany", "Home", personalDetailsModel);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
