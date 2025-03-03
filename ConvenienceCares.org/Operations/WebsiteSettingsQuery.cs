using CMS.ContentEngine;
using ConvenienceCare;
using NACS.Portal.Core.Operations;

namespace ConvenienceCares.Operations;


public record WebsiteSettingsQuery : IQuery<WebSiteSettings>;
public record WebsiteSettingsQueryResponse(WebSiteSettings Settings);

public class WebsiteSettingsQueryHandler(ContentItemQueryTools tools) : ContentItemQueryHandler<WebsiteSettingsQuery, WebSiteSettings>(tools)
{
    public override async Task<WebSiteSettings> Handle(WebsiteSettingsQuery request, CancellationToken cancellationToken)
    {
        var b = new ContentItemQueryBuilder().ForContentType(WebSiteSettings.CONTENT_TYPE_NAME, p => p
            .TopN(1)
            .Columns([
                nameof(WebSiteSettings.PageTitleFormat),nameof(WebSiteSettings.NavigationMenuFolderPath),
                nameof(WebSiteSettings.HeaderLogo),nameof(WebSiteSettings.Footer_CopyRightText),
                nameof(WebSiteSettings.Footer_CopyRightUrl)
            ]));

        var r = await Executor.GetMappedResult<WebSiteSettings>(b, DefaultQueryOptions, cancellationToken);

        return r.First();
    }

    protected override ICacheDependencyKeysBuilder AddDependencyKeys(WebsiteSettingsQuery query, WebSiteSettings result, ICacheDependencyKeysBuilder builder) =>
        builder.AllContentItems(WebSiteSettings.CONTENT_TYPE_NAME);
}

