using CMS.EmailEngine;
<<<<<<< HEAD
=======
using Kentico.Content.Web.Mvc.Routing;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
>>>>>>> parent of 9e97e66 (ui component branch create)
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConvenienceCares.Configuration;

public static class ServiceCollectionXperienceExtensions
{
    public static IServiceCollection AddAppXperience(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env) =>
        services
<<<<<<< HEAD
=======
            .AddKentico(features =>
            {
                features.UsePageBuilder(new PageBuilderOptions
                {
                    //DefaultSectionIdentifier = "NacsShow.SingleColumnSection",
                    //RegisterDefaultSection = true,
                    ContentTypeNames = new[] {

                        Convenience.Page.CONTENT_TYPE_NAME
                    }
                });

                features.UseEmailMarketing();
                features.UseWebPageRouting();
            })


>>>>>>> parent of 9e97e66 (ui component branch create)
            .Configure<SmtpOptions>(config.GetSection("SmtpOptions"))
                    .AddXperienceSmtp()

            //.AddXperienceSystemSmtp(options =>
            //{
            //    var smtpConfig = config.GetSection("SmtpOptions");
            //    options.Server = new SmtpServer
            //    {
            //        Host = smtpConfig["Server:Host"],
            //        Port = smtpConfig.GetValue<int>("Server:Port"),
            //        UserName = smtpConfig["Server:UserName"],
            //        Password = smtpConfig["Password"]
            //    };
            //    options.Encoding = Encoding.UTF8;
            //    options.TransferEncoding = TransferEncoding.QuotedPrintable;
            //})

            .Configure<EmailQueueOptions>(o =>
            {
                o.ArchiveDuration = 14;
            })
            ;

}
