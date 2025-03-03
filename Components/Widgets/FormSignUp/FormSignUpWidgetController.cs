using System.Threading.Tasks;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.OnlineForms;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.Widgets.FormSignUp
{
    public class FormSignUpWidgetController : Controller
    {
        private readonly IBizFormInfoProvider bizFormInfoProvider;

        public FormSignUpWidgetController(IBizFormInfoProvider bizFormInfoProvider)
        {
            this.bizFormInfoProvider = bizFormInfoProvider;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormSignUp(FormSignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please check your inputs." });
            }

            var formObject = bizFormInfoProvider.Get("FormSignUp");

            if (formObject == null)
            {
                return StatusCode(500, "Form configuration not found");
            }

            var formClass = DataClassInfoProvider.GetDataClassInfo(formObject.FormClassID);
            if (formClass == null)
            {
                return StatusCode(500, "Form Class configuration not found");
            }

            // Create a new form item
            var newFormItem = BizFormItem.New(formClass.ClassName);
            newFormItem.SetValue("FirstName", ValidationHelper.GetString(model.FirstName, ""));
            newFormItem.SetValue("LastName", ValidationHelper.GetString(model.LastName, ""));
            newFormItem.SetValue("Email", ValidationHelper.GetString(model.Email, ""));
            newFormItem.SetValue("CompanyName", ValidationHelper.GetString(model.CompanyName, ""));
            newFormItem.Insert();

            // Return success response
            return Json(new { success = true, message = "You have signed up for the webinar. See you there!" });
        }
    }
}
