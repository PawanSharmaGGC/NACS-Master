using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;

namespace Convenience.org.PageTemplates.ArticlesDetailPage.Operations
{
    public record ArticlesDetailPageQuery(RoutedWebPage page) : WebPageRoutedQuery<Article>(page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<ArticlesDetailPageQuery, Article>(tools)
    {
        public override async Task<Article> Handle(ArticlesDetailPageQuery request, CancellationToken cancellationToken)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.page.WebsiteChannelName, request.page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<Article>, DefaultQueryOptions, cancellationToken);

            return r.FirstOrDefault();
        }
    }
}
