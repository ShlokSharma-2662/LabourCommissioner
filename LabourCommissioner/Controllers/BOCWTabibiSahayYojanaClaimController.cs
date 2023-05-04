using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using javax.swing.text.html.parser;
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
using sun.security.jca;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using FileResult = Microsoft.AspNetCore.Mvc.FileResult;

namespace LabourCommissioner.Controllers
{

    public class BOCWTabibiSahayYojanaClaimController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBOCWTabibiSahayYojanaClaimService _iBOCWTabibiSahayYojanaService;


        public BOCWTabibiSahayYojanaClaimController(IBOCWTabibiSahayYojanaClaimService iBOCWTabibiSahayYojanaService,
          IConfiguration config, IHttpClientFactory clientFactory,
          IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iBOCWTabibiSahayYojanaService = iBOCWTabibiSahayYojanaService ??
                                              throw new ArgumentNullException(nameof(_iBOCWTabibiSahayYojanaService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> AppPersonalDetails(string strid, string strTabId, string strApplicationId)
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
            var model = _iBOCWTabibiSahayYojanaService.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.bocw_tbsy_claim), nameof(EnumLookup.tablename.tabentry));
            ViewBag.AppPersonalDetails = (await model.ConfigureAwait(false)).ToList();
            ViewBag.ApplicationId = ApplicationId;

            return View();
        }


        //manually adeed 

        [HttpGet]
        public async Task<IActionResult> PersonalDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            var UserName = User.FindFirst(ClaimTypes.Name).Value;

            long ApplicationId = Convert.ToInt64(strApplicationId);

            var model = await _iBOCWTabibiSahayYojanaService.GetCompanyDetailsByUserName(UserName);
            var model1 = await _iBOCWTabibiSahayYojanaService.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.bocw_tbsy_claim), nameof(EnumLookup.tablename.personaldetails));
            model.hospitalname = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.ServiceId = ServiceId;
            ViewBag.TabSequenceNo = 1;

            ViewBag.isFilled = isFilled;

            ViewBag.ApplicationId = ApplicationId;
            if (isFilled)
            {
                return PartialView("BOCWTabibiSahayYojanaClaim/_BOCWTabibiSahayYojanaClaimPersonalDetails", model1);
            }
            return PartialView("BOCWTabibiSahayYojanaClaim/_BOCWTabibiSahayYojanaClaimPersonalDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWTabibiSahayYojanaClaimSchemeDetails(string ServiceId, string strApplicationId, bool isFilled)
        {
            long ApplicationId = Convert.ToInt64(strApplicationId);
            long serviceId = Convert.ToInt64(ServiceId);
            string tablename = nameof(EnumLookup.tablename.personaldetails);
            string schemaname = nameof(EnumLookup.schemaname.bocw_tbsy);

            var model1 = await _iBOCWTabibiSahayYojanaService.GetApplicationSchemeDetailsByAppId(ApplicationId);

            var model = await _iBOCWTabibiSahayYojanaService.GetCheckedUPApplicationDetailsByServiceId(serviceId, schemaname, tablename);
            model = model.ToList();
            ViewBag.isFilled = isFilled;



            //var totalsahay = _iBOCWTabibiSahayYojanaClaimService.GetTotalsahayByServiceID(serviceId);
            //ViewBag.TotalSahay = totalsahay.Result.totalsahay;

            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 2;
            ViewBag.ApplicationId = ApplicationId;
            return PartialView("BOCWTabibiSahayYojanaClaim/_BOCWTabibiSahayYojanaClaimSchemeDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWTabibiSahayYojanaClaimDocument(string ServiceId, string strApplicationId, bool isFilled)
        {
            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 3;
            var model = await _iBOCWTabibiSahayYojanaService.GetDocumentFileDetails(Convert.ToInt32(ServiceId), Convert.ToInt32(ApplicationId));


            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iBOCWTabibiSahayYojanaService.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.bocw_tbsy_claim), nameof(EnumLookup.tablename.documentdetails));
                return PartialView("BOCWTabibiSahayYojanaClaim/_BOCWTabibiSahayYojanaClaimDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("BOCWTabibiSahayYojanaClaim/_BOCWTabibiSahayYojanaClaimDocument", model);
            }



            //IEnumerable<DocumentDetails> model = await _iBOCWTabibiSahayYojanaClaimService.GetFileDocuments(Convert.ToInt32(ServiceId));
            //return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);

            //DocumentDetails model = await _iBOCWTabibiSahayYojanaClaimService.GetDocumentsDetails(Convert.ToInt32(ServiceId));
            // return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
        }

        [HttpGet]
        public IActionResult TermsCondition(string ServiceId, string strApplicationId, bool isFilled)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("BOCWTabibiSahayYojanaClaim/_BOCWTabibiSahayYojanaClaimTermsCondition");
        }

        //[HttpPost]

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _iBOCWTabibiSahayYojanaService.GetDistrict();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetSubject(string subjectId)
        {
            var subject = _iBOCWTabibiSahayYojanaService.GetSubject(subjectId);
            return Json(new { data = subject });
        }

        [HttpGet]
        public IActionResult GetBenifitByCourseId(int courseId, string semesteryear)
        {
            var course = _iBOCWTabibiSahayYojanaService.GetBenifitByCourseId(courseId, semesteryear);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = course });
        }

        [HttpGet]
        public IActionResult GetTalukaByDistrictId(int districtId)
        {
            var regions = _iBOCWTabibiSahayYojanaService.GetTalukaByDistrictId(districtId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }
        public IActionResult GetSemesterbyCourseId(int courseid)
        {
            var regions = _iBOCWTabibiSahayYojanaService.GetSemesterbyCourseId(courseid);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var regions = _iBOCWTabibiSahayYojanaService.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public ActionResult GetEducation()
        {
            var regions = _iBOCWTabibiSahayYojanaService.GetEducation(nameof(EnumLookup.ResourcesType.Education));
            return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddUpdateApplication(BOCW_TBSYClaim_personalDetails personalDetails, string strTabId, string strApplicationNo)
        {
            int TabSequenceNo = Convert.ToInt32(strTabId);
            string ApplicationNo = strApplicationNo;
            personalDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            personalDetails.HostName = CommonUtils.GetHostName();
            personalDetails.RegistrationNo = User.FindFirst(ClaimTypes.Name).Value;
            personalDetails.RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            personalDetails.schemaname = nameof(EnumLookup.schemaname.bocw_tbsy_claim);
            personalDetails.tablename = nameof(EnumLookup.tablename.personaldetails);
            personalDetails.applicationfrom = Convert.ToInt32(EnumLookup.applicationfrom.web);

            personalDetails.CreatedDate = DateTime.Now;
            personalDetails.IsDeleted = false;
            personalDetails.ApplicationNo = ApplicationNo;
            personalDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            //personalDetails.AadharCardNo = CommonUtils.EncryptCRY(personalDetails.AadharCardNo);

            var regResponse = await _iBOCWTabibiSahayYojanaService.AddUpdateApplication(personalDetails);
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
                    return RedirectToAction("AppPersonalDetails", "BOCWTabibiSahayYojanaClaim");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "BOCWTabibiSahayYojanaClaim", personalDetails);

            //return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", personalDetails);

            return RedirectToAction("BOCWTabibiSahayYojanaClaimSchemeDetails", "BOCWTabibiSahayYojanaClaim");
        }

        public async Task<IActionResult> AddSchemeDetails(IList<BOCW_TBSYClaim_personalDetails> schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            var serviceid = Convert.ToInt32(_claimPincipal.FindFirstValue("ServiceId"));
            string tablename = nameof(EnumLookup.tablename.personaldetails);
            string schemaname = nameof(EnumLookup.schemaname.bocw_tbsy);
            IList<BOCW_TBSYClaim_personalDetails> model = (IList<BOCW_TBSYClaim_personalDetails>)await _iBOCWTabibiSahayYojanaService.GetCheckedUPApplicationDetailsByServiceId(serviceid, schemaname, tablename);
            var IpAddress = CommonUtils.GetLocalIPAddress();
            var HostName = CommonUtils.GetHostName();
            var finalmodel = model.ToList();
            var dtData = CommonUtils.ToDataTable(finalmodel);
            var rowCnt = dtData.Rows.Count;
            var totalemployeeclaimed = rowCnt;
            var sahay = _iBOCWTabibiSahayYojanaService.GetTotalsahayByServiceID(36);
            var totalsahay = sahay.Result.totalsahay;
            var finaltotalsahay = totalsahay * rowCnt;

            DataColumn dataColumn = new DataColumn("totalsahay", typeof(decimal));
            dtData.Columns.Add(dataColumn);


            for (var i = 0; i < rowCnt; i++)
            {
                DataRow rowToUpdate = dtData.Rows[i]; // Assuming you want to update the first row
                rowToUpdate.SetField("IpAddress", IpAddress);
                rowToUpdate.SetField("HostName", HostName);
                rowToUpdate.SetField("tabsequenceno", 2);
                rowToUpdate.SetField("totalsahay", Convert.ToDecimal(finaltotalsahay));
                dtData.AcceptChanges();
            }


            var regResponse = await _iBOCWTabibiSahayYojanaService.AddSchemeDetails(dtData);
            return RedirectToAction("AppPersonalDetails",
                new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt((regResponse.Id).ToString())), strTabId = "2", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt((regResponse.ApplicationNo).ToString())) });
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
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.bocw_tbsy_claim);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
                        }
                        else
                        {
                            return PartialView("BOCWTabibiSahayYojanaClaim/_BOCWTabibiSahayYojanaClaimDocument");
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
                    responseMessage = await _iBOCWTabibiSahayYojanaService.AddUpdateDocumentDetailsNew(dtData);
                }
                if (dtData.Rows.Count == 0 && responseMessage != null && responseMessage.Error == 0)
                {
                    responseMessage.ApplicationNo = ApplicationId;
                    responseMessage.Id = ServiceId;
                }
                //ResponseMessage model = await _iBOCWTabibiSahayYojanaClaimService.AddUpdateDocumentDetails(lstdocumentFileDetails);
                return RedirectToAction("AppPersonalDetails",
                    new { strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.Id.ToString())), strTabId = "3", strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.ApplicationNo.ToString())) });
            }
            //}
            //else
            //{
            //    IList<DocumentFileDetails> model = await _iBOCWTabibiSahayYojanaClaimService.GetDocumentFileDetails(Convert.ToInt32(2));
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
            //    //    List<DocumentDetails> model = await _iBOCWTabibiSahayYojanaClaimService.SaveDocumentDetails(lstcouchDBResponse);
            //    //    return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
            //    //}


            //    return View();
            //}

            #endregion

            //return View();
            return RedirectToAction("AppPersonalDetails", "BOCWTabibiSahayYojanaClaim", documentFileDetails);
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
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.bocw_tbsy_claim);
            //  finalSubmitModel.userid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";

            var regResponse = await _iBOCWTabibiSahayYojanaService.FinalSubmit(finalSubmitModel);
            if (regResponse != null)
                if (regResponse != null && regResponse.Error == 0)
                {

                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                    //SMSModel modelSMSModel = await _iBOCWTabibiSahayYojanaService.GetSmsContentForService(serviceId, ApplicationId, (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.bocw_tbsy_claim), nameof(EnumLookup.tablename.personaldetails));
                    //if (modelSMSModel != null)
                    //{
                    //    bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                    //    if (isSendSMS)
                    //    {
                    //        await _iBOCWTabibiSahayYojanaService.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                    //    }

                    //}


                    var msg = regResponse.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
                }

            var errorMsg = "Form Subbmission Failed..!!";
            //  TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");

            return RedirectToAction("TermsCondition", "BOCWTabibiSahayYojanaClaim");
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
