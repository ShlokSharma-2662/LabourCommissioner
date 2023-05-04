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
    public class BOCWServiceRoutineService : IBOCWServiceRoutineService
    {
        private readonly IBOCWServiceRoutineRepository _bocwserviceRoutineRepository;
        public BOCWServiceRoutineService(IBOCWServiceRoutineRepository bocwserviceRoutineRepository)
        {
            _bocwserviceRoutineRepository = bocwserviceRoutineRepository ?? throw new ArgumentNullException(nameof(bocwserviceRoutineRepository));
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> BOCWGetAadeshDataForRoutine(DateTime? fromDate, DateTime? toDate)
        {
            return await _bocwserviceRoutineRepository.BOCWGetAadeshDataForRoutine(fromDate, toDate);
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> UpdateBOCWPaymentInfo(string payinfoids, string filename, int confirmuploadedstatus, int verifiedstatus)
        {
            return await _bocwserviceRoutineRepository.UpdateBOCWPaymentInfo(payinfoids, filename, confirmuploadedstatus, verifiedstatus);
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> BOCWGetAadeshDataForFetchReturnCSVFile()
        {
            return await _bocwserviceRoutineRepository.BOCWGetAadeshDataForFetchReturnCSVFile();
        }
        public async Task<ResponseMessage> SaveBOCWPaymentResponse(DataTable dtData, string? IpAddress, string? HostName)
        {
            return await _bocwserviceRoutineRepository.SaveBOCWPaymentResponse(dtData, IpAddress, HostName);
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
