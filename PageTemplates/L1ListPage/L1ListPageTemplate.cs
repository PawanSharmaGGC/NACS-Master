using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[assembly: RegisterPageTemplate("Convenience.PageTemplate.L1ListPageTemplate",
    "L1 List Page Template",
    customViewName: "~/PageTemplates/L1ListPage/_L1ListPage.cshtml",
    Description = "L1 List Page Template")]

namespace Convenience.org.PageTemplates.L1ListPage
{
    public class L1ListPageTemplateController : Controller
    {
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor _executor;
        private readonly IWebsiteChannelContext _channelContext;

        public L1ListPageTemplateController(IWebPageDataContextRetriever _contextRetriever,
            IContentQueryExecutor executor,
            IWebsiteChannelContext channelContext
            )
        {
            contextRetriever = _contextRetriever;
            _channelContext = channelContext;
            _executor = executor;
        }


        public async Task<IActionResult> Index()
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            var webPageGuid = data.WebPage.WebPageItemGUID;

            var pageItembuilder = new ContentItemQueryBuilder()
                .ForContentTypes(parameters =>
                {
                    parameters.ForWebsite(_channelContext.WebsiteChannelName).WithContentTypeFields().WithLinkedItems(1);
                })
                .Parameters(parameter =>
                {
                    parameter.Where(i => i.WhereEquals("WebPageItemGuid", webPageGuid));
                });

            var page = await _executor.GetMappedResult<IContentItemFieldsSource>(pageItembuilder);

            return new TemplateResult(page);
        }
    }
}
