using Convenience;

using ConvenienceCares.Operations;
using ConvenienceCares.PageTemplates;
using ConvenienceCares.Services;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Services;

[assembly: RegisterWebPageRoute(Page.CONTENT_TYPE_NAME, typeof(NACSFoundationPageTemplateController), WebsiteChannelNames = new[] { Constants.WEBSITE_CHANNEL_NAME })]

namespace ConvenienceCares.PageTemplates;

public class NACSFoundationPageTemplateController : Controller
{
    private readonly IMediator mediator;
    private readonly IWebPageDataContextRetriever contextRetriever;
    private readonly SocialMetaTagService metaService;
    private readonly IAssetItemService itemService;
    private readonly IHttpContextAccessor contextAccessor;

    public NACSFoundationPageTemplateController(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever,
        SocialMetaTagService _metaService, IAssetItemService _itemService, IHttpContextAccessor _contextAccessor)
    {
        mediator = _mediator;
        contextRetriever = _contextRetriever;
        metaService = _metaService;
        itemService = _itemService;
        contextAccessor = _contextAccessor;
    }
  
    public async Task<IActionResult> Index()
    {
        if (!contextRetriever.TryRetrieve(out var data))
        {
            return NotFound();
        }

        var page = await mediator.Send(new NACSFoundationPageQuery(data.WebPage));

        SetMetaFields(page);

        return new TemplateResult(page); //ConveniencePageViewModel.GetViewModel(page));
    }

    private void SetMetaFields(Page page)
    {
        // social meta tags
        var metaSocialMediaHandler = page.SocialMediaHandler;
        var metaTitle = !string.IsNullOrEmpty(page.Title) ? page.Title :
                        !string.IsNullOrEmpty(page.SystemFields.WebPageItemName) ? page.SystemFields.WebPageItemName : page.DefaultTitle;
        var metaDescription = !string.IsNullOrEmpty(page.Description) ? page.Description : page.DefaultDescription;

        var metaImage = itemService.RetrieveMediaFileImage(page.Image?.FirstOrDefault()).GetAwaiter().GetResult()?.URLData?.RelativePath;
        if (metaImage == null)
        {
            metaImage = itemService.RetrieveMediaFileImage(page.RollupImage?.FirstOrDefault()).GetAwaiter().GetResult()?.URLData?.RelativePath;
        }
        string UrlSchemeHost = $"{contextAccessor.HttpContext?.Request.Scheme}://{contextAccessor.HttpContext?.Request.Host}";

        metaImage = !string.IsNullOrEmpty(metaImage) ? UrlSchemeHost + metaImage.Replace("~", "") : page.DefaultImage;

        metaService.SetMeta(new(metaSocialMediaHandler, metaTitle, metaDescription, metaImage));

    }
}
