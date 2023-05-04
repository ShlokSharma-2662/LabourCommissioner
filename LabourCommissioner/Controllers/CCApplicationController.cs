using com.test;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using LabourCommissioner.Views.Shared.Components.SearchBar;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using X.PagedList;
using static LabourCommissioner.Abstraction.EnumLookup;

namespace LabourCommissioner.Controllers
{
    public class CCApplicationController : Controller
    {
        private readonly ICCApplicationService _iccapplicationService;
        private readonly ICCApplicationRepository _ccapplicationRepository;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _defaultPageSize;
        private readonly string _maximumPageSize;
        private readonly string _paymentURL;
        private readonly string _returnURL;
        private readonly string _encryptedKey;

        public CCApplicationController(IConfiguration config, IHttpClientFactory clientFactory, IWebHostEnvironment webHostEnvironment,
            ICCApplicationService ccapplicationService, ICCApplicationRepository ccapplicationRepository, IHttpContextAccessor httpContextAccessor)

        {
            _iccapplicationService = ccapplicationService ?? throw new ArgumentNullException(nameof(ccapplicationService));
            _ccapplicationRepository = ccapplicationRepository;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _config = config;
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _webHostEnvironment = webHostEnvironment;
            _defaultPageSize = _config["PageConfig:DefaultPageSize"];
            _maximumPageSize = _config["PageConfig:MaximumPageSize"];
            _paymentURL = _config["CTPCred:PaymentURL"];
            _returnURL = _config["CTPCred:ReturnURL"];
            _encryptedKey = _config["CTPCred:EncryptedKey"];

        }



        [HttpGet]
        public async Task<IActionResult> CCApplication(string strApplicationId)
        {
            long ApplicationId;
            if (strApplicationId == null || strApplicationId == "")
            {
                ApplicationId = 0;
            }
            else
            {
                ApplicationId = Convert.ToInt32(CommonUtils.Decrypt(HttpUtility.UrlDecode(strApplicationId)));

            }
            var stateModel = _iccapplicationService.GetAllStates();
            var states = stateModel.Result.ToList();
            ViewBag.States = states;

            var districtModel = _iccapplicationService.GetDistrict();
            var district = districtModel.Result.ToList();
            ViewBag.District = district;

            var totalsahay = _iccapplicationService.GetTotalsahayByServiceID((int)EnumLookup.schemaname.cesscollection);
            ViewBag.TotalSahay = totalsahay.Result.totalsahay;

            if (ApplicationId > 0)
            {
                var filledModel = await _iccapplicationService.GetApplicationDetailsByAppId(ApplicationId);
                if (ApplicationId > 0 && filledModel != null)
                {
                    filledModel.isfilled = true;
                    var talukaModel = _iccapplicationService.GetTalukaByDistrictId(Convert.ToInt32(filledModel.districtid));
                    var taluka = talukaModel.Result.ToList();

                    var villageModel = _iccapplicationService.GetVillageByDistrictIdAndTalukaId(Convert.ToInt32(filledModel.districtid), Convert.ToInt32(filledModel.talukaid));
                    var village = villageModel.Result.ToList();

                    ViewBag.Taluka = taluka;
                    ViewBag.Village = village;

                    return View(filledModel);
                }
            }



            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ApplicationDetails(int? pageNo, int pageSize, long districtId = 0, long talukaId = 0, long villageId = 0, string fromDate = "", string toDate = "", int statusId = 0, string? search = "", string export = "")
        {
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

            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<CCApplicationDetails> modelList;

            modelList = await _iccapplicationService.GetCCApplicationDetails(pageNo, pageSize, districtId, talukaId, villageId, dtFromDate, dtToDate, statusId, search);
            int recsCount = modelList.Count();
            int recSkip = (int)((pageNo - 1) * pageSize);

            var finalResult = modelList.Skip(recSkip).Take(pageSize).ToList();

            ViewBag.PageSizes = GetPageSizes(pageSize);

            var districtModel = _iccapplicationService.GetDistrict();
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;
            ViewBag.DistrictId = districtId;

            var talukaModel = _iccapplicationService.GetTalukaByDistrictId(Convert.ToInt32(districtId));
            var talukaList = talukaModel.Result.ToList();
            ViewBag.TalukaList = talukaList;
            ViewBag.TalukaId = talukaId;

            var villageModel = _iccapplicationService.GetVillageByDistrictIdAndTalukaId(Convert.ToInt32(districtId), Convert.ToInt32(talukaId));
            var villageList = villageModel.Result.ToList();
            ViewBag.VillageList = villageList;
            ViewBag.VillageId = villageId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");


            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<CCApplicationDetails>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "ApplicationDetails",
                Controller = "CCApplication",
                SearchText = search,
                EDistrictId = districtId,
                EVillageId = villageId,
                ETalukaId = talukaId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),
                StatusId = statusId,
            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            ViewBag.Status = statusId;


            if (export.ToLower() == "export")
            {
                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr No.", typeof(System.String));
                dtExportData.Columns.Add("Application No.", typeof(System.String));
                dtExportData.Columns.Add("CESS Payer Name", typeof(System.String));
                dtExportData.Columns.Add("Date of Collection", typeof(System.String));
                dtExportData.Columns.Add("Cost of Construction", typeof(System.Decimal));
                dtExportData.Columns.Add("CESS Percentage", typeof(System.Decimal));
                dtExportData.Columns.Add("Total CESS", typeof(System.Decimal));
                dtExportData.Columns.Add("District", typeof(System.String));
                dtExportData.Columns.Add("Taluka", typeof(System.String));
                dtExportData.Columns.Add("Village", typeof(System.String));
                dtExportData.Columns.Add("PIN Code", typeof(System.String));
                dtExportData.Columns.Add("Address in English", typeof(System.String));
                dtExportData.Columns.Add("Address in Gujarati", typeof(System.String));
                dtExportData.Columns.Add("Submitted Date", typeof(System.String));






                for (int i = 0; i < finalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr No."] = Convert.ToString(finalResult[i].srno);
                    row["Application No."] = Convert.ToString(finalResult[i].applicationno);
                    row["CESS Payer Name"] = Convert.ToString(finalResult[i].cesspayername);
                    if (finalResult[i].dateofcollection != null && Convert.ToString(finalResult[i].dateofcollection != null) != "")
                        row["Date of Collection"] = Convert.ToDateTime(finalResult[i].dateofcollection).ToString("dd/MM/yyyy");
                    else
                        row["Date of Collection"] = DBNull.Value;
                    row["Cost of Construction"] = Convert.ToString(finalResult[i].costofconstruction);
                    row["CESS Percentage"] = Convert.ToString(finalResult[i].cesspercentage);
                    row["Total CESS"] = Convert.ToString(finalResult[i].totalcess);
                    row["District"] = Convert.ToString(finalResult[i].DistrictName);
                    row["Taluka"] = Convert.ToString(finalResult[i].TalukaName);
                    row["Village"] = Convert.ToString(finalResult[i].VillageName);
                    row["PIN Code"] = Convert.ToString(finalResult[i].pincode);
                    row["Address in English"] = Convert.ToString(finalResult[i].addressineng);
                    row["Address in Gujarati"] = Convert.ToString(finalResult[i].addressinguj);
                    if (finalResult[i].submitteddate != null && Convert.ToString(finalResult[i].submitteddate != null) != "")
                        row["Submitted Date"] = Convert.ToDateTime(finalResult[i].submitteddate).ToString("dd/MM/yyyy");
                    else
                        row["Submitted Date"] = DBNull.Value;


                    dtExportData.Rows.Add(row);
                }

                var schemaName = Enum.GetName(typeof(schemaname), (int)EnumLookup.schemaname.cesscollection);
                string tableName = "CESS_ApplicationDetails";
                string fileName = "CESS_ApplicationDetails.csv";


                byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName);
                string contenType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(result, contenType, fileName);
            }


            return View(finalResult);
        }


        private List<SelectListItem> GetPageSizes(int selectedPageSize = 50)
        {
            var pageSizes = new List<SelectListItem>();
            for (int lp = 50; lp <= 500; lp += 50)
            {
                if (lp == selectedPageSize)
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString(), true));
                else
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString()));
            }
            return pageSizes;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCCApplication(CCApplicationDetails ccApplicationDetails, string strApplicationNo)
        {

            try
            {

                if (ModelState.IsValid)
                {

                    string ApplicationNo = strApplicationNo;
                    ccApplicationDetails.serviceid = (int)EnumLookup.schemaname.cesscollection;
                    ccApplicationDetails.ipaddress = CommonUtils.GetLocalIPAddress();
                    ccApplicationDetails.hostname = CommonUtils.GetHostName();
                    ccApplicationDetails.registrationid = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
                    ccApplicationDetails.applicationfrom = Convert.ToInt32(EnumLookup.applicationfrom.web);
                    ccApplicationDetails.applicationno = strApplicationNo;

                    ResponseMessage regResponse = await _iccapplicationService.AddUpdateCCApplication(ccApplicationDetails);

                    if (regResponse != null)
                    {
                        string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                        if (regResponse != null && regResponse.Error == 0)
                        {
                            //CommonUtils commonFunction = new CommonUtils(_config);
                            //string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                            //var res = commonFunction.SendApplicationRegisteredMail(regResponse.ApplicantName, regResponse.email, regResponse.Msg, rootPath, "BOCWApproveRejectSendbackMail.htm", Convert.ToString(1));

                            //SMSModel modelSMSModel = await _iccapplicationService.GetSmsContentForService(ccApplicationDetails.serviceid, regResponse.Id, (int)EnumLookup.smstype.AppRequested, nameof(EnumLookup.schemaname.cesscollection), nameof(EnumLookup.tablename.applicationdetails));
                            //if (modelSMSModel != null)
                            //{
                            //    bool isSendSMS = commonFunction.SendServiceSMSWithDBLogINGujarati(modelSMSModel.SmsContent, modelSMSModel.MobileNo, modelSMSModel.TemplateId);
                            //    if (isSendSMS)
                            //    {
                            //        await _iccapplicationService.AddSMSLogs(modelSMSModel.MobileNo, serviceId, modelSMSModel.SmsContent, Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId")));
                            //    }
                            //}


                            var msg = regResponse.Msg;
                            TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                            return RedirectToAction("ApplicationDetails", "CCApplication");
                        }

                        else
                        {
                            TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                            ModelState.Clear();
                            CCApplicationDetails empEmpty = new CCApplicationDetails();
                            return RedirectToAction("AddCCApplication", "CCApplication");
                        }
                    }
                    else
                    {
                        TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
                        return RedirectToAction("ApplicationDetails", "CCApplication");
                    }

                }
                return RedirectToAction("AddCCApplication", "CCApplication");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CCCompletedAppForPayment(int? pageNo, int pageSize, string fromDate = "", string toDate = "", int statusId = 0, string? search = "", string export = "")
        {

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

            search = String.IsNullOrEmpty(search) ? "" : Convert.ToString(search);
            IEnumerable<CCApplicationDetails> modelList;

            modelList = await _iccapplicationService.GetCCCompletedAppForPayment(pageNo, pageSize, dtFromDate, dtToDate, statusId, search);

            int recsCount = modelList.GroupBy(x => x.paymentinfotransid).Count();
            int recSkip = (int)((pageNo - 1) * pageSize);


            var finalResult = modelList.DistinctBy(d => d.paymentinfotransid).Skip(recSkip).Take(pageSize).ToList();

            var distinctpaymentinfotransIds = string.Join(",", finalResult.Select(p => p.paymentinfotransid.ToString()));
            finalResult = modelList.Where(s => distinctpaymentinfotransIds.Contains(Convert.ToString(s.paymentinfotransid))).ToList();

            ViewBag.PageSizes = GetPageSizes(pageSize);

            //var districtModel = _iccapplicationService.GetDistrict();
            //var districtList = districtModel.Result.ToList();
            //ViewBag.DistrictList = districtList;
            //ViewBag.DistrictId = districtId;

            //var talukaModel = _iccapplicationService.GetTalukaByDistrictId(Convert.ToInt32(districtId));
            //var talukaList = talukaModel.Result.ToList();
            //ViewBag.TalukaList = talukaList;
            //ViewBag.TalukaId = talukaId;

            //var villageModel = _iccapplicationService.GetVillageByDistrictIdAndTalukaId(Convert.ToInt32(districtId), Convert.ToInt32(talukaId));
            //var villageList = villageModel.Result.ToList();
            //ViewBag.VillageList = villageList;
            //ViewBag.VillageId = villageId;

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");


            int TotalPageCount = modelList.Count() > 0 ? 1 : 0;
            var pagedList = new StaticPagedList<CCApplicationDetails>(modelList, pageIndex, pageSize, TotalPageCount);

            var startdate = Convert.ToDateTime(dtFromDate);
            var formatPageStartDate = String.Format("{0:dd/MM/yyyy}", startdate);

            var enddate = Convert.ToDateTime(dtToDate);
            var formatPageEndDate = String.Format("{0:dd/MM/yyyy}", enddate);

            SPager SearchPager = new SPager(recsCount, (int)pageNo, pageSize)
            {
                Action = "CCCompletedAppForPayment",
                Controller = "CCApplication",
                SearchText = search,
                //EDistrictId = districtId,
                //EVillageId = villageId,
                //ETalukaId = talukaId,
                StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy"),
                EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy"),
                StatusId = statusId,
            };
            ViewBag.SearchPager = SearchPager;
            ViewBag.ControllerName = SearchPager.Controller;
            ViewBag.ActionName = SearchPager.Action;
            ViewBag.Search = search;
            ViewBag.Status = statusId;

            return View(finalResult);
        }

        #region CTP Payment

        [HttpGet]
        public async Task<IActionResult> GetCTPPaymentSummaryPopUp(PaymentDetails paymentDetails)
        {
            ViewBag.PaymentDetails = paymentDetails;
            return PartialView("_CTPSummaryPopup");
        }

        [HttpGet]
        public async Task<IActionResult> SendCESSForCTPPayment(PaymentDetails paymentDetails)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            if (!string.IsNullOrEmpty(paymentDetails.fromdate) && !string.IsNullOrEmpty(paymentDetails.fromdate) && paymentDetails != null && paymentDetails.applicationidlist != null && paymentDetails.applicationidlist.Length > 0)
            {
                long registrationId = Convert.ToInt32(_claimPincipal.FindFirstValue("RegistrationId"));
                string ipAddress = CommonUtils.GetLocalIPAddress();
                string hostName = CommonUtils.GetHostName();

                DateTime? dtFromDate = null;
                DateTime? dtToDate = null;

                var finalDateTime = DateTime.ParseExact(paymentDetails.fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo = String.Format("{0:dd/MM/yyyy}", finalDateTime);
                dtFromDate = DateTime.ParseExact(demo, "dd/MM/yyyy", null);

                var finalDateTime1 = DateTime.ParseExact(paymentDetails.todate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var demo1 = String.Format("{0:dd/MM/yyyy}", finalDateTime1);
                dtToDate = DateTime.ParseExact(demo1, "dd/MM/yyyy", null);


                DataTable dtApplicationIds = new DataTable();
                dtApplicationIds.Columns.Add("applicationid", typeof(long));

                for (int i = 0; i < paymentDetails.applicationidlist.Count(); i++)
                {
                    dtApplicationIds.Rows.Add(Convert.ToInt64(paymentDetails.applicationidlist[i]));
                }

                IEnumerable<CTPPaymentDetails> modelList;
                modelList = await _iccapplicationService.InsertPaymentTransactionInfo(dtApplicationIds, registrationId, dtFromDate, dtToDate, ipAddress, hostName);
                var finalResult = modelList.ToList();



                if (finalResult != null && finalResult.Count() > 0)
                {
                    responseMessage.ErrorCode = 0;
                    responseMessage.Msg = "Record is ready for payment submission!!!";
                    //TempData["Message"] = CommonUtils.ConcatString("Record is ready for payment submission!!!", Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");
                    //string registrationid = Convert.ToString(finalResult[0].registrationid);
                    //string paymentinitdate = Convert.ToString(finalResult[0].paymentinitdate);
                    //string transactionid = Convert.ToString(finalResult[0].transactionid);
                    //string taxtype = Convert.ToString(finalResult[0].taxtype);
                    //string merchantid = Convert.ToString(finalResult[0].merchantid);
                    //string registrationno = Convert.ToString(finalResult[0].registrationno);
                    //string name = Convert.ToString(finalResult[0].name);
                    //string emailid = Convert.ToString(finalResult[0].emailid);
                    //string phoneno = Convert.ToString(finalResult[0].phoneno);
                    //string tokenno = Convert.ToString(finalResult[0].tokenno);
                    //string totalamount = Convert.ToString(finalResult[0].totalamount);
                    //string taxperiodfrom = Convert.ToString(finalResult[0].taxperiodfrom);
                    //string taxperiodto = Convert.ToString(finalResult[0].taxperiodto);
                    //string TransactionPurpose = "000-0000-00-000-00";


                    //string lstrToTransfer = "";
                    //lstrToTransfer += "User_id =" + registrationid + "|Init_date=" + paymentinitdate + "|Transaction_id=" + transactionid;
                    //lstrToTransfer += "|Tax_type=" + taxtype + "|MerchantId=" + merchantid + "|RegNo=" + registrationno + "|Name=" + name;
                    //lstrToTransfer += "|Email_id=" + emailid + "|Phone_no=" + phoneno + "|Token_no=" + tokenno;
                    //lstrToTransfer += "|Total_amount=" + totalamount + "|Tax_period_from=" + taxperiodfrom + "|Tax_period_to=" + taxperiodto;
                    //lstrToTransfer += "|Purpose=" + TransactionPurpose;

                    //string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    //rootPath = Path.Combine(rootPath + "/", _encryptedKey);

                    //var objAesEncryptBase = new AESEncryptBase("D:\\GIT Lab Projects\\LabourCommissioner\\LabourCommissioner\\LabourCommissioner\\wwwroot\\CTP Documents\\IBDeptencKey.key");
                    ////var objAesEncryptBase = new AESEncryptBase("D:\\GIT Lab Projects\\LabourCommissioner\\LabourCommissioner\\LabourCommissioner\\wwwroot\\CTP Documents\\KeyFile.text");
                    //string lstrEncToTransfer = objAesEncryptBase.encrypt(lstrToTransfer);

                    //string strReturnURL = _returnURL;
                    //string strENCReturnURL = objAesEncryptBase.encrypt(strReturnURL);


                    //NameValueCollection data = new NameValueCollection();
                    //data.Add("CTP_DATA", lstrEncToTransfer);
                    //data.Add("Dept_call", "first");
                    //data.Add("RU", strReturnURL);
                    //data.Add("DU", strENCReturnURL);
                    //ViewBag.CTPFormlist = CommonUtils.CreateCTPPOSTForm(_paymentURL, data);
                    //return View("CTPPaymentPage");
                }
                else
                {
                    responseMessage.ErrorCode = 1;
                    responseMessage.Msg = "No data found!!!";
                    //TempData["Message"] = CommonUtils.ConcatString("No data found!!!", Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");

                }
            }
            else
            {
                responseMessage.ErrorCode = 1;
                responseMessage.Msg = "Somthing went wrong please try after sometime.";
                //TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }


            return Json(responseMessage);
           //return RedirectToAction("CCCompletedAppForPayment");
            //return View("CCCompletedAppForPayment");

        }

        [HttpGet]
        public async Task<IActionResult> CTPMakePayment(string strPaymentinfoTransId)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            long paymentinfoTransId = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(strPaymentinfoTransId)));

            if (paymentinfoTransId > 0)
            {
                
                IEnumerable<CTPPaymentDetails> modelList;
                modelList = await _iccapplicationService.GetDataForCTPMakePayment(paymentinfoTransId);
                var finalResult = modelList.ToList();



                if (finalResult != null && finalResult.Count() > 0)
                {
                    string registrationid = Convert.ToString(finalResult[0].registrationid);
                    string paymentinitdate = Convert.ToString(finalResult[0].paymentinitdate);
                    string transactionid = Convert.ToString(finalResult[0].transactionid);
                    string taxtype = Convert.ToString(finalResult[0].taxtype);
                    string merchantid = Convert.ToString(finalResult[0].merchantid);
                    string registrationno = Convert.ToString(finalResult[0].registrationno);
                    string name = Convert.ToString(finalResult[0].name);
                    string emailid = Convert.ToString(finalResult[0].emailid);
                    string phoneno = Convert.ToString(finalResult[0].phoneno);
                    string tokenno = Convert.ToString(finalResult[0].tokenno);
                    string totalamount = Convert.ToString(finalResult[0].totalamount);
                    string taxperiodfrom = Convert.ToString(finalResult[0].taxperiodfrom);
                    string taxperiodto = Convert.ToString(finalResult[0].taxperiodto);
                    string TransactionPurpose = "000-0000-00-000-00";


                    string lstrToTransfer = "";
                    lstrToTransfer += "User_id =" + registrationid + "|Init_date=" + paymentinitdate + "|Transaction_id=" + transactionid;
                    lstrToTransfer += "|Tax_type=" + taxtype + "|MerchantId=" + merchantid + "|RegNo=" + registrationno + "|Name=" + name;
                    lstrToTransfer += "|Email_id=" + emailid + "|Phone_no=" + phoneno + "|Token_no=" + tokenno;
                    lstrToTransfer += "|Total_amount=" + totalamount + "|Tax_period_from=" + taxperiodfrom + "|Tax_period_to=" + taxperiodto;
                    lstrToTransfer += "|Purpose=" + TransactionPurpose;

                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    rootPath = Path.Combine(rootPath + "/", _encryptedKey);

                    var objAesEncryptBase = new AESEncryptBase("D:\\GIT Lab Projects\\LabourCommissioner\\LabourCommissioner\\LabourCommissioner\\wwwroot\\CTP Documents\\IBDeptencKey.key");
                    //var objAesEncryptBase = new AESEncryptBase("D:\\GIT Lab Projects\\LabourCommissioner\\LabourCommissioner\\LabourCommissioner\\wwwroot\\CTP Documents\\KeyFile.text");
                    string lstrEncToTransfer = objAesEncryptBase.encrypt(lstrToTransfer);

                    string strReturnURL = _returnURL;
                    string strENCReturnURL = objAesEncryptBase.encrypt(strReturnURL);


                    NameValueCollection data = new NameValueCollection();
                    data.Add("CTP_DATA", lstrEncToTransfer);
                    data.Add("Dept_call", "first");
                    data.Add("RU", strReturnURL);
                    data.Add("DU", strENCReturnURL);
                    ViewBag.CTPFormlist = CommonUtils.CreateCTPPOSTForm(_paymentURL, data);
                    return View("CTPPaymentPage");
                }
                else
                {
                    responseMessage.ErrorCode = 1;
                    responseMessage.Msg = "No data found!!!";
                    //TempData["Message"] = CommonUtils.ConcatString("No data found!!!", Convert.ToString((int)EnumLookup.ResponseMsgType.info), "||");

                }
            }
            else
            {
                responseMessage.ErrorCode = 1;
                responseMessage.Msg = "Somthing went wrong please try after sometime.";
                //TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            }


            return Json(responseMessage);
            //return RedirectToAction("CCCompletedAppForPayment");
            //return View("CCCompletedAppForPayment");

        }

        [HttpPost]
        public async Task<IActionResult> CTPPaymentReturnResponse()
        {
            try
            {

                #region FirstCall
                string encryptedResponse = "";
                string decryptedResponse = "";

                if (HttpContext.Request != null && HttpContext.Request.Form.ContainsKey("enc_data"))
                {
                    encryptedResponse = Convert.ToString(HttpContext.Request.Form["enc_data"]);

                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    rootPath = Path.Combine(rootPath + "/", _encryptedKey);
                    var objAesEncryptBase = new AESEncryptBase(rootPath);
                    decryptedResponse = objAesEncryptBase.decrypt(encryptedResponse);

                    string[] splitResponse = decryptedResponse.Split('|');

                    if (splitResponse.Length > 0)
                    {
                        string User_id = "";
                        string Token_No = "";
                        string Transaction_id = "";
                        string Init_date = "";
                        var valueDictionary = new Dictionary<string, string>();
                        foreach (string a in splitResponse)
                        {
                            string[] b = a.Split('=');
                            valueDictionary[b[0]] = b[1];
                        }

                        if (valueDictionary.Count() > 0)
                        {
                            if (valueDictionary.ContainsKey("User_id"))
                            {
                                User_id = Convert.ToString(valueDictionary.GetValueOrDefault("User_id"));
                            }
                            if (valueDictionary.ContainsKey("Token_No"))
                            {
                                Token_No = Convert.ToString(valueDictionary.GetValueOrDefault("Token_No"));
                            }
                            if (valueDictionary.ContainsKey("Transaction_id"))
                            {
                                Transaction_id = Convert.ToString(valueDictionary.GetValueOrDefault("Transaction_id"));
                            }
                            if (valueDictionary.ContainsKey("Init_date"))
                            {
                                Init_date = Convert.ToString(valueDictionary.GetValueOrDefault("Init_date"));
                            }

                            IEnumerable<CTPPaymentDetails> modelList;
                            modelList = await _iccapplicationService.CheckTransactionTokenExistorNot(Convert.ToInt64(User_id), Convert.ToInt64(Token_No), Transaction_id);

                            if (modelList != null && modelList.Count() > 0)
                            {
                                NameValueCollection data = new NameValueCollection();
                                data.Add("Dept_call", "second");
                                data.Add("token_valid", "true");
                                data.Add("Transaction_id", Transaction_id);
                                data.Add("User_id", User_id);

                                string lstrToTransfer = "Dept_Call=second|token_valid=true|Transaction_id=" + Transaction_id + "|User_id=" + User_id;
                                var objAesEncryptBaseSecond = new AESEncryptBase(rootPath);
                                string lstrEncToTransfer = objAesEncryptBaseSecond.encrypt(lstrToTransfer);
                                data.Add("CTP_DATA", lstrEncToTransfer);

                                ViewBag.CTPFormlist = CommonUtils.CreateCTPPOSTForm(_paymentURL, data);
                                return View("CTPPaymentPage");
                            }
                        }
                    }


                }
                #endregion

                #region SecondCall

                if (HttpContext.Request != null && HttpContext.Request.Form.ContainsKey("encdata"))
                {
                    encryptedResponse = Convert.ToString(HttpContext.Request.Form["encdata"]);

                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    rootPath = Path.Combine(rootPath + "/", _encryptedKey);
                    var objAesEncryptBase = new AESEncryptBase(rootPath);
                    decryptedResponse = objAesEncryptBase.decrypt(encryptedResponse);

                    string[] splitResponse = decryptedResponse.Split('|');

                    if (splitResponse.Length > 0)
                    {
                        Hashtable ctpResponse = new Hashtable();
                        for (int pcnt = 0; pcnt < splitResponse.Length; pcnt++)
                        {
                            ctpResponse.Add(splitResponse[pcnt].Split('=')[0], splitResponse[pcnt].Split('=')[1]);
                        }

                        if (ctpResponse.Count > 0)
                        {
                            long UserId = Convert.ToInt64(Convert.ToString(ctpResponse["User_id"]) != null ? Convert.ToInt64(ctpResponse["User_id"]) : 0);
                            long TranId = Convert.ToInt64(Convert.ToString(ctpResponse["Transaction_id"]) != null ? Convert.ToInt64(ctpResponse["Transaction_id"]) : 0);
                            string RegNo = Convert.ToString(ctpResponse["Regn_no"]);
                            string BankRefNo = Convert.ToString(ctpResponse["Bank_ref_no"]);
                            string BankName = Convert.ToString(ctpResponse["Bank_name"]);
                            long DlrRefNo = Convert.ToInt64(Convert.ToString(ctpResponse["Dlr_ref_no"]) != null ? Convert.ToInt64(ctpResponse["Dlr_ref_no"]) : 0);
                            string CIN = Convert.ToString(ctpResponse["Cin"]);
                            string Amount = Convert.ToString(ctpResponse["Amount"]);
                            string PaymentDate = Convert.ToString(ctpResponse["Pymnt_date"]);
                            string Status = Convert.ToString(ctpResponse["Status"]);
                            string StatusDesc = Convert.ToString(ctpResponse["Status_desc"]);

                            DateTime? dtPaymentDate = null;
                            if (!string.IsNullOrEmpty(PaymentDate))
                            {
                                var finalDateTime = DateTime.ParseExact(PaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var demo = String.Format("{0:dd/MM/yyyy}", finalDateTime);
                                dtPaymentDate = DateTime.ParseExact(demo, "dd/MM/yyyy", null);
                            }
                            
                            IEnumerable<CTPPaymentDetails> modelList;
                            modelList = await _iccapplicationService.UpdatePaymentTransactionInfo(UserId, TranId, RegNo, BankRefNo, BankName, DlrRefNo, CIN, Amount, dtPaymentDate,
                                Status, StatusDesc);
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        #endregion

    }
}
