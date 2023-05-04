using LabourCommissioner.Abstraction;
using LabourCommissioner.Abstraction.DataModels;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.Common;
using LabourCommissionerAPI.ResponseModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LabourCommissionerAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _iregistrationService;
        private readonly IAccountService _iaccountService;        
        ApiResponse apiResponse = new ApiResponse();

        public RegistrationController(IRegistrationService registrationService, IAccountService iaccountService, IConfiguration configuration)
        {
            _iregistrationService = registrationService;
            _iaccountService = iaccountService;            
        }

        [HttpPost("addRegister")]
        public async Task<IActionResult> AddRegistration(Registration registration)
        {

            try
            {
                var register = await _iregistrationService.AddUpdateRegistration(registration);

                if (register != null)
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                    apiResponse.Result = register;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Register_Success);
                    apiResponse.StackTrace = null;
                }
                else
                {
                    apiResponse.StatusCode = (int)EnumLookup.StatusCode.Not_Found;
                    apiResponse.Result = null;
                    apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Not_Found);
                    apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Register_Not_Added);
                    apiResponse.StackTrace = null;
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = null;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }

            return Ok(apiResponse);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(Usermaster model)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                if (ModelState.IsValid)
                {
                    Usermaster user = await _iaccountService.AthenticateUser(model);
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.CitizenRoleId.ToString()),
                        new Claim("RegistrationId", user.RegistrationId.ToString()),
                        new Claim("UserType", user.UserType.ToString()),
                        new Claim("DisplayName", Convert.ToString(user.DisplayName)),
                        new Claim("MobileNo", Convert.ToString(user.MobileNo)),
                        new Claim("EmailId", String.IsNullOrEmpty(user.EmailId) ? "" : Convert.ToString(user.EmailId)),
                        new Claim("BeneficiaryType", Convert.ToString(user.BeneficiaryType)),
                        new Claim("PostId", user.PostId.ToString())
                        };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTime.UtcNow.AddHours(2)
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
                    if (user != null)
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                        apiResponse.Result = user;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.User_Login);
                        apiResponse.StackTrace = null;
                    }
                    else
                    {
                        apiResponse.StatusCode = (int)EnumLookup.StatusCode.Unauthorized;
                        apiResponse.Result = null;
                        apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Unauthorized);
                        apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Login_Failed);
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
            return Ok(apiResponse);

        }
    }
}
