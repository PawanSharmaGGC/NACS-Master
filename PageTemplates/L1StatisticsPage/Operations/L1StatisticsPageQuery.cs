using CMS.ContentEngine;
using System.Threading.Tasks;
using System.Threading;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;
using CMS.Websites;
using System.Linq;

namespace Convenience.org.PageTemplates.L1StatisticsPage.Operations
{
    public record L1StatisticsPageQuery(RoutedWebPage page) : WebPageRoutedQuery<L1Statistics>(page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<L1StatisticsPageQuery, L1Statistics>(tools)
    {
        public override async Task<L1Statistics> Handle(L1StatisticsPageQuery request, CancellationToken cancellationToken)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<L1Statistics>, DefaultQueryOptions, cancellationToken);

            return r.FirstOrDefault();
        }
    }

}
