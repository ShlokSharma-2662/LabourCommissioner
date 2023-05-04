using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;

namespace LabourCommissioner.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }
        public async Task<Usermaster> AthenticateUser(Usermaster usermaster)
        {
            return await _accountRepository.AthenticateUser(usermaster);
        }

        public async Task<Usermaster> MethodForGetData(Usermaster emailModel)
        {
            var res = await _accountRepository.MethodForGetData(emailModel);
            return res;
        }

        public async Task<ForgetPassword> ResetPassword(ForgetPassword resetpassword)
        {
            var res = await _accountRepository.ResetPassword(resetpassword);
            return res;
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            var res = _accountRepository.AddSMSLogs(mobileNo, serviceId, smsContent, userId);
            return await res;
        }

        public async Task<Employeemaster> AthenticateEmployee(Employeemaster model)
        {
            return await _accountRepository.AthenticateEmployee(model);
        }
        public async Task<ChangePasswordModel> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            return await _accountRepository.ChangePassword(changePasswordModel);
        }

        public async Task<Employeemaster> AthenticateCompany(Employeemaster model)
        {
            return await _accountRepository.AthenticateCompany(model);
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

        Task<Usermaster> IBaseService<Usermaster>.GetASync(long entityID)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Usermaster>> IBaseService<Usermaster>.GetAllASync()
        {
            throw new NotImplementedException();
        }

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

        Task<IEnumerable<Usermaster>> IBaseService<Usermaster>.GetListAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
