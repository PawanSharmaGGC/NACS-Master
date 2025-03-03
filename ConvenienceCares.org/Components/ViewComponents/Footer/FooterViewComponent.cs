using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Websites.Routing;
using CMS.Websites;
using ConvenienceCares.Repository;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ConvenienceCares.Models;
using CMS.Helpers;
using MediatR;
using ConvenienceCares.Operations;

namespace ConvenienceCares.Components.ViewComponents.Footer;

public class FooterViewComponent : ViewComponent
{
    private readonly IPreferredLanguageRetriever currentLanguageRetriever;
    private readonly IMediator mediator;
    private readonly WebSiteSettingsRepository webSiteSettingsRepository;
    private readonly IInfoProvider<WebsiteChannelInfo> websiteChannelInfoProvider;
    private readonly IInfoProvider<ChannelInfo> channelInfoProvider;
    protected readonly IWebsiteChannelContext websiteChannelContext;
    private readonly IHttpContextAccessor contextAccessor;



    public FooterViewComponent(IPreferredLanguageRetriever currentLanguageRetriever, IMediator mediator, WebSiteSettingsRepository webSiteSettingsRepository,
        IInfoProvider<WebsiteChannelInfo> websiteChannelInfoProvider, IWebsiteChannelContext websiteChannelContext
        , IInfoProvider<ChannelInfo> channelInfoProvider, IHttpContextAccessor contextAccessor)
    {
        this.currentLanguageRetriever = currentLanguageRetriever;
        this.mediator = mediator;
        this.webSiteSettingsRepository = webSiteSettingsRepository;
        this.websiteChannelInfoProvider = websiteChannelInfoProvider;
        this.websiteChannelContext = websiteChannelContext;
        this.channelInfoProvider = channelInfoProvider;
        this.contextAccessor = contextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var languageName = currentLanguageRetriever.Get();

        //var websiteSettings = await mediator.Send(new WebsiteSettingsQuery());
        var websiteSettings = await webSiteSettingsRepository.GetWebSiteSettingsAsync(languageName, cancellationToken: HttpContext.RequestAborted);

        var websiteChannelInfo = await websiteChannelInfoProvider.GetAsync(websiteChannelContext.WebsiteChannelID);
        var channel = await channelInfoProvider.GetAsync(websiteChannelContext.WebsiteChannelName);

        // Validate if the url is a well-formed URI
        string footerUrl = ValidationHelper.GetString(websiteSettings?.Footer_CopyRightUrl, "");
        if (!string.IsNullOrEmpty(footerUrl) && (!footerUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !footerUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
        {
            UriBuilder footerUriBuilder = new UriBuilder(contextAccessor.HttpContext?.Request.Scheme, footerUrl);
            footerUrl = footerUriBuilder.Uri.ToString();
        }

        FooterViewModel footerViewModel = new FooterViewModel()
        {
            CopyRightText = websiteSettings?.Footer_CopyRightText,
            CopyRightUrl = footerUrl,
            CopyRightUrlText = websiteSettings?.Footer_CopyRightUrl,
        };

        return View("~/Components/ViewComponents/Footer/Footer.cshtml", footerViewModel);
    }
}
