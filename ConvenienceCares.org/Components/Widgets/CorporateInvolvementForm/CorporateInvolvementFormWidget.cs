using CMS.DataEngine;
using CMS.Websites;

using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Models;

namespace ConvenienceCares.Components.Widgets.CorporateInvolvementForm;

public class CorporateInvolvementFormWidgetViewComponent : ViewComponent
{
	IInfoProvider<WebsiteCaptchaSettingsInfo> infoProvider;

	public CorporateInvolvementFormWidgetViewComponent(IInfoProvider<WebsiteCaptchaSettingsInfo> infoProvider) => this.infoProvider = infoProvider;
	public IViewComponentResult Invoke(ComponentViewModel<FormWidgetProperties> vm)
	{
		var captchaInfo = infoProvider.Get();
		var channelCaptchaInfo = captchaInfo.GetEnumerableTypedResult().FirstOrDefault();

		var model = new FormWidgetViewModel(vm.Properties);

		if (channelCaptchaInfo != null)
		{
			model.captchaSiteKey = channelCaptchaInfo.WebsiteCaptchaSettingsReCaptchaSiteKey.ToString();
		}
		else
		{
			model.captchaSiteKey = string.Empty; // or handle the null case appropriately
		}

		return View("~/Components/Widgets/CorporateInvolvementForm/_CorporateInvolvementForm.cshtml", model);
	}
}
