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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using FileResult = Microsoft.AspNetCore.Mvc.FileResult;

namespace LabourCommissioner.Controllers
{
    //TEST

    [Authorize]
    public class BOCWSikshanSahayYojanaController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBOCWSikshanSahayYojanaService _iBOCWSikshanSahayYojanaService;

        public BOCWSikshanSahayYojanaController(IBOCWSikshanSahayYojanaService iBOCWSikshanSahayYojanaService,
            IConfiguration config, IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iBOCWSikshanSahayYojanaService = iBOCWSikshanSahayYojanaService ??
                                              throw new ArgumentNullException(nameof(_iBOCWSikshanSahayYojanaService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult DateValidation()
        {
            return RedirectToAction("ApplicationDetails", "Home");
        }

        public async Task<IActionResult> AppPersonalDetails(string strid, string strTabId, string strApplicationId)
        {
            var RegistrationID = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
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
            //var model = _iBOCWSikshanSahayYojanaService.GetServiceTabByServiceId(id);
            //  ViewBag.AppPersonalDetails = model.Result.ToList();
            var model = _iBOCWSikshanSahayYojanaService.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.bocw_ssy), nameof(EnumLookup.tablename.tabentry));
            ViewBag.AppPersonalDetails = (await model.ConfigureAwait(false)).ToList();

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
        public async Task<IActionResult> PersonalDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            var RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;


            // long ApplicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            long ApplicationId = Convert.ToInt64(strApplicationId);

            var model = await _iBOCWSikshanSahayYojanaService.GetPersonalDetailsByRegNo(RegistrationNo);
            var model1 = await _iBOCWSikshanSahayYojanaService.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.bocw_ssy), nameof(EnumLookup.tablename.personaldetails));
            //model1.ICardFromDate = model.ICardFromDate;
            //model1.ICardToDate = model.ICardToDate;

            ViewBag.Message = CommonUtils.ConcatString("જો શારીરિક રીતે શક્ષમ હોય તો ઉંમર ૩૦ થી વધારે ના હોવી જોઈએ.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            model.ApplicationNo = User.FindFirst(ClaimTypes.Name).Value;

            ViewBag.ServiceId = ServiceId;
            ViewBag.Name = model.Name;
            ViewBag.TabSequenceNo = 1;

            var stateModel = _iBOCWSikshanSahayYojanaService.GetAllStates();
            var states = (await stateModel.ConfigureAwait(false)).ToList();

            ViewBag.States = states;
            ViewBag.isFilled = isFilled;

            var districtModel = _iBOCWSikshanSahayYojanaService.GetDistrict();
            var district = (await districtModel.ConfigureAwait(false)).ToList();

            ViewBag.District = district;
            ViewBag.ApplicationId = ApplicationId;
            model.RegisteredUserName = model.Name;

            if (isFilled)
            {
                model1.RegisteredUserName = model.Name;
                model1.ICardFromDate = model.ICardFromDate;
                model1.ICardToDate = model.ICardToDate;
                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(model1.CouchDBDocId, model1.FileName);
                var base64Image = Convert.ToBase64String(await objCouchDBResponse.ImageData.ConfigureAwait(false));
                ViewBag.Image = base64Image;

                var PtalukaModel = _iBOCWSikshanSahayYojanaService.GetTalukaByDistrictId(model1.PDistrictId);
                var Ptaluka = (await PtalukaModel.ConfigureAwait(false)).ToList();

                var PvillageModel =
                    _iBOCWSikshanSahayYojanaService.GetVillageByDistrictIdAndTalukaId(model1.PDistrictId, model1.PTalukaId);
                var Pvillage = (await PvillageModel.ConfigureAwait(false)).ToList();

                var talukaModel = _iBOCWSikshanSahayYojanaService.GetTalukaByDistrictId(model1.CDistrictId);
                var Ctaluka = (await talukaModel.ConfigureAwait(false)).ToList();

                var villageModel =
                    _iBOCWSikshanSahayYojanaService.GetVillageByDistrictIdAndTalukaId(model1.CDistrictId, model1.CTalukaId);
                var Cvillage = (await villageModel.ConfigureAwait(false)).ToList();

                ViewBag.CTaluka = Ctaluka;
                ViewBag.CVillage = Cvillage;
                ViewBag.PTaluka = Ptaluka;
                ViewBag.PVillage = Pvillage;
                model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                return PartialView("BOCWSikshanSahayYojana/_PersonalDetails", model1);
            }
            model.Name = "";
            model.MobileNo = "";
            model.AadharCardNo = "";

            return PartialView("BOCWSikshanSahayYojana/_PersonalDetails", model);


            //return PartialView("BOCWSikshanSahayYojana/_PersonalDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWSikshanSahayYojanaSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            long ApplicationId = Convert.ToInt64(strApplicationId);
            var model1 = await _iBOCWSikshanSahayYojanaService.GetApplicationSchemeDetailsByAppId(ApplicationId);
            ViewBag.isFilled = isFilled;



            var educationModel = _iBOCWSikshanSahayYojanaService.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            IEnumerable<System.Web.Mvc.SelectListItem> items = null;
            if (model1 != null)
            {
                if (model1.Schmename == "2")
                {
                    items = (await educationModel.ConfigureAwait(false)).Where(c => c.Value == "9").Select(c => new System.Web.Mvc.SelectListItem
                    {
                        Value = c.Value,
                        Text = c.Text
                    });
                }
                else
                {
                    items = (await educationModel.ConfigureAwait(false))
                   .Where(c => c.Value != "9")
                   .Select(c => new System.Web.Mvc.SelectListItem
                   {
                       Value = c.Value,
                       Text = c.Text
                   });
                }
            }
            var education = (await educationModel.ConfigureAwait(false)).ToList();
            ViewBag.Syllabus = items;



            var semesterModel = _iBOCWSikshanSahayYojanaService.GetEducation(nameof(EnumLookup.ResourcesType.Semester));
            var semester = (await semesterModel.ConfigureAwait(false)).ToList();


            ViewBag.Semester = semester;


            if (model1 == null)
            {
                var degreeModel = _iBOCWSikshanSahayYojanaService.GetEducation(nameof(EnumLookup.ResourcesType.Degree));
                var degree = degreeModel.Result.ToList();
                ViewBag.Degree = degree;
                ViewBag.Course = 0;
                ViewBag.AcadmicYearSem = 0;

            }
            else
            {
                if (isFilled == true)
                {
                    var semesterYear = _iBOCWSikshanSahayYojanaService.GetSemesterbyCourseId(Convert.ToInt32(model1.Course));
                    semester = semesterYear.Result.ToList();
                    var degreeModel = _iBOCWSikshanSahayYojanaService.GetSubject(Convert.ToString(model1.Syllabus));
                    var degree = degreeModel.Result.ToList();
                    ViewBag.Degree = degree;
                    ViewBag.Semester = semester;
                }
            }


            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;
            // ViewBag.ApplicationId = ApplicationId;

            return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaSchemeDetails", model1);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWSikshanSahayYojanaDocument(string ServiceId, string strApplicationId, bool isFilled)
        {
            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 3;
            var model = await _iBOCWSikshanSahayYojanaService.GetDocumentFileDetails(Convert.ToInt32(ServiceId), Convert.ToInt32(ApplicationId));


            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iBOCWSikshanSahayYojanaService.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.bocw_ssy), nameof(EnumLookup.tablename.documentdetails));
                return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
            }



            //IEnumerable<DocumentDetails> model = await _iBOCWSikshanSahayYojanaService.GetFileDocuments(Convert.ToInt32(ServiceId));
            //return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);

            //DocumentDetails model = await _iBOCWSikshanSahayYojanaService.GetDocumentsDetails(Convert.ToInt32(ServiceId));
            // return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
        }

        [HttpGet]
        public IActionResult TermsCondition(string ServiceId, string strApplicationId, bool isFilled)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("BOCWSikshanSahayYojana/_TermsCondition");
        }

        //[HttpPost]

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _iBOCWSikshanSahayYojanaService.GetDistrict();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetSubject(string subjectId)
        {
            var subject = _iBOCWSikshanSahayYojanaService.GetSubject(subjectId);
            return Json(new { data = subject });
        }

        [HttpGet]
        public IActionResult GetBenifitByCourseId(int courseId, string semesteryear)
        {
            var course = _iBOCWSikshanSahayYojanaService.GetBenifitByCourseId(courseId, semesteryear);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = course });
        }

        [HttpGet]
        public IActionResult GetTalukaByDistrictId(int districtId)
        {
            var regions = _iBOCWSikshanSahayYojanaService.GetTalukaByDistrictId(districtId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }
        public IActionResult GetSemesterbyCourseId(int courseid)
        {
            var regions = _iBOCWSikshanSahayYojanaService.GetSemesterbyCourseId(courseid);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var regions = _iBOCWSikshanSahayYojanaService.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetEducation(int schemeType)
        {
            var regions = _iBOCWSikshanSahayYojanaService.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            IEnumerable<System.Web.Mvc.SelectListItem> items = null;
            if (regions.Result != null && regions.Result.Count() > 0 && schemeType == 1)
            {
                items = regions.Result
               .Where(c => c.Value != "9")
               .Select(c => new System.Web.Mvc.SelectListItem
               {
                   Value = c.Value,
                   Text = c.Text
               });
            }
            else if (regions.Result != null && regions.Result.Count() > 0 && schemeType == 2)
            {
                items = regions.Result
               .Where(c => c.Value == "9")
               .Select(c => new System.Web.Mvc.SelectListItem
               {
                   Value = c.Value,
                   Text = c.Text
               });
            }

            return Json(new { data = items });
        }

        //[HttpGet]
        //public ActionResult GetSyllabus(int schemeType)
        //{
        //    var regions = _iBOCWSikshanSahayYojanaService.GetEducation(nameof(EnumLookup.ResourcesType.Education));
        //    return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //}

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
            personalDetails.schemaname = nameof(EnumLookup.schemaname.bocw_ssy);
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
                    return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", personalDetails);
                }
            }

            #endregion


            var regResponse = await _iBOCWSikshanSahayYojanaService.AddUpdateApplication(personalDetails);
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
                    return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", personalDetails);

            //return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", personalDetails);

            return RedirectToAction("BOCWSikshanSahayYojanaSchemeDetails", "BOCWSikshanSahayYojana");
        }

        public async Task<IActionResult> AddSchemeDetails(SchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            schemeDetails.ApplicationId = ApplicationId;
            schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            schemeDetails.HostName = CommonUtils.GetHostName();
            schemeDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            schemeDetails.TabSequenceNo = TabSequenceNo;
            schemeDetails.schemaname = nameof(EnumLookup.schemaname.bocw_ssy);
            schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
            // schemeDetails.UserId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var regResponse = await _iBOCWSikshanSahayYojanaService.AddSchemeDetails(schemeDetails);
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
                            return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument");
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
                    responseMessage = await _iBOCWSikshanSahayYojanaService.AddUpdateDocumentDetailsNew(dtData);
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
            //}
            //else
            //{
            //    IList<DocumentFileDetails> model = await _iBOCWSikshanSahayYojanaService.GetDocumentFileDetails(Convert.ToInt32(2));
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
            //    //    List<DocumentDetails> model = await _iBOCWSikshanSahayYojanaService.SaveDocumentDetails(lstcouchDBResponse);
            //    //    return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
            //    //}


            //    return View();
            //}

            #endregion

            //return View();
            return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", documentFileDetails);
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
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.bocw_ssy);
            //  finalSubmitModel.userid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";

            var regResponse = await _iBOCWSikshanSahayYojanaService.FinalSubmit(finalSubmitModel);
            if (regResponse != null)
                if (regResponse != null && regResponse.Error == 0)
                {

                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                    SMSModel modelSMSModel = await _iBOCWSikshanSahayYojanaService.GetSmsContentForService(serviceId, ApplicationId, (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.bocw_ssy), nameof(EnumLookup.tablename.personaldetails));
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iBOCWSikshanSahayYojanaService.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                        }

                    }


                    var msg = regResponse.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
                }

            var errorMsg = "Form Subbmission Failed..!!";
            //  TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");

            return RedirectToAction("TermsCondition", "BOCWSikshanSahayYojana");
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

        // [AllowAnonymous]
        // [AcceptVerbs("Get", "Post")]
        // public ActionResult DateValidation(string dateofbirth)
        //{

        //     string pattern = @"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$";
        //     bool flag = Regex.IsMatch(dateofbirth, pattern);

        //     if (flag == false)
        //     {
        //         return Json($"કૃપા કરીને dd/MM/yyyy ફોર્મેટમાં તારીખ દાખલ કરો.");
        //     }
        //     else
        //     {
        //         return Json(true);
        //     }

        // }


        //[HttpGet]
        //public async Task<IActionResult> SchemeUsers(SchemeUserModel schemeUserModel)
        //{
        //    List<SchemeUserModel> schemeUserModel1 = await _schemeUserServices.GetSchemeUser(schemeUserModel);
        //    return View(schemeUserModel1);
        //}
    }

}