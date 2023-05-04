using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Services.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;
        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository ?? throw new ArgumentNullException(nameof(homeRepository));
        }
        //public async Task<IEnumerable<ServiceMaster>> BindServicesUserWiseFilter()
        //{
        //    return await _homeRepository.BindServicesUserWiseFilter();
        //}
        public async Task<IEnumerable<ServiceMaster>> BindServicesUserWiseFilter(int userType, int beneficiaryType, int postId, int roleIds, int servicesubtypeid)
        {
            return await _homeRepository.BindServicesUserWiseFilter(userType, beneficiaryType, postId, roleIds, servicesubtypeid);
        }

        public async Task<long> GetLevelNoFromPostId(long postid)
        {
            return await _homeRepository.GetLevelNoFromPostId(postid);
        }

        public async Task<ServiceMaster> GetSchemeByServiceId(long ServiceId, long registrationid)
        {
            return await _homeRepository.GetSchemeByServiceId(ServiceId,registrationid);
        }

        public async Task<ApplicationDetailsModel> GetServiceId()
        {
            return await _homeRepository.GetServiceId();
        }

        public async Task<IEnumerable<bocw_ssy_personaldetails>> GetCitizen(int ApplicationId)
        {
            return await _homeRepository.GetCitizen(ApplicationId);
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> GetApplicationDetails(long registrationId, long serviceId, string schemaName, string tableName)
        {
            return await _homeRepository.GetApplicationDetails(registrationId, serviceId, schemaName, tableName);
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_Claim_GetApplication(long registrationId, long serviceId, string schemaName, string tableName)
        {
            return await _homeRepository.Glwb_TSY_Claim_GetApplication(registrationId, serviceId, schemaName, tableName);
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> GetBocw_TBSYApplication(long registrationId, long serviceId, string schemaName, string tableName)
        {
            return await _homeRepository.GetBocw_TBSYApplication(registrationId, serviceId, schemaName, tableName);
        }

        public async Task<IEnumerable<ApplicationDetailsModel>> GetGLWB_HTYApplicationDetailsForClaim(long registrationId, long serviceId, string schemaName, string tableName)
        {
            return await _homeRepository.GetGLWB_HTYApplicationDetailsForClaim(registrationId, serviceId, schemaName, tableName);
        }


        public async Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_GetApplication(long registrationId, long serviceId, string schemaName, string tableName)
        {
            return await _homeRepository.Glwb_TSY_GetApplication(registrationId, serviceId, schemaName, tableName);
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> Bocw_Tbsy_GetApplication(long registrationId, long serviceId, string schemaName, string tableName,long usertype)
        {
            return await _homeRepository.Bocw_Tbsy_GetApplication(registrationId, serviceId, schemaName, tableName, usertype);
        }
        public async Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_Hospital_GetApplication(long registrationId, long serviceId, string schemaName, string tableName)
        {
            return await _homeRepository.Glwb_TSY_Hospital_GetApplication(registrationId, serviceId, schemaName, tableName);
        }

        public async Task<ResponseMessage> GetApplicationCountByRegistrationIdAndServiceId(long registrationId, long serviceId)
        {
            return await _homeRepository.GetApplicationCountByRegistrationIdAndServiceId(registrationId, serviceId);
        }

        public async Task<Usermaster> GetUserId(Usermaster usermaster)
        {
            return await _homeRepository.GetUserId(usermaster);
        }

        public async Task<IEnumerable<ServiceMaster>> GetSchemeByBeneficiaryTypeId(long beneficiaryTypeId)
        {
            return await _homeRepository.GetSchemeByBeneficiaryTypeId(beneficiaryTypeId);
        }
        //public async Task<ServiceMaster> GetServicesByHodId(int HodId)
        //{
        //    return await _homeRepository.GetServicesByHodId(HodId);
        //}
        public async Task<ResponseMessage> AddExceptionLog(ExceptionLogger exceptionLogger)
        {
            return await _homeRepository.AddExceptionLog(exceptionLogger);
        }
        public async Task<ResponseMessage> CheckENirmanCardExpiry(long registrationId)
        {
            return await _homeRepository.CheckENirmanCardExpiry(registrationId);
        }
        public async Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            return await _homeRepository.GetPersonalDetailsByRegNo(RegistrationNo);
        }
        public async Task<ResponseMessage> UpdateeNirmanCardxpiryDate(long registrationId, DateTime? iCardToDateOld, DateTime? iCardToDateNew, DateTime? iCardFromDateOld, DateTime? iCardFromDateNew)
        {
            return await _homeRepository.UpdateeNirmanCardxpiryDate(registrationId, iCardToDateOld, iCardToDateNew, iCardFromDateOld, iCardFromDateNew);
        }

        public async Task<ResponseMessage> getaadharcardcountbyaadharnoandserviceid(string aadharcardno, long serviceId)
        {
            var res = _homeRepository.getaadharcardcountbyaadharnoandserviceid(aadharcardno, serviceId);
            return await res;
        }
        public async Task<ResponseMessage> UpdateGLWBUserCompany(PersonalDetailsModel personalDetailsModel)
        {
            var res = _homeRepository.UpdateGLWBUserCompany(personalDetailsModel);
            return await res;
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
