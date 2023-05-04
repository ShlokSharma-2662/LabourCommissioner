using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LabourCommissioner.Controllers
{
    public class BOCWMasterController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBOCWOfficeMasterService _iIOfficeMasterService;

        public BOCWMasterController(IBOCWOfficeMasterService officeMasterService, IConfiguration config, IHttpClientFactory clientFactory,
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)   
        {
            _iIOfficeMasterService = officeMasterService ??
                                              throw new ArgumentNullException(nameof(_iIOfficeMasterService)); ;
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult BOCWMaster()
        {
            return View();
        }

        public async Task<IActionResult> BOCWOfficeMasterList()
        {

            var model1 = await _iIOfficeMasterService.GetOfficeDetails();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PermissionRequirement(PermissionConstant.IsInsert, PermissionConstant.IsUpdate)]
        public async Task<IActionResult> AddOfficeDetails(OfficeDetailsModel officeDetails)
        {
            officeDetails.ipaddress = CommonUtils.GetLocalIPAddress();
            officeDetails.hostname = CommonUtils.GetHostName();
            officeDetails.createdby = Convert.ToInt32(_claimPincipal.FindFirstValue("PostId"));
            var model = _iIOfficeMasterService.AddOfficeDetails(officeDetails);

            return RedirectToAction("BOCWOfficeMasterList", "BOCWMaster");
        }
    }
}
