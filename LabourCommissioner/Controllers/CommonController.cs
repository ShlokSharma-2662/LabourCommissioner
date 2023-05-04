using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common.Utility;
using LabourCommissioner.Models;
//using LabourCommissioner.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Web;
using LabourCommissioner.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Text.RegularExpressions;
using LabourCommissioner.CustomAuthorization;

namespace LabourCommissioner.Controllers
{
    [Authorize]
    //[ServiceFilter(typeof(PermissionRequirementFilter))]
    public class CommonController : Controller
    {
        private readonly ICommonService _iCommonService;
        private readonly ICommonRepository _CommonRepository;
        private readonly ISchemeService _iscchemeService;
        private readonly ISchemeUserServices _schemeUserServices;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<CommonController> _localizer;
        private readonly IHtmlLocalizer<CommonController> _htmlLocalizer;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _isexceptionmailrequired;
        private readonly string _bocwRegistrationAPI;
        private readonly string _glwbRegistrationAPI;

        public CommonController(IStringLocalizer<CommonController> localizer, IConfiguration config, IWebHostEnvironment webHostEnvironment, IHtmlLocalizer<CommonController> htmlLocalizer, ICommonService CommonService, ICommonRepository CommonRepository, ISchemeService schemeService, ISchemeUserServices schemeUserServices,
            IHttpContextAccessor httpContextAccessor)

        {
            _iCommonService = CommonService ?? throw new ArgumentNullException(nameof(CommonService));
            _CommonRepository = CommonRepository;
            _iscchemeService = schemeService ?? throw new ArgumentNullException(nameof(schemeService));
            _schemeUserServices = schemeUserServices;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _localizer = localizer;
            _htmlLocalizer = htmlLocalizer;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _isexceptionmailrequired = _config["SMTPConfig:_IsExceptionMailRequired"];
            _bocwRegistrationAPI = _config["RegistrationAPI:BOCW"];
            _glwbRegistrationAPI = _config["RegistrationAPI:GLWB"];
        }

        [HttpGet]
        public IActionResult GetDistrict()
        {
            var regions = _iCommonService.GetDistrict();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetTalukaByDistrictId(int districtId)
        {
            var regions = _iCommonService.GetTalukaByDistrictId(districtId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            var regions = _iCommonService.GetVillageByDistrictIdAndTalukaId(districtId, talukaId);
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }

        [HttpGet]
        public IActionResult GetAllStates()
        {
            var regions = _iCommonService.GetAllStates();
            //return Json(regions, System.Web.Mvc.JsonRequestBehavior.AllowGet);
            return Json(new { data = regions });
        }


    }

}
