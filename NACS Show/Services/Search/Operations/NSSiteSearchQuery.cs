using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc;

using NACS.Portal.Core.Operations;

namespace NACSShow.Services.Search.Operations
{
    public record NSSiteSearchQuery(RoutedWebPage Page) : WebPageRoutedQuery<NACSShow.Search>(Page);

    public class NSSiteQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<NSSiteSearchQuery, NACSShow.Search>(tools)
    {
        public override async Task<NACSShow.Search> Handle(NSSiteSearchQuery request, CancellationToken cancellationToken = default)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<NACSShow.Search>, DefaultQueryOptions, cancellationToken);

            return r.First();
        }
    }
}
