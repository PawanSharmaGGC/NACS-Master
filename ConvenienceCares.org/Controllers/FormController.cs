using ConvenienceCares.Models;
using Microsoft.AspNetCore.Mvc;
using CMS.Core;
using CMS.DataEngine;
using CMS.EmailEngine;
using CMS.OnlineForms;
using CMS.Websites;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ConvenienceCares.Controllers;

public class FormController : Controller
{
	//private readonly IFormProcessingService _formProcessingService;

 //   public FormController(IFormProcessingService formProcessingService)
 //   {
 //       _formProcessingService = formProcessingService;
 //   }

	//#region Form save data

	
	
    private readonly IInfoProvider<BizFormInfo> _bizFormInfoProvider;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IInfoProvider<WebsiteCaptchaSettingsInfo> _websiteCaptchaInfoProvider;

    public FormController(
        IInfoProvider<BizFormInfo> bizFormInfoProvider,
        IEmailService emailService,
        IConfiguration configuration,
        IInfoProvider<WebsiteCaptchaSettingsInfo> websiteCaptchaInfoProvider)
    {
        _bizFormInfoProvider = bizFormInfoProvider;
        _emailService = emailService;
        _configuration = configuration;
        _websiteCaptchaInfoProvider = websiteCaptchaInfoProvider;
    }

    #region Form save data


    ////Contact form submit
    //[HttpPost]
    //[Route("/ContactForm/SaveData")]
    //public async Task<IActionResult> ContactFormSaveData(ContactFormViewModel formData) =>
    //        Json(await _formProcessingService.ProcessFormDataAsync(formData, "Contact", "EmailSettings:Contact", "Contact form", true));
    //Get involved form submit

    //   [HttpPost]
    //   [Route("/GetInvolvedForm/SaveData")]
    //   public async Task<IActionResult> GetInvolvedFormSaveData(GetInvolvedFormViewModel formData) =>
    //       Json(await _formProcessingService.ProcessFormDataAsync(formData, "NACSFoundation_GetInvolved", "EmailSettings:GetInvolved", "Get involved form"));

    ////Sponsor nacs form submit
    //[HttpPost]

    //[Route("/SponsorNACSForm/SaveData")]
    //public async Task<IActionResult> SponsorNACSFormSaveData(SponsorNACSFormViewModel formData) =>
    //       Json(await _formProcessingService.ProcessFormDataAsync(formData, "SponsorTheNACSFoundation", "EmailSettings:SponsorNACS", "Sponsor NACS foundation form"));

    ////Corporate involvement form submit
    //[HttpPost]
    //[Route("/CorporateInvolvementForm/SaveData")]
    //public async Task<IActionResult> CorporateInvolvementFormSaveData(CorporateInvolvementFormViewModel formData) =>
    //       Json(await _formProcessingService.ProcessFormDataAsync(formData, "CorporateInvolvement", "EmailSettings:CorporateInvolvement", "Corporate involvement form", true));

    ////NACS 24/7 day form submit
    //[HttpPost]
    //[Route("/NACS24by7DayForm/SaveData")]
    //   public async Task<IActionResult> NACS_24_7DayFormSaveData(NACS_24_7DayFormViewModel formData) =>
    //       Json(await _formProcessingService.ProcessFormDataAsync(formData, "NACSFoundation_24_7Day", "EmailSettings:NACS_24_7Day", "NACS foundation - 24/7 day form"));

    ////NACS 24/7 day form submit
    //[HttpPost]
    //[Route("/AwarenessCampaignForm/SaveData")]
    //   public async Task<IActionResult> AwarenessCampaignFormSaveData(AwarenessCampaignFormViewModel formData) =>
    //       Json(await _formProcessingService.ProcessFormDataAsync(formData, "NACSFoundationAwarenessCampaign", "EmailSettings:AwarenessCampaign", "NACS foundation awareness campaign form", true));

    ////Scholarship updates form submit
    //[HttpPost]
    //[Route("/ScholarshipUpdatesForm/UpdateData")]
    //   public async Task<IActionResult> ScholarshipUpdatesFormUpdateData(ScholarshipUpdatesFormViewModel formData) =>
    //       Json(await _formProcessingService.ProcessFormDataAsync(formData, "ScholarshipUpdates", "EmailSettings:ScholarshipUpdates", "NACS foundation scholarship updates form"));

    
    //Contact form submit
    [HttpPost]
    [Route("/ContactForm/SaveData")]
    public Task<IActionResult> ContactFormSaveData(ContactFormViewModel formData) =>
        ProcessFormDataAsync(formData, "Contact", "EmailSettings:Contact", "Contact form", true);

    //Get involved form submit
    [HttpPost]
    public Task<IActionResult> GetInvolvedFormSaveData(GetInvolvedFormViewModel formData) =>
        ProcessFormDataAsync(formData, "NACSFoundation_GetInvolved", "EmailSettings:GetInvolved", "Get involved form");

    //Sponsor nacs form submit
    [HttpPost]
    [Route("/SponsorNACSForm/SaveData")]
    public Task<IActionResult> SponsorNACSFormSaveData(SponsorNACSFormViewModel formData) =>
        ProcessFormDataAsync(formData, "SponsorTheNACSFoundation", "EmailSettings:SponsorNACS", "Sponsor NACS foundation form");

    //Corporate involvement form submit
    [HttpPost]
    [Route("/CorporateInvolvementForm/SaveData")]
    public Task<IActionResult> CorporateInvolvementFormSaveData(CorporateInvolvementFormViewModel formData) =>
        ProcessFormDataAsync(formData, "CorporateInvolvement", "EmailSettings:CorporateInvolvement", "Corporate involvement form", true);

    //NACS 24/7 day form submit
    [HttpPost]
    [Route("/NACS24by7DayForm/SaveData")]
    public Task<IActionResult> NACS_24_7DayFormSaveData(NACS_24_7DayFormViewModel formData) =>
        ProcessFormDataAsync(formData, "NACSFoundation_24_7Day", "EmailSettings:NACS_24_7Day", "NACS foundation - 24/7 day form");

    //NACS 24/7 day form submit
    [HttpPost]
    [Route("/AwarenessCampaignForm/SaveData")]
    public Task<IActionResult> AwarenessCampaignFormSaveData(AwarenessCampaignFormViewModel formData) =>
        ProcessFormDataAsync(formData, "NACSFoundationAwarenessCampaign", "EmailSettings:AwarenessCampaign", "NACS foundation awareness campaign form", true);

    #endregion

    #region Private Methods

    private async Task<IActionResult> ProcessFormDataAsync<T>(
        T formData,
        string formKey,
        string emailSettingsKey,
        string submitFormName,
        bool hasRecaptcha = false) where T : class
    {
        if (formData == null)
        {
			LogException(nameof(ProcessFormDataAsync), new Exception("Invalid form data"), $"SubmitFormName: {submitFormName} - Invalid form data.");
            return Json(new { success = false, message = "Invalid form data." });
        }

        try
        {
            if (hasRecaptcha)
            {
                var recaptchaResponse = GetRecaptchaResponse(formData);
                if (!await VerifyRecaptcha(recaptchaResponse))
                    return Json(new { success = false, message = "reCaptcha validation failed." });
            }

            return await ProcessFormDataInternal(formData, formKey, emailSettingsKey, submitFormName);
        }
        catch (Exception ex)
        {
            LogException(nameof(ProcessFormDataAsync), ex, $"SubmitFormName: {submitFormName}");
            return Json(new { success = false, message = "The entered values cannot be saved. Please see the fields below for details." });
        }
    }

	private async Task<IActionResult> ProcessFormDataInternal<T>(
		T formData,
		string formKey,
		string emailSettingsKey,
		string submitFormName) where T : class
	{
		var formObject = await Task.Run(() => _bizFormInfoProvider.Get(formKey));
		if (formObject == null)
		{
			LogException(nameof(ProcessFormDataInternal), new Exception("Form Object not found"), $"SubmitFormName: {submitFormName} - Form configuration not found.");
			return Json(new { success = false, message = "The entered values cannot be saved. Please see the fields below for details." });
		}

		var formClass = await Task.Run(() => DataClassInfoProvider.GetDataClassInfo(formObject.FormClassID));
		if (formClass == null)
		{
			LogException(nameof(ProcessFormDataInternal), new Exception("Form Class Definition not found"), $"SubmitFormName: {submitFormName} - Form class configuration not found.");
			return Json(new { success = false, message = "The entered values cannot be saved. Please see the fields below for details." });
		}

		var className = formClass.ClassName;
		var newFormItem = BizFormItem.New(className);

		if (!SetFormItemValues(newFormItem, formData))
			return Json(new { success = false, message = "The entered values cannot be saved. Please see the fields below for details." });

		await Task.Run(() => newFormItem.Insert());

		if (!SendNotificationEmail(formData, formKey, emailSettingsKey, submitFormName))
		{
			LogException(nameof(ProcessFormDataInternal), new Exception("Notification Email not sent"), $"SubmitFormName: {submitFormName} - Failed to send notification email.");
			return Json(new { success = true });
		}

		return Json(new { success = true });
	}

	private bool SendNotificationEmail<T>(
		T formData,
		string formKey,
		string emailSettingsKey,
		string submitFormName) where T : class
	{
		try
		{
			var emailFrom = _configuration[$"{emailSettingsKey}:From"] ?? string.Empty;
			var emailTo = _configuration[$"{emailSettingsKey}:To"] ?? string.Empty;
			var emailSubject = _configuration[$"{emailSettingsKey}:Subject"] ?? string.Empty;

			var emailBody = GenerateEmailBody(formData, formKey);

			var emailViewModel = new EmailViewModel
			{
				EmailFrom = emailFrom,
				EmailTo = emailTo,
				EmailBody = emailBody,
				EmailSubject = emailSubject,
				SubmitFormName = submitFormName
			};

			return SendEmail(emailViewModel);
		}
		catch (Exception ex)
		{
			LogException(nameof(SendNotificationEmail), ex);
			return false;
		}
	}

    private bool SendEmail(EmailViewModel emailViewModel)
    {
        try
        {
            var msg = new EmailMessage
            {
                From = emailViewModel.EmailFrom,
                Recipients = emailViewModel.EmailTo,
                Priority = EmailPriorityEnum.Normal,
                Subject = emailViewModel.EmailSubject,
                Body = emailViewModel.EmailBody
            };

            _emailService.SendEmail(msg);
            return true;
        }
        catch (Exception ex)
        {
            LogException(nameof(SendEmail), ex,
                $"Could not send email. From: {emailViewModel.EmailFrom}, Recipients: {emailViewModel.EmailTo}, " +
                $"Subject: {emailViewModel.EmailSubject}, SubmitFormName: {emailViewModel.SubmitFormName}");
            return false;
        }
    }

    private string GetRecaptchaResponse<T>(T formData) where T : class
    {
        var recaptchaProperty = formData.GetType().GetProperties()
            .FirstOrDefault(p => p.Name.Contains("recaptcha", StringComparison.OrdinalIgnoreCase));

        return recaptchaProperty?.GetValue(formData)?.ToString() ?? string.Empty;
    }

    private async Task<bool> VerifyRecaptcha(string response)
    {
        if (string.IsNullOrEmpty(response))
            return false;

        try
        {
            var captchaInfo = _websiteCaptchaInfoProvider.Get().FirstOrDefault();
            if (captchaInfo == null)
                return false;

            var secret = captchaInfo.WebsiteCaptchaSettingsReCaptchaSecretKey.ToString();
            var requestUri = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={response}";

            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync(requestUri, null);
                var jsonResult = await result.Content.ReadAsStringAsync();

				var jsonData = JsonConvert.DeserializeObject<dynamic>(jsonResult);

				if (jsonData == null)
					return false;
				return jsonData.success == "true";
            }
        }
        catch (Exception ex)
        {
            LogException(nameof(VerifyRecaptcha), ex);
            return false;
        }
    }

    private bool SetFormItemValues(BizFormItem formItem, dynamic formData)
    {
        try
        {
            foreach (var property in formData.GetType().GetProperties())
            {
                if (property.PropertyType.Name.Contains("list", StringComparison.OrdinalIgnoreCase))
                    formItem.SetValue(property.Name, string.Join(";", property.GetValue(formData)));
                else
                    formItem.SetValue(property.Name, property.GetValue(formData));
            }
            return true;
        }
        catch (Exception ex)
        {
            LogException(nameof(SetFormItemValues), ex);
            return false;
        }
    }

    private string GenerateEmailBody(dynamic formData, string formKey)
    {
        var emailBody = "";

        if (formKey.Equals("NACSFoundationAwarenessCampaign", StringComparison.OrdinalIgnoreCase))
        {
            emailBody += AddAwarenessCampaignEmailBody(formData);
        }

        foreach (var property in formData.GetType().GetProperties())
        {
            if (property.Name.Contains("recaptcha", StringComparison.OrdinalIgnoreCase))
                continue;

            var value = property.PropertyType.Name.Contains("list", StringComparison.OrdinalIgnoreCase)
                ? string.Join(";", property.GetValue(formData))
                : property.GetValue(formData)?.ToString();



            emailBody += $"<p>{property.Name}: {value}</p><br/>";
        }
        return emailBody;
    }

    private string AddAwarenessCampaignEmailBody(dynamic formData)
    {
        var firstName = string.Empty;
        var lastName = string.Empty;
        var company = string.Empty;
        var emailBody = string.Empty;
        foreach (var property in formData.GetType().GetProperties())
        {
            var value = property.PropertyType.Name.Contains("list", StringComparison.OrdinalIgnoreCase)
                ? string.Join(";", property.GetValue(formData))
                : property.GetValue(formData)?.ToString();

            if (property.Name.Contains("FirstName", StringComparison.OrdinalIgnoreCase))
                firstName = value;
            else if (property.Name.Contains("LastName", StringComparison.OrdinalIgnoreCase))
                lastName = value;
            else if (property.Name.Contains("Company", StringComparison.OrdinalIgnoreCase))
                company = value;


        }
        emailBody = $"{firstName} {lastName} from {company} is interested in supporting NACS Foundation.<br />\r\n<br />";
        return emailBody;
    }

    private void LogException(string methodName, Exception ex, string additionalInfo = "")
    {
        var logService = Service.Resolve<IEventLogService>();
        logService.LogException(nameof(FormController), methodName, ex, additionalInfo);
    }

    #endregion
}

