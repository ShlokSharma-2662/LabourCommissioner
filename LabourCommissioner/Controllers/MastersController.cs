using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using LabourCommissioner.Abstraction.Services;
using System.Web;
using LabourCommissioner.Abstraction;

namespace LabourCommissioner.Controllers
{
    public class MastersController : Controller
    {

        private readonly IMastersService _ihomeService;
        private readonly IMastersRepository _homeRepository;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MastersController(IStringLocalizer<HomeController> localizer, IConfiguration config, IHttpClientFactory clientFactory,
            IWebHostEnvironment webHostEnvironment, IHtmlLocalizer<HomeController> htmlLocalizer,
            IMastersService homeService, IMastersRepository homeRepository, ISchemeService schemeService, ISchemeUserServices schemeUserServices,
            IHttpContextAccessor httpContextAccessor)

        {
            _ihomeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
            _homeRepository = homeRepository;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _localizer = localizer;
            _htmlLocalizer = htmlLocalizer;
            _config = config;
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _webHostEnvironment = webHostEnvironment;
        }

        //District Master

        [HttpGet]
        public async Task<IActionResult> DistrictMaster()
        {

            var districtModel = _ihomeService.DistrictMaster();
            var finalResult = districtModel.Result.ToList();

            return View(finalResult);

        }

        public async Task<IActionResult> GetDistrictMasters(string strdistrictid, bool isFilled)
        {
            long districtid = 0;
            ViewBag.isFilled = isFilled;
            if (strdistrictid != "" && strdistrictid != null)
            {
                districtid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(Convert.ToString(strdistrictid))));
            }

            if (districtid > 0)
            {
                var filledModel = await _ihomeService.getdistrictdatabyid(districtid);
                if (districtid > 0 && filledModel != null)
                {

                    var model = await _ihomeService.getdistrictdatabyid(Convert.ToInt32(filledModel.districtid));
                    return View(filledModel);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateDeleteDistrictMaster(DistrictMaster addDistrictMasters)
        {
            

            if (addDistrictMasters.districtid > 0)
            {
                addDistrictMasters.action = nameof(EnumLookup.CRUDPurpose.U);
            }
            else
            {
                addDistrictMasters.action = nameof(EnumLookup.CRUDPurpose.I);
            }


            addDistrictMasters.ipaddress = CommonUtils.GetLocalIPAddress();
            addDistrictMasters.hostname = CommonUtils.GetHostName();
            addDistrictMasters.createdby = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var regResponse = await _ihomeService.AddUpdateDistrictMaster(addDistrictMasters);
            if (regResponse != null)
            {
                string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    ModelState.Clear();
                    return RedirectToAction("DistrictMaster", "Masters");
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    return RedirectToAction("GetDistrictMasters", "Masters");
                }
            }


            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("GetDistrictMasters", "Masters");


        }

        [HttpGet]
        public async Task<IActionResult> DeleteDistrict(long districtId)
        {

            DistrictMaster districtMaster=new DistrictMaster();
            districtMaster.districtid = districtId;
            districtMaster.action = nameof(EnumLookup.CRUDPurpose.D);
            districtMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            districtMaster.hostname = CommonUtils.GetHostName();
            districtMaster.createdby = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);

            var regResponse = await _ihomeService.AddUpdateDistrictMaster(districtMaster);
            return Json(new { data = regResponse });
        }

        // Taluka MAster

        [HttpGet]
        public async Task<IActionResult> TalukaMaster()
        {

            var districtModel = _ihomeService.TalukaMaster();
            var finalResult = districtModel.Result.ToList();

            return View(finalResult);

        }

        public async Task<IActionResult> GetTalukaMaster(string strtalukaid, bool isFilled)
        {
            var districtModel = _ihomeService.GetDistrict();
            var district = districtModel.Result.ToList();
            ViewBag.District = district;
            

            long talukaid = 0;
            ViewBag.isFilled = isFilled;
            if (strtalukaid != "" && strtalukaid != null)
            {
                talukaid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(Convert.ToString(strtalukaid))));
            }

            if (talukaid > 0)
            {
                var filledModel = await _ihomeService.gettalukabyId(talukaid);
                if (talukaid > 0 && filledModel != null)
                {

                    var model = await _ihomeService.gettalukabyId(Convert.ToInt32(filledModel.talukaid));
                    return View(filledModel);
                }
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateDeleteTalukaMaster(TalukaMaster addTalukaMaster)
        {


            if (addTalukaMaster.talukaid > 0)
            {
                addTalukaMaster.action = nameof(EnumLookup.CRUDPurpose.U);
            }
            else
            {
                addTalukaMaster.action = nameof(EnumLookup.CRUDPurpose.I);
            }


            addTalukaMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            addTalukaMaster.hostname = CommonUtils.GetHostName();
            addTalukaMaster.createdby = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var regResponse = await _ihomeService.AddUpdateTalukaMaster(addTalukaMaster);
            if (regResponse != null)
            {
                string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    ModelState.Clear();
                    return RedirectToAction("TalukaMaster", "Masters");
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    return RedirectToAction("GetTalukaMaster", "Masters");
                }
            }


            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("GetTalukaMaster", "Masters");


        }

        [HttpGet]
        public async Task<IActionResult> DeleteTaluka(long talukaid)
        {

            TalukaMaster talukaMaster = new TalukaMaster();
            talukaMaster.talukaid = talukaid;
            talukaMaster.action = nameof(EnumLookup.CRUDPurpose.D);
            talukaMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            talukaMaster.hostname = CommonUtils.GetHostName();
            talukaMaster.createdby = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);

            var regResponse = await _ihomeService.AddUpdateTalukaMaster(talukaMaster);
            return Json(new { data = regResponse });
        }

        //Document Master

        [HttpGet]
        public async Task<IActionResult> DocumentMaster()
        {

            var districtModel = _ihomeService.DocumentMaster();
            var finalResult = districtModel.Result.ToList();

            return View(finalResult);

        }

        public async Task<IActionResult> GetDocumentMaster(string strdocumentmasterid, bool isFilled)
        {
            int beneficiarytypeid = 2;
            var ServicesModel = _ihomeService.bindservicemaster(beneficiarytypeid);
            var ServicesList = ServicesModel.Result.ToList();
            ViewBag.ServicesList = ServicesList;

            long documentmasterid = 0;
            ViewBag.isFilled = isFilled;
            if (strdocumentmasterid != "" && strdocumentmasterid != null)
            {
                documentmasterid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(Convert.ToString(strdocumentmasterid))));
            }

            if (documentmasterid > 0)
            {
                var filledModel = await _ihomeService.getdocumentbyid(documentmasterid);
                if (documentmasterid > 0 && filledModel != null)
                {

                    var model = await _ihomeService.getdocumentbyid(Convert.ToInt32(filledModel.documentmasterid));
                    return View(filledModel);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateDeleteDocumentMaster(DocumentMaster addDocumentMaster)
        {


            if (addDocumentMaster.documentmasterid > 0)
            {
                addDocumentMaster.action = nameof(EnumLookup.CRUDPurpose.U);
            }
            else
            {
                addDocumentMaster.action = nameof(EnumLookup.CRUDPurpose.I);
            }


            addDocumentMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            addDocumentMaster.hostname = CommonUtils.GetHostName();
            addDocumentMaster.createdby = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var regResponse = await _ihomeService.AddDocumentsMasters(addDocumentMaster);
            if (regResponse != null)
            {
                string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    ModelState.Clear();
                    return RedirectToAction("DocumentMaster", "Masters");
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    return RedirectToAction("GetDocumentMaster", "Masters");
                }
            }


            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("GetDocumentMaster", "Masters");


        }

        [HttpGet]
        public async Task<IActionResult> DeleteDocument(long documentmasterid)
        {

            DocumentMaster documentMaster = new DocumentMaster();
            documentMaster.documentmasterid = documentmasterid;
            documentMaster.action = nameof(EnumLookup.CRUDPurpose.D);
            documentMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            documentMaster.hostname = CommonUtils.GetHostName();
            documentMaster.createdby = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);

            var regResponse = await _ihomeService.AddDocumentsMasters(documentMaster);
            return Json(new { data = regResponse });
        }

        //Resource Master

        [HttpGet]
        public async Task<IActionResult> ResourceMaster()
        {

            var ResourceModel = _ihomeService.ResourceMaster();
            var finalResult = ResourceModel.Result.ToList();

            return View(finalResult);

        }

        public async Task<IActionResult> GetResourceMaster(string strresourceid, bool isFilled)
        {
            var resourceModel = _ihomeService.bindservicemaster();
            var resourceList = resourceModel.Result.ToList();
            ViewBag.resourceList = resourceList;
            

            long resourceid = 0;
            ViewBag.isFilled = isFilled;
            if (strresourceid != "" && strresourceid != null) 
            {
                resourceid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(Convert.ToString(strresourceid))));
            }

            if (resourceid > 0)
            {
                var filledModel = await _ihomeService.getresourcebyid(resourceid);
                if (resourceid > 0 && filledModel != null)
                {

                    var model = await _ihomeService.getresourcebyid(Convert.ToInt32(filledModel.resourceid));
                    return View(filledModel);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateDeleteResourceMaster(ResourceMaster addResourceMaster)
        {


            if (addResourceMaster.resourceid > 0)
            {
                addResourceMaster.action = nameof(EnumLookup.CRUDPurpose.U);
            }
            else
            {
                addResourceMaster.action = nameof(EnumLookup.CRUDPurpose.I);
            }


            addResourceMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            addResourceMaster.hostname = CommonUtils.GetHostName();
            addResourceMaster.createdby = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var regResponse = await _ihomeService.AddResourceMaster(addResourceMaster);
            if (regResponse != null)
            {
                string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    ModelState.Clear();
                    return RedirectToAction("ResourceMaster", "Masters");
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    return RedirectToAction("GetResourceMaster", "Masters");
                }
            }


            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("GetResourceMaster", "Masters");


        }

        [HttpGet]
        public async Task<IActionResult> DeleteResource(int resourceid)
        {

            ResourceMaster resourceMaster = new ResourceMaster();
            resourceMaster.resourceid = resourceid;
            resourceMaster.action = nameof(EnumLookup.CRUDPurpose.D);
            resourceMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            resourceMaster.hostname = CommonUtils.GetHostName();
            resourceMaster.createdby = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);

            var regResponse = await _ihomeService.AddResourceMaster(resourceMaster);
            return Json(new { data = regResponse });
        }

        //village master


        [HttpGet]
        public async Task<IActionResult> VillageMaster(long talukaid,long districtid = 0)
        {

            var districtModel = _ihomeService.GetDistrict();
            var district = districtModel.Result.ToList();
            ViewBag.DistrictList = district;
            ViewBag.districtid = districtid;

            var talukaModel = _ihomeService.GetTalukaByDistrictId(Convert.ToInt32(districtid));
            var talukaList = talukaModel.Result.ToList();
            ViewBag.TalukaList = talukaList;
            ViewBag.TalukaId = talukaid;


            var villageModel = _ihomeService.getvillagebyDistrictTalukaId(Convert.ToInt32(districtid), Convert.ToInt32(talukaid));
            var finalResult = villageModel.Result.ToList();

            return View(finalResult);
            //return View();

        }

        public async Task<IActionResult> GetVillageMaster(string strvillageId,string strdistrictid, string strtalukaid, bool isFilled)
        {
            long villageid = 0;
            long districtid = 0;
            long talukaid = 0;

            var districtModel = _ihomeService.GetDistrict();
            var district = districtModel.Result.ToList();
            ViewBag.DistrictList = district;
            ViewBag.DistrictId = districtid;

            var talukaModel = _ihomeService.GetTalukaByDistrictId(Convert.ToInt32(districtid));
            var talukaList = talukaModel.Result.ToList();
            ViewBag.TalukaList = talukaList;
            ViewBag.TalukaId = talukaid;

            
            ViewBag.isFilled = isFilled;
            if (strvillageId != "" && strvillageId != null)
            {
                villageid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(Convert.ToString(strvillageId))));
                districtid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(Convert.ToString(strdistrictid))));
                talukaid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(Convert.ToString(strtalukaid))));
            }

            if (villageid > 0)
            {
                var filledModel = await _ihomeService.getvillagedatabyid(districtid,villageid,talukaid);
                if (villageid > 0 && filledModel != null)
                {

                    var model = await _ihomeService.getvillagedatabyid(Convert.ToInt32(filledModel.districtid), Convert.ToInt32(filledModel.villageid), Convert.ToInt32(filledModel.talukaid));
                    return View(filledModel);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateDeleteVillageMaster(VillageMaster addVillageMaster)
        {


            if (addVillageMaster.villageid > 0)
            {
                addVillageMaster.action = nameof(EnumLookup.CRUDPurpose.U);
            }
            else
            {
                addVillageMaster.action = nameof(EnumLookup.CRUDPurpose.I);
            }


            addVillageMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            addVillageMaster.hostname = CommonUtils.GetHostName();
            addVillageMaster.createdby = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var regResponse = await _ihomeService.AddVillageMaster(addVillageMaster);
            if (regResponse != null)
            {
                string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    ModelState.Clear();
                    return RedirectToAction("VillageMaster", "Masters");
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    return RedirectToAction("GetVillageMaster", "Masters");
                }
            }


            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("GetVillageMaster", "Masters");


        }

        [HttpGet]
        public async Task<IActionResult> DeleteVillage(long villageid)
        {

            VillageMaster villageMaster = new VillageMaster();
            villageMaster.villageid = villageid;
            villageMaster.action = nameof(EnumLookup.CRUDPurpose.D);
            villageMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            villageMaster.hostname = CommonUtils.GetHostName();
            villageMaster.createdby = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);

            var regResponse = await _ihomeService.AddVillageMaster(villageMaster);
            return Json(new { data = regResponse });
        }

        //Service Schedular Master

        [HttpGet]
        public async Task<IActionResult> ServiceSchedulerMaster()
        {

            var ServiceSchedulerModel = _ihomeService.ServiceScheduler();
            var finalResult = ServiceSchedulerModel.Result.ToList();

            return View(finalResult);

        }

        public async Task<IActionResult> GetServiceSchedulerMaster(string strserviceschedulerid, bool isFilled)
        {
            int beneficiarytypeid = 2;
            var ServicesModel = _ihomeService.bindservicemaster(beneficiarytypeid);
            var ServicesList = ServicesModel.Result.ToList();
            ViewBag.resourceList = ServicesList;


            long serviceschedulerid = 0;
            ViewBag.isFilled = isFilled;
            if (strserviceschedulerid != "" && strserviceschedulerid != null)
            {
                serviceschedulerid = Convert.ToInt64(CommonUtils.Decrypt(HttpUtility.UrlDecode(Convert.ToString(strserviceschedulerid))));
            }

            if (serviceschedulerid > 0)
            {
                var filledModel = await _ihomeService.getserviceschedulerbyid(serviceschedulerid);
                if (serviceschedulerid > 0 && filledModel != null)
                {

                    var model = await _ihomeService.getserviceschedulerbyid(Convert.ToInt32(filledModel.serviceschedulerid));
                    return View(filledModel);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUpdateDeleteServiceSchedular(ServiceSchedular addServiceSchedular)
        {


            if (addServiceSchedular.serviceschedulerid > 0)
            {
                addServiceSchedular.action = nameof(EnumLookup.CRUDPurpose.U);
            }
            else
            {
                addServiceSchedular.action = nameof(EnumLookup.CRUDPurpose.I);
            }


            addServiceSchedular.ipaddress = CommonUtils.GetLocalIPAddress();
            addServiceSchedular.hostname = CommonUtils.GetHostName();
            addServiceSchedular.createdby = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var regResponse = await _ihomeService.AddUpdateDeleteServiceSchedular(addServiceSchedular);
            if (regResponse != null)
            {
                string errorMsg = regResponse.Msg == null ? "Somthing went wrong please try again." : regResponse.Msg;
                if (regResponse != null && regResponse != null && regResponse.Error == 0)
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    ModelState.Clear();
                    return RedirectToAction("ServiceSchedulerMaster", "Masters");
                }
                else
                {
                    TempData["Message"] = CommonUtils.ConcatString(errorMsg, Convert.ToString((int)EnumLookup.ResponseMsgType.warning), "||");
                    ModelState.Clear();
                    return RedirectToAction("GetServiceSchedulerMaster", "Masters");
                }
            }


            TempData["Message"] = CommonUtils.ConcatString("Somthing went wrong please try after sometime.", Convert.ToString((int)EnumLookup.ResponseMsgType.error), "||");
            return RedirectToAction("GetServiceSchedulerMaster", "Masters");


        }

        [HttpGet]
        public async Task<IActionResult> DeleteServiceScheduler(long serviceschedulerid)
        {

            ServiceSchedular serviceschdulerMaster = new ServiceSchedular();
            serviceschdulerMaster.serviceschedulerid = serviceschedulerid;
            serviceschdulerMaster.action = nameof(EnumLookup.CRUDPurpose.D);
            serviceschdulerMaster.ipaddress = CommonUtils.GetLocalIPAddress();
            serviceschdulerMaster.hostname = CommonUtils.GetHostName();
            serviceschdulerMaster.createdby = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);

            var regResponse = await _ihomeService.AddUpdateDeleteServiceSchedular(serviceschdulerMaster);
            return Json(new { data = regResponse });
        }
    }
}
