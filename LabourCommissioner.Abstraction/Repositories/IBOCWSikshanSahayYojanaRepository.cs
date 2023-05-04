﻿using LabourCommissioner.Abstraction.DataModels;
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
using System.Collections;

namespace LabourCommissioner.Abstraction.Repositories
{
    public interface IBOCWSikshanSahayYojanaRepository : IBaseRepository<TabModel>
    {
        Task<List<TabModel>> GetServiceTabByServiceId(int ServiceId);
        Task<List<TabModel>> GetTabSequenceByApplicationId(int ApplicationId, int id, string schemaname, string tablename);
        Task<List<SelectListItem>> GetAllStates();
        Task<PersonalDetailsModel> GetPersonalDetailsByRegNo(string RegistrationNo);
        Task<PersonalDetailsModelAPI> GetPersonalDetailsByRegNoAPI(string RegistrationNo);        
        Task<PersonalDetailsModel> GetApplicationDetailsByAppId(long ApplicationId, string schemaname, string tablename);
        Task<PersonalDetailsModelAPI> GetApplicationDetailsByAppIdAPI(long ApplicationId, string schemaname, string tablename);
        Task<SchemeDetails> GetApplicationSchemeDetailsByAppId(long ApplicationId);
        Task<IList<DocumentFileDetails>> GetUploadedDocuments(long ApplicationId, long serviceId, string schemaname, string tablename);
        Task<IEnumerable<SelectListItem>> GetDistrict();

        Task<IEnumerable<SelectListItem>> GetSubject(string subjectId);
        Task<IEnumerable<SelectListItem>> GetTalukaByDistrictId(int districtId);
        Task<IEnumerable<SelectListItem>> GetSemesterbyCourseId(int courseid);
        Task<IEnumerable> GetBenifitByCourseId(int courseId, string semesteryear);
        Task<IEnumerable<SelectListItem>> GetVillageByDistrictIdAndTalukaId(int districtId, int talukaId);
        Task<IEnumerable<SelectListItem>> GetEducation(string ResourceType);
        Task<IEnumerable<DocumentDetails>> GetFileDocuments(int ServiceId);

        Task<ResponseMessage> AddUpdateApplication(PersonalDetailsModel personalDetailsModel);
        Task<ResponseMessage> AddUpdateApplicationAPI(PersonalDetailsModelAPI personalDetailsModel);
        Task<ResponseMessage> AddSchemeDetails(SchemeDetails schemeDetails);
        Task<IList<DocumentFileDetails>> GetDocumentFileDetails(int ServiceId, int ApplicationId);
        Task<ResponseMessage> AddUpdateDocumentDetails(IList<DocumentFileDetails> lstdocumentFileDetails);

        Task<ResponseMessage> AddUpdateDocumentDetailsNew(DataTable dtData);

        Task<ResponseMessage> FinalSubmit(FinalSubmitModel finalSubmitModel);

        Task<SMSModel> GetSmsContentForService(long serviceId, long ApplicationId, int SMSType, string schemaname, string tablename);
        Task<ResponseMessage> AddSMSLogs(string mobileNo, long serviceId, string smsContent, long userId);
    }
}