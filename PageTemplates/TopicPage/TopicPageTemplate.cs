using System.Threading.Tasks;
using Convenience;
using Convenience.org.PageTemplates.HomePage;
using Convenience.org.PageTemplates.TopicPage;
using Convenience.org.PageTemplates.TopicPage.Operations;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageTemplate(
    identifier: Topic.CONTENT_TYPE_NAME,
    name: "Topic Page template",
    propertiesType: null,
    customViewName: "~/PageTemplates/TopicPage/_TopicPage.cshtml",
    ContentTypeNames = [Topic.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: Topic.CONTENT_TYPE_NAME,
    controllerType: typeof(TopicPageTemplate))]

namespace Convenience.org.PageTemplates.TopicPage
{
    public class TopicPageTemplate : Controller
    {

        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public TopicPageTemplate(IMediator mediator, IWebPageDataContextRetriever contextRetriever)
        {
            this.mediator = mediator;
            this.contextRetriever = contextRetriever;
        }

        public async Task<IActionResult> Index()
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            var page = await mediator.Send(new TopicPageQuery(data.WebPage));

            return new TemplateResult(page);
        }

    }
}
