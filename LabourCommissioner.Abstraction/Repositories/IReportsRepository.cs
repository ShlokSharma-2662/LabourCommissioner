using LabourCommissioner.Abstraction.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LabourCommissioner.Abstraction.ViewDataModels;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;
using LabourCommissioner.Common;
using System.Data;

namespace LabourCommissioner.Abstraction.Repositories
{
    public interface IReportsRepository : IBaseRepository<TabModel>
    {
        Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId);
        Task<PersonalDetailsModel> GetReportPersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWBSSC_PersonalDetailsModel> GetReportGLWB_SSC_PersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWBPSY_PersonalDetailsModel> GetReportGLWB_PSY_PersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWBCycle_personalDetails> GetReportGLWB_CSY_PersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWBMSL_Personaldetails> GetReportGLWB_MSL_PersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWBASY_PersonalDetailsModel> GetReportGLWB_aSY_PersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWB_TSY_personalDetails> GetReportGLWB_tsy_PersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWBhty_PersonalDetailsModel> GetReportGLWB_HTY_PersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWBHSC_PersonalDetailsModel> GetReportGLWB_Hsc_PersonalDetailsByAppId(long ApplicationId, long servicesId,string schemaName, string tableName);
        Task<GLWBHSS_PersonalDetails> GetReportGLWB_Hss_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName);
        Task<GLWBADSY_PersonalDetailsModel> GetReportGLWB_adsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName);
        Task<ServiceMaster> GetSchemeByServiceIdgetscheme(long _serviceid);
        Task<SchemeDetails> GetSchemeDetailsByAppId(long ApplicationId,string schemaName, string tableName);
        //added scheme
        Task<BOCWBPSYSchemeDetails> getrptbocwbpsyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<GLWBADSYSchemeDetails> GetReportGLWB_adsy_SchemeDetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWACSYSchemeDetails> getrptbocwacsyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWTBSYSchemeDetails> getrptbocwTbsyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWASYSchemeDetails> getrptbocwasyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWPSYSchemeDetails> getrptbocwpsyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWTSYSchemeDetails> getrptbocwtsyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWVCYSchemeDetails> getrptbocwvcyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<GLWBHSCSchemeDetails> getrptglwbhscschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<GLWBPSY_SchemeDetails> getrptglwbpsyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<GLWBSSCSchemeDetails> getrptglwbsscschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWPIPSchemeDetails> getrptbocwpipschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWVRSchemeDetails> getrptbocwvrschemedetails(long ApplicationId, string schemaName, string tableName);

        Task<GLWBHSS_SchemeDetails> getrptglwbhssschemedetails(long ApplicationId, string schemaName, string tableName);

        Task<GLWBMSL_SchemeDetails> getrptglwb_msl_schemedetails(long ApplicationId, string schemaName, string tableName);
        Task<GLWBCYCLE_Schemedetails> getrptglwb_csy_schemedetails(long ApplicationId, string schemaName, string tableName);

        Task<GLWBhtySchemeDetails> getrptglwb_hty_schemedetails(long ApplicationId, string schemaName, string tableName);
        
        Task<GLWBASYSchemeDetails> getrptglwb_asy_schemedetails(long ApplicationId, string schemaName, string tableName);

        Task<List<DocumentFileDetails>> GetdocumentDetailsByAppId(long ApplicationId, string schemaName, string tableName, long servicesId);

        Task<GLWBHLS_PersonalDetailsModel> GetReportGLWB_HLS_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName);
        Task<GLWBHLS_SchemeDetails> GetReportGLWB_HLS_SchemeDetails(long ApplicationId, string schemaName, string tableName);

        Task<List<PendencyReportDetails>> GetRPTDistrictWisePendencyData(long serviceId, DateTime? dtFromDate, DateTime? dtToDate, long districtId, long talukaId, int beneficiaryType, string schemaName);
        Task<List<EmpApplicationDetailsModel>> GetRPTBOCWApplicationDetailsList(long districtId, long talukaId, DateTime? dtFromDate, DateTime? dtToDate, int status, long postId, int beneficiaryType, string? schemaName);
        Task<List<EmpApplicationDetailsModel>> GetRPTBOCWApplicationDetailsListGLWB_TSY(long districtId, DateTime? dtFromDate, DateTime? dtToDate, int status, long postId, int beneficiaryType, string? schemaName);
        Task<List<EmpApplicationDetailsModel>> GetRPTBOCWPendencyDaysList(long serviceId, long applicationid, int applicationstatus, string? schemaName);
        
        //LSY
        Task<GLWBLSY_PersonalDetails> GetReportGLWB_Lsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName);
        Task<GLWBLSY_SchemeDetails> getrptglwblsyschemedetails(long ApplicationId, string schemaName, string tableName);

        //GLWB_TSY
        Task<GLWB_TSY_personalDetails> GetReportGLWB_Tsy_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName);
        Task<GLWBTSYSchemeDetails> getrptglwb_tsy_schemedetails(long ApplicationId, string schemaName, string tableName);
        Task<List<CompanyWorkerDetails>> getrptglwb_tsy_companyWorkerDetails(long ApplicationId, string schemaName, string tableName);
        Task<List<CompanyWorkerDetails>> getrpt_glwb_tsy_getagecount(long ApplicationId, string schemaName, string tableName);

        //GLWB_TSY_Claim
        Task<GLWB_TSYClaim_personalDetails> GetReportGLWB_Tsy_Claim_PersonalDetailsByAppId(long ApplicationId, long servicesId, string schemaName, string tableName);
        Task<GLWBTSYSchemeDetails> getrptglwb_tsy_claim_schemedetails(long ApplicationId, string schemaName, string tableName);
        Task<List<CompanyWorkerDetails>> getrptglwb_tsy_claim_companyWorkerDetails(long ApplicationId, string schemaName, string tableName);
        Task<List<CompanyWorkerDetails>> getrpt_glwb_tsy_claim_getagecount(long ApplicationId, string schemaName, string tableName);
        //bocw_hssy

        Task<BOCWHssySchemeDetails> getrptbocw_hssy_schemedetails(long ApplicationId, string schemaName, string tableName);
        Task<BOCWNanjiSchemeDetails> getrpt_bocw_nanaji_schemedetails(long ApplicationId, string schemaName, string tableName);
        Task<List<NanajiFamilyMemberDetails>> getrpt_bocw_nanaji_Familymembersdetailsschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<List<familymember>> getrptbocw_hssy_familyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<List<StudentMemberDetails>> getrpt_bocw_hssy_childrenfamilyschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<List<StudentHostelMemberDetails>> getrpt_bocw_hssy_childrenhostelschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<List<FamilyMemberDetails>> getrptbocw_hty_familymembersschemedetails(long ApplicationId, string schemaName, string tableName);
        Task<List<BOCWVR_OtherSchemeDetails>> getrpt_bocw_vr_otherschemedetails(long applicationId, string schemaName, string tableName);
        Task<List<FamilyMemberTravelDetails>> getrptbocw_hty_claim_familymembersschemedetails(long applicationId, string schemaName, string tableName);
    }


}