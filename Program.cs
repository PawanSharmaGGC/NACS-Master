using CMS.Core;
using CMS.EmailEngine;
using Convenience.org.Helpers;
using CMS.EventLog;
using Convenience.org.Components.Widgets.AlumniContent;
using Convenience.org.Repositories;
using Convenience.org.Repositories.Interfaces;
using ConvenienceCares.Configuration;
using ConvenienceCares.Operations;

using Kentico.Activities.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Membership;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Kentico.Xperience.Cloud;

using Lucene.Net.Util;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

using NACS.Infrastructure;
using NACS.Portal.Core;
using NACS.Portal.Core.Operations;

using NACSMagazine.PageTemplates.CategoryPage;
using NACSMagazine.PageTemplates.SearchPage;
using NACSMagazine.Rendering;

//using NACSShow.Components.Widgets.NewsArticleListing;
using NACSShow.Helpers;
using NACSShow.Services.Search.Operations;

//using NACSShow.Services.Search.Operations;

using Slugify;
using Convenience.org.Components.Widgets.CommitteeListing;
using Microsoft.Extensions.Configuration;
using NACS.Helper.CustomerService;
using System.ServiceModel;
using Convenience;
using Convenience.org.Components.Widgets.CommitteeRoster;


var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var config = builder.Configuration;

builder.Services.AddAppLuceneSearch(config);
builder.Services.AddSingleton<IChannelDataProvider, ChannelDataProvider>();
builder.Services.AddSingleton<ICacheDependencyKeysBuilder, CacheDependencyKeysBuilder>();

// Register the Dynamics 365 connectivity and other services
builder.Services.Configure<Dynamics365Options>(builder.Configuration.GetSection("D365"));
builder.Services.Configure<MarketingListIdsConfig>(builder.Configuration.GetSection("MarketingListIds"));

builder.Services.AddSingleton<IEventLogService, EventLogService>();  

builder.Services.AddSingleton<Dynamics365Connectivity>();
builder.Services.AddScoped<Dynamics365DataService>();
builder.Services.AddTransient<ICustomTableService, CustomTableService>();

builder.Services.AddScoped<PageServices>();

ConfigurationManagerSimulator.Initialize(builder.Configuration);

var serviceConfig = builder.Configuration.GetSection("NACSAPI");

var binding = new BasicHttpBinding
{
    MaxBufferSize = serviceConfig.GetValue<int>("MaxBufferSize", 64000000),
    MaxReceivedMessageSize = serviceConfig.GetValue<int>("MaxReceivedMessageSize", 64000000),
    Security = new BasicHttpSecurity
    {
        Mode = BasicHttpSecurityMode.Transport 
    }
};

var endpoint = new EndpointAddress(serviceConfig.GetValue<string>("BaseAddress") ?? "https://api-test.nacsonline.com/nacssoap/CustomerDataService.asmx");

builder.Services.AddScoped<NACSAPICustomerSoapClient>(sp => new NACSAPICustomerSoapClient(binding, endpoint));
builder.Services.AddScoped<ICommitteeService, CommitteeService>();
builder.Services.AddScoped<ICommitteeMemberInfoService, CommitteeMemberInfoService>();
builder.Services.AddScoped<ICommitteeRosterService, CommitteeRosterService>();




builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});
builder.Services.Configure<RouteOptions>(options =>
{

    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
    options.LowercaseQueryStrings = false;
})
    .AddLocalization()
    .Configure<RequestLocalizationOptions>(o =>
    {
        o.ApplyCurrentCultureToResponseHeaders = true;
        o.FallBackToParentUICultures = true;
        o.DefaultRequestCulture = new RequestCulture("en-US");
        o.SupportedUICultures ??= [];
        o.SupportedUICultures.AddRange(
            [
                new("en"),
                new("en-US")
            ]);
    })
    .Configure<StaticFileOptions>(o =>
    {
        o.OnPrepareResponse = context =>
        {
            context.Context.Response.Headers.Append("Cache-Control", "public, max-age=604800");
        };
    })
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider =
            (type, factory) => factory.Create(typeof(NACSMagazine.Resources.SharedResources));

        options.DataAnnotationLocalizerProvider =
            (type, factory) => factory.Create(typeof(NACSShow.Resources.SharedResources));
    })
    .Services
    .AddHttpContextAccessor()
    .AddHttpClient();
    

builder.Services.AddXperienceCloudApplicationInsights(builder.Configuration);

if (builder.Environment.IsQa() || builder.Environment.IsUat() || builder.Environment.IsProduction())
{
    builder.Services.AddKenticoCloud(builder.Configuration);
    builder.Services.AddXperienceCloudSendGrid(builder.Configuration);
}

// Enable desired Kentico Xperience features
builder.Services.AddKentico(features =>
{
     features.UsePageBuilder(new PageBuilderOptions
         {
         ContentTypeNames = new[]
        {
            //Enables Page Builder for content types using their generated classes
            Convenience.Article.CONTENT_TYPE_NAME,
            Convenience.Home.CONTENT_TYPE_NAME,
            Convenience.Form.CONTENT_TYPE_NAME,
            Convenience.GenericLeadGen.CONTENT_TYPE_NAME,
            Convenience.L1Statistics.CONTENT_TYPE_NAME,
            Convenience.Topic.CONTENT_TYPE_NAME,
            Convenience.Page.CONTENT_TYPE_NAME,
            Convenience.EventPage.CONTENT_TYPE_NAME,
            NACSMagazine.Home.CONTENT_TYPE_NAME,
            NACSMagazine.Issue.CONTENT_TYPE_NAME,
            NACSMagazine.Article.CONTENT_TYPE_NAME,
            NACSMagazine.LandingPage.CONTENT_TYPE_NAME,
            NACSMagazine.Search.CONTENT_TYPE_NAME,
            NACSMagazine.CategoryPage.CONTENT_TYPE_NAME,
            NACSShow.Home.CONTENT_TYPE_NAME,
            NACSShow.Page.CONTENT_TYPE_NAME,
            NACSShow.Video.CONTENT_TYPE_NAME,
            NACSShow.NewsArticlePage.CONTENT_TYPE_NAME,
            NACSShow.DailyNewsPage.CONTENT_TYPE_NAME,
            PersonPage.CONTENT_TYPE_NAME,
            Webinar.CONTENT_TYPE_NAME,
        }
     });
    
     features.UseActivityTracking();
     features.UseWebPageRouting();
     features.UseEmailStatisticsLogging();
     features.UseEmailMarketing();
})
.Configure<SmtpOptions>(config.GetSection("SmtpOptions"))
    .AddXperienceSmtp()
.Configure<EmailQueueOptions>(o =>
{
    o.ArchiveDuration = 14;
});
builder.Services.AddScoped<ITier3ContentCardRepository, Tier3ContentCardRepository>();
builder.Services.AddScoped<MediaLibraryHelpers>();

builder.Services.AddIdentity<ApplicationUser, NoOpApplicationRole>(options =>
{
    // Ensures that disabled member accounts cannot sign in
    options.SignIn.RequireConfirmedAccount = true;
    // Ensures unique emails for registered accounts
    options.User.RequireUniqueEmail = true;
})
    .AddUserStore<ApplicationUserStore<ApplicationUser>>()
    .AddRoleStore<NoOpApplicationRoleStore>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddDIServices();

builder.Services.AddAppServices(config);

builder.Services
            .AddSingleton<ISlugHelper>(_ => new SlugHelper(new SlugHelperConfiguration()))
            .AddSingleton<MediaLibraryHelpers>()
            .AddScoped<ViewService>();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CategoryPageQuery).Assembly))
            .AddTransient<WebPageCommandTools>()
            .AddTransient<WebPageQueryTools>()
            .AddTransient<ContentItemQueryTools>()
            .AddTransient<DataItemCommandTools>()
            .AddTransient<DataItemQueryTools>();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(SearchQuery).Assembly));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(NSSiteSearchQuery).Assembly));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(WebsiteSettingsQuery).Assembly));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(SpeakerTaxonomiesQuery).Assembly));
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(NACSFoundationPageQuery).Assembly));

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });


var assembly = typeof(NACSMagazine.Features.Home.NACSMagazineHomeController).Assembly;
builder.Services.AddControllersWithViews()
    .AddApplicationPart(assembly)
    .AddRazorRuntimeCompilation();

builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
{ options.FileProviders.Add(new EmbeddedFileProvider(assembly)); });

var nsAssembly = typeof(NACSShow.Features.Home.NACSShowHomeController).Assembly;
builder.Services.AddControllersWithViews()
    .AddApplicationPart(nsAssembly)
    .AddRazorRuntimeCompilation();

builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
{ options.FileProviders.Add(new EmbeddedFileProvider(nsAssembly)); });

//var ccAssembly = typeof(ConvenienceCares.Features.Home.CCHomeController).Assembly;
//builder.Services.AddControllersWithViews()
//    .AddApplicationPart(nsAssembly)
//    .AddRazorRuntimeCompilation();

//builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
//{ options.FileProviders.Add(new EmbeddedFileProvider(ccAssembly)); });

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddRazorPages();

builder.Services.AddKenticoTagManager(builder.Configuration);

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new FeatureLocationExpander());
});

var app = builder.Build();
app.InitKentico();

app.UseStaticFiles();

app.UseSession();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseCors();
app.UseAuthorization();
app.MapDefaultControllerRoute();

app.MapRazorPages();
app.MapControllers();

if (builder.Environment.IsQa() || builder.Environment.IsUat() || builder.Environment.IsProduction())
{
    app.UseKenticoCloud();
}

app.UseKentico();

app.Kentico().MapRoutes();

app.Run();
