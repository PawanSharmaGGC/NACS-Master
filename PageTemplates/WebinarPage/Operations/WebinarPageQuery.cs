using CMS.ContentEngine;
using CMS.Websites;
using System.Threading.Tasks;
using System.Threading;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;
using System.Linq;

namespace Convenience.org.PageTemplates.WebinarPage.Operations
{
    public record WebinarPageQuery(RoutedWebPage page) : WebPageRoutedQuery<Webinar>(page);

    public class WebinarPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<WebinarPageQuery, Webinar>(tools)
    {
        public override async Task<Webinar> Handle(WebinarPageQuery request, CancellationToken cancellationToken)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.page.WebsiteChannelName, request.page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<Webinar>, DefaultQueryOptions, cancellationToken);

            return r.FirstOrDefault();
        }
    }
}
