using CMS.ContentEngine;
using System.Threading.Tasks;
using System.Threading;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;
using CMS.Websites;
using System.Linq;

namespace Convenience.org.PageTemplates.HomePage.Operations
{
    public record HomePageQuery(RoutedWebPage page) : WebPageRoutedQuery<Home>(page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<HomePageQuery, Home>(tools)
    {
        public override async Task<Home> Handle(HomePageQuery request, CancellationToken cancellationToken)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.page.WebsiteChannelName,request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<Home>, DefaultQueryOptions, cancellationToken);

            return r.FirstOrDefault();
        }
    }

}
