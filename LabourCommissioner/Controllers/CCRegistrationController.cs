using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace LabourCommissioner.Controllers
{
    public class CCRegistrationController : Controller
    {
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICCRegistrationService _iccregistrationService;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpClientFactory _clientFactory;

        private readonly string _citizenregTemplateId;
        private readonly string _bocwEmailLogo;
        private readonly string _glwbEmailLogo;


        public CCRegistrationController(ICCRegistrationService ccregistrationService, IConfiguration config, IHttpClientFactory clientFactory, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _iccregistrationService = ccregistrationService ?? throw new ArgumentNullException(nameof(ccregistrationService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _citizenregTemplateId = _config["SMSTemplateId:TCitizenRegistration"];
            _bocwEmailLogo = _config["EmailLogo:BOCW"];
            _glwbEmailLogo = _config["EmailLogo:GLWB"];

        }



        //[Route("Registration")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Registration()
        {
            CCRegistration cCRegistration = new CCRegistration();
            var userTypeModel = _iccregistrationService.GetCCUserType("CCUserType");
            var userType = userTypeModel.Result.ToList();
            ViewBag.UserTypeList = userType;
            return View(cCRegistration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddRegistration(CCRegistration registration)
        {

            try
            {
                //Remove Local Authority Model Properites from model state
                ModelState.Remove("resname");
                ModelState.Remove("resdesignation");
                ModelState.Remove("resmobileno");
                ModelState.Remove("locname");
                ModelState.Remove("locdesignation");
                ModelState.Remove("locmobileno");
                if (ModelState.IsValid)
                {

                    registration.ipaddress = CommonUtils.GetLocalIPAddress();
                    registration.hostname = CommonUtils.GetHostName();
                    ResponseMessage regResponse = await _iccregistrationService.AddUpdateRegistration(registration);
                    if (regResponse != null)
                    {
                        string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                        if (regResponse != null && regResponse.Error == 0)
                        {
                            CommonUtils commonFunction = new CommonUtils(_config);
                            string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                            var res = commonFunction.SendCCRegisteredMail(registration.Name, registration.EmailId, Convert.ToString(regResponse.Id), registration.Password, _bocwEmailLogo , rootPath, "Registration.html", 1);

                            var msg = "સન્માન પોર્ટલ પર તમારું User Id : " + Convert.ToString(regResponse.Id) + "  Password : " + registration.Password + " - LSEDEPT";
                            bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(msg, registration.MobileNo, _citizenregTemplateId);

                            if (isSendSMS)
                            {
                                await _iccregistrationService.AddSMSLogs(registration.MobileNo, 0, msg, Convert.ToInt32(regResponse.registrationId));
                            }

                            TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                            ModelState.Clear();
                            Registration empEmpty = new Registration();
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                            ModelState.Clear();
                            Registration empEmpty = new Registration();
                            return RedirectToAction("Registration", "CCRegistration");
                        }
                    }
                    else
                    {
                        TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                        return RedirectToAction("Registration", "CCRegistration", registration);
                    }

                }
                return RedirectToAction("Registration", "CCRegistration", registration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult UserAlreadyExist(string? PANTANNo)
        {
            Task<bool> isExist = _iccregistrationService.UserAlreadyExist(PANTANNo);
            if (isExist.Result)
                return Json(false);
            else
                return Json(true);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddLocalAuthority(CCRegistration registration)
        {

            try
            {


                registration.RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
                registration.ipaddress = CommonUtils.GetLocalIPAddress();
                registration.hostname = CommonUtils.GetHostName();

                ResponseMessage regResponse = await _iccregistrationService.AddUpdateAuthorityDetails(registration);
                if (regResponse != null)
                {
                    string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                    if (regResponse != null && regResponse.Error == 0)
                    {
                        Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);

                        obj.AddUpdateClaim("isFilledAuthority", "1");
                        TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                        ModelState.Clear();
                        Registration empEmpty = new Registration();
                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {
                        TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                        ModelState.Clear();
                        Registration empEmpty = new Registration();
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                    return RedirectToAction("Registration", "CCRegistration", registration);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
