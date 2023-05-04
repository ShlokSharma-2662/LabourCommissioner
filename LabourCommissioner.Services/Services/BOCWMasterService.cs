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
using System.Web.Mvc;

namespace LabourCommissioner.Services.Services
{
    public class BOCWMasterService : IBOCWOfficeMasterService
    {
        private readonly IBOCWMasterRepository _iBOCWMasterServiceRepository;
        public BOCWMasterService(IBOCWMasterRepository iBOCWMasterServiceRepository)
        {
            _iBOCWMasterServiceRepository = iBOCWMasterServiceRepository ?? throw new ArgumentNullException(nameof(iBOCWMasterServiceRepository));
        }

        public async Task<ResponseMessage> AddOfficeDetails(OfficeDetailsModel officeDetails)
        {
            var res = await _iBOCWMasterServiceRepository.AddOfficeDetails(officeDetails);
            return res;
        }

        public async Task<IEnumerable<OfficeDetailsModel>> GetOfficeDetails()
        {
            var res = await _iBOCWMasterServiceRepository.GetOfficeDetails();
            return res;
        }
    }
}
