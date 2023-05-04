using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Services.Services
{
    public class EmployeeMasterService : IEmployeeMasterService
    {
        private readonly IEmployeeMasterRepository _homeRepository;
        public EmployeeMasterService(IEmployeeMasterRepository homeRepository)
        {
            _homeRepository = homeRepository ?? throw new ArgumentNullException(nameof(homeRepository));
        }
        //public async Task<IEnumerable<ServiceMaster>> BindServicesUserWiseFilter()
        //{
        //    return await _homeRepository.BindServicesUserWiseFilter();


        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            var res = await _homeRepository.GetDistrict();
            return res;
        }
        public async Task<IEnumerable<SelectListItem>> GetRole(long districtid)
        {
            var res = await _homeRepository.GetRole(districtid);
            return res;
        }

        public async Task<PostMaster> GetPostData(long postid)
        {
            var res = await _homeRepository.GetPostData(postid);
            return res;
        }

        public async Task<IEnumerable<PostMaster>> GetPostMasterData(long postId, long districtId, long talukaid, bool isactive)
        {
            var res = await _homeRepository.GetPostMasterData(postId, districtId, talukaid, isactive);
            return res;
        }

        public async Task<ResponseMessage> AddUpdateDeletePost(long districtId, long postid, long roleId, string postshortname, string postname, string password, string emailid, string contactno, bool isActive, string action)
        {
            var res = await _homeRepository.AddUpdateDeletePost(districtId, postid, roleId, postshortname, postname, password, emailid, contactno, isActive, action);
            return res;
        }

        public async Task<IEnumerable<Menumaster>> GetMenuMasterData(Menumaster menumaster)
        {
            var res = await _homeRepository.GetMenuMasterData(menumaster);
            return res;
        }

        public async Task<IEnumerable<SelectListItem>> bindservicemaster(int beneficiarytypeid)
        {
            var res = await _homeRepository.bindservicemaster(beneficiarytypeid);
            return res;
        }

        #region Not Implemented Methods
        public Task<long> AddAsync(Registration entity)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(Registration entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetAllASync()
        {
            throw new NotImplementedException();
        }

        public Task<Registration> GetASync(long entityID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Registration entity)
        {
            throw new NotImplementedException();
        }

        //Task<Usermaster> IBaseService<Usermaster>.GetASync(long entityID)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<IEnumerable<Usermaster>> IBaseService<Usermaster>.GetAllASync()
        //{
        //    throw new NotImplementedException();
        //}

        public Task<long> AddAsync(Usermaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Usermaster entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Usermaster entity)
        {
            throw new NotImplementedException();
        }













        //Task<IEnumerable<Usermaster>> IBaseService<Usermaster>.GetListAsync()
        //{
        //    throw new NotImplementedException();
        //}


        #endregion
    }
}
