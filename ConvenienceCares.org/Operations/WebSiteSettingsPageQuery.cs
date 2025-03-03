using CMS.ContentEngine;
using CMS.Websites;
using ConvenienceCare;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;

namespace ConvenienceCares.Operations;

public record WebSiteSettingsPageQuery(RoutedWebPage Page) : WebPageRoutedQuery<WebSiteSettings>(Page);

public class WebSiteSettingsPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<WebSiteSettingsPageQuery, WebSiteSettings>(tools)
{
    public override async Task<WebSiteSettings> Handle(WebSiteSettingsPageQuery request, CancellationToken cancellationToken = default)
    {
        var queryBuilder = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

        var webPages = await Executor.GetWebPageResult(queryBuilder, WebPageMapper.Map<WebSiteSettings>, DefaultQueryOptions, cancellationToken);

        return webPages.First();
    }
}

