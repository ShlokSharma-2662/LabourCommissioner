using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LabourCommissioner.Views.Shared.Components.AdminMenu
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly IEmployeeHomeService _ihomeService;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminMenuViewComponent(IEmployeeHomeService homeService, IHttpContextAccessor httpContextAccessor)
        {
            _ihomeService = homeService ?? throw new ArgumentNullException(nameof(homeService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
        }
        public  IViewComponentResult Invoke(LabourCommissioner.Abstraction.ViewDataModels.AdminMenu adminMenu)
        {
            IEnumerable<LabourCommissioner.Abstraction.ViewDataModels.AdminMenu> model = null;
            if (_claimPincipal != null && _claimPincipal.Claims.Count() > 0)
            {
                int usertypeId = Convert.ToInt32(_claimPincipal.FindFirst("UserType").Value != null ? _claimPincipal.FindFirst("UserType").Value : 0);
                long serviceId = Convert.ToInt32(_claimPincipal.FindFirst("ServiceId").Value != null ? _claimPincipal.FindFirst("ServiceId").Value : 0);
                long parentmenuId = 0;
                string roleId = Convert.ToString(_claimPincipal.FindFirst(ClaimTypes.Role).Value != null ? _claimPincipal.FindFirst(ClaimTypes.Role).Value : "");
                long postId = Convert.ToInt32(_claimPincipal.FindFirst("PostId").Value != null ? _claimPincipal.FindFirst("PostId").Value : 0);
                model = _ihomeService.BindMenuRoleWise(usertypeId, serviceId, parentmenuId, roleId, postId);
            }
           
            return View("AdminMenuViewComponent", model);

        }
    }

}
