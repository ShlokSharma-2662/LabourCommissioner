using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using LabourCommissionerAPI.ResponseModel;
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
    [Authorize]
    public class HomeController : ControllerBase
    {

        private readonly IHomeService _ihomeService;
        //private readonly IHomeRepository _homeRepository;
        //private readonly ISchemeService _iscchemeService;
        //private readonly ISchemeUserServices _schemeUserServices;
        //private readonly ClaimsPrincipal _claimPincipal;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IStringLocalizer<HomeController> _localizer;
        //private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
        //private readonly IConfiguration _config;
        //private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly string _isexceptionmailrequired;
        //private readonly string _bocwRegistrationAPI;
        //private readonly string _glwbRegistrationAPI;
        ApiResponse apiResponse = new ApiResponse();


        public HomeController(IHomeService homeService)

        {
            _ihomeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
            //_homeRepository = homeRepository;
            //_iscchemeService = schemeService ?? throw new ArgumentNullException(nameof(schemeService));
            //_schemeUserServices = schemeUserServices;
            //_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            //_claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            //_localizer = localizer;
            //_htmlLocalizer = htmlLocalizer;
            //_config = config;
            //_webHostEnvironment = webHostEnvironment;
            //_isexceptionmailrequired = _config["SMTPConfig:_IsExceptionMailRequired"];
            //_bocwRegistrationAPI = _config["RegistrationAPI:BOCW"];
            //_glwbRegistrationAPI = _config["RegistrationAPI:GLWB"];
        }


        [HttpGet("home")]
        public async Task<IActionResult> Home(int userType, int beneficiaryType, int postId, int roleIds, int servicesubtypeid)
        {
            try
            {
                var serviceMasters = await _ihomeService.BindServicesUserWiseFilter(userType, beneficiaryType, postId, roleIds, servicesubtypeid);

                if (serviceMasters != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = serviceMasters;
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

        [HttpGet("getSchemeDescription")]
        public async Task<IActionResult> GetSchemeDescription(long registrationId, string ServiceId)
        {
            try
            {
                var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(ServiceId), registrationId);

                if (model != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Description_Successfull);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Description_Not_Successfull);
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

        [HttpGet("applicationDetails")]
        public async Task<IActionResult> ApplicationDetails(long registrationId, long serviceId)
        {


            try
            {
                
                ServiceMaster objServiceMaster = await _ihomeService.GetSchemeByServiceId(serviceId,registrationId);
                ResponseMessage model1 = await _ihomeService.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);


                if (serviceId == 7 || serviceId == 2 || serviceId == 5 || serviceId == 4 || serviceId == 6 || serviceId == 15 || serviceId == 14 ||
                    serviceId == 3 || serviceId == 20 || serviceId == 8 || serviceId == 22 || serviceId == 27 || serviceId == 12 || serviceId == 18 ||
                    serviceId == 19 || serviceId == 23 || serviceId == 24 || serviceId == 16 || serviceId == 11 || serviceId == 13 || serviceId == 9 ||
                    serviceId == 21 || serviceId == 30 || serviceId == 10 && serviceId == 32 && serviceId == 34 && model1 != null)
                {

                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model1;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.ApplicationDetails_Succefull);
                    apiResponse.StackTrace = null;
                }

                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.ApplicationDetails_Not_Successfull);
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

        [HttpGet("ApplicationDetailss")]
        public async Task<IActionResult> ApplicationDetailss(long serviceId, string schemaName, string tableName, long registrationId)
        {


            try
            {
                var model = await _ihomeService.GetApplicationDetails(registrationId, serviceId, schemaName, tableName);


                if (model != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Application_Details_Get_Success);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Application_No_Record_Found);
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

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //[AllowAnonymous]
        //[HttpGet("error")]
        //public IActionResult Error()
        //{            
        //    try
        //    {

        //        var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        //        if (exceptionDetails != null)
        //        {
        //            ExceptionLogger exceptionLogger = new ExceptionLogger();
        //            UserCoockiesModel userCoockiesModel = new UserCoockiesModel();
        //            exceptionLogger.UserId = Convert.ToInt32(userCoockiesModel.RegistrationId);
        //            exceptionLogger.ControllerName = Convert.ToString(((object[])((ExceptionHandlerFeature)exceptionDetails).RouteValues.Values)[0]);
        //            exceptionLogger.ActionName = Convert.ToString(((object[])((ExceptionHandlerFeature)exceptionDetails).RouteValues.Values)[1]);
        //            exceptionLogger.ExceptionMessage = Convert.ToString(exceptionDetails.Error.Message);
        //            exceptionLogger.ExceptionStackTrace = Convert.ToString(exceptionDetails.Error.StackTrace);
        //            exceptionLogger.RequestURI = Convert.ToString(exceptionDetails.Path);
        //            string LineNumber = Convert.ToString(exceptionDetails.Error.StackTrace.Substring(exceptionDetails.Error.StackTrace.IndexOf(' ')));
        //            LineNumber = Regex.Match(LineNumber, @"\d+").Value;
        //            exceptionLogger.LineNumber = LineNumber == "" ? 0 : Convert.ToInt32(LineNumber);
        //            exceptionLogger.LogFrom = Convert.ToInt32((int)EnumLookup.exceptionlogfrom.API);
        //            exceptionLogger.IpAddress = CommonUtils.GetLocalIPAddress();
        //            exceptionLogger.HostName = CommonUtils.GetHostName();

        //            var model = _ihomeService.AddExceptionLog(exceptionLogger);
        //            if (_isexceptionmailrequired == "1")
        //            {
        //                CommonUtils commonFunction = new CommonUtils(_config);
        //                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
        //                var res = commonFunction.SendExceptionMail(exceptionLogger, rootPath, "ExceptionMail.html");
        //            }
        //            apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
        //            apiResponse.Result = model;
        //            apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
        //            apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.ApplicationDetails_Succefull);
        //            apiResponse.StackTrace = null;
        //        }                
        //        else
        //        {
        //            apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
        //            apiResponse.Result = null;
        //            apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
        //            apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.ApplicationDetails_Not_Successfull);
        //            apiResponse.StackTrace = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
        //        apiResponse.Result = null;
        //        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
        //        apiResponse.Message = ex.StackTrace;
        //    }

        //    return Ok(apiResponse);            
        //}

        [AllowAnonymous]
        [HttpPost("GetUserId")]
        public async Task<IActionResult> GetUserId(Usermaster usermaster)
        {

            try
            {
                    var model = await _ihomeService.GetUserId(usermaster);
                    ModelState.Clear();


                    if (model != null)
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                        apiResponse.Result = model;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.User_get_Successfull);
                        apiResponse.StackTrace = null;
                    }
                    else
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                        apiResponse.Result = null;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.User_Not_found);
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

        [HttpGet("ForgetPasswordReset")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPasswordReset(ForgetPassword uForgetPassword)
        {
            try
            {
                ModelState.Clear();
                // return View(uForgetPassword);

                if (uForgetPassword != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = uForgetPassword;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Reset_Password_Successfull);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Reset_Not_successfully);
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

