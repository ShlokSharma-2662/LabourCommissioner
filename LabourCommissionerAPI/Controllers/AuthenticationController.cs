using LabourCommissioner.Abstraction;
using LabourCommissionerAPI.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LabourCommissionerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        ApiResponse apiResponse = new ApiResponse();

        public AuthenticationController(IConfiguration appConfig)
        {
            _config = appConfig;
        }

        [HttpGet("GenerateJSONWebToken")]
        public async Task<IActionResult> GenerateToken()
        {
            try
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Sucess;
                apiResponse.Result = Convert.ToString(GenerateJSONWebToken());
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Success);
                apiResponse.Message = EnumLookup.GetDescription(EnumLookup.Message.Token_Success);
                apiResponse.StackTrace = null;

            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)EnumLookup.StatusCode.Internal_Server_Error;
                apiResponse.Result = false;
                apiResponse.Status = EnumLookup.GetDescription(EnumLookup.Status.Fail);
                apiResponse.Message = ex.StackTrace;
            }

            return Ok(apiResponse);
        }
        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(Convert.ToInt64(_config["Jwt:DurationInMinutes"])),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
