using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageTemplate("Convenience.PageTemplate.TopicsOverviewPageTemplate",
    "Topics Overview Page Template",
    customViewName: "~/PageTemplates/TopicsOverviewPage/_TopicsOverviewPage.cshtml",
    Description = "Topics Overview Page Template")]

namespace Convenience.org.PageTemplates.TopicsOverviewPage
{
    public class TopicsOverviewPageTemplateController : Controller
    {
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor _executor;
        private readonly IWebsiteChannelContext _channelContext;

        public TopicsOverviewPageTemplateController(IWebPageDataContextRetriever _contextRetriever,
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
