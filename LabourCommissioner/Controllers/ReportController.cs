using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using static LabourCommissioner.Abstraction.EnumLookup;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using Wkhtmltopdf.NetCore;
using HtmlAgilityPack;
using ClosedXML.Excel;
using LabourCommissioner.Views.Shared.Components.SearchBar;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LabourCommissioner.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportsService _iReportsService;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly UserCookies cookies;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly IGeneratePdf _generatePdf;
        private readonly bool _maximumPageSize;
        private readonly string _defaultPageSize;

        public ReportController(IWebHostEnvironment webHostEnvironment, IReportsService iReportsService, IConfiguration config, IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor, IGeneratePdf generatePdf)
        {
            _webHostEnvironment = webHostEnvironment;
            _iReportsService = iReportsService ?? throw new ArgumentNullException(nameof(_iReportsService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _defaultPageSize = _config["PageConfig:DefaultPageSize"];
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _generatePdf = generatePdf;
        }


        [HttpGet]
        public async Task<IActionResult> DownloadApplicationReport(string strApplicationId, string strServiceId)
        {

            #region Set RDLC
            string rdlcFileName = "";
            long ApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            long servicesId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));

            ServiceMaster objServiceMaster = await _iReportsService.GetSchemeByServiceIdgetscheme(servicesId);
            string schemaName = "";
            string tableName = "personaldetails";
            string ActionName = "";

            if (objServiceMaster != null)
            {
                ViewBag.ControllerName = objServiceMaster.ControllerName;
                ViewBag.ActionName = objServiceMaster.ActionName;
                schemaName = objServiceMaster.SchemaName;
                rdlcFileName = objServiceMaster.reportname;


            }
            ViewBag.servicesId = servicesId;
            string base64Image = "";
            DataTable dtPersonalDetailData = new DataTable();
            DataTable dtSchemeData = new DataTable();
            DataTable dmfamilynanajiData = new DataTable();
            DataTable dmotherschemedetailsData = new DataTable();
            DataTable dmfamilychildrenData = new DataTable();
            DataTable dmChildrenHostelData = new DataTable();
            DataTable dmfamilyData = new DataTable();
            DataTable dmfamilymemberssData = new DataTable();
            DataTable dmfamilymemberssTravelData = new DataTable();
            DataTable dmCompanyWorkersData = new DataTable();
            DataTable dmAgeCountData = new DataTable();
            DataTable dmCompanyWorkersDataclaim = new DataTable();
            DataTable dmAgeCountDataclaim = new DataTable();

            //personal details

            #region Personal Details
            DataSet ds = new DataSet();
            if (schemaName == "glwb_ssc")
            {
                List<GLWBSSC_PersonalDetailsModel> lstGLWB_SSCpersonalDetailsModels = new List<GLWBSSC_PersonalDetailsModel>();
                GLWBSSC_PersonalDetailsModel model = await _iReportsService.GetReportGLWB_SSC_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_SSCpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_SSCpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_SSCpersonalDetailsModels[0].CouchDBDocId, lstGLWB_SSCpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "glwb_hsc")
            {
                List<GLWBHSC_PersonalDetailsModel> lstGLWB_Hsc_personalDetailsModels = new List<GLWBHSC_PersonalDetailsModel>();
                GLWBHSC_PersonalDetailsModel model = await _iReportsService.GetReportGLWB_Hsc_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Hsc_personalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Hsc_personalDetailsModels);


                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Hsc_personalDetailsModels[0].CouchDBDocId, lstGLWB_Hsc_personalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }



            else if (schemaName == "glwb_psy")
            {
                List<GLWBPSY_PersonalDetailsModel> lstGLWB_Psy_personalDetailsModels = new List<GLWBPSY_PersonalDetailsModel>();
                GLWBPSY_PersonalDetailsModel model = await _iReportsService.GetReportGLWB_PSY_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Psy_personalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Psy_personalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Psy_personalDetailsModels[0].CouchDBDocId, lstGLWB_Psy_personalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            // add cycle and mahila 
            else if (schemaName == "glwb_csy")
            {
                List<GLWBCycle_personalDetails> lstGLWB_Psy_personalDetailsModels = new List<GLWBCycle_personalDetails>();
                GLWBCycle_personalDetails model = await _iReportsService.GetReportGLWB_CSY_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Psy_personalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Psy_personalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Psy_personalDetailsModels[0].CouchDBDocId, lstGLWB_Psy_personalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "glwb_asy")
            {
                List<GLWBASY_PersonalDetailsModel> lstGLWB_Psy_personalDetailsModels = new List<GLWBASY_PersonalDetailsModel>();
                GLWBASY_PersonalDetailsModel model = await _iReportsService.GetReportGLWB_aSY_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Psy_personalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Psy_personalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Psy_personalDetailsModels[0].CouchDBDocId, lstGLWB_Psy_personalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            //else if (schemaName == "glwb_tsy")
            //{
            //    List<GLWB_TSY_personalDetails> lstGLWB_Psy_personalDetailsModels = new List<GLWB_TSY_personalDetails>();
            //    GLWB_TSY_personalDetails model = await _iReportsService.GetReportGLWB_tsy_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            //    //model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
            //    //model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
            //    lstGLWB_Psy_personalDetailsModels.Add(model);
            //    dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Psy_personalDetailsModels);

            //    //CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Psy_personalDetailsModels[0].CouchDBDocId, lstGLWB_Psy_personalDetailsModels[0].FileName);
            //    //base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
            //    ds.Tables.Add(dtPersonalDetailData);
            //}
            else if (schemaName == "glwb_hty")
            {
                List<GLWBhty_PersonalDetailsModel> lstGLWB_Psy_personalDetailsModels = new List<GLWBhty_PersonalDetailsModel>();
                GLWBhty_PersonalDetailsModel model = await _iReportsService.GetReportGLWB_HTY_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Psy_personalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Psy_personalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Psy_personalDetailsModels[0].CouchDBDocId, lstGLWB_Psy_personalDetailsModels[0].FileName);
                //base64Image = null;
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "glwb_hty_claim")
            {
                List<GLWBhty_PersonalDetailsModel> lstGLWB_Psy_personalDetailsModels = new List<GLWBhty_PersonalDetailsModel>();
                GLWBhty_PersonalDetailsModel model = await _iReportsService.GetReportGLWB_HTY_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Psy_personalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Psy_personalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Psy_personalDetailsModels[0].CouchDBDocId, lstGLWB_Psy_personalDetailsModels[0].FileName);
                //base64Image = null;
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "glwb_msl")
            {
                List<GLWBMSL_Personaldetails> lstGLWB_Psy_personalDetailsModels = new List<GLWBMSL_Personaldetails>();
                GLWBMSL_Personaldetails model = await _iReportsService.GetReportGLWB_MSL_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Psy_personalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Psy_personalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Psy_personalDetailsModels[0].CouchDBDocId, lstGLWB_Psy_personalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "glwb_adsy")
            {
                List<GLWBADSY_PersonalDetailsModel> lstGLWB_Psy_personalDetailsModels = new List<GLWBADSY_PersonalDetailsModel>();
                GLWBADSY_PersonalDetailsModel model = await _iReportsService.GetReportGLWB_adsy_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Psy_personalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Psy_personalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Psy_personalDetailsModels[0].CouchDBDocId, lstGLWB_Psy_personalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "bocw_nanji")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            //BOCW common for all 

            else if (schemaName == "bocw_ssy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);

                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "bocw_acsy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);

                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "bocw_vr")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);

                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "bocw_hssy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);

                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "bocw_pip")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);

                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "bocw_tbsy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);

                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "bocw_tsy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "bocw_vcy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "bocw_psy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "bocw_bpsy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "bocw_asy")
            {
                List<PersonalDetailsModel> lstpersonalDetailsModels = new List<PersonalDetailsModel>();
                PersonalDetailsModel model = await _iReportsService.GetReportPersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);

                if (model.HasENirmanCard == true)
                {
                    model.uniqueidnumber = CommonUtils.MaskString(model.uniqueidnumber);
                }
                else
                {
                    model.ShramikAadharCardNo = CommonUtils.MaskString(model.ShramikAadharCardNo);
                }


                lstpersonalDetailsModels.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstpersonalDetailsModels);
                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstpersonalDetailsModels[0].CouchDBDocId, lstpersonalDetailsModels[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "glwb_hls")
            {
                List<GLWBHLS_PersonalDetailsModel> lstGLWBHLS_PersonalDetailsModel = new List<GLWBHLS_PersonalDetailsModel>();
                GLWBHLS_PersonalDetailsModel model = await _iReportsService.GetReportGLWB_HLS_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWBHLS_PersonalDetailsModel.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWBHLS_PersonalDetailsModel);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWBHLS_PersonalDetailsModel[0].CouchDBDocId, lstGLWBHLS_PersonalDetailsModel[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "glwb_hss")
            {
                List<GLWBHSS_PersonalDetails> lstGLWB_Hss_personalDetails = new List<GLWBHSS_PersonalDetails>();
                GLWBHSS_PersonalDetails model = await _iReportsService.GetReportGLWB_Hss_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Hss_personalDetails.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Hss_personalDetails);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Hss_personalDetails[0].CouchDBDocId, lstGLWB_Hss_personalDetails[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "glwb_lsy")
            {
                List<GLWBLSY_PersonalDetails> lstGLWB_Lsy_personalDetails = new List<GLWBLSY_PersonalDetails>();
                GLWBLSY_PersonalDetails model = await _iReportsService.GetReportGLWB_Lsy_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                model.AadharCardNo = CommonUtils.DecryptCRY(model.AadharCardNo);
                model.MaskedAadharCardNo = CommonUtils.MaskString(model.AadharCardNo);
                lstGLWB_Lsy_personalDetails.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Lsy_personalDetails);

                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetAttachmentByteArray(lstGLWB_Lsy_personalDetails[0].CouchDBDocId, lstGLWB_Lsy_personalDetails[0].FileName);
                base64Image = Convert.ToBase64String(objCouchDBResponse.ImageData.Result);
                ds.Tables.Add(dtPersonalDetailData);
            }
            else if (schemaName == "glwb_tsy")
            {
                List<GLWB_TSY_personalDetails> lstGLWB_Tsy_personalDetails = new List<GLWB_TSY_personalDetails>();
                GLWB_TSY_personalDetails model = await _iReportsService.GetReportGLWB_Tsy_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                lstGLWB_Tsy_personalDetails.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Tsy_personalDetails);
                ds.Tables.Add(dtPersonalDetailData);
            }

            else if (schemaName == "glwb_tsy_claim")
            {
                List<GLWB_TSYClaim_personalDetails> lstGLWB_Tsy_claim_personalDetails = new List<GLWB_TSYClaim_personalDetails>();
                GLWB_TSYClaim_personalDetails model = await _iReportsService.GetReportGLWB_Tsy_Claim_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
                lstGLWB_Tsy_claim_personalDetails.Add(model);
                dtPersonalDetailData = CommonUtils.ToDataTable(lstGLWB_Tsy_claim_personalDetails);
                ds.Tables.Add(dtPersonalDetailData);
            }



            #endregion

            #region Scheme Details



            //schemedetails start


            if (schemaName == "bocw_ssy")
            {

                List<SchemeDetails> lstschemeDetailsModels = new List<SchemeDetails>();
                SchemeDetails scheme = await _iReportsService.GetSchemeDetailsByAppId(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                lstschemeDetailsModels.Add(scheme);
                dtSchemeData = CommonUtils.ToDataTable(lstschemeDetailsModels);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "bocw_asy")
            {

                List<BOCWASYSchemeDetails> bocwasyscheme = new List<BOCWASYSchemeDetails>();
                BOCWASYSchemeDetails scheme7 = await _iReportsService.getrptbocwasyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwasyscheme.Add(scheme7);
                dtSchemeData = CommonUtils.ToDataTable(bocwasyscheme);
                ds.Tables.Add(dtSchemeData);
            }

            //manual added schemas names
            else if (schemaName == "bocw_tbsy")
            {
                List<BOCWTBSYSchemeDetails> bocwpsyscheme = new List<BOCWTBSYSchemeDetails>();
                BOCWTBSYSchemeDetails scheme2 = await _iReportsService.getrptbocwTbsyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpsyscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "bocw_acsy")
            {
                List<BOCWACSYSchemeDetails> bocwpsyscheme = new List<BOCWACSYSchemeDetails>();
                BOCWACSYSchemeDetails scheme2 = await _iReportsService.getrptbocwacsyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpsyscheme);
                ds.Tables.Add(dtSchemeData);
            }

            else if (schemaName == "bocw_vr")
            {
                List<BOCWVRSchemeDetails> bocwpsyscheme = new List<BOCWVRSchemeDetails>();
                BOCWVRSchemeDetails scheme2 = await _iReportsService.getrptbocwvrschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpsyscheme);
                ds.Tables.Add(dtSchemeData);

                List<BOCWVR_OtherSchemeDetails> otherschemeDetailsModel = new List<BOCWVR_OtherSchemeDetails>();
                otherschemeDetailsModel = await _iReportsService.getrpt_bocw_vr_otherschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.otherschemedetails));

                dmotherschemedetailsData = CommonUtils.ToDataTable(otherschemeDetailsModel);
                ds.Tables.Add(dmotherschemedetailsData);

            }
            else if (schemaName == "bocw_bpsy")
            {
                List<BOCWBPSYSchemeDetails> bocwpsyscheme = new List<BOCWBPSYSchemeDetails>();
                BOCWBPSYSchemeDetails scheme2 = await _iReportsService.getrptbocwbpsyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpsyscheme);
                ds.Tables.Add(dtSchemeData);



            }

            else if (schemaName == "bocw_hssy")
            {
                List<BOCWHssySchemeDetails> bocw_hssyscheme = new List<BOCWHssySchemeDetails>();
                BOCWHssySchemeDetails scheme1 = await _iReportsService.getrptbocw_hssy_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocw_hssyscheme.Add(scheme1);
                dtSchemeData = CommonUtils.ToDataTable(bocw_hssyscheme);
                ds.Tables.Add(dtSchemeData);


                List<familymember> lstfamilyDetailsModels = new List<familymember>();
                lstfamilyDetailsModels = await _iReportsService.getrptbocw_hssy_familyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.familydetails));

                dmfamilyData = CommonUtils.ToDataTable(lstfamilyDetailsModels);
                ds.Tables.Add(dmfamilyData);

                List<StudentMemberDetails> lstfamilyChildrenDetailsModels = new List<StudentMemberDetails>();
                lstfamilyChildrenDetailsModels = await _iReportsService.getrpt_bocw_hssy_childrenfamilyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.childrenfamilydetails));

                dmfamilychildrenData = CommonUtils.ToDataTable(lstfamilyChildrenDetailsModels);
                ds.Tables.Add(dmfamilychildrenData);

                List<StudentHostelMemberDetails> lstchildrenHostelDetailsModels = new List<StudentHostelMemberDetails>();
                lstchildrenHostelDetailsModels = await _iReportsService.getrpt_bocw_hssy_childrenhostelschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.childrenhosteldetails));

                dmChildrenHostelData = CommonUtils.ToDataTable(lstchildrenHostelDetailsModels);
                ds.Tables.Add(dmChildrenHostelData);


            }
            else if (schemaName == "bocw_nanji")
            {
                List<BOCWNanjiSchemeDetails> bocwnanajischeme = new List<BOCWNanjiSchemeDetails>();
                BOCWNanjiSchemeDetails scheme2 = await _iReportsService.getrpt_bocw_nanaji_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwnanajischeme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwnanajischeme);
                ds.Tables.Add(dtSchemeData);
                List<NanajiFamilyMemberDetails> lstfamilymemberModels = new List<NanajiFamilyMemberDetails>();
                lstfamilymemberModels = await _iReportsService.getrpt_bocw_nanaji_Familymembersdetailsschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.familydetails));
                dmfamilynanajiData = CommonUtils.ToDataTable(lstfamilymemberModels);
                ds.Tables.Add(dmfamilynanajiData);
            }
            else if (schemaName == "bocw_pip")
            {
                List<BOCWPIPSchemeDetails> bocwpipscheme = new List<BOCWPIPSchemeDetails>();
                BOCWPIPSchemeDetails scheme2 = await _iReportsService.getrptbocwpipschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpipscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpipscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "bocw_tsy")
            {
                List<BOCWTSYSchemeDetails> bocwtsyscheme = new List<BOCWTSYSchemeDetails>();
                BOCWTSYSchemeDetails scheme2 = await _iReportsService.getrptbocwtsyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwtsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwtsyscheme);
                ds.Tables.Add(dtSchemeData);
            }

            else if (schemaName == "bocw_vcy")
            {
                List<BOCWVCYSchemeDetails> bocwvcyscheme = new List<BOCWVCYSchemeDetails>();
                BOCWVCYSchemeDetails scheme2 = await _iReportsService.getrptbocwvcyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwvcyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwvcyscheme);
                ds.Tables.Add(dtSchemeData);
            }

            else if (schemaName == "bocw_psy")
            {
                List<BOCWPSYSchemeDetails> bocwbpsyscheme = new List<BOCWPSYSchemeDetails>();
                BOCWPSYSchemeDetails scheme3 = await _iReportsService.getrptbocwpsyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwbpsyscheme.Add(scheme3);
                dtSchemeData = CommonUtils.ToDataTable(bocwbpsyscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "glwb_hsc")
            {

                List<GLWBHSCSchemeDetails> glwbhscscheme = new List<GLWBHSCSchemeDetails>();
                GLWBHSCSchemeDetails scheme4 = await _iReportsService.getrptglwbhscschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbhscscheme.Add(scheme4);
                dtSchemeData = CommonUtils.ToDataTable(glwbhscscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "glwb_psy")
            {
                List<GLWBPSY_SchemeDetails> glwbpsyscheme = new List<GLWBPSY_SchemeDetails>();
                GLWBPSY_SchemeDetails scheme5 = await _iReportsService.getrptglwbpsyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbpsyscheme.Add(scheme5);
                dtSchemeData = CommonUtils.ToDataTable(glwbpsyscheme);
                ds.Tables.Add(dtSchemeData);
            }

            else if (schemaName == "glwb_ssc")
            {
                List<GLWBSSCSchemeDetails> glwbsscscheme = new List<GLWBSSCSchemeDetails>();
                GLWBSSCSchemeDetails scheme6 = await _iReportsService.getrptglwbsscschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbsscscheme.Add(scheme6);
                dtSchemeData = CommonUtils.ToDataTable(glwbsscscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "glwb_msl")
            {
                List<GLWBMSL_SchemeDetails> glwbsscscheme = new List<GLWBMSL_SchemeDetails>();
                GLWBMSL_SchemeDetails scheme6 = await _iReportsService.getrptglwb_msl_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbsscscheme.Add(scheme6);
                dtSchemeData = CommonUtils.ToDataTable(glwbsscscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "glwb_asy")
            {
                List<GLWBASYSchemeDetails> glwbsscscheme = new List<GLWBASYSchemeDetails>();
                GLWBASYSchemeDetails scheme6 = await _iReportsService.getrptglwb_asy_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbsscscheme.Add(scheme6);
                dtSchemeData = CommonUtils.ToDataTable(glwbsscscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "glwb_csy")
            {
                List<GLWBCYCLE_Schemedetails> glwbsscscheme = new List<GLWBCYCLE_Schemedetails>();
                GLWBCYCLE_Schemedetails scheme6 = await _iReportsService.getrptglwb_csy_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbsscscheme.Add(scheme6);
                dtSchemeData = CommonUtils.ToDataTable(glwbsscscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "glwb_hls")
            {

                List<GLWBHLS_SchemeDetails> glwbhlsscheme = new List<GLWBHLS_SchemeDetails>();
                GLWBHLS_SchemeDetails scheme4 = await _iReportsService.GetReportGLWB_HLS_SchemeDetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbhlsscheme.Add(scheme4);
                dtSchemeData = CommonUtils.ToDataTable(glwbhlsscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "glwb_adsy")
            {

                List<GLWBADSYSchemeDetails> glwbhlsscheme = new List<GLWBADSYSchemeDetails>();
                GLWBADSYSchemeDetails scheme4 = await _iReportsService.GetReportGLWB_adsy_SchemeDetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbhlsscheme.Add(scheme4);
                dtSchemeData = CommonUtils.ToDataTable(glwbhlsscheme);
                ds.Tables.Add(dtSchemeData);
            }
            //else if (schemaName == "glwb_tsy")
            //{

            //    List<GLWBTSYSchemeDetails> glwbhscscheme = new List<GLWBTSYSchemeDetails>();
            //    GLWBTSYSchemeDetails scheme4 = await _iReportsService.getrptglwb_tsy_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
            //    glwbhscscheme.Add(scheme4);
            //    dtSchemeData = CommonUtils.ToDataTable(glwbhscscheme);
            //    ds.Tables.Add(dtSchemeData);
            //}
            else if (schemaName == "glwb_hty")
            {
                List<GLWBhtySchemeDetails> bocwpsyscheme = new List<GLWBhtySchemeDetails>();
                GLWBhtySchemeDetails scheme2 = await _iReportsService.getrptglwb_hty_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpsyscheme);
                ds.Tables.Add(dtSchemeData);


                List<FamilyMemberDetails> lstfamilymemberssDetailsModels = new List<FamilyMemberDetails>();
                lstfamilymemberssDetailsModels = await _iReportsService.getrptbocw_hty_familymembersschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.familydetails));
                foreach (var item in lstfamilymemberssDetailsModels)
                {
                    item.aadharcard = CommonUtils.MaskString(item.aadharcard);
                }
                dmfamilymemberssData = CommonUtils.ToDataTable(lstfamilymemberssDetailsModels);
                ds.Tables.Add(dmfamilymemberssData);

            }

            else if (schemaName == "glwb_hty_claim")
            {
                List<GLWBhtySchemeDetails> bocwpsyscheme = new List<GLWBhtySchemeDetails>();
                GLWBhtySchemeDetails scheme2 = await _iReportsService.getrptglwb_hty_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpsyscheme);
                ds.Tables.Add(dtSchemeData);


                List<FamilyMemberTravelDetails> lstfamilymemberstravelDetailsModels = new List<FamilyMemberTravelDetails>();
                lstfamilymemberstravelDetailsModels = await _iReportsService.getrptbocw_hty_claim_familymembersschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.traveldetails));
                dmfamilymemberssTravelData = CommonUtils.ToDataTable(lstfamilymemberstravelDetailsModels);
                ds.Tables.Add(dmfamilymemberssTravelData);
            }

            else if (schemaName == "glwb_tsy")
            {
                List<GLWBTSYSchemeDetails> bocwpsyscheme = new List<GLWBTSYSchemeDetails>();
                GLWBTSYSchemeDetails scheme2 = await _iReportsService.getrptglwb_tsy_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpsyscheme);
                ds.Tables.Add(dtSchemeData);


                List<CompanyWorkerDetails> lstcompanyWorkerDetails = new List<CompanyWorkerDetails>();
                lstcompanyWorkerDetails = await _iReportsService.getrptglwb_tsy_companyWorkerDetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.companyworkerdetails));
                dmCompanyWorkersData = CommonUtils.ToDataTable(lstcompanyWorkerDetails);
                ds.Tables.Add(dmCompanyWorkersData);

                List<CompanyWorkerDetails> lstAgeCountDetails = new List<CompanyWorkerDetails>();
                lstAgeCountDetails = await _iReportsService.getrpt_glwb_tsy_getagecount(ApplicationId, schemaName, nameof(EnumLookup.tablename.companyworkerdetails));
                dmAgeCountData = CommonUtils.ToDataTable(lstAgeCountDetails);
                dmAgeCountData.TableName = "AgeCount";
                ds.Tables.Add(dmAgeCountData);
            }

            else if (schemaName == "glwb_hss")
            {

                List<GLWBHSS_SchemeDetails> glwbhssscheme = new List<GLWBHSS_SchemeDetails>();
                GLWBHSS_SchemeDetails scheme21 = await _iReportsService.getrptglwbhssschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwbhssscheme.Add(scheme21);
                dtSchemeData = CommonUtils.ToDataTable(glwbhssscheme);
                ds.Tables.Add(dtSchemeData);
            }
            else if (schemaName == "glwb_lsy")
            {

                List<GLWBLSY_SchemeDetails> glwblsyscheme = new List<GLWBLSY_SchemeDetails>();
                GLWBLSY_SchemeDetails scheme30 = await _iReportsService.getrptglwblsyschemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                glwblsyscheme.Add(scheme30);
                dtSchemeData = CommonUtils.ToDataTable(glwblsyscheme);
                ds.Tables.Add(dtSchemeData);
            }


            else if (schemaName == "glwb_tsy_claim")
            {
                List<GLWBTSYSchemeDetails> bocwpsyscheme = new List<GLWBTSYSchemeDetails>();
                GLWBTSYSchemeDetails scheme2 = await _iReportsService.getrptglwb_tsy_claim_schemedetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.schemedetails));
                bocwpsyscheme.Add(scheme2);
                dtSchemeData = CommonUtils.ToDataTable(bocwpsyscheme);
                ds.Tables.Add(dtSchemeData);


                List<CompanyWorkerDetails> lstcompanyWorkerDetails = new List<CompanyWorkerDetails>();
                lstcompanyWorkerDetails = await _iReportsService.getrptglwb_tsy_claim_companyWorkerDetails(ApplicationId, schemaName, nameof(EnumLookup.tablename.companyworkerdetails));
                dmCompanyWorkersDataclaim = CommonUtils.ToDataTable(lstcompanyWorkerDetails);
                ds.Tables.Add(dmCompanyWorkersDataclaim);

                List<CompanyWorkerDetails> lstAgeCountDetails = new List<CompanyWorkerDetails>();
                lstAgeCountDetails = await _iReportsService.getrpt_glwb_tsy_claim_getagecount(ApplicationId, schemaName, nameof(EnumLookup.tablename.companyworkerdetails));
                dmAgeCountDataclaim = CommonUtils.ToDataTable(lstAgeCountDetails);
                dmAgeCountDataclaim.TableName = "AgeCountClaim";
                ds.Tables.Add(dmAgeCountDataclaim);
            }
            #endregion

            #region Document Details
            List<DocumentFileDetails> lstdocumentDetailsModels = new List<DocumentFileDetails>();
            lstdocumentDetailsModels = await _iReportsService.GetdocumentDetailsByAppId(ApplicationId, schemaName, nameof(EnumLookup.tablename.documentdetails), servicesId);
            DataTable dmDocumentsData = new DataTable();
            dmDocumentsData = CommonUtils.ToDataTable(lstdocumentDetailsModels);
            ds.Tables.Add(dmDocumentsData);
            #endregion

            #endregion




            //DataSet ds = new DataSet();
            //ds.Tables.Add(dtPersonalDetailData);
            //ds.Tables.Add(dtSchemeData);
            //ds.Tables.Add(dmfamilynanajiData);
            //ds.Tables.Add(dmfamilyData);
            //ds.Tables.Add(dmfamilychildrenData);
            //ds.Tables.Add(dmChildrenHostelData); 
            //ds.Tables.Add(dmDocumentsData);

            string rootPath = $"{this._webHostEnvironment.WebRootPath}";
            //rdlcFileName = reportName;
            string reportName = "ApplicationDetails";

            //return File(CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, base64Image), "text/html", reportName + ".html");
            //if (servicesId == 7)
            //{
            //    return File(CommonUtils.TestGenerateReportExcel(ds, rootPath, "Test.rdlc", reportName, "PDF", true, base64Image), "application/pdf", reportName + ".pdf");
            //}
            //else
            //{
            //return File(CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, base64Image), "text/html", reportName + ".html");


            //_generatePdf.SetConvertOptions(options);


            //var elementsWithStyleAttribute = htmlDoc.DocumentNode.SelectNodes("//@style");
            //foreach (var element in elementsWithStyleAttribute)
            //{
            //    element.Attributes["style"].Remove();
            //}
            //htmlDoc.Save();
            try
            {
                byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, base64Image);

                string strHTML = System.Text.Encoding.UTF8.GetString(reportData);

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(strHTML);
                var pdf = _generatePdf.GetPDF(strHTML);
                var pdfStream = new System.IO.MemoryStream();
                pdfStream.Write(pdf, 0, pdf.Length);
                pdfStream.Position = 0;
                return new FileStreamResult(pdfStream, "application/pdf");
            }
            catch (Exception e)
            {
                throw e;
            }
            //System.IO.File.WriteAllBytes(Path.Combine(rootPath, "assets//css", reportName + ".pdf"), reportData);

            //return File(CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "PDF", true, base64Image), "application/pdf", reportName + ".pdf");

            //}
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> PendencyReport(string strServiceId, string fromDate = "", string toDate = "", int hodid = 0, string export = "")
        {


            long serviceId = 0;
            if (strServiceId == null || strServiceId == "")
            {
                serviceId = Convert.ToInt64(_claimPincipal.FindFirst("ServiceId").Value);
            }
            else
            {
                serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            }
            long districtId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
            long talukaId = 0;
            int beneficiaryType = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            int postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            var schemaName = Enum.GetName(typeof(schemaname), serviceId); ;

            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                if (now.Month < 3)
                {
                    var startDate = new DateTime((now.Year - 1), 4, 1);
                    var formatStartDate = String.Format("{0:dd/MM/yyyy}", startDate);
                    dtFromDate = DateTime.ParseExact(formatStartDate, "dd/MM/yyyy", null);
                }
                else
                {
                    var startDate = new DateTime(now.Year, 4, 1);
                    var formatStartDate = String.Format("{0:dd/MM/yyyy}", startDate);
                    dtFromDate = DateTime.ParseExact(formatStartDate, "dd/MM/yyyy", null);
                }
                //var startDate = new DateTime(now.Year, 4, 1);
                //var formatStartDate = String.Format("{0:dd/MM/yyyy}", startDate);
                //dtFromDate = DateTime.ParseExact(formatStartDate, "dd/MM/yyyy", null);
            }
            else
            {
                var finalDateTime = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo = String.Format("{0:dd/MM/yyyy}", finalDateTime);
                dtFromDate = DateTime.ParseExact(demo, "dd/MM/yyyy", null);
            }
            if (string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.Now;
                var formatEndDate = String.Format("{0:dd/MM/yyyy}", endDate);
                dtToDate = DateTime.ParseExact(formatEndDate, "dd/MM/yyyy", null);
            }
            else
            {
                var finalDateTime1 = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo1 = String.Format("{0:dd/MM/yyyy}", finalDateTime1);
                dtToDate = DateTime.ParseExact(demo1, "dd/MM/yyyy", null);
                //dtToDate = Convert.ToDateTime(toDate);
            }

            IEnumerable<PendencyReportDetails> modelList;
            if (postId == 37 && beneficiaryType == 3)
            {
                if (hodid == 4)
                {
                    beneficiaryType = 4;
                    postId = 38;
                    //hod = 1;

                }
                else
                {
                    beneficiaryType = 5;
                    postId = 39;
                }

            }



            modelList = await _iReportsService.GetRPTDistrictWisePendencyData(serviceId, dtFromDate, dtToDate, districtId, talukaId, beneficiaryType, schemaName);
            ServiceMaster objServiceMaster = await _iReportsService.GetSchemeByServiceIdgetscheme(serviceId);
            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
            ViewBag.ServiceId = Convert.ToString(serviceId);
            ViewBag.ServiceName = objServiceMaster.ServiceNameGujarati;
            ViewBag.Faicon = objServiceMaster.Faicon;
            ViewBag.Hodid = hodid;




            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();

                if (beneficiaryType == 1 || beneficiaryType == 4 || hodid == 4)
                {
                    dtExportData.Columns.Add("SrNo.", typeof(System.Int64));
                    dtExportData.Columns.Add("District Name", typeof(System.String));
                    dtExportData.Columns.Add("Total", typeof(System.Int64));
                    dtExportData.Columns.Add("Approved", typeof(System.Int64));
                    dtExportData.Columns.Add("Rejected", typeof(System.Int64));
                    dtExportData.Columns.Add("Send Back", typeof(System.Int64));
                    dtExportData.Columns.Add("Pending", typeof(System.Int64));
                    dtExportData.Columns.Add("Nirikshak", typeof(System.Int64));
                    dtExportData.Columns.Add("GLO BOCW Head Office", typeof(System.Int64));
                    dtExportData.Columns.Add("BOCW Member Secretory", typeof(System.Int64));

                }
                else
                {
                    dtExportData.Columns.Add("SrNo.", typeof(System.Int64));
                    dtExportData.Columns.Add("District Name", typeof(System.String));
                    dtExportData.Columns.Add("Total", typeof(System.Int64));
                    dtExportData.Columns.Add("Approved", typeof(System.Int64));
                    dtExportData.Columns.Add("Rejected", typeof(System.Int64));
                    dtExportData.Columns.Add("Send Back", typeof(System.Int64));
                    dtExportData.Columns.Add("Pending", typeof(System.Int64));
                    dtExportData.Columns.Add("Regional Officer", typeof(System.Int64));
                    dtExportData.Columns.Add("L.W.O", typeof(System.Int64));
                    dtExportData.Columns.Add("Welfare Commissioner", typeof(System.Int64));
                }

                IEnumerable<PendencyReportDetails> distinctDataList = null;
                if (modelList != null)
                {
                    distinctDataList = modelList.DistinctBy(o => o.districtid);
                }
                int index = 1;

                foreach (var item in distinctDataList)
                {
                    var row = dtExportData.NewRow();
                    row["SrNo."] = Convert.ToInt64(index);
                    row["District Name"] = Convert.ToString(item.districtname);
                    row["Total"] = Convert.ToInt64(item.totalapplication);
                    row["Approved"] = Convert.ToInt64(item.approved);
                    row["Rejected"] = Convert.ToInt64(item.rejected);
                    row["Send Back"] = Convert.ToInt64(item.sendback);
                    row["Pending"] = Convert.ToInt64(item.totalpending);

                    if (beneficiaryType == 1 || beneficiaryType == 4 || hodid == 4)
                    {
                        row["Nirikshak"] = Convert.ToInt64(modelList.Where(x => x.orderby == 10 && x.districtid == @item.districtid).Select(s => s.pending).FirstOrDefault());
                        row["GLO BOCW Head Office"] = Convert.ToInt64(modelList.Where(x => x.orderby == 20 && x.districtid == @item.districtid).Select(s => s.pending).FirstOrDefault());
                        row["BOCW Member Secretory"] = Convert.ToInt64(modelList.Where(x => x.orderby == 30 && x.districtid == @item.districtid).Select(s => s.pending).FirstOrDefault());
                    }
                    else
                    {
                        row["Regional Officer"] = Convert.ToInt64(modelList.Where(x => x.orderby == 50 && x.districtid == @item.districtid).Select(s => s.pending).FirstOrDefault());
                        row["L.W.O"] = Convert.ToInt64(modelList.Where(x => x.orderby == 60 && x.districtid == @item.districtid).Select(s => s.pending).FirstOrDefault());
                        row["Welfare Commissioner"] = Convert.ToInt64(modelList.Where(x => x.orderby == 70 && x.districtid == @item.districtid).Select(s => s.pending).FirstOrDefault());
                    }
                    dtExportData.Rows.Add(row);
                    index++;

                }

                long tottotalapplication = 0;
                long totapproved = 0;
                long totrejected = 0;
                long totsendback = 0;
                long tottotalpending = 0;
                long totNI = 0;
                long totGLO = 0;
                long totMS = 0;
                long totRO = 0;
                long totLWO = 0;
                long totWC = 0;


                tottotalapplication = distinctDataList.Sum(x => Convert.ToInt64(x.totalapplication));
                totapproved = distinctDataList.Sum(x => Convert.ToInt64(x.approved));
                totrejected = distinctDataList.Sum(x => Convert.ToInt64(x.rejected));
                totsendback = distinctDataList.Sum(x => Convert.ToInt64(x.sendback));
                tottotalpending = distinctDataList.Sum(x => Convert.ToInt64(x.totalpending));

                totNI = modelList.Where(x => x.orderby == 10).Sum(s => Convert.ToInt64(s.pending));
                totGLO = modelList.Where(x => x.orderby == 20).Sum(s => Convert.ToInt64(s.pending));
                totMS = modelList.Where(x => x.orderby == 30).Sum(s => Convert.ToInt64(s.pending));
                totRO = modelList.Where(x => x.orderby == 50).Sum(s => Convert.ToInt64(s.pending));
                totLWO = modelList.Where(x => x.orderby == 60).Sum(s => Convert.ToInt64(s.pending));
                totWC = modelList.Where(x => x.orderby == 70).Sum(s => Convert.ToInt64(s.pending));

                #region TOTAL Row
                if (dtExportData != null && dtExportData.Rows.Count > 0)
                {
                    var rowTotal = dtExportData.NewRow();
                    rowTotal[0] = DBNull.Value;
                    rowTotal[1] = "TOTAL";
                    rowTotal[2] = tottotalapplication;
                    rowTotal[3] = totapproved;
                    rowTotal[4] = totrejected;
                    rowTotal[5] = totsendback;
                    rowTotal[6] = tottotalpending;
                    if (beneficiaryType == 1 || beneficiaryType == 4 || hodid == 4)
                    {
                        rowTotal[7] = totNI;
                        rowTotal[8] = totGLO;
                        rowTotal[9] = totMS;
                    }
                    else
                    {
                        rowTotal[7] = totRO;
                        rowTotal[8] = totLWO;
                        rowTotal[9] = totWC;
                    }
                    dtExportData.Rows.Add(rowTotal);
                }

                #endregion


                string tableName = "PendencyReport";
                string fileName = "PendencyReport.csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }


            return View(modelList);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ApplicationDetailsPendencyReport(int? pageNo, int pageSize, string strServiceId, long districtId, string fromDate = "", string toDate = "", int status = 0, string export = "", int hodid = 0)
        {
            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);

            obj.AddUpdateClaim("ServiceId", CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            if (pageNo == null)
            {
                pageNo = 1;
            }
            if (export.ToLower() == "export")
            {
                pageSize = Convert.ToInt32(_maximumPageSize);
            }
            if (pageSize == 0)
            {
                pageSize = Convert.ToInt32(_defaultPageSize);
            }
            if (pageNo < 1) pageNo = 1;


            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            //long districtId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
            //long districtId = 0;
            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            long talukaId = 0;
            int beneficiaryType = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var schemaName = Enum.GetName(typeof(schemaname), serviceId); ;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, 4, 1);
                var formatStartDate = String.Format("{0:dd/MM/yyyy}", startDate);
                dtFromDate = DateTime.ParseExact(formatStartDate, "dd/MM/yyyy", null);
            }
            else
            {
                var finalDateTime = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo = String.Format("{0:dd/MM/yyyy}", finalDateTime);
                dtFromDate = DateTime.ParseExact(demo, "dd/MM/yyyy", null);
            }
            if (string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.Now;
                var formatEndDate = String.Format("{0:dd/MM/yyyy}", endDate);
                dtToDate = DateTime.ParseExact(formatEndDate, "dd/MM/yyyy", null);
            }
            else
            {
                var finalDateTime1 = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo1 = String.Format("{0:dd/MM/yyyy}", finalDateTime1);
                dtToDate = DateTime.ParseExact(demo1, "dd/MM/yyyy", null);
                //dtToDate = Convert.ToDateTime(toDate);
            }

            if (postId == 37 && beneficiaryType == 3)
            {
                if (hodid == 4)
                {
                    beneficiaryType = 4;
                    postId = 38;
                }
                else
                {
                    beneficiaryType = 5;
                    postId = 39;
                }
            }

            if (serviceId == 24 || serviceId == 34)
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;
                modelList = await _iReportsService.GetRPTBOCWApplicationDetailsListGLWB_TSY(districtId, dtFromDate, dtToDate, status, postId, beneficiaryType, schemaName);
                ServiceMaster objServiceMaster = await _iReportsService.GetSchemeByServiceIdgetscheme(serviceId);

                ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
                ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
                ViewBag.ServiceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
                ViewBag.DistrictId = districtId;
                ViewBag.Status = status;
                ViewBag.ServiceName = objServiceMaster.ServiceNameGujarati;
                ViewBag.Faicon = objServiceMaster.Faicon;



                // return export employee reference 
                int recsCount = modelList.Count();
                int recSkip = (int)((pageNo - 1) * pageSize);
                var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.PageSizes = GetPageSizes(pageSize);
                SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
                {
                    Action = "ApplicationDetailsPendencyReport",
                    Controller = "Report",
                    strServiceId = strServiceId,
                    EDistrictId = districtId,
                    ETalukaId = talukaId,
                    StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                    EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                                                                                  //BeneficiaryId = strbeneficiaryid

                };
                ViewBag.SearchPager = SearchPager;
                ViewBag.ControllerName = SearchPager.Controller;
                ViewBag.ActionName = SearchPager.Action;



                if (export.ToLower() == "export")
                {
                    DataTable dtExportData = new DataTable();
                    dtExportData.Columns.Add("SrNo.", typeof(System.String));
                    dtExportData.Columns.Add("Application No", typeof(System.String));
                    dtExportData.Columns.Add("Applicant Name", typeof(System.String));
                    dtExportData.Columns.Add("Father or Husband Name", typeof(System.String));
                    dtExportData.Columns.Add("DateOfBirth", typeof(System.DateTime));
                    dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                    dtExportData.Columns.Add("Phone No.", typeof(System.String));
                    dtExportData.Columns.Add("Email", typeof(System.String));
                    dtExportData.Columns.Add("Caste", typeof(System.String));
                    dtExportData.Columns.Add("Gender", typeof(System.String));
                    dtExportData.Columns.Add("Current Address", typeof(System.String));
                    dtExportData.Columns.Add("Current District", typeof(System.String));
                    dtExportData.Columns.Add("Current Taluka", typeof(System.String));
                    dtExportData.Columns.Add("Current Village", typeof(System.String));
                    dtExportData.Columns.Add("Current Pincode", typeof(System.String));
                    dtExportData.Columns.Add("Permanent Address", typeof(System.String));
                    dtExportData.Columns.Add("Permanent District", typeof(System.String));
                    dtExportData.Columns.Add("Permanent Taluka", typeof(System.String));
                    dtExportData.Columns.Add("Permanent Village", typeof(System.String));
                    dtExportData.Columns.Add("Permanent Pincode", typeof(System.String));
                    dtExportData.Columns.Add("Application Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Approved Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Application Status", typeof(System.String));
                    dtExportData.Columns.Add("Bank Name", typeof(System.String));
                    dtExportData.Columns.Add("Bank Branch", typeof(System.String));
                    dtExportData.Columns.Add("IFSC Code", typeof(System.String));
                    dtExportData.Columns.Add("Bank Account No.", typeof(System.String));




                    for (int i = 0; i < finalResult.Count; i++)
                    {
                        var row = dtExportData.NewRow();
                        row["SrNo."] = Convert.ToString(finalResult[i].srno);
                        row["Application No"] = Convert.ToString(finalResult[i].applicationno);
                        row["Applicant Name"] = Convert.ToString(finalResult[i].name);
                        row["Father or Husband Name"] = Convert.ToString(finalResult[i].fatherorhusbandname);
                        row["DateOfBirth"] = Convert.ToString(finalResult[i].dateofbirth);
                        row["Mobile No."] = Convert.ToString(finalResult[i].mobileno);
                        row["Phone No."] = Convert.ToString(finalResult[i].phoneno);
                        row["Email"] = Convert.ToString(finalResult[i].emailid);
                        row["Caste"] = Convert.ToString(finalResult[i].castename);
                        row["Gender"] = Convert.ToString(finalResult[i].gender);
                        row["Current Address"] = Convert.ToString(finalResult[i].caddressineng);
                        row["Current District"] = Convert.ToString(finalResult[i].cDistrictName);
                        row["Current Taluka"] = Convert.ToString(finalResult[i].ctalukaname);
                        row["Current Village"] = Convert.ToString(finalResult[i].cvillagename);
                        row["Current Pincode"] = Convert.ToString(finalResult[i].cpincode);
                        row["Permanent Address"] = Convert.ToString(finalResult[i].paddressineng);
                        row["Permanent District"] = Convert.ToString(finalResult[i].pDistrictName);
                        row["Permanent Taluka"] = Convert.ToString(finalResult[i].ptalukaname);
                        row["Permanent Village"] = Convert.ToString(finalResult[i].pvillagename);
                        row["Permanent Pincode"] = Convert.ToString(finalResult[i].ppincode);
                        row["Application Date"] = Convert.ToDateTime(finalResult[i].applicationdate).ToString("MM/dd/yyyy");

                        if (finalResult[i].approveddate != null && Convert.ToString(finalResult[i].approveddate != null) != "")
                            row["Approved Date"] = Convert.ToDateTime(finalResult[i].approveddate).ToString("MM/dd/yyyy");
                        else
                            row["Approved Date"] = DBNull.Value;

                        row["Application Status"] = Convert.ToString(finalResult[i].isapprovestatus);

                        row["Bank Name"] = Convert.ToString(finalResult[i].BankName);
                        row["Bank Branch"] = Convert.ToString(finalResult[i].BankBranch);
                        row["IFSC Code"] = Convert.ToString(finalResult[i].IFSCCode);
                        row["Bank Account No."] = Convert.ToString(finalResult[i].BankAccountNo);

                        dtExportData.Rows.Add(row);
                    }


                    string tableName = schemaName + "_ApplicationDetails";
                    string fileName = schemaName + "_ApplicationDetails.csv";


                    byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                    string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(result, contenType, fileName);
                }

                return View(finalResult);
            }
            else
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;
                modelList = await _iReportsService.GetRPTBOCWApplicationDetailsList(districtId, talukaId, dtFromDate, dtToDate, status, postId, beneficiaryType, schemaName);
                ServiceMaster objServiceMaster = await _iReportsService.GetSchemeByServiceIdgetscheme(serviceId);

                ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
                ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
                ViewBag.ServiceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
                ViewBag.DistrictId = districtId;
                ViewBag.Status = status;
                ViewBag.ServiceName = objServiceMaster.ServiceNameGujarati;
                ViewBag.Faicon = objServiceMaster.Faicon;



                // return export employee reference 
                int recsCount = modelList.Count();
                int recSkip = (int)((pageNo - 1) * pageSize);
                var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();

                ViewBag.PageSizes = GetPageSizes(pageSize);
                SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
                {
                    Action = "ApplicationDetailsPendencyReport",
                    Controller = "Report",
                    strServiceId = strServiceId,
                    EDistrictId = districtId,
                    ETalukaId = talukaId,
                    StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                    EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                                                                                  //BeneficiaryId = strbeneficiaryid

                };
                ViewBag.SearchPager = SearchPager;
                ViewBag.ControllerName = SearchPager.Controller;
                ViewBag.ActionName = SearchPager.Action;



                if (export.ToLower() == "export")
                {
                    DataTable dtExportData = new DataTable();
                    dtExportData.Columns.Add("SrNo.", typeof(System.String));
                    dtExportData.Columns.Add("Application No", typeof(System.String));
                    dtExportData.Columns.Add("Applicant Name", typeof(System.String));
                    dtExportData.Columns.Add("Father or Husband Name", typeof(System.String));
                    dtExportData.Columns.Add("DateOfBirth", typeof(System.DateTime));
                    dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                    dtExportData.Columns.Add("Phone No.", typeof(System.String));
                    dtExportData.Columns.Add("Email", typeof(System.String));
                    dtExportData.Columns.Add("Caste", typeof(System.String));
                    dtExportData.Columns.Add("Gender", typeof(System.String));
                    dtExportData.Columns.Add("Current Address", typeof(System.String));
                    dtExportData.Columns.Add("Current District", typeof(System.String));
                    dtExportData.Columns.Add("Current Taluka", typeof(System.String));
                    dtExportData.Columns.Add("Current Village", typeof(System.String));
                    dtExportData.Columns.Add("Current Pincode", typeof(System.String));
                    dtExportData.Columns.Add("Permanent Address", typeof(System.String));
                    dtExportData.Columns.Add("Permanent District", typeof(System.String));
                    dtExportData.Columns.Add("Permanent Taluka", typeof(System.String));
                    dtExportData.Columns.Add("Permanent Village", typeof(System.String));
                    dtExportData.Columns.Add("Permanent Pincode", typeof(System.String));
                    dtExportData.Columns.Add("Application Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Approved Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Application Status", typeof(System.String));
                    dtExportData.Columns.Add("Bank Name", typeof(System.String));
                    dtExportData.Columns.Add("Bank Branch", typeof(System.String));
                    dtExportData.Columns.Add("IFSC Code", typeof(System.String));
                    dtExportData.Columns.Add("Bank Account No.", typeof(System.String));




                    for (int i = 0; i < finalResult.Count; i++)
                    {
                        var row = dtExportData.NewRow();
                        row["SrNo."] = Convert.ToString(finalResult[i].srno);
                        row["Application No"] = Convert.ToString(finalResult[i].applicationno);
                        row["Applicant Name"] = Convert.ToString(finalResult[i].name);
                        row["Father or Husband Name"] = Convert.ToString(finalResult[i].fatherorhusbandname);
                        row["DateOfBirth"] = Convert.ToString(finalResult[i].dateofbirth);
                        row["Mobile No."] = Convert.ToString(finalResult[i].mobileno);
                        row["Phone No."] = Convert.ToString(finalResult[i].phoneno);
                        row["Email"] = Convert.ToString(finalResult[i].emailid);
                        row["Caste"] = Convert.ToString(finalResult[i].castename);
                        row["Gender"] = Convert.ToString(finalResult[i].gender);
                        row["Current Address"] = Convert.ToString(finalResult[i].caddressineng);
                        row["Current District"] = Convert.ToString(finalResult[i].cDistrictName);
                        row["Current Taluka"] = Convert.ToString(finalResult[i].ctalukaname);
                        row["Current Village"] = Convert.ToString(finalResult[i].cvillagename);
                        row["Current Pincode"] = Convert.ToString(finalResult[i].cpincode);
                        row["Permanent Address"] = Convert.ToString(finalResult[i].paddressineng);
                        row["Permanent District"] = Convert.ToString(finalResult[i].pDistrictName);
                        row["Permanent Taluka"] = Convert.ToString(finalResult[i].ptalukaname);
                        row["Permanent Village"] = Convert.ToString(finalResult[i].pvillagename);
                        row["Permanent Pincode"] = Convert.ToString(finalResult[i].ppincode);
                        row["Application Date"] = Convert.ToDateTime(finalResult[i].applicationdate).ToString("MM/dd/yyyy");

                        if (finalResult[i].approveddate != null && Convert.ToString(finalResult[i].approveddate != null) != "")
                            row["Approved Date"] = Convert.ToDateTime(finalResult[i].approveddate).ToString("MM/dd/yyyy");
                        else
                            row["Approved Date"] = DBNull.Value;

                        row["Application Status"] = Convert.ToString(finalResult[i].isapprovestatus);

                        row["Bank Name"] = Convert.ToString(finalResult[i].BankName);
                        row["Bank Branch"] = Convert.ToString(finalResult[i].BankBranch);
                        row["IFSC Code"] = Convert.ToString(finalResult[i].IFSCCode);
                        row["Bank Account No."] = Convert.ToString(finalResult[i].BankAccountNo);

                        dtExportData.Rows.Add(row);
                    }


                    string tableName = schemaName + "_ApplicationDetails";
                    string fileName = schemaName + "_ApplicationDetails.csv";


                    byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                    string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(result, contenType, fileName);
                }

                return View(finalResult);
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> DayWisePendencyReport(string strServiceId, string strapplicationid, int applicationstatus = 0)
        {
            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            long applicationid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strapplicationid)));
            long talukaId = 0;
            var schemaName = Enum.GetName(typeof(schemaname), serviceId); ;


            IEnumerable<EmpApplicationDetailsModel> modelList;
            modelList = await _iReportsService.GetRPTBOCWPendencyDaysList(serviceId, applicationid, applicationstatus, schemaName);
            ServiceMaster objServiceMaster = await _iReportsService.GetSchemeByServiceIdgetscheme(serviceId);


            ViewBag.ServiceId = strServiceId;
            ViewBag.ApplicationStatus = applicationstatus;
            ViewBag.ServiceName = objServiceMaster.ServiceNameGujarati;
            ViewBag.Faicon = objServiceMaster.Faicon;

            return View(modelList);
        }

        private List<SelectListItem> GetPageSizes(int selectedPageSize = 10)
        {
            var pageSizes = new List<SelectListItem>();
            if (selectedPageSize == 5)
                pageSizes.Add(new SelectListItem("5", "5", true));
            else
                pageSizes.Add(new SelectListItem("5", "5"));

            for (int lp = 10; lp <= 100; lp += 10)
            {
                if (lp == selectedPageSize)
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString(), true));
                else
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString()));
            }
            return pageSizes;
        }

    }
}
