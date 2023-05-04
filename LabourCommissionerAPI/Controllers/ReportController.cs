using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Common;
using LabourCommissionerAPI.ResponseModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LabourCommissioner.Common.Utility;
using Wkhtmltopdf.NetCore;
using System.Web;
using System.Data;
using LabourCommissioner.Abstraction.ViewDataModels;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using System.Windows.Forms;
using Microsoft.AspNetCore.Hosting;


namespace LabourCommissionerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReportController : ControllerBase
    {


        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IReportsService _iReportsService;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly UserCookies cookies;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly IGeneratePdf _generatePdf;
        //private readonly bool _maximumPageSize;
        private readonly string _defaultPageSize;

        ApiResponse apiResponse = new ApiResponse();

        public ReportController(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IReportsService iReportsService, IConfiguration config, IHttpClientFactory clientFactory,
            IGeneratePdf generatePdf)
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

        //[HttpGet("ReportDownload")]
        [HttpGet("{ReportDownload}")]
        public async Task<IActionResult> DownloadApplicationReport(string strApplicationId, string strServiceId)
        {
            string rdlcFileName = "";
            long ApplicationId = Convert.ToInt64(strApplicationId);
            long servicesId = Convert.ToInt64(strServiceId);

            ServiceMaster objServiceMaster = await _iReportsService.GetSchemeByServiceIdgetscheme(servicesId);
            string schemaName = "";
            string tableName = "personaldetails";
            string ActionName = "";
            string ControllerName = "";

            if (objServiceMaster != null)
            {
                ControllerName = objServiceMaster.ControllerName;
                ActionName = objServiceMaster.ActionName;
                schemaName = objServiceMaster.SchemaName;
                rdlcFileName = objServiceMaster.reportname;


            }
            servicesId = servicesId;
            string base64Image = "";
            DataTable dtPersonalDetailData = new DataTable();
            DataTable dtSchemeData = new DataTable();
            DataTable dmfamilynanajiData = new DataTable();
            DataTable dmotherschemedetailsData = new DataTable();
            DataTable dmfamilychildrenData = new DataTable();
            DataTable dmChildrenHostelData = new DataTable();
            DataTable dmfamilyData = new DataTable();
            DataTable dmfamilymemberssData = new DataTable();
            DataTable dmCompanyWorkersData = new DataTable();
            DataTable dmAgeCountData = new DataTable();

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
            #endregion

            #region Document Details
            List<DocumentFileDetails> lstdocumentDetailsModels = new List<DocumentFileDetails>();
            lstdocumentDetailsModels = await _iReportsService.GetdocumentDetailsByAppId(ApplicationId, schemaName, nameof(EnumLookup.tablename.documentdetails), servicesId);
            DataTable dmDocumentsData = new DataTable();
            dmDocumentsData = CommonUtils.ToDataTable(lstdocumentDetailsModels);
            ds.Tables.Add(dmDocumentsData);
            #endregion


            var slnPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString();

            var rootPath = Path.Combine(slnPath, "LabourCommissioner", "wwwroot");

            //string rootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            //var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            //string rootPath = Directory.GetParent(Directory.GetCurrentDirectory(), "wwwroot");
            // string rootPath = _webHostEnvironment.ContentRootPath;
            // string rootPath = $"{this._webHostEnvironment.ContentRootPath}";
            //string contantrootPath = $"{this._webHostEnvironment.ContentRootPath}";
            //otherLayer.SetRootPath(rootPath);
            //otherLayer.SetRootPath(rootPath);

            //string rootPath = $"{_webHostEnvironment.ContentRootPath}";
            //string baserootpath.SetRootPath(rootPath);

            // string rootPath = AppDomain.CurrentDomain.BaseDirectory; 
            //otherLayer.SetRootPath(rootPath);                                                      

            string reportName = "ApplicationDetails";

            try
            {
                byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, base64Image);

                string strHTML = System.Text.Encoding.UTF8.GetString(reportData);



                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(strHTML);

                var pdf = _generatePdf.GetPDF(strHTML);
                var pdfStream = new System.IO.MemoryStream();
                pdfStream.Write(pdf, 0, pdf.Length);
                pdfStream.Position = 0;
                var bytes = pdfStream.ToArray();
                var base64string = Convert.ToBase64String(bytes);
                //return new FileStreamResult(pdfStream, "application/pdf");

               

                if (base64string != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = base64string;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Report_Get_Success);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Report_No_Record_Found);
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
