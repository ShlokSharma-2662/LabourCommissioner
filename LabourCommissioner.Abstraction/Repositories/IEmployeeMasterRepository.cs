using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Abstraction.Repositories
{
    public interface IEmployeeMasterRepository
    {
        Task<IEnumerable<SelectListItem>> GetDistrict();
        Task<IEnumerable<SelectListItem>> GetRole(long districtid);
        Task<IEnumerable<PostMaster>> GetPostMasterData(long postId, long districtId, long talukaid, bool isactive);
        Task<ResponseMessage> AddUpdateDeletePost(long districtId, long postid, long roleId, string postshortname, string postname, string password, string emailid, string contactno, bool isActive, string action);
        Task<PostMaster> GetPostData(long postid);
        Task<IEnumerable<Menumaster>> GetMenuMasterData(Menumaster menumaster);
        Task<IEnumerable<SelectListItem>> bindservicemaster(int beneficiarytypeid);
    }
}
