using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[assembly: RegisterPageTemplate("Convenience.PageTemplate.DirectoryPageTemplate",
    "Directory Page Template",
    customViewName: "~/PageTemplates/DirectoryPage/_DirectoryPage.cshtml",
    Description = "Directory Page Template")]

namespace Convenience.org.PageTemplates.DirectoryPage
{
    public class DirectoryPageTemplateController : Controller
    {
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor _executor;
        private readonly IWebsiteChannelContext _channelContext;

        public DirectoryPageTemplateController(IWebPageDataContextRetriever _contextRetriever,
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
