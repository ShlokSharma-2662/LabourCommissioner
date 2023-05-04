using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Convert = System.Convert;
using Microsoft.AspNetCore.Authorization;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissioner.Controllers
{
    public class GLWBLaptopSubsidyYojnaController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGLWBLaptopSubsidyYojnaService _iGLWBLaptopSubsidyYojnaservice;


        public GLWBLaptopSubsidyYojnaController(IGLWBLaptopSubsidyYojnaService iGLWBLaptopSubsidyYojnaService, IConfiguration config, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iGLWBLaptopSubsidyYojnaservice = iGLWBLaptopSubsidyYojnaService ??
                                              throw new ArgumentNullException(nameof(_iGLWBLaptopSubsidyYojnaservice));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext!.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult AppPersonalDetails(string strid, string strTabId, string strApplicationId)
        {
            var RegistrationID = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            int id = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strid)));
            int TabId = 0;
            int ApplicationId;
            if (strApplicationId == null || strApplicationId == "")
            {
                ApplicationId = 0;
            }
            else
            {
                ApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));

            }
            var model = _iGLWBLaptopSubsidyYojnaservice.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.glwb_lsy), nameof(EnumLookup.tablename.tabentry));
            ViewBag.AppPersonalDetails = model.Result.ToList();
            ViewBag.ApplicationId = ApplicationId;

            return View();
        }


        //manually adeed 

        [HttpGet]
        public async Task<IActionResult> PersonalDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;


            // long ApplicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            long ApplicationId = Convert.ToInt64(strApplicationId);

            var model = await _iGLWBLaptopSubsidyYojnaservice.GetPersonalDetailsByRegNo(RegistrationNo);
            var model1 = await _iGLWBLaptopSubsidyYojnaservice.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.glwb_lsy), nameof(EnumLookup.tablename.personaldetails));
            //model1.ICardFromDate = model.ICardFromDate;
            //model1.ICardToDate = model.ICardToDate;

            ViewBag.Message = CommonUtils.ConcatString("જો શારીરિક રીતે શક્ષમ હોય તો ઉંમર ૩૦ થી વધારે ના હોવી જોઈએ.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            model.ApplicationNo = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.ServiceId = ServiceId;
            ViewBag.TabSequenceNo = 1;

            var stateModel = _iGLWBLaptopSubsidyYojnaservice.GetAllStates();
            var states = stateModel.Result.ToList();

            ViewBag.States = states;
            ViewBag.isFilled = isFilled;

            var districtModel = _iGLWBLaptopSubsidyYojnaservice.GetDistrict();
            var district = districtModel.Result.ToList();

            ViewBag.District = district;
            ViewBag.ApplicationId = ApplicationId;
            if (isFilled)
            {
                model1.ICardFromDate = model.ICardFromDate;
                model1.ICardToDate = model.ICardToDate;
                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(model1.CouchDBDocId, model1.FileName);
                var base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ViewBag.Image = base64Image;

                //var PtalukaModel = _iGLWBLaptopSubsidyYojnaservice.GetTalukaByDistrictId(model1.PDistrictId);
                //var Ptaluka = PtalukaModel.Result.ToList();

                //var PvillageModel =
                //    _iGLWBLaptopSubsidyYojnaservice.GetVillageByDistrictIdAndTalukaId(model1.PDistrictId, model1.PTalukaId);
                //var Pvillage = PvillageModel.Result.ToList();

                var talukaModel = _iGLWBLaptopSubsidyYojnaservice.GetTalukaByDistrictId(model1.CDistrictId);
                var Ctaluka = talukaModel.Result.ToList();

                var villageModel =
                    _iGLWBLaptopSubsidyYojnaservice.GetVillageByDistrictIdAndTalukaId(model1.CDistrictId, model1.CTalukaId);
                var Cvillage = villageModel.Result.ToList();

                ViewBag.CTaluka = Ctaluka;
                ViewBag.CVillage = Cvillage;
                //ViewBag.PTaluka = Ptaluka;
                //ViewBag.PVillage = Pvillage;
                model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                return PartialView("GLWBLaptopSubsidyYojna/_GLWBLaptopSubsidyYojnaPersonalDetails", model1);
            }

            return PartialView("GLWBLaptopSubsidyYojna/_GLWBLaptopSubsidyYojnaPersonalDetails", model);


            //return PartialView("GLWBLaptopSubsidyYojna/_GLWBLaptopSubsidyYojnaPersonalDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBLaptopSubsidyYojnaSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            long ApplicationId = Convert.ToInt64(strApplicationId);
            int serviceId = Convert.ToInt32(ServiceId);




            var model1 = await _iGLWBLaptopSubsidyYojnaservice.GetApplicationSchemeDetailsByAppId(ApplicationId);

            var val = 10;

            if (model1 != null)
            {
                var degree = _iGLWBLaptopSubsidyYojnaservice.GetSubject(val);
                var degreeList = degree.Result.ToList();
                ViewBag.Degree = degreeList;
                ViewBag.CourseId = model1.courseid;
            }


            ViewBag.isFilled = isFilled;
            //string currYear = DateTime.Now.Year.ToString();
            //List<SelectListItem> yearList = new List<SelectListItem>();
            //yearList.Add(new SelectListItem
            //{
            //    Text = currYear,
            //    Value = currYear,
            //    Selected = true
            //});
            long startYear = 2021;
            long currYear = DateTime.Now.Year;

            List<SelectListItem> yearList = new List<SelectListItem>();

            while (startYear <= currYear)
            {
                yearList.Add(new SelectListItem
                {
                    Text = startYear.ToString(),
                    Value = startYear.ToString(),
                    Selected = true
                });
                startYear++;
            }
            List<SelectListItem> std = new List<SelectListItem>();
            std.Add(new SelectListItem
            {
                Text = "10th",
                Value = "10",
                Selected = true
            });

            ViewBag.Year = yearList;
            //var totalsahay = _iGLWBLaptopSubsidyYojnaservice.GetTotalsahayByServiceID(serviceId);
            //ViewBag.TotalSahay = totalsahay.Result.totalsahay;

            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;
            ViewBag.ApplicationId = ApplicationId;
            return PartialView("GLWBLaptopSubsidyYojna/_GLWBLaptopSubsidyYojnaSchemeDetails", model1);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBLaptopSubsidyYojnaDocument(string ServiceId, string strApplicationId, bool isFilled)
        {

            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 3;
            var model = await _iGLWBLaptopSubsidyYojnaservice.GetDocumentFileDetails(Convert.ToInt32(ServiceId), Convert.ToInt32(ApplicationId));


            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iGLWBLaptopSubsidyYojnaservice.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.glwb_lsy), nameof(EnumLookup.tablename.documentdetails));
                return PartialView("GLWBLaptopSubsidyYojna/_GLWBLaptopSubsidyYojnaDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("GLWBLaptopSubsidyYojna/_GLWBLaptopSubsidyYojnaDocument", model);
            }
        }

        [HttpGet]
        public IActionResult GLWBLaptopSubsidyYojnaTermsCondition(string ServiceId, string strApplicationId, bool isFilled)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("GLWBLaptopSubsidyYojna/_GLWBLaptopSubsidyYojnaTermsCondition");
        }

        //[HttpPost]

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _iGLWBLaptopSubsidyYojnaservice.GetDistrict();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetSubject(int subjectId)
        {
            var subject = _iGLWBLaptopSubsidyYojnaservice.GetSubject(subjectId);
            return Json(new { data = subject });
        }

        [HttpGet]
        public IActionResult GetTalukaByDistrictId(int districtId)
        {
            var regions = _iGLWBLaptopSubsidyYojnaservice.GetTalukaByDistrictId(districtId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }


        [HttpGet]
        public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var regions = _iGLWBLaptopSubsidyYojnaservice.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public ActionResult GetEducation()
        {
            var regions = _iGLWBLaptopSubsidyYojnaservice.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddUpdateApplication(GLWBLSY_PersonalDetails personalDetails, string strTabId, string strApplicationNo)
        {
            int TabSequenceNo = Convert.ToInt32(strTabId);
            string ApplicationNo = strApplicationNo;
            personalDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            personalDetails.HostName = CommonUtils.GetHostName();
            personalDetails.RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            personalDetails.RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            personalDetails.schemaname = nameof(EnumLookup.schemaname.glwb_lsy);
            personalDetails.tablename = nameof(EnumLookup.tablename.personaldetails);
            personalDetails.applicationfrom = Convert.ToInt32(EnumLookup.applicationfrom.web);

            if (personalDetails.Photo != null)
            {
                var extension = Path.GetExtension(personalDetails.Photo.FileName);
                personalDetails.FileName = personalDetails.RegistrationId + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Convert.ToString(extension);
            }
            personalDetails.CreatedDate = DateTime.Now;
            personalDetails.IsDeleted = false;
            personalDetails.ApplicationNo = ApplicationNo;
            personalDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            personalDetails.AadharCardNo = CommonUtils.EncryptCRY(personalDetails.AadharCardNo);

            #region Upload Profile Photo to CouchDB

            var couchDBRequest = new CouchDBRequest();
            var couchDBResponse = new CouchDBResponse();

            if (personalDetails.Photo != null && personalDetails.FileName != "" && personalDetails.Photo.Length > 0)
            {
                //Insert new attachment

                byte[] fileBytes = null;
                using (var msstream = new MemoryStream())
                {
                    await personalDetails.Photo.CopyToAsync(msstream);
                    fileBytes = msstream.ToArray();
                }

                couchDBRequest.FileName = personalDetails.FileName;
                couchDBRequest.AttachmentData = fileBytes;
                couchDBRequest.FileExtension = Path.GetExtension(personalDetails.FileName).Replace(".", "").ToUpper();
                couchDBRequest.CreatedOn = DateTime.Now.ToString();
                couchDBResponse = await new CommonUtils(_config, _clientFactory).UplodToCouchDB(couchDBRequest);
                if (couchDBResponse != null && couchDBResponse.IsSuccess)
                {
                    personalDetails.CouchDBDocId = couchDBResponse.Id;
                    personalDetails.CouchDBDocRevId = couchDBResponse.Rev;
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                        Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                    return RedirectToAction("AppPersonalDetails", "GLWBLaptopSubsidyYojna", personalDetails);
                }
            }

            #endregion


            var regResponse = await _iGLWBLaptopSubsidyYojnaservice.AddUpdateApplication(personalDetails);
            if (regResponse != null)
            {
                //string errorMsg = regResponse.ResponseMsg == null ? "Somthing went wrong please try again." : regResponse.ResponseMsg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    //TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                    ModelState.Clear();
                    var empEmpty = new PersonalDetailsModel();
                    //return RedirectToAction("BOCWSikshanSahayYojanaSchemeDetails", "BOCWSikshanSahayYojana");
                    return RedirectToAction("AppPersonalDetails",
                        new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(personalDetails.ServiceId.ToString())), strTabId = "1", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())) });
                }
                else
                {
                    //TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    var empEmpty = new Registration();
                    return RedirectToAction("AppPersonalDetails", "GLWBLaptopSubsidyYojna");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "GLWBLaptopSubsidyYojna", personalDetails);

            //return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", personalDetails);

            return RedirectToAction("GLWBLaptopSubsidyYojnaSchemeDetails", "GLWBLaptopSubsidyYojna");
        }

        public async Task<IActionResult> AddSchemeDetails(GLWBLSY_SchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            schemeDetails.ApplicationId = ApplicationId;
            schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            schemeDetails.HostName = CommonUtils.GetHostName();
            schemeDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            schemeDetails.TabSequenceNo = TabSequenceNo;
            schemeDetails.schemaname = nameof(EnumLookup.schemaname.glwb_lsy);
            schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
            // schemeDetails.UserId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var regResponse = await _iGLWBLaptopSubsidyYojnaservice.AddSchemeDetails(schemeDetails);
            return RedirectToAction("AppPersonalDetails",
                new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(schemeDetails.ServiceId.ToString())), strTabId = "2", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())) });
        }

        public async Task<IActionResult> UploadDocument(IList<DocumentFileDetails> documentFileDetails, string strTabSequenceNo, string strApplicationId, string strServiceId)
        {
            //if(ModelState.Is)
            //return RedirectToAction("AppPersonalDetails", new { id = 2, TabId = 4 });
            //if (ModelState.IsValid)
            //{
            //  int TabSequenceNo = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strTabSequenceNo)));
            int TabSequenceNo = Convert.ToInt32(strTabSequenceNo);
            int ApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            int ServiceId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));

            if (documentFileDetails != null && documentFileDetails.Count() > 0)
            {
                var lstdocumentFileDetails = new List<DocumentFileDetails>();
                var RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
                foreach (var item in documentFileDetails)
                {
                    var objdocumentFileDetails = new DocumentFileDetails();
                    var couchDBRequest = new CouchDBRequest();
                    var couchDBResponse = new CouchDBResponse();

                    if (item.IdImage != null && item.IdImage.File != null && item.IdImage.File.FileName != "" &&
                        item.IdImage.File.Length > 0)
                    {


                        var extension = Path.GetExtension(item.IdImage.File.FileName);
                        string FileName = RegistrationId + "_" + item.DocumentName + "_" + DateTime.Now.ToString("yyyyMMDDhhmmss") +
                                                   Convert.ToString(extension);
                        //Insert new attachment

                        byte[] fileBytes = null;
                        using (var msstream = new MemoryStream())
                        {
                            await item.IdImage.File.CopyToAsync(msstream);
                            fileBytes = msstream.ToArray();
                        }

                        couchDBRequest.FileName = FileName;//item.IdImage.File.FileName;
                        couchDBRequest.AttachmentData = fileBytes;
                        couchDBRequest.FileExtension =
                            Path.GetExtension(item.IdImage.File.FileName).Replace(".", "").ToUpper();
                        ;
                        couchDBRequest.CreatedOn = DateTime.Now.ToString();
                        couchDBResponse = await new CommonUtils(_config, _clientFactory).UplodToCouchDB(couchDBRequest);
                        if (couchDBResponse != null && couchDBResponse.IsSuccess)
                        {
                            objdocumentFileDetails.ApplicationId = ApplicationId;
                            objdocumentFileDetails.DocumentMasterId = item.DocumentMasterId;
                            objdocumentFileDetails.DocumentName = FileName;
                            objdocumentFileDetails.DocumentNumber = item.DocumentNumber;
                            objdocumentFileDetails.CouchDBDocId = couchDBResponse.Id;
                            objdocumentFileDetails.CouchDBDocRevId = couchDBResponse.Rev;
                            objdocumentFileDetails.ServiceId = ServiceId;
                            objdocumentFileDetails.CreatedBy = RegistrationId;
                            objdocumentFileDetails.IpAddress = CommonUtils.GetLocalIPAddress();
                            objdocumentFileDetails.HostName = CommonUtils.GetHostName();
                            objdocumentFileDetails.TabSequenceNo = item.TabSequenceNo;
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.bocw_ssy);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
                        }
                        else
                        {
                            return PartialView("GLWBLaptopSubsidyYojna/_GLWBLaptopSubsidyYojnaDocument");
                        }
                    }
                }

                var dtData = CommonUtils.ToDataTable(lstdocumentFileDetails);
                dtData.Columns.Remove("DocumentNameGuj");
                dtData.Columns.Remove("ServiceDocumentType");
                dtData.Columns.Remove("DocumentTypeIds");
                dtData.Columns.Remove("IsCompulsary");
                dtData.Columns.Remove("IsNumberInput");
                dtData.Columns.Remove("IsActive");
                dtData.Columns.Remove("IdImage");

                ResponseMessage responseMessage = new ResponseMessage();
                if (dtData.Rows.Count > 0)
                {
                    responseMessage = await _iGLWBLaptopSubsidyYojnaservice.AddUpdateDocumentDetailsNew(dtData);
                }
                if (dtData.Rows.Count == 0 && responseMessage != null && responseMessage.Error == 0)
                {
                    responseMessage.ApplicationNo = ApplicationId;
                    responseMessage.Id = ServiceId;
                }
                //ResponseMessage model = await _iBOCWSikshanSahayYojanaService.AddUpdateDocumentDetails(lstdocumentFileDetails);
                return RedirectToAction("AppPersonalDetails",
                    new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.Id.ToString())), strTabId = "3", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.ApplicationNo.ToString())) });
            }


            //return View();
            return RedirectToAction("AppPersonalDetails", "GLWBLaptopSubsidyYojna", documentFileDetails);
        }

        [HttpPost]
        public async Task<IActionResult> FinalSubmit(FinalSubmitModel finalSubmitModel, string strApplicationId, string strServiceId, string strTabSequenceNo)
        {

            long ApplicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            long TabSequenceNo = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strTabSequenceNo)));

            finalSubmitModel.ApplicationId = ApplicationId;
            finalSubmitModel.serviceid = serviceId;
            finalSubmitModel.tabsequenceno = TabSequenceNo;
            finalSubmitModel.ipaddress = CommonUtils.GetLocalIPAddress();
            finalSubmitModel.hostname = CommonUtils.GetHostName();
            finalSubmitModel.ResigtrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            //finalSubmitModel.tablename = "bocw_ssy.personaldetails";
            //finalSubmitModel.schemaname = "bocw_ssy";
            finalSubmitModel.tablename = nameof(EnumLookup.tablename.personaldetails);
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.glwb_lsy);
            //  finalSubmitModel.userid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";

            var regResponse = await _iGLWBLaptopSubsidyYojnaservice.FinalSubmit(finalSubmitModel);
            if (regResponse != null)
                if (regResponse != null && regResponse.Error == 0)
                {

                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                    SMSModel modelSMSModel = await _iGLWBLaptopSubsidyYojnaservice.GetSmsContentForService(serviceId, ApplicationId, (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.glwb_lsy), nameof(EnumLookup.tablename.personaldetails));
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iGLWBLaptopSubsidyYojnaservice.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                        }

                    }


                    var msg = regResponse.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
                }

            var errorMsg = "Form Subbmission Failed..!!";
            //  TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");

            return RedirectToAction("TermsCondition", "GLWBLaptopSubsidyYojna");
        }

        public async Task<FileResult> DownloadDocument(string id, string fileName)
        {
            try
            {
                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetDocumentByteArray(id, fileName);

                if (objCouchDBResponse != null)
                {
                    byte[] fileBytes = await objCouchDBResponse.ImageData;
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }

                return File("", "");
            }
            catch (Exception e)
            {
                throw e;
            }




        }


    }
}
