using System.Threading.Tasks;
using Convenience;
using Convenience.org.PageTemplates.L1StatisticsPage;
using Convenience.org.PageTemplates.L1StatisticsPage.Operations;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageTemplate(
    identifier: L1Statistics.CONTENT_TYPE_NAME,
    name: "L1Statistics Page template",
    propertiesType: null,
    customViewName: "~/PageTemplates/L1StatisticsPage/_L1StatisticsPage.cshtml",
    ContentTypeNames = [L1Statistics.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: L1Statistics.CONTENT_TYPE_NAME,
    controllerType: typeof(L1StatisticsPageTemplate))]

namespace Convenience.org.PageTemplates.L1StatisticsPage
{
    public class L1StatisticsPageTemplate : Controller
    {

        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public L1StatisticsPageTemplate(IMediator mediator, IWebPageDataContextRetriever contextRetriever)
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

            var page = await mediator.Send(new L1StatisticsPageQuery(data.WebPage));

            return new TemplateResult(page);
        }

    }
}
