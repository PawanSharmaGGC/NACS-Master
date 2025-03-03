using CMS.ContentEngine;
using CMS.Websites;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;

namespace ConvenienceCares.Operations;

public record FilePageQuery(RoutedWebPage FilePage) : WebPageRoutedQuery<ConvenienceCare.File>(FilePage);

public class ConvenienceCareFileQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<FilePageQuery, ConvenienceCare.File>(tools)
{
    public override async Task<ConvenienceCare.File> Handle(FilePageQuery request, CancellationToken cancellationToken = default)
    {
        var queryBuilder = new ContentItemQueryBuilder().ForWebPage(request.Page.WebsiteChannelName, request.Page);

        var webPages = await Executor.GetWebPageResult(queryBuilder, WebPageMapper.Map<ConvenienceCare.File>, DefaultQueryOptions, cancellationToken);

        return webPages.First();
    }
}
