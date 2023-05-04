using Dapper;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.DataRepository.Repositories
{
    public class AuthorizeRepository : BaseRepository<Registration>, IAuthorizeRepository
    {

        //private readonly BaseRepository dbHelper;
        //private readonly shramsetuContext context;
        public IConfiguration appConfig;

        public AuthorizeRepository(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        //public AuthorizeRepository(shramsetuContext context)
        //{
        //    this.context = context;
        //    dbHelper = new BaseRepository(this.context);
        //}

        public bool CheckPermission(string path, string roleIds, bool? isInsert, bool? isView, bool? isUpdate, bool? isDelete)
        {
            using (var conn = GetConnection())
            {
                var queryParameters = new DynamicParameters();
                //queryParameters.Add("@RegistrationNo", registration.RegistrationNo);
                queryParameters.Add("_path", path);
                queryParameters.Add("_RoleIds", roleIds);
                queryParameters.Add("_IsInsert", isInsert);
                queryParameters.Add("_IsView", isView);
                queryParameters.Add("_IsUpdate", isUpdate);
                queryParameters.Add("_IsDelete", isDelete);
                var result = conn.QueryFirstOrDefaultAsync<Menumaster>(Procedures.CheckPermission, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                return result != null;
            }
            //List<MenuRoleMapping> userRoleMapping = userRoleData.ToList();

            //bool isRole = false;

            //MenuRoleMapping? roleMapping = userRoleMapping?.Where(x => x.Path == path && roleIds.Contains(x.RoleId.ToString()))?.FirstOrDefault();
            //if (roleMapping == null)
            //    return false;

            //if (isInsert != null)
            //    isRole = roleMapping.IsInsert == isInsert;
            //if (isView != null && !isRole)
            //    isRole = roleMapping.IsView == isView;
            //if (isUpdate != null && !isRole)
            //    isRole = roleMapping.IsUpdate == isUpdate;
            //if (isDelete != null && !isRole)
            //    isRole = roleMapping.IsDelete == isDelete;

            //return isRole;

        }
        private List<MenuRoleMapping> userRoleData
            = new() {
                new(){ Path = "/Home/Home", RoleId = 1, IsInsert = true, IsView = true, IsDelete = true},
                new(){ Path = "b", RoleId = 2, IsInsert = false, IsView = true, IsUpdate = true}
            };
        private class MenuRoleMapping
        {
            public string Path { get; set; }
            public int RoleId { get; set; }
            public bool IsInsert { get; set; }
            public bool IsView { get; set; }
            public bool IsUpdate { get; set; }
            public bool IsDelete { get; set; }
        }
    }
}
