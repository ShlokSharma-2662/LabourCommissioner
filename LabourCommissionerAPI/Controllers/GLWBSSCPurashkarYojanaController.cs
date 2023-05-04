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
    public class GLWBSSCPurashkarYojanaController : ControllerBase
    {
        private readonly IGLWBSSCPurashkarYojanaService _iGLWBSSCPurashkarYojanaService;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ApiResponse apiResponse = new ApiResponse();


        public GLWBSSCPurashkarYojanaController(IGLWBSSCPurashkarYojanaService iGLWBSSCPurashkarYojanaService,
            IConfiguration config, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iGLWBSSCPurashkarYojanaService = iGLWBSSCPurashkarYojanaService ?? throw new ArgumentNullException(nameof(_iGLWBSSCPurashkarYojanaService));
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
                var model = await _iGLWBSSCPurashkarYojanaService.GetTabSequenceByApplicationId(ApplicationId, id, nameof(EnumLookup.schemaname.glwb_ssc), nameof(EnumLookup.tablename.tabentry));

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
        public async Task<IActionResult> GLWBSSCPurashkarYojanaPersonalDetails(string ServiceId, string strApplicationId, bool isFilled, [FromBody]UserCoockiesModel userCoockiesModel)
        {
            try
            {
                var RegistrationNo = userCoockiesModel.UserName;

                var model = await _iGLWBSSCPurashkarYojanaService.GetPersonalDetailsByRegNo(RegistrationNo);
                model.ApplicationNo = userCoockiesModel.UserName;
                //UserCoockiesModel userCoockiesModel = new UserCoockiesModel();
                if (isFilled)
                {
                    long ApplicationId = Convert.ToInt64(strApplicationId);
                    var model1 = await _iGLWBSSCPurashkarYojanaService.GetApplicationDetailsByAppId(ApplicationId, nameof(EnumLookup.schemaname.glwb_ssc), nameof(EnumLookup.tablename.personaldetails));

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
        public async Task<IActionResult> AddUpdateApplication([FromForm] GLWBSSC_PersonalDetailsModel personalDetails, [FromForm] UserCoockiesModel userCoockiesModel, string strTabId)
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
                personalDetails.schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
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

                var regResponse = await _iGLWBSSCPurashkarYojanaService.AddUpdateApplication(personalDetails);
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
        public async Task<IActionResult> GLWBSSCPurashkarYojanaSchemeDetails(string ServiceId, string strApplicationId)
        {
            try
            {
                long ApplicationId = Convert.ToInt64(strApplicationId);
                var model1 = await _iGLWBSSCPurashkarYojanaService.GetApplicationSchemeDetailsByAppId(ApplicationId);
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
        public async Task<IActionResult> AddSchemeDetails([FromBody] GLWBSSCSchemeDetailsAPI glwbSSCSchemeDetailsAPI, int TabSequenceNo, int ApplicationId)
        {
            try
            {
                GLWBSSCSchemeDetails schemeDetails = new GLWBSSCSchemeDetails();
                schemeDetails = glwbSSCSchemeDetailsAPI.schemeDetails;
                schemeDetails.ApplicationId = ApplicationId;
                schemeDetails.TabSequenceNo = TabSequenceNo;
                schemeDetails.CreatedDate = DateTime.Now;
                schemeDetails.IpAddress = CommonUtils.GetLocalIPAddress();
                schemeDetails.HostName = CommonUtils.GetHostName();
                schemeDetails.CreatedBy = Convert.ToInt32(glwbSSCSchemeDetailsAPI.userCoockiesModel.RegistrationId);
                schemeDetails.schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
                schemeDetails.tablename = nameof(EnumLookup.tablename.schemedetails);
                var model = await _iGLWBSSCPurashkarYojanaService.AddSchemeDetails(schemeDetails);
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
        public async Task<IActionResult> GLWBSSCPurashkarYojanaDocument(string ServiceId, string strApplicationId, bool isFilled)
        {
            try
            {
                string ApplicationId = strApplicationId;
                var schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
                var tablename = nameof(EnumLookup.tablename.documentdetails);
                var model = await _iGLWBSSCPurashkarYojanaService.GetUploadedDocuments(Convert.ToInt64(ApplicationId), schemaname, tablename, Convert.ToInt64(ServiceId));
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
                            objdocumentFileDetails.schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
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
                        responseMessage = await _iGLWBSSCPurashkarYojanaService.AddUpdateDocumentDetailsNew(dtData);
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
        public async Task<IActionResult> FinalSubmit([FromBody]FinalSubmitAPI finalSubmitAPI, string strApplicationId, string strServiceId, string strTabSequenceNo)
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
                finalSubmitModel.schemaname = nameof(EnumLookup.schemaname.glwb_ssc);
                finalSubmitModel.userid = Convert.ToInt64(finalSubmitAPI.userCoockiesModel.UserId);
                finalSubmitModel.benificiarytype = Convert.ToInt32(finalSubmitAPI.userCoockiesModel.beneficiarytype);
                finalSubmitModel.greensoldierreferralcode = "";

                var regResponse = await _iGLWBSSCPurashkarYojanaService.FinalSubmit(finalSubmitModel);

                if (regResponse != null)
                {
                    CommonUtils commonFunction = new CommonUtils(_config);
                    string rootPath = $"{this._webHostEnvironment.ContentRootPath}";
                    var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(finalSubmitModel.benificiarytype));

                    SMSModel modelSMSModel = await _iGLWBSSCPurashkarYojanaService.GetSmsContentForService(Convert.ToInt64(strServiceId), Convert.ToInt64(strApplicationId), (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.glwb_ssc), nameof(EnumLookup.tablename.personaldetails));
                    if (modelSMSModel != null)
                    {
                        bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                        if (isSendSMS)
                        {
                            await _iGLWBSSCPurashkarYojanaService.AddSMSLogs(modelSMSModel.MobileNo, Convert.ToInt64(strServiceId), modelSMSModel.SmsContent, Convert.ToInt32(finalSubmitAPI.userCoockiesModel.RegistrationId));
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
            catch(Exception ex)
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


