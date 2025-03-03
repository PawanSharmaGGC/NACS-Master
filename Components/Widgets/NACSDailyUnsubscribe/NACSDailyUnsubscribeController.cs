using CMS.Core;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.EmailLibrary;
using CMS.MacroEngine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Xrm.Sdk;
using NACS.Protech.Entities;
using NACS.Protech.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Convenience.org.Components.Widgets.NACSDailyUnsubscribe
{
    public class NACSDailyUnsubscribeController : Controller
    {
        private readonly IEventLogService _eventLogService;
        private readonly IEmailService _emailService;
        private readonly IInfoProvider<EmailTemplateInfo> _emailTemplateInfoProvider;
        public NACSDailyUnsubscribeController(IEventLogService eventLogService, IEmailService emailService, IInfoProvider<EmailTemplateInfo> emailTemplateInfoProvider)
        {
            _eventLogService = eventLogService;
            _emailService = emailService;
            _emailTemplateInfoProvider = emailTemplateInfoProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Update(string email)
        {
            var vm = new NACSDailyUnsubscribeViewModel();
            try
            {
                var conRepo = new ContactRepository();
                Contact con = conRepo.FindByPrimaryEmail(email).FirstOrDefault();

                var optins = conRepo.GetById<OptIns>(con.Id);
                optins.NACSDaily = false;
                conRepo.Save(optins);

                vm.ShowErrorPanel = false;
                vm.ShowUnsubscribedPanel = true;
                vm.ShowUnsubscribedViaEmailPanel = false;

                return View("~/Components/Widgets/NACSDailyUnsubscribe/_NACSDailyUnsubscribe.cshtml", vm);
            }
            catch (Exception ex)
            {
                vm.ShowErrorPanel = true;
                vm.ShowUnsubscribedPanel = false;
                vm.ShowUnsubscribedViaEmailPanel = false;
                vm.ErrorMessage = "An error occurred while trying to unsubscribe from NACS Daily.";
                var emailTemplate = _emailTemplateInfoProvider.Get("SiteErrorMessageAlert").EmailTemplateCode;
                string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

                //Replacement's
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Date %\}", DateTime.Today.ToString());
                emailTemplate = Regex.Replace(emailTemplate, @"\{% User %\}", email);
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

                return View("~/Components/Widgets/NACSDailyUnsubscribe/_NACSDailyUnsubscribe.cshtml", vm);
            }
        }
    }
}
