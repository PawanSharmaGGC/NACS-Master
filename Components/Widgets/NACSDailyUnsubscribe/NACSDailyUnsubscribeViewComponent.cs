using Convenience.org.Components.Widgets.NACSDailyUnsubscribe;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using NACS.Protech.Entities;
using NACS.Protech.Framework;
using System;
using System.Linq;
using CMS.Core;
using CMS.EmailEngine;
using CMS.DataEngine;
using CMS.EmailLibrary;
using System.Text.RegularExpressions;

[assembly: RegisterWidget(identifier: NACSDailyUnsubscribeViewComponent.IDENTIFIER, name: "NACSDailyUnsubscribe",
    viewComponentType: typeof(NACSDailyUnsubscribeViewComponent),
    propertiesType: typeof(NACSDailyUnsubscribeProperties), Description = "NACSDailyUnsubscribe",
    IconClass = "icon-box", AllowCache = true)]

namespace Convenience.org.Components.Widgets.NACSDailyUnsubscribe
{
    public class NACSDailyUnsubscribeViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "NACSDailyUnsubscribe";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventLogService _eventLogService;
        private readonly IEmailService _emailService;
        private readonly IInfoProvider<EmailTemplateInfo> _emailTemplateInfoProvider;

        public NACSDailyUnsubscribeViewComponent(IEventLogService eventLogService, IEmailService emailService, IInfoProvider<EmailTemplateInfo> emailTemplateInfoProvider, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _eventLogService = eventLogService;
            _emailService = emailService;
            _emailTemplateInfoProvider = emailTemplateInfoProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<NACSDailyUnsubscribeProperties> properties)
        {
            string qs_nacsid = _httpContextAccessor.HttpContext.Request.Query["nacsid"];
            string qs_email = _httpContextAccessor.HttpContext.Request.Query["email"];
            var vm = new NACSDailyUnsubscribeViewModel();
            //If info in query string, run unsubscribe Web service
            if (qs_nacsid != string.Empty || qs_email != string.Empty)
            {
                var conRepo = new ContactRepository();
                Contact con = null;

                try
                {
                    if (!string.IsNullOrEmpty(qs_nacsid)) { con = conRepo.GetByAnyId(qs_nacsid); }
                    else if (!string.IsNullOrEmpty(qs_email))
                    {
                        con = conRepo.FindByPrimaryEmail(qs_email).First();
                    }

                    if (con != null)
                    {
                        var optins = conRepo.GetById<OptIns>(con.Id);
                        optins.NACSDaily = false;
                        conRepo.Save(optins);
                        vm.ShowErrorPanel = false;
                        vm.ShowUnsubscribedPanel = true;
                        vm.ShowUnsubscribedViaEmailPanel = false;
                    }
                    else
                    {
                        vm.ShowErrorPanel = true;
                        vm.ShowUnsubscribedPanel = false;
                        vm.ShowUnsubscribedViaEmailPanel = false;
                        vm.ErrorMessage = "An error occurred while trying to unsubscribe from NACS Daily.";
                    }
                }
                catch (Exception ex)
                {
                    vm.ShowErrorPanel = true;
                    vm.ShowUnsubscribedPanel = false;
                    vm.ShowUnsubscribedViaEmailPanel = false;
                    vm.ErrorMessage = "An error occurred while trying to unsubscribe from NACS Daily.";

                    vm.ErrorMessage = "An error occurred while trying to unsubscribe from NACS Daily.";
                    var emailTemplate = _emailTemplateInfoProvider.Get("SiteErrorMessageAlert").EmailTemplateCode;
                    string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

                    //Replacement's
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% Date %\}", DateTime.Today.ToString());
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% User %\}", qs_nacsid + "," + qs_email);
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% ErrorMsg %\}", vm.ErrorMessage);
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% ASPNetError %\}", ex.Message);
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% Page %\}", currentUrl);

                    // Creates the email message
                    EmailMessage msg = new EmailMessage()
                    {
                        From = "webmaster@nacsonline.com",
                        Recipients = "webmaster@nacsonline.com",
                        Subject = "NACS ONLINE ERROR",
                        Body = emailTemplate,
                        EmailFormat = EmailFormatEnum.Html,
                    };

                    // Sends out the email message
                    await _emailService.SendEmail(msg);

                    //log the exception for future trobleshooting
                    _eventLogService.LogException("An error occurred while trying to unsubscribe from NACS Daily.", EventTypeEnum.Error.ToString(), ex, additionalMessage: "NACS Exceptions");
                }
            }
            else
            {
                vm.ShowErrorPanel = true;
                vm.ShowUnsubscribedPanel = false;
                vm.ShowUnsubscribedViaEmailPanel = false;
                vm.ErrorMessage = "<strong>An error occurred while trying to unsubscribe from NACS Daily: </strong>Incomplete Query String";
            }

            return View("~/Components/Widgets/NACSDailyUnsubscribe/_NACSDailyUnsubscribe.cshtml", vm);
        }
    }
}