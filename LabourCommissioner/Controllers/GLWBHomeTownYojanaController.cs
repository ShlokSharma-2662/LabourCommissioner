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
using System.Data;

namespace LabourCommissioner.Controllers
{
    public class GLWBHomeTownYojanaController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGLWBHomeTownYojanaService _iGLWBHomeTownYojanaservice;

        public GLWBHomeTownYojanaController(IGLWBHomeTownYojanaService iGLWBHomeTownYojanaService, IConfiguration config, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iGLWBHomeTownYojanaservice = iGLWBHomeTownYojanaService ??
                                              throw new ArgumentNullException(nameof(_iGLWBHomeTownYojanaservice));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext!.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult AppPersonalDetails(string strid, string strTabId, string strApplicationId, string strOrgServiceId = "")
        {
            var RegistrationID = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            int id = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strid)));
            int OrgServiceId = 0;
            if (strOrgServiceId != "")
            {
                OrgServiceId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strOrgServiceId)));
            }
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

            if (OrgServiceId == 32)
            {
                var model = _iGLWBHomeTownYojanaservice.GetTabSequenceByApplicationId(ApplicationId, OrgServiceId, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.tabentry));
                ViewBag.AppPersonalDetails = model.Result.ToList();
            }
            else
            {
                var model = _iGLWBHomeTownYojanaservice.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.tabentry));
                ViewBag.AppPersonalDetails = model.Result.ToList();
            }
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

            var model = await _iGLWBHomeTownYojanaservice.GetPersonalDetailsByRegNo(RegistrationNo);
            var model1 = await _iGLWBHomeTownYojanaservice.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.personaldetails));
            //model1.ICardFromDate = model.ICardFromDate;
            //model1.ICardToDate = model.ICardToDate;

            ViewBag.Message = CommonUtils.ConcatString("જો શારીરિક રીતે શક્ષમ હોય તો ઉંમર ૩૦ થી વધારે ના હોવી જોઈએ.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            model.ApplicationNo = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.ServiceId = ServiceId;
            ViewBag.TabSequenceNo = 1;

            var stateModel = _iGLWBHomeTownYojanaservice.GetAllStates();
            var states = stateModel.Result.ToList();

            ViewBag.States = states;
            ViewBag.isFilled = isFilled;

            var districtModel = _iGLWBHomeTownYojanaservice.GetDistrict();
            var district = districtModel.Result.ToList();

            ViewBag.District = district;
            ViewBag.ApplicationId = ApplicationId;
            if (ServiceId == "32" && isFilled == false)
            {
                model1.ICardFromDate = model.ICardFromDate;
                model1.ICardToDate = model.ICardToDate;
                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(model1.CouchDBDocId, model1.FileName);
                var base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ViewBag.Image = base64Image;
                var talukaModel = _iGLWBHomeTownYojanaservice.GetTalukaByDistrictId(model1.CDistrictId);
                var Ctaluka = talukaModel.Result.ToList();
                var villageModel = _iGLWBHomeTownYojanaservice.GetVillageByDistrictIdAndTalukaId(model1.CDistrictId, model1.CTalukaId);
                var Cvillage = villageModel.Result.ToList();
                ViewBag.CTaluka = Ctaluka;
                ViewBag.CVillage = Cvillage;
                model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaPersonalDetails", model1);
            }
            else
            {
                if (isFilled)
                {
                    model1.ICardFromDate = model.ICardFromDate;
                    model1.ICardToDate = model.ICardToDate;
                    CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(model1.CouchDBDocId, model1.FileName);
                    var base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                    ViewBag.Image = base64Image;
                    var talukaModel = _iGLWBHomeTownYojanaservice.GetTalukaByDistrictId(model1.CDistrictId);
                    var Ctaluka = talukaModel.Result.ToList();
                    var villageModel = _iGLWBHomeTownYojanaservice.GetVillageByDistrictIdAndTalukaId(model1.CDistrictId, model1.CTalukaId);
                    var Cvillage = villageModel.Result.ToList();
                    ViewBag.CTaluka = Ctaluka;
                    ViewBag.CVillage = Cvillage;
                    model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                    model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                    return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaPersonalDetails", model1);
                }
            }

            return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaPersonalDetails", model);


            //return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaPersonalDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBHomeTownYojanaSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            // return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaSchemeDetails");
            long ApplicationId = Convert.ToInt64(strApplicationId);
            int serviceId = Convert.ToInt32(ServiceId);
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            var model = await _iGLWBHomeTownYojanaservice.GetPersonalDetailsByRegNo(RegistrationNo);
            var ENirmanCardNo = model.RegistrationNo;
            ViewBag.EnirmanCardNo = ENirmanCardNo;

            var model1 = await _iGLWBHomeTownYojanaservice.GetApplicationSchemeDetailsByAppId(ApplicationId);

            //if(model1.lstFamilyMemberDetails == null)
            //{
            //    FamilyMemberDetails objFamilyMemberDetails = new FamilyMemberDetails();
            //    objFamilyMemberDetails.name = "";
            //    model1.lstFamilyMemberDetails.Add(objFamilyMemberDetails);
            //}

            ViewBag.isFilled = isFilled;

            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;
            ViewBag.ApplicationId = ApplicationId;
            //IEnumerable<GLWBhtySchemeDetails> model = await _iGLWBHomeTownYojanaservice.GetApplicationDetails( serviceId, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.personaldetails));

            return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaSchemeDetails", model1);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBHomeTownYojanaClaimSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            // return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaSchemeDetails");
            long ApplicationId = Convert.ToInt64(strApplicationId);
            int serviceId = Convert.ToInt32(ServiceId);
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            var model = await _iGLWBHomeTownYojanaservice.GetPersonalDetailsByRegNo(RegistrationNo);
            var ENirmanCardNo = model.RegistrationNo;
            ViewBag.EnirmanCardNo = ENirmanCardNo;

            var model1 = await _iGLWBHomeTownYojanaservice.GetApplicationSchemeDetailsByAppId(ApplicationId);

            //if(model1.lstFamilyMemberDetails == null)
            //{
            //    FamilyMemberDetails objFamilyMemberDetails = new FamilyMemberDetails();
            //    objFamilyMemberDetails.name = "";
            //    model1.lstFamilyMemberDetails.Add(objFamilyMemberDetails);
            //}

            ViewBag.isFilled = isFilled;

            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;
            ViewBag.ApplicationId = ApplicationId;
            //IEnumerable<GLWBhtySchemeDetails> model = await _iGLWBHomeTownYojanaservice.GetApplicationDetails( serviceId, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.personaldetails));

            return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaSchemeDetails", model1);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBHomeTownYojanaDocument(string ServiceId, string strApplicationId, bool isFilled)
        {

            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 3;
            var model = await _iGLWBHomeTownYojanaservice.GetDocumentFileDetails(Convert.ToInt32(ServiceId), Convert.ToInt32(ApplicationId));


            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iGLWBHomeTownYojanaservice.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.documentdetails));
                return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaDocument", model);
            }
        }

        [HttpGet]
        public IActionResult TermsCondition(string ServiceId, string strApplicationId, bool isFilled)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaTermsCondition");
        }

        //[HttpPost]

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _iGLWBHomeTownYojanaservice.GetDistrict();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }


        [HttpGet]
        public IActionResult GetTalukaByDistrictId(int districtId)
        {
            var regions = _iGLWBHomeTownYojanaservice.GetTalukaByDistrictId(districtId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }


        [HttpGet]
        public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var regions = _iGLWBHomeTownYojanaservice.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public ActionResult GetEducation()
        {
            var regions = _iGLWBHomeTownYojanaservice.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddUpdateApplication(GLWBhty_PersonalDetailsModel personalDetails, string strTabId, string strApplicationNo)
        {
            int TabSequenceNo = Convert.ToInt32(strTabId);
            string ApplicationNo = strApplicationNo;
            personalDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            personalDetails.HostName = CommonUtils.GetHostName();
            personalDetails.RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            personalDetails.RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            personalDetails.schemaname = nameof(EnumLookup.schemaname.glwb_hty);
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
                    return RedirectToAction("AppPersonalDetails", "GLWBHomeTownYojana", personalDetails);
                }
            }

            #endregion


            var regResponse = await _iGLWBHomeTownYojanaservice.AddUpdateApplication(personalDetails);
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
                    return RedirectToAction("AppPersonalDetails", "GLWBHomeTownYojana");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "GLWBHomeTownYojana", personalDetails);

            //return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", personalDetails);

            return RedirectToAction("GLWBHomeTownYojanaSchemeDetails", "GLWBHomeTownYojana");
        }

        public async Task<IActionResult> AddSchemeDetails(GLWBhtySchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            schemeDetails.ApplicationId = ApplicationId;
            schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            schemeDetails.HostName = CommonUtils.GetHostName();
            schemeDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            schemeDetails.TabSequenceNo = TabSequenceNo;
            schemeDetails.schemaname = nameof(EnumLookup.schemaname.glwb_hty);
            schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
            // schemeDetails.UserId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);


            if (schemeDetails.lstFamilyMemberDetails != null && schemeDetails.lstFamilyMemberDetails.Count() > 0)
            {
                var lstFamilyMemberDetails = new List<FamilyMemberDetails>();
                var RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));

                foreach (var item in schemeDetails.lstFamilyMemberDetails)
                {

                    var objFamilyMemberDetails = new FamilyMemberDetails();
                    var couchDBRequest = new CouchDBRequest();
                    var couchDBResponse = new CouchDBResponse();
                    if (item.File != null && item.File.FileName != "" && item.File.Length > 0)
                    {
                        var extension = Path.GetExtension(item.File.FileName);
                        string fileWithOutExt = item.File.FileName.Substring(0, item.File.FileName.Length - extension.Length);
                        string FileName = RegistrationId + "_" + fileWithOutExt + "_" + DateTime.Now.ToString("yyyyMMDDhhmmss") +
                                                   Convert.ToString(extension);
                        //Insert new attachment

                        byte[] fileBytes = null;
                        using (var msstream = new MemoryStream())
                        {
                            await item.File.CopyToAsync(msstream);
                            fileBytes = msstream.ToArray();
                        }

                        couchDBRequest.FileName = FileName;//item.IdImage.File.FileName;
                        couchDBRequest.AttachmentData = fileBytes;
                        couchDBRequest.FileExtension =
                            Path.GetExtension(item.File.FileName).Replace(".", "").ToUpper();

                        couchDBRequest.CreatedOn = DateTime.Now.ToString();
                        couchDBResponse = await new CommonUtils(_config, _clientFactory).UplodToCouchDB(couchDBRequest);
                        if (couchDBResponse != null && couchDBResponse.IsSuccess)
                        {

                            objFamilyMemberDetails.familydetailsid = item.familydetailsid;
                            objFamilyMemberDetails.name = item.name;
                            objFamilyMemberDetails.age = item.age;
                            objFamilyMemberDetails.relation = item.relation;
                            objFamilyMemberDetails.joborcor = item.joborcor;
                            objFamilyMemberDetails.islabour = item.islabour;
                            objFamilyMemberDetails.aadharcard = item.aadharcard;
                            objFamilyMemberDetails.DocumentName = FileName;
                            objFamilyMemberDetails.CouchDBDocId = couchDBResponse.Id;
                            objFamilyMemberDetails.CouchDBDocRevId = couchDBResponse.Rev;
                            objFamilyMemberDetails.CreatedBy = RegistrationId;
                            lstFamilyMemberDetails.Add(objFamilyMemberDetails);

                        }
                        else
                        {
                            return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaSchemeDetails");
                        }
                    }
                    else if (item.CouchDBDocId != "" && item.CouchDBDocRevId != "" && item.isDeleted == false)
                    {
                        objFamilyMemberDetails.familydetailsid = item.familydetailsid;
                        objFamilyMemberDetails.name = item.name;
                        objFamilyMemberDetails.age = item.age;
                        objFamilyMemberDetails.relation = item.relation;
                        objFamilyMemberDetails.joborcor = item.joborcor;
                        objFamilyMemberDetails.islabour = item.islabour;
                        objFamilyMemberDetails.aadharcard = item.aadharcard;
                        objFamilyMemberDetails.isDeleted = item.isDeleted;
                        objFamilyMemberDetails.DocumentName = item.DocumentName;
                        objFamilyMemberDetails.CouchDBDocId = item.CouchDBDocId;
                        objFamilyMemberDetails.CouchDBDocRevId = item.CouchDBDocRevId;
                        objFamilyMemberDetails.CreatedBy = RegistrationId;
                        lstFamilyMemberDetails.Add(objFamilyMemberDetails);
                    }


                }
                var dtData = CommonUtils.ToDataTable(lstFamilyMemberDetails);


                var regResponse = await _iGLWBHomeTownYojanaservice.AddSchemeDetails(schemeDetails, dtData);
                return RedirectToAction("AppPersonalDetails", new
                {
                    strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(schemeDetails.ServiceId.ToString())),
                    strTabId = "2",
                    strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString()))
                });


            }
            return RedirectToAction("AppPersonalDetails", "GLWBHomeTownYojana", schemeDetails);

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
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.glwb_hty);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
                        }
                        else
                        {
                            return PartialView("GLWBHomeTownYojana/_GLWBHomeTownYojanaDocument");
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
                    responseMessage = await _iGLWBHomeTownYojanaservice.AddUpdateDocumentDetailsNew(dtData);
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
            return RedirectToAction("AppPersonalDetails", "GLWBHomeTownYojana", documentFileDetails);
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
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.glwb_hty);
            //  finalSubmitModel.userid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";

            var regResponse = await _iGLWBHomeTownYojanaservice.FinalSubmit(finalSubmitModel);
            if (regResponse != null)
                if (regResponse != null && regResponse.Error == 0)
                {

                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                    SMSModel modelSMSModel = await _iGLWBHomeTownYojanaservice.GetSmsContentForService(serviceId, ApplicationId, (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.personaldetails));
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iGLWBHomeTownYojanaservice.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                        }

                    }


                    var msg = regResponse.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
                }

            var errorMsg = "Form Subbmission Failed..!!";
            //  TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");

            return RedirectToAction("TermsCondition", "GLWBHomeTownYojana");
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

