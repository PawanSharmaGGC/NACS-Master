using CMS.ContentEngine;
using CMS.Websites;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;

namespace NACSShow.Components.Widgets.NewsArticleListing;

public record NewsArticlePageQuery(RoutedWebPage Page) : WebPageRoutedQuery<NewsArticle>(Page);

public class NewsArticlePageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<NewsArticlePageQuery, NewsArticle>(tools)
{
    public override async Task<NewsArticle> Handle(NewsArticlePageQuery request, CancellationToken cancellationToken = default)
    {
        var b = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

        var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<NewsArticle>, DefaultQueryOptions, cancellationToken);

        return r.First();
    }
}
