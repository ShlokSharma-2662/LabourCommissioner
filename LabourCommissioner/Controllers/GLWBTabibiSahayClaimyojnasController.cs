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
    public class GLWBTabibiSahayClaimyojnasController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGLWBTabibiSahayClaimyojnasService _iGLWBTabibiSahayService;


        public GLWBTabibiSahayClaimyojnasController(IGLWBTabibiSahayClaimyojnasService iGLWBTabibiSahayService,
          IConfiguration config, IHttpClientFactory clientFactory,
          IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iGLWBTabibiSahayService = iGLWBTabibiSahayService ??
                                              throw new ArgumentNullException(nameof(_iGLWBTabibiSahayService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult AppPersonalDetails(string strid, string strTabId, string strClaimApplicationId, string strApplicationId, string strOrgServiceId = "", string lwbaccountno ="")
        {
            var RegistrationID = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            int id = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strid)));
            int OrgServiceId = 0;
            if (strOrgServiceId != "")
            {
                OrgServiceId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strOrgServiceId)));
            }
            int TabId = 0;
            int ClaimApplicationId;
            if (strClaimApplicationId == null || strClaimApplicationId == "")
            {
                ClaimApplicationId = 0;
            }
            else
            {
                ClaimApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strClaimApplicationId)));

            }
            int ApplicationId;
            if (strApplicationId == null || strApplicationId == "")
            {
                ApplicationId = 0;
            }
            else
            {
                ApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));

            }
            //if (id == 16)
            //{
            var model = _iGLWBTabibiSahayService.GetTabSequenceByApplicationId(ClaimApplicationId, OrgServiceId, nameof(EnumLookup.schemaname.glwb_tsy_claim), nameof(EnumLookup.tablename.tabentry));
            ViewBag.AppPersonalDetails = model.Result.ToList();
            //}
            //else
            //{
            //    var model = _iGLWBHomeTownYojanaservice.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.glwb_hty_claim), nameof(EnumLookup.tablename.tabentry));
            //    ViewBag.AppPersonalDetails = model.Result.ToList();
            //}


            ViewBag.ClaimApplicationId = ClaimApplicationId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.lwbaccountno = lwbaccountno;
            return View();
        }


        //manually adeed 

        [HttpGet]
        public async Task<IActionResult> PersonalDetails(string ServiceId, string strApplicationId, bool isFilled, string strClaimApplicationId, string lwbaccountno)
        {
            var UserName = lwbaccountno;
            long ClaimApplicationId = Convert.ToInt64(strClaimApplicationId);
            long ApplicationId = Convert.ToInt64(strApplicationId);

            var model = await _iGLWBTabibiSahayService.GetCompanyDetailsByUserName(UserName);
            var model1 = await _iGLWBTabibiSahayService.GetApplicationDetailsByAppId(ClaimApplicationId, nameof(EnumLookup.schemaname.glwb_tsy_claim), nameof(EnumLookup.tablename.personaldetails));
            //model.hospitalname = User.FindFirst(ClaimTypes.Name).Value;
            model.hospitalname = _claimPincipal.FindFirstValue("DisplayName");
            ViewBag.ServiceId = ServiceId;
            ViewBag.TabSequenceNo = 1;

            ViewBag.isFilled = isFilled;
            ViewBag.lwbaccountno = lwbaccountno;
            ViewBag.ApplicationId = ApplicationId;
            if (isFilled)
            {
                return PartialView("GLWBTabibiSahayClaimyojnas/_GLWBTabibiSahayClaimyojnasPersonalDetails", model1);
            }

            return PartialView("GLWBTabibiSahayClaimyojnas/_GLWBTabibiSahayClaimyojnasPersonalDetails", model);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBTabibiSahayClaimyojnasSchemeDetails(string ServiceId, string strApplicationId, string strClaimApplicationId, bool isFilled)
        {
            long ApplicationId = Convert.ToInt64(strApplicationId);
            long ClaimApplicationId = Convert.ToInt64(strClaimApplicationId);
            int serviceId = Convert.ToInt32(ServiceId);


            var totalsahay = _iGLWBTabibiSahayService.GetTotalsahayByServiceID(serviceId);
            ViewBag.TotalSahay = totalsahay.Result.totalsahay;
            ViewBag.isFilled = isFilled;

            if (isFilled == true)
            {
                var model1 = await _iGLWBTabibiSahayService.GetApplicationSchemeDetailsByAppId(ClaimApplicationId);
                var lwbaccountno1 = model1.lwbaccountno;
                var employeelabourdetails1 = _iGLWBTabibiSahayService.BindEmployee(lwbaccountno1);
                var employeedetails1 = employeelabourdetails1.Result.ToList();

                if (model1 != null)
                {
                    ViewBag.EmployeeDetailsID = model1.lstCompanyWorkerDetails[0].registrationid;
                }
                ViewBag.EmployeeDetails = employeedetails1;

                ViewBag.ServiceId = ServiceId;
                ViewBag.ApplicationId = ApplicationId;
                ViewBag.TabSequenceNo = 2;
                ViewBag.ClaimApplicationId = strClaimApplicationId;
                ViewBag.lwbaccountno = lwbaccountno1;
                return PartialView("GLWBTabibiSahayClaimyojnas/_GLWBTabibiSahayClaimyojnasSchemeDetails", model1);
            }

            else
            {
                var model = await _iGLWBTabibiSahayService.Glwb_Tsy_getapplicationschemedetailsbyappidclaim(ApplicationId);
                var lwbaccountno = model.lwbaccountno;
                var employeelabourdetails = _iGLWBTabibiSahayService.BindEmployee(lwbaccountno);
                var employeedetails = employeelabourdetails.Result.ToList();

                if (model != null)
                {
                    ViewBag.EmployeeDetailsID = model.lstCompanyWorkerDetails[0].registrationid;
                }
                ViewBag.EmployeeDetails = employeedetails;

                ViewBag.ServiceId = ServiceId;
                ViewBag.ApplicationId = ApplicationId;
                ViewBag.TabSequenceNo = 2;
                ViewBag.ClaimApplicationId = strClaimApplicationId;
                ViewBag.lwbaccountno = lwbaccountno;
                return PartialView("GLWBTabibiSahayClaimyojnas/_GLWBTabibiSahayClaimyojnasSchemeDetails", model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GLWBTabibiSahayClaimyojnasDocument(string ServiceId, string strApplicationId, bool isFilled, string strClaimApplicationId, string lwbaccountno)
        {
            string ApplicationId = strApplicationId;
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.ClaimApplicationId = strClaimApplicationId;
            if (isFilled == true)
            {
                ViewBag.ClaimApplicationId = ApplicationId;
                
            }
            ViewBag.TabSequenceNo = 3;
            var model = await _iGLWBTabibiSahayService.GetDocumentFileDetails(Convert.ToInt32(ServiceId), Convert.ToInt32(ApplicationId));
            ViewBag.lwbaccountno = lwbaccountno;

            if (isFilled == true)
            {
                var GetUploadedDocuments = await _iGLWBTabibiSahayService.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.glwb_tsy_claim), nameof(EnumLookup.tablename.documentdetails));
                return PartialView("GLWBTabibiSahayClaimyojnas/_GLWBTabibiSahayClaimyojnasDocument", GetUploadedDocuments);
            }
            else
            {
                return PartialView("GLWBTabibiSahayClaimyojnas/_GLWBTabibiSahayClaimyojnasDocument", model);
            }
        }

        [HttpGet]
        public IActionResult TermsCondition(string ServiceId, string strApplicationId, bool isFilled, string strClaimApplicationId)
        {
            int ApplicationId = Convert.ToInt32(strApplicationId);
            int ClaimApplicationId = Convert.ToInt32(strClaimApplicationId);
            ViewBag.ServiceId = ServiceId;
            ViewBag.ApplicationId = ApplicationId;
            ViewBag.ClaimApplicationId = ClaimApplicationId;
            ViewBag.TabSequenceNo = 4;
            return PartialView("GLWBTabibiSahayClaimyojnas/_GLWBTabibiSahayClaimyojnasTermsCondition");
        }


        [HttpGet]
        public IActionResult BindEmployee()
        {
            var lwbaccountno = User.FindFirst(ClaimTypes.Name).Value;
            var regions = _iGLWBTabibiSahayService.BindEmployee(lwbaccountno);
            return Json(new { data = regions });
        }


        [HttpGet]
        public IActionResult GetGLWBEmployeeDetailsbyRegistrationid(int registrationid)
        {
            var regions = _iGLWBTabibiSahayService.GetGLWBEmployeeDetailsbyRegistrationid(registrationid);
            return Json(new { data = regions });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateApplication(GLWB_TSYClaim_personalDetails personalDetails, string strTabId, string strApplicationNo)
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


            personalDetails.schemaname = nameof(EnumLookup.schemaname.glwb_tsy_claim);
            personalDetails.tablename = nameof(EnumLookup.tablename.personaldetails);
            personalDetails.applicationfrom = Convert.ToInt32(EnumLookup.applicationfrom.web);

            personalDetails.CreatedDate = DateTime.Now;
            personalDetails.IsDeleted = false;
            personalDetails.ApplicationNo = ApplicationNo;
            personalDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));

            var regResponse = await _iGLWBTabibiSahayService.AddUpdateApplication(personalDetails);
            if (regResponse != null)
            {
                //string errorMsg = regResponse.ResponseMsg == null ? "Somthing went wrong please try again." : regResponse.ResponseMsg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    //TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                    ModelState.Clear();
                    var empEmpty = new PersonalDetailsModel();
                    var lwbaccountno = personalDetails.LwbAccountNo;
                    //return RedirectToAction("BOCWSikshanSahayYojanaSchemeDetails", "BOCWSikshanSahayYojana");
                    return RedirectToAction("AppPersonalDetails",
                        new
                        {
                            strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(personalDetails.ServiceId.ToString())),
                            strOrgServiceId = HttpUtility.UrlEncode(CommonUtils.Encrypt(personalDetails.ServiceId.ToString())),

                            strTabId = "1",
                            strClaimApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())),
                            strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())),
                            lwbaccountno = lwbaccountno

                        });
                }
                else
                {
                    //TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    var empEmpty = new Registration();
                    return RedirectToAction("AppPersonalDetails", "GLWBTabibiSahayClaimyojnas");
                }
            }

            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.",
                Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("AppPersonalDetails", "GLWBTabibiSahayClaimyojnas", personalDetails);

            //return RedirectToAction("AppPersonalDetails", "BOCWSikshanSahayYojana", personalDetails);

            return RedirectToAction("GLWBTabibiSahayClaimyojnasSchemeDetails", "GLWBTabibiSahayClaimyojnas");
        }

        public async Task<IActionResult> AddSchemeDetails(GLWBTSYSchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            schemeDetails.ApplicationId = ApplicationId;
            schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
            schemeDetails.HostName = CommonUtils.GetHostName();
            schemeDetails.CreatedBy = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            schemeDetails.TabSequenceNo = TabSequenceNo;
            schemeDetails.schemaname = nameof(EnumLookup.schemaname.glwb_tsy_claim);
            schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
            //var finaltotalsahay = schemeDetails.totalsahay * 25 / 100;
            // schemeDetails.UserId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (schemeDetails.lstCompanyWorkerDetails != null && schemeDetails.lstCompanyWorkerDetails.Count() > 0)
            {
                var lstCompanyWorkerDetails = new List<CompanyWorkerDetails>();
                var RegistrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
                int count = 0;
                foreach (var item in schemeDetails.lstCompanyWorkerDetails)
                {

                    var objCompanyWorkerDetails = new CompanyWorkerDetails();

                    if (item.isDeleted == false)
                    {
                        objCompanyWorkerDetails.workerdetailsid = item.workerdetailsid;
                        objCompanyWorkerDetails.registrationid = item.registrationid;
                        //objCompanyWorkerDetails.workername = item.workername;
                        //objCompanyWorkerDetails.MobileNo = item.MobileNo;
                        objCompanyWorkerDetails.gender = item.gender;
                        objCompanyWorkerDetails.DateOfBirth = item.DateOfBirth;
                        objCompanyWorkerDetails.ageyear = item.ageyear;
                        objCompanyWorkerDetails.sanmanregistrationno = item.sanmanregistrationno;
                        objCompanyWorkerDetails.ischeckeup = item.ischeckeup;

                        objCompanyWorkerDetails.CreatedBy = RegistrationId;
                        lstCompanyWorkerDetails.Add(objCompanyWorkerDetails);
                    }
                    if (item.ischeckeup == true)
                    {
                        count++;
                    }


                }
                schemeDetails.finaltotalsahay = schemeDetails.totalsahay * count;
                var dtData = CommonUtils.ToDataTable(lstCompanyWorkerDetails);
                var lwbaccountno = schemeDetails.lwbaccountno;

                var regResponse = await _iGLWBTabibiSahayService.AddSchemeDetails(schemeDetails, dtData);
                return RedirectToAction("AppPersonalDetails", new
                {
                    strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(schemeDetails.ServiceId.ToString())),
                    strTabId = "2",
                    strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())),
                    strClaimApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(regResponse.Id.ToString())),
                    strOrgServiceId = HttpUtility.UrlEncode(CommonUtils.Encrypt(schemeDetails.ServiceId.ToString())),
                    lwbaccountno = lwbaccountno
                });


            }

            return RedirectToAction("AppPersonalDetails", "GLWBTabibiSahayClaimyojnas", schemeDetails);
        }

        public async Task<IActionResult> UploadDocument(IList<DocumentFileDetails> documentFileDetails, string strTabSequenceNo, string strApplicationId, string strServiceId, string strClaimApplicationId, string lwbaccountno)
        {
            //if(ModelState.Is)
            //return RedirectToAction("AppPersonalDetails", new { id = 2, TabId = 4 });
            //if (ModelState.IsValid)
            //{
            //  int TabSequenceNo = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strTabSequenceNo)));
            int TabSequenceNo = Convert.ToInt32(strTabSequenceNo);
            int ApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            int ClaimApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strClaimApplicationId)));
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
                            objdocumentFileDetails.ApplicationId = ClaimApplicationId;
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
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.glwb_tsy_claim);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
                        }
                        else
                        {
                            return PartialView("GLWBTabibiSahayClaimyojnas/_GLWBTabibiSahayClaimyojnas");
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
                    responseMessage = await _iGLWBTabibiSahayService.AddUpdateDocumentDetailsNew(dtData);
                }
                if (dtData.Rows.Count == 0 && responseMessage != null && responseMessage.Error == 0)
                {
                    responseMessage.ApplicationNo = ApplicationId;
                    responseMessage.Id = ServiceId;
                }

                

                //ResponseMessage model = await _iGLWBTabibiSahayService.AddUpdateDocumentDetails(lstdocumentFileDetails);
                return RedirectToAction("AppPersonalDetails",
                    new
                    {
                        strid = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.Id.ToString())),
                        strOrgServiceId = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.Id.ToString())),
                        strTabId = "3",
                        strApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.ApplicationNo.ToString())),
                        strClaimApplicationId = HttpUtility.UrlEncode(CommonUtils.Encrypt(responseMessage.ApplicationNo.ToString())),
                        lwbaccountno = lwbaccountno
                    });
            }
            //}
            //else
            //{
            //    IList<DocumentFileDetails> model = await _iGLWBTabibiSahayService.GetDocumentFileDetails(Convert.ToInt32(2));
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
            //    //    List<DocumentDetails> model = await _iGLWBTabibiSahayService.SaveDocumentDetails(lstcouchDBResponse);
            //    //    return PartialView("BOCWSikshanSahayYojana/_BOCWSikshanSahayYojanaDocument", model);
            //    //}


            //    return View();
            //}

            #endregion

            //return View();
            return RedirectToAction("AppPersonalDetails", "GLWBTabibiSahayClaimyojnas", documentFileDetails);
        }

        [HttpPost]
        public async Task<IActionResult> FinalSubmit(FinalSubmitModel finalSubmitModel, string strApplicationId, string strServiceId, string strTabSequenceNo, string strClaimApplicationId)
        {

            long ApplicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            long ClaimApplicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strClaimApplicationId)));
            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            long TabSequenceNo = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strTabSequenceNo)));

            finalSubmitModel.ApplicationId = ApplicationId;
            finalSubmitModel.ClaimApplicationId = ClaimApplicationId;
            finalSubmitModel.serviceid = serviceId;
            finalSubmitModel.tabsequenceno = TabSequenceNo;
            finalSubmitModel.ipaddress = CommonUtils.GetLocalIPAddress();
            finalSubmitModel.hostname = CommonUtils.GetHostName();
            finalSubmitModel.ResigtrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            //finalSubmitModel.tablename = "bocw_ssy.personaldetails";
            //finalSubmitModel.schemaname = "bocw_ssy";
            finalSubmitModel.tablename = nameof(EnumLookup.tablename.personaldetails);
            finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.glwb_tsy_claim);
            //  finalSubmitModel.userid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
            finalSubmitModel.userid = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            finalSubmitModel.benificiarytype = Convert.ToInt32(_claimPincipal.FindFirstValue("BeneficiaryType"));
            finalSubmitModel.greensoldierreferralcode = "";

            var regResponse = await _iGLWBTabibiSahayService.FinalSubmit(finalSubmitModel);
            if (regResponse != null)
                if (regResponse != null && regResponse.Error == 0)
                {

                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                    SMSModel modelSMSModel = await _iGLWBTabibiSahayService.GetSmsContentForService(serviceId, ApplicationId, (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.glwb_tsy_claim), nameof(EnumLookup.tablename.personaldetails));
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iGLWBTabibiSahayService.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                        }

                    }


                    var msg = regResponse.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("ApplicationDetails", "Home", new { strserviceId = CommonUtils.Encrypt(HttpUtility.UrlEncode(regResponse.Id.ToString())) });
                }

            var errorMsg = "Form Subbmission Failed..!!";
            //  TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");

            return RedirectToAction("TermsCondition", "GLWBTabibiSahay");
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
