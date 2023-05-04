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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using FileResult = Microsoft.AspNetCore.Mvc.FileResult;

namespace LabourCommissioner.Controllers
{
    [Authorize]
    public class BOCWBhagyaLaxmiBondYojnaController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBOCWBhagyaLaxmiBondYojnaService _iBOCWBhagyaLaxmiBondYojnaService;
        public BOCWBhagyaLaxmiBondYojnaController(IBOCWBhagyaLaxmiBondYojnaService iBOCWBhagyaLaxmiBondYojnaService,
        IConfiguration config, IHttpClientFactory clientFactory,
        IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iBOCWBhagyaLaxmiBondYojnaService = iBOCWBhagyaLaxmiBondYojnaService ??
                                              throw new ArgumentNullException(nameof(_iBOCWBhagyaLaxmiBondYojnaService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult AppPersonalDetails(string strid, string strTabId, string strApplicationId)
        {

            int id = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strid)));
            //int TabId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strTabId)));
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
            //var model = _iBOCWBhagyaLaxmiBondYojnaService.GetServiceTabByServiceId(id);
            //  ViewBag.AppPersonalDetails = model.Result.ToList();
            var model = _iBOCWBhagyaLaxmiBondYojnaService.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.bocw_bpsy), nameof(EnumLookup.tablename.tabentry));
            ViewBag.AppPersonalDetails = model.Result.ToList();

            //if (tab.Result == null)
            //{
            //    ViewBag.TabSequenceNo = TabId + 1;
            //}
            //else
            //{
            //    if (Convert.ToInt32(tab.Result.SequenceNo) != 4)
            //    {
            //        ViewBag.TabSequenceNo = Convert.ToInt32(tab.Result.SequenceNo) + 1;
            //        ViewBag.isFilled = tab.Result.isfilled;
            //    }
            //    else
            //    {
            //        ViewBag.TabSequenceNo = Convert.ToInt32(tab.Result.SequenceNo);
            //        ViewBag.isFilled = tab.Result.isfilled;
            //    }

            //}

            ViewBag.ApplicationId = ApplicationId;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> BOCWBhagyaLaxmiBondYojnaPersonalDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;


            // long ApplicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            long ApplicationId = Convert.ToInt64(strApplicationId);

            var model = await _iBOCWBhagyaLaxmiBondYojnaService.GetPersonalDetailsByRegNo(RegistrationNo);

            model.ApplicationNo = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.ServiceId = ServiceId;
            ViewBag.TabSequenceNo = 1;

            var stateModel = _iBOCWBhagyaLaxmiBondYojnaService.GetAllStates();
            var states = stateModel.Result.ToList();

            ViewBag.States = states;
            ViewBag.isFilled = isFilled;

            var districtModel = _iBOCWBhagyaLaxmiBondYojnaService.GetDistrict();
            var district = districtModel.Result.ToList();

            ViewBag.District = district;
            ViewBag.ApplicationId = ApplicationId;
            if (isFilled)
            {
                var model1 = await _iBOCWBhagyaLaxmiBondYojnaService.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.bocw_bpsy), nameof(EnumLookup.tablename.personaldetails));

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(model1.CouchDBDocId, model1.FileName);
                var base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ViewBag.Image = base64Image;

                var PtalukaModel = _iBOCWBhagyaLaxmiBondYojnaService.GetTalukaByDistrictId(model1.PDistrictId);
                var Ptaluka = PtalukaModel.Result.ToList();

                var PvillageModel =
                    _iBOCWBhagyaLaxmiBondYojnaService.GetVillageByDistrictIdAndTalukaId(model1.PDistrictId, model1.PTalukaId);
                var Pvillage = PvillageModel.Result.ToList();

                var talukaModel = _iBOCWBhagyaLaxmiBondYojnaService.GetTalukaByDistrictId(model1.CDistrictId);
                var Ctaluka = talukaModel.Result.ToList();

                var villageModel =
                    _iBOCWBhagyaLaxmiBondYojnaService.GetVillageByDistrictIdAndTalukaId(model1.CDistrictId, model1.CTalukaId);
                var Cvillage = villageModel.Result.ToList();

                ViewBag.CTaluka = Ctaluka;
                ViewBag.CVillage = Cvillage;
                ViewBag.PTaluka = Ptaluka;
                ViewBag.PVillage = Pvillage;
                model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                return PartialView("BOCWBhagyaLaxmiBondYojna/_BOCWBhagyaLaxmiBondYojnaPersonalDetails", model1);
            }
            if (model.AadharCardNo != null)
            {
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
            }

            return PartialView("BOCWBhagyaLaxmiBondYojna/_BOCWBhagyaLaxmiBondYojnaPersonalDetails", model);


            //return PartialView("BOCWSikshanSahayYojana/_PersonalDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWBhagyaLaxmiBondYojnaSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            long ApplicationId = Convert.ToInt64(strApplicationId);
            var model1 = await _iBOCWBhagyaLaxmiBondYojnaService.GetApplicationSchemeDetailsByAppId(ApplicationId);
            ViewBag.isFilled = isFilled;



            var appDetailsModel = await _iBOCWBhagyaLaxmiBondYojnaService.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.bocw_bpsy), nameof(EnumLookup.tablename.personaldetails));


            if (appDetailsModel != null)
            {
                string Year = Convert.ToString(appDetailsModel.DateOfBirth.Value.Year);
                string Month = Convert.ToString(appDetailsModel.DateOfBirth.Value.Month.ToString().Length == 1 ? string.Concat(0, appDetailsModel.DateOfBirth.Value.Month.ToString()) : appDetailsModel.DateOfBirth.Value.Month.ToString());
                string Day = Convert.ToString(appDetailsModel.DateOfBirth.Value.Day.ToString().Length == 1 ? string.Concat(0, appDetailsModel.DateOfBirth.Value.Day.ToString()) : appDetailsModel.DateOfBirth.Value.Day.ToString());
                //string Day = Convert.ToString(appDetailsModel.DateOfBirth.Value.Day);
                ViewBag.DOB = Convert.ToString(Year + "-" + Month + "-" + Day);
                ViewBag.Date = Convert.ToString(Day + "/" + Month + "/" + Year);
            }


            //var educationModel = _iBOCWBhagyaLaxmiBondYojnaService.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            //var education = educationModel.Result.ToList();
            //ViewBag.Syllabus = education;

            //var semesterModel = _iBOCWBhagyaLaxmiBondYojnaService.GetEducation(nameof(EnumLookup.ResourcesType.Semester));
            //var semester = semesterModel.Result.ToList();
            //ViewBag.Semester = semester;


            //if (model1 == null)
            //{
            //    var degreeModel = _iBOCWBhagyaLaxmiBondYojnaService.GetEducation(nameof(EnumLookup.ResourcesType.Degree));
            //    var degree = degreeModel.Result.ToList();
            //    ViewBag.Degree = degree;
            //}
            //else
            //{
            //if (isFilled == true)
            //{
            //    var degreeModel = _iBOCWBhagyaLaxmiBondYojnaService.GetSubject(model1.Syllabus);
            //    var degree = degreeModel.Result.ToList();
            //    ViewBag.Degree = degree;
            //}
            //}


            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;
            // ViewBag.ApplicationId = ApplicationId;

            return PartialView("BOCWBhagyaLaxmiBondYojna/_BOCWBhagyaLaxmiBondYojnaSchemeDetails", model1);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWBhagyaLaxmiBondYojnaDocument(string ServiceId, string strApplicationId, bool isFilled)
        {
            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 3;
            var model = await _iBOCWBhagyaLaxmiBondYojnaService.GetDocumentFileDetails(Convert.ToInt32(ServiceId), Convert.ToInt32(ApplicationId));


            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iBOCWBhagyaLaxmiBondYojnaService.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.bocw_bpsy), nameof(EnumLookup.tablename.documentdetails));
                return PartialView("BOCWBhagyaLaxmiBondYojna/_BOCWBhagyaLaxmiBondYojnaDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("BOCWBhagyaLaxmiBondYojna/_BOCWBhagyaLaxmiBondYojnaDocument", model);
            }



            //IEnumerable<DocumentDetails> model = await _iBOCWBhagyaLaxmiBondYojnaService.GetFileDocuments(Convert.ToInt32(ServiceId));
            //return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);

            //DocumentDetails model = await _iBOCWBhagyaLaxmiBondYojnaService.GetDocumentsDetails(Convert.ToInt32(ServiceId));
            // return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
        }

        [HttpGet]
        public IActionResult BOCWBhagyaLaxmiBondYojnaTermsCondition(string ServiceId, string strApplicationId, bool isFilled)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("BOCWBhagyaLaxmiBondYojna/_BOCWBhagyaLaxmiBondYojnaTermsCondition");
        }

        //[HttpPost]

        //[HttpGet]
        //public IActionResult GetDistrict()
        //{
        //    var regions = _iBOCWBhagyaLaxmiBondYojnaService.GetDistrict();
        //    //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //    return Json(new { data = regions });
        //}

        //[HttpGet]
        //public IActionResult GetSubject(int subjectId)
        //{
        //    var subject = _iBOCWBhagyaLaxmiBondYojnaService.GetSubject(subjectId);
        //    return Json(new { data = subject });
        //}

        //[HttpGet]
        //public IActionResult GetBenifitByCourseId(int courseId)
        //{
        //    var course = _iBOCWBhagyaLaxmiBondYojnaService.GetBenifitByCourseId(courseId);
        //    //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //    return Json(new { data = course });
        //}

        //[HttpGet]
        //public IActionResult GetTalukaByDistrictId(int districtId)
        //{
        //    var regions = _iBOCWBhagyaLaxmiBondYojnaService.GetTalukaByDistrictId(districtId);
        //    //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //    return Json(new { data = regions });
        //}

        //[HttpGet]
        //public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        //{
        //    var regions = _iBOCWBhagyaLaxmiBondYojnaService.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
        //    //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //    return Json(new { data = regions });
        //}

        //[HttpGet]
        //public ActionResult GetEducation()
        //{
        //    var regions = _iBOCWBhagyaLaxmiBondYojnaService.GetEducation(nameof(EnumLookup.ResourcesType.Education));
        //    return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public IActionResult GettotalsahaybysahayId(int serviceid, int relation, int gender, int isApplied, int tharavId)
        {
            var res = _iBOCWBhagyaLaxmiBondYojnaService.GettotalsahaybysahayId(serviceid, relation, gender, isApplied, tharavId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = res });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddUpdateApplication(PersonalDetailsModel personalDetails, string strTabId, string strApplicationNo)
        {
            int TabSequenceNo = Convert.ToInt32(strTabId);
            string ApplicationNo = strApplicationNo;

            personalDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            personalDetails.HostName = CommonUtils.GetHostName();
            personalDetails.RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            personalDetails.RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            personalDetails.schemaname = nameof(EnumLookup.schemaname.bocw_bpsy);
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
                    return RedirectToAction("AppPersonalDetails", "BOCWBhagyaLaxmiBondYojna", personalDetails);
                }
            }

            #endregion


            var regResponse = await _iBOCWBhagyaLaxmiBondYojnaService.AddUpdateApplication(personalDetails);
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
                    return RedirectToAction("AppPersonalDetails", "BOCWBhagyaLaxmiBondYojna");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "BOCWBhagyaLaxmiBondYojna", personalDetails);

            //return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", personalDetails);

            return RedirectToAction("BOCWBhagyaLaxmiBondYojnaSchemeDetails", "BOCWBhagyaLaxmiBondYojna");
        }

        public async Task<IActionResult> AddSchemeDetails(BOCWBPSYSchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            schemeDetails.ApplicationId = ApplicationId;
            schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            schemeDetails.HostName = CommonUtils.GetHostName();
            schemeDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            schemeDetails.TabSequenceNo = TabSequenceNo;
            schemeDetails.schemaname = nameof(EnumLookup.schemaname.bocw_bpsy);
            schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
            // schemeDetails.UserId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var regResponse = await _iBOCWBhagyaLaxmiBondYojnaService.AddSchemeDetails(schemeDetails);
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
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.bocw_bpsy);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
                        }
                        else
                        {
                            return PartialView("BOCWBhagyaLaxmiBondYojna/_BOCWBhagyaLaxmiBondYojnaDocument");
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
                    responseMessage = await _iBOCWBhagyaLaxmiBondYojnaService.AddUpdateDocumentDetailsNew(dtData);
                }
                if (dtData.Rows.Count == 0 && responseMessage != null && responseMessage.Error == 0)
                {
                    responseMessage.ApplicationNo = ApplicationId;
                    responseMessage.Id = ServiceId;
                }
                //ResponseMessage model = await _iBOCWBhagyaLaxmiBondYojnaService.AddUpdateDocumentDetails(lstdocumentFileDetails);
                return RedirectToAction("AppPersonalDetails",
                    new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.Id.ToString())), strTabId = "3", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.ApplicationNo.ToString())) });
            }
            //}
            //else
            //{
            //    IList<DocumentFileDetails> model = await _iBOCWBhagyaLaxmiBondYojnaService.GetDocumentFileDetails(Convert.ToInt32(2));
            //    return RedirectToAction("AppPersonalDetails", new { id = 2, TabId = 4 });
            //}

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
            //    //    List<DocumentDetails> model = await _iBOCWBhagyaLaxmiBondYojnaService.SaveDocumentDetails(lstcouchDBResponse);
            //    //    return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
            //    //}


            //    return View();
            //}

            #endregion

            //return View();
            return RedirectToAction("AppPersonalDetails", "BOCWBhagyaLaxmiBondYojna", documentFileDetails);
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
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.bocw_bpsy);
            //  finalSubmitModel.userid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";

            var regResponse = await _iBOCWBhagyaLaxmiBondYojnaService.FinalSubmit(finalSubmitModel);
            //if (regResponse != null)
            //if (regResponse != null && regResponse.Error == 0)
            //{
            //    var msg = "Your Form Is Submitted SuccessFully..!!";
            //    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
            //    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
            //}
            if (regResponse != null && regResponse.Error == 0)
            {

                CommonUtils commonFunction = new CommonUtils(_config);
                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                SMSModel modelSMSModel = await _iBOCWBhagyaLaxmiBondYojnaService.GetSmsContentForService(serviceId, ApplicationId, (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.bocw_bpsy), nameof(EnumLookup.tablename.personaldetails));
                if (modelSMSModel != null)
                {
                    bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                    if (isSendSMS)
                    {
                        await _iBOCWBhagyaLaxmiBondYojnaService.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                    }

                }


                var msg = regResponse.Msg;
                TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
            }

            var errorMsg = "Form Subbmission Failed..!!";
            //  TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");

            return RedirectToAction("BOCWBhagyaLaxmiBondYojnaTermsCondition", "BOCWBhagyaLaxmiBondYojna");
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
