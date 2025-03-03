using CMS.ContentEngine;
using System.Threading.Tasks;
using System.Threading;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Operations;
using CMS.Websites;
using System.Linq;
using Convenience.org.PageTemplates.FormPage.Operations;

namespace Convenience.org.PageTemplates.FormPage.Operations
{
    public record FormPageQuery(RoutedWebPage page) : WebPageRoutedQuery<Form>(page);

    public class CategoryPageQueryHandler(WebPageQueryTools tools) : WebPageQueryHandler<FormPageQuery, Form>(tools)
    {
        public override async Task<Form> Handle(FormPageQuery request, CancellationToken cancellationToken)
        {
            var b = new ContentItemQueryBuilder().ForWebPage(request.page.WebsiteChannelName, request.Page);

            var r = await Executor.GetWebPageResult(b, WebPageMapper.Map<Form>, DefaultQueryOptions, cancellationToken);

            return r.FirstOrDefault();
        }
    }

}
