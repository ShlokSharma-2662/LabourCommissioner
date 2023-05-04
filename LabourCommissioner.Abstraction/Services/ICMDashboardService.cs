using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Abstraction.Services
{
    public interface ICMDashboardService : IBaseService<CCApplicationDetails>
    {

        Task<IEnumerable<SelectListItem>> GetServiceMasterByBeneficiaryIdforCMD(int beneficiarytypeid);
        Task<IEnumerable<SelectListItem>> GetDistrict();
        Task<IEnumerable<SelectListItem>> GetYear();
        Task<IEnumerable<SelectListItem>> GetMonth();
        Task<IEnumerable<CMDApplicationDetails>> GetCMDApplicationDetailslist(long appYear, long appMonth, long beneficiarytypeid,int statusId);
        Task<CMDApplicationDetails> GetCMDApplicationDetailsForInsert(long applicationId, long appYear, long appMonth, long serviceId);
        Task<ResponseMessage> AddUpdateCMDApplication(DataTable dtData, CMDApplicationDetails cmdApplicationDetails);
        Task<ResponseMessage> CMDSubmitApplication(long appYear, long appMonth, long serviceId, long userId,string ipAddress, string hostName);

        Task<CMDAPIApplicationDetails> GetBOCWCMDApplicationDetails(long appYear, long appMonth, long serviceId);
    }
}
