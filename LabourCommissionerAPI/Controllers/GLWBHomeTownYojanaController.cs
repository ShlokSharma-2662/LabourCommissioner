using DocumentFormat.OpenXml.EMMA;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using LabourCommissionerAPI.ResponseModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissionerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GLWBHomeTownYojanaController : ControllerBase
    {
        private readonly IGLWBHomeTownYojanaService _iGLWBHomeTownYojanaservice;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ApiResponse apiResponse = new ApiResponse();


        public GLWBHomeTownYojanaController(IGLWBHomeTownYojanaService iGLWBHomeTownYojanaservice,
            IConfiguration config, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iGLWBHomeTownYojanaservice = iGLWBHomeTownYojanaservice ?? throw new ArgumentNullException(nameof(_iGLWBHomeTownYojanaservice));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }




        [HttpGet("appPersonalDetails")]
        public async Task<IActionResult> AppPersonalDetails(int ApplicationId, int id, string schemaname, string tablename)
        {
            try
            {
                var model = await _iGLWBHomeTownYojanaservice.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.tabentry));

                if (model != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.TabSequence_Display);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.TabSequence_Not_Display);
                    apiResponse.StackTrace = null;
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }
            return Ok(apiResponse);
        }

        [HttpPost("personaldetails")]
        public async Task<IActionResult> GLWBHomeTownYojanaPersonalDetails(string ServiceId, string strApplicationId, bool isFilled, [FromBody] UserCoockiesModel userCoockiesModel)
        {
            try
            {
                var RegistrationNo = userCoockiesModel.UserName;
                var model = await _iGLWBHomeTownYojanaservice.GetPersonalDetailsByRegNo(RegistrationNo);
                model.ApplicationNo = userCoockiesModel.UserName;
                if (isFilled)
                {
                    long ApplicationId = Convert.ToInt64(strApplicationId);
                    var model1 = await _iGLWBHomeTownYojanaservice.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.personaldetails));
                    model1.RegistrationNo = userCoockiesModel.UserName;
                    model1.registrationname = model.Name;
                    model1.AadharCardNo = CommonUtils.DecryptCRY(model1.AadharCardNo);
                    model1.MaskedAadharCardNo = CommonUtils.MaskString(model1.AadharCardNo);
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model1;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Get_PersonnalDetails);
                    apiResponse.StackTrace = null;
                }
                else if (!isFilled)
                {

                    model.ApplicationNo = userCoockiesModel.UserName;

                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Get_PersonnalDetails);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Not_Get_PersonnalDetails);
                    apiResponse.StackTrace = null;
                }

            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }

            return Ok(apiResponse);

        }




        [HttpPost("addupdatePersonalDetails")]
        public async Task<IActionResult> AddUpdateApplication([FromForm] GLWBhty_PersonalDetailsModel personalDetails, [FromForm] UserCoockiesModel userCoockiesModel, string strTabId)
        {
            try
            {
                int TabSequenceNo = Convert.ToInt32(strTabId);
                personalDetails.RegistrationNo = userCoockiesModel.UserName;
                personalDetails.RegistrationId = Convert.ToInt32(userCoockiesModel.RegistrationId);
                personalDetails.IpAddress = CommonUtils.GetLocalIPAddress();
                personalDetails.HostName = CommonUtils.GetHostName();
                personalDetails.CreatedDate = DateTime.Now;
                personalDetails.CreatedBy = Convert.ToInt32(userCoockiesModel.RegistrationId);
                personalDetails.IsDeleted = false;
                personalDetails.schemaname = nameof(EnumLookup.schemaname.glwb_hty);
                personalDetails.tablename = nameof(EnumLookup.tablename.personaldetails);
                personalDetails.AadharCardNo = CommonUtils.EncryptCRY(personalDetails.AadharCardNo);
                personalDetails.applicationfrom = Convert.ToInt32(EnumLookup.applicationfrom.mobile);
                if (personalDetails.Photo != null)
                {
                    var extension = Path.GetExtension(personalDetails.Photo.FileName);
                    personalDetails.FileName = personalDetails.RegistrationId + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Convert.ToString(extension);
                }

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
                }

                #endregion

                var regResponse = await _iGLWBHomeTownYojanaservice.AddUpdateApplication(personalDetails);
                if (regResponse != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = regResponse;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.PersonnalDetails_Added);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.PersonnalDetails_Not_Added);
                    apiResponse.StackTrace = null;
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }
            return Ok(apiResponse);
        }

        [HttpGet("getschemedetails")]
        public async Task<IActionResult> GLWBHomeTownYojanaSchemeDetails(string ServiceId, string strApplicationId)
        {
            try
            {
                long ApplicationId = Convert.ToInt64(strApplicationId);
                var model1 = await _iGLWBHomeTownYojanaservice.GetApplicationSchemeDetailsByAppId(ApplicationId);
                if (model1 != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model1;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Get_SchemeDetails);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Not_Get_SchemeDetails);
                    apiResponse.StackTrace = null;
                }

            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }
            return Ok(apiResponse);
        }
        [HttpPost("addSchemeDetail")]
        public async Task<IActionResult> AddSchemeDetails([FromForm] GLWBhtySchemeDetails schemeDetails, int TabSequenceNo, int ApplicationId)
        {
            try
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
                    var model = await _iGLWBHomeTownYojanaservice.AddSchemeDetails(schemeDetails, dtData);
                    if (model != null)
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                        apiResponse.Result = model;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.schemeDetails_Added);
                        apiResponse.StackTrace = null;
                    }
                    else
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                        apiResponse.Result = null;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.schemeDetails_Not_Added);
                        apiResponse.StackTrace = null;
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }
            return Ok(apiResponse);
        }


        [HttpGet("getdocuments")]
        public async Task<IActionResult> GLWBHomeTownYojanaDocument(string ServiceId, string strApplicationId)
        {
            try
            {
                string ApplicationId = strApplicationId;
                var model = await _iGLWBHomeTownYojanaservice.GetUploadedDocuments(Convert.ToInt64(ApplicationId), Convert.ToInt32(ServiceId), nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.documentdetails));
                if (model != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = model;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Get_DocumentDetails);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Not_Get_DocumentDetails);
                    apiResponse.StackTrace = null;
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }
            return Ok(apiResponse);

        }

        [HttpGet("DownloadDocuments")]
        public async Task<IActionResult> DownloadDocuments(string id, string fileName)
        {
            try
            {
                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetDocumentByteArray(id, fileName);

                if (objCouchDBResponse != null)
                {
                    byte[] fileBytes = await objCouchDBResponse.ImageData;
                    string base64String = Convert.ToBase64String(fileBytes);
                    // return File(base64String, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = objCouchDBResponse;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Documents_Display);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.No_Documents_Display);
                    apiResponse.StackTrace = null;
                }

            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }
            return Ok(apiResponse);
        }


        [HttpPost("uploadDocuments")]
        public async Task<IActionResult> UploadDocument([FromForm] IList<DocumentFileDetails> documentFileDetails, [FromForm] UserCoockiesModel userCoockiesModel, string strTabSequenceNo, string strApplicationId, string strServiceId)
        {
            int TabSequenceNo = Convert.ToInt32(strTabSequenceNo);


            if (documentFileDetails != null && documentFileDetails.Count() > 0)
            {
                var lstdocumentFileDetails = new List<DocumentFileDetails>();
                var RegistrationId = Convert.ToInt32(userCoockiesModel.RegistrationId);
                foreach (var item in documentFileDetails)
                {
                    var objdocumentFileDetails = new DocumentFileDetails();
                    var couchDBRequest = new CouchDBRequest();
                    var couchDBResponse = new CouchDBResponse();

                    if (item.IdImage != null && item.IdImage.File != null && item.IdImage.File.FileName != "" &&
                        item.IdImage.File.Length > 0)
                    {


                        var extension = Path.GetExtension(item.IdImage.File.FileName);
                        string FileName = RegistrationId + "_" + item.DocumentName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") +
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
                            objdocumentFileDetails.ApplicationId = Convert.ToInt64(strApplicationId);
                            objdocumentFileDetails.DocumentMasterId = item.DocumentMasterId;
                            objdocumentFileDetails.DocumentName = FileName;
                            objdocumentFileDetails.DocumentNumber = item.DocumentNumber;
                            objdocumentFileDetails.CouchDBDocId = couchDBResponse.Id;
                            objdocumentFileDetails.CouchDBDocRevId = couchDBResponse.Rev;
                            objdocumentFileDetails.ServiceId = Convert.ToInt64(strServiceId);
                            objdocumentFileDetails.CreatedBy = RegistrationId;
                            objdocumentFileDetails.IpAddress = CommonUtils.GetLocalIPAddress();
                            objdocumentFileDetails.HostName = CommonUtils.GetHostName();
                            objdocumentFileDetails.TabSequenceNo = TabSequenceNo;
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.glwb_hty);
                            objdocumentFileDetails.tablename = nameof(EnumLookup.tablename.documentdetails);
                            lstdocumentFileDetails.Add(objdocumentFileDetails);
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
                try
                {
                    ResponseMessage responseMessage = new ResponseMessage();
                    if (dtData.Rows.Count > 0)
                    {
                        responseMessage = await _iGLWBHomeTownYojanaservice.AddUpdateDocumentDetailsNew(dtData);
                    }
                    if (dtData.Rows.Count == 0 && responseMessage != null && responseMessage.Error == 0)
                    {
                        responseMessage.ApplicationNo = Convert.ToInt64(strApplicationId);
                        responseMessage.Id = Convert.ToInt64(strServiceId);
                    }

                    if (responseMessage != null)
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                        apiResponse.Result = responseMessage;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Get_DocumentDetails);
                        apiResponse.StackTrace = null;
                    }
                    else
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                        apiResponse.Result = null;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Not_Get_DocumentDetails);
                        apiResponse.StackTrace = null;
                    }
                }
                catch (Exception ex)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                    apiResponse.Message = ex.StackTrace;
                }

            }
            return Ok(apiResponse);
        }


        [HttpPost("finalsubmits")]
        public async Task<IActionResult> FinalSubmit([FromBody] FinalSubmitAPI finalSubmitAPI, string strApplicationId, string strServiceId, string strTabSequenceNo)
        {
            try
            {
                FinalSubmitModel finalSubmitModel = new FinalSubmitModel();
                finalSubmitModel = finalSubmitAPI.finalSubmitModel;
                finalSubmitModel.ApplicationId = Convert.ToInt64(strApplicationId);
                finalSubmitModel.serviceid = Convert.ToInt64(strServiceId);
                finalSubmitModel.tabsequenceno = Convert.ToInt64(strTabSequenceNo);
                finalSubmitModel.ipaddress = CommonUtils.GetLocalIPAddress();
                finalSubmitModel.hostname = CommonUtils.GetHostName();
                finalSubmitModel.ResigtrationId = Convert.ToInt32(finalSubmitAPI.userCoockiesModel.RegistrationId);
                finalSubmitModel.tablename = nameof(EnumLookup.tablename.personaldetails);
                finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.glwb_hty);
                finalSubmitModel.userid = Convert.ToInt64(finalSubmitAPI.userCoockiesModel.UserId);
                finalSubmitModel.benificiarytype = Convert.ToInt32(finalSubmitAPI.userCoockiesModel.beneficiarytype);
                finalSubmitModel.greensoldierreferralcode = "";

                var regResponse = await _iGLWBHomeTownYojanaservice.FinalSubmit(finalSubmitModel);

                if (regResponse != null)
                {
                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.ContentRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                    SMSModel modelSMSModel = await _iGLWBHomeTownYojanaservice.GetSmsContentForService(Convert.ToInt64(strServiceId), Convert.ToInt64(strApplicationId), (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.glwb_hty), nameof(EnumLookup.tablename.personaldetails));
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iGLWBHomeTownYojanaservice.AddSMSLogs(modelSMSModel.MobileNo, Convert.ToInt64(strServiceId), modelSMSModel.SmsContent, Convert.ToInt32(finalSubmitAPI.userCoockiesModel.RegistrationId));
                        }

                    }

                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = regResponse;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Send_FinalSubmit);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.FinalSubmit_Not_Send);
                    apiResponse.StackTrace = null;
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }
            return Ok(apiResponse);
        }
    }
}


