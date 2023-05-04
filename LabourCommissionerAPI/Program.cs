using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.DataRepository.Repositories;
using LabourCommissioner.Services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using Wkhtmltopdf.NetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNetCore.Hosting;


var builder = WebApplication.CreateBuilder(args);




// Add services to the container.


var rootPath = builder.Environment.WebRootPath;
var contantrootPath = builder.Environment.ContentRootPath;


builder.Services.AddControllers();
builder.Services.AddWkhtmltopdf("Rotativa");


builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
})
.AddCookie("Cookies", options =>
{
    options.LogoutPath = "/registration/login";
});
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie("Cookies",options =>
//{
//    options.LogoutPath = "/registration/login";
//});

//builder.Services.AddSingleton<IWebHostEnvironment, IWebHostEnvironment>();
        
builder.Services.AddAuthorization();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ClaimsPrincipal>();
builder.Services.AddScoped<IBOCWSikshanSahayYojanaService, BOCWSikshanSahayYojanaService>();
builder.Services.AddScoped<IBOCWSikshanSahayYojanaRepository, BOCWSikshanSahayYojanaRepository>();
builder.Services.AddScoped<IGLWBSSCPurashkarYojanaService, GLWBSSCPurashkarYojanaService>();
builder.Services.AddScoped<IGLWBSSCPurashkarYojanaRepository, GLWBSSCPurashkarYojanaRepository>();
builder.Services.AddScoped<IGLWBHSCPurashkarYojanaService, GLWBHSCPurashkarYojanaService>();
builder.Services.AddScoped<IGLWBHSCPurashkarYojanaRepository, GLWBHSCPurashkarYojanaRepository>();
builder.Services.AddScoped<IGLWBPrasutiSahayBetiProtsahanYojnaService, GLWBPrasutiSahayBetiProtsahanYojnaService>();
builder.Services.AddScoped<IGLWBPrasutiSahayBetiProtsahanYojnaRepository, GLWBPrasutiSahayBetiProtsahanYojnaRepository>();
builder.Services.AddScoped<IGLWBTabibiSahayService, GLWBTabibiSahayService>();
builder.Services.AddScoped<IGLWBTabibiSahayRepository, GLWBTabibiSahayRepository>();
builder.Services.AddScoped<IGLWBMahilashramLagnaSahayService, GLWBMahilashramLagnaSahayService>();
builder.Services.AddScoped<IGLWBMahilashramLagnaSahayRepository, GLWBMahilashramLagnaSahayRepository>();
builder.Services.AddScoped<IBOCWBeforePrasutiSahayService, BOCWBeforePrasutiSahayService>();
builder.Services.AddScoped<IBOCWBeforePrasutiSahayRepository, BOCWBeforePrasutiSahayRepository>();
builder.Services.AddScoped<IBOCWBhagyaLaxmiBondYojnaService, BOCWBhagyaLaxmiBondYojnaService>();
builder.Services.AddScoped<IBOCWBhagyaLaxmiBondYojnaRepository, BOCWBhagyaLaxmiBondYojnaRepository>();
builder.Services.AddScoped<IBOCWPIPSahayYojanaService, BOCWPIPSahayYojanaService>();
builder.Services.AddScoped<IBOCWPIPSahayYojanaRepository, BOCWPIPSahayYojanaRepository>();

builder.Services.AddScoped<IBOCWAntyesthiYojanaService, BOCWAntyesthiYojanaService>();
builder.Services.AddScoped<IBOCWAntyesthiYojanaRepository, BOCWAntyesthiYojanaRepository>();

builder.Services.AddScoped<IBOCWTabibiSahayYojanaService, BOCWTabibiSahayYojanaService>();
builder.Services.AddScoped<IBOCWTabibiSahayYojanaRepository, BOCWTabibiSahayYojanaRepository>();

builder.Services.AddScoped<IBOCWTabletSahayYojanaService, BOCWTabletSahayYojanaService>();
builder.Services.AddScoped<IBOCWTabletSahayYojanaRepository, BOCWTabletSahayYojanaRepository>();

builder.Services.AddScoped<IBOCWVYAVSHAYIKService, BOCWVYAVSHAYIKService>();
builder.Services.AddScoped<IBOCWVYAVSHAYIKRepository, BOCWVYAVSHAYIKRepository>();

builder.Services.AddScoped<IBOCWAccidentalSahayYojanaService, BOCWAccidentalSahayYojanaService>();
builder.Services.AddScoped<IBOCWAccidentalSahayYojanaRepository, BOCWAccidentalSahayYojanaRepository>();

builder.Services.AddScoped<IBOCWVishishtaCoachingYojanaService, BOCWVishishtaCoachingYojanaService>();
builder.Services.AddScoped<IBOCWVishishtaCoachingYojanaRepository, BOCWVishishtaCoachingYojanaRepository>();

builder.Services.AddScoped<IBOCWNanajiYojanaService, BOCWNanajiYojanaService>();
builder.Services.AddScoped<IBOCWNanajiYojanaRepository, BOCWNanajiYojanaRepository>();

builder.Services.AddScoped<IBOCWHostelSahayYojanaService, BOCWHostelSahayYojanaService>();
builder.Services.AddScoped<IBOCWHostelSahayYojanaRepository, BOCWHostelSahayYojanaRepository>();

builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();

builder.Services.AddScoped<IGLWBLaptopSubsidyYojnaService, GLWBLaptopSubsidyYojnaService>();
builder.Services.AddScoped<IGLWBLaptopSubsidyYojnaRepository, GLWBLaptopSubsidyYojnaRepository>();

builder.Services.AddScoped<IGLWBCycleYojnaService, GLWBCycleYojnaService>();
builder.Services.AddScoped<IGLWBCycleYojnaRepository, GLWBCycleYojnaRepository>();


builder.Services.AddScoped<IGLWBAccidentalSahayYojanaService, GLWBAccidentalSahayYojanaService>();
builder.Services.AddScoped<IGLWBAccidentalSahayYojanaRepository, GLWBAccidentalSahayYojanaRepository>();

builder.Services.AddScoped<IGLWBAccidentalDisabilitySahayYojanaService, GLWBAccidentalDisabilitySahayYojanaService>();
builder.Services.AddScoped<IGLWBAccidentalDisabilitySahayYojanaRepository, GLWBAccidentalDisabilitySahayYojanaRepository>();

builder.Services.AddScoped<IGLWBHomeLoanSubsidyYojanaService, GLWBHomeLoanSubsidyYojanaService>();
builder.Services.AddScoped<IGLWBHomeLoanSubsidyYojanaRepository, GLWBHomeLoanSubsidyYojanaRepository>();


builder.Services.AddScoped<IGLWBHomeTownYojanaService, GLWBHomeTownYojanaService>();
builder.Services.AddScoped<IGLWBHomeTownYojanaRepository, GLWBHomeTownYojanaRepository>();

builder.Services.AddScoped<IGLWBHigherStudyService, GLWBHigherStudyService>();
builder.Services.AddScoped<IGLWBHigherStudyRepository, GLWBHigherStudyRepository>();

builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>();

builder.Services.AddScoped<IBOCWTabibiSahayYojanaClaimService, BOCWTabibiSahayYojanaClaimService>();
builder.Services.AddScoped<IBOCWTabibiSahayYojanaClaimRepository, BOCWTabibiSahayYojanaClaimRepository>();

builder.Services.AddScoped<ICMDashboardService, CMDashboardService>();
builder.Services.AddScoped<ICMDashboardRepository, CMDashboardRepository>();

//builder.Services.AddScoped<IReportService, ReportService>();
//builder.Services.AddScoped<IReportRepository, ReportRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy(
      "CorsPolicy",
      builder => builder.WithOrigins("http://localhost")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});

var socketsHandler = new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(10),
    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
    MaxConnectionsPerServer = 10
};

var client = new HttpClient(socketsHandler);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");

});
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
