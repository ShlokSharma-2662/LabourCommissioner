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
    public interface IServiceRoutineRepository
    {
        Task<IEnumerable<AadeshPaymentDetailsModel>> BOCWGetAadeshDataForRoutine(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<AadeshPaymentDetailsModel>> UpdateBOCWPaymentInfo(string payinfoids, string filename, int confirmuploadedstatus, int verifiedstatus);
        Task<IEnumerable<AadeshPaymentDetailsModel>> BOCWGetAadeshDataForFetchReturnCSVFile();
        Task<ResponseMessage> SaveBOCWPaymentResponse(DataTable dtData, string? IpAddress, string? HostName);
        Task<IEnumerable<AadeshPaymentDetailsModel>> GLWBGetAadeshDataForRoutine(DateTime? fromDate, DateTime? toDate);
        Task<IEnumerable<AadeshPaymentDetailsModel>> UpdateGLWBPaymentInfo(string payinfoids, string filename, int confirmuploadedstatus, int verifiedstatus);
        Task<IEnumerable<AadeshPaymentDetailsModel>> GLWBGetAadeshDataForFetchReturnCSVFile();
        Task<ResponseMessage> SaveGLWBPaymentResponse(DataTable dtData, string? IpAddress, string? HostName);
    }
}
