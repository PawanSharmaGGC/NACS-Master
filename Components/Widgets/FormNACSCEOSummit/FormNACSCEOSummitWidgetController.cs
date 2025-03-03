using System.Configuration;
using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.Helpers;
using CMS.OnlineForms;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.Widgets.FormNACSCEOSummit
{
    public class FormNACSCEOSummitWidgetController : Controller
    {
        private readonly IBizFormInfoProvider bizFormInfoProvider;

        public FormNACSCEOSummitWidgetController(IBizFormInfoProvider bizFormInfoProvider)
        {
            this.bizFormInfoProvider = bizFormInfoProvider;
        }

        // This action handles the form submission (POST request)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormNACSCEOSummit(FormNACSCEOSummitViewModel model)
        {
            if (!ModelState.IsValid) // Check if the form data is valid
            {
                return Json(new { success = false, message = "Please check your inputs." });
            }

            // Get form object from BizForms
            var formObject = bizFormInfoProvider.Get("FormNACSCEOSummit");

            if (formObject == null) // If form configuration is not found
            {
                return StatusCode(500, "Form configuration not found");
            }

            var formClass = DataClassInfoProvider.GetDataClassInfo(formObject.FormClassID);
            if (formClass == null) // If form class configuration is not found
            {
                return StatusCode(500, "Form Class configuration not found");
            }

            // Create a new form item
            var newFormItem = BizFormItem.New(formClass.ClassName);
            newFormItem.SetValue("FirstName", ValidationHelper.GetString(model.FirstName, ""));
            newFormItem.SetValue("LastName", ValidationHelper.GetString(model.LastName, ""));
            newFormItem.SetValue("Email", ValidationHelper.GetString(model.Email, ""));
            newFormItem.SetValue("CompanyName", ValidationHelper.GetString(model.CompanyName, ""));
            newFormItem.SetValue("DropDown", ValidationHelper.GetString(model.DropDown, ""));
            newFormItem.Insert(); // Insert the form item into the database

            // Prepare the email body
            var emailBody = $@"<p> Email : {model.Email}</p>";

            var emailMessage = new EmailMessage
            {
                // Set the email properties here (e.g., From, To, Subject, Body)
                // Example:
                // From = "noreply@yourdomain.com",
                // Recipients = "recipient@domain.com",
                // Subject = "NACS CEO Summit Submission",
                // Body = emailBody,
            };

            // Send the email
            // EmailSender.SendEmailAsync(emailMessage); // Use your email sending logic here.

            // Return a JSON response indicating success
            return Json(new { success = true, message = "You have submitted NACS CEO Summit, See you there!" });
        }
    }
}
