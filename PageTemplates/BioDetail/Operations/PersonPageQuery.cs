using CMS.ContentEngine;
using CMS.Websites;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Convenience.org.PageTemplates.BioDetail.Operations;

public record PersonPageQuery(RoutedWebPage page) : WebPageRoutedQuery<PersonPage>(page);

public class PersonPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<PersonPageQuery, PersonPage>(tools)
{
    public override async Task<PersonPage> Handle(PersonPageQuery request, CancellationToken cancellationToken)
    {
        var builder = new ContentItemQueryBuilder()
                        .ForWebPage(PersonPage.CONTENT_TYPE_NAME, request.Page.WebPageItemGUID, queryParameters => queryParameters
                        .WithContentTypeFields().WithLinkedItems(1));
        var result = await Executor.GetWebPageResult(builder, WebPageMapper.Map<PersonPage>, DefaultQueryOptions, cancellationToken);

        return result.FirstOrDefault();
    }
}

