using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace LabourCommissioner.CustomAuthorization
{
    public class PermissionRequirementFilter : Attribute, IAsyncAuthorizationFilter, IFilterFactory
    {
        private readonly IAuthorizeRepository authorizeRepository;
        private readonly IAuthorizationService authService;

        public PermissionRequirementFilter(IAuthorizeRepository authorizeRepository, IAuthorizationService authService)
        {
            this.authorizeRepository = authorizeRepository;
            this.authService = authService;
        }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new PermissionRequirementFilter(
            (IAuthorizeRepository)serviceProvider.GetService(typeof(IAuthorizeRepository)), (IAuthorizationService)serviceProvider.GetService(typeof(IAuthorizationService)));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //PermissionRequirementAttribute obj = context.Filters.FirstOrDefault(f => f.GetType() == typeof(PermissionRequirementAttribute)) as PermissionRequirementAttribute;
            //List<PermissionConstant> allowedPermissions = obj?.GetAllowedPermissions();
            //bool ok = (await authService.AuthorizeAsync(context.HttpContext.User, null, new AccessToRouteRequirement(allowedPermissions.ToArray()))).Succeeded;

            //if (!ok) context.Result = new ChallengeResult();

            //if (context.HttpContext.User == null || !context.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    context.Result = new RedirectResult("/Home/AccessDenied");
            //    return;
            //}

            string path = context.HttpContext.Request.Path;

            if (path != "" && path != "/")
            {

                PermissionRequirementAttribute obj = context.Filters.FirstOrDefault(f => f.GetType() == typeof(PermissionRequirementAttribute)) as PermissionRequirementAttribute;
                List<PermissionConstant> allowedPermissions = obj?.GetAllowedPermissions();

                string roleIds = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
                bool isAuthorized = false;
                if (allowedPermissions.Contains(PermissionConstant.IsNone))
                {
                    isAuthorized = true;
                }
                else
                {
                    isAuthorized = authorizeRepository.CheckPermission(path, roleIds,
                                allowedPermissions.Contains(PermissionConstant.IsInsert) ? true : null,
                                allowedPermissions.Contains(PermissionConstant.IsView) ? true : null,
                                allowedPermissions.Contains(PermissionConstant.IsUpdate) ? true : null,
                                allowedPermissions.Contains(PermissionConstant.IsDelete) ? true : null);
                }
                if (!isAuthorized)
                {
                    context.Result = new RedirectResult("/Home/AccessDenied");
                }
            }
        }
    }
}
