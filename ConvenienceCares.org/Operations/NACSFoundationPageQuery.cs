using CMS.ContentEngine;
using CMS.Websites;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;
using Convenience;

namespace ConvenienceCares.Operations;

public record NACSFoundationPageQuery(RoutedWebPage Page) : WebPageRoutedQuery<Page>(Page);

public class ConveniencePageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<NACSFoundationPageQuery, Page>(tools)
{
    public override async Task<Page> Handle(NACSFoundationPageQuery request, CancellationToken cancellationToken = default)
    {
        var queryBuilder = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

        var webPages = await Executor.GetWebPageResult(queryBuilder, WebPageMapper.Map<Page>, DefaultQueryOptions, cancellationToken);

        return webPages.First();
    }
}
