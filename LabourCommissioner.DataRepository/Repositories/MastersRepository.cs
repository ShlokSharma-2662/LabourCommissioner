using Dapper;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using LabourCommissioner.Abstraction;
using Newtonsoft.Json;
using Dapper;


namespace LabourCommissioner.DataRepository.Repositories
{
    public class MastersRepository : BaseRepository<Registration>, IMastersRepository
    {
        public IConfiguration appConfig;
        private readonly UserCookies cookies;
        private readonly ClaimsPrincipal _claimPincipal;
        public MastersRepository(IConfiguration config, IHttpContextAccessor _httpContextAccessor) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
            this.cookies = new UserCookies(_httpContextAccessor);
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
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
        public async Task<ResponseMessage> AddUpdateDistrictMaster(DistrictMaster addDistrictMasters)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.AddUpdateDeleteDistricMaster(@in_postid,@in_districtid,@in_districtcode,@in_districtname,@in_districtnameguj,@in_districtheadofficename," +
                        "@in_districtheadofficenameguj,@in_action,@in_isactive,@in_createdby,@in_ipaddress," +
                        "@in_isdeleted,@in_hostname,@out_error,@out_msg,@out_id,@out_applicatinno)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_postid", addDistrictMasters.postid);
                    queryParameters.Add("@in_districtid", addDistrictMasters.districtid);
                    queryParameters.Add("@in_districtcode", addDistrictMasters.districtcode);
                    queryParameters.Add("@in_districtname", addDistrictMasters.districtname);
                    queryParameters.Add("@in_districtnameguj", addDistrictMasters.districtnameguj);
                    queryParameters.Add("@in_districtheadofficename", addDistrictMasters.districtheadofficename);
                    queryParameters.Add("@in_districtheadofficenameguj", addDistrictMasters.districtheadofficenameguj);
                    queryParameters.Add("@in_action", addDistrictMasters.action);
                    queryParameters.Add("@in_isactive", addDistrictMasters.isactive);
                  
                    queryParameters.Add("@in_createdby", addDistrictMasters.createdby);
                    queryParameters.Add("@in_ipaddress", addDistrictMasters.ipaddress);
                    queryParameters.Add("@in_isdeleted", addDistrictMasters.isdeleted);
                    queryParameters.Add("@in_hostname", addDistrictMasters.hostname);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
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
        
        public async Task<ResponseMessage> AddUpdateTalukaMaster(TalukaMaster addTalukaMaster)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addupdatedeleteTalukamaster(@in_talukaid,@in_districtid,@in_talukaname,@in_talukanameguj,@in_iscorporation,@in_isactive," +
                        "@in_isdeleted,@in_action,@in_createdby,@in_createddate," +
                        "@in_modifiedby,@in_modifieddate,@in_hostname,@in_ipaddress,@out_error,@out_msg,@out_id,@out_applicatinno)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_talukaid", addTalukaMaster.talukaid);
                    queryParameters.Add("@in_districtid", addTalukaMaster.districtid);
                    queryParameters.Add("@in_talukaname", addTalukaMaster.talukaname);
                    //queryParameters.Add("@in_districtname", addDistrictMasters.DistrictName);
                    //queryParameters.Add("@in_districtnameguj", addDistrictMasters.DistrictNameGuj);
                    queryParameters.Add("@in_talukanameguj", addTalukaMaster.talukanameguj);
                    queryParameters.Add("@in_iscorporation", addTalukaMaster.iscorporation);
                    queryParameters.Add("@in_isactive", addTalukaMaster.isactive);
                    queryParameters.Add("@in_isdeleted", addTalukaMaster.isdeleted);
                    queryParameters.Add("@in_action", addTalukaMaster.action);

                    queryParameters.Add("@in_createdby", addTalukaMaster.createdby);
                    queryParameters.Add("@in_createddate", addTalukaMaster.createddate);
                    queryParameters.Add("@in_modifiedby", addTalukaMaster.modifiedby);
                    queryParameters.Add("@in_modifieddate", addTalukaMaster.modifieddate);
                    queryParameters.Add("@in_hostname", addTalukaMaster.hostname);
                    queryParameters.Add("@in_ipaddress", addTalukaMaster.ipaddress);
                    //queryParameters.Add("@in_isheadoffice", addDistrictMasters.isheadoffice);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
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
        public async Task<IEnumerable<DistrictMaster>> DistrictMaster()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<DistrictMaster>(Procedures.GetDistrictMaster, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<VillageMaster>> VillageMaster()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<VillageMaster>(Procedures.getvillagemasters, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<DocumentMaster>> DocumentMaster()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<DocumentMaster>(Procedures.getdocumentmaster, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ServiceSchedular>> ServiceScheduler()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<ServiceSchedular>(Procedures.getServiceschedulermaster, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DistrictMaster> getdistrictdatabyid(long districtid)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtid);

                    var result = await conn.QueryFirstOrDefaultAsync<DistrictMaster>(Procedures.getdistrictdata, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        } 
        public async Task<ServiceSchedular> getserviceschedulerbyid(long serviceschedulerid)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_serviceschedulerid", serviceschedulerid);

                    var result = await conn.QueryFirstOrDefaultAsync<ServiceSchedular>(Procedures.getserviceschedulerdata, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        } 
        public async Task<VillageMaster> getvillagedatabyid(long districtid,long villageid,long talukaid)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtid);
                    queryParameters.Add("_villageid", villageid);
                    queryParameters.Add("_talukaid", talukaid);

                    var result = await conn.QueryFirstOrDefaultAsync<VillageMaster>(Procedures.getvillagedata, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }
        public async Task<DocumentMaster> getdocumentbyid(long documentmasterid)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_documentmasterid", documentmasterid);

                    var result = await conn.QueryFirstOrDefaultAsync<DocumentMaster>(Procedures.getdocumentmasterdata, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        } 
        public async Task<ResourceMaster> getresourcebyid(long resourceid)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_resourceid", resourceid);

                    var result = await conn.QueryFirstOrDefaultAsync<ResourceMaster>(Procedures.getresourcedata, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public async Task<IEnumerable<TalukaMaster>> TalukaMaster()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<TalukaMaster>(Procedures.gettalukamasterdata, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ResourceMaster>> ResourceMaster()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<ResourceMaster>(Procedures.bindresourcevalues, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ResourceMaster>> bindservicemaster()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var result = (await conn.QueryAsync<ResourceMaster>(Procedures.bindresourcevaluess, commandType: CommandType.StoredProcedure)).ToList(); ;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TalukaMaster> gettalukabyId(long talukaid)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_talukaid", talukaid);

                    var result = await conn.QueryFirstOrDefaultAsync<TalukaMaster>(Procedures.gettalukadata, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public async Task<IEnumerable<SelectListItem>> bindservicemaster(int beneficiarytypeid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("in_beneficiarytypeid", beneficiarytypeid);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetServiceMasterByBeneficiaryId, queryParameters, commandType: CommandType.StoredProcedure));
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseMessage> AddDocumentsMasters(DocumentMaster addDocumentMaster)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addupdatedeletedocumentmaster(@in_documentmasterid,@in_documentname,@in_documentnameguj,@in_documentshortname,@in_serviceid,@in_servicedocumenttype," +
                        "@in_documenttypeids,@in_action,@in_iscompulsary,@in_isnumberinput,@in_isactive," +
                        "@in_orderby,@in_isdeleted,@in_isdisplayashash,@in_createdby,@in_createddate,@in_modifiedby,@in_modifieddate,@in_hostname,@in_ipaddress,@out_error,@out_msg,@out_id,@out_applicatinno)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_documentmasterid", addDocumentMaster.documentmasterid);
                    queryParameters.Add("@in_documentname", addDocumentMaster.documentname);
                    queryParameters.Add("@in_documentnameguj", addDocumentMaster.documentnameguj);
                    queryParameters.Add("@in_documentshortname", addDocumentMaster.documentshortname);
                    queryParameters.Add("@in_serviceid", addDocumentMaster.serviceid);
                    queryParameters.Add("@in_servicedocumenttype", addDocumentMaster.servicedocumenttype);
                    queryParameters.Add("@in_documenttypeids", addDocumentMaster.documenttypeids);
                    queryParameters.Add("@in_action", addDocumentMaster.action);
                    queryParameters.Add("@in_iscompulsary", addDocumentMaster.iscompulsary);

                    queryParameters.Add("@in_isnumberinput", addDocumentMaster.isnumberinput);
                    queryParameters.Add("@in_isactive", addDocumentMaster.isactive);
                    queryParameters.Add("@in_orderby", addDocumentMaster.orderby);
                    queryParameters.Add("@in_isdeleted", addDocumentMaster.isdeleted);
                    queryParameters.Add("@in_isdisplayashash", addDocumentMaster.isdisplayashash);
                    queryParameters.Add("@in_createdby", addDocumentMaster.createdby);
                    queryParameters.Add("@in_createddate", addDocumentMaster.createddate);
                    queryParameters.Add("@in_modifiedby", addDocumentMaster.modifiedby);
                    queryParameters.Add("@in_modifieddate", addDocumentMaster.modifieddate);
                    queryParameters.Add("@in_hostname", addDocumentMaster.hostname);
                    queryParameters.Add("@in_ipaddress", addDocumentMaster.ipaddress);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
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
        
        public async Task<ResponseMessage> AddResourceMaster(ResourceMaster addResourceMaster)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addupdatedeleteresourcemaster(@in_resourceid,@in_resourcetype,@in_keyid,@in_keyvalue,@in_keyvalueotherlang,@in_orderby," +
                        "@in_action,@in_isactive,@in_createdby,@in_ipaddress," +
                        "@in_isdeleted,@in_hostname,@out_error,@out_msg,@out_id,@out_applicatinno)";


                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_resourceid", addResourceMaster.resourceid);
                    queryParameters.Add("@in_resourcetype", addResourceMaster.resourcetype);
                    queryParameters.Add("@in_keyid", addResourceMaster.keyid);
                    queryParameters.Add("@in_keyvalue", addResourceMaster.keyvalue);
                    queryParameters.Add("@in_keyvalueotherlang", addResourceMaster.keyvalueotherlang);
                    queryParameters.Add("@in_orderby", addResourceMaster.orderby);
                    queryParameters.Add("@in_action", addResourceMaster.action);
                    queryParameters.Add("@in_isactive", addResourceMaster.IsActive);

                    queryParameters.Add("@in_createdby", addResourceMaster.createdby);
                    queryParameters.Add("@in_ipaddress", addResourceMaster.ipaddress);
                    queryParameters.Add("@in_isdeleted", addResourceMaster.isdeleted);
                    queryParameters.Add("@in_hostname", addResourceMaster.hostname);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
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
        public async Task<ResponseMessage> AddUpdateDeleteServiceSchedular(ServiceSchedular addServiceSchedular)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addupdatedeleteservicescheduler(@in_serviceschedulerid,@in_serviceid,@in_startdate,@in_enddate,@in_isactive,@in_createdby," +
                        "@in_action,@in_createddate,@in_modifiedby,@in_modifieddate," +
                        "@in_ipaddress,@in_hostname,@out_error,@out_msg,@out_id,@out_applicatinno)";


                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_serviceschedulerid", addServiceSchedular.serviceschedulerid);
                    queryParameters.Add("@in_serviceid", addServiceSchedular.serviceid);
                    queryParameters.Add("@in_startdate", addServiceSchedular.startdate);
                    queryParameters.Add("@in_enddate", addServiceSchedular.enddate);
                    queryParameters.Add("@in_isactive", addServiceSchedular.isactive);
                    queryParameters.Add("@in_createdby", addServiceSchedular.createdby);
                    queryParameters.Add("@in_action", addServiceSchedular.action);
                    queryParameters.Add("@in_createddate", addServiceSchedular.createddate);

                    queryParameters.Add("@in_modifiedby", addServiceSchedular.modifiedby);
                    queryParameters.Add("@in_modifieddate", addServiceSchedular.modifieddate);
                    queryParameters.Add("@in_ipaddress", addServiceSchedular.ipaddress);
                    queryParameters.Add("@in_hostname", addServiceSchedular.hostname);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
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
        public async Task<ResponseMessage> AddVillageMaster(VillageMaster addVillageMaster)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var procName = "CALL public.addupdatedeletevillagemaster(@in_villageid,@in_talukaid,@in_districtid,@in_villagename,@in_iscorporation,@in_villagenameguj," +
                        "@in_villagecode,@in_action,@in_isactive,@in_isdeleted,@in_createdby," +
                        "@in_createddate,@in_modifiedby,@in_modifieddate,@in_ipaddress,@in_hostname,@out_error,@out_msg,@out_id,@out_applicatinno)";

                    ResponseMessage res = new ResponseMessage();
                    var queryParameters = new Dapper.DynamicParameters();
                    queryParameters.Add("@in_villageid", addVillageMaster.villageid);
                    queryParameters.Add("@in_talukaid", addVillageMaster.talukaid);
                    queryParameters.Add("@in_districtid", addVillageMaster.districtid);
                    queryParameters.Add("@in_villagename", addVillageMaster.villagename);
                    queryParameters.Add("@in_iscorporation", addVillageMaster.iscorporation);
                    queryParameters.Add("@in_villagenameguj", addVillageMaster.villagenameguj);
                    queryParameters.Add("@in_villagecode", addVillageMaster.villagecode);
                    queryParameters.Add("@in_action", addVillageMaster.action);
                    queryParameters.Add("@in_isactive", addVillageMaster.IsActive);

                    queryParameters.Add("@in_isdeleted", addVillageMaster.isdeleted);
                    queryParameters.Add("@in_createdby", addVillageMaster.createdby);
                    queryParameters.Add("@in_createddate", addVillageMaster.createddate);
                    queryParameters.Add("@in_modifiedby", addVillageMaster.modifiedby);
                    queryParameters.Add("@in_modifieddate", addVillageMaster.modifieddate);
                    queryParameters.Add("@in_ipaddress", addVillageMaster.ipaddress);
                    queryParameters.Add("@in_hostname", addVillageMaster.hostname);
                    queryParameters.Add("@out_error", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_msg", "", direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_id", 0, direction: ParameterDirection.InputOutput);
                    queryParameters.Add("@out_applicatinno", 0, direction: ParameterDirection.InputOutput);
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

        public async Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtid);
                    var result = (await conn.QueryAsync<SelectListItem>(Procedures.GetTalukaByDistrictId, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<VillageMaster>> getvillagebyDistrictTalukaId(int districtid, int talukaid)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("_districtid", districtid);
                    queryParameters.Add("_talukaid", talukaid);
                    //await conn.QueryFirstOrDefaultAsync<TalukaMaster>(Procedures.gettalukadata, queryParameters, commandType: System.Data.CommandType.StoredProcedure);
                    var result = (await conn.QueryAsync<VillageMaster>(Procedures.GetVillageByDistrictIdAndTalukaIds, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


