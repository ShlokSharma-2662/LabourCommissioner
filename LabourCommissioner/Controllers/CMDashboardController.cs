using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using System.Web;

namespace LabourCommissioner.Controllers
{
    public class CMDashboardController : Controller
    {
        private readonly ICMDashboardService _cmDashboardService;
        private readonly ICMDashboardRepository _cmDashboardRepository;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _defaultPageSize;
        private readonly string _maximumPageSize;
        public CMDashboardController(IConfiguration config, IHttpClientFactory clientFactory, IWebHostEnvironment webHostEnvironment,
            ICMDashboardService cmDashboardService, ICMDashboardRepository cmDashboardRepository, IHttpContextAccessor httpContextAccessor)

        {
            _cmDashboardService = cmDashboardService ?? throw new ArgumentNullException(nameof(cmDashboardService));
            _cmDashboardRepository = cmDashboardRepository;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _config = config;
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _webHostEnvironment = webHostEnvironment;
            _defaultPageSize = _config["PageConfig:DefaultPageSize"];
            _maximumPageSize = _config["PageConfig:MaximumPageSize"];
        }

        [HttpGet]
        public async Task<IActionResult> ApplicationDetails(string strappYear, string strappMonth, int statusId)
        {
            long appYear = 0;
            long appMonth = 0;
            long serviceId = 0;
            if (strappYear != null && strappYear != "")
            {
                appYear = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strappYear)));
            }
            else
            {
                appYear = DateTime.Now.Year;
            }
            if (strappMonth != null && strappMonth != "")
            {
                appMonth = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strappMonth)));
            }
            else
            {
                appMonth = DateTime.Now.Month == 1 ? DateTime.Now.Month : DateTime.Now.Month - 1;

            }


            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);


            var yearModel = _cmDashboardService.GetYear();
            IEnumerable<System.Web.Mvc.SelectListItem> itemsyearModel = null;
            if (yearModel != null)
            {
                itemsyearModel = yearModel.Result.ToList().Select(u => new System.Web.Mvc.SelectListItem
                {
                    Text = u.Text,
                    Value = CommonUtils.Encrypt(u.Value)
                });


            }
            ViewBag.YearList = itemsyearModel;


            var monthModel = _cmDashboardService.GetMonth();
            IEnumerable<System.Web.Mvc.SelectListItem> itemsmonthModel = null;
            if (monthModel != null)
            {
                itemsmonthModel = monthModel.Result.ToList().Select(u => new System.Web.Mvc.SelectListItem
                {
                    Text = u.Text,
                    Value = CommonUtils.Encrypt(u.Value)
                });


            }
            ViewBag.MonthList = itemsmonthModel;



            var filledModel = await _cmDashboardService.GetCMDApplicationDetailslist(appYear, appMonth, benificiarytypeid, statusId);

            ViewBag.AppYearId = CommonUtils.Encrypt(Convert.ToString(appYear));
            ViewBag.AppMonthId = CommonUtils.Encrypt(Convert.ToString(appMonth));
            ViewBag.Status = statusId;

            return View(filledModel);
        }

        [HttpGet]
        public async Task<IActionResult> CMDApplication(string strApplicationId, string strappYear, string strappMonth, string strserviceId, string strAction, string strstatusId)
        {
            long ApplicationId = 0;
            long appYear = 0;
            long appMonth = 0;
            long serviceId = 0;
            long action = 0;
            long statusId = 0;
            if (strApplicationId != null && strApplicationId != "")
            {
                ApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            }
            if (strappYear != null && strappYear != "")
            {
                appYear = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strappYear)));
            }
            if (strappMonth != null && strappMonth != "")
            {
                appMonth = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strappMonth)));
            }
            if (strserviceId != null && strserviceId != "")
            {
                serviceId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strserviceId)));
            }
            if (strAction != null && strAction != "")
            {
                action = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strAction)));
            }
            if (strstatusId != null && strstatusId != "")
            {
                statusId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strstatusId)));
            }


            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var serviceModel = _cmDashboardService.GetServiceMasterByBeneficiaryIdforCMD(benificiarytypeid);
            IEnumerable<System.Web.Mvc.SelectListItem> itemsserviceModel = null;
            if (serviceModel != null)
            {
                itemsserviceModel = serviceModel.Result.ToList().Select(u => new System.Web.Mvc.SelectListItem
                {
                    Text = u.Text,
                    Value = CommonUtils.Encrypt(u.Value)
                });


            }
            ViewBag.ServiceList = itemsserviceModel;


            var yearModel = _cmDashboardService.GetYear();
            IEnumerable<System.Web.Mvc.SelectListItem> itemsyearModel = null;
            if (yearModel != null)
            {
                itemsyearModel = yearModel.Result.ToList().Select(u => new System.Web.Mvc.SelectListItem
                {
                    Text = u.Text,
                    Value = CommonUtils.Encrypt(u.Value)
                });


            }
            ViewBag.YearList = itemsyearModel;



            var monthModel = _cmDashboardService.GetMonth();
            IEnumerable<System.Web.Mvc.SelectListItem> itemsmonthModel = null;
            if (monthModel != null)
            {
                itemsmonthModel = monthModel.Result.ToList().Select(u => new System.Web.Mvc.SelectListItem
                {
                    Text = u.Text,
                    Value = CommonUtils.Encrypt(u.Value)
                });


            }
            ViewBag.MonthList = itemsmonthModel;



            var filledModel = await _cmDashboardService.GetCMDApplicationDetailsForInsert(ApplicationId, appYear, appMonth, serviceId);


            long submittedCount = filledModel.lstCMDApplicationDetails.Where(x => x.issubmitted == true).ToList().Count();






            ViewBag.AppYearId = CommonUtils.Encrypt(Convert.ToString(appYear));
            ViewBag.AppMonthId = CommonUtils.Encrypt(Convert.ToString(appMonth));
            ViewBag.ServiceId = CommonUtils.Encrypt(Convert.ToString(serviceId));
            ViewBag.Action = CommonUtils.Encrypt(Convert.ToString(action));
            ViewBag.Status = CommonUtils.Encrypt(Convert.ToString(statusId));
            if (submittedCount > 0 && action != 1)
            {
                ViewBag.Message = CommonUtils.ConcatString("Application is already submitted for Year " + appYear + " & Month " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(appMonth.ToString())) + " for selected scheme", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                return View();
            }

            return View(filledModel);
        }


        [HttpPost]
        public async Task<IActionResult> AddCMDApplication(CMDApplicationDetails cmdApplicationDetails)
        {

            try
            {

                //if (ModelState.IsValid)
                //{
                cmdApplicationDetails.appyear = cmdApplicationDetails.appyear;
                cmdApplicationDetails.appmonth = cmdApplicationDetails.appmonth;
                cmdApplicationDetails.serviceid = cmdApplicationDetails.serviceid;
                cmdApplicationDetails.userId = Convert.ToInt64(_claimPincipal.FindFirst("PostId").Value);
                cmdApplicationDetails.ipaddress = CommonUtils.GetLocalIPAddress();
                cmdApplicationDetails.hostname = CommonUtils.GetHostName();
                cmdApplicationDetails.applicationfrom = Convert.ToInt32(EnumLookup.applicationfrom.web);
                DataTable dtData = new DataTable();
                if (cmdApplicationDetails != null && cmdApplicationDetails.lstCMDApplicationDetails != null && cmdApplicationDetails.lstCMDApplicationDetails.Count() > 0)
                {
                    dtData = CommonUtils.ToDataTable(cmdApplicationDetails.lstCMDApplicationDetails);

                    ResponseMessage regResponse = await _cmDashboardService.AddUpdateCMDApplication(dtData, cmdApplicationDetails);
                    if (regResponse != null)
                    {
                        string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                        if (regResponse != null && regResponse.Error == 0)
                        {
                            TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                            ModelState.Clear();
                            Registration empEmpty = new Registration();
                            return RedirectToAction("ApplicationDetails", "CMDashboard");
                        }
                        else
                        {
                            TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                            ModelState.Clear();
                            Registration empEmpty = new Registration();
                            return RedirectToAction("AddCMDApplication", "CMDashboard", cmdApplicationDetails);
                        }
                    }
                    else
                    {
                        TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                        return RedirectToAction("AddCMDApplication", "CMDashboard", cmdApplicationDetails);
                    }
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString("No data found to insert.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                    return RedirectToAction("AddCMDApplication", "CMDashboard", cmdApplicationDetails);
                }



                //}
                //else
                //{
                //    TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try again.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                //    return RedirectToAction("AddCMDApplication", "CMDashboard", cmdApplicationDetails);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GenerateCMDApplicationCSVReport(string strApplicationId, string strappYear, string strappMonth, string strserviceId)
        {

            long ApplicationId = 0;
            long appYear = 0;
            long appMonth = 0;
            long serviceId = 0;

            if (strApplicationId != null && strApplicationId != "")
            {
                ApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));
            }
            if (strappYear != null && strappYear != "")
            {
                appYear = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strappYear)));
            }
            if (strappMonth != null && strappMonth != "")
            {
                appMonth = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strappMonth)));
            }
            if (strserviceId != null && strserviceId != "")
            {
                serviceId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strserviceId)));
            }




            var benificiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);

            //Task<CMDApplicationDetails> modelList;
            var modelList = await _cmDashboardService.GetCMDApplicationDetailsForInsert(ApplicationId, appYear, appMonth, serviceId);
            var finalResult = modelList.lstCMDApplicationDetails.ToList();


            DataTable dtExportData = new DataTable();
            dtExportData.Columns.Add("Sr.No.", typeof(System.Int32));
            dtExportData.Columns.Add("District Name", typeof(System.String));
            dtExportData.Columns.Add("Financial Target", typeof(System.Decimal));
            dtExportData.Columns.Add("Physical Target", typeof(System.Decimal));
            dtExportData.Columns.Add("Physical Achievement", typeof(System.Decimal));
            dtExportData.Columns.Add("Financial Achievement", typeof(System.Decimal));
            dtExportData.Columns.Add("Application Received", typeof(System.Decimal));
            dtExportData.Columns.Add("Sanction Dispose", typeof(System.Decimal));
            dtExportData.Columns.Add("Reject Dispose", typeof(System.Decimal));
            dtExportData.Columns.Add("Pending", typeof(System.Decimal));
            dtExportData.Columns.Add("Pending 90 days", typeof(System.Int64));
            dtExportData.Columns.Add("Dcode", typeof(System.Int32));
            dtExportData.Columns.Add("As on Date", typeof(System.DateTime));


            for (int i = 0; i < finalResult.Count; i++)
            {
                var row = dtExportData.NewRow();
                row["Sr.No."] = Convert.ToInt32(finalResult[i].srno);
                row["District Name"] = Convert.ToString(finalResult[i].districtname);
                row["Financial Target"] = Convert.ToDecimal(finalResult[i].fintarget).ToString("f2");
                row["Physical Target"] = Convert.ToDecimal(finalResult[i].phytarget).ToString("f2");
                row["Physical Achievement"] = Convert.ToDecimal(finalResult[i].phyachievement).ToString("f2");
                row["Financial Achievement"] = Convert.ToDecimal(finalResult[i].finachievement).ToString("f2");
                row["Application Received"] = Convert.ToDecimal(finalResult[i].appreceived).ToString("f2");
                row["Sanction Dispose"] = Convert.ToDecimal(finalResult[i].appsanction).ToString("f2");
                row["Reject Dispose"] = Convert.ToDecimal(finalResult[i].appreject).ToString("f2");
                row["Pending"] = Convert.ToDecimal(finalResult[i].apppending).ToString("f2");
                row["Pending 90 days"] = Convert.ToInt64(finalResult[i].appdaypending);
                row["Dcode"] = Convert.ToString(finalResult[i].dcode);
                if (finalResult[i].asondate != null && Convert.ToString(finalResult[i].asondate != null) != "")
                    row["As on Date"] = Convert.ToDateTime(finalResult[i].asondate).ToString("MM/dd/yyyy");
                else
                    row["As on Date"] = DBNull.Value;

                dtExportData.Rows.Add(row);
            }


            #region TOTAL Row
            decimal FinancialTarget = 0;
            decimal PhysicalTarget = 0;
            decimal PhysicalAchievement = 0;
            decimal FinancialAchievement = 0;
            decimal ApplicationReceived = 0;
            decimal SanctionDispose = 0;
            decimal RejectDispose = 0;
            decimal Pending = 0;
            decimal Pending90days = 0;


            FinancialTarget = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.fintarget));
            PhysicalTarget = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.phytarget));
            PhysicalAchievement = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.phyachievement));
            FinancialAchievement = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.finachievement));
            ApplicationReceived = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.appreceived));
            SanctionDispose = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.appsanction));
            RejectDispose = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.appreject));
            Pending = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.apppending));
            Pending90days = modelList.lstCMDApplicationDetails.ToList().Sum(s => Convert.ToDecimal(s.appdaypending));

            if (dtExportData != null && dtExportData.Rows.Count > 0)
            {
                var rowTotal = dtExportData.NewRow();
                rowTotal[0] = DBNull.Value;
                rowTotal[1] = "TOTAL";
                rowTotal[2] = FinancialTarget;
                rowTotal[3] = PhysicalTarget;
                rowTotal[4] = PhysicalAchievement;
                rowTotal[5] = FinancialAchievement;
                rowTotal[6] = ApplicationReceived;
                rowTotal[7] = SanctionDispose;
                rowTotal[8] = RejectDispose;
                rowTotal[9] = Pending;
                rowTotal[10] = Pending90days;
                rowTotal[11] = DBNull.Value;
                rowTotal[12] = DBNull.Value;
                dtExportData.Rows.Add(rowTotal);
            }

            #endregion

            string tableName = "CMD Application Details";
            string fileName = "CMDApplicationDetails.csv";


            byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
            string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(result, contenType, fileName);

        }
        [HttpGet]
        public async Task<IActionResult> CMDSubmitApplication(long appYear, long appMonth, long serviceId)
        {
            long userId = Convert.ToInt64(_claimPincipal.FindFirst("PostId").Value);
            string ipAddress = CommonUtils.GetLocalIPAddress();
            string hostName = CommonUtils.GetHostName();
            var model = await _cmDashboardService.CMDSubmitApplication(appYear, appMonth, serviceId, userId, ipAddress, hostName);
            return Json(new { data = model });
        }
    }
}
