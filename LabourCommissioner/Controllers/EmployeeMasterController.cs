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
using LabourCommissioner.Abstraction;
using System.Web;
using static LabourCommissioner.Abstraction.EnumLookup;

namespace LabourCommissioner.Controllers
{
    public class EmployeeMasterController : Controller
    {

        private readonly IEmployeeMasterService _ihomeService;
        private readonly IEmployeeMasterRepository _homeRepository;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHtmlLocalizer<HomeController> _htmlLocalizer;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmployeeMasterController(IStringLocalizer<HomeController> localizer, IConfiguration config, IHttpClientFactory clientFactory,
            IWebHostEnvironment webHostEnvironment, IHtmlLocalizer<HomeController> htmlLocalizer,
            IEmployeeMasterService homeService, IEmployeeMasterRepository homeRepository, ISchemeService schemeService, ISchemeUserServices schemeUserServices,
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

        public IActionResult PostMaster(long districtId = 0, long talukaid = 0, int isactive = 1, string action = "")
        {
            var districtModel = _ihomeService.GetDistrict();
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;
            ViewBag.DistrictId = districtId;
            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            bool isActive = true;
            if (isactive == 0)
            {
                isActive = false;
            }

            var PostMasterModel = _ihomeService.GetPostMasterData(postId, districtId, talukaid, isActive);
            var finalResult = PostMasterModel.Result.ToList();

            return View(finalResult);
        }

        public async Task<IActionResult> AddUpdatePost(long postid = 0, string ActionId = "")
        {
            var districtModel = _ihomeService.GetDistrict();
            var districtList = districtModel.Result.ToList();
            ViewBag.DistrictList = districtList;

            PostMaster model = new PostMaster();
            ViewBag.Action = ActionId;
            if (ActionId == "U")
            {
                //ViewBag.DistrictId = districtid;
                model = await _ihomeService.GetPostData(postid);
                ViewBag.DistrictId = model.districtid;
                var roles = _ihomeService.GetRole(model.districtid);
                var roleList = roles.Result.ToList();
                ViewBag.RoleList = roleList;
                ViewBag.RoleId = model.roleid;
                ViewBag.Action = ActionId;

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddUpdateDeletePost(long districtId, long roleId, string postshortname, string postname, string password, string emailid, string contactno, bool isActive, string action, long postId, string actionId)
        {
            //long postid = 0;
            if (action == "I")
            {
                postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            }
            if (actionId == "D")
            {
                action = "D";
            }

            var regResponse = _ihomeService.AddUpdateDeletePost(districtId, postId, roleId, postshortname, postname, password, emailid, contactno, isActive, action);
            if (regResponse.Result != null)
                if (regResponse != null && regResponse.Result.Error == 0)
                {
                    var msg = regResponse.Result.Msg;
                    TempData["Message"] = CommonUtils.ConcatString(msg, Convert.ToString((int)EnumLookup.ResponseMsgType.success), "||");
                    return RedirectToAction("PostMaster");
                }

            var errorMsg = "Post Adding Failed..!!";
            return RedirectToAction("PostMaster");
        }

        public IActionResult GetRole(long districtId)
        {
            var roles = _ihomeService.GetRole(districtId);
            return Json(new { data = roles });
        }

        [HttpGet]
        public IActionResult MenuMaster(Menumaster menumaster)
        {
            int beneficiarytypeid = Convert.ToInt32(_claimPincipal.FindFirst("BeneficiaryType").Value);
            var ServicesModel = _ihomeService.bindservicemaster(beneficiarytypeid);
            var ServicesList = ServicesModel.Result.ToList();
            ViewBag.ServicesList = ServicesList;
            long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value);
            menumaster.CreatedBy = postId;
            

            var MenuMasterModel = _ihomeService.GetMenuMasterData(menumaster);
            var finalResult = MenuMasterModel.Result.ToList();

            return View(finalResult);
        }

    }

}
