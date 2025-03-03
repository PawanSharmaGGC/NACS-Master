using CMS.DataEngine;
using CMS.Websites;

using Kentico.PageBuilder.Web.Mvc;

using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Models;


namespace ConvenienceCares.Components.Widgets.AwarenessCampaignForm;

public class AwarenessCampaignFormWidgetViewComponent : ViewComponent
{
    IInfoProvider<WebsiteCaptchaSettingsInfo> infoProvider;

    public AwarenessCampaignFormWidgetViewComponent(IInfoProvider<WebsiteCaptchaSettingsInfo> infoProvider)
    {
        this.infoProvider = infoProvider;
    }
    public IViewComponentResult Invoke(ComponentViewModel<FormWidgetProperties> vm)
    {
        var captchaInfo = infoProvider.Get();
        var channelCaptchaInfo = captchaInfo.GetEnumerableTypedResult().FirstOrDefault();

        var model = new FormWidgetViewModel(vm.Properties);

        model.captchaSiteKey = channelCaptchaInfo?.WebsiteCaptchaSettingsReCaptchaSiteKey.ToString();
        return View("~/Components/Widgets/AwarenessCampaignForm/_AwarenessCampaignForm.cshtml", model);
    }

}
