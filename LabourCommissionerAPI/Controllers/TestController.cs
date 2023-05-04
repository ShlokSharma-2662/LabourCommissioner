using DocumentFormat.OpenXml.EMMA;
using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Abstraction.ViewDataModels;
using LabourCommissioner.Common;
using LabourCommissioner.Common.Utility;
using LabourCommissionerAPI.ResponseModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using static LabourCommissioner.Abstraction.ViewDataModels.DocumentDetails;

namespace LabourCommissionerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : ControllerBase
    {
        private readonly IBOCWSikshanSahayYojanaService _iBOCWSikshanSahayYojanaService;
        private readonly ClaimsPrincipal _claimPincipal;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ApiResponse apiResponse = new ApiResponse();


        public TestController(IBOCWSikshanSahayYojanaService iBOCWSikshanSahayYojanaService,
            IConfiguration config, IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _iBOCWSikshanSahayYojanaService = iBOCWSikshanSahayYojanaService ?? throw new ArgumentNullException(nameof(_iBOCWSikshanSahayYojanaService));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));

            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _claimPincipal = _httpContextAccessor.HttpContext.User ??
                             throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext.User));
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("addupdatePersonalDetailsTest")]
        public async Task<IActionResult> AddUpdateApplication([FromForm] PersonalDetailsModelTest personalDetails)
        {
            try
            {
                if (personalDetails  != null && personalDetails.Photo != null )
                {
                    personalDetails.Phototest = personalDetails.Photo.FileName; 
                    if (personalDetails != null)
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                        apiResponse.Result = personalDetails;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.PersonnalDetails_Added);
                        apiResponse.StackTrace = null;
                    }
                    else
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                        apiResponse.Result = null;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.PersonnalDetails_Not_Added);
                        apiResponse.StackTrace = null;
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }
            return Ok(personalDetails);
        }

        
    }
}


