using CMS.Core;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.EmailLibrary;
using CMS.MacroEngine;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;

namespace Convenience.org.Components.Widgets.GeneralContactForm
{
    public class GeneralContactFormController : Controller
    {

        private readonly IInfoProvider<EmailTemplateInfo> emailTemplateInfoProvider;
        private readonly IEmailService emailService;
        private readonly IEventLogService _eventLogService;

        public GeneralContactFormController(IInfoProvider<EmailTemplateInfo> emailTemplateInfoProvider, IEmailService emailService, IEventLogService eventLogService)
        {
            this.emailTemplateInfoProvider = emailTemplateInfoProvider;
            this.emailService = emailService;
            _eventLogService = eventLogService;
        }
        [HttpPost]
        public bool SendEmails(GeneralContactFormModel model)
        {
            SendNotificationEmailToStaff(model);
            SendConfirmationEmailToUser(model);
            return true;
        }
        internal void SendNotificationEmailToStaff(GeneralContactFormModel model)
        {
            string Issue = model.Issue + (!string.IsNullOrEmpty(model.Event) ? " (" + model.Event + ")" : "");
            try
            {
                var emailTemplate = emailTemplateInfoProvider.Get("GeneralContactFormNotification").EmailTemplateCode;
                //Replacement's
                emailTemplate = Regex.Replace(emailTemplate, @"\{% FirstName %\}", model.FirstName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% LastName %\}", model.LastName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Email %\}", model.Email);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% CompanyName %\}", model.CompanyName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% PhoneExt %\}", model.Phone + (!string.IsNullOrEmpty(model.PhoneExt) ? string.Empty : " Ext: " + model.PhoneExt));

                emailTemplate = Regex.Replace(emailTemplate, @"\{% UnsubDaily %\}", model.IsUnsubDaily ? "Unsubscribe from NACS Daily<br />" : string.Empty);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% UnsubMagazine %\}", model.IsSubMagazine ? "Unsubscribe from NACS Magazine<br />" : string.Empty);

                //Section Added By Sam
                if (!string.IsNullOrEmpty(model.SameCompany))
                {
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% CompanyConfirmation %\}", model.SameCompany + " " + model.DiffCompany);
                }
                else if (!string.IsNullOrEmpty(model.DiffCompany))
                {
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% CompanyConfirmation %\}", model.DiffCompany + " " + model.SameCompany);
                }

                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldInfoLabel %\}", model.OldInfoLabel);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewInfoLabel %\}", model.NewInfoLabel);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldFirstName %\}", model.OldFirstName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldLastName %\}", model.OldLastName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldEmail %\}", model.OldEmail);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldPhone %\}", model.OldPhone);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldExt %\}", model.OldExt);

                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewFirstName %\}", model.NewFirstName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewLastName %\}", model.NewLastName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewEmail %\}", model.NewEmail);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewPhone %\}", model.NewPhone);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewExt %\}", model.NewExt);

                //End of Section
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Message %\}", model.Message);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Issue %\}", model.Issue);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% PrivacyRequest %\}", model.IsPrivacyRequest ? "Send me all my personal data<br />" : "");
                emailTemplate = Regex.Replace(emailTemplate, @"\{% PrivacyDelete %\}", model.IsPrivacyDelete ? "Delete my personal data entirely<br />" : "");

                string strFromMail = "info@convenience.org";
                string strToMail = model.Email.Trim();
                var msg = new EmailMessage();
                // Creates the email message object
                if (emailTemplate != null)
                {
                    msg.EmailFormat = EmailFormatEnum.Html;
                    msg.Recipients = strToMail;
                    msg.From = strFromMail;
                    msg.ReplyTo = strFromMail;
                    msg.Subject = string.Format("NACS Contact Confirmation  -  {0}", model.Subject);
                    msg.Body = emailTemplate;
                }
                emailService.SendEmail(msg);
            }
            catch (Exception ex)
            {
                //log exceptions
                _eventLogService.LogException("An error occurred while trying to SendConfirmationEmailToUser", EventTypeEnum.Error.ToString(), ex, additionalMessage: "NACS Exceptions");
            }
        }

        internal void SendConfirmationEmailToUser(GeneralContactFormModel model)
        {
            string Issue = model.Issue + (!string.IsNullOrEmpty(model.Event) ? " (" + model.Event + ")" : "");

            try
            {
                var emailTemplate = emailTemplateInfoProvider.Get("GeneralContactFormConfirmation").EmailTemplateCode;
                //Replacement's
                emailTemplate = Regex.Replace(emailTemplate, @"\{% FirstName %\}", model.FirstName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% LastName %\}", model.LastName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Email %\}", model.Email);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% CompanyName %\}", model.CompanyName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% PhoneExt %\}", model.Phone + (!string.IsNullOrEmpty(model.PhoneExt) ? string.Empty : " Ext: " + model.PhoneExt));

                emailTemplate = Regex.Replace(emailTemplate, @"\{% UnsubDaily %\}", model.IsUnsubDaily ? "Unsubscribe from NACS Daily<br />" : string.Empty);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% UnsubMagazine %\}", model.IsSubMagazine ? "Unsubscribe from NACS Magazine<br />" : string.Empty);

                //Section Added By Sam
                if (!string.IsNullOrEmpty(model.SameCompany))
                {
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% CompanyConfirmation %\}", model.SameCompany + " " + model.DiffCompany);
                }
                else if (!string.IsNullOrEmpty(model.DiffCompany))
                {
                    emailTemplate = Regex.Replace(emailTemplate, @"\{% CompanyConfirmation %\}", model.DiffCompany + " " + model.SameCompany);
                }

                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldInfoLabel %\}", model.OldInfoLabel);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewInfoLabel %\}", model.NewInfoLabel);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldFirstName %\}", model.OldFirstName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldLastName %\}", model.OldLastName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldEmail %\}", model.OldEmail);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldPhone %\}", model.OldPhone);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% OldExt %\}", model.OldExt);

                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewFirstName %\}", model.NewFirstName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewLastName %\}", model.NewLastName);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewEmail %\}", model.NewEmail);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewPhone %\}", model.NewPhone);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% NewExt %\}", model.NewExt);

                //End of Section
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Message %\}", model.Message);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% Issue %\}", model.Issue);
                emailTemplate = Regex.Replace(emailTemplate, @"\{% PrivacyRequest %\}", model.IsPrivacyRequest ? "Send me all my personal data<br />" : "");
                emailTemplate = Regex.Replace(emailTemplate, @"\{% PrivacyDelete %\}", model.IsPrivacyDelete ? "Delete my personal data entirely<br />" : "");

                string strFromMail = "info@convenience.org";
                string strToMail = model.Email.Trim();
                var msg = new EmailMessage();
                // Creates the email message object
                if (emailTemplate != null)
                {
                    msg.EmailFormat = EmailFormatEnum.Html;
                    msg.Recipients = strToMail;
                    msg.From = strFromMail;
                    msg.ReplyTo = strFromMail;
                    msg.Subject = string.Format("Ask NACS  -  {0} - {1}", model.Subject,model.Email);
                    msg.Body = emailTemplate;
                }
                emailService.SendEmail(msg);
            }
            catch (Exception ex)
            {
                //log exceptions
                _eventLogService.LogException("An error occurred while trying to SendConfirmationEmailToUser", EventTypeEnum.Error.ToString(), ex, additionalMessage: "NACS Exceptions");
            }
        }
    }
}
