using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.WebPages;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2016.Excel;
using Highsoft.Web.Mvc.Charts;
using HtmlAgilityPack;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using LabourCommissioner.CustomAuthorization;
using LabourCommissioner.Views.Shared.Components.SearchBar;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Localization;
using Wkhtmltopdf.NetCore;
using X.PagedList;
using static LabourCommissioner.Abstraction.EnumLookup;

namespace LabourCommissioner.Controllers
{
    public class EmployeeHomeController : Controller
    {
        private readonly IEmployeeHomeService _ihomeService;
        private readonly IEmployeeHomeRepository _homeRepository;
        private readonly ISchemeService _iscchemeService;
        private readonly ISchemeUserServices _schemeUserServices;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _isexceptionmailrequired;
        private readonly string _defaultPageSize;
        private readonly string _maximumPageSize;
        private readonly string _bocwEmailLogo;
        private readonly string _glwbEmailLogo;
        private readonly IGeneratePdf _generatePdf;
        public EmployeeHomeController(IStringLocalizer<HomeController> localizer, IConfiguration config, IHttpClientFactory clientFactory, IWebHostEnvironment webHostEnvironment, IHtmlLocalizer<HomeController> htmlLocalizer, IEmployeeHomeService homeService, IEmployeeHomeRepository homeRepository, ISchemeService schemeService, ISchemeUserServices schemeUserServices,
            IHttpContextAccessor httpContextAccessor, IGeneratePdf generatePdf)

        {
            _ihomeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
            _homeRepository = homeRepository;
            _iscchemeService = schemeService ?? throw new ArgumentNullException(nameof(schemeService));
            _schemeUserServices = schemeUserServices;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _localizer = localizer;
            _htmlLocalizer = htmlLocalizer;
            _config = config;
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _webHostEnvironment = webHostEnvironment;
            _isexceptionmailrequired = _config["SMTPConfig:_IsExceptionMailRequired"];
            _defaultPageSize = _config["PageConfig:DefaultPageSize"];
            _maximumPageSize = _config["PageConfig:MaximumPageSize"];
            _bocwEmailLogo = _config["EmailLogo:BOCW"];
            _glwbEmailLogo = _config["EmailLogo:GLWB"];
            _generatePdf = generatePdf;
        }


        public async Task<IActionResult> Index()
        {
            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);

            obj.AddUpdateClaim("ServiceId", "0");

            var usertype = Convert.ToInt32(_claimPincipal.FindFirst("UserType").Value);
            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var postid = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            var roleid = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);

            IEnumerable<ServiceMaster> model = await _ihomeService.BindServicesEmployeeWiseFilter(usertype, benificiarytypeid, postid, roleid);
            var endrole = model.ToList()[0].ApprovalEndRoleId;
            var levelno = await _ihomeService.GetLevelNoFromPostId(Convert.ToInt64(postid));
            ViewBag.LastLevel = false;
            if (levelno != null)
            {
                if (Convert.ToInt32(levelno) == endrole)
                {
                    ViewBag.LastLevel = true;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeDashboard(string strserviceId, long finYear = 0)
        {
            if (finYear == 0)
            {
                finYear = DateTime.Now.Year;
                long currentMonth = DateTime.Now.Month;
                if (currentMonth <= 3)
                {
                    finYear = finYear - 1;
                }
            }
            ViewBag.FinYear = finYear;


            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strserviceId)));
            var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(serviceId));
            long districtId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
            long talukaId = Convert.ToInt32(_claimPincipal.FindFirst("TalukaId").Value);
            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            ViewBag.serviceId = strserviceId;

            //List<SelectListItem> finYearList = new List<SelectListItem>();
            //long startYear = 2021;
            //long currYear = DateTime.Now.Year;
            //for (long year = startYear; year <= currYear; year++)
            //{
            //    finYearList.Add(new SelectListItem
            //    {
            //        Text = year.ToString() + " - " + (year + 1).ToString(),
            //        Value = year.ToString(),
            //        Selected = startYear == currYear ? true : false
            //    });
            //    startYear++;
            //}

            //ViewBag.FinYearList = finYearList;
            var finYearList = _ihomeService.GetFinancialYearList();
            ViewBag.FinYearList = finYearList.Result.ToList();


            IEnumerable<ApplicationCountForDashBoard> model = await _ihomeService.BindAppcountDistrictTalukaWiseForDashboard(finYear, districtId, postId, schemaName);
            if (serviceId == 24 || serviceId == 34)
            {
                IEnumerable<CountForDashBoard> countModel = await _ihomeService.BindCountForDashboardGLWB_tsy(finYear, districtId, postId, serviceId);
                List<ColumnSeriesData> totalCountData = new List<ColumnSeriesData>();
                List<ColumnSeriesData> pendingData = new List<ColumnSeriesData>();
                List<ColumnSeriesData> approvedData = new List<ColumnSeriesData>();
                List<ColumnSeriesData> rejectedData = new List<ColumnSeriesData>();
                List<string> districtData = new List<string>();


                if (model != null && model.Count() > 0)
                {
                    model.ToList().ForEach(d => totalCountData.Add(new ColumnSeriesData
                    {
                        Y = d.totalcount
                    }));
                    model.ToList().ForEach(d => pendingData.Add(new ColumnSeriesData
                    {
                        Y = d.pending
                    }));
                    model.ToList().ForEach(d => approvedData.Add(new ColumnSeriesData
                    {
                        Y = d.approved
                    }));
                    model.ToList().ForEach(d => rejectedData.Add(new ColumnSeriesData
                    {
                        Y = d.rejected
                    }));

                    model.ToList().ForEach(d => districtData.Add(d.districtname));
                }
                ViewData["TotalCountData"] = totalCountData;
                ViewData["PendingData"] = pendingData;
                ViewData["ApprovedData"] = approvedData;
                ViewData["RejectedData"] = rejectedData;
                ViewData["DistrictData"] = districtData;
                return View(countModel);
            }
            else
            {
                IEnumerable<CountForDashBoard> countModel1 = await _ihomeService.BindCountForDashboard(finYear, districtId, postId, talukaId, serviceId);
                List<ColumnSeriesData> totalCountData = new List<ColumnSeriesData>();
                List<ColumnSeriesData> pendingData = new List<ColumnSeriesData>();
                List<ColumnSeriesData> approvedData = new List<ColumnSeriesData>();
                List<ColumnSeriesData> rejectedData = new List<ColumnSeriesData>();
                List<string> districtData = new List<string>();


                if (model != null && model.Count() > 0)
                {
                    model.ToList().ForEach(d => totalCountData.Add(new ColumnSeriesData
                    {
                        Y = d.totalcount
                    }));
                    model.ToList().ForEach(d => pendingData.Add(new ColumnSeriesData
                    {
                        Y = d.pending
                    }));
                    model.ToList().ForEach(d => approvedData.Add(new ColumnSeriesData
                    {
                        Y = d.approved
                    }));
                    model.ToList().ForEach(d => rejectedData.Add(new ColumnSeriesData
                    {
                        Y = d.rejected
                    }));

                    model.ToList().ForEach(d => districtData.Add(d.districtname));
                }
                ViewData["TotalCountData"] = totalCountData;
                ViewData["PendingData"] = pendingData;
                ViewData["ApprovedData"] = approvedData;
                ViewData["RejectedData"] = rejectedData;
                ViewData["DistrictData"] = districtData;

                return View(countModel1);
            }

            //List<double> tokyoValues = new List<double> { 49.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4 };
            //List<double> nyValues = new List<double> { 83.6, 78.8, 98.5, 93.4, 106.0, 84.5, 105.0, 104.3, 91.2 };
            //List<double> berlinValues = new List<double> { 42.4, 33.2, 34.5, 39.7, 52.6, 75.5, 57.4, 60.4, 47.6 };
            //List<double> londonValues = new List<double> { 48.9, 38.8, 39.3, 41.4, 47.0, 48.3, 59.0, 59.6, 52.4 };

            //List<ColumnSeriesData> tokyoData = new List<ColumnSeriesData>();
            //List<ColumnSeriesData> nyData = new List<ColumnSeriesData>();
            //List<ColumnSeriesData> berlinData = new List<ColumnSeriesData>();
            //List<ColumnSeriesData> londonData = new List<ColumnSeriesData>();

            //tokyoValues.ForEach(p => tokyoData.Add(new ColumnSeriesData
            //{
            //    Y = p
            //}));
            //nyValues.ForEach(p => nyData.Add(new ColumnSeriesData
            //{
            //    Y = p
            //}));
            //berlinValues.ForEach(p => berlinData.Add(new ColumnSeriesData
            //{
            //    Y = p
            //}));
            //londonValues.ForEach(p => londonData.Add(new ColumnSeriesData
            //{
            //    Y = p
            //}));


        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AdminEmployeeDashboard(long finYear = 0, string beneficiarytype = "", string strServiceId = "")
        {

            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);
            obj.AddUpdateClaim("ServiceId", CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            if (finYear == 0)
            {
                finYear = DateTime.Now.Year;
                long currentMonth = DateTime.Now.Month;
                if (currentMonth <= 3)
                {
                    finYear = finYear - 1;
                }
            }
            ViewBag.FinYear = finYear;
            //long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strserviceId)));
            //var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(serviceId));
            long districtId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
            long talukaId = Convert.ToInt32(_claimPincipal.FindFirst("TalukaId").Value);
            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            long hodid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            //ViewBag.serviceId = strserviceId;

            //Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);

            //obj.AddUpdateClaim("PostId", "38");
            //obj.AddUpdateClaim("BeneficiaryType", "4");
            if (beneficiarytype == "4")
            {
                postId = 38;
                hodid = 4;
            }
            if (beneficiarytype == "5")
            {
                postId = 39;
                hodid = 5;
            }

            long upostId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            long uhodid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);


            //List<SelectListItem> finYearList = new List<SelectListItem>();
            //long startYear = 2021;
            //long currYear = DateTime.Now.Year;
            //for (long year = startYear; year <= currYear; year++)
            //{
            //    finYearList.Add(new SelectListItem
            //    {
            //        Text = year.ToString() + " - " + (year + 1).ToString(),
            //        Value = year.ToString(),
            //        Selected = startYear == currYear ? true : false
            //    });
            //    startYear++;
            //}
            var finYearList = _ihomeService.GetFinancialYearList();
            ViewBag.FinYearList = finYearList.Result.ToList();
            IEnumerable<CountForDashBoard> countModel = await _ihomeService.BindAdminDashBoardData(hodid, districtId, postId, talukaId, finYear);
            ViewBag.CountList = countModel.ToList();
            IEnumerable<CountForDashBoard> gridModel = await _ihomeService.BindAdminGridData(finYear, hodid, districtId, postId, talukaId);
            ViewBag.PostId = upostId;
            ViewBag.hodid = hodid;
            ViewBag.BeneficiaryType = beneficiarytype;
            return View(gridModel);
        }

        [HttpGet]
        //[TypeFilter(typeof(PermissionRequirementFilter))]
        public async Task<IActionResult> EmployeeApplicationDetails(int? pageNo, int pageSize, long districtId = 0, long talukaId = 0, long villageId = 0, string fromDate = "", string toDate = "", int statusId = 1, string? search = "", string strServiceId = "", string export = "", long divisionId = 0, int hodid = 0, int subServiceId = 0, int isbpsyservice = 0)
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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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

            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            if (postId == 37)
            {
                postId = 38;
            }

            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            ViewBag.hodid = hodid;
            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<EmpApplicationDetailsModel> modelList;

            long logindistrictId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
            string ApproveBtnName = "";

            if (districtId == 0)
            {
                if (logindistrictId == 34)
                {
                    districtId = postId == 35 ? 1 : 0;
                }
                else
                {
                    districtId = logindistrictId;
                }

            }
            modelList = await _ihomeService.BOCW_SSY_GetApplicationDetailsList(pageNo, pageSize, districtId, talukaId, villageId, dtFromDate, dtToDate, statusId, search, postId, serviceId, "", schemaName, subServiceId,isbpsyservice);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();

            ViewBag.PageSizes = GetPageSizes(pageSize);


            var villageModel = _ihomeService.GetVillageByDistrictIdAndTalukaId(Convert.ToInt32(districtId), Convert.ToInt32(talukaId));
            var villageList = villageModel.Result.ToList();


            var districtModel = _ihomeService.GetDistrict();
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;
            ViewBag.DistrictId = districtId;

            var talukaModel = _ihomeService.GetTalukaByDistrictId(Convert.ToInt32(districtId));
            var talukaList = talukaModel.Result.ToList();
            ViewBag.TalukaList = talukaList;


            var subserviceModel = _ihomeService.GetSubServiceByServiceId(Convert.ToInt32(serviceId));
            var subserviceList = subserviceModel.Result.ToList();
            ViewBag.SubServiceList = subserviceList;




            ViewBag.VillageList = villageList;
            ViewBag.VillageId = villageId;
            ViewBag.TalukaId = talukaId;
            ViewBag.ServiceId = serviceId;
            ViewBag.SubServiceId = subServiceId;
            ViewBag.IsbpsyService = isbpsyservice;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
            ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            ViewBag.ApproveBtnName = postId == 35 ? "Approve Applications & Generate Aadesh" : "Approve Applications";
            //ViewBag.ApproveBtnName = "Approve Applications";


            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "EmployeeApplicationDetails",
                Controller = "EmployeeHome",
                strServiceId = strServiceId,
                SearchText = search,
                EDistrictId = districtId,
                EVillageId = villageId,
                ETalukaId = talukaId,
                EDivisionId = divisionId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                //BeneficiaryId = strbeneficiaryid
                StatusId = statusId,
                SubServiceId = subServiceId,

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            //ViewBag.strbeneficiaryid = strbeneficiaryid;
            ViewBag.Status = statusId;


            if (export.ToLower() == "export")
            {

                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("SrNo.", typeof(System.String));
                dtExportData.Columns.Add("e-Nirman Card No.", typeof(System.String));
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
                dtExportData.Columns.Add("Remarks", typeof(System.String));
                dtExportData.Columns.Add("Bank Name", typeof(System.String));
                dtExportData.Columns.Add("Bank Branch", typeof(System.String));
                dtExportData.Columns.Add("IFSC Code", typeof(System.String));
                dtExportData.Columns.Add("Bank Account No.", typeof(System.String));
                dtExportData.Columns.Add("Account Holder Name.", typeof(System.String));
                if (serviceId == 7)
                {
                    dtExportData.Columns.Add("Degree", typeof(System.String));
                    dtExportData.Columns.Add("Course", typeof(System.String));
                    dtExportData.Columns.Add("School Benifits", typeof(System.Decimal));
                    dtExportData.Columns.Add("Hostel Benifits", typeof(System.Decimal));
                    dtExportData.Columns.Add("Book Benifits", typeof(System.Decimal));
                    dtExportData.Columns.Add("Total Sahay", typeof(System.Decimal));
                }
                else if (serviceId == 3)
                {
                    dtExportData.Columns.Add("Applied Before", typeof(System.String));
                    dtExportData.Columns.Add("PsDateofBirth", typeof(System.String));
                    dtExportData.Columns.Add("Child Gender", typeof(System.String));
                    dtExportData.Columns.Add("Total Sahay", typeof(System.Decimal));
                    dtExportData.Columns.Add("Duration Between Application Subbmitted Date & DeliveryDate", typeof(System.Int32));
                }
                else if (serviceId == 2)
                {
                    dtExportData.Columns.Add("Duration Between Application Subbmitted Date & ExpDeliveryDate", typeof(System.Int32));
                    dtExportData.Columns.Add("Total Sahay", typeof(System.Decimal));
                }
                else if (serviceId != 16)
                {
                    dtExportData.Columns.Add("Total Sahay", typeof(System.Decimal));
                    //dtExportData.Columns.Add("Total Sahay", typeof(System.Decimal));
                }





                for (int i = 0; i < finalResult.Count; i++)
                {
                    var dob = String.Format("{0:MM/dd/yyyy}", finalResult[i].dateofbirth);
                    var row = dtExportData.NewRow();
                    row["SrNo."] = Convert.ToString(finalResult[i].srno);
                    row["e-Nirman Card No."] = Convert.ToString(finalResult[i].enirmancardno);
                    row["Application No"] = Convert.ToString(finalResult[i].applicationno);
                    row["Applicant Name"] = Convert.ToString(finalResult[i].name);
                    row["Father or Husband Name"] = Convert.ToString(finalResult[i].fatherorhusbandname);
                    //row["DateOfBirth"] = Convert.ToString(finalResult[i].dateofbirth);
                    row["DateOfBirth"] = dob;
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
                    row["Remarks"] = Convert.ToString(finalResult[i].approvalremarks);
                    row["Bank Name"] = Convert.ToString(finalResult[i].BankName);
                    row["Bank Branch"] = Convert.ToString(finalResult[i].BankBranch);
                    row["IFSC Code"] = Convert.ToString(finalResult[i].IFSCCode);
                    row["Bank Account No."] = Convert.ToString(finalResult[i].BankAccountNo);
                    row["Account Holder Name."] = Convert.ToString(finalResult[i].AccountHolderName);

                    if (serviceId == 7)
                    {
                        row["Degree"] = Convert.ToString(finalResult[i].strdegree);
                        row["Course"] = Convert.ToString(finalResult[i].course);
                        row["School Benifits"] = Convert.ToDecimal(finalResult[i].schoolbenifits);
                        row["Hostel Benifits"] = Convert.ToDecimal(finalResult[i].hostelbenifits);
                        row["Book Benifits"] = Convert.ToDecimal(finalResult[i].booksbenifits);
                        row["Total Sahay"] = Convert.ToDecimal(finalResult[i].totalsahay);
                    }
                    else if (serviceId == 3)
                    {
                        row["Applied Before"] = Convert.ToString(finalResult[i].stralreadyapplied);
                        row["PsDateofBirth"] = Convert.ToDateTime(finalResult[i].psdateofbirth).ToString("dd-MM-yyyy");
                        row["Child Gender"] = Convert.ToString(finalResult[i].childgender);
                        row["Total Sahay"] = Convert.ToDecimal(finalResult[i].totalsahay);
                        row["Duration Between Application Subbmitted Date & DeliveryDate"] = Convert.ToDecimal(finalResult[i].diffdays);
                    }
                    else if (serviceId == 2)
                    {
                        row["Duration Between Application Subbmitted Date & ExpDeliveryDate"] = Convert.ToDecimal(finalResult[i].diffdays);
                        row["Total Sahay"] = Convert.ToDecimal(finalResult[i].totalsahay);
                    }
                    else if (serviceId != 16)
                    {
                        row["Total Sahay"] = Convert.ToDecimal(finalResult[i].totalsahay);
                    }

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

        [HttpGet]
        public async Task<IActionResult> GLWBEmployeeApplicationDetails(int? pageNo, int pageSize, long districtId = 0, long talukaId = 0, long villageId = 0, string fromDate = "", string toDate = "", int statusId = 1, string? search = "", string strServiceId = "", string export = "", long divisionId = 0, int hodid = 0)
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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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
            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            ViewBag.hodid = hodid;
            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<EmpApplicationDetailsModel> modelList;
            divisionId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);



            if (divisionId == 34)
            {
                divisionId = 0;
            }
            else
            {
                if (divisionId == 0)
                {
                    divisionId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
                }
                else
                {
                    divisionId = divisionId;
                }

            }

            modelList = await _ihomeService.GLWB_GetApplicationDetailsList(pageNo, pageSize, divisionId, talukaId, districtId, dtFromDate, dtToDate, statusId, search, postId, serviceId, "", schemaName);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);
            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.PageSizes = GetPageSizes(pageSize);


            var villageModel = _ihomeService.GetVillageByDistrictIdAndTalukaId(Convert.ToInt32(districtId), Convert.ToInt32(talukaId));
            var villageList = villageModel.Result.ToList();


            var districtModel = _ihomeService.GetDistrictNamebyDivisionId(Convert.ToInt32(divisionId));
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;
            ViewBag.DistrictId = districtId;

            var divisionModel = _ihomeService.GetDistrict();
            var divisionList = divisionModel.Result.ToList();
            ViewBag.DivisionList = divisionList.ToList();
            ViewBag.DivisionId = divisionId;

            var talukaModel = _ihomeService.GetTalukaByDistrictId(Convert.ToInt32(districtId));
            var talukaList = talukaModel.Result.ToList();
            ViewBag.TalukaList = talukaList;




            ViewBag.VillageList = villageList;
            ViewBag.VillageId = villageId;
            ViewBag.TalukaId = talukaId;
            ViewBag.ServiceId = serviceId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
            ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            ViewBag.ApproveBtnName = postId == 44 ? "Approve Applications & Generate Aadesh" : "Approve Applications";
            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "GLWBEmployeeApplicationDetails",
                Controller = "EmployeeHome",
                strServiceId = strServiceId,
                SearchText = search,
                EDistrictId = districtId,
                EVillageId = villageId,
                ETalukaId = talukaId,
                EDivisionId = divisionId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                StatusId = statusId,
                //BeneficiaryId = strbeneficiaryid

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            //ViewBag.strbeneficiaryid = strbeneficiaryid;
            ViewBag.Status = statusId;
            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("SrNo.", typeof(System.String));
                dtExportData.Columns.Add("LWB Account No.", typeof(System.String));
                dtExportData.Columns.Add("Organization Name", typeof(System.String));
                dtExportData.Columns.Add("Application No", typeof(System.String));
                dtExportData.Columns.Add("Applicant Name", typeof(System.String));
                dtExportData.Columns.Add("Father or Husband Name", typeof(System.String));
                dtExportData.Columns.Add("DateOfBirth", typeof(System.DateTime));
                dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                dtExportData.Columns.Add("Phone No.", typeof(System.String));
                dtExportData.Columns.Add("Email", typeof(System.String));
                dtExportData.Columns.Add("Gender", typeof(System.String));
                dtExportData.Columns.Add("Current Address", typeof(System.String));
                dtExportData.Columns.Add("Current District", typeof(System.String));
                dtExportData.Columns.Add("Current Taluka", typeof(System.String));
                dtExportData.Columns.Add("Current Village", typeof(System.String));
                dtExportData.Columns.Add("Current Pincode", typeof(System.String));
                dtExportData.Columns.Add("Application Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Approved Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Application Status", typeof(System.String));
                dtExportData.Columns.Add("Bank Name", typeof(System.String));
                dtExportData.Columns.Add("Bank Branch", typeof(System.String));
                dtExportData.Columns.Add("IFSC Code", typeof(System.String));
                dtExportData.Columns.Add("Bank Account No.", typeof(System.String));
                if (serviceId != 16)
                {
                    dtExportData.Columns.Add("Total Sahay", typeof(System.Decimal));
                }
                if (serviceId == 23)
                {
                    dtExportData.Columns.Add("Child Date of Birth", typeof(System.String));
                    dtExportData.Columns.Add("No of Male Child", typeof(System.Int32));
                    dtExportData.Columns.Add("No of Female Child", typeof(System.Int32));
                }
                if (serviceId == 14)
                {
                    dtExportData.Columns.Add("Bill Date", typeof(System.String));
                    dtExportData.Columns.Add("Bill No", typeof(System.String));
                }
                if (serviceId == 22)
                {
                    dtExportData.Columns.Add("Marriage Date", typeof(System.String));
                }

                for (int i = 0; i < finalResult.Count; i++)
                {
                    var dob = String.Format("{0:MM/dd/yyyy}", finalResult[i].dateofbirth);
                    var row = dtExportData.NewRow();
                    row["SrNo."] = Convert.ToString(finalResult[i].srno);
                    row["LWB Account No."] = Convert.ToString(finalResult[i].lwbaccountno);
                    row["Organization Name"] = Convert.ToString(finalResult[i].organizationname);
                    row["Application No"] = Convert.ToString(finalResult[i].applicationno);
                    row["Applicant Name"] = Convert.ToString(finalResult[i].name);
                    row["Father or Husband Name"] = Convert.ToString(finalResult[i].fatherorhusbandname);
                    row["DateOfBirth"] = dob;
                    row["Mobile No."] = Convert.ToString(finalResult[i].mobileno);
                    row["Phone No."] = Convert.ToString(finalResult[i].phoneno);
                    row["Email"] = Convert.ToString(finalResult[i].emailid);
                    row["Gender"] = Convert.ToString(finalResult[i].gender);
                    row["Current Address"] = Convert.ToString(finalResult[i].caddressineng);
                    row["Current District"] = Convert.ToString(finalResult[i].cDistrictName);
                    row["Current Taluka"] = Convert.ToString(finalResult[i].ctalukaname);
                    row["Current Village"] = Convert.ToString(finalResult[i].cvillagename);
                    row["Current Pincode"] = Convert.ToString(finalResult[i].cpincode);
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
                    if (serviceId != 16)
                    {
                        row["Total Sahay"] = Convert.ToString(finalResult[i].totalsahay);
                    }
                    if (serviceId == 23)
                    {
                        row["Child Date of Birth"] = Convert.ToString(finalResult[i].childdateofbirth);
                        row["No of Male Child"] = Convert.ToString(finalResult[i].noofchildemale);
                        row["No of Female Child"] = Convert.ToString(finalResult[i].noofchildfemale);
                    }
                    if (serviceId == 14)
                    {
                        row["Bill Date"] = Convert.ToString(finalResult[i].billdate);
                        row["Bill No"] = Convert.ToString(finalResult[i].billno);
                    }
                    if (serviceId == 22)
                    {
                        row["Marriage Date"] = Convert.ToString(finalResult[i].marriagedate);
                    }
                    dtExportData.Rows.Add(row);
                }

                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    dtExportData.TableName = schemaName + "_ApplicationDetails";
                //    var ws = wb.Worksheets.Add(dtExportData);

                //    ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                //    //START : set borders to each used cell in excel
                //    ws.CellsUsed().Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.BottomBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.TopBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.LeftBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.LeftBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.RightBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.RightBorderColor = ClosedXML.Excel.XLColor.Black;
                //    //END : set borders to each used cell in excel

                //    ws.Columns().AdjustToContents();

                //    using (MemoryStream stream = new MemoryStream())
                //    {
                //        wb.SaveAs(stream);
                //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", schemaName + "_ApplicationDetails" + ".xlsx");
                //    }
                //}

                string tableName = schemaName + "_ApplicationDetails";
                string fileName = schemaName + "_ApplicationDetails.csv";

                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);


            }


            return View(finalResult);
        }


        [HttpGet]
        public async Task<IActionResult> GLWBHospitalApplicationDetails(int? pageNo, int pageSize, string fromDate = "", string toDate = "", int statusId = 1, string? search = "", string strServiceId = "", string export = "", long divisionId = 0, int hodid = 0)
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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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
            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            ViewBag.hodid = hodid;
            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<EmpApplicationDetailsModel> modelList;
            divisionId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);



            if (divisionId == 34)
            {
                divisionId = 0;
            }
            else
            {
                if (divisionId == 0)
                {
                    divisionId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
                }
                else
                {
                    divisionId = divisionId;
                }

            }

            modelList = await _ihomeService.GLWB_Hospital_GetApplicationDetailsList(pageNo, pageSize, divisionId,
                dtFromDate, dtToDate, statusId, search, postId, serviceId, "", schemaName);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);
            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.PageSizes = GetPageSizes(pageSize);


            //var villageModel = _ihomeService.GetVillageByDistrictIdAndTalukaId(Convert.ToInt32(districtId), Convert.ToInt32(talukaId));
            //var villageList = villageModel.Result.ToList();


            //var districtModel = _ihomeService.GetDistrictNamebyDivisionId(Convert.ToInt32(divisionId));
            //var districtList = districtModel.Result.ToList();
            //ViewBag.DistrictList = districtList;
            //ViewBag.DistrictId = districtId;

            var divisionModel = _ihomeService.GetDistrict();
            var divisionList = divisionModel.Result.ToList();
            ViewBag.DivisionList = divisionList.ToList();
            ViewBag.DivisionId = divisionId;

            //var talukaModel = _ihomeService.GetTalukaByDistrictId(Convert.ToInt32(districtId));
            //var talukaList = talukaModel.Result.ToList();
            //ViewBag.TalukaList = talukaList;




            //ViewBag.VillageList = villageList;
            //ViewBag.VillageId = villageId;
            //ViewBag.TalukaId = talukaId;
            ViewBag.ServiceId = serviceId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
            ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            ViewBag.ApproveBtnName = postId == 44 ? "Approve Applications & Generate Aadesh" : "Approve Applications";
            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "GLWBHospitalApplicationDetails",
                Controller = "EmployeeHome",
                strServiceId = strServiceId,
                SearchText = search,
                //EDistrictId = districtId,
                //EVillageId = villageId,
                //ETalukaId = talukaId,
                EDivisionId = divisionId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                StatusId = statusId,
                //BeneficiaryId = strbeneficiaryid

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            //ViewBag.strbeneficiaryid = strbeneficiaryid;
            ViewBag.Status = statusId;
            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("SrNo.", typeof(System.String));
                dtExportData.Columns.Add("Application No", typeof(System.String));
                dtExportData.Columns.Add("Company Name", typeof(System.String));
                dtExportData.Columns.Add("Total Employees for Chekup", typeof(System.String));
                dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                dtExportData.Columns.Add("Email", typeof(System.String));
                dtExportData.Columns.Add("Address", typeof(System.String));
                dtExportData.Columns.Add("City", typeof(System.String));
                dtExportData.Columns.Add("District", typeof(System.String));
                dtExportData.Columns.Add("Taluka", typeof(System.String));
                dtExportData.Columns.Add("Pincode", typeof(System.String));
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
                    row["Company Name"] = Convert.ToString(finalResult[i].companyname);
                    row["Total Employees for Chekup"] = Convert.ToString(finalResult[i].totalemployeesforcheckup);
                    row["Mobile No."] = Convert.ToString(finalResult[i].hrManagerMobile);
                    row["Email"] = Convert.ToString(finalResult[i].hrmanageremail);
                    row["Address"] = Convert.ToString(finalResult[i].address);
                    row["City"] = Convert.ToString(finalResult[i].city);
                    row["District"] = Convert.ToString(finalResult[i].district);
                    row["Taluka"] = Convert.ToString(finalResult[i].taluka);
                    row["Pincode"] = Convert.ToString(finalResult[i].pincode);
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

                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    dtExportData.TableName = schemaName + "_ApplicationDetails";
                //    var ws = wb.Worksheets.Add(dtExportData);

                //    ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                //    //START : set borders to each used cell in excel
                //    ws.CellsUsed().Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.BottomBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.TopBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.LeftBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.LeftBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.RightBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.RightBorderColor = ClosedXML.Excel.XLColor.Black;
                //    //END : set borders to each used cell in excel

                //    ws.Columns().AdjustToContents();

                //    using (MemoryStream stream = new MemoryStream())
                //    {
                //        wb.SaveAs(stream);
                //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", schemaName + "_ApplicationDetails" + ".xlsx");
                //    }
                //}

                string tableName = schemaName + "_ApplicationDetails";
                string fileName = schemaName + "_ApplicationDetails.csv";

                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);


            }


            return View(finalResult);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBHospitalApplicationDetailsClaim(int? pageNo, int pageSize, string fromDate = "", string toDate = "", int statusId = 1, string? search = "", string strServiceId = "", string export = "", long divisionId = 0, int hodid = 0)
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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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
            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            ViewBag.hodid = hodid;
            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<EmpApplicationDetailsModel> modelList;
            divisionId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);



            if (divisionId == 34)
            {
                divisionId = 0;
            }
            else
            {
                if (divisionId == 0)
                {
                    divisionId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
                }
                else
                {
                    divisionId = divisionId;
                }

            }

            modelList = await _ihomeService.GLWBHospitalApplicationDetailsClaim(pageNo, pageSize, divisionId,
                dtFromDate, dtToDate, statusId, search, postId, serviceId, "", schemaName);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);
            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();
            ViewBag.PageSizes = GetPageSizes(pageSize);


            //var villageModel = _ihomeService.GetVillageByDistrictIdAndTalukaId(Convert.ToInt32(districtId), Convert.ToInt32(talukaId));
            //var villageList = villageModel.Result.ToList();


            //var districtModel = _ihomeService.GetDistrictNamebyDivisionId(Convert.ToInt32(divisionId));
            //var districtList = districtModel.Result.ToList();
            //ViewBag.DistrictList = districtList;
            //ViewBag.DistrictId = districtId;

            var divisionModel = _ihomeService.GetDistrict();
            var divisionList = divisionModel.Result.ToList();
            ViewBag.DivisionList = divisionList.ToList();
            ViewBag.DivisionId = divisionId;

            //var talukaModel = _ihomeService.GetTalukaByDistrictId(Convert.ToInt32(districtId));
            //var talukaList = talukaModel.Result.ToList();
            //ViewBag.TalukaList = talukaList;




            //ViewBag.VillageList = villageList;
            //ViewBag.VillageId = villageId;
            //ViewBag.TalukaId = talukaId;
            ViewBag.ServiceId = serviceId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
            ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            ViewBag.ApproveBtnName = postId == 44 ? "Approve Applications & Generate Aadesh" : "Approve Applications";
            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "GLWBHospitalApplicationDetailsClaim",
                Controller = "EmployeeHome",
                strServiceId = strServiceId,
                SearchText = search,
                //EDistrictId = districtId,
                //EVillageId = villageId,
                //ETalukaId = talukaId,
                EDivisionId = divisionId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                StatusId = statusId,
                //BeneficiaryId = strbeneficiaryid

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            //ViewBag.strbeneficiaryid = strbeneficiaryid;
            ViewBag.Status = statusId;
            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("SrNo.", typeof(System.String));
                dtExportData.Columns.Add("Application No", typeof(System.String));
                dtExportData.Columns.Add("Hospital Name", typeof(System.String));
                dtExportData.Columns.Add("Hospital Address", typeof(System.String));
                dtExportData.Columns.Add("Hospital Mobile No.", typeof(System.String));
                dtExportData.Columns.Add("Application Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Approved Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Total Sahay", typeof(System.String));
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
                    row["Hospital Name"] = Convert.ToString(finalResult[i].hospitalname);
                    row["Hospital Address"] = Convert.ToString(finalResult[i].hospitaladdress);
                    row["Hospital Mobile No."] = Convert.ToString(finalResult[i].hospitalmobile);
                    row["Application Date"] = Convert.ToDateTime(finalResult[i].applicationdate).ToString("MM/dd/yyyy");

                    if (finalResult[i].approveddate != null && Convert.ToString(finalResult[i].approveddate != null) != "")
                        row["Approved Date"] = Convert.ToDateTime(finalResult[i].approveddate).ToString("MM/dd/yyyy");
                    else
                        row["Approved Date"] = DBNull.Value;
                    row["Total Sahay"] = Convert.ToString(finalResult[i].finaltotalsahay);
                    row["Application Status"] = Convert.ToString(finalResult[i].isapprovestatus);


                    row["Bank Name"] = Convert.ToString(finalResult[i].BankName);
                    row["Bank Branch"] = Convert.ToString(finalResult[i].BankBranch);
                    row["IFSC Code"] = Convert.ToString(finalResult[i].IFSCCode);
                    row["Bank Account No."] = Convert.ToString(finalResult[i].BankAccountNo);
                    dtExportData.Rows.Add(row);
                }

                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    dtExportData.TableName = schemaName + "_ApplicationDetails";
                //    var ws = wb.Worksheets.Add(dtExportData);

                //    ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                //    //START : set borders to each used cell in excel
                //    ws.CellsUsed().Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.BottomBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.TopBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.LeftBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.LeftBorderColor = ClosedXML.Excel.XLColor.Black;
                //    ws.CellsUsed().Style.Border.RightBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                //    ws.CellsUsed().Style.Border.RightBorderColor = ClosedXML.Excel.XLColor.Black;
                //    //END : set borders to each used cell in excel

                //    ws.Columns().AdjustToContents();

                //    using (MemoryStream stream = new MemoryStream())
                //    {
                //        wb.SaveAs(stream);
                //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", schemaName + "_ApplicationDetails" + ".xlsx");
                //    }
                //}

                string tableName = schemaName + "_AppDetails";
                string fileName = schemaName + "_ApplicationDetails.csv";

                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);


            }


            return View(finalResult);
        }


        [HttpGet]
        public async Task<IActionResult> GetUploadedDocumentForEmployee(string ApplicationId, string ServiceId)
        {
            if (ServiceId == "24" || ServiceId == "34")
            {
                string schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(ServiceId));
                var model = await _ihomeService.GetGlwb_TsyUploadedDocumentForEmployee(Convert.ToInt32(ApplicationId), Convert.ToInt32(ServiceId), schemaName, nameof(EnumLookup.tablename.documentdetails));
                return PartialView("_EmployeeDocumentView", model);
            }
            else
            {
                string schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(ServiceId));
                var model = await _ihomeService.GetUploadedDocumentForEmployee(Convert.ToInt32(ApplicationId), Convert.ToInt32(ServiceId), schemaName, nameof(EnumLookup.tablename.documentdetails));
                return PartialView("_EmployeeDocumentView", model);
            }
            //return View("EmployeeDocumentView", model);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadDocument(string id, string fileName)
        {
            try
            {
                string[] splFileName = fileName.Split(".");
                CouchDBResponse objCouchDBResponse = await new CommonUtils(_config, _clientFactory).GetDocumentByteArray(id, fileName);

                if (objCouchDBResponse != null)
                {
                    byte[] fileBytes = await objCouchDBResponse.ImageData;
                    Stream stream = new MemoryStream(fileBytes);
                    //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, splFileName[0] + "." + splFileName[1]);
                    var provider = new FileExtensionContentTypeProvider();
                    var contentType = provider.TryGetContentType(fileName, out var type)
                        ? type : "application/octet-stream";

                    return new FileStreamResult(stream, contentType);
                }

                return File("", "");
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private List<SelectListItem> GetPageSizes(int selectedPageSize = 50)
        {
            var pageSizes = new List<SelectListItem>();
            //if (selectedPageSize == 5)
            //    pageSizes.Add(new SelectListItem("5", "5", true));
            //else
            //    pageSizes.Add(new SelectListItem("5", "5"));

            for (int lp = 50; lp <= 500; lp += 50)
            {
                if (lp == selectedPageSize)
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString(), true));
                else
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString()));
            }
            return pageSizes;
        }

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _ihomeService.GetDistrict();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetNextRole(int serviceid, int postid, long districtid, long talukaid)
        {
            var regions = _ihomeService.GetNextRole(serviceid, postid, districtid, talukaid);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        //public IActionResult GetSendBackRole(int serviceid, int postid, long districtid, long talukaid, long aplicationid)
        //{
        //    var regions = _ihomeService.GetSendBackRole(serviceid, postid, districtid, talukaid, aplicationid);
        //    //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        //    return Json(new { data = regions });
        //}

        [HttpGet]
        public async Task<IActionResult> GetApplicationHistory(long applicationid, int serviceid)
        {
            if (serviceid == 34)
            {
                var schemaName = Enum.GetName(typeof(schemaname), serviceid);
                var res = await _ihomeService.GetApplicationHistory_Glwb_Tsy_claim(applicationid, schemaName);
                //return Json(data, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                return Json(new { response = res });
                //return PartialView("_ViewHistoryActions",data);
            }
            else
            {
                var schemaName = Enum.GetName(typeof(schemaname), serviceid);
                var res = await _ihomeService.GetApplicationHistory(applicationid, schemaName);
                //return Json(data, System.Web.Mvc.JsonRequestBehavior.AllowGet);
                return Json(new { response = res });
                //return PartialView("_ViewHistoryActions",data);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApproveApplication(string toRole, string remarks, string ApplicationNo, string approvalstate, string ApplicationId, string ServiceId, string FromPostId, string approvalstatus, string toSendBackRole, string[] applicationidlist, IFormFile IdImage)
        {
            string ipAddress = CommonUtils.GetLocalIPAddress();
            string hostName = CommonUtils.GetHostName();
            var routeData = _httpContextAccessor.HttpContext.Request.RouteValues;
            string[] topostid = toRole.Split("/");
            int toroleid = Convert.ToInt32(topostid[0]);
            string postid = topostid[1];
            //var schemaname = nameof(EnumLookup.schemaname.bocw_ssy);
            var serviceid = Convert.ToInt32(ServiceId);
            var schemaName = Enum.GetName(typeof(schemaname), serviceid);
            var tablename = nameof(EnumLookup.tablename.approvaldetails);
            int beneficiarytype = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var objModel = await _ihomeService.IsLastLevel(Convert.ToInt32(ServiceId), Convert.ToInt32(FromPostId), Convert.ToInt32(ApplicationId));
            if (approvalstate == "1")
            {
                if (objModel.isLastLevel == 1)
                {
                    approvalstatus = Convert.ToString((int)EnumLookup.ApplicationStatus.Approve);
                }
                else
                {
                    approvalstatus = Convert.ToString((int)EnumLookup.ApplicationStatus.Pending);
                }

            }

            DataTable dtApplicationIds = new DataTable();
            dtApplicationIds.Columns.Add("applicationid", typeof(long));
            var couchDBRequest = new CouchDBRequest();
            var couchDBResponse = new CouchDBResponse();
            EmpApplicationDetailsModel empApplicationDetailsModel = new EmpApplicationDetailsModel();


            if (IdImage != null)
            {
                var extension = Path.GetExtension(IdImage.FileName);
                string FileName = ApplicationId + "_" + "DISH Aaheval" + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") +
                                           Convert.ToString(extension);
                byte[] fileBytes = null;
                using (var msstream = new MemoryStream())
                {
                    await IdImage.CopyToAsync(msstream);
                    fileBytes = msstream.ToArray();
                }
                couchDBRequest.FileName = FileName;//item.IdImage.File.FileName;
                couchDBRequest.AttachmentData = fileBytes;
                couchDBRequest.FileExtension =
                    Path.GetExtension(IdImage.FileName).Replace(".", "").ToUpper();
                couchDBRequest.CreatedOn = DateTime.Now.ToString();
                couchDBResponse = await new CommonUtils(_config, _clientFactory).UplodToCouchDB(couchDBRequest);

                if (couchDBResponse.IsSuccess == true)
                {
                    DocumentDetails.DocumentFileDetails model = new DocumentDetails.DocumentFileDetails();
                    model.ApplicationId = Convert.ToInt64(ApplicationId);
                    model.DocumentName = FileName;
                    model.DocumentMasterId = 84;
                    model.CreatedBy = Convert.ToInt64(FromPostId);
                    model.IpAddress = ipAddress;
                    model.HostName = hostName;
                    model.CouchDBDocId = couchDBResponse.Id;
                    model.CouchDBDocRevId = couchDBResponse.Rev;
                    model.schemaname = nameof(EnumLookup.schemaname.bocw_acsy);
                    model.tablename = nameof(EnumLookup.tablename.documentdetails);

                    ResponseMessage responseMessage = new ResponseMessage();

                    responseMessage = await _ihomeService.AddDocumentForBOCW_ACSY(model);
                }
            }

            for (int i = 0; i < applicationidlist.Count(); i++)
            {
                dtApplicationIds.Rows.Add(Convert.ToInt64(applicationidlist[i]));
            }
            //var approveModel = await _ihomeService.ApproveApplication(ApplicationId, approvalstate, toroleid, approvalstatus, remarks, FromPostId, ServiceId, postid, schemaName, tablename, beneficiarytype);
            var approveModel = await _ihomeService.ApproveMultipleApplication(ApplicationId, approvalstate, toroleid, approvalstatus, remarks, FromPostId, ServiceId, postid, schemaName, tablename, beneficiarytype, dtApplicationIds, ipAddress, hostName);
            List<EmpApplicationDetailsModel> lstResult = approveModel.ToList();

            if (approveModel != null)
            {

                for (int i = 0; i < lstResult.Count(); i++)
                {
                    string errorMsg = lstResult[i].Msg == null ? "Somthing went wrong please try again." : lstResult[i].Msg;
                    if (approveModel != null && lstResult[i].Error == 0)
                    {

                        if (objModel.isLastLevel == 1)
                        {
                            var benificiaryType = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
                            CommonUtils commonFunction = new CommonUtils(_config);
                            string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                            var res = commonFunction.SendARSApplicationMail(lstResult[i].applicantname, lstResult[i].email, lstResult[i].Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", benificiaryType == 1 ? _bocwEmailLogo : _glwbEmailLogo);

                            SMSModel modelSMSModel = await _ihomeService.GetSmsContentForService(Convert.ToInt64(ServiceId), lstResult[i].applicationid, (int)EnumLookup.smstype.AppAccepted, schemaName, nameof(EnumLookup.tablename.personaldetails));
                            if (modelSMSModel != null)
                            {
                                bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                                if (isSendSMS)
                                {
                                    await _ihomeService.AddSMSLogs(modelSMSModel.MobileNo, Convert.ToInt64(ServiceId), modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                                }

                            }

                            //#region Generate Aadesh Report

                            //empApplicationDetailsModel.aadeshreport = await GenerateBOCWAadeshReport(applicationidlist, schemaName);

                            //#endregion
                            string message = "Application Approved & Aadesh Generated..!!";
                            empApplicationDetailsModel.Msg = message;
                        }
                        else
                        {
                            string message = "Application Approved..!!";
                            empApplicationDetailsModel.Msg = message;
                        }


                        ModelState.Clear();

                        return Json(empApplicationDetailsModel);
                    }
                    else
                    {
                        empApplicationDetailsModel.Msg = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                        ModelState.Clear();
                        return Json(empApplicationDetailsModel);
                    }
                }
            }
            else
            {
                empApplicationDetailsModel.Msg = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                return Json(empApplicationDetailsModel);
            }


            return Json(empApplicationDetailsModel);
        }




        //public async Task<IActionResult> ApproveActions(EmpApplicationDetailsModel model)
        //{
        //    ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
        //    ModelState.Clear();
        //    return View(model);
        //}

        public async Task<IActionResult> SendBackApplication(string remarks, string ApplicationNo, string approvalstate, string ApplicationId, string ServiceId, string FromPostId, string approvalstatus, string toSendBackRole)
        {
            string[] tosendbackpostid = toSendBackRole.Split("/");
            int tosendbackroleid = Convert.ToInt32(tosendbackpostid[0]);
            string sendbackpostid = tosendbackpostid[1];


            var schemaname = Enum.GetName(typeof(schemaname), Convert.ToInt32(ServiceId));
            var tablename = nameof(EnumLookup.tablename.approvaldetails);
            int levelno = Convert.ToInt32(_claimPincipal.FindFirst("OrderBy").Value);
            int beneficiarytype = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var objModel = await _ihomeService.IsLastLevel(Convert.ToInt32(ServiceId), Convert.ToInt32(sendbackpostid), Convert.ToInt32(ApplicationId));

            if (approvalstate == "2")
            {
                approvalstatus = Convert.ToString((int)EnumLookup.ApplicationStatus.Pending);
            }


            var approveModel = await _ihomeService.ApproveApplication(ApplicationId, approvalstate, tosendbackroleid, approvalstatus, remarks, FromPostId, ServiceId, sendbackpostid, schemaname, tablename, beneficiarytype);

            if (approveModel != null)
            {
                string errorMsg = approveModel.Msg == null ? "Somthing went wrong please try again." : approveModel.Msg;
                if (approveModel != null && approveModel.Error == 0)
                {

                    if (tosendbackroleid == levelno)
                    {
                        var benificiaryType = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
                        var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(ServiceId));
                        CommonUtils commonFunction = new CommonUtils(_config);
                        string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                        var res = commonFunction.SendARSApplicationMail(approveModel.applicantname, approveModel.email, approveModel.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", benificiaryType == 1 ? _bocwEmailLogo : _glwbEmailLogo);

                        SMSModel modelSMSModel = await _ihomeService.GetSmsContentForService(Convert.ToInt64(ServiceId), Convert.ToInt64(ApplicationId), (int)EnumLookup.smstype.AppReverted, schemaName, nameof(EnumLookup.tablename.personaldetails));
                        if (modelSMSModel != null)
                        {
                            bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                            if (isSendSMS)
                            {
                                await _ihomeService.AddSMSLogs(modelSMSModel.MobileNo, Convert.ToInt64(ServiceId), modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                            }

                        }
                    }

                    string message = "Application SentBack..!!";
                    approveModel.Msg = message;
                    ModelState.Clear();
                    return Json(approveModel);

                }
                else
                {
                    approveModel.Msg = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    return Json(approveModel);
                }
            }
            else
            {
                approveModel.Msg = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                return Json(approveModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RejectApplication(string toRole, string remarks, string ApplicationNo, string approvalstate, string ApplicationId, string ServiceId, string FromPostId, string approvalstatus, string toSendBackRole, string[] applicationidlist)
        {

            string ipAddress = CommonUtils.GetLocalIPAddress();
            string hostName = CommonUtils.GetHostName();
            var routeData = _httpContextAccessor.HttpContext.Request.RouteValues;
            string[] topostid = toRole.Split("/");
            int toroleid = Convert.ToInt32(topostid[0]);
            string postid = topostid[1];
            var schemaname = Enum.GetName(typeof(schemaname), Convert.ToInt32(ServiceId));
            var tablename = nameof(EnumLookup.tablename.approvaldetails);
            int beneficiarytype = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var objModel = await _ihomeService.IsLastLevel(Convert.ToInt32(ServiceId), Convert.ToInt32(FromPostId), Convert.ToInt32(ApplicationId));
            if (approvalstate == "3")
            {
                approvalstatus = Convert.ToString((int)EnumLookup.ApplicationStatus.Reject);

            }

            DataTable dtApplicationIds = new DataTable();
            dtApplicationIds.Columns.Add("applicationid", typeof(long));

            for (int i = 0; i < applicationidlist.Count(); i++)
            {
                dtApplicationIds.Rows.Add(Convert.ToInt64(applicationidlist[i]));
            }

            var approveModel = await _ihomeService.ApproveMultipleApplication(ApplicationId, approvalstate, toroleid, approvalstatus, remarks, FromPostId, ServiceId, postid, schemaname, tablename, beneficiarytype, dtApplicationIds, ipAddress, hostName);
            List<EmpApplicationDetailsModel> lstResult = approveModel.ToList();
            EmpApplicationDetailsModel empApplicationDetailsModel = new EmpApplicationDetailsModel();
            if (approveModel != null)
            {

                for (int i = 0; i < lstResult.Count(); i++)
                {
                    string errorMsg = lstResult[i].Msg == null ? "Somthing went wrong please try again." : lstResult[i].Msg;
                    if (approveModel != null && lstResult[i].Error == 0)
                    {

                        if (objModel.isLastLevel == 1)
                        {
                            var benificiaryType = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
                            CommonUtils commonFunction = new CommonUtils(_config);
                            string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                            var res = commonFunction.SendARSApplicationMail(lstResult[i].applicantname, lstResult[i].email, lstResult[i].Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", benificiaryType == 1 ? _bocwEmailLogo : _glwbEmailLogo);

                            SMSModel modelSMSModel = await _ihomeService.GetSmsContentForService(Convert.ToInt64(ServiceId), lstResult[i].applicationid, (int)EnumLookup.smstype.AppRejected, schemaname, nameof(EnumLookup.tablename.personaldetails));
                            if (modelSMSModel != null)
                            {
                                bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                                if (isSendSMS)
                                {
                                    await _ihomeService.AddSMSLogs(modelSMSModel.MobileNo, Convert.ToInt64(ServiceId), modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                                }

                            }
                        }

                        string message = "Application Rejected..!!";
                        empApplicationDetailsModel.Msg = message;
                        ModelState.Clear();

                        return Json(empApplicationDetailsModel);
                    }
                    else
                    {
                        empApplicationDetailsModel.Msg = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                        ModelState.Clear();
                        return Json(empApplicationDetailsModel);
                    }
                }
            }
            else
            {
                empApplicationDetailsModel.Msg = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                return Json(empApplicationDetailsModel);
            }
            return Json(empApplicationDetailsModel);
        }


        [HttpGet]
        public async Task<IActionResult> GetEmployeePopUp(EmpApproveApplication empApproveApplication)
        {
            int postid = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            int districtid = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
            int talukaid = Convert.ToInt32(_claimPincipal.FindFirst("TalukaId").Value);
            int levelno = Convert.ToInt32(_claimPincipal.FindFirst("OrderBy").Value);

            var regions = _ihomeService.GetNextRole(Convert.ToInt32(empApproveApplication.serviceid), postid, districtid, talukaid);
            var objModel = await _ihomeService.IsLastLevel(Convert.ToInt32(empApproveApplication.serviceid), Convert.ToInt32(postid), Convert.ToInt32(empApproveApplication.applicationid));
            bool isLastLevel = objModel.isLastLevel == 1 ? true : false;

            var GetUploadedDocuments = await _ihomeService.getemployeeuploadeddocumentsbyappid(Convert.ToInt64(empApproveApplication.applicationid));
            //var couchDBID = await _

            ViewBag.NextRoles = regions.Result.ToList();
            ViewBag.isLastLevel = isLastLevel;
            ViewBag.ApproveApplication = empApproveApplication;
            ViewBag.DocumentsList = GetUploadedDocuments;
            return PartialView("ApproveActions");
        }

        [HttpGet]
        public async Task<IActionResult> GetRejectPopUp(EmpApproveApplication empApproveApplication)
        {
            ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            ViewBag.RejectApplication = empApproveApplication;
            return PartialView("_RejectActions");
        }

        [HttpGet]
        public async Task<IActionResult> GetSendBackPopUp(EmpApproveApplication empApproveApplication)
        {
            int postid = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            int districtid = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
            int talukaid = Convert.ToInt32(_claimPincipal.FindFirst("TalukaId").Value);
            int levelno = Convert.ToInt32(_claimPincipal.FindFirst("OrderBy").Value);
            int beneficiarytype = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var serviceId = Convert.ToInt32(empApproveApplication.serviceid);
            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            if (serviceId == 24 || serviceId == 34)
            {
                var regions = _ihomeService.GetSendBackRoleGLWb_TSY(Convert.ToInt32(empApproveApplication.serviceid), postid, districtid, Convert.ToInt32(empApproveApplication.applicationid), schemaName, beneficiarytype);
                bool isFirstLevel = false;
                if (regions.Result.ToList().Count == 1)
                {
                    string[] topostid = regions.Result.ToList()[0].Value.Split("/");
                    if (topostid != null)
                    {
                        if (Convert.ToInt32(topostid[0]) == levelno)
                        {
                            isFirstLevel = true;
                        }
                    }
                }
                ViewBag.SendBackRoles = regions.Result.ToList();
                ViewBag.SendBackApplication = empApproveApplication;
                ViewBag.isFirstLevel = isFirstLevel;
                return PartialView("_SendBackActions");
            }
            else
            {
                var regions = _ihomeService.GetSendBackRole(Convert.ToInt32(empApproveApplication.serviceid), postid, districtid, talukaid, Convert.ToInt32(empApproveApplication.applicationid), schemaName, beneficiarytype);
                bool isFirstLevel = false;
                if (regions.Result.ToList().Count == 1)
                {
                    string[] topostid = regions.Result.ToList()[0].Value.Split("/");
                    if (topostid != null)
                    {
                        if (Convert.ToInt32(topostid[0]) == levelno)
                        {
                            isFirstLevel = true;
                        }
                    }
                }
                ViewBag.SendBackRoles = regions.Result.ToList();
                ViewBag.SendBackApplication = empApproveApplication;
                ViewBag.isFirstLevel = isFirstLevel;
                return PartialView("_SendBackActions");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewHistoryPopUp(EmpApproveApplication empApproveApplication)
        {

            //var serviceId = Convert.ToInt32(empApproveApplication.serviceid);
            //var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            //var data = await _ihomeService.GetApplicationHistory(Convert.ToInt64(empApproveApplication.applicationid),schemaName);
            //ViewBag.History = data.ToList();
            ViewBag.ViewHistoryApplication = empApproveApplication;
            return PartialView("_ViewHistoryActions");
        }

        [HttpGet]
        public async Task<IActionResult> ViewHospitalScheduleTimePopUp(HospitalScheduleTime hospitalScheduleTime)
        {
            ViewBag.HospitalScheduleTime = hospitalScheduleTime;
            return PartialView("_HospitalTimeSchedule");
        }

        [HttpGet]
        public async Task<IActionResult> UploadApplicationReport(HospitalScheduleTime hospitalScheduleTime)
        {
            var GetUploadedDocuments = await _ihomeService.gethospitaluploadeddocumentsbyappid(Convert.ToInt64(hospitalScheduleTime.ApplicationId));
            ViewBag.DocumentsList = GetUploadedDocuments;
            ViewBag.HospitalScheduleTime = hospitalScheduleTime;
            return PartialView("_HospitalApplicationReportUpload");
        }

        [HttpGet]
        public async Task<IActionResult> GetHospitalScheduleTime(long applicationid, long serviceid)
        {
            var res = await _ihomeService.GetHospitalScheduleTime(applicationid, serviceid);
            return Json(new { response = res });
        }

        public async Task<IActionResult> AddTimeScheduleHospital(HospitalScheduleTime hospitalScheduleTime)
        {
            hospitalScheduleTime.CreatedDate = DateTime.Now;
            hospitalScheduleTime.IpAddress = CommonUtils.GetLocalIPAddress();
            hospitalScheduleTime.HostName = CommonUtils.GetHostName();
            hospitalScheduleTime.IsDeleted = false;
            var approveModel = await _ihomeService.TimeScheduleHospital(hospitalScheduleTime);

            if (approveModel != null)
            {
                CommonUtils commonFunction = new CommonUtils(_config);
                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                var res = commonFunction.SendApplicationRegisteredMail(approveModel.ApplicantName, approveModel.email, approveModel.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", "2");
            }
            return Json(approveModel);

        }

        [HttpPost]
        public async Task<IActionResult> UploadUserApplicationReport(HospitalScheduleTime hospitalScheduleTime, IFormFile IdImage)
        {
            hospitalScheduleTime.CreatedDate = DateTime.Now;
            hospitalScheduleTime.IpAddress = CommonUtils.GetLocalIPAddress();
            hospitalScheduleTime.HostName = CommonUtils.GetHostName();
            hospitalScheduleTime.IsDeleted = false;
            var couchDBRequest = new CouchDBRequest();
            var couchDBResponse = new CouchDBResponse();
            if (IdImage != null)
            {
                var ApplicationId = hospitalScheduleTime.ApplicationId;
                var extension = Path.GetExtension(IdImage.FileName);
                string FileName = hospitalScheduleTime.ApplicationNo + "_" + "Application Report" + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + Convert.ToString(extension);
                byte[] fileBytes = null;
                using (var msstream = new MemoryStream())
                {
                    await IdImage.CopyToAsync(msstream);
                    fileBytes = msstream.ToArray();
                }
                couchDBRequest.FileName = FileName;//item.IdImage.File.FileName;
                couchDBRequest.AttachmentData = fileBytes;
                couchDBRequest.FileExtension =
                    Path.GetExtension(IdImage.FileName).Replace(".", "").ToUpper();
                couchDBRequest.CreatedOn = DateTime.Now.ToString();
                couchDBResponse = await new CommonUtils(_config, _clientFactory).UplodToCouchDB(couchDBRequest);

                if (couchDBResponse.IsSuccess == true)
                {
                    DocumentDetails.DocumentFileDetails model = new DocumentDetails.DocumentFileDetails();
                    model.ApplicationId = Convert.ToInt64(ApplicationId);
                    hospitalScheduleTime.DocumentName = FileName;
                    hospitalScheduleTime.CouchDBDocId = couchDBResponse.Id;
                    hospitalScheduleTime.CouchDBDocRevId = couchDBResponse.Rev;
                    ResponseMessage responseMessage = new ResponseMessage();
                    var approveModel = await _ihomeService.UploadUserApplicationReport(hospitalScheduleTime);
                    if (approveModel != null)
                    {
                        return Json(approveModel);
                    }
                }
            }
            return PartialView("_HospitalApplicationReportUpload");
        }

        [HttpGet]
        public async Task<IActionResult> ViewRegisteredApplicant(int? pageNo, int pageSize, int hodId, long districtId = 0, string fromDate = "", string toDate = "", string? search = "", string export = "")
        {

            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);

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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;

            if (!string.IsNullOrEmpty(fromDate))
            {
                var finalfromDateTime = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo = String.Format("{0:dd/MM/yyyy}", finalfromDateTime);
                dtFromDate = DateTime.ParseExact(demo, "dd/MM/yyyy", null);
            }
            if (!string.IsNullOrEmpty(toDate))
            {

                var finalToDateTime = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo1 = String.Format("{0:dd/MM/yyyy}", finalToDateTime);
                dtToDate = DateTime.ParseExact(demo1, "dd/MM/yyyy", null);
            }


            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<RegisteredApplicant> modelList;

            districtId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);
            hodId = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);


            modelList = await _ihomeService.ViewRegisteredApplicant(hodId, dtFromDate, dtToDate, districtId, pageNo, pageSize, search);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();

            ViewBag.PageSizes = GetPageSizes(pageSize);
            ViewBag.StartDate = dtFromDate == null ? "" : Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = dtToDate == null ? "" : Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
            ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<RegisteredApplicant>(modelList, pageIndex, pageSize, TotalPageCount);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "ViewRegisteredApplicant",
                Controller = "EmployeeHome",
                SearchText = search,
                EDistrictId = districtId,
                StartDate = dtFromDate == null ? "" : Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),
                EndDate = dtToDate == null ? "" : Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            ViewBag.DistrictId = districtId;



            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                dtExportData.Columns.Add("UserID", typeof(System.String));
                dtExportData.Columns.Add("Applicant Name", typeof(System.String));
                dtExportData.Columns.Add("Gender", typeof(System.String));
                dtExportData.Columns.Add("BirthDate", typeof(System.DateTime));
                dtExportData.Columns.Add("Aadhar Card Number", typeof(System.String));
                dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                dtExportData.Columns.Add("Email ID", typeof(System.String));
                dtExportData.Columns.Add("Registration Date", typeof(System.DateTime));




                for (int i = 0; i < finalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr. No."] = Convert.ToString(finalResult[i].srno);
                    row["UserID"] = Convert.ToString(finalResult[i].registrationno);
                    row["Applicant Name"] = Convert.ToString(finalResult[i].name);
                    row["Gender"] = Convert.ToString(finalResult[i].gender);

                    if (finalResult[i].dateofbirth != null && Convert.ToString(finalResult[i].dateofbirth != null) != "")
                        row["BirthDate"] = Convert.ToDateTime(finalResult[i].dateofbirth).ToString("MM/dd/yyyy");
                    else
                        row["BirthDate"] = DBNull.Value;

                    row["Aadhar Card Number"] = Convert.ToString(finalResult[i].aadharno);
                    row["Mobile No."] = Convert.ToString(finalResult[i].mobileno);
                    row["Email ID"] = Convert.ToString(finalResult[i].emailid);

                    if (finalResult[i].registrationdate != null && Convert.ToString(finalResult[i].registrationdate != null) != "")
                        row["Registration Date"] = Convert.ToDateTime(finalResult[i].registrationdate).ToString("MM/dd/yyyy");
                    else
                        row["Registration Date"] = DBNull.Value;

                    dtExportData.Rows.Add(row);
                }


                string tableName = "Registered Applicants";
                string fileName = "RegisteredApplicants" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }



            return View(finalResult);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string RegistrationId)
        {
            long userId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            string password = CommonUtils.GenerateRandomStrongPassword(8);
            string ipAddress = CommonUtils.GetLocalIPAddress();
            string hostName = CommonUtils.GetHostName();
            var model = await _ihomeService.ResetPassword(Convert.ToInt64(RegistrationId), userId, password, ipAddress, hostName);
            return Json(new { data = model });
        }


        [HttpGet]
        public async Task<IActionResult> DeleteRegistration(string RegistrationId)
        {
            long userId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            long beneficiaryTypeId = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            string ipAddress = CommonUtils.GetLocalIPAddress();
            string hostName = CommonUtils.GetHostName();
            var model = await _ihomeService.DeleteRegistration(Convert.ToInt64(RegistrationId), userId, beneficiaryTypeId, ipAddress, hostName);
            return Json(new { data = model });
        }

        [HttpGet]
        public async Task<IActionResult> ViewUpdateCitizenMobileNo(string RegistrationId, string MobileNo)
        {
            RegisteredApplicant registeredApplicant = new RegisteredApplicant();
            registeredApplicant.registrationid = Convert.ToInt64(RegistrationId);
            registeredApplicant.mobileno = MobileNo;

            return PartialView("_UpdateCitizenMobileNo", registeredApplicant);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCitizenMobileNo(string RegistrationId, string MobileNo)
        {
            long userId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var model = await _ihomeService.UpdateCitizenMobileNo(Convert.ToInt64(RegistrationId), userId, MobileNo);

            if (model != null && model.Error == 1)
            {
                TempData["Message"] = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
            }
            else
            {
                TempData["Message"] = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }

            return RedirectToAction("ViewRegisteredApplicant");
        }

        [HttpGet]
        public async Task<IActionResult> ViewApplicantionStatus(string applicationno = "", string dateofbirth = "")
        {
            DateTime? dob = null;
            if (!string.IsNullOrEmpty(dateofbirth))
            {
                var finalfromDateTime = DateTime.ParseExact(dateofbirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo = String.Format("{0:dd/MM/yyyy}", finalfromDateTime);
                dob = DateTime.ParseExact(demo, "dd/MM/yyyy", null);
            }
            ViewBag.Dateofbirth = dob == null ? "" : Convert.ToDateTime(dob).ToString("dd/MM/yyyy");
            dob = dob == null ? DateTime.MinValue : dob;
            applicationno = String.IsNullOrEmpty(applicationno) ? "0000000000" : Convert.ToString(applicationno);

            ApplicationStatusModel modelList;
            IEnumerable<ApplicationStatusDetails> statusList;


            //modelList = await _ihomeService.ViewApplicationStatus(applicationno, dob);
            modelList = await _ihomeService.ViewApplicationStatus(applicationno, dob);
            statusList = await _ihomeService.GetRoleForApplicationStatus(applicationno, dob);

            List<ApplicationStatusDetails> applicationStatusDetailsList = new List<ApplicationStatusDetails>();


            if (modelList != null && statusList != null && statusList.Count() > 0)
            {
                foreach (var status in statusList)
                {
                    ApplicationStatusDetails applicationStatusDetails = new ApplicationStatusDetails();
                    applicationStatusDetails.orderby = status.orderby;
                    applicationStatusDetails.TimelineStatus = status.TimelineStatus;
                    applicationStatusDetails.description = status.description;
                    applicationStatusDetails.role_name = status.role_name;
                    applicationStatusDetailsList.Add(applicationStatusDetails);
                }
                modelList.applicationStatusDetails = applicationStatusDetailsList;
            }

            if (modelList != null && statusList != null && statusList.Count() > 0)
            {
                ViewBag.ApplicationNo = applicationno;
                return View(modelList);
            }
            else
            {
                ViewBag.ApplicationNo = "";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeApplicationSearch(string applicationno = "")
        {
            applicationno = String.IsNullOrEmpty(applicationno) ? "0000000000" : Convert.ToString(applicationno);
            int hodid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            ApplicationStatusModel modelList;
            IEnumerable<ApplicationStatusDetails> statusList;
            if (hodid == 5)
            {
                hodid = 2;
            }
            else if (hodid == 4)
            {
                hodid = 1;
            }


            //modelList = await _ihomeService.ViewApplicationStatus(applicationno, dob);
            modelList = await _ihomeService.ViewApplicationEmployeeSearch(applicationno, hodid);
            statusList = await _ihomeService.GetRoleForApplicationStatusForEmployee(applicationno, hodid);

            List<ApplicationStatusDetails> applicationStatusDetailsList = new List<ApplicationStatusDetails>();


            if (modelList != null && statusList != null && statusList.Count() > 0)
            {
                foreach (var status in statusList)
                {
                    ApplicationStatusDetails applicationStatusDetails = new ApplicationStatusDetails();
                    applicationStatusDetails.orderby = status.orderby;
                    applicationStatusDetails.TimelineStatus = status.TimelineStatus;
                    applicationStatusDetails.description = status.description;
                    applicationStatusDetails.role_name = status.role_name;
                    applicationStatusDetailsList.Add(applicationStatusDetails);
                }
                modelList.applicationStatusDetails = applicationStatusDetailsList;
            }

            if (modelList != null && statusList != null && statusList.Count() > 0)
            {
                ViewBag.ApplicationNo = applicationno;
                return View(modelList);
            }
            else
            {
                ViewBag.ApplicationNo = "";
                return View();
            }

        }

        [HttpGet]
        public async Task<IActionResult> ViewTotalApplicationDetails(int? pageNo, int pageSize, int hodId, long serviceId = 0, long isApproved = 0, string fromDate = "", string toDate = "", string? search = "", string export = "")
        {

            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);

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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
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
            }


            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<CompletedApplicationDetailsModel> modelList;


            hodId = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);


            modelList = await _ihomeService.ViewTotalApplicationDetails(hodId, dtFromDate, dtToDate, serviceId, isApproved, pageNo, pageSize, search);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();


            var usertype = Convert.ToInt32(_claimPincipal.FindFirst("UserType").Value);
            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var postid = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            var roleid = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);

            var serviceModel = _ihomeService.GetServiceMasterByBeneficiaryId(benificiarytypeid);
            var services = serviceModel.Result.ToList();

            ViewBag.ServiceList = services;


            ViewBag.PageSizes = GetPageSizes(pageSize);
            ViewBag.StartDate = dtFromDate == null ? "" : Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = dtToDate == null ? "" : Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
            ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<CompletedApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "ViewTotalApplicationDetails",
                Controller = "EmployeeHome",
                SearchText = search,
                ServiceId = serviceId,
                StartDate = dtFromDate == null ? "" : Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),
                EndDate = dtToDate == null ? "" : Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),
                isApproved = isApproved,

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            ViewBag.ServiceId = serviceId;
            ViewBag.IsApproved = isApproved;

            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr.No.", typeof(System.String));
                dtExportData.Columns.Add("HOD Name", typeof(System.String));
                dtExportData.Columns.Add("Service Name", typeof(System.String));
                dtExportData.Columns.Add("Application Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Application No.", typeof(System.String));
                dtExportData.Columns.Add("Applicant Name", typeof(System.String));
                dtExportData.Columns.Add("Caste", typeof(System.String));
                dtExportData.Columns.Add("BirthDate", typeof(System.DateTime));
                dtExportData.Columns.Add("District", typeof(System.String));
                dtExportData.Columns.Add("Taluka", typeof(System.String));
                dtExportData.Columns.Add("Village", typeof(System.String));
                dtExportData.Columns.Add("Mobile No.", typeof(System.String));




                for (int i = 0; i < finalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr.No."] = Convert.ToString(finalResult[i].srno);
                    row["HOD Name"] = Convert.ToString(finalResult[i].hodname);
                    row["Service Name"] = Convert.ToString(finalResult[i].servicename);

                    if (finalResult[i].applicationdate != null && Convert.ToString(finalResult[i].applicationdate != null) != "")
                        row["Application Date"] = Convert.ToDateTime(finalResult[i].applicationdate).ToString("MM/dd/yyyy");
                    else
                        row["Application Date"] = DBNull.Value;

                    row["Application No."] = Convert.ToString(finalResult[i].applicationno);
                    row["Applicant Name"] = Convert.ToString(finalResult[i].name);
                    row["Caste"] = Convert.ToString(finalResult[i].castename);
                    if (finalResult[i].dateofbirth != null && Convert.ToString(finalResult[i].dateofbirth != null) != "")
                        row["BirthDate"] = Convert.ToDateTime(finalResult[i].dateofbirth).ToString("MM/dd/yyyy");
                    else
                        row["BirthDate"] = DBNull.Value;
                    row["District"] = Convert.ToString(finalResult[i].districtname);
                    row["Taluka"] = Convert.ToString(finalResult[i].talukaname);
                    row["Village"] = Convert.ToString(finalResult[i].villagename);
                    row["Mobile No."] = Convert.ToString(finalResult[i].mobileno);


                    dtExportData.Rows.Add(row);
                }


                string tableName = "";
                if (isApproved == 0)
                {
                    tableName = "Total Application";
                }
                else if (isApproved == 1)
                {
                    tableName = "Pending Application";
                }
                else if (isApproved == 2)
                {
                    tableName = "Completed Application";
                }
                else if (isApproved == 3)
                {
                    tableName = "Rejected Application";
                }
                string fileName = tableName + " " + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }





            return View(finalResult);
        }

        [HttpGet]
        //[TypeFilter(typeof(PermissionRequirementFilter))]
        public async Task<IActionResult> BOCWCompletedApplicationForPayment(int? pageNo, int pageSize, long districtId = 0, long talukaId = 0, long villageId = 0, string fromDate = "", string toDate = "", string? search = "", string strServiceId = "", string export = "", long divisionId = 0, int hodid = 0)
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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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

            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            if (postId == 37)
            {
                postId = 38;
            }

            long serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            ViewBag.hodid = hodid;
            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<EmpApplicationDetailsModel> modelList;

            long logindistrictId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);


            if (districtId == 0)
            {
                if (logindistrictId == 34)
                {
                    districtId = 0;
                }
                else
                {
                    districtId = logindistrictId;
                }

            }
            modelList = await _ihomeService.BOCWCompletedApplicationForPayment(pageNo, pageSize, districtId, talukaId, villageId, dtFromDate, dtToDate, search, postId, serviceId, "", schemaName);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();

            ViewBag.PageSizes = GetPageSizes(pageSize);


            var villageModel = _ihomeService.GetVillageByDistrictIdAndTalukaId(Convert.ToInt32(districtId), Convert.ToInt32(talukaId));
            var villageList = villageModel.Result.ToList();


            var districtModel = _ihomeService.GetDistrict();
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;
            ViewBag.DistrictId = districtId;

            var talukaModel = _ihomeService.GetTalukaByDistrictId(Convert.ToInt32(districtId));
            var talukaList = talukaModel.Result.ToList();
            ViewBag.TalukaList = talukaList;




            ViewBag.VillageList = villageList;
            ViewBag.VillageId = villageId;
            ViewBag.TalukaId = talukaId;
            ViewBag.ServiceId = serviceId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");
            ViewBag.RoleId = Convert.ToInt32(User.FindFirst(ClaimTypes.Role).Value);
            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "EmployeeApplicationDetails",
                Controller = "EmployeeHome",
                strServiceId = strServiceId,
                SearchText = search,
                EDistrictId = districtId,
                EVillageId = villageId,
                ETalukaId = talukaId,
                EDivisionId = divisionId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                                                                              //BeneficiaryId = strbeneficiaryid
                                                                              //StatusId = statusId,

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            //ViewBag.strbeneficiaryid = strbeneficiaryid;
            //ViewBag.Status = statusId;


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

        [HttpGet]
        public async Task<ContentResult> DownloadBOCWAadeshReport(EmpApproveApplication empApproveApplication)
        {
            var serviceid = Convert.ToInt32(empApproveApplication.serviceid);
            var schemaName = Enum.GetName(typeof(schemaname), serviceid);

            string applicationids = "";
            for (int i = 0; i < empApproveApplication.applicationidlist.Count(); i++)
            {
                applicationids += empApproveApplication.applicationidlist[i] + ",";
            }

            IEnumerable<EmpApplicationDetailsModel> modelList;


            modelList = await _ihomeService.GetAppDetailsByAppIdForViewAadesh(applicationids.Trim(','), schemaName);


            DataSet ds = new DataSet();
            if (modelList != null && modelList.Count() > 0)
            {
                DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                ds.Tables.Add(dtData);

                string rdlcFileName = "BOCWAadeshReport.rdlc";
                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                string reportName = "AadeshDetails";

                try
                {
                    byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

                    string strHTML = System.Text.Encoding.UTF8.GetString(reportData);

                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(strHTML);
                    var pdf = _generatePdf.GetPDF(strHTML);
                    var pdfStream = new System.IO.MemoryStream();
                    pdfStream.Write(pdf, 0, pdf.Length);
                    pdfStream.Position = 0;
                    string base64 = Convert.ToBase64String(pdf, 0, pdf.Length);

                    return Content(base64);
                    //return new FileStreamResult(pdfStream, "application/pdf");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return null;
                //return new FileStreamResult(null, "application/pdf");
            }



        }


        [HttpGet]
        public async Task<IActionResult> BOCWViewAadeshDetails(int? pageNo, int pageSize, long districtId = 0, string fromDate = "", string toDate = "", int statusId = 0, string? search = "", long serviceId = 0, string export = "")
        {
            if (serviceId == 0)
            {
                serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            }

            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);
            obj.AddUpdateClaim("ServiceId", Convert.ToString(serviceId));

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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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

            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var serviceModel = _ihomeService.GetServiceMasterByBeneficiaryId(benificiarytypeid);
            var services = serviceModel.Result.ToList();

            ViewBag.ServiceList = services;

            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<EmpApplicationDetailsModel> modelList;

            long logindistrictId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);


            modelList = await _ihomeService.BOCWViewAadeshDetails(pageNo, pageSize, districtId, dtFromDate, dtToDate, statusId, search, serviceId, schemaName);
            int recsCount = modelList.GroupBy(x => x.aadeshid).Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.DistinctBy(d => d.aadeshid).Skip(recSkip).Take(pageSize).ToList();



            var distinctAadeshIds = string.Join(",", finalResult.Select(p => p.aadeshid.ToString()));

            //finalResult = modelList.Where(x => x.aadeshid == finalResult[0].aadeshid).ToList();
            finalResult = modelList.Where(s => distinctAadeshIds.Contains(Convert.ToString(s.aadeshid))).ToList();
            ViewBag.PageSizes = GetPageSizes(pageSize);

            var districtModel = _ihomeService.GetDistrict();
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;
            ViewBag.DistrictId = districtId;

            ViewBag.ServiceId = serviceId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");

            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "BOCWViewAadeshDetails",
                Controller = "EmployeeHome",
                ServiceId = serviceId,
                SearchText = search,
                EDistrictId = districtId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                StatusId = statusId,

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            ViewBag.Status = statusId;

            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                dtExportData.Columns.Add("Aadesh No.", typeof(System.String));
                dtExportData.Columns.Add("Financial Year", typeof(System.String));
                dtExportData.Columns.Add("Aadesh Generated Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Send For Payment Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                dtExportData.Columns.Add("Application Count", typeof(System.String));
                //dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                //dtExportData.Columns.Add("Email ID", typeof(System.String));
                //dtExportData.Columns.Add("Registration Date", typeof(System.DateTime));

                var distinctfinalResult = finalResult.DistinctBy(o => o.aadeshid).ToList();

                for (int i = 0; i < distinctfinalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr. No."] = Convert.ToString(distinctfinalResult[i].srno);
                    row["Aadesh No."] = Convert.ToString(distinctfinalResult[i].aadeshno);
                    row["Financial Year"] = Convert.ToString(distinctfinalResult[i].financialyear);
                    if (distinctfinalResult[i].aadeshcreateddate != null && Convert.ToString(distinctfinalResult[i].aadeshcreateddate != null) != "")
                        row["Aadesh Generated Date"] = Convert.ToDateTime(distinctfinalResult[i].aadeshcreateddate).ToString("MM/dd/yyyy");
                    else
                        row["Aadesh Generated Date"] = DBNull.Value;

                    if (distinctfinalResult[i].sendforpaymentdate != null && Convert.ToString(distinctfinalResult[i].sendforpaymentdate != null) != "")
                        row["Send For Payment Date"] = Convert.ToDateTime(distinctfinalResult[i].sendforpaymentdate).ToString("MM/dd/yyyy");
                    else
                        row["Send For Payment Date"] = DBNull.Value;

                    row["Total Sahay"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Sum(s => Convert.ToInt64(s.totalsahay)));
                    row["Application Count"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Count());
                    dtExportData.Rows.Add(row);
                }


                string tableName = "Total Aadesh";
                string fileName = "TotalAadesh" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }


            return View(finalResult);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateBOCWAadeshReport(string straadeshId, string export = "")
        {
            //var serviceid = Convert.ToInt32(empApproveApplication.serviceid);
            //var schemaName = Enum.GetName(typeof(schemaname), serviceid);
            long aadeshId = 0;
            if (straadeshId != null && straadeshId != "")
                aadeshId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(straadeshId)));

            long serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(serviceId));


            IEnumerable<EmpApplicationDetailsModel> modelList;
            modelList = await _ihomeService.GetAadeshDetailsbyAadeshid(aadeshId, schemaName);
            var finalResult = modelList.ToList();


            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                dtExportData.Columns.Add("e-Nirman Card No.", typeof(System.String));
                dtExportData.Columns.Add("Applicatio No.", typeof(System.String));
                dtExportData.Columns.Add("Applicant Name", typeof(System.String));
                dtExportData.Columns.Add("DateOfBirth", typeof(System.DateTime));
                dtExportData.Columns.Add("District", typeof(System.String));
                dtExportData.Columns.Add("Taluka", typeof(System.String));
                dtExportData.Columns.Add("Village", typeof(System.String));
                dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                dtExportData.Columns.Add("Application Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Approved Date", typeof(System.DateTime));
                if (serviceId == 7)
                {
                    dtExportData.Columns.Add("Degree", typeof(System.String));
                    dtExportData.Columns.Add("Course", typeof(System.String));
                }
                if (serviceId == 3)
                {
                    dtExportData.Columns.Add("Child Gender", typeof(System.String));
                }
                dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                dtExportData.Columns.Add("Payment Disbursement Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Transaction Date", typeof(System.DateTime));


                for (int i = 0; i < finalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr. No."] = Convert.ToString(finalResult[i].srno);
                    row["e-Nirman Card No."] = Convert.ToString(finalResult[i].enirmancardno);
                    row["Applicatio No."] = Convert.ToString(finalResult[i].applicationno);
                    row["Applicant Name"] = Convert.ToString(finalResult[i].name);
                    if (finalResult[i].dateofbirth != null && Convert.ToString(finalResult[i].dateofbirth != null) != "")
                        row["DateOfBirth"] = Convert.ToDateTime(finalResult[i].dateofbirth).ToString("MM/dd/yyyy");
                    else
                        row["DateOfBirth"] = DBNull.Value;
                    row["District"] = Convert.ToString(finalResult[i].cDistrictName);
                    row["Taluka"] = Convert.ToString(finalResult[i].ctalukaname);
                    row["Village"] = Convert.ToString(finalResult[i].cvillagename);
                    row["Mobile No."] = Convert.ToString(finalResult[i].mobileno);
                    if (finalResult[i].applicationdate != null && Convert.ToString(finalResult[i].applicationdate != null) != "")
                        row["Application Date"] = Convert.ToDateTime(finalResult[i].applicationdate).ToString("MM/dd/yyyy");
                    else
                        row["Application Date"] = DBNull.Value;
                    if (finalResult[i].approveddate != null && Convert.ToString(finalResult[i].approveddate != null) != "")
                        row["Approved Date"] = Convert.ToDateTime(finalResult[i].approveddate).ToString("MM/dd/yyyy");
                    else
                        row["Approved Date"] = DBNull.Value;

                    if (serviceId == 7)
                    {
                        row["Degree"] = Convert.ToString(finalResult[i].strdegree);
                        row["Course"] = Convert.ToString(finalResult[i].course);
                    }
                    if (serviceId == 3)
                    {
                        row["Child Gender"] = Convert.ToString(finalResult[i].childgender);
                    }

                    row["Total Sahay"] = Convert.ToString(finalResult[i].totalsahay);
                    if (finalResult[i].confirmuploadeddate != null && Convert.ToString(finalResult[i].confirmuploadeddate != null) != "")
                        row["Payment Disbursement Date"] = Convert.ToDateTime(finalResult[i].confirmuploadeddate).ToString("MM/dd/yyyy");
                    else
                        row["Payment Disbursement Date"] = DBNull.Value;
                    if (finalResult[i].transactiondate != null && Convert.ToString(finalResult[i].transactiondate != null) != "")
                        row["Transaction Date"] = Convert.ToDateTime(finalResult[i].transactiondate).ToString("MM/dd/yyyy");
                    else
                        row["Transaction Date"] = DBNull.Value;


                    dtExportData.Rows.Add(row);
                }


                string tableName = "Application Aadesh Details";
                string fileName = "ApplicationAadeshDetails" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }

            DataSet ds = new DataSet();
            if (modelList != null && modelList.Count() > 0)
            {
                DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                ds.Tables.Add(dtData);

                string rdlcFileName = "BOCWAadeshReport.rdlc";
                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                string reportName = "AadeshDetails";

                try
                {
                    byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

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
            }
            else
            {
                return null;
                //return new FileStreamResult(null, "application/pdf");
            }



        }

        [HttpGet]
        public async Task<IActionResult> DownloadBOCWAadeshApplicationCSV(string straadeshId)
        {
            long aadeshId = 0;
            if (straadeshId != null && straadeshId != "")
                aadeshId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(straadeshId)));

            long serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(serviceId));


            IEnumerable<EmpApplicationDetailsModel> modelList;
            modelList = await _ihomeService.GetAadeshDetailsbyAadeshid(aadeshId, schemaName);


            DataSet ds = new DataSet();
            if (modelList != null && modelList.Count() > 0)
            {
                DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                ds.Tables.Add(dtData);

                string rdlcFileName = "BOCWAadeshReport.rdlc";
                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                string reportName = "AadeshDetails";

                try
                {
                    byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

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
            }
            else
            {
                return null;
                //return new FileStreamResult(null, "application/pdf");
            }



        }

        [HttpGet]
        public async Task<IActionResult> BOCWCompletedAadeshForPayment(int? pageNo, int pageSize, long districtId = 0, string fromDate = "", string toDate = "", string? search = "", int statusId = 0, string strServiceId = "", string export = "")
        {

            long serviceId = 0;
            if (strServiceId != "")
            {
                serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            }

            if (serviceId == 0)
            {
                serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            }

            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);
            obj.AddUpdateClaim("ServiceId", Convert.ToString(serviceId));

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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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

            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var serviceModel = _ihomeService.GetServiceMasterByBeneficiaryId(benificiarytypeid);
            var services = serviceModel.Result.ToList();

            ViewBag.ServiceList = services;

            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<EmpApplicationDetailsModel> modelList;

            long logindistrictId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);


            modelList = await _ihomeService.BOCWViewAadeshDetails(pageNo, pageSize, districtId, dtFromDate, dtToDate, statusId, search, serviceId, schemaName);
            int recsCount = modelList.GroupBy(x => x.aadeshid).Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.DistinctBy(d => d.aadeshid).Skip(recSkip).Take(pageSize).ToList();



            var distinctAadeshIds = string.Join(",", finalResult.Select(p => p.aadeshid.ToString()));

            //finalResult = modelList.Where(x => x.aadeshid == finalResult[0].aadeshid).ToList();
            finalResult = modelList.Where(s => distinctAadeshIds.Contains(Convert.ToString(s.aadeshid))).ToList();
            ViewBag.PageSizes = GetPageSizes(pageSize);

            var districtModel = _ihomeService.GetDistrict();
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;
            ViewBag.DistrictId = districtId;

            ViewBag.ServiceId = serviceId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");

            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "BOCWCompletedAadeshForPayment",
                Controller = "EmployeeHome",
                ServiceId = serviceId,
                SearchText = search,
                EDistrictId = districtId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                StatusId = statusId,

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            ViewBag.Status = statusId;


            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                dtExportData.Columns.Add("Aadesh No.", typeof(System.String));
                dtExportData.Columns.Add("Financial Year", typeof(System.String));
                dtExportData.Columns.Add("Aadesh Generated Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Send For Payment Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                dtExportData.Columns.Add("Application Count", typeof(System.String));
                //dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                //dtExportData.Columns.Add("Email ID", typeof(System.String));
                //dtExportData.Columns.Add("Registration Date", typeof(System.DateTime));

                var distinctfinalResult = finalResult.DistinctBy(o => o.aadeshid).ToList();

                for (int i = 0; i < distinctfinalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr. No."] = Convert.ToString(distinctfinalResult[i].srno);
                    row["Aadesh No."] = Convert.ToString(distinctfinalResult[i].aadeshno);
                    row["Financial Year"] = Convert.ToString(distinctfinalResult[i].financialyear);
                    if (distinctfinalResult[i].aadeshcreateddate != null && Convert.ToString(distinctfinalResult[i].aadeshcreateddate != null) != "")
                        row["Aadesh Generated Date"] = Convert.ToDateTime(distinctfinalResult[i].aadeshcreateddate).ToString("MM/dd/yyyy");
                    else
                        row["Aadesh Generated Date"] = DBNull.Value;

                    if (distinctfinalResult[i].sendforpaymentdate != null && Convert.ToString(distinctfinalResult[i].sendforpaymentdate != null) != "")
                        row["Send For Payment Date"] = Convert.ToDateTime(distinctfinalResult[i].sendforpaymentdate).ToString("MM/dd/yyyy");
                    else
                        row["Send For Payment Date"] = DBNull.Value;

                    row["Total Sahay"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Sum(s => Convert.ToInt64(s.totalsahay)));
                    row["Application Count"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Count());
                    dtExportData.Rows.Add(row);
                }


                string tableName = "Total Aadesh";
                string fileName = "TotalAadesh" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }



            return View(finalResult);
        }

        [HttpGet]
        public async Task<IActionResult> BOCWSendForPayment(PaymentDetails paymentDetails)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (paymentDetails != null && paymentDetails.aadeshidlist != null && paymentDetails.aadeshidlist.Length > 0)
            {
                DataTable dtAadeshIds = new DataTable();
                dtAadeshIds.Columns.Add("aadeshid", typeof(long));

                for (int i = 0; i < paymentDetails.aadeshidlist.Count(); i++)
                {
                    dtAadeshIds.Rows.Add(Convert.ToInt64(paymentDetails.aadeshidlist[i]));
                }


                var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(paymentDetails.serviceid));
                long serviceId = Convert.ToInt64(paymentDetails.serviceid);

                string ipAddress = CommonUtils.GetLocalIPAddress();
                string hostName = CommonUtils.GetHostName();
                long postId = Convert.ToInt64(_claimPincipal.FindFirst("PostId").Value);

                var approveModel = await _ihomeService.BOCWSendForPayment(dtAadeshIds, serviceId, schemaName, postId, ipAddress, hostName);

                responseMessage.ErrorCode = 0;
                responseMessage.Msg = "Selected aadesh is successfully sent for payment process..!!";
            }
            else
            {
                responseMessage.ErrorCode = 1;
                responseMessage.Msg = "Somthing went wrong please try again..!!";
            }

            return Json(responseMessage);

        }

        #region GLWB Aadesh
        [HttpGet]
        public async Task<ContentResult> DownloadGLWBAadeshReport(EmpApproveApplication empApproveApplication)
        {
            var serviceid = Convert.ToInt32(empApproveApplication.serviceid);
            var schemaName = Enum.GetName(typeof(schemaname), serviceid);

            string applicationids = "";
            for (int i = 0; i < empApproveApplication.applicationidlist.Count(); i++)
            {
                applicationids += empApproveApplication.applicationidlist[i] + ",";
            }

            IEnumerable<EmpApplicationDetailsModel> modelList;


            modelList = await _ihomeService.GetAppDetailsByAppIdForGLWBViewAadesh(applicationids.Trim(','), schemaName);


            DataSet ds = new DataSet();
            if (modelList != null && modelList.Count() > 0)
            {
                DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                ds.Tables.Add(dtData);

                string rdlcFileName = "GLWBAadeshReport.rdlc";
                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                string reportName = "AadeshDetails";

                try
                {
                    byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

                    string strHTML = System.Text.Encoding.UTF8.GetString(reportData);

                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(strHTML);
                    var pdf = _generatePdf.GetPDF(strHTML);
                    var pdfStream = new System.IO.MemoryStream();
                    pdfStream.Write(pdf, 0, pdf.Length);
                    pdfStream.Position = 0;
                    string base64 = Convert.ToBase64String(pdf, 0, pdf.Length);

                    return Content(base64);
                    //return new FileStreamResult(pdfStream, "application/pdf");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                return null;
                //return new FileStreamResult(null, "application/pdf");
            }



        }

        [HttpGet]
        public async Task<IActionResult> GLWBViewAadeshDetails(int? pageNo, int pageSize, long districtId = 0, string fromDate = "", string toDate = "", int statusId = 0, string? search = "", long serviceId = 0, string export = "")
        {
            if (serviceId == 0)
            {
                serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            }

            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);
            obj.AddUpdateClaim("ServiceId", Convert.ToString(serviceId));

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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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

            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var serviceModel = _ihomeService.GetServiceMasterByBeneficiaryId(benificiarytypeid);
            var services = serviceModel.Result.ToList();

            ViewBag.ServiceList = services;

            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);

            if (serviceId == 24 || serviceId == 34)
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;

                long logindistrictId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);


                modelList = await _ihomeService.Glwb_Tsy_ViewAadeshDetails(pageNo, pageSize, dtFromDate, dtToDate, statusId, search, serviceId, schemaName);
                int recsCount = modelList.GroupBy(x => x.aadeshid).Count();
                int recSkip = (int)((pageNo - 1) * pageSize);

                var finalResult = modelList.DistinctBy(d => d.aadeshid).Skip(recSkip).Take(pageSize).ToList();



                var distinctAadeshIds = string.Join(",", finalResult.Select(p => p.aadeshid.ToString()));

                //finalResult = modelList.Where(x => x.aadeshid == finalResult[0].aadeshid).ToList();
                finalResult = modelList.Where(s => distinctAadeshIds.Contains(Convert.ToString(s.aadeshid))).ToList();
                ViewBag.PageSizes = GetPageSizes(pageSize);

                var districtModel = _ihomeService.GetDistrict();
                var districtList = districtModel.Result.ToList();
                ViewBag.DistrictList = districtList;
                ViewBag.DistrictId = districtId;

                ViewBag.ServiceId = serviceId;

                ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
                ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");

                int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
                var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

                var startdate = Convert.ToDateTime(dtFromDate);
                var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

                var enddate = Convert.ToDateTime(dtToDate);
                var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

                SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
                {
                    Action = "GLWBViewAadeshDetails",
                    Controller = "EmployeeHome",
                    ServiceId = serviceId,
                    SearchText = search,
                    EDistrictId = districtId,
                    StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                    EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                    StatusId = statusId,

                };
                ViewBag.SearchPager = SearchPager;
                ViewBag.ControllerName = SearchPager.Controller;
                ViewBag.ActionName = SearchPager.Action;
                ViewBag.Search = search;
                var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
                ViewBag.SchemeName = model.ServiceNameGujarati;
                ViewBag.Status = statusId;




                if (export.ToLower() == "export")
                {
                    DataTable dtExportData = new DataTable();
                    dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                    dtExportData.Columns.Add("Aadesh No.", typeof(System.String));
                    dtExportData.Columns.Add("Financial Year", typeof(System.String));
                    dtExportData.Columns.Add("Aadesh Generated Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Send For Payment Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                    dtExportData.Columns.Add("Application Count", typeof(System.String));

                    var distinctfinalResult = finalResult.DistinctBy(o => o.aadeshid).ToList();

                    for (int i = 0; i < distinctfinalResult.Count; i++)
                    {
                        var row = dtExportData.NewRow();
                        //row["Sr. No."] = Convert.ToString(distinctfinalResult[i].srno);
                        //row["Aadesh No."] = Convert.ToString(distinctfinalResult[i].aadeshno);
                        //row["Financial Year"] = Convert.ToString(distinctfinalResult[i].financialyear);
                        //if (distinctfinalResult[i].aadeshcreateddate != null && Convert.ToString(distinctfinalResult[i].aadeshcreateddate != null) != "")
                        //    row["Aadesh Generated Date"] = Convert.ToDateTime(distinctfinalResult[i].aadeshcreateddate).ToString("MM/dd/yyyy");
                        //else
                        //    row["Aadesh Generated Date"] = DBNull.Value;
                        //row["Total Sahay"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Sum(s => Convert.ToInt64(s.totalsahay)));
                        //row["Application Count"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Count());
                        row["Sr. No."] = Convert.ToString(distinctfinalResult[i].srno);
                        row["Aadesh No."] = Convert.ToString(distinctfinalResult[i].aadeshno);
                        row["Financial Year"] = Convert.ToString(distinctfinalResult[i].financialyear);
                        if (distinctfinalResult[i].aadeshcreateddate != null && Convert.ToString(distinctfinalResult[i].aadeshcreateddate != null) != "")
                            row["Aadesh Generated Date"] = Convert.ToDateTime(distinctfinalResult[i].aadeshcreateddate).ToString("MM/dd/yyyy");
                        else
                            row["Aadesh Generated Date"] = DBNull.Value;

                        if (distinctfinalResult[i].sendforpaymentdate != null && Convert.ToString(distinctfinalResult[i].sendforpaymentdate != null) != "")
                            row["Send For Payment Date"] = Convert.ToDateTime(distinctfinalResult[i].sendforpaymentdate).ToString("MM/dd/yyyy");
                        else
                            row["Send For Payment Date"] = DBNull.Value;

                        row["Total Sahay"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Sum(s => Convert.ToInt64(s.totalsahay)));
                        row["Application Count"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Count());

                        dtExportData.Rows.Add(row);
                    }


                    string tableName = "Total Aadesh";
                    string fileName = "TotalAadesh" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                    byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                    string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(result, contenType, fileName);
                }



                return View(finalResult);
            }
            else
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;

                long logindistrictId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);


                modelList = await _ihomeService.GLWBViewAadeshDetails(pageNo, pageSize, dtFromDate, dtToDate, statusId, search, serviceId, schemaName);
                int recsCount = modelList.GroupBy(x => x.aadeshid).Count();
                int recSkip = (int)((pageNo - 1) * pageSize);

                var finalResult = modelList.DistinctBy(d => d.aadeshid).Skip(recSkip).Take(pageSize).ToList();



                var distinctAadeshIds = string.Join(",", finalResult.Select(p => p.aadeshid.ToString()));

                //finalResult = modelList.Where(x => x.aadeshid == finalResult[0].aadeshid).ToList();
                finalResult = modelList.Where(s => distinctAadeshIds.Contains(Convert.ToString(s.aadeshid))).ToList();
                ViewBag.PageSizes = GetPageSizes(pageSize);

                var districtModel = _ihomeService.GetDistrict();
                var districtList = districtModel.Result.ToList();
                ViewBag.DistrictList = districtList;
                ViewBag.DistrictId = districtId;

                ViewBag.ServiceId = serviceId;

                ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
                ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");

                int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
                var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

                var startdate = Convert.ToDateTime(dtFromDate);
                var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

                var enddate = Convert.ToDateTime(dtToDate);
                var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

                SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
                {
                    Action = "GLWBViewAadeshDetails",
                    Controller = "EmployeeHome",
                    ServiceId = serviceId,
                    SearchText = search,
                    EDistrictId = districtId,
                    StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                    EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                    StatusId = statusId,

                };
                ViewBag.SearchPager = SearchPager;
                ViewBag.ControllerName = SearchPager.Controller;
                ViewBag.ActionName = SearchPager.Action;
                ViewBag.Search = search;
                var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
                ViewBag.SchemeName = model.ServiceNameGujarati;
                ViewBag.Status = statusId;


                if (export.ToLower() == "export")
                {
                    DataTable dtExportData = new DataTable();
                    dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                    dtExportData.Columns.Add("Aadesh No.", typeof(System.String));
                    dtExportData.Columns.Add("Financial Year", typeof(System.String));
                    dtExportData.Columns.Add("Aadesh Generated Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Send For Payment Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                    dtExportData.Columns.Add("Application Count", typeof(System.String));
                    //dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                    //dtExportData.Columns.Add("Email ID", typeof(System.String));
                    //dtExportData.Columns.Add("Registration Date", typeof(System.DateTime));

                    var distinctfinalResult = finalResult.DistinctBy(o => o.aadeshid).ToList();


                    for (int i = 0; i < distinctfinalResult.Count; i++)
                    {
                        var row = dtExportData.NewRow();
                        row["Sr. No."] = Convert.ToString(distinctfinalResult[i].srno);
                        row["Aadesh No."] = Convert.ToString(distinctfinalResult[i].aadeshno);
                        row["Financial Year"] = Convert.ToString(distinctfinalResult[i].financialyear);
                        if (distinctfinalResult[i].aadeshcreateddate != null && Convert.ToString(distinctfinalResult[i].aadeshcreateddate != null) != "")
                            row["Aadesh Generated Date"] = Convert.ToDateTime(distinctfinalResult[i].aadeshcreateddate).ToString("MM/dd/yyyy");
                        else
                            row["Aadesh Generated Date"] = DBNull.Value;

                        if (distinctfinalResult[i].sendforpaymentdate != null && Convert.ToString(distinctfinalResult[i].sendforpaymentdate != null) != "")
                            row["Send For Payment Date"] = Convert.ToDateTime(distinctfinalResult[i].sendforpaymentdate).ToString("MM/dd/yyyy");
                        else
                            row["Send For Payment Date"] = DBNull.Value;

                        row["Total Sahay"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Sum(s => Convert.ToInt64(s.totalsahay)));
                        row["Application Count"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Count());

                        dtExportData.Rows.Add(row);
                    }


                    string tableName = "Total Aadesh";
                    string fileName = "TotalAadesh" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                    byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                    string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(result, contenType, fileName);
                }

                return View(finalResult);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GenerateGLWBAadeshReport(string straadeshId, string strapplicationId, string export = "")
        {
            //var serviceid = Convert.ToInt32(empApproveApplication.serviceid);
            //var schemaName = Enum.GetName(typeof(schemaname), serviceid);
            long aadeshId = 0;
            long applicationId = 0;
            if (straadeshId != null && straadeshId != "")
                aadeshId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(straadeshId)));
            if (strapplicationId != null && strapplicationId != "")
                applicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strapplicationId)));

            long serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(serviceId));

            if (serviceId == 24)
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;
                modelList = await _ihomeService.GetGlwbTSYAadeshDetailsbyAadeshid(aadeshId, schemaName, applicationId);


                DataSet ds = new DataSet();
                if (modelList != null && modelList.Count() > 0)
                {
                    DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                    ds.Tables.Add(dtData);

                    string rdlcFileName = "GLWBHomeTownSadhyantikApproval.rdlc";
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    string reportName = "AadeshDetails";

                    try
                    {
                        byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

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
                }
                else
                {
                    return null;
                    //return new FileStreamResult(null, "application/pdf");
                }
            }
            else if (serviceId == 34)
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;
                modelList = await _ihomeService.GetGlwbTSYAadeshDetailsbyAadeshid(aadeshId, schemaName, applicationId);


                DataSet ds = new DataSet();
                if (modelList != null && modelList.Count() > 0)
                {
                    DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                    ds.Tables.Add(dtData);

                    string rdlcFileName = "GLWBAadeshReport.rdlc";
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    string reportName = "AadeshDetails";

                    try
                    {
                        byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

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
                }
                else
                {
                    return null;
                    //return new FileStreamResult(null, "application/pdf");
                }
            }
            else
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;
                modelList = await _ihomeService.GetGLWBAadeshDetailsbyAadeshid(aadeshId, schemaName, applicationId);
                var finalResult = modelList.ToList();


                if (export.ToLower() == "export")
                {
                    DataTable dtExportData = new DataTable();
                    dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                    //dtExportData.Columns.Add("e-Nirman Card No.", typeof(System.String));
                    dtExportData.Columns.Add("Applicatio No.", typeof(System.String));
                    dtExportData.Columns.Add("Applicant Name", typeof(System.String));
                    dtExportData.Columns.Add("DateOfBirth", typeof(System.DateTime));
                    dtExportData.Columns.Add("District", typeof(System.String));
                    dtExportData.Columns.Add("Taluka", typeof(System.String));
                    dtExportData.Columns.Add("Village", typeof(System.String));
                    dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                    dtExportData.Columns.Add("Application Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Approved Date", typeof(System.DateTime));
                    if (serviceId == 23)
                    {
                        dtExportData.Columns.Add("No of Male Child", typeof(System.Int32));
                        dtExportData.Columns.Add("No of Female Child", typeof(System.Int32));
                    }
                    dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                    dtExportData.Columns.Add("Payment Disbursement Date", typeof(System.DateTime));
                    dtExportData.Columns.Add("Transaction Date", typeof(System.DateTime));


                    for (int i = 0; i < finalResult.Count; i++)
                    {
                        var row = dtExportData.NewRow();
                        row["Sr. No."] = Convert.ToString(finalResult[i].srno);
                        //row["e-Nirman Card No."] = Convert.ToString(finalResult[i].enirmancardno);
                        row["Applicatio No."] = Convert.ToString(finalResult[i].applicationno);
                        row["Applicant Name"] = Convert.ToString(finalResult[i].name);
                        if (finalResult[i].dateofbirth != null && Convert.ToString(finalResult[i].dateofbirth != null) != "")
                            row["DateOfBirth"] = Convert.ToDateTime(finalResult[i].dateofbirth).ToString("MM/dd/yyyy");
                        else
                            row["DateOfBirth"] = DBNull.Value;
                        row["District"] = Convert.ToString(finalResult[i].cDistrictName);
                        row["Taluka"] = Convert.ToString(finalResult[i].ctalukaname);
                        row["Village"] = Convert.ToString(finalResult[i].cvillagename);
                        row["Mobile No."] = Convert.ToString(finalResult[i].mobileno);
                        if (finalResult[i].applicationdate != null && Convert.ToString(finalResult[i].applicationdate != null) != "")
                            row["Application Date"] = Convert.ToDateTime(finalResult[i].applicationdate).ToString("MM/dd/yyyy");
                        else
                            row["Application Date"] = DBNull.Value;
                        if (finalResult[i].approveddate != null && Convert.ToString(finalResult[i].approveddate != null) != "")
                            row["Approved Date"] = Convert.ToDateTime(finalResult[i].approveddate).ToString("MM/dd/yyyy");
                        else
                            row["Approved Date"] = DBNull.Value;
                        if (serviceId == 23)
                        {
                            row["No of Male Child"] = Convert.ToString(finalResult[i].noofchildemale);
                            row["No of Female Child"] = Convert.ToString(finalResult[i].noofchildfemale);
                        }
                        row["Total Sahay"] = Convert.ToString(finalResult[i].totalsahay);
                        if (finalResult[i].confirmuploadeddate != null && Convert.ToString(finalResult[i].confirmuploadeddate != null) != "")
                            row["Payment Disbursement Date"] = Convert.ToDateTime(finalResult[i].confirmuploadeddate).ToString("MM/dd/yyyy");
                        else
                            row["Payment Disbursement Date"] = DBNull.Value;
                        if (finalResult[i].transactiondate != null && Convert.ToString(finalResult[i].transactiondate != null) != "")
                            row["Transaction Date"] = Convert.ToDateTime(finalResult[i].transactiondate).ToString("MM/dd/yyyy");
                        else
                            row["Transaction Date"] = DBNull.Value;


                        dtExportData.Rows.Add(row);
                    }


                    string tableName = "Application Aadesh Details";
                    string fileName = "ApplicationAadeshDetails" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                    byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                    string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(result, contenType, fileName);
                }

                DataSet ds = new DataSet();
                if (modelList != null && modelList.Count() > 0)
                {
                    DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                    ds.Tables.Add(dtData);

                    string rdlcFileName = "GLWBAadeshReport.rdlc";
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    string reportName = "AadeshDetails";

                    try
                    {
                        byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

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
                }
                else
                {
                    return null;
                    //return new FileStreamResult(null, "application/pdf");
                }
            }


        }
        [HttpGet]
        public async Task<IActionResult> GenerateGLWBIndividualAadeshReport(string straadeshId, string strapplicationId)
        {
            //var serviceid = Convert.ToInt32(empApproveApplication.serviceid);
            //var schemaName = Enum.GetName(typeof(schemaname), serviceid);
            long aadeshId = 0;
            long applicationId = 0;
            if (straadeshId != null && straadeshId != "")
                aadeshId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(straadeshId)));
            if (strapplicationId != null && strapplicationId != "")
                applicationId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strapplicationId)));

            long serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(serviceId));

            if (serviceId == 34)
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;
                modelList = await _ihomeService.GetGlwbTSYAadeshDetailsbyAadeshid(aadeshId, schemaName, applicationId);


                DataSet ds = new DataSet();
                if (modelList != null && modelList.Count() > 0)
                {
                    DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                    ds.Tables.Add(dtData);

                    string rdlcFileName = "GLWBIndividualAadeshReport.rdlc";
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    string reportName = "AadeshDetails";

                    try
                    {
                        byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

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
                }
                else
                {
                    return null;
                    //return new FileStreamResult(null, "application/pdf");
                }

            }
            else
            {
                IEnumerable<EmpApplicationDetailsModel> modelList;
                modelList = await _ihomeService.GetGLWBAadeshDetailsbyAadeshid(aadeshId, schemaName, applicationId);


                DataSet ds = new DataSet();
                if (modelList != null && modelList.Count() > 0)
                {
                    DataTable dtData = CommonUtils.ToDataTable(modelList.ToList());
                    ds.Tables.Add(dtData);

                    string rdlcFileName = "GLWBIndividualAadeshReport.rdlc";
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    string reportName = "AadeshDetails";

                    try
                    {
                        byte[] reportData = CommonUtils.GenerateReportExcel(ds, rootPath, rdlcFileName, reportName, "HTML5", true, "");

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
                }
                else
                {
                    return null;
                    //return new FileStreamResult(null, "application/pdf");
                }
            }

        }

        [HttpGet]
        public async Task<IActionResult> GLWBCompletedAadeshForPayment(int? pageNo, int pageSize, string fromDate = "", string toDate = "", string? search = "", int statusId = 0, string strServiceId = "", string export = "")
        {

            long serviceId = 0;
            if (strServiceId != "")
            {
                serviceId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strServiceId)));
            }

            if (serviceId == 0)
            {
                serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            }

            Extensions obj = new Extensions(_config, _clientFactory, _httpContextAccessor);
            obj.AddUpdateClaim("ServiceId", Convert.ToString(serviceId));

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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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

            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var serviceModel = _ihomeService.GetServiceMasterByBeneficiaryId(benificiarytypeid);
            var services = serviceModel.Result.ToList();

            ViewBag.ServiceList = services;

            var schemaName = Enum.GetName(typeof(schemaname), serviceId);
            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<EmpApplicationDetailsModel> modelList;

            long logindistrictId = Convert.ToInt32(_claimPincipal.FindFirst("DistrictId").Value);


            modelList = await _ihomeService.GLWBViewAadeshDetails(pageNo, pageSize, dtFromDate, dtToDate, statusId, search, serviceId, schemaName);
            int recsCount = modelList.GroupBy(x => x.aadeshid).Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.DistinctBy(d => d.aadeshid).Skip(recSkip).Take(pageSize).ToList();



            var distinctAadeshIds = string.Join(",", finalResult.Select(p => p.aadeshid.ToString()));

            //finalResult = modelList.Where(x => x.aadeshid == finalResult[0].aadeshid).ToList();
            finalResult = modelList.Where(s => distinctAadeshIds.Contains(Convert.ToString(s.aadeshid))).ToList();
            ViewBag.PageSizes = GetPageSizes(pageSize);

            //var districtModel = _ihomeService.GetDistrict();
            //var districtList = districtModel.Result.ToList();
            //ViewBag.DistrictList = districtList;
            //ViewBag.DistrictId = districtId;

            ViewBag.ServiceId = serviceId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");

            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<EmpApplicationDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "GLWBCompletedAadeshForPayment",
                Controller = "EmployeeHome",
                ServiceId = serviceId,
                SearchText = search,
                //EDistrictId = districtId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(startdate),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),//Convert.ToDateTime(enddate),
                StatusId = statusId,

            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            ViewBag.Status = statusId;


            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                dtExportData.Columns.Add("Aadesh No.", typeof(System.String));
                dtExportData.Columns.Add("Financial Year", typeof(System.String));
                dtExportData.Columns.Add("Aadesh Generated Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Send For Payment Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                dtExportData.Columns.Add("Application Count", typeof(System.String));
                //dtExportData.Columns.Add("Mobile No.", typeof(System.String));
                //dtExportData.Columns.Add("Email ID", typeof(System.String));
                //dtExportData.Columns.Add("Registration Date", typeof(System.DateTime));

                var distinctfinalResult = finalResult.DistinctBy(o => o.aadeshid).ToList();


                for (int i = 0; i < distinctfinalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr. No."] = Convert.ToString(distinctfinalResult[i].srno);
                    row["Aadesh No."] = Convert.ToString(distinctfinalResult[i].aadeshno);
                    row["Financial Year"] = Convert.ToString(distinctfinalResult[i].financialyear);
                    if (distinctfinalResult[i].aadeshcreateddate != null && Convert.ToString(distinctfinalResult[i].aadeshcreateddate != null) != "")
                        row["Aadesh Generated Date"] = Convert.ToDateTime(distinctfinalResult[i].aadeshcreateddate).ToString("MM/dd/yyyy");
                    else
                        row["Aadesh Generated Date"] = DBNull.Value;

                    if (distinctfinalResult[i].sendforpaymentdate != null && Convert.ToString(distinctfinalResult[i].sendforpaymentdate != null) != "")
                        row["Send For Payment Date"] = Convert.ToDateTime(distinctfinalResult[i].sendforpaymentdate).ToString("MM/dd/yyyy");
                    else
                        row["Send For Payment Date"] = DBNull.Value;

                    row["Total Sahay"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Sum(s => Convert.ToInt64(s.totalsahay)));
                    row["Application Count"] = Convert.ToString(finalResult.Where(x => x.aadeshid == distinctfinalResult[i].aadeshid).Count());

                    dtExportData.Rows.Add(row);
                }


                string tableName = "Total Aadesh";
                string fileName = "TotalAadesh" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }


            return View(finalResult);
        }

        [HttpGet]
        public async Task<IActionResult> GLWBSendForPayment(PaymentDetails paymentDetails)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (paymentDetails != null && paymentDetails.aadeshidlist != null && paymentDetails.aadeshidlist.Length > 0)
            {
                DataTable dtAadeshIds = new DataTable();
                dtAadeshIds.Columns.Add("aadeshid", typeof(long));

                for (int i = 0; i < paymentDetails.aadeshidlist.Count(); i++)
                {
                    dtAadeshIds.Rows.Add(Convert.ToInt64(paymentDetails.aadeshidlist[i]));
                }


                var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(paymentDetails.serviceid));
                long serviceId = Convert.ToInt64(paymentDetails.serviceid);

                string ipAddress = CommonUtils.GetLocalIPAddress();
                string hostName = CommonUtils.GetHostName();
                long postId = Convert.ToInt64(_claimPincipal.FindFirst("PostId").Value);

                var approveModel = await _ihomeService.GLWBSendForPayment(dtAadeshIds, serviceId, schemaName, postId, ipAddress, hostName);

                responseMessage.ErrorCode = 0;
                responseMessage.Msg = "Selected aadesh is successfully sent for payment process..!!";
            }
            else
            {
                responseMessage.ErrorCode = 1;
                responseMessage.Msg = "Somthing went wrong please try again..!!";
            }

            return Json(responseMessage);

        }
        #endregion


        #region Payment Details

        #region BOCW
        [HttpGet]
        public async Task<IActionResult> BOCWPaymentHistory(int? pageNo, int pageSize, long districtId = 0, string fromDate = "", string toDate = "", string? search = "", long serviceId = 0, int statusId = 0, string export = "")
        {
            if (serviceId == 0)
            {
                serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            }



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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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
            }



            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var serviceModel = _ihomeService.GetServiceMasterByBeneficiaryId(benificiarytypeid);
            var services = serviceModel.Result.ToList();
            ViewBag.ServiceList = services;


            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);

            IEnumerable<PaymentDetailsModel> modelList;
            modelList = await _ihomeService.GetBOCWPaymentHistory(pageNo, pageSize, districtId, dtFromDate, dtToDate, search, serviceId, statusId);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();
            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                dtExportData.Columns.Add("Aadesh No.", typeof(System.String));
                dtExportData.Columns.Add("Applicatio No.", typeof(System.String));
                dtExportData.Columns.Add("Account Holder Name", typeof(System.String));
                dtExportData.Columns.Add("Account No.", typeof(System.String));
                dtExportData.Columns.Add("IFSC Code", typeof(System.String));
                dtExportData.Columns.Add("Payment Type", typeof(System.String));
                dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                dtExportData.Columns.Add("UTR No.", typeof(System.String));
                dtExportData.Columns.Add("Transaction Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Remarks", typeof(System.String));
                dtExportData.Columns.Add("Transaction Status", typeof(System.String));
                //dtExportData.Columns.Add("Transaction Date", typeof(System.DateTime));


                for (int i = 0; i < finalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr. No."] = Convert.ToString(finalResult[i].srno);
                    row["Aadesh No."] = Convert.ToString(finalResult[i].aadeshno);
                    row["Applicatio No."] = Convert.ToString(finalResult[i].applicationno);
                    row["Account Holder Name"] = Convert.ToString(finalResult[i].accountholdername);
                    row["Account No."] = Convert.ToString(finalResult[i].beneficiaryaccountno);
                    row["IFSC Code"] = Convert.ToString(finalResult[i].ifsccode);
                    row["Payment Type"] = Convert.ToString(finalResult[i].paymenttype);
                    row["Total Sahay"] = Convert.ToString(finalResult[i].amount);
                    row["UTR No."] = Convert.ToString(finalResult[i].utrno);
                    if (finalResult[i].transactiondate != null && Convert.ToString(finalResult[i].transactiondate != null) != "")
                        row["Transaction Date"] = Convert.ToDateTime(finalResult[i].transactiondate).ToString("MM/dd/yyyy");
                    else
                        row["Transaction Date"] = DBNull.Value;
                    row["Remarks"] = Convert.ToString(finalResult[i].remarks);
                    row["Transaction Status"] = Convert.ToString(finalResult[i].transactionstatus);



                    dtExportData.Rows.Add(row);
                }


                string tableName = "BOCW Payment History";
                string fileName = "BOCWPaymentHistory" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }


            ViewBag.PageSizes = GetPageSizes(pageSize);

            var districtModel = _ihomeService.GetDistrict();
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;
            ViewBag.DistrictId = districtId;

            ViewBag.ServiceId = serviceId;


            ViewBag.StartDate = dtFromDate != null ? Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy") : "";
            ViewBag.EndDate = dtToDate != null ? Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy") : "";

            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<PaymentDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "BOCWPaymentHistory",
                Controller = "EmployeeHome",
                ServiceId = serviceId,
                SearchText = search,
                EDistrictId = districtId,
                StartDate = dtFromDate != null ? Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy") : "",//Convert.ToDateTime(startdate),
                EndDate = dtToDate != null ? Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy") : "",//Convert.ToDateTime(enddate),
                StatusId = statusId,
            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            ViewBag.Status = statusId;

            return View(finalResult);
        }

        [HttpGet]
        public async Task<IActionResult> ViewUpdateBOCWBankDetails(PaymentDetailsModel paymentDetailsModel)
        {
            //PaymentDetailsModel paymentDetailsModel = new PaymentDetailsModel();
            //paymentDetailsModel.payinfoid = PayInfoId;
            //paymentDetailsModel.serviceid = ServiceId;
            //paymentDetailsModel.applicationid = ApplicationId;

            return PartialView("_UpdateBOCWBankDetails", paymentDetailsModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBOCWBankDetails(PaymentDetailsModel paymentDetailsModel)
        {

            if (paymentDetailsModel != null)
            {
                long userId = Convert.ToInt64(_claimPincipal.FindFirst("PostId").Value);
                var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(paymentDetailsModel.serviceid));
                var model = await _ihomeService.UpdateBOCWBankDetails(paymentDetailsModel.applicationid, paymentDetailsModel.serviceid, paymentDetailsModel.beneficiaryaccountno, paymentDetailsModel.ifsccode, paymentDetailsModel.remarks, userId, schemaName);

                if (model != null && model.Error == 1)
                {
                    TempData["Message"] = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                }
            }
            return RedirectToAction("BOCWPaymentHistory");
        }

        [HttpGet]
        public async Task<IActionResult> BOCWSendForPaymentFailedRecord(PaymentDetails paymentDetails)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (paymentDetails != null && paymentDetails.payinfoidlist != null && paymentDetails.payinfoidlist.Length > 0)
            {
                DataTable dtPayInfoIds = new DataTable();
                dtPayInfoIds.Columns.Add("applicationid", typeof(long));

                for (int i = 0; i < paymentDetails.payinfoidlist.Count(); i++)
                {
                    dtPayInfoIds.Rows.Add(Convert.ToInt64(paymentDetails.payinfoidlist[i]));
                }


                var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(paymentDetails.serviceid));
                long serviceId = Convert.ToInt64(paymentDetails.serviceid);

                string ipAddress = CommonUtils.GetLocalIPAddress();
                string hostName = CommonUtils.GetHostName();
                long postId = Convert.ToInt64(_claimPincipal.FindFirst("PostId").Value);

                var approveModel = await _ihomeService.BOCWSendForPaymentFailedRecord(dtPayInfoIds, serviceId, schemaName, postId, ipAddress, hostName);

                responseMessage.ErrorCode = 0;
                responseMessage.Msg = "Selected application is successfully sent for payment process..!!";
            }
            else
            {
                responseMessage.ErrorCode = 1;
                responseMessage.Msg = "Somthing went wrong please try again..!!";
            }

            return Json(responseMessage);

        }

        #endregion

        #region GLWB
        [HttpGet]
        public async Task<IActionResult> GLWBPaymentHistory(int? pageNo, int pageSize, string fromDate = "", string toDate = "", string? search = "", long serviceId = 0, int statusId = 0, string export = "")
        {
            if (serviceId == 0)
            {
                serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value);
            }


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

            int pageIndex = pageNo.HasValue ? Convert.ToInt32(pageNo) : 1;
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
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
            }



            var model = await _ihomeService.GetSchemeByServiceId(Convert.ToInt32(serviceId));
            ViewBag.SchemeName = model.ServiceNameGujarati;
            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var serviceModel = _ihomeService.GetServiceMasterByBeneficiaryId(benificiarytypeid);
            var services = serviceModel.Result.ToList();
            ViewBag.ServiceList = services;


            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);

            IEnumerable<PaymentDetailsModel> modelList;
            modelList = await _ihomeService.GetGLWBPaymentHistory(pageNo, pageSize, dtFromDate, dtToDate, search, serviceId, statusId);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();
            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr. No.", typeof(System.String));
                dtExportData.Columns.Add("Aadesh No.", typeof(System.String));
                dtExportData.Columns.Add("Applicatio No.", typeof(System.String));
                dtExportData.Columns.Add("Account Holder Name", typeof(System.String));
                dtExportData.Columns.Add("Account No.", typeof(System.String));
                dtExportData.Columns.Add("IFSC Code", typeof(System.String));
                dtExportData.Columns.Add("Payment Type", typeof(System.String));
                dtExportData.Columns.Add("Total Sahay", typeof(System.String));
                dtExportData.Columns.Add("UTR No.", typeof(System.String));
                dtExportData.Columns.Add("Transaction Date", typeof(System.DateTime));
                dtExportData.Columns.Add("Remarks", typeof(System.String));
                dtExportData.Columns.Add("Transaction Status", typeof(System.String));
                //dtExportData.Columns.Add("Transaction Date", typeof(System.DateTime));


                for (int i = 0; i < finalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr. No."] = Convert.ToString(finalResult[i].srno);
                    row["Aadesh No."] = Convert.ToString(finalResult[i].aadeshno);
                    row["Applicatio No."] = Convert.ToString(finalResult[i].applicationno);
                    row["Account Holder Name"] = Convert.ToString(finalResult[i].accountholdername);
                    row["Account No."] = Convert.ToString(finalResult[i].beneficiaryaccountno);
                    row["IFSC Code"] = Convert.ToString(finalResult[i].ifsccode);
                    row["Payment Type"] = Convert.ToString(finalResult[i].paymenttype);
                    row["Total Sahay"] = Convert.ToString(finalResult[i].amount);
                    row["UTR No."] = Convert.ToString(finalResult[i].utrno);
                    if (finalResult[i].transactiondate != null && Convert.ToString(finalResult[i].transactiondate != null) != "")
                        row["Transaction Date"] = Convert.ToDateTime(finalResult[i].transactiondate).ToString("MM/dd/yyyy");
                    else
                        row["Transaction Date"] = DBNull.Value;
                    row["Remarks"] = Convert.ToString(finalResult[i].remarks);
                    row["Transaction Status"] = Convert.ToString(finalResult[i].transactionstatus);



                    dtExportData.Rows.Add(row);
                }


                string tableName = "GLWB Payment History";
                string fileName = "GLWBPaymentHistory" + DateTime.Now.ToString("yyyyMMdd") + ".csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }
            ViewBag.PageSizes = GetPageSizes(pageSize);
            ViewBag.ServiceId = serviceId;
            ViewBag.StartDate = dtFromDate != null ? Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy") : "";
            ViewBag.EndDate = dtToDate != null ? Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy") : "";

            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<PaymentDetailsModel>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "GLWBPaymentHistory",
                Controller = "EmployeeHome",
                ServiceId = serviceId,
                SearchText = search,
                StartDate = dtFromDate != null ? Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy") : "",//Convert.ToDateTime(startdate),
                EndDate = dtToDate != null ? Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy") : "",//Convert.ToDateTime(enddate),
                StatusId = statusId,
            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            ViewBag.Status = statusId;

            return View(finalResult);
        }

        [HttpGet]
        public async Task<IActionResult> ViewUpdateGLWBBankDetails(PaymentDetailsModel paymentDetailsModel)
        {
            return PartialView("_UpdateGLWBBankDetails", paymentDetailsModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGLWBBankDetails(PaymentDetailsModel paymentDetailsModel)
        {

            if (paymentDetailsModel != null)
            {
                long userId = Convert.ToInt64(_claimPincipal.FindFirst("PostId").Value);
                var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(paymentDetailsModel.serviceid));
                var model = await _ihomeService.UpdateGLWBBankDetails(paymentDetailsModel.applicationid, paymentDetailsModel.serviceid, paymentDetailsModel.beneficiaryaccountno, paymentDetailsModel.ifsccode, paymentDetailsModel.remarks, userId, schemaName);

                if (model != null && model.Error == 1)
                {
                    TempData["Message"] = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString(model.Msg, Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                }
            }
            return RedirectToAction("GLWBPaymentHistory");
        }

        [HttpGet]
        public async Task<IActionResult> GLWBSendForPaymentFailedRecord(PaymentDetails paymentDetails)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (paymentDetails != null && paymentDetails.payinfoidlist != null && paymentDetails.payinfoidlist.Length > 0)
            {
                DataTable dtPayInfoIds = new DataTable();
                dtPayInfoIds.Columns.Add("applicationid", typeof(long));

                for (int i = 0; i < paymentDetails.payinfoidlist.Count(); i++)
                {
                    dtPayInfoIds.Rows.Add(Convert.ToInt64(paymentDetails.payinfoidlist[i]));
                }


                var schemaName = Enum.GetName(typeof(schemaname), Convert.ToInt32(paymentDetails.serviceid));
                long serviceId = Convert.ToInt64(paymentDetails.serviceid);

                string ipAddress = CommonUtils.GetLocalIPAddress();
                string hostName = CommonUtils.GetHostName();
                long postId = Convert.ToInt64(_claimPincipal.FindFirst("PostId").Value);

                var approveModel = await _ihomeService.GLWBSendForPaymentFailedRecord(dtPayInfoIds, serviceId, schemaName, postId, ipAddress, hostName);

                responseMessage.ErrorCode = 0;
                responseMessage.Msg = "Selected application is successfully sent for payment process..!!";
            }
            else
            {
                responseMessage.ErrorCode = 1;
                responseMessage.Msg = "Somthing went wrong please try again..!!";
            }

            return Json(responseMessage);

        }
        #endregion

        #endregion




    }
}
