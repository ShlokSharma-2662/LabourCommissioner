using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Abstraction.Services
{
    public interface ICCRegistrationService : IBaseService<CCRegistration>
    {
        Task<IEnumerable<SelectListItem>> GetCCUserType(string ResourceType);
        Task<bool> UserAlreadyExist(string? PANTANNo);
        Task<ResponseMessage> AddUpdateRegistration(CCRegistration registration);
        Task<ResponseMessage> AddUpdateAuthorityDetails(CCRegistration registration);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
    }
}
