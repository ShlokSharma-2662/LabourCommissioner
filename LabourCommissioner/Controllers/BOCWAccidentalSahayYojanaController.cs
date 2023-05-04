using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using LabourCommissioner.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using FileResult = Microsoft.AspNetCore.Mvc.FileResult;

namespace LabourCommissioner.Controllers
{
    [Authorize]
    public class BOCWAccidentalSahayYojanaController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBOCWAccidentalSahayYojanaService _iBOCWAccidentalSahayYojanaService;

        public BOCWAccidentalSahayYojanaController(IBOCWAccidentalSahayYojanaService iBOCWAccidentalSahayYojanaService,
            IConfiguration config, IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iBOCWAccidentalSahayYojanaService = iBOCWAccidentalSahayYojanaService ??
                                              throw new ArgumentNullException(nameof(_iBOCWAccidentalSahayYojanaService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
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
            var model = _iBOCWAccidentalSahayYojanaService.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.bocw_acsy), nameof(EnumLookup.tablename.tabentry));
            ViewBag.AppPersonalDetails = model.Result.ToList();
            ViewBag.ApplicationId = ApplicationId;

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> PersonalDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;


            long ApplicationId = Convert.ToInt64(strApplicationId);

            var model = await _iBOCWAccidentalSahayYojanaService.GetPersonalDetailsByRegNo(RegistrationNo);
            var model1 = await _iBOCWAccidentalSahayYojanaService.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.bocw_acsy), nameof(EnumLookup.tablename.personaldetails));

            ViewBag.Message = CommonUtils.ConcatString("જો શારીરિક રીતે શક્ષમ હોય તો ઉંમર ૩૦ થી વધારે ના હોવી જોઈએ.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            model.ApplicationNo = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.ServiceId = ServiceId;
            ViewBag.TabSequenceNo = 1;

            var stateModel = _iBOCWAccidentalSahayYojanaService.GetAllStates();
            var states = stateModel.Result.ToList();

            ViewBag.States = states;
            ViewBag.isFilled = isFilled;

            var districtModel = _iBOCWAccidentalSahayYojanaService.GetDistrict();
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

                var PtalukaModel = _iBOCWAccidentalSahayYojanaService.GetTalukaByDistrictId(model1.PDistrictId);
                var Ptaluka = PtalukaModel.Result.ToList();
                var PvillageModel =
                    _iBOCWAccidentalSahayYojanaService.GetVillageByDistrictIdAndTalukaId(model1.PDistrictId, model1.PTalukaId);
                var Pvillage = PvillageModel.Result.ToList();

                var talukaModel = _iBOCWAccidentalSahayYojanaService.GetTalukaByDistrictId(model1.CDistrictId);
                var Ctaluka = talukaModel.Result.ToList();
                var villageModel =
                    _iBOCWAccidentalSahayYojanaService.GetVillageByDistrictIdAndTalukaId(model1.CDistrictId, model1.CTalukaId);
                var Cvillage = villageModel.Result.ToList();

                ViewBag.CTaluka = Ctaluka;
                ViewBag.CVillage = Cvillage;
                ViewBag.PTaluka = Ptaluka;
                ViewBag.PVillage = Pvillage;
                model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                return PartialView("BOCWAccidentalSahayYojana/_BOCWAccidentalSahayYojanaPersonalDetails", model1);
            }

            return PartialView("BOCWAccidentalSahayYojana/_BOCWAccidentalSahayYojanaPersonalDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWAccidentalSahayYojanaSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {

            long ApplicationId = Convert.ToInt64(strApplicationId);
            int serviceId = Convert.ToInt32(ServiceId);
            var model1 = await _iBOCWAccidentalSahayYojanaService.GetApplicationSchemeDetailsByAppId(ApplicationId);
            ViewBag.isFilled = isFilled;

            var totalsahay = _iBOCWAccidentalSahayYojanaService.GetTotalsahayByServiceID(serviceId);
            ViewBag.TotalSahay = totalsahay.Result.totalsahay;


            ViewBag.Message = CommonUtils.ConcatString("મૃતક ની ઉંમર ૧૮ થી ૬૦ જ હોવી જોઈએ.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            var appDetailsModel = await _iBOCWAccidentalSahayYojanaService.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.bocw_acsy), nameof(EnumLookup.tablename.personaldetails));
            if (appDetailsModel != null)
            {
                string Year = Convert.ToString(appDetailsModel.DateOfBirth.Value.Year);
                string Month = Convert.ToString(appDetailsModel.DateOfBirth.Value.Month.ToString().Length == 1 ? string.Concat(0, appDetailsModel.DateOfBirth.Value.Month.ToString()) : appDetailsModel.DateOfBirth.Value.Month.ToString());
                string Day = Convert.ToString(appDetailsModel.DateOfBirth.Value.Day);
                ViewBag.DOB = Convert.ToString(Year + "-" + Month + "-" + Day);
            }
            var stateModel = _iBOCWAccidentalSahayYojanaService.GetAllStates();
            var states = stateModel.Result.ToList();
            ViewBag.States = states;
            var districtModel = _iBOCWAccidentalSahayYojanaService.GetDistrict();
            var district = districtModel.Result.ToList();
            ViewBag.District = district;
            ViewBag.ApplicationId = ApplicationId;
            if (isFilled)
            {
                var PtalukaModel = _iBOCWAccidentalSahayYojanaService.GetTalukaByDistrictId(model1.district);
                var Ptaluka = PtalukaModel.Result.ToList();
                var PvillageModel =
                    _iBOCWAccidentalSahayYojanaService.GetVillageByDistrictIdAndTalukaId(model1.district, model1.taluka);
                var Pvillage = PvillageModel.Result.ToList();
                ViewBag.PTaluka = Ptaluka;
                ViewBag.PVillage = Pvillage;
            }
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;

            return PartialView("BOCWAccidentalSahayYojana/_BOCWAccidentalSahayYojanaSchemeDetails", model1);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWAccidentalSahayYojanaDocument(string ServiceId, string strApplicationId, bool isFilled)
        {
            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 3;
            var model = await _iBOCWAccidentalSahayYojanaService.GetDocumentFileDetails(Convert.ToInt32(ServiceId), Convert.ToInt32(ApplicationId));
            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iBOCWAccidentalSahayYojanaService.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.bocw_acsy), nameof(EnumLookup.tablename.documentdetails));
                return PartialView("BOCWAccidentalSahayYojana/_BOCWAccidentalSahayYojanaDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("BOCWAccidentalSahayYojana/_BOCWAccidentalSahayYojanaDocument", model);
            }
        }

        [HttpGet]
        public IActionResult TermsCondition(string ServiceId, string strApplicationId, bool isFilled)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("BOCWAccidentalSahayYojana/_BOCWAccidentalSahayYojanaTermsCondition");
        }

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _iBOCWAccidentalSahayYojanaService.GetDistrict();
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetSubject(string subjectId)
        {
            var subject = _iBOCWAccidentalSahayYojanaService.GetSubject(subjectId);
            return Json(new { data = subject });
        }

        [HttpGet]
        public IActionResult GetTalukaByDistrictId(int districtId)
        {
            var regions = _iBOCWAccidentalSahayYojanaService.GetTalukaByDistrictId(districtId);
            return Json(new { data = regions });
        }
        public IActionResult GetSemesterbyCourseId(int courseid)
        {
            var regions = _iBOCWAccidentalSahayYojanaService.GetSemesterbyCourseId(courseid);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var regions = _iBOCWAccidentalSahayYojanaService.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            return Json(new { data = regions });
        }

        [HttpGet]
        public ActionResult GetEducation()
        {
            var regions = _iBOCWAccidentalSahayYojanaService.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        public IActionResult GettotalsahaybysahayId(int serviceid, int deathdisability)
        {
            var course = _iBOCWAccidentalSahayYojanaService.GettotalsahaybysahayId(serviceid, deathdisability);
            return Json(new { data = course });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateApplication(PersonalDetailsModel personalDetails, string strTabId, string strApplicationNo)
        {
            int TabSequenceNo = Convert.ToInt32(strTabId);
            string ApplicationNo = strApplicationNo;
            personalDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            personalDetails.HostName = CommonUtils.GetHostName();
            personalDetails.RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            personalDetails.RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            personalDetails.schemaname = nameof(EnumLookup.schemaname.bocw_acsy);
            personalDetails.tablename = nameof(EnumLookup.tablename.personaldetails);
            personalDetails.applicationfrom = Convert.ToInt32(EnumLookup.applicationfrom.web);
            if(personalDetails.BeneficiaryType == 6)
            {
                personalDetails.HasENirmanCard = Convert.ToBoolean(EnumLookup.HasENirmancard.No);
            }
            else
            {
                personalDetails.HasENirmanCard = Convert.ToBoolean(EnumLookup.HasENirmancard.Yes);
            }

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
                    return RedirectToAction("AppPersonalDetails", "BOCWAccidentalSahayYojana", personalDetails);
                }
            }

            #endregion


            var regResponse = await _iBOCWAccidentalSahayYojanaService.AddUpdateApplication(personalDetails);
            if (regResponse != null)
            {
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    ModelState.Clear();
                    var empEmpty = new PersonalDetailsModel();

                    return RedirectToAction("AppPersonalDetails",
                        new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(personalDetails.ServiceId.ToString())), strTabId = "1", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())) });
                }
                else
                {
                    ModelState.Clear();
                    var empEmpty = new Registration();
                    return RedirectToAction("AppPersonalDetails", "BOCWAccidentalSahayYojana");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "BOCWAccidentalSahayYojana", personalDetails);



            return RedirectToAction("BOCWSikshanSahayYojanaSchemeDetails", "BOCWAccidentalSahayYojana");
        }

        public async Task<IActionResult> AddSchemeDetails(BOCWACSYSchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            schemeDetails.ApplicationId = ApplicationId;
            schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            schemeDetails.HostName = CommonUtils.GetHostName();
            schemeDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            schemeDetails.TabSequenceNo = TabSequenceNo;
            schemeDetails.schemaname = nameof(EnumLookup.schemaname.bocw_acsy);
            schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
            var regResponse = await _iBOCWAccidentalSahayYojanaService.AddSchemeDetails(schemeDetails);
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
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.bocw_acsy);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
                        }
                        else
                        {
                            return PartialView("BOCWAccidentalSahayYojana/_BOCWAccidentalSahayYojanaDocument");
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
                    responseMessage = await _iBOCWAccidentalSahayYojanaService.AddUpdateDocumentDetailsNew(dtData);
                }
                if (dtData.Rows.Count == 0 && responseMessage != null && responseMessage.Error == 0)
                {
                    responseMessage.ApplicationNo = ApplicationId;
                    responseMessage.Id = ServiceId;
                }
                return RedirectToAction("AppPersonalDetails",
                    new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.Id.ToString())), strTabId = "3", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.ApplicationNo.ToString())) });
            }

            #region OLD

            //if (documentDetails.FormFile != null && documentDetails.FormFile.Count() > 0)
            //{
            //    DocumentDetails objdocumentDetails = new DocumentDetails();
            //    List<DocumentDetails> lstdocumentDetails = new List<DocumentDetails>();
            //    foreach (IFormFile item in documentDetails.FormFile)
            //    {

            //        CouchDBRequest couchDBRequest = new CouchDBRequest();
            //        CouchDBResponse couchDBResponse = new CouchDBResponse();

            //        if (item.FileName != null && item.FileName != "" && item.Length > 0)
            //        {
            //            // Insert new attachment  

            //            byte[] fileBytes = null;
            //            using (MemoryStream msstream = new MemoryStream())
            //            {
            //                await item.CopyToAsync(msstream);
            //                fileBytes = msstream.ToArray();

            //            }
            //            couchDBRequest.FileName = item.FileName;
            //            couchDBRequest.AttachmentData = fileBytes;
            //            couchDBRequest.FileExtension = System.IO.Path.GetExtension(item.FileName).Replace(".", "").ToUpper(); ;
            //            couchDBRequest.CreatedOn = DateTime.Now.ToString();
            //            //CommonUtils commonUtils = new CommonUtils(_config);
            //            couchDBResponse = await new CommonUtils(_config, _clientFactory).UplodToCouchDB(couchDBRequest);
            //            if (couchDBResponse != null && couchDBResponse.IsSuccess)
            //            {
            //                objdocumentDetails.DocumentMasterId = documentDetails.DocumentMasterId;
            //                objdocumentDetails.DocumentName = item.FileName;
            //                objdocumentDetails.DocumentNumber = documentDetails.DocumentNumber;
            //                objdocumentDetails.DocumentMasterId = documentDetails.DocumentMasterId;
            //                objdocumentDetails.DocumentMasterId = documentDetails.DocumentMasterId;
            //                //lstcouchDBResponse.Add(couchDBResponse);
            //            }
            //            else
            //            {
            //                return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument");
            //            }
            //        }
            //        else
            //        {
            //            //// Update existing attachment  
            //            //var docData = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray("1", Convert.ToString(item.FileName));
            //            //couchDBRequest.AttachmentData = docData.Result;
            //            //couchDBRequest.CreatedOn = DateTime.Now.ToString();
            //            //var result = await new CommonUtils(_config, _clientFactory).UpdateAttachment(couchDBRequest);
            //            //if (result.IsSuccess)
            //            //{
            //            //    return RedirectToAction("Index");
            //            //}
            //        }
            //    }
            //    //if (lstcouchDBResponse != null && lstcouchDBResponse.Count() > 0)
            //    //{
            //    //    List<DocumentDetails> model = await _iBOCWSikshanSahayYojanaService.SaveDocumentDetails(lstcouchDBResponse);
            //    //    return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
            //    //}


            //    return View();
            //}

            #endregion

            return RedirectToAction("AppPersonalDetails", "BOCWAccidentalSahayYojana", documentFileDetails);
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
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.bocw_acsy);
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";
            var regResponse = await _iBOCWAccidentalSahayYojanaService.FinalSubmit(finalSubmitModel);
            if (regResponse != null)
                if (regResponse != null && regResponse.Error == 0)
                {

                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                    SMSModel modelSMSModel = await _iBOCWAccidentalSahayYojanaService.GetSmsContentForService(serviceId, ApplicationId, (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.bocw_acsy), nameof(EnumLookup.tablename.personaldetails));
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iBOCWAccidentalSahayYojanaService.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                        }

                    }
                    var msg = regResponse.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
                }

            var errorMsg = "Form Subbmission Failed..!!";

            return RedirectToAction("TermsCondition", "BOCWAccidentalSahayYojana");
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
