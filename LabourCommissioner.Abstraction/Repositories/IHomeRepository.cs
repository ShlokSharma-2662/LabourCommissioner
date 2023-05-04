using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Abstraction.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<ServiceMaster>> BindServicesUserWiseFilter(int userTypeId, int beneficiaryType, int postId, int roleIds, int servicesubtypeid);

        Task<long> GetLevelNoFromPostId(long postid);
        Task<ServiceMaster> GetSchemeByServiceId(long ServiceId, long registrationid);
        Task<IEnumerable<ServiceMaster>> GetSchemeByBeneficiaryTypeId(long beneficiaryTypeId);
        Task<ApplicationDetailsModel> GetServiceId();
       
        Task<IEnumerable<bocw_ssy_personaldetails>> GetCitizen(int ApplicationId);
        Task<IEnumerable<ApplicationDetailsModel>> Bocw_Tbsy_GetApplication(long registrationId,long serviceId, string schemaName, string tableName, long usertype);
        Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_GetApplication(long registrationId,long serviceId, string schemaName, string tableName);
        Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_Hospital_GetApplication(long registrationId,long serviceId, string schemaName, string tableName);
        Task<IEnumerable<ApplicationDetailsModel>> GetApplicationDetails(long registrationId,long serviceId, string schemaName, string tableName);
        Task<IEnumerable<ApplicationDetailsModel>> Glwb_TSY_Claim_GetApplication(long registrationId,long serviceId, string schemaName, string tableName);
        Task<IEnumerable<ApplicationDetailsModel>> GetBocw_TBSYApplication(long registrationId,long serviceId, string schemaName, string tableName);

        // Task<ServiceMaster> GetServicesByHodId(int HodId);


        Task<ResponseMessage> GetApplicationCountByRegistrationIdAndServiceId(long registrationId,long serviceId);
        Task<ResponseMessage> AddExceptionLog(ExceptionLogger exceptionLogger);
        Task<Usermaster> GetUserId(Usermaster usermaster);
        Task<ResponseMessage> CheckENirmanCardExpiry(long registrationId);
        Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo);
        Task<ResponseMessage> UpdateeNirmanCardxpiryDate(long registrationId, DateTime? iCardToDateOld, DateTime? iCardToDateNew, DateTime? iCardFromDateOld, DateTime? iCardFromDateNew);
        Task<ResponseMessage> getaadharcardcountbyaadharnoandserviceid(string aadharcardno, long serviceId);
        Task<IEnumerable<ApplicationDetailsModel>> GetGLWB_HTYApplicationDetailsForClaim(long registrationId, long serviceId, string schemaName, string tableName);
        Task<ResponseMessage> UpdateGLWBUserCompany(PersonalDetailsModel personalDetailsModel);
    }
}
