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
using Convert = System.Convert;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using Microsoft.AspNetCore.Authorization;

namespace LabourCommissioner.Controllers
{
    [Authorize]
    public class GLWBPrasutiSahayBetiProtsahanYojnaController : Controller 
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGLWBPrasutiSahayBetiProtsahanYojnaService _iglwbprasutisahaybetiprotsahanyojnaservice;

        public GLWBPrasutiSahayBetiProtsahanYojnaController(IGLWBPrasutiSahayBetiProtsahanYojnaService iglwbprasutisahaybetiprotsahanyojnaservice, IConfiguration config, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iglwbprasutisahaybetiprotsahanyojnaservice = iglwbprasutisahaybetiprotsahanyojnaservice ??
                                              throw new ArgumentNullException(nameof(_iglwbprasutisahaybetiprotsahanyojnaservice));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext!.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult AppPersonalDetails(string strid, string strTabId, string strApplicationId)
        {

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
            //var model = _iglwbhscpurashkaryojanaservice.GetServiceTabByServiceId(id);
            var model = _iglwbprasutisahaybetiprotsahanyojnaservice.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.glwb_psy), nameof(EnumLookup.tablename.tabentry));
            ViewBag.AppPersonalDetails = model.Result.ToList();
            ViewBag.ApplicationId = ApplicationId;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Getaadharcardcountbyaadharnoandserviceid(string aadharcardno, long serviceId)
        {            
            var model =await _iglwbprasutisahaybetiprotsahanyojnaservice.getaadharcardcountbyaadharnoandserviceid(aadharcardno,serviceId);            
            
            return Json(new { data = model });
        }


        [HttpGet]
        public async Task<IActionResult> GLWBPrasutiSahayBetiProtsahanYojnaPersonalDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;

            

            // long ApplicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            long ApplicationId = Convert.ToInt64(strApplicationId);

            var model = await _iglwbprasutisahaybetiprotsahanyojnaservice.GetPersonalDetailsByRegNo(RegistrationNo);

            model.ApplicationNo = User.FindFirst(ClaimTypes.Name).Value;
            model.RegisteredAadharCardNo = model.AadharCardNo;
            ViewBag.ServiceId = ServiceId;
            ViewBag.TabSequenceNo = 1;

            //var aadharcardno = model.AadharCardNo;
            //var serviceId = Convert.ToInt64(ServiceId);
            //var Count = await _iglwbprasutisahaybetiprotsahanyojnaservice.getaadharcardcountbyaadharnoandserviceid(aadharcardno, serviceId);
            //ViewBag.Count = Count.AadharCardNo;
            
            var stateModel = _iglwbprasutisahaybetiprotsahanyojnaservice.GetAllStates();
            var states = stateModel.Result.ToList();

            ViewBag.States = states;
            ViewBag.isFilled = isFilled;

            var districtModel = _iglwbprasutisahaybetiprotsahanyojnaservice.GetDistrict();
            var district = districtModel.Result.ToList();

            ViewBag.District = district;
            ViewBag.ApplicationId = ApplicationId;

            

            if (isFilled)
            {
                var model1 = await _iglwbprasutisahaybetiprotsahanyojnaservice.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.glwb_psy), nameof(EnumLookup.tablename.personaldetails));

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(model1.CouchDBDocId, model1.FileName);
                var base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ViewBag.Image = base64Image;


                var talukaModel = _iglwbprasutisahaybetiprotsahanyojnaservice.GetTalukaByDistrictId(model1.CDistrictId);
                var Ctaluka = talukaModel.Result.ToList();

                var villageModel =
                    _iglwbprasutisahaybetiprotsahanyojnaservice.GetVillageByDistrictIdAndTalukaId(model1.CDistrictId, model1.CTalukaId);
                var Cvillage = villageModel.Result.ToList();

                ViewBag.CTaluka = Ctaluka;
                ViewBag.CVillage = Cvillage;
                model1.RegisteredAadharCardNo = model.AadharCardNo;
                model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                return PartialView("GLWBPrasutiSahayBetiProtsahanYojna/_GLWBPrasutiSahayBetiProtsahanYojnaPersonalDetails", model1);
            }

            return PartialView("GLWBPrasutiSahayBetiProtsahanYojna/_GLWBPrasutiSahayBetiProtsahanYojnaPersonalDetails", model);
        }

        public async Task<IActionResult> GLWBPrasutiSahayBetiProtsahanYojnaSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            long ApplicationId = Convert.ToInt64(strApplicationId);
            var model1 = await _iglwbprasutisahaybetiprotsahanyojnaservice.GetApplicationSchemeDetailsByAppId(ApplicationId);
            ViewBag.isFilled = isFilled;

            //int serviceId = Convert.ToInt32(ServiceId);
            //var totalsahay = _iglwbprasutisahaybetiprotsahanyojnaservice.GetTotalsahayByServiceID(serviceId);
            //ViewBag.TotalSahay = totalsahay.Result.totalsahay;

            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;
            // ViewBag.ApplicationId = ApplicationId;

            return PartialView("GLWBPrasutiSahayBetiProtsahanYojna/_GLWBPrasutiSahayBetiProtsahanYojnaSchemeDetails", model1);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBPrasutiSahayBetiProtsahanYojnaDocument(string ServiceId, string strApplicationId, bool isFilled)
        {
            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 3;
            var model = await _iglwbprasutisahaybetiprotsahanyojnaservice.GetDocumentFileDetails(Convert.ToInt32(ServiceId));


            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iglwbprasutisahaybetiprotsahanyojnaservice.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.glwb_psy), nameof(EnumLookup.tablename.documentdetails));
                return PartialView("GLWBPrasutiSahayBetiProtsahanYojna/_GLWBPrasutiSahayBetiProtsahanYojnaDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("GLWBPrasutiSahayBetiProtsahanYojna/_GLWBPrasutiSahayBetiProtsahanYojnaDocument", model);
            }
        }

        [HttpGet]
        public IActionResult GLWBPrasutiSahayBetiProtsahanYojnaTermsCondition(string ServiceId, string strApplicationId, bool isFilled)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("GLWBPrasutiSahayBetiProtsahanYojna/_GLWBPrasutiSahayBetiProtsahanYojnaTermsCondition");
        }

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _iglwbprasutisahaybetiprotsahanyojnaservice.GetDistrict();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetPSYTotalSahay(int serviceId,int male,int female)
        {
            var sahay = _iglwbprasutisahaybetiprotsahanyojnaservice.GetTotalsahayByServiceID(serviceId,male,female);
            // passing no of male child in relation parameter
            // passing no of female child in gender parameter            
            return Json(new { data = sahay});
        }

        [HttpGet]
        public IActionResult GetSubject(int subjectId)
        {
            var subject = _iglwbprasutisahaybetiprotsahanyojnaservice.GetSubject(subjectId);
            return Json(new { data = subject });
        }

        [HttpGet]
        public IActionResult GetTalukaByDistrictId(int districtId)
        {
            var regions = _iglwbprasutisahaybetiprotsahanyojnaservice.GetTalukaByDistrictId(districtId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }
        

        [HttpGet]
        public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var regions = _iglwbprasutisahaybetiprotsahanyojnaservice.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public ActionResult GetEducation()
        {
            var regions = _iglwbprasutisahaybetiprotsahanyojnaservice.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddUpdateApplication(GLWBPSY_PersonalDetailsModel personalDetails, string strTabId, string strApplicationNo)
        {
            int TabSequenceNo = Convert.ToInt32(strTabId);
            if (strApplicationNo == null)
            {
                strApplicationNo = "0";
            }
            string ApplicationNo = strApplicationNo;
            personalDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            personalDetails.HostName = CommonUtils.GetHostName();
            personalDetails.RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            personalDetails.RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            personalDetails.schemaname = nameof(EnumLookup.schemaname.glwb_psy);
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
                    return RedirectToAction("AppPersonalDetails", "GLWBPrasutiSahayBetiProtsahanYojna", personalDetails);
                }
            }

            #endregion


            var regResponse = await _iglwbprasutisahaybetiprotsahanyojnaservice.AddUpdateApplication(personalDetails);
            if (regResponse != null)
            {
                //string errorMsg = regResponse.ResponseMsg == null ? "Somthing went wrong please try again." : regResponse.ResponseMsg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    //TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                    ModelState.Clear();
                    var empEmpty = new GLWBHSC_PersonalDetailsModel();
                    //return RedirectToAction("GLWBHSCPurashkarYojanaSchemeDetails", "GLWBHSCPurashkarYojana");
                    return RedirectToAction("AppPersonalDetails",
                        new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(personalDetails.ServiceId.ToString())), strTabId = "1", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())) });
                }
                else
                {
                    //TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    var empEmpty = new Registration();
                    return RedirectToAction("AppPersonalDetails", "GLWBPrasutiSahayBetiProtsahanYojna");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "GLWBPrasutiSahayBetiProtsahanYojna", personalDetails);

            //return RedirectToAction("GLWBHSCPurashkarYojanaAppPersonalDetails", "GLWBHSCPurashkarYojana", personalDetails);

            return RedirectToAction("AppPersonalDetails", "GLWBPrasutiSahayBetiProtsahanYojna");
        }

        public async Task<IActionResult> AddSchemeDetails(GLWBPSY_SchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            schemeDetails.ApplicationId = ApplicationId;
            schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            schemeDetails.HostName = CommonUtils.GetHostName();
            schemeDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            schemeDetails.TabSequenceNo = TabSequenceNo;
            schemeDetails.schemaname = nameof(EnumLookup.schemaname.glwb_psy);
            schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
            // schemeDetails.UserId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var regResponse = await _iglwbprasutisahaybetiprotsahanyojnaservice.AddSchemeDetails(schemeDetails);
            return RedirectToAction("AppPersonalDetails",
                new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(schemeDetails.ServiceId.ToString())), strTabId = "2", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())) });
        }

        public async Task<IActionResult> UploadDocument(IList<DocumentFileDetails> documentFileDetails, string strTabSequenceNo, string strApplicationId, string strServiceId)
        {
            
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
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.glwb_psy);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
                        }
                        else
                        {
                            return PartialView("GLWBPrasutiSahayBetiProtsahanYojna/_GLWBPrasutiSahayBetiProtsahanYojnaDocument");
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
                    responseMessage = await _iglwbprasutisahaybetiprotsahanyojnaservice.AddUpdateDocumentDetailsNew(dtData);
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
            
            return RedirectToAction("AppPersonalDetails", "GLWBPrasutiSahayBetiProtsahanYojna", documentFileDetails);
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
            finalSubmitModel.tablename = nameof(EnumLookup.tablename.personaldetails);
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.glwb_psy);
            //  finalSubmitModel.userid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";

            var regResponse = await _iglwbprasutisahaybetiprotsahanyojnaservice.FinalSubmit(finalSubmitModel);
            if (regResponse != null)
                if (regResponse != null && regResponse.Error == 0)
                {

                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));
                    var schemaname = nameof(EnumLookup.schemaname.glwb_psy);
                    var tablename = nameof(EnumLookup.tablename.personaldetails);
                    SMSModel modelSMSModel = await _iglwbprasutisahaybetiprotsahanyojnaservice.GetSmsContentForService(serviceId, ApplicationId, 1, schemaname, tablename);
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iglwbprasutisahaybetiprotsahanyojnaservice.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                        }
                    }
                    var msg = regResponse.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
                }

            var errorMsg = "Form Subbmission Failed..!!";
            //  TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");

            return RedirectToAction("GLWBHSCPurashkarYojanaTermsCondition", "GLWBHSCPurashkarYojana");
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
