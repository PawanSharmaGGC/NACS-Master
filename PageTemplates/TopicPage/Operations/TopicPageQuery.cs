using CMS.ContentEngine;
using System.Threading.Tasks;
using System.Threading;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;
using CMS.Websites;
using System.Linq;

namespace Convenience.org.PageTemplates.TopicPage.Operations
{
    public record TopicPageQuery(RoutedWebPage page) : WebPageRoutedQuery<Topic>(page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<TopicPageQuery, Topic>(tools)
    {
        public override async Task<Topic> Handle(TopicPageQuery request, CancellationToken cancellationToken)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<Topic>, DefaultQueryOptions, cancellationToken);

            return r.FirstOrDefault();
        }
    }

}
