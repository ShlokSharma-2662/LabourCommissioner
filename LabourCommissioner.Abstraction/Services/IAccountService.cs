using LabourCommissioner.Abstraction.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;

namespace LabourCommissioner.Abstraction.Services
{
    public interface IAccountService : IBaseService<Usermaster>
    {
        Task<Usermaster> AthenticateUser(Usermaster login);
        Task<Usermaster> MethodForGetData(Usermaster emailModel);
        Task<ForgetPassword> ResetPassword(ForgetPassword resetpassword);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
        Task<Employeemaster> AthenticateEmployee(Employeemaster model);
        Task<ChangePasswordModel> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<Employeemaster> AthenticateCompany(Employeemaster model);
    }
}
