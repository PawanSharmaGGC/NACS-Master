using ConvenienceCares.Interface.Services;
using ConvenienceCares.Models;
using ConvenienceCares.Operations;
using MediatR;
using NACS.Portal.Core.Services;

namespace ConvenienceCares.Services;

public class SocialMetaTagService
{
    private readonly IMediator mediator;
    private readonly IAssetItemService assetItemService;
    private WebpageMetaTagsViewModel meta = new("", "", "", "");

    public SocialMetaTagService(IMediator mediator,
    IAssetItemService assetItemService)
    {
        this.mediator = mediator;
        this.assetItemService = assetItemService;
    }

    public async Task<WebpageMetaTagsViewModel> GetMeta()
    {
        var settings = await mediator.Send(new WebsiteSettingsQuery());

        string titlePattern = settings.PageTitleFormat ?? "{0}";
        string pageTitle = meta.DefaultMetaTitle;

        string fullTitle = string.Format(titlePattern, pageTitle).Trim(' ').TrimStart('|').Trim(' ');

        meta = meta with { DefaultMetaTitle = fullTitle };

        return meta;
    }

    public void SetMeta(WebpageMetaTagsViewModel meta) => this.meta = meta;
}