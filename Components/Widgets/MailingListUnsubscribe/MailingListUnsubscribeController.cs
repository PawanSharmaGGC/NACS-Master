using CMS.Core;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.EmailLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NACS.Protech.Entities;
using NACS.Protech.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Convenience.org.Components.Widgets.MailingListUnsubscribe
{
    public class MailingListUnsubscribeController : Controller
    {
        private readonly IEventLogService _eventLogService;
        private readonly IEmailService _emailService;
        private readonly IInfoProvider<EmailTemplateInfo> _emailTemplateInfoProvider;
        public MailingListUnsubscribeController(IEventLogService eventLogService, IEmailService emailService, IInfoProvider<EmailTemplateInfo> emailTemplateInfoProvider)
        {
            _eventLogService = eventLogService;
            _emailService = emailService;
            _emailTemplateInfoProvider = emailTemplateInfoProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Update(string nacsKey, string listName)
        {
            var vm = new MailingListUnsubscribeViewModel();
            try
            {
                var genRepo = new GeneralRepository();
                var optins = genRepo.GetById<OptIns>(new ProtechId(nacsKey));

                switch (listName)
                {
                    case "FMN Retailer Marketer":
                        optins.FMN_RetailerMarketer = false;
                        genRepo.Save(optins);
                        break;
                    case "FMN Commercial":
                        optins.FMN_CommercialFuels = false;
                        genRepo.Save(optins);
                        break;
                    case "Fuel for Thought":
                        optins.FuelForThought = false;
                        genRepo.Save(optins);
                        break;
                    case "Fuels Market News Weekly":
                        var isUnsubscribe = Unsubscribe("Fuels Market News Weekly", nacsKey);
                        if (!isUnsubscribe)
                        {
                            vm.ShowErrorPanel = true;
                            vm.ShowPromptPanel = false;
                            vm.ShowUnsubscribedPanel = false;
                            vm.ErrorMessage = "An error occurred while trying to unsubscribe from this mailing list.";
                            return View("~/Components/Widgets/MailingListUnsubscribe/_MailingListUnsubscribe.cshtml", vm);
                        }
                        break;
                }

                vm.ShowErrorPanel = false;
                vm.ShowPromptPanel = false;
                vm.ShowUnsubscribedPanel = true;

                return View("~/Components/Widgets/MailingListUnsubscribe/_MailingListUnsubscribe.cshtml", vm);
            }
            catch (Exception ex)
            {
                vm.ShowErrorPanel = true;
                vm.ShowPromptPanel = false;
                vm.ShowUnsubscribedPanel = false;
                vm.ErrorMessage = "An error occurred while trying to unsubscribe from this mailing list.";

                var emailTemplate = _emailTemplateInfoProvider.Get("SiteErrorMessageAlert").EmailTemplateCode;
                string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

                //Replacement's
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Date %\}", DateTime.Today.ToString());
                emailTemplate = Regex.Replace(emailTemplate, @"\{% User %\}", nacsKey + "," + listName);
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
                _eventLogService.LogException("An error occurred while trying to unsubscribe from this mailing list.", EventTypeEnum.Error.ToString(), ex, additionalMessage: "NACS Exceptions");

                return View("~/Components/Widgets/MailingListUnsubscribe/_MailingListUnsubscribe.cshtml", vm);
            }
        }

        private bool SendNotification(string listname, string id)
        {
            bool notificationSent = false;
            try
            {
                string To = "bremoyer@convenience.org";
                string From = "webmaster@convenience.org";
                string Sub = "Unsubscribe Wanted";

                EmailMessage em = new EmailMessage();
                em.EmailFormat = EmailFormatEnum.Html;
                em.From = From;
                em.Recipients = To;
                em.Subject = Sub;
                em.Body = "Please unsubscribe this contact from the " + listname + "list: "
                        + "<br/>ContactID: " + id;

                // Sends out the email message
                _emailService.SendEmail(em);
                notificationSent = true;
            }
            catch (Exception ex)
            {
                //log the exception for future trobleshooting
                var emailTemplate = _emailTemplateInfoProvider.Get("SiteErrorMessageAlert").EmailTemplateCode;
                string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

                //Replacement's
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Date %\}", DateTime.Today.ToString());
                emailTemplate = Regex.Replace(emailTemplate, @"\{% User %\}", id + "," + listname);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% ErrorMsg %\}", "An error occurred while trying to unsubscribe from this mailing list.");
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
                _emailService.SendEmail(msg);
                notificationSent = false;
            }
            return notificationSent;
        }

        private bool Unsubscribe(string listName, string contactid)
        {
            var genRepo = new GeneralRepository();
            MarketingList list = new MarketingList();
            bool removed = false;
            bool isUnsubscribed = true;
            try
            {
                //list found
                var mlist = genRepo.GetAll<MarketingList>("listname", listName).ToList();
                list = mlist.FirstOrDefault();

                ProtechId pid = new ProtechId(contactid);
                removed = list.RemoveMember(pid);
            }
            catch (Exception ex)
            {
                //no list found
                isUnsubscribed = SendNotification("Fuels Market News Weekly", contactid);
            }
            return isUnsubscribed;
        }
    }
}
