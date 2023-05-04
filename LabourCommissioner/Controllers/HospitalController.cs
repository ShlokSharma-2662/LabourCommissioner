using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.WebPages;
using ClosedXML.Excel;
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
    public class HospitalController : Controller
    {
        private readonly IHospitalService _ihomeService;
        private readonly IHospitalRepository _homeRepository;
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
        public HospitalController(IStringLocalizer<HomeController> localizer, IConfiguration config,
            IHttpClientFactory clientFactory, IWebHostEnvironment webHostEnvironment, 
            IHtmlLocalizer<HomeController> htmlLocalizer, IHospitalService homeService,
            IHospitalRepository homeRepository, ISchemeService schemeService, ISchemeUserServices schemeUserServices,
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




        [HttpGet]
        public async Task<IActionResult> GLWBHospitalApplicationDetails(int? pageNo, int pageSize, long districtId = 0, long talukaId = 0, long villageId = 0, string fromDate = "", string toDate = "", int statusId = 1, string? search = "", string strServiceId = "", string export = "", long divisionId = 0, int hodid = 0)
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

            modelList = await _ihomeService.GLWB_Hospital_GetApplicationDetailsList(pageNo, pageSize, divisionId, talukaId, districtId, dtFromDate, dtToDate, statusId, search, postId, serviceId, "", schemaName);
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
                dtExportData.Columns.Add("Total Sahay", typeof(System.Decimal));

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
                    row["Total Sahay"] = Convert.ToString(finalResult[i].totalsahay);
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


    }
}
