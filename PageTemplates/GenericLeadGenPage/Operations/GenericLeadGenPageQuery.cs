using CMS.ContentEngine;
using System.Threading.Tasks;
using System.Threading;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;
using CMS.Websites;
using System.Linq;

namespace Convenience.org.PageTemplates.GenericLeadGenPage.Operations
{
    public record GenericLeadGenPageQuery(RoutedWebPage page) : WebPageRoutedQuery<GenericLeadGen>(page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<GenericLeadGenPageQuery, GenericLeadGen>(tools)
    {
        public override async Task<GenericLeadGen> Handle(GenericLeadGenPageQuery request, CancellationToken cancellationToken)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<GenericLeadGen>, DefaultQueryOptions, cancellationToken);

            return r.FirstOrDefault();
        }
    }

}
