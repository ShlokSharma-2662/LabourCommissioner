using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace LabourCommissioner.Controllers
{
    public class BOCWServiceRoutineController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBOCWServiceRoutineService _iBOCWServiceRoutineService;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly UserCookies cookies;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly string _BOCWRequestPath;
        private readonly string _BOCWResponsePath;
        private readonly string _BOCWCorporateId;
        public BOCWServiceRoutineController(IWebHostEnvironment webHostEnvironment, IBOCWServiceRoutineService iBOCWServiceRoutineService, IConfiguration config, IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _iBOCWServiceRoutineService = iBOCWServiceRoutineService ?? throw new ArgumentNullException(nameof(_iBOCWServiceRoutineService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _BOCWRequestPath = _config["SBISFTP:BOCWRequestPath"];
            _BOCWResponsePath = _config["SBISFTP:BOCWResponsePath"];
            _BOCWCorporateId = _config["SBISFTP:BOCWCorporateId"];

        }

        public async Task BOCWUploadFileOnServer(string fromDate = "", string toDate = "")
        {

            Log.Information("*************** START : BOCWUploadFileOnServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");


            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now.AddDays(-1);
                var startDate = new DateTime(now.Year, now.Month, now.Day);
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
                DateTime now = DateTime.Now;
                var endDate = new DateTime(now.Year, now.Month, now.Day);
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



            IEnumerable<AadeshPaymentDetailsModel> modelList;

            modelList = await _iBOCWServiceRoutineService.BOCWGetAadeshDataForRoutine(dtFromDate, dtToDate);
            var finalResult = modelList.ToList();
            if (modelList != null && modelList.Count() > 0)
            {

                DataTable dtExportData = new DataTable();
                dtExportData.Columns.Add("Sr No", typeof(System.String));
                dtExportData.Columns.Add("Name", typeof(System.String));
                dtExportData.Columns.Add("Place", typeof(System.String));
                dtExportData.Columns.Add("Application No", typeof(System.String));
                dtExportData.Columns.Add("Beneficiary Account No", typeof(System.String));
                dtExportData.Columns.Add("IFSC Code", typeof(System.String));
                dtExportData.Columns.Add("Amount", typeof(System.Decimal));
                dtExportData.Columns.Add("Board Name", typeof(System.String));
                dtExportData.Columns.Add("Board Account Number From Which Amount Transfer", typeof(System.String));
                dtExportData.Columns.Add("Type", typeof(System.String));
                dtExportData.Columns.Add("Unique Id", typeof(System.String));
                dtExportData.Columns.Add("Scheme Name", typeof(System.String));

                for (int i = 0; i < finalResult.Count; i++)
                {
                    var row = dtExportData.NewRow();
                    row["Sr No"] = Convert.ToString(finalResult[i].payinfoid);
                    row["Name"] = Convert.ToString(finalResult[i].applicantname);
                    row["Place"] = Convert.ToString(finalResult[i].districtname);
                    row["Application No"] = Convert.ToString(finalResult[i].applicationno);
                    row["Beneficiary Account No"] = Convert.ToString(finalResult[i].beneficiaryaccountno);
                    row["IFSC Code"] = Convert.ToString(finalResult[i].ifsccode);
                    row["Amount"] = Convert.ToString(finalResult[i].amount);
                    row["Board Name"] = Convert.ToString(finalResult[i].boardname);
                    row["Board Account Number From Which Amount Transfer"] = Convert.ToString(finalResult[i].boardaccountno);
                    row["Type"] = Convert.ToString(finalResult[i].accounttype);
                    row["Unique Id"] = Convert.ToString(finalResult[i].uniqueid);
                    row["Scheme Name"] = Convert.ToString(finalResult[i].schemaname);

                    dtExportData.Rows.Add(row);
                }
                string CorporateId = "";
                string SchemaName = "";
                string FWDistrictId = "";
                if (finalResult != null && finalResult.Count() > 0)
                {
                    CorporateId = Convert.ToString(finalResult[0].corporateid);
                    SchemaName = Convert.ToString(finalResult[0].schemaname).ToUpper();
                    FWDistrictId = Convert.ToString(finalResult[0].fwdistrictid).ToUpper();
                }

                string tableName = "AadeshApplicationDetails";
                //byte[] result = CommonUtils.ExportToCSV(dtExportData, tableName, false);
                byte[] result = CommonUtils.ExportToCSVUsingComma(dtExportData);

                string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                string fileName = CorporateId + "_" + SchemaName + "_" + FWDistrictId + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                rootPath = Path.Combine(rootPath + "/", _BOCWRequestPath);

                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }

                string filePath = Path.Combine(rootPath + "/", fileName);
                System.IO.File.WriteAllBytes(filePath, result);

                Log.Information("GeneratedFileForUploadToServer : " + filePath + " ");



                CommonUtils fileuploadFunction = new CommonUtils(_config);
                int fileuploadresult = fileuploadFunction.BOCWUploadFileOnServer(filePath);

                //int fileuploadresult = 1;

                if (fileuploadresult == 1)
                {
                    #region FileUpload Status



                    string payinfoids = "";
                    for (int i = 0; i < finalResult.Count; i++)
                    {
                        payinfoids += finalResult[i].payinfoid + ",";
                    }

                    IEnumerable<AadeshPaymentDetailsModel> updatemodelList;

                    updatemodelList = await _iBOCWServiceRoutineService.UpdateBOCWPaymentInfo(payinfoids.Trim(','), fileName, (int)EnumLookup.AadeshPaymentInfoStatus.Approve, (int)EnumLookup.AadeshPaymentInfoStatus.Pending);

                    Log.Information("Update FileName &  Confirm Upload Status in  bocwpaymentinfo (PaymentInfoIds): " + payinfoids + " ");

                    #endregion
                }
            }

            Log.Information("*************** END : BOCWUploadFileOnServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");

        }

        public async Task BOCWFetchReturnCSVFileFromServer()
        {

            Log.Information("*************** START : BOCWFetchReturnCSVFileFromServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
            try
            {
                IEnumerable<AadeshPaymentDetailsModel> modelList;

                modelList = await _iBOCWServiceRoutineService.BOCWGetAadeshDataForFetchReturnCSVFile();
                var finalResult = modelList.DistinctBy(d => d.filename).ToList();

                if (finalResult != null && finalResult.Count() > 0)
                {
                    string distinctFiles = string.Join(",", finalResult.Select(p => p.filename.ToString()));
                    string rootPath = $"{this._webHostEnvironment.WebRootPath}";
                    string[] fileToRead = distinctFiles.Split(",");

                    string filesToRead = "";
                    for (int i = 0; i < fileToRead.Length; i++)
                    {
                        filesToRead += fileToRead[i].ToString() + ",";
                    }

                    Log.Information("FilesToRead : " + filesToRead + " ");

                    CommonUtils filereadFunction = new CommonUtils(_config);
                    int fileuploadresult = filereadFunction.BOCWReadFileFromServer(fileToRead, rootPath);


                    #region Read CSV From Local Server And Update Database

                    Log.Information("*************** START : Read CSV From Local Server And Update Database (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
                    rootPath = Path.Combine(rootPath + "/", _BOCWResponsePath);
                    foreach (var item in fileToRead)
                    {
                        string fileName = _BOCWCorporateId + "_REV_" + item;
                        CommonUtils localFileReadFunction = new CommonUtils(_config);
                        DataTable dtData = localFileReadFunction.BOCWReadCSVFileFromLocalServer(rootPath, fileName);


                        if (dtData != null && dtData.Rows.Count > 0)
                        {

                            ResponseMessage responseMessage = new ResponseMessage();
                            responseMessage = await _iBOCWServiceRoutineService.SaveBOCWPaymentResponse(dtData, CommonUtils.GetLocalIPAddress(), CommonUtils.GetHostName());
                        }

                        Log.Information("*************** END : Read CSV From Local Server And Update Database (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");

                    }

                    #endregion



                }
            }
            catch (Exception e)
            {
                //ServerWriteToFile("Copy File From Server Exception - " + DateTime.Now + " : " + e.Message, "ServerException");
            }

            Log.Information("*************** END : BOCWFetchReturnCSVFileFromServer (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }

        public void TestServiceRoutin()
        {
            Log.Information("*************** START : TestServiceRoutin (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
            Log.Information("*************** END : TestServiceRoutin (Date : " + DateTime.Now.ToString("dd/MM/yyyy hhmmss") + " )  ***************");
        }


        [HttpGet]
        public async Task<IActionResult> BOCWManualPaymentDetails(string fromDate = "", string toDate = "")
        {
            ViewBag.StartDate = fromDate;
            ViewBag.EndDate = toDate;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> BOCWPaymentDisbursement(string fromDate = "", string toDate = "")
        {
            DateTime? dtFromDate = null;
            DateTime? dtToDate = null;
            if (string.IsNullOrEmpty(fromDate))
            {
                DateTime now = DateTime.Now.AddDays(-1);
                var startDate = new DateTime(now.Year, now.Month, now.Day);
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
                DateTime now = DateTime.Now;
                var endDate = new DateTime(now.Year, now.Month, now.Day);
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



            IEnumerable<AadeshPaymentDetailsModel> modelList;
            modelList = await _iBOCWServiceRoutineService.BOCWGetAadeshDataForRoutine(dtFromDate, dtToDate);
            var finalResult = modelList.ToList();

            ViewBag.StartDate = Convert.ToDateTime(dtFromDate).ToString("dd/MM/yyyy");
            ViewBag.EndDate = Convert.ToDateTime(dtToDate).ToString("dd/MM/yyyy");

            ViewBag.FileGenerationData = finalResult;
            return PartialView("BOCWServiceRoutine/_BOCWPDFileGeneration", finalResult);
        }
        public IActionResult BOCWPaymentResponseDetails()
        {
            return PartialView("BOCWServiceRoutine/_BOCWPaymentResponse");
        }

        [HttpPost]
        public async Task<IActionResult> SendForDisbursement(string fromDate = "", string toDate = "")
        {
            BOCWUploadFileOnServer(fromDate, toDate);
            TempData["Message"] = CommonUtils.ConcatString("File successfully sent for disbursment!!", Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
            return RedirectToAction("BOCWManualPaymentDetails", new { fromDate = fromDate, toDate = toDate });
        }
        [HttpPost]
        public async Task<IActionResult> FetchPaymentResponse()
        {
            BOCWFetchReturnCSVFileFromServer();
            TempData["PaymentMessage"] = "Payment files successfully received on portal.";
            return RedirectToAction("BOCWManualPaymentDetails");
        }
    }
}
