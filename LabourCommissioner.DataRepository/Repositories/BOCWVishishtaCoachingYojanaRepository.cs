using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dapper;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NpgsqlTypes;
using static Dapper.SqlMapper;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissioner.DataRepository.Repositories
{
    public class BOCWVishishtaCoachingYojanaRepository : BaseRepository<TabModel>, IBOCWVishishtaCoachingYojanaRepository
    {
        public IConfiguration appConfig;
        public BOCWVishishtaCoachingYojanaRepository(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }
        public async Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", ServiceId);
                    var result = (await conn.QueryAsync<TabModel>(Procedures.GetServiceTabByServiceId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", Convert.ToInt64(ApplicationId));
                    queryParameters.Add("in_tblname", tablename);
                    queryParameters.Add("in_schemaname", schemaname);
                    queryParameters.Add("in_serviceid", Convert.ToInt64(id));
                    var result = (await conn.QueryAsync<TabModel>(Procedures.GetTabSequenceByApplicationId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SelectListItem>> GetAllStates()
        {
            try
            {

                using (var conn = GetConnection())
                {

                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetAllStates, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_regno", RegistrationNo);

                    var result = await conn.QueryFirstOrDefaultAsync<PersonalDetailsModel>(Procedures.GetPersonalDetailsByRegNo, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_schemaname", schemaname);
                    queryParameters.Add("in_tablename", tablename);

                    var result = await conn.QueryFirstOrDefaultAsync<PersonalDetailsModel>(Procedures.GetApplicationDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BOCWVCYSchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);

                    var result = await conn.QueryFirstOrDefaultAsync<BOCWVCYSchemeDetails>(Procedures.BOCW_VCY_GetApplicationSchemeDetailsByAppId, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<BOCWVCYSchemeDetails> GetTotalsahayByServiceID(int serviceId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", serviceId);
                    queryParameters.Add("_relation", 0);
                    queryParameters.Add("_gender", 0);
                    queryParameters.Add("_other1", 0);
                    queryParameters.Add("_tharavid", 0);
                    var result = (await conn.QueryAsync<BOCWVCYSchemeDetails>(Procedures.GetTotalsahayByServiceID, queryParameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_serviceid", serviceId);
                    queryParameters.Add("in_schemaname", schemaname);
                    queryParameters.Add("in_tablename", tablename);

                    var result = (await conn.QueryAsync<DocumentFileDetails>(Procedures.GetUploadedDocuments, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    //var result = (await conn.QueryAsync<IList<DocumentFileDetails>>(Procedures.GetUploadedDocuments, queryParameters, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<SelectListItem>> GetDistrict()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetDistrict, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetSubject(string subjectId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_subjectid", Convert.ToInt32(subjectId));
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetDegreeBySubjectId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtId);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetTalukaByDistrictId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetSemesterbyCourseId(int courseid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_courseid", courseid);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetSemesterbyCourseId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable> GetBenifitByCourseId(int courseId, string semesteryear)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_courseid", courseId);
                    queryParameters.Add("_semesteryear", semesteryear);
                    var result = (await conn.QueryAsync(Procedures.GetBenifitByCourseId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtId);
                    queryParameters.Add("_talukaid", talukaId);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetVillageByDistrictIdAndTalukaId, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_resourcetype", ResourceType);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.BindResourceValue, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", ServiceId);

                    var result = (await conn.QueryAsync<DocumentDetails>(Procedures.GetFileDocuments, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<ResponseMessage> AddUpdateApplication(PersonalDetailsModel personalDetailsModel)

        public async Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceid", ServiceId);
                    queryParameters.Add("_applicationid", ApplicationId);

                    var result = (await conn.QueryAsync<DocumentFileDetails>(Procedures.GetFileDocuments, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> AddUpdateApplication(PersonalDetailsModel personalDetailsModel)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.bocw_vcy_insertpersonaldetails(@in_registrationid,@in_userprofileid,@in_photo,@in_name,@in_nameingujarati," +
                        "@in_fatherorhusbandnameingujarati,@in_fatherorhusbandname,@in_dateofbirth,@in_mobileno,@in_phoneno,@in_emailid,@in_casteid,@in_gender,@in_isdisabled," +
                        "@in_caddressineng,@in_caddressinguj,@in_cdistrictid,@in_ctalukaid ,@in_cvillageid,@in_cpincode,@in_paddressineng,@in_paddressinguj," +
                        "@in_pstateid,@in_pdistrictid,@in_ptalukaid,@in_pvillageid,@in_ppincode,@in_createdby,@in_createddate,@in_isdeleted,@in_ipaddress,@in_hostname," +
                        "@in_sequenceno,@in_couchdbdocid,@in_couchdbdocrevid,@in_aadharcardno,@in_age,@in_pdistrictname,@in_ptalukaname,@in_pvillagename,@in_applicationno,@in_serviceid," +
                        "@in_schemaname,@in_tablename,@in_applicationfrom,@out_error,@out_msg,@out_id,@out_applicatinno)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_registrationid", personalDetailsModel.RegistrationId);
                    queryParameters.Add("@in_userprofileid", personalDetailsModel.UserProfileId);
                    queryParameters.Add("@in_photo", personalDetailsModel.FileName);
                    queryParameters.Add("@in_name", personalDetailsModel.Name);
                    queryParameters.Add("@in_nameingujarati", personalDetailsModel.NameinGujarati);
                    queryParameters.Add("@in_fatherorhusbandnameingujarati", personalDetailsModel.FatherOrHusbandNameinGujarati);
                    queryParameters.Add("@in_fatherorhusbandname", personalDetailsModel.FatherOrHusbandName);
                    queryParameters.Add("@in_dateofbirth", personalDetailsModel.DateOfBirth);
                    queryParameters.Add("@in_mobileno", personalDetailsModel.MobileNo);
                    queryParameters.Add("@in_phoneno", personalDetailsModel.PhoneNo);
                    queryParameters.Add("@in_emailid", personalDetailsModel.EmailId);
                    queryParameters.Add("@in_casteid", personalDetailsModel.CasteId);
                    queryParameters.Add("@in_gender", personalDetailsModel.Gender);
                    queryParameters.Add("@in_isdisabled", personalDetailsModel.isDisabled);
                    queryParameters.Add("@in_caddressineng", personalDetailsModel.CAddressInEng);
                    queryParameters.Add("@in_caddressinguj", personalDetailsModel.CAddressInGuj);
                    queryParameters.Add("@in_cdistrictid", personalDetailsModel.CDistrictId);
                    queryParameters.Add("@in_ctalukaid", personalDetailsModel.CTalukaId);
                    queryParameters.Add("@in_cvillageid", personalDetailsModel.CVillageId);
                    queryParameters.Add("@in_cpincode", personalDetailsModel.CPinCode);
                    queryParameters.Add("@in_paddressineng", personalDetailsModel.PAddressInEng);
                    queryParameters.Add("@in_paddressinguj", personalDetailsModel.PAddressInGuj);
                    queryParameters.Add("@in_pstateid", personalDetailsModel.PStateId);
                    queryParameters.Add("@in_pdistrictid", personalDetailsModel.PDistrictId);
                    queryParameters.Add("@in_ptalukaid", personalDetailsModel.PTalukaId);
                    queryParameters.Add("@in_pvillageid", personalDetailsModel.PVillageId);
                    queryParameters.Add("@in_ppincode", personalDetailsModel.PPinCode);
                    queryParameters.Add("@in_createdby", personalDetailsModel.CreatedBy);
                    queryParameters.Add("@in_createddate", personalDetailsModel.CreatedDate);
                    queryParameters.Add("@in_isdeleted", personalDetailsModel.IsDeleted);
                    queryParameters.Add("@in_ipaddress", personalDetailsModel.IpAddress);
                    queryParameters.Add("@in_hostname", personalDetailsModel.HostName);
                    queryParameters.Add("@in_serviceid", personalDetailsModel.ServiceId);
                    queryParameters.Add("@in_sequenceno", personalDetailsModel.TabSequenceNo);
                    queryParameters.Add("@in_couchdbdocid", personalDetailsModel.CouchDBDocId);
                    queryParameters.Add("@in_couchdbdocrevid", personalDetailsModel.CouchDBDocRevId);
                    queryParameters.Add("@in_aadharcardno", personalDetailsModel.AadharCardNo);
                    queryParameters.Add("@in_age", personalDetailsModel.Age);
                    queryParameters.Add("@in_pdistrictname", personalDetailsModel.PDistrictNameInEng);
                    queryParameters.Add("@in_ptalukaname", personalDetailsModel.PTalukaNameInEng);
                    queryParameters.Add("@in_pvillagename", personalDetailsModel.PVillageNameInEng);
                    queryParameters.Add("@in_applicationno", personalDetailsModel.ApplicationNo);
                    queryParameters.Add("@in_schemaname", personalDetailsModel.schemaname);
                    queryParameters.Add("@in_tablename", personalDetailsModel.tablename);
                    queryParameters.Add("@in_applicationfrom", personalDetailsModel.applicationfrom);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
                    //var result = await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.BOCW_SSY_AddUpdateApplication, queryParameters);
                    // var result = await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.BOCW_SSY_AddUpdateApplication, queryParameters);

                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<long>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Id = queryParameters.Get<long>("@out_id");
                    res.ApplicationNo = queryParameters.Get<long>("@out_applicatinno");


                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> AddSchemeDetails(BOCWVCYSchemeDetails schemeDetails)
        {

            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL bocw_vcy_insertschemedetails(@in_applicationid,@in_serviceid,@in_userid,@in_sequenceno," +
                        "@in_studentname,@in_dateofbirth,@in_compcoachingdetails,@in_acadmicyearsem,@in_admissionstartdate,@in_institutename," +
                        "@in_registrationfee,@in_coachingfee,@in_trainingfee,@in_examfee,@in_totalsahay,@in_identitycardrenewal,@in_renewaldate,@in_createdby," +
                        "@in_isdeleted,@in_ipaddress,@in_hostname,@in_bankname,@in_bankbranch,@in_ifsccode,@in_bankaccountno,@in_accountholdername," +
                        "@in_schemaname,@in_tablename," +
                        "@out_error,@out_msg,@out_id,@out_applicatinno)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationid", schemeDetails.ApplicationId);
                    queryParameters.Add("@in_serviceid", schemeDetails.ServiceId);
                    queryParameters.Add("@in_userid", schemeDetails.UserId);
                    queryParameters.Add("@in_sequenceno", schemeDetails.TabSequenceNo);                    
                    queryParameters.Add("@in_studentname", schemeDetails.studentname);                    
                    queryParameters.Add("@in_dateofbirth", schemeDetails.dateofbirth);                    
                    queryParameters.Add("@in_compcoachingdetails", schemeDetails.compcoachingdetails);                    
                    queryParameters.Add("@in_acadmicyearsem", schemeDetails.acadmicyearsem);                    
                    queryParameters.Add("@in_admissionstartdate", schemeDetails.admissionstartdate);                    
                    queryParameters.Add("@in_institutename", schemeDetails.institutename);                    
                    queryParameters.Add("@in_registrationfee", schemeDetails.registrationfee);                    
                    queryParameters.Add("@in_coachingfee", schemeDetails.coachingfee);                    
                    queryParameters.Add("@in_trainingfee", schemeDetails.trainingfee);                    
                    queryParameters.Add("@in_examfee", schemeDetails.examfee);                    
                    queryParameters.Add("@in_totalsahay", schemeDetails.totalsahay);                    
                    queryParameters.Add("@in_identitycardrenewal", schemeDetails.identitycardrenewal);                    
                    queryParameters.Add("@in_renewaldate", schemeDetails.renewaldate);                    
                    queryParameters.Add("@in_createdby", schemeDetails.CreatedBy);
                    queryParameters.Add("@in_isdeleted", schemeDetails.IsDeleted);
                    queryParameters.Add("@in_ipaddress", schemeDetails.IpAddress);
                    queryParameters.Add("@in_hostname", schemeDetails.HostName);
                    queryParameters.Add("@in_bankname", schemeDetails.BankName);
                    queryParameters.Add("@in_bankbranch", schemeDetails.BankBranch);
                    queryParameters.Add("@in_ifsccode", schemeDetails.IFSCCode);
                    queryParameters.Add("@in_bankaccountno", schemeDetails.BankAccountNo);
                    queryParameters.Add("@in_accountholdername", schemeDetails.AccountHolderName);
                    queryParameters.Add("@in_schemaname", schemeDetails.schemaname);
                    queryParameters.Add("@in_tablename", schemeDetails.tablename);                    
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
                    //var result = await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.BOCW_SSY_AddSchemeDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<long>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.Id = queryParameters.Get<long>("@out_id");
                    res.ApplicationNo = queryParameters.Get<long>("@out_applicatinno");
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails)
        {
            try
            {
                var procName = "CALL public.bocw_vcy_insertdocumentdetails(@in_applicationid,@in_documentmasterid,@in_documentname,@in_documentnumber,@in_createddate,@in_CreatedBy," +
                       "@in_ModifiedBy,@in_modifieddate,@in_isdeleted,@in_IpAddress,@in_HostName,@in_CouchDBDocId," +
                       "@in_serviceid,@in_sequenceno,@out_error,@out_msg,@out_applicationid,@out_id)";
                ResponseMessage responseMessage = new ResponseMessage();

                using (var conn = GetConnection())
                {
                    int iteration = 0;
                    foreach (var item in lstdocumentFileDetails)
                    {
                        var queryParameters = new DynamicParameters();
                        queryParameters.Add("@in_applicationid", item.ApplicationId);
                        queryParameters.Add("@in_documentmasterid", item.DocumentMasterId);
                        queryParameters.Add("@in_documentname", Convert.ToString(item.DocumentName));
                        queryParameters.Add("@in_documentnumber", Convert.ToString(item.DocumentNumber), null);
                        queryParameters.Add("@in_createddate", item.CreatedDate);
                        queryParameters.Add("@in_CreatedBy", item.CreatedBy);
                        queryParameters.Add("@in_ModifiedBy", item.ModifiedBy, null);
                        queryParameters.Add("@in_modifieddate", item.ModifiedDate, null);
                        queryParameters.Add("@in_isdeleted", item.IsDeleted);
                        queryParameters.Add("@in_IpAddress", item.IpAddress);
                        queryParameters.Add("@in_HostName", item.HostName);
                        queryParameters.Add("@in_CouchDBDocId", item.CouchDBDocId);
                        queryParameters.Add("@in_serviceid", item.ServiceId);
                        queryParameters.Add("@in_sequenceno", item.TabSequenceNo);
                        queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                        queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                        queryParameters.Add("@out_applicationid", 0, direction: ParameterDirection.InputOutput);
                        queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                        // var result = await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.BOCW_SSY_AddUpdateDocumentDetails, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                        var result = conn.Execute(procName, queryParameters);
                        if (result != null)
                        {
                            responseMessage.Error = queryParameters.Get<long>("@out_error");
                            responseMessage.Msg = queryParameters.Get<string>("@out_msg");
                            responseMessage.ApplicationNo = queryParameters.Get<long>("@out_applicationid");
                            responseMessage.Id = queryParameters.Get<long>("@out_id");
                            //responseMessage.ApplicationNo = queryParameters.Get<long>("@out_applicatinno");
                        }

                        iteration++;
                        if (iteration < lstdocumentFileDetails.Count)
                        {
                            var procName1 = "CALL public.bocw_vcy_tabentry(@in_applicationid,@in_serviceid,@in_userid,@in_sequenceno)";
                            var queryParametersTab = new DynamicParameters();
                            queryParametersTab.Add("@in_applicationid", item.ApplicationId);
                            queryParametersTab.Add("@in_serviceid", item.ServiceId);
                            queryParametersTab.Add("@in_userid", item.CreatedBy);
                            queryParametersTab.Add("@in_sequenceno", item.SequenceNo);

                            //var tabResult = await conn.QueryFirstOrDefaultAsync<ResponseMessage>(Procedures.BOCW_SSY_InsertTabEntryDocumentDetails, queryParametersTab, commandType: System.Data.CommandType.StoredProcedure);
                            var tabResult = conn.Execute(procName1, queryParametersTab);
                        }
                    }



                    return responseMessage;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.bocw_vcy_addupdateappdeclaration(@in_applicationid,@in_registrationid,@in_userid,@in_isagree,@in_ipaddress,@in_hostname," +
                        "@in_schemaname,@in_tablename,@in_sequenceno,@in_serviceid,@in_greensoldierreferralcode," +
                        "@out_error,@out_applicationname,@out_applicationid,@out_msg,@out_email)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_applicationid", finalSubmitModel.ApplicationId);
                    queryParameters.Add("@in_registrationid", finalSubmitModel.ResigtrationId);
                    queryParameters.Add("@in_userid", finalSubmitModel.userid);
                    queryParameters.Add("@in_isagree", finalSubmitModel.isagree);
                    //queryParameters.Add("@in_issubmitted", finalSubmitModel.issubmitted, DbType.Boolean);
                    queryParameters.Add("@in_ipaddress", finalSubmitModel.ipaddress);
                    queryParameters.Add("@in_hostname", finalSubmitModel.hostname);
                    queryParameters.Add("@in_schemaname", finalSubmitModel.schemaname);
                    queryParameters.Add("@in_tablename", finalSubmitModel.tablename);
                    // queryParameters.Add("@in_redirecturl", finalSubmitModel.redirecturl);
                    queryParameters.Add("@in_sequenceno", finalSubmitModel.tabsequenceno);
                    queryParameters.Add("@in_serviceid", finalSubmitModel.serviceid);
                    queryParameters.Add("@in_greensoldierreferralcode", finalSubmitModel.greensoldierreferralcode);
                    //queryParameters.Add("@in_benificiarytype", finalSubmitModel.benificiarytype);
                    //queryParameters.Add("@in_itiid", finalSubmitModel.itiid);
                    //queryParameters.Add("@in_districtid", finalSubmitModel.districtid);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicationname", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicationid", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_email", "", direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);

                     res.Error = queryParameters.Get<long>("@out_error");
                     res.email = queryParameters.Get<string>("@out_email");
                     res.Id = queryParameters.Get<long>("@out_applicationid");
                     res.Msg = queryParameters.Get<string>("@out_msg");
                    res.ApplicantName = queryParameters.Get<string>("@out_applicationname");

                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData)    
        {

            try
            {
                var dsdsd = EnumLookup.DataTableToJson(dtData);
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.bocw_vcy_insertdocumentdetails_json(@in_documenttbl,@in_isupdated,@in_schemaname,@in_tablename,@out_error,@out_msg,@out_applicationid,@out_id)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_documenttbl",new JsonParameter(JsonConvert.SerializeObject(dtData)));
                    queryParameters.Add("@in_isupdated", false);
                    queryParameters.Add("@in_schemaname", Convert.ToString(dtData.Rows[0]["schemaname"]));
                    queryParameters.Add("@in_tablename", Convert.ToString(dtData.Rows[0]["tablename"]));
                    queryParameters.Add("@out_error",0,direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg","",direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicationid", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);

                    var result = conn.Execute(procName, queryParameters);
                    res.Error = queryParameters.Get<Int64>("@out_error");
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    res.ApplicationNo = queryParameters.Get<long>("@out_applicationid");
                    res.Id = queryParameters.Get<long>("@out_id");

                    return res;
                }
            }
            
            catch (Exception ex)
            {
                throw ex;
            }

            #region OLD
            //try
            //{
            //    using (var conn = GetConnection())
            //    {
            //        conn.Open();
            //        var queryParameters = new DynamicParameters();
            //        conn.TypeMapper.MapComposite<p_in_document>("ty_documentdetails");
            //        var result = await conn.QueryFirstOrDefaultAsync<ResponseMessage>("bocw_ssy_insertdocumentdetails_withtable", new
            //        {
            //            p_in_document = new p_in_document
            //            {
            //                couchdbdocid = Convert.ToString(dtData.Rows[0]["couchdbdocid"])
            //            }
            //        }, commandType: System.Data.CommandType.StoredProcedure);
            //        return result;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
        }

        public async Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_serviceid", serviceId);
                    queryParameters.Add("in_applicationid", ApplicationId);
                    queryParameters.Add("in_smstype", SMSType);
                    queryParameters.Add("in_schemaname", schemaname);
                    queryParameters.Add("in_tablename", tablename);

                    var result = await conn.QueryFirstOrDefaultAsync<SMSModel>(Procedures.GetSmsContentForService, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addsmslogs(@in_mobileno,@in_serviceid,@in_smscontent,@in_userid,@out_msg)";
                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@in_mobileno", mobileNo);
                    queryParameters.Add("@in_serviceid", serviceId);
                    queryParameters.Add("@in_smscontent", smsContent);
                    queryParameters.Add("@in_userid", userId);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    var result = conn.Execute(procName, queryParameters);
                    res.Msg = queryParameters.Get<string>("@out_msg");
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
