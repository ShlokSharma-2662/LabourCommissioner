using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LabourCommissioner.CustomAuthorization
{
    [AttributeUsage(AttributeTargets.All)]
    public class PermissionRequirementAttribute : Attribute, IFilterMetadata
    {
        private readonly List<PermissionConstant> allowedPermissions;

        public PermissionRequirementAttribute(params PermissionConstant[] permissions)
        {
            this.allowedPermissions = new List<PermissionConstant>();
            this.allowedPermissions.AddRange(permissions);
        }

        public List<PermissionConstant> GetAllowedPermissions()
        {
            return this.allowedPermissions;
        }
    }
}
