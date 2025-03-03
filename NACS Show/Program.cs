using Kentico.Activities.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Kentico.Xperience.Cloud;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NACSShow.Helpers;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;


builder.Services.Configure<RouteOptions>(options =>
{

    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
    options.LowercaseQueryStrings = false;
})

    .AddLocalization()
    .Configure<StaticFileOptions>(o =>
    {
        o.OnPrepareResponse = context =>
        {
            context.Context.Response.Headers.Append("Cache-Control", "public, max-age=604800");
        };
    })
    .AddControllersWithViews()
    .AddViewLocalization()
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
    features.UsePageBuilder(new PageBuilderOptions { ContentTypeNames = new[] { NACSShow.Page.CONTENT_TYPE_NAME } });
    features.UseWebPageRouting();
    features.UseActivityTracking();
    features.UseEmailStatisticsLogging();
    features.UseEmailMarketing();
});

builder.Services.AddAuthentication();

builder.Services.AddAuthorization();

builder.Services.AddDIServices();

builder.Services.AddAuthentication();

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddRazorPages();

builder.Services.AddKenticoTagManager(builder.Configuration);


var app = builder.Build();

app.InitKentico();

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseKentico();

app.UseCors();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

if (builder.Environment.IsQa() || builder.Environment.IsUat() || builder.Environment.IsProduction())
{
    app.UseKenticoCloud();
}

app.Kentico().MapRoutes();

app.Run();
