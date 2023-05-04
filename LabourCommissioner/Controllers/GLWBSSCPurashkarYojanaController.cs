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

namespace LabourCommissioner.Controllers
{
    [Authorize]
    public class GLWBSSCPurashkarYojanaController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGLWBSSCPurashkarYojanaService _iglwbsscpurashkaryojanaservice;

        public GLWBSSCPurashkarYojanaController(IGLWBSSCPurashkarYojanaService iglwbsscpurashkaryojanaservice, IConfiguration config, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iglwbsscpurashkaryojanaservice = iglwbsscpurashkaryojanaservice ??
                                              throw new ArgumentNullException(nameof(_iglwbsscpurashkaryojanaservice));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext!.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AppPersonalDetails(string strid, string strTabId, string strApplicationId)
        {
            //return View("AppPersonalDetails_OLD");
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
            //var model = _iglwbsscpurashkaryojanaservice.GetServiceTabByServiceId(id);
            var model = _iglwbsscpurashkaryojanaservice.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.glwb_ssc), nameof(EnumLookup.tablename.tabentry));
            ViewBag.AppPersonalDetails = model.Result.ToList();
            ViewBag.ApplicationId = ApplicationId;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GLWBSSCPurashkarYojanaPersonalDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;



            // long ApplicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            long ApplicationId = Convert.ToInt64(strApplicationId);

            var model = await _iglwbsscpurashkaryojanaservice.GetPersonalDetailsByRegNo(RegistrationNo);
            var model1 = await _iglwbsscpurashkaryojanaservice.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.glwb_ssc), nameof(EnumLookup.tablename.personaldetails));

            List<SelectListItem> relationList = new List<SelectListItem>();
            relationList.Add(new SelectListItem
            {
                Text = "પુત્ર",
                Value = "1",
                Selected = true
            });
            relationList.Add(new SelectListItem
            {
                Text = "પુત્રી",
                Value = "2",
                Selected = true
            });
            relationList.Add(new SelectListItem
            {
                Text = "પત્ની",
                Value = "3",
                Selected = true
            });

            ViewBag.Relation = relationList;

            model.ApplicationNo = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.ServiceId = ServiceId;
            ViewBag.TabSequenceNo = 1;

            var stateModel = _iglwbsscpurashkaryojanaservice.GetAllStates();
            var states = stateModel.Result.ToList();

            ViewBag.States = states;
            ViewBag.isFilled = isFilled;

            var districtModel = _iglwbsscpurashkaryojanaservice.GetDistrict();
            var district = districtModel.Result.ToList();

            ViewBag.District = district;
            ViewBag.ApplicationId = ApplicationId;
            if (isFilled)
            {
                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(model1.CouchDBDocId, model1.FileName);
                var base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ViewBag.Image = base64Image;


                var talukaModel = _iglwbsscpurashkaryojanaservice.GetTalukaByDistrictId(model1.CDistrictId);
                var Ctaluka = talukaModel.Result.ToList();

                var villageModel =
                    _iglwbsscpurashkaryojanaservice.GetVillageByDistrictIdAndTalukaId(model1.CDistrictId, model1.CTalukaId);
                var Cvillage = villageModel.Result.ToList();

                ViewBag.CTaluka = Ctaluka;
                ViewBag.CVillage = Cvillage;
                model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaPersonalDetails", model1);
            }
            model.AadharCardNo = "";

            return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaPersonalDetails", model);


            //return PartialView("GLWBSSCPurashkarYojana/_PersonalDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBSSCPurashkarYojanaSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            var model = await _iglwbsscpurashkaryojanaservice.GetPersonalDetailsByRegNo(RegistrationNo);
            var ENirmanCardNo = model.RegistrationNo;
            long ApplicationId = Convert.ToInt64(strApplicationId);
            ViewBag.EnirmanCardNo = ENirmanCardNo;
            var model1 = await _iglwbsscpurashkaryojanaservice.GetApplicationSchemeDetailsByAppId(ApplicationId);
            //if (model1 == null)
            //{
            //    model1.ENirmanCardNo = ENirmanCardNo;
            //}
            //long startYear = 2021;
            //long currYear = DateTime.Now.Year;
            long startYear = 2022;
            long currYear = startYear;


            int serviceId = Convert.ToInt32(ServiceId);
            var totalsahay = _iglwbsscpurashkaryojanaservice.GetTotalsahayByServiceID(serviceId);
            ViewBag.TotalSahay = totalsahay.Result.totalsahay;

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
            ViewBag.Standard = std;
            ViewBag.isFilled = isFilled;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;

            return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaSchemeDetails", model1);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBSSCPurashkarYojanaDocument(string ServiceId, string strApplicationId, bool isFilled)
        {
            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 3;
            var model = await _iglwbsscpurashkaryojanaservice.GetDocumentFileDetails(Convert.ToInt32(ServiceId));

            var schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
            var tablename = nameof(EnumLookup.tablename.documentdetails);
            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iglwbsscpurashkaryojanaservice.GetUploadedDocuments(Convert.ToInt64(ApplicationId), schemaname, tablename, Convert.ToInt64(ServiceId));
                return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaDocument", model);
            }



            //IEnumerable<DocumentDetails> model = await _iglwbsscpurashkaryojanaservice.GetFileDocuments(Convert.ToInt32(ServiceId));
            //return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaDocument", model);

            //DocumentDetails model = await _iglwbsscpurashkaryojanaservice.GetDocumentsDetails(Convert.ToInt32(ServiceId));
            // return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaDocument", model);
        }

        [HttpGet]
        public IActionResult GLWBSSCPurashkarYojanaTermsCondition(string ServiceId, string strApplicationId, bool isFilled)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaTermsCondition");
        }

        //[HttpPost]

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _iglwbsscpurashkaryojanaservice.GetDistrict();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetSubject(int subjectId)
        {
            var subject = _iglwbsscpurashkaryojanaservice.GetSubject(subjectId);
            return Json(new { data = subject });
        }

        //[HttpGet]
        //public IActionResult GetBenifitByCourseId(int courseId)
        //{
        //    var course = _iglwbsscpurashkaryojanaservice.GetBenifitByCourseId(courseId);
        //    //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //    return Json(new { data = course });
        //}

        [HttpGet]
        public IActionResult GetTalukaByDistrictId(int districtId)
        {
            var regions = _iglwbsscpurashkaryojanaservice.GetTalukaByDistrictId(districtId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var regions = _iglwbsscpurashkaryojanaservice.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public ActionResult GetEducation()
        {
            var regions = _iglwbsscpurashkaryojanaservice.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddUpdateApplication(GLWBSSC_PersonalDetailsModel personalDetails, string strTabId, string strApplicationNo)
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
            personalDetails.schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
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
                    return RedirectToAction("AppPersonalDetails", "GLWBSSCPurashkarYojana", personalDetails);
                }
            }

            #endregion


            var regResponse = await _iglwbsscpurashkaryojanaservice.AddUpdateApplication(personalDetails);
            if (regResponse != null)
            {
                //string errorMsg = regResponse.ResponseMsg == null ? "Somthing went wrong please try again." : regResponse.ResponseMsg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    //TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                    ModelState.Clear();
                    var empEmpty = new GLWBSSC_PersonalDetailsModel();
                    //return RedirectToAction("GLWBSSCPurashkarYojanaSchemeDetails", "GLWBSSCPurashkarYojana");
                    return RedirectToAction("AppPersonalDetails",
                        new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(personalDetails.ServiceId.ToString())), strTabId = "1", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())) });
                }
                else
                {
                    //TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    var empEmpty = new Registration();
                    return RedirectToAction("AppPersonalDetails", "GLWBSSCPurashkarYojana");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "GLWBSSCPurashkarYojana", personalDetails);

            //return RedirectToAction("AppPersonalDetails", "GLWBSSCPurashkarYojana", personalDetails);

            return RedirectToAction("GLWBSSCPurashkarYojanaSchemeDetails", "GLWBSSCPurashkarYojana");
        }

        public async Task<IActionResult> AddSchemeDetails(GLWBSSCSchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            schemeDetails.ApplicationId = ApplicationId;
            schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            schemeDetails.HostName = CommonUtils.GetHostName();
            schemeDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            schemeDetails.TabSequenceNo = TabSequenceNo;
            schemeDetails.schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
            schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
            // schemeDetails.UserId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var regResponse = await _iglwbsscpurashkaryojanaservice.AddSchemeDetails(schemeDetails);
            return RedirectToAction("AppPersonalDetails",
                new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(schemeDetails.ServiceId.ToString())), strTabId = "2", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())) });
        }

        public async Task<IActionResult> UploadDocument(IList<DocumentDetails.DocumentFileDetails> documentFileDetails, string strTabSequenceNo, string strApplicationId, string strServiceId)
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
                var lstdocumentFileDetails = new List<DocumentDetails.DocumentFileDetails>();
                var RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
                foreach (var item in documentFileDetails)
                {
                    var objdocumentFileDetails = new DocumentDetails.DocumentFileDetails();
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

                        couchDBRequest.CreatedOn = DateTime.Now.ToString();
                        couchDBResponse = await new CommonUtils(_config, _clientFactory).UplodToCouchDB(couchDBRequest);
                        if (couchDBResponse != null && couchDBResponse.IsSuccess)
                        {
                            objdocumentFileDetails.ApplicationId = ApplicationId;
                            objdocumentFileDetails.DocumentMasterId = item.DocumentMasterId;
                            objdocumentFileDetails.DocumentName = FileName;
                            objdocumentFileDetails.DocumentNumber = item.DocumentNumber;
                            objdocumentFileDetails.CouchDBDocId = couchDBResponse.Id;
                            objdocumentFileDetails.ServiceId = ServiceId;
                            objdocumentFileDetails.CreatedBy = RegistrationId;
                            objdocumentFileDetails.IpAddress = CommonUtils.GetLocalIPAddress();
                            objdocumentFileDetails.HostName = CommonUtils.GetHostName();
                            objdocumentFileDetails.TabSequenceNo = item.TabSequenceNo;
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
                        }
                        else
                        {
                            return PartialView("GLWBSSCPurashkarYojana/_GLWBSSCPurashkarYojanaDocument");
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

                var model = await _iglwbsscpurashkaryojanaservice.AddUpdateDocumentDetailsNew(dtData);
                if (dtData.Rows.Count == 0 && model != null && model.Error == 0)
                {
                    model.ApplicationNo = ApplicationId;
                    model.Id = ServiceId;
                }
                //ResponseMessage model = await _iglwbsscpurashkaryojanaservice.AddUpdateDocumentDetails(lstdocumentFileDetails);
                return RedirectToAction("AppPersonalDetails",
                    new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(model.Id.ToString())), strTabId = "3", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(model.ApplicationNo.ToString())) });
            }
            return RedirectToAction("AppPersonalDetails", "GLWBSSCPurashkarYojana", documentFileDetails);
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
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
            //  finalSubmitModel.userid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";

            var regResponse = await _iglwbsscpurashkaryojanaservice.FinalSubmit(finalSubmitModel);
            if (regResponse != null)
                if (regResponse != null && regResponse.Error == 0)
                {

                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));
                    var schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
                    var tablename = nameof(EnumLookup.tablename.personaldetails);
                    SMSModel modelSMSModel = await _iglwbsscpurashkaryojanaservice.GetSmsContentForService(serviceId, ApplicationId, 1, schemaname, tablename);
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iglwbsscpurashkaryojanaservice.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                        }
                    }
                    var msg = regResponse.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
                }

            var errorMsg = "Form Subbmission Failed..!!";
            //  TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");

            return RedirectToAction("GLWBSSCPurashkarYojanaTermsCondition", "GLWBSSCPurashkarYojana");
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
