using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabourCommissioner.Common
{
    public class Procedures
    {
        public static string AddUpdateRegistration = "addregistration";
        public static string BOCW_SSY_AddUpdateApplication = "bocw_ssy_insertpersonaldetails";
        public static string GetLevelNoFromPostId = "getlevelfrompostid";
        public static string BOCW_SSY_AddSchemeDetails = "bocw_ssy_insertschemedetails";
        public static string GetApplicationCountByRegId = "getapplicationcountbyregid";
        public static string GetApplicationCountByRegIdAndServiceId = "getapplicationcountbyregidandserviceid";
        public static string GetUserId = "useridrecovery";
        public static string BOCW_SSY_AddUpdateDocumentDetails = "BOCW_SSY_InsertDocumentDetails";
        public static string BOCW_SSY_InsertTabEntryDocumentDetails = "BOCW_SSY_TabEntry";
        public static string AthenticateUser = "authenticate_cuser";
        public static string AthenticateEmployee = "employeeauthenticate";
        public static string authenticateotheruser = "authenticateotheruser";
        public static string ChangePassword = "changepassword";
        public static string BindServicesUserWiseFilter = "bindservicesuserwisefilter";
        public static string BindAppcountDistrictTalukaWiseForDashboard = "bindappcountdistricttalukawisefordashboard";
        public static string BindAppCountForDashboard = "bindappcountfordashboard";
        public static string BindCountForDashboardGLWB_tsy = "bindappcountfordashboardglwb_tsy";
        public static string BindAdminDashBoardData = "bindadmindashboarddata";
        public static string BindAppCountSchemeWiseForDashboard = "bindappcountschemewisefordashboard";
        public static string BindMenuRoleWise = "bindmenurolewise";
        public static string CheckPermission = "checkpermission";

        public static string GetSchemeByServiceId = "getschemebyserviceid";

        public static string GetServiceId = "getserviceid";
        public static string GetSchemeByBeneficiaryId = "getschemebybeneficiarytypeid";
        public static string CitizenPasswordRecovery = "citizenpasswordrecovery";
        public static string GetServiceTabByServiceId = "getservicetabbyserviceid";
        public static string GetTabSequenceByApplicationId = "GetTabSequenceByApplicationId";
        public static string GetPersonalDetailsByRegNo = "getpersonaldetailsbyregno";
        public static string GetWorkerDetailsByRegNo = "getworkerdetailsbyregno";
        public static string GetCompanyDetailsByUserName = "getcompanydetailsbyusername";
        public static string GetHospitalDetailsByUserName = "gethospitaldetailsbyusername";
        public static string GetApplicationDetailsByAppId = "getapplicationdetailsbyappid";
        public static string GLWB_GetApplicationDetailsByAppId = "glwb_getapplicationdetailsbyappid";
        public static string GLWB_TSY_GetApplicationDetailsByAppId = "glwb_tsy_getapplicationdetailsbyappid";
        public static string GLWB_TSY_Claim_GetApplicationDetailsByAppId = "glwb_tsy_claim_getapplicationdetailsbyappid";
        public static string GetApplicationSchemeDetailsByAppId = "getapplicationschemedetailsbyappid";
        public static string bocw_Hssy_getapplicationschemedetailsbyappid = "bocw_Hssy_getapplicationschemedetailsbyappid";
        public static string BOCW_ACSY_GetApplicationSchemeDetailsByAppId = "bocw_acsy_getapplicationschemedetailsbyappid";
        public static string GetBocw_Tbsy_Claim_ApplicationDetailsByAppId = "bocw_tbsy_claim_getapplicationdetailsbyappid";
        public static string getbocw_tbsyapplicationforhospital = "getbocw_tbsyapplicationforhospital";
        public static string getemployeeuploadeddocumentsbyappid = "bocw_acsy_getemployeeuploadeddocumentsbyappid";
        public static string gethospitaluploadeddocumentsbyappid = "bocw_tbsy_gethospitaluploadeddocumentsbyappid";

        public static string Bocw_vrgetapplicationschemedetailsbyappid = "bocw_vrgetapplicationschemedetailsbyappid";
        public static string Bocw_vrgetapplicationschemedetailsbyappidotherschemedetails = "bocw_vr_getapplicationschemedetailsbyappidotherschemedetails";
        public static string getrpt_bocw_vr_otherschemedetails = "getrpt_bocw_vr_otherschemedetails";

        public static string BOCW_Nanji_GetApplicationSchemeDetailsByAppId = "bocw_nanji_getapplicationschemedetailsbyappid";
        public static string BOCW_VCY_GetApplicationSchemeDetailsByAppId = "bocw_vcy_getapplicationschemedetailsbyappid";
        public static string GLWBHSC_GetApplicationSchemeDetailsByAppId = "glwbhsc_getapplicationschemedetailsbyappid";
        public static string GLWBSSC_GetApplicationSchemeDetailsByAppId = "glwbssc_getapplicationschemedetailsbyappid";
        public static string GetUploadedDocuments = "getuploadeddocuments";
        public static string GetDistrict = "binddistrictmaster";
        public static string GetDistrictForMaster = "binddistrictmasterformaster";
        public static string GetRole = "bindrolemaster";
        public static string GetPostData = "getpostdata";
        public static string getpostmasterdata = "getpostmasterdata";
        public static string getmenumasterdata = "getmenumasterdata";
        public static string getdistrictdata = "getdistrictdata";
        public static string getserviceschedulerdata = "getserviceschedulerdata";
        public static string getvillagedata = "getvillagedata";
        public static string getdocumentmasterdata = "getdocumentmasterdata";
        public static string getresourcedata = "getresourcedata";
        public static string getServicebybenificiaryid = "bindservicemasters";
        public static string bindresourcevalues = "bindresourcevalues";
        public static string bindresourcevaluess = "bindresourcevaluess";
        public static string getdocumentmaster = "getdocumentmaster";
        public static string gettalukadata = "gettalukadata";
        public static string gettalukamasterdata = "gettalukamasterdata";
        public static string GetDistrictMaster = "getdistrictmaster";
        public static string getServiceschedulermaster = "getServiceschedulermaster";
        public static string getvillagemasters = "getvillagemasters";
        public static string binddiseasesandinjuriesmaster = "binddiseasesandinjuriesmaster";
        public static string GetFinancialYearList = "getallfinancialyearlist";
        public static string bindglwbemployee = "bindglwbemployee";
        public static string GetGLWBEmployeeDetailsbyRegistrationid = "getglwbemployeedetailsbyregistrationid";
        public static string GetDivision = "binddivisionmaster";
        public static string GetEmployeeRoleForApprove = "getemprolesforapprove";
        public static string GetEmployeeRoleForSendBack = "getemprolesforsendback";
        public static string GetSendBackRoleGLWb_TSY = "getemprolesforsendbackglwb_tsy";
        public static string GetApplicationHistory = "bocw_ssy_getapprovaldetails";
        public static string GetApplicationHistory_Glwb_Tsy_claim = "glwb_tsy_claim_getapprovaldetails";
        public static string GetHospitalScheduleTime = "gethospitalscheduletime";
        public static string GetAllStates = "bindstatemaster";
        public static string GetTalukaByDistrictId = "bindtalukamaster";
        public static string GetSubServiceByServiceId = "bindsubservicemaster";
        public static string GetDistrictNamebyDivisionId = "getdistrictnamebydivisionid";
        public static string GetSemesterbyCourseId = "bindsemester";
        public static string GetBenifitByCourseId = "getbenifitsrs";

        public static string GetTotalsahayByServiceID = "gettotalsahaybyserviceid";

        public static string Get_BOCW_BPSY_TotalsahayByServiceID = "get_bocw_bpsy_totalsahaybyserviceid";
        public static string GetVillageByDistrictIdAndTalukaId = "bindvillagemaster";
        public static string GetVillageByDistrictIdAndTalukaIds = "bindvillagemasters";
        public static string BindResourceValue = "bindresourcevalue";
        public static string GetFileDocuments = "getdocumentlabel";
        public static string GetSchemeUsers = "getschemeusers";
        public static string GetCitizen = "getapplication";
        public static string GetApplication = "getapplication";
        public static string GetGLWB_HTYApplication = "getglwb_htyapplication";
        public static string Glwb_TSY_Claim_GetApplication = "glwb_tsy_claim_getapplication";
        public static string GetBocw_TBSYApplication = "getbocw_tbsyapplication";
        public static string Glwb_TSY_GetApplication = "glwb_tsy_getapplication";
        public static string Bocw_Tbsy_GetApplication = "bocw_tbsy_getapplication";
        public static string Glwb_TSY_Hospital_GetApplication = "glwb_tsy_hospital_getapplication";
        public static string GLWB_HSC_GetApplication = "glwb_hsc_getapplication";
        public static string GetDegreeBySubjectId = "bindcoursemaster";
        public static string GetLastLevel = "getislastlevel";
        public static string GetReportPersonalDetailsByAppId = "getrptpersonaldetails";
        public static string GetReportGLWB_SSC_PersonalDetailsByAppId = "getrpt_glwb_ssc_personaldetails";
        public static string GetReportGLWB_PSY_PersonalDetailsByAppId = "getrpt_glwb_psy_personaldetails";
        public static string getrpt_glwb_asy_personaldetails = "getrpt_glwb_asy_personaldetails";
        public static string GetReportGLWB_Hsc_PersonalDetailsByAppId = "getrpt_glwb_hsc_personaldetails";
        public static string GetReportGLWB_adsy_PersonalDetailsByAppId = "getrpt_glwb_adsy_personaldetails";

        public static string GetReportGLWB_MSL_PersonalDetailsByAppId = "getrpt_glwb_msl_personaldetails";
        public static string GetReportGLWB_CSY_PersonalDetailsByAppId = "getrpt_glwb_csy_personaldetails";

        public static string GetSchemeDetailsByAppId = "getrptschemedetails";
        public static string GetdocumentDetailsByAppId = "getrptdocumentdetails";
        public static string GetSmsContentForService = "getsmscontentforservice";
        public static string GLWB_TSY_GetSmsContentForService = "glwb_tsy_getsmscontentforservice";
        public static string GLWB_TSY_claim_GetSmsContentForService = "glwb_tsy_claim_getsmscontentforservice";


        public static string GLWBPSY_GetApplicationSchemeDetailsByAppId = "glwb_psy_getapplicationschemedetailsbyappid";
        public static string GLWBASY_GetApplicationSchemeDetailsByAppId = "glwb_asy_getapplicationschemedetailsbyappid";
        public static string GLWBADSY_GetApplicationSchemeDetailsByAppId = "glwb_adsy_getapplicationschemedetailsbyappid";
        public static string glwb_hty_getapplicationschemedetailsbyappid = "glwb_hty_getapplicationschemedetailsbyappid";
        public static string glwb_hty_claim_getapplicationschemedetailsbyappid = "glwb_hty_claim_getapplicationschemedetailsbyappid";


        public static string getrpt_bocw_bpsy_schemedetails = "getrpt_bocw_bpsy_schemedetails";
        public static string getrpt_bocw_tbsy_schemedetails = "getrpt_bocw_tbsy_schemedetails";
        public static string getrpt_bocw_psy_schemedetails = "getrpt_bocw_psy_schemedetails";
        public static string getrpt_glwb_hsc_schemedetails = "getrpt_glwb_hsc_schemedetails";
        public static string getrpt_glwb_psy_schemedetails = "getrpt_glwb_psy_schemedetails";
        public static string getrpt_glwb_ssc_schemedetails = "getrpt_glwb_ssc_schemedetails";
        public static string getrpt_bocw_asy_schemedetails = "getrpt_bocw_asy_schemedetails";
        public static string getrpt_bocw_tsy_schemedetails = "getrpt_bocw_tsy_schemedetails";
        public static string getrpt_bocw_vcy_schemedetails = "getrpt_bocw_vcy_schemedetails";
        public static string getrpt_bocw_pip_schemedetails = "getrpt_bocw_pip_schemedetails";
        public static string getrptglwb_hty_schemedetails = "getrpt_glwb_hty_schemedetails";
        public static string getrptglwb_hty_claim_schemedetails = "getrpt_glwb_hty_claim_schemedetails";
        public static string getrpt_glwb_asy_schemedetails = "getrpt_glwb_asy_schemedetails";
        
        public static string getrptbocwvrschemedetails = "getrpt_bocw_vr_schemedetails";
        public static string getrpt_bocw_acsy_schemedetails = "getrpt_bocw_acsy_schemedetails";
        public static string GetReportGLWB_adsy_SchemeDetails = "getrpt_glwb_adsy_schemedetails";


        public static string GetReportGLWB_tsy_PersonalDetailsByAppId = "GetReportGLWB_tsy_PersonalDetailsByAppId";
        public static string GetReportGLWB_HTY_PersonalDetailsByAppId = "getrpt_glwb_hty_personaldetails";

        public static string getrptglwb_msl_schemedetails = "getrpt_glwb_msl_schemedetails";
        public static string getrptglwb_csy_schemedetails = "getrpt_glwb_csy_schemedetails";


        public static string BOCW_BPSY_GetApplicationSchemeDetailsByAppId = "bocw_bpsy_getapplicationschemedetailsbyappid";
        public static string BOCW_PSY_GetApplicationSchemeDetailsByAppId = "bocw_psy_getapplicationschemedetailsbyappid";
        public static string GLWB_CSY_GetApplicationSchemeDetailsByAppId = "glwbcsy_getapplicationschemedetailsbyappid";


        //add Antyesthi
        public static string BOCW_asy_AddSchemeDetails = "bocw_asy_insertschemedetails";
        public static string BOCW_asy_AddUpdateApplication = "bocw_asy_insertpersonaldetails";
        public static string BOCW_asy_GetApplicationSchemeDetailsByAppId = "bocw_asy_getapplicationschemedetailsbyappid";
        public static string BOCW_tbsy_GetApplicationSchemeDetailsByAppId = "bocw_tbsy_getapplicationschemedetailsbyappid";
        public static string bocw_hssy_getapplicationschemedetailsbyappid = "bocw_hssy_getapplicationschemedetailsbyappid";


        public static string bocw_hssy_getapplicationschemedetailsbyappidHostel = "bocw_hssy_getapplicationschemedetailsbyappidHostel";
        public static string bocw_hssy_getapplicationschemedetailsbyappidfamilydetails = "bocw_hssy_getapplicationschemedetailsbyappidfamilydetails";

        //glwb_tabibi sahay
        public static string glwb_tsy_GetApplicationSchemeDetailsByAppId = "glwb_tsy_getapplicationschemedetailsbyappid";
        public static string glwb_tsy_claim_GetApplicationSchemeDetailsByAppId = "glwb_tsy_claim_getapplicationschemedetailsbyappid";
        public static string Glwb_Tsy_getapplicationschemedetailsbyappidclaim = "glwb_tsy_getapplicationschemedetailsbyappidclaim";
        public static string getrptglwb_tsy_schemedetails = "getrpt_glwb_tsy_schemedetails";
        public static string getrptglwb_tsy_companyWorkerDetails = "getrpt_glwb_tsy_companyworkerdetails";
        public static string getrpt_glwb_tsy_getagecount = "getrpt_glwb_tsy_getagecount";
         
        // add tablet sahay
        public static string BOCW_tsy_GetApplicationSchemeDetailsByAppId = "bocw_tsy_getapplicationschemedetailsbyappid";
        public static string BOCW_tsy_AddSchemeDetails = "bocw_tsy_insertschemedetails";
        public static string BOCW_tsy_AddUpdateApplication = "bocw_tsy_insertpersonaldetails";

        // add PIP sahay
        public static string BOCW_pip_GetApplicationSchemeDetailsByAppId = "bocw_pip_getapplicationschemedetailsbyappid";
        public static string BOCW_pip_AddSchemeDetails = "bocw_pip_insertschemedetails";
        public static string BOCW_pip_AddUpdateApplication = "bocw_pip_insertpersonaldetails";
        //add MSL sahay 
        public static string GLWBMSL_GetApplicationSchemeDetailsByAppId = "glwbmsl_getapplicationschemedetailsbyappid";

        //public static string BOCW_SSY_GetApplicationDetailsList = "bocw_ssy_getapplicationdetailslist";
        public static string BOCW_GetApplicationDetailsList = "bocw_getapplicationdetailslist";
        public static string GLWB_GetApplicationDetailsList = "glwb_getapplicationdetailslist";
        public static string GLWB_Hospital_GetApplicationDetailsList = "glwb_tsy_getapplicationdetailslist";
        public static string GLWBHospitalApplicationDetailsClaim = "glwb_tsy_claim_getapplicationdetailslist";
        public static string GetUploadedDocumentForEmployee = "getuploadeddocumentsforemployee";
        public static string GetGlwb_TsyUploadedDocumentForEmployee = "getGLWB_TsyUploadeddocumentsforemployee";
        public static string AddMultipleApprovalDetails = "addmultipleapprovaldetails";
        public static string bocw_tbsy_claim_addmultipleapprovaldetails = "bocw_tbsy_claim_addmultipleapprovaldetails";
        public static string GLWB_HTY_AddMultipleApprovalDetails = "glwb_hty_addmultipleapprovaldetails";

        //Add home Loan Yojana
        public static string GLWB_HLS_Insertschemedetails = "glwb_hls_insertschemedetails";
        public static string GLWB_HLS_Getschemedetails = "glwb_hls_getschemedetails";
        public static string getrpt_glwb_hls_personaldetails = "getrpt_glwb_hls_personaldetails";
        public static string getrpt_glwb_hls_schemedetails = "getrpt_glwb_hls_schemedetails";


        

        //HSS
        public static string GLWBHSS_GetApplicationSchemeDetailsByAppId = "glwbhss_getapplicationschemedetailsbyappid";
        public static string GetReportGLWB_Hss_PersonalDetailsByAppId = "getrpt_glwb_hss_personaldetails";
        public static string getrpt_glwb_hss_schemedetails = "getrpt_glwb_hss_schemedetails";

       
        public static string RPTBOCWGETApplicationDetailsList = "rptbocwgetapplicationdetailslist";
        public static string GetRPTBOCWApplicationDetailsListGLWB_TSY = "rptbocwgetapplicationdetailslistglwb_tsy";
        public static string GetRPTDistrictWisePendencyData = "rptdistrictwisependencydata";
        public static string RPTBOCWGETPendencyDaysList = "rptgetpendencydayslist";

        //dynamic add row schemes
        public static string GetApplicationSchemeDetails = "GetApplicationSchemeDetails";

        //LSY
        public static string GLWBLSY_GetApplicationSchemeDetailsByAppId = "glwblsy_getapplicationschemedetailsbyappid";
        public static string GetReportGLWB_Lsy_PersonalDetailsByAppId = "getrpt_glwb_lsy_personaldetails";
        public static string getrpt_glwb_lsy_schemedetails = "getrpt_glwb_lsy_schemedetails";

        //GLWB_TSY
        public static string GetReportGLWB_Tsy_PersonalDetailsByAppId = "getrpt_glwb_tsy_personaldetails";


        //GLWB_TSY_Claim
        public static string GetReportGLWB_Tsy_Claim_PersonalDetailsByAppId = "getrpt_glwb_tsy_claim_personaldetails";
        public static string getrptglwb_tsy_claim_schemedetails = "getrpt_glwb_tsy_claim_schemedetails";
        public static string getrptglwb_tsy_claim_companyWorkerDetails = "getrpt_glwb_tsy_claim_companyworkerdetails";
        public static string getrpt_glwb_tsy_claim_getagecount = "getrpt_glwb_tsy_claim_getagecount";


        //bocw_hssy

        public static string getrptbocw_hssy_schemedetails = "getrpt_bocw_hssy_schemedetails";
        public static string getrpt_bocw_nanaji_schemedetails = "getrpt_bocw_nanaji_schemedetails";
        public static string getrpt_bocw_hssy_familyschemedetails = "getrpt_bocw_hssy_familyschemedetails";
        public static string getrpt_bocw_hssy_childrenfamilyschemedetails = "getrpt_bocw_hssy_childrenfamilyschemedetails";
        public static string getrpt_bocw_hssy_childrenhostelschemedetails = "getrpt_bocw_hssy_childrenhostelschemedetails";
        public static string getrpt_bocw_nanaji_Familymembersdetailsschemedetails = "getrpt_bocw_nanaji_Familymembersdetailsschemedetails";
        public static string getrpt_glwb_hty_Familyssschemedetails = "getrpt_glwb_hty_Familyssschemedetails";
        public static string getrpt_glwb_hty_claim_Familyssschemedetails = "getrpt_glwb_hty_claim_Familyssschemedetails";
        public static string GetRegisteredApplicantDetails = "getregisteredapplicantdetails";
        public static string ViewApplicationStatusNew = "viewapplicationstatusnew";
        public static string GetRoleForApplicationStatus = "getroleforapplicationstatus";
        public static string GetTotalApplicationDetails = "gettotalapplicationdetails";
        public static string GetServiceMasterByBeneficiaryId = "bindservicemaster";
        public static string GetServiceMasterByBeneficiaryIdforCMD = "bindservicemasterforcmd";
        public static string CheckENirmanCardExpiry = "checkenirmancardexpiry";
        public static string UpdateeNirmanCardxpiryDate = "updateenirmancardexpirydate";
        public static string ViewApplicationEmployeeSearch = "viewapplicationemployeesearch";
        public static string GetRoleForApplicationStatusForEmployee = "getroleforapplicationstatusforemployee";
        public static string getaadharcardcountbyaadharnoandserviceid = "getaadharcardcountbyaadharnoandserviceid";
        public static string BOCWGetCompletedApplicationForPayment = "bocw_getcompletedapplicationforpayment";
        public static string GetAppDetailsByAppIdForViewAadesh = "getappdetailsbyappidforviewaadesh";
        public static string BOCWViewAadeshDetails = "bocw_viewaadeshdetails";
        public static string GetAadeshDetailsbyAadeshid = "getaadeshdetailsbyaadeshid";
        public static string BOCWSendForPayment = "bocwsendforpayment";
        public static string BOCWGetAadeshDataForRoutine = "bocwgetaadeshdataforroutine";
        public static string UpdateBOCWPaymentInfo = "updatebocwpaymentinfo";

        public static string GetAppDetailsByAppIdForGLWBViewAadesh = "getappdetailsbyappidforglwbviewaadesh";
        public static string GLWBViewAadeshDetails = "glwb_viewaadeshdetails";
        public static string Glwb_Tsy_ViewAadeshDetails = "glwb_tsy_viewaadeshdetails";
        public static string GetGLWBAadeshDetailsbyAadeshid = "getglwbaadeshdetailsbyaadeshid";
        public static string GetGlwbTSYAadeshDetailsbyAadeshid = "getglwbTSYaadeshdetailsbyaadeshid";

        public static string BOCWGetAadeshDataForFetchReturnCSVFile = "bocwgetaadeshdataforreturncsvfile";
        public static string SaveBOCWPaymentResponse = "savebocwpaymentresponse";
        public static string GetBOCWPaymentHistory = "getbocwpaymenthistory";
        public static string GLWBSendForPayment = "glwbsendforpayment";

        public static string GLWBGetAadeshDataForRoutine = "glwbgetaadeshdataforroutine";
        public static string UpdateGLWBPaymentInfo = "updateglwbpaymentinfo";
        public static string GLWBGetAadeshDataForFetchReturnCSVFile = "glwbgetaadeshdataforreturncsvfile";
        public static string GetGLWBPaymentHistory = "getglwbpaymenthistory";
        public static string BOCWSendForPaymentFailedRecord = "bocwsendforpaymentfailedrecord";
        public static string GLWBSendForPaymentFailedRecord = "glwbsendforpaymentfailedrecord";
        public static string GetCCApplicationDetails = "cesscollection.getccapplicationdetails";
        public static string GetCCCompletedAppForPayment = "cesscollection.getcccompletedappforpayment";
        public static string GetCCApplicationDetailsByAppId = "cesscollection.getccapplicationdetailsbyappid";
        public static string InsertPaymentTransactionInfo = "cesscollection.insertpaymenttransactioninfo";
        public static string CheckTransactionTokenExistorNot = "cesscollection.checktransactiontokenexistornot";
        public static string UpdatePaymentTransactionInfo = "cesscollection.updatepaymenttransactioninfo";
        public static string GetDataForCTPMakePayment = "cesscollection.getdataforctpmakepayment";

        public static string GetYear = "getallyearslist";
        public static string GetMonth = "getallmonthlist";
        public static string GetCMDApplicationDetailslist = "cmdashboard.getcmdapplicationdetailslist";
        public static string GetCMDApplicationDetailsForInsert = "cmdashboard.getcmdapplicationdetailsforinsert";
        public static string GetBOCWCMDApplicationDetails = "cmdashboard.api_getcmdapplicationdetails";

    }
}