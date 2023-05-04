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
    public class GLWBServiceRoutineService : IGLWBServiceRoutineService
    {
        private readonly IGLWBServiceRoutineRepository _glwbserviceRoutineRepository;
        public GLWBServiceRoutineService(IGLWBServiceRoutineRepository glwbserviceRoutineRepository)
        {
            _glwbserviceRoutineRepository = glwbserviceRoutineRepository ?? throw new ArgumentNullException(nameof(glwbserviceRoutineRepository));
        }

        public async Task<IEnumerable<AadeshPaymentDetailsModel>> GLWBGetAadeshDataForRoutine(DateTime? fromDate, DateTime? toDate)
        {
            return await _glwbserviceRoutineRepository.GLWBGetAadeshDataForRoutine(fromDate, toDate);
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> UpdateGLWBPaymentInfo(string payinfoids, string filename, int confirmuploadedstatus, int verifiedstatus)
        {
            return await _glwbserviceRoutineRepository.UpdateGLWBPaymentInfo(payinfoids, filename, confirmuploadedstatus, verifiedstatus);
        }
        public async Task<IEnumerable<AadeshPaymentDetailsModel>> GLWBGetAadeshDataForFetchReturnCSVFile()
        {
            return await _glwbserviceRoutineRepository.GLWBGetAadeshDataForFetchReturnCSVFile();
        }
        public async Task<ResponseMessage> SaveGLWBPaymentResponse(DataTable dtData, string? IpAddress, string? HostName)
        {
            return await _glwbserviceRoutineRepository.SaveGLWBPaymentResponse(dtData, IpAddress, HostName);
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
