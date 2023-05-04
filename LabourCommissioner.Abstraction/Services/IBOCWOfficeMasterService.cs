using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LabourCommissioner.Abstraction.Services
{
    public interface IBOCWOfficeMasterService
    {
        Task<ResponseMessage> AddOfficeDetails(OfficeDetailsModel officeDetails);
        Task<IEnumerable<OfficeDetailsModel>> GetOfficeDetails();
    }
}
