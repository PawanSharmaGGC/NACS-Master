using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.EmailEngine;
using CMS.OnlineForms;
using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.FormDownloadReport
{
    public class FormDownloadReportWidgetController : Controller
    {
        private readonly IBizFormInfoProvider bizFormInfoProvider;
        private readonly IConfiguration _configuration;

        public FormDownloadReportWidgetController(IBizFormInfoProvider bizFormInfoProvider, IConfiguration configuration)
        {
            this.bizFormInfoProvider = bizFormInfoProvider;
            _configuration = configuration;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormDownloadReport(FormDownloadReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please check your inputs." });
            }
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = _configuration["reCAPTCHA:SecretKey"];
            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                return Json(new { success = false, message = "Please complete the reCAPTCHA." });
            }
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", secretKey),
                    new KeyValuePair<string, string>("response", recaptchaResponse)
                });

                var result = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                if (result.ReasonPhrase != "OK")
                {
                    return Json(new { success = false, message = "CAPTCHA verification failed. Please try again." });
                }
                else
                {
                    var formObject = bizFormInfoProvider.Get("FormDownloadReport");
                    if (formObject == null)
                    {
                        return StatusCode(500, "Form configuration not found");
                    }

                    var formClass = DataClassInfoProvider.GetDataClassInfo(formObject.FormClassID);
                    if (formClass == null)
                    {
                        return StatusCode(500, "Form Class configuration not found");
                    }

                    var newFormItem = BizFormItem.New(formClass.ClassName);
                    newFormItem.SetValue("FirstName", model.FirstName);
                    newFormItem.SetValue("LastName", model.LastName);
                    newFormItem.SetValue("Email", model.Email);
                    newFormItem.SetValue("CompanyName", model.CompanyName);
                    newFormItem.SetValue("UploadFile", model.UploadFile);
                    newFormItem.SetValue("Placeholder", model.Placeholder);
                    newFormItem.SetValue("MessageField", model.Message);
                    newFormItem.SetValue("DropDown", model.DropDown);
                    newFormItem.Insert();
                    return Json(new { success = true, message = "You have submitted the Download Report form. See you there!" });
                }
            }
            
        }
    }
}
