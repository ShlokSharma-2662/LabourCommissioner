using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sun.security.util;
using System.Reflection;

namespace LabourCommissioner.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRegistrationService _iregistrationService;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string _citizenregTemplateId;
        private readonly string _bocwEmailLogo;
        private readonly string _glwbEmailLogo;


        public RegistrationController(IRegistrationService registrationService, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _iregistrationService = registrationService ?? throw new ArgumentNullException(nameof(registrationService));
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _citizenregTemplateId = _config["SMSTemplateId:TCitizenRegistration"];
            _bocwEmailLogo = _config["EmailLogo:BOCW"];
            _glwbEmailLogo = _config["EmailLogo:GLWB"];

        }



        [Route("Registration")]
        public IActionResult Index()
        {
            //var vm = new Registration
            //{
            //    Gender = new List<Gender>
            //    {
            //        new Gender {Value = 1, Text = "Male"},
            //        new Gender {Value = 2, Text = "Female"}
            //}
            //};


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddRegistration(Registration registration)
        {
            var Name = "";
            var Email = "";
            var Password = "";
            var MobileNo = "";
            try
            {
                if (registration.BeneficiaryType == 6)
                {
                    ModelState.Remove("Name");
                    ModelState.Remove("DateOfBirth");
                    ModelState.Remove("MobileNo");
                    ModelState.Remove("EmailId");
                    ModelState.Remove("Gender");
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");

                    Name = registration.OtherUserName;
                    Email = registration.OtherUserEmailId;
                    Password = registration.OtherUserPassword;
                    MobileNo = registration.OtherUserMobileNo;

                }
                else
                {
                    ModelState.Remove("OtherUserConfirmPassword");
                    ModelState.Remove("OtherUserPassword");
                    ModelState.Remove("OtherUserEmailId");
                    ModelState.Remove("OtherUserMobileNo");
                    ModelState.Remove("OtherUserGender");
                    ModelState.Remove("OtherUserName");

                    Name = registration.Name;
                    Email = registration.EmailId;
                    Password = registration.Password;
                    MobileNo = registration.MobileNo;

                }

                if (ModelState.IsValid)
                {
                    long id = registration.RegistrationId;
                    bool isValidnumber = aadharcard.validateVerhoeff(registration.regunique.UniqueIdnumber.ToString());
                    if (isValidnumber)
                    {
                        registration.ipaddress = CommonUtils.GetLocalIPAddress();
                        registration.hostname = CommonUtils.GetHostName();
                        int? beneficiarytype = registration.BeneficiaryType;
                        ResponseMessage regResponse = await _iregistrationService.AddUpdateRegistration(registration);
                        if (regResponse != null)
                        {
                            string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                            if (regResponse != null && regResponse.Error == 0)
                            {
                                CommonUtils commonFunction = new CommonUtils(_config);
                                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                                var res = commonFunction.SendCitizenRegisteredMail(Name, Email, Convert.ToString(regResponse.Id), Password, registration.BeneficiaryType == 1 ? _bocwEmailLogo : _glwbEmailLogo, rootPath, "Registration.html", beneficiarytype);

                                var msg = "સન્માન પોર્ટલ પર તમારું User Id : " + Convert.ToString(regResponse.Id) + "  Password : " + registration.Password + " - LSEDEPT";
                                bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(msg, MobileNo, _citizenregTemplateId);

                                if (isSendSMS)
                                {
                                    await _iregistrationService.AddSMSLogs(MobileNo, 0, msg, Convert.ToInt32(regResponse.registrationId));
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
                                return RedirectToAction("Registration", "Home");
                            }
                        }
                        else
                        {
                            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                            return RedirectToAction("Registration", "Home", registration);
                        }
                    }
                    else
                    {
                        //ViewData["Message"] = "Aadhar Card Is Not Valid.";
                        TempData["Message"] = CommonUtils.ConcatString("Aadhar Card Is Not Valid.", Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                        return RedirectToAction("Registration", "Home", registration);
                    }
                }
                return RedirectToAction("Registration", "Home", registration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
