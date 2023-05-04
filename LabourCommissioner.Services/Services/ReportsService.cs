using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissioner.Services.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsService(IReportsRepository ireportsRepository)
        {
            _reportsRepository = ireportsRepository;
        }

        public async Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId)
        {
            var res = _reportsRepository.GetApplicationDetailsByAppId(ApplicationId);
            return await res;
        }
        public async Task<PersonalDetailsModel> GetReportPersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportPersonalDetailsByAppId( ApplicationId, servicesId,schemaName,  tableName);
            return await res;
        }
        public async Task<GLWBSSC_PersonalDetailsModel> GetReportGLWB_SSC_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_SSC_PersonalDetailsByAppId( ApplicationId, servicesId,schemaName,  tableName);
            return await res;
        } 
        public async Task<GLWBPSY_PersonalDetailsModel> GetReportGLWB_PSY_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_PSY_PersonalDetailsByAppId( ApplicationId, servicesId,schemaName,  tableName);
            return await res;
        } 
        public async Task<GLWBHSC_PersonalDetailsModel> GetReportGLWB_Hsc_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_Hsc_PersonalDetailsByAppId( ApplicationId, servicesId,schemaName,  tableName);
            return await res;
        }

        public async Task<GLWBHSS_PersonalDetails> GetReportGLWB_Hss_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_Hss_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWBCycle_personalDetails> GetReportGLWB_CSY_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_CSY_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWBADSY_PersonalDetailsModel> GetReportGLWB_adsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_adsy_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }
        public async Task<GLWBADSYSchemeDetails> GetReportGLWB_adsy_SchemeDetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_adsy_SchemeDetails(ApplicationId,  schemaName, tableName);
            return await res;
        }

        public async Task<GLWBMSL_Personaldetails> GetReportGLWB_MSL_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_MSL_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWBASY_PersonalDetailsModel> GetReportGLWB_aSY_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_aSY_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }
        public async Task<GLWBhty_PersonalDetailsModel> GetReportGLWB_HTY_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_HTY_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWB_TSY_personalDetails> GetReportGLWB_tsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_tsy_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }
          


        public async Task<ServiceMaster> GetSchemeByServiceIdgetscheme( long _serviceid)
        {
            var res = _reportsRepository.GetSchemeByServiceIdgetscheme(_serviceid);
            return await res;
        }
        public async Task<SchemeDetails> GetSchemeDetailsByAppId(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetSchemeDetailsByAppId(ApplicationId, schemaName, tableName);
            return await res;
        }
        //scheme added
        public async Task<BOCWBPSYSchemeDetails> getrptbocwbpsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocwbpsyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<BOCWTBSYSchemeDetails> getrptbocwTbsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocwTbsyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<BOCWASYSchemeDetails> getrptbocwasyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocwasyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        } 
        public async Task<BOCWTSYSchemeDetails> getrptbocwtsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocwtsyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<BOCWVCYSchemeDetails> getrptbocwvcyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocwvcyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<BOCWACSYSchemeDetails> getrptbocwacsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocwacsyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<BOCWPSYSchemeDetails> getrptbocwpsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocwpsyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
            public async Task<BOCWVRSchemeDetails> getrptbocwvrschemedetails(long ApplicationId, string schemaName, string tableName)
            {
                var res = _reportsRepository.getrptbocwvrschemedetails(ApplicationId, schemaName, tableName);
                return await res;
            }

            public async Task<GLWBHSCSchemeDetails> getrptglwbhscschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwbhscschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<GLWBPSY_SchemeDetails> getrptglwbpsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwbpsyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<GLWBSSCSchemeDetails> getrptglwbsscschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwbsscschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWBASYSchemeDetails> getrptglwb_asy_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwb_asy_schemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<BOCWPIPSchemeDetails> getrptbocwpipschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocwpipschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWBMSL_SchemeDetails> getrptglwb_msl_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwb_msl_schemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<GLWBCYCLE_Schemedetails> getrptglwb_csy_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwb_csy_schemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWBhtySchemeDetails> getrptglwb_hty_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwb_hty_schemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        

        public async Task<GLWBHSS_SchemeDetails> getrptglwbhssschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwbhssschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<List<DocumentFileDetails>> GetdocumentDetailsByAppId(long ApplicationId, string schemaName, string tableName, long servicesId)  
        {
            var res = _reportsRepository.GetdocumentDetailsByAppId(ApplicationId, schemaName, tableName, servicesId);
            return await res;
        }
        public async Task<List<PendencyReportDetails>> GetRPTDistrictWisePendencyData(long serviceId, DateTime? dtFromDate, DateTime? dtToDate, long districtId, long talukaId, int beneficiaryType, string schemaName)
        {
            var res = _reportsRepository.GetRPTDistrictWisePendencyData(serviceId, dtFromDate, dtToDate, districtId, talukaId, beneficiaryType, schemaName);
            return await res;
        }


        public async Task<GLWBHLS_PersonalDetailsModel> GetReportGLWB_HLS_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_HLS_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWBHLS_SchemeDetails> GetReportGLWB_HLS_SchemeDetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_HLS_SchemeDetails(ApplicationId, schemaName, tableName);
            return await res;
        }


        public async Task<List<EmpApplicationDetailsModel>> GetRPTBOCWApplicationDetailsList(long districtId, long talukaId, DateTime? dtFromDate, DateTime? dtToDate, int status, long postId, int beneficiaryType, string? schemaName)
        {
            var res = _reportsRepository.GetRPTBOCWApplicationDetailsList(districtId,talukaId,dtFromDate, dtToDate, status,postId, beneficiaryType, schemaName);
            return await res;
        }
        public async Task<List<EmpApplicationDetailsModel>> GetRPTBOCWApplicationDetailsListGLWB_TSY(long districtId, DateTime? dtFromDate, DateTime? dtToDate, int status, long postId, int beneficiaryType, string? schemaName)
        {
            var res = _reportsRepository.GetRPTBOCWApplicationDetailsListGLWB_TSY(districtId, dtFromDate, dtToDate, status, postId, beneficiaryType, schemaName);
            return await res;
        }

        public async Task<List<EmpApplicationDetailsModel>> GetRPTBOCWPendencyDaysList(long serviceId, long applicationid, int applicationstatus, string? schemaName)
        {
            var res = _reportsRepository.GetRPTBOCWPendencyDaysList(serviceId, applicationid, applicationstatus, schemaName);
            return await res;
        }


        #region Not Implemented
        public Task<TabModel> GetASync(long entityID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TabModel>> GetAllASync()
        {
            throw new NotImplementedException();
        }

        public Task<long> AddAsync(TabModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TabModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TabModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TabModel>> GetListAsync()
        {
            throw new NotImplementedException();
        }





        #endregion

        //lsy
        public async Task<GLWBLSY_PersonalDetails> GetReportGLWB_Lsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_Lsy_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }

        public async Task<GLWBLSY_SchemeDetails> getrptglwblsyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwblsyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        //GLWB_TSY
        public async Task<GLWB_TSY_personalDetails> GetReportGLWB_Tsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_Tsy_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }
        public async Task<GLWBTSYSchemeDetails> getrptglwb_tsy_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwb_tsy_schemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<List<CompanyWorkerDetails>> getrptglwb_tsy_companyWorkerDetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwb_tsy_companyWorkerDetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<List<CompanyWorkerDetails>> getrpt_glwb_tsy_getagecount(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrpt_glwb_tsy_getagecount(ApplicationId, schemaName, tableName);
            return await res;
        }

        //GLWB_TSY_Claim
        public async Task<GLWB_TSYClaim_personalDetails> GetReportGLWB_Tsy_Claim_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName)
        {
            var res = _reportsRepository.GetReportGLWB_Tsy_Claim_PersonalDetailsByAppId(ApplicationId, servicesId, schemaName, tableName);
            return await res;
        }
        public async Task<GLWBTSYSchemeDetails> getrptglwb_tsy_claim_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwb_tsy_claim_schemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<List<CompanyWorkerDetails>> getrptglwb_tsy_claim_companyWorkerDetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptglwb_tsy_claim_companyWorkerDetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<List<CompanyWorkerDetails>> getrpt_glwb_tsy_claim_getagecount(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrpt_glwb_tsy_claim_getagecount(ApplicationId, schemaName, tableName);
            return await res;
        }

        //bocw_hssy

        public async Task<BOCWHssySchemeDetails> getrptbocw_hssy_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocw_hssy_schemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<BOCWNanjiSchemeDetails> getrpt_bocw_nanaji_schemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrpt_bocw_nanaji_schemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<List<familymember>> getrptbocw_hssy_familyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocw_hssy_familyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<List<NanajiFamilyMemberDetails>> getrpt_bocw_nanaji_Familymembersdetailsschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrpt_bocw_nanaji_Familymembersdetailsschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<List<StudentMemberDetails>> getrpt_bocw_hssy_childrenfamilyschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrpt_bocw_hssy_childrenfamilyschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<List<StudentHostelMemberDetails>> getrpt_bocw_hssy_childrenhostelschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrpt_bocw_hssy_childrenhostelschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }
        public async Task<List<FamilyMemberDetails>> getrptbocw_hty_familymembersschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocw_hty_familymembersschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<List<FamilyMemberTravelDetails>> getrptbocw_hty_claim_familymembersschemedetails(long ApplicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrptbocw_hty_claim_familymembersschemedetails(ApplicationId, schemaName, tableName);
            return await res;
        }

        public async Task<List<BOCWVR_OtherSchemeDetails>> getrpt_bocw_vr_otherschemedetails(long applicationId, string schemaName, string tableName)
        {
            var res = _reportsRepository.getrpt_bocw_vr_otherschemedetails(applicationId, schemaName, tableName);
            return await res;
        }
    }
}
