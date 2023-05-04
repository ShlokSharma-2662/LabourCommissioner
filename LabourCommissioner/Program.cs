using AspNetCoreRateLimit;
using LabourCommissioner.Abstraction.Repositories;
using LabourCommissioner.Abstraction.Services;
using LabourCommissioner.DataRepository.Repositories;
using LabourCommissioner.Services.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using System.Text.RegularExpressions;
using System.Security.Principal;
using LabourCommissioner.CustomAuthorization;
using DNTCaptcha.Core;
using Wkhtmltopdf.NetCore;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Filters;
using LabourCommissioner.Controllers;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(new JsonFormatter(), "wwwroot//LogFiles//LogWrite.text")
    .CreateLogger();


//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Logger(lc => lc
//        .Filter.ByExcluding(Matching.FromSource<CCApplicationController>())
//        .WriteTo.File(new JsonFormatter(), "wwwroot//LogFiles//LogWrite.text"))
//    .WriteTo.Logger(lc => lc
//        .Filter.ByIncludingOnly(Matching.FromSource<HomeController>())
//        .WriteTo.File(new JsonFormatter(), "wwwroot//LogFiles//LogWrite1.text"))
//    .CreateLogger();



var rootPath = builder.Environment.WebRootPath;
var contantrootPath = builder.Environment.ContentRootPath;
// Add services to the container.
builder.Services.AddControllersWithViews();
{
    var services = builder.Services;
    services.AddHttpContextAccessor();

    services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

    services.AddMvc()
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

    services.AddDNTCaptcha(options =>
      options.UseCookieStorageProvider()
          .ShowThousandsSeparators(false)
          .WithEncryptionKey("This is my secure key!")
          .UseCustomFont(Path.Combine(rootPath, "assets//css", "OpenSans-Bold.ttf"))
  );
    services.AddWkhtmltopdf("Rotativa");

    services.Configure<RequestLocalizationOptions>(options =>
    {
        //var culture = CultureInfo.CreateSpecificCulture("en-US");
        var dateformat = new DateTimeFormatInfo
        {
            ShortDatePattern = "dd/MM/yyyy",
            LongDatePattern = "dd/MM/yyyy",
        };
        //        //culture.DateTimeFormat = dateformat;

        //        //CultureInfo ist = new CultureInfo("en-US");
        //        //string shortUsDateFormatString = ist.DateTimeFormat.ShortDatePattern;
        //        //string shortUsTimeFormatString = ist.DateTimeFormat.ShortTimePattern;
        //        //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        //        //DateTime[] dates = {
        //        //     new DateTime(2019, 10, 9),
        //        //     new DateTime(2020, 1, 2)
        //};

        //using var sw = new StreamWriter(@"dates.dat");
        //sw.Write(String.Format(CultureInfo.InvariantCulture,
        //    "{0:d}|{1:d}", dates[0], dates[1]));

        var cultures = new CultureInfo[]
        {
            new CultureInfo("en"),
            new CultureInfo("gu"),
        };
        cultures[1].DateTimeFormat = dateformat;

        options.DefaultRequestCulture = new RequestCulture("gu");
        options.SupportedCultures = cultures;
        options.SupportedUICultures = cultures;
    });




    services.AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddDataAnnotationsLocalization();



    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.LogoutPath = "/account/login";
    });
    services.AddHttpClient();


    services.AddMemoryCache();
    services.Configure<IpRateLimitOptions>(options =>
    {
        options.EnableEndpointRateLimiting = true;
        options.StackBlockedRequests = false;
        options.HttpStatusCode = 429;
        options.RealIpHeader = "X-Real-IP";
        options.ClientIdHeader = "X-ClientId";
        options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "POST:/MailTemplate/ForgetPassword",
                Period = "1m",
                Limit = 1,
                MonitorMode=true,
            }
        };
    });

    services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
    services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    services.AddInMemoryRateLimiting();
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddScoped<PermissionRequirementFilter>();
    services.AddScoped<IAuthorizeRepository, AuthorizeRepository>();
    services.AddScoped<ISchemeUserServices, SchemeUserServices>();
    services.AddScoped<ISchemeUserRepository, SchemeUserRepository>();
    services.AddScoped<IRegistrationService, RegistrationService>();
    services.AddScoped<IRegistrationRepository, RegistrationRepository>();
    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<IHomeService, HomeService>();
    services.AddScoped<IHomeRepository, HomeRepository>();
    services.AddScoped<IEmployeeHomeService, EmployeeHomeService>();
    services.AddScoped<IEmployeeHomeRepository, EmployeeHomeRepository>();
    services.AddScoped<ISchemeService, SchemeService>();
    services.AddScoped<ISchemeRepository, SchemeRepository>();


    services.AddScoped<IBOCWOfficeMasterService, BOCWMasterService>();
    services.AddScoped<IBOCWMasterRepository, BOCWOfficeMasterRepository>();

    services.AddScoped<IBOCWSikshanSahayYojanaService, BOCWSikshanSahayYojanaService>();
    services.AddScoped<IBOCWSikshanSahayYojanaRepository, BOCWSikshanSahayYojanaRepository>();

    services.AddScoped<IBOCWBhagyaLaxmiBondYojnaService, BOCWBhagyaLaxmiBondYojnaService>();

    services.AddScoped<IBOCWBhagyaLaxmiBondYojnaRepository, BOCWBhagyaLaxmiBondYojnaRepository>();

    services.AddScoped<IBOCWAntyesthiYojanaService, BOCWAntyesthiYojanaService>();
    services.AddScoped<IBOCWAntyesthiYojanaRepository, BOCWAntyesthiYojanaRepository>();



    services.AddScoped<IGLWBHSCPurashkarYojanaService, GLWBHSCPurashkarYojanaService>();
    services.AddScoped<IGLWBHSCPurashkarYojanaRepository, GLWBHSCPurashkarYojanaRepository>();

    services.AddScoped<IGLWBSSCPurashkarYojanaService, GLWBSSCPurashkarYojanaService>();
    services.AddScoped<IGLWBSSCPurashkarYojanaRepository, GLWBSSCPurashkarYojanaRepository>();

    services.AddScoped<IBOCWBeforePrasutiSahayService, BOCWBeforePrasutiSahayService>();
    services.AddScoped<IBOCWBeforePrasutiSahayRepository, BOCWBeforePrasutiSahayRepository>();

    services.AddScoped<IBOCWTabletSahayYojanaService, BOCWTabletSahayYojanaService>();
    services.AddScoped<IBOCWTabletSahayYojanaRepository, BOCWTabletSahayYojanaRepository>();

    services.AddScoped<IGLWBMahilashramLagnaSahayService, GLWBMahilashramLagnaSahayService>();
    services.AddScoped<IGLWBMahilashramLagnaSahayRepository, GLWBMahilashramLagnaSahayRepository>();

    services.AddScoped<IBOCWPIPSahayYojanaService, BOCWPIPSahayYojanaService>();
    services.AddScoped<IBOCWPIPSahayYojanaRepository, BOCWPIPSahayYojanaRepository>();


    services.AddScoped<IBOCWTabibiSahayYojanaService, BOCWTabibiSahayYojanaService>();
    services.AddScoped<IBOCWTabibiSahayYojanaRepository, BOCWTabibiSahayYojanaRepository>();

    services.AddScoped<IBOCWTabibiSahayYojanaClaimService, BOCWTabibiSahayYojanaClaimService>();
    services.AddScoped<IBOCWTabibiSahayYojanaClaimRepository, BOCWTabibiSahayYojanaClaimRepository>();

    services.AddScoped<IGLWBTabibiSahayService, GLWBTabibiSahayService>();
    services.AddScoped<IGLWBTabibiSahayRepository, GLWBTabibiSahayRepository>();

    services.AddScoped<IGLWBCycleYojnaService, GLWBCycleYojnaService>();
    services.AddScoped<IGLWBCycleYojnaRepository, GLWBCycleYojnaRepository>();

     services.AddScoped<IMastersService, MastersService>();
    services.AddScoped<IMastersRepository, MastersRepository>();



    services.AddScoped<IBOCWAccidentalSahayYojanaService, BOCWAccidentalSahayYojanaService>();
    services.AddScoped<IBOCWAccidentalSahayYojanaRepository, BOCWAccidentalSahayYojanaRepository>();


    services.AddScoped<IGLWBHomeLoanSubsidyYojanaService, GLWBHomeLoanSubsidyYojanaService>();
    services.AddScoped<IGLWBHomeLoanSubsidyYojanaRepository, GLWBHomeLoanSubsidyYojanaRepository>();

    services.AddScoped<IGLWBPrasutiSahayBetiProtsahanYojnaService, GLWBPrasutiSahayBetiProtsahanYojnaService>();
    services.AddScoped<IGLWBPrasutiSahayBetiProtsahanYojnaRepository, GLWBPrasutiSahayBetiProtsahanYojnaRepository>();

    services.AddScoped<IGLWBAccidentalSahayYojanaService, GLWBAccidentalSahayYojanaService>();
    services.AddScoped<IGLWBAccidentalSahayYojanaRepository, GLWBAccidentalSahayYojanaRepository>();

    services.AddScoped<IBOCWNanajiYojanaService, BOCWNanajiYojanaService>();
    services.AddScoped<IBOCWNanajiYojanaRepository, BOCWNanajiYojanaRepository>();

    services.AddScoped<IBOCWVishishtaCoachingYojanaService, BOCWVishishtaCoachingYojanaService>();
    services.AddScoped<IBOCWVishishtaCoachingYojanaRepository, BOCWVishishtaCoachingYojanaRepository>();

    services.AddScoped<IGLWBAccidentalDisabilitySahayYojanaService, GLWBAccidentalDisabilitySahayYojanaService>();
    services.AddScoped<IGLWBAccidentalDisabilitySahayYojanaRepository, GLWBAccidentalDisabilitySahayYojanaRepository>();

    services.AddScoped<IGLWBHigherStudyService, GLWBHigherStudyService>();
    services.AddScoped<IGLWBHigherStudyRepository, GLWBHigherStudyRepository>();

    services.AddScoped<IBOCWVYAVSHAYIKService, BOCWVYAVSHAYIKService>();
    services.AddScoped<IBOCWVYAVSHAYIKRepository, BOCWVYAVSHAYIKRepository>();

    services.AddScoped<IReportsService, ReportsService>();
    services.AddScoped<IReportsRepository, ReportsRepository>();

    services.AddScoped<IGLWBHomeTownClaimYojanaService, GLWBHomeTownClaimYojanaService>();
    services.AddScoped<IGLWBHomeTownClaimYojanaRepository, GLWBHomeTownClaimYojanaRepository>();

    services.AddScoped<IGLWBTabibiSahayClaimyojnasService, GLWBTabibiSahayClaimyojnasService>();
    services.AddScoped<IGLWBTabibiSahayClaimyojnasRepository, GLWBTabibiSahayClaimyojnasRepository>();

    services.AddScoped<IGLWBHomeTownYojanaService, GLWBHomeTownYojanaService>();
    services.AddScoped<IGLWBHomeTownYojanaRepository, GLWBHomeTownYojanaRepository>();

    services.AddScoped<IBOCWHostelSahayYojanaService, BOCWHostelSahayYojanaService>();
    services.AddScoped<IBOCWHostelSahayYojanaRepository, BOCWHostelSahayYojanaRepository>();


    services.AddScoped<ClaimsPrincipal>();
    services.AddScoped<PermissionRequirementFilter>();

    //services.AddScoped(typeof(IReportRepository<>), typeof(ReportRepository<>));

    services.AddScoped<IGLWBLaptopSubsidyYojnaService, GLWBLaptopSubsidyYojnaService>();
    services.AddScoped<IGLWBLaptopSubsidyYojnaRepository, GLWBLaptopSubsidyYojnaRepository>();

    services.AddScoped<IServiceRoutineService, ServiceRoutineService>();
    services.AddScoped<IServiceRoutineRepository, ServiceRoutineRepository>();

    services.AddScoped<IBOCWServiceRoutineService, BOCWServiceRoutineService>();
    services.AddScoped<IBOCWServiceRoutineRepository, BOCWServiceRoutineRepository>();

    services.AddScoped<IGLWBServiceRoutineService, GLWBServiceRoutineService>();
    services.AddScoped<IGLWBServiceRoutineRepository, GLWBServiceRoutineRepository>();

    services.AddScoped<ICCRegistrationService, CCRegistrationService>();
    services.AddScoped<ICCRegistrationRepository, CCRegistrationRepository>();

    services.AddScoped<ICCHomeService, CCHomeService>();
    services.AddScoped<ICCHomeRepository, CCHomeRepository>();

    services.AddScoped<ICCApplicationService, CCApplicationService>();
    services.AddScoped<ICCApplicationRepository, CCApplicationRepository>();

    services.AddScoped<ICMDashboardService, CMDashboardService>();
    services.AddScoped<ICMDashboardRepository, CMDashboardRepository>();

    services.AddScoped<IEmployeeMasterService, EmployeeMasterService>();
    services.AddScoped<IEmployeeMasterRepository, EmployeeMasterRepository>();


}

var app = builder.Build();
app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.Use(async (context, next) =>
//{
//    await next();
//    if (context.Response.StatusCode == 404)
//    {
//        context.Request.Path = "/Home/NotFound";
//        await next();
//    }
//});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization(((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

//var SupportedCultures = new CultureInfo[]
//        {
//            new CultureInfo("en"),
//            new CultureInfo("gu"),
//        };
//app.UseRequestLocalization(new RequestLocalizationOptions
//{
//    DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en"),
//    SupportedCultures = SupportedCultures,
//    SupportedUICultures = SupportedCultures

//});
//var locOptions = ((IApplicationBuilder)app).ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>(); 
//app.UseRequestLocalization(locOptions.Value);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
