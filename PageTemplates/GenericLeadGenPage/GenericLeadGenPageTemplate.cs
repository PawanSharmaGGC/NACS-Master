using System.Threading.Tasks;
using Convenience;
using Convenience.org.PageTemplates.GenericLeadGenPage;
using Convenience.org.PageTemplates.GenericLeadGenPage.Operations;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageTemplate(
    identifier: L1Statistics.CONTENT_TYPE_NAME,
    name: "L1Statistics Page template",
    propertiesType: null,
    customViewName: "~/PageTemplates/GenericLeadGenPage/_GenericLeadGenPage.cshtml",
    ContentTypeNames = [GenericLeadGen.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: GenericLeadGen.CONTENT_TYPE_NAME,
    controllerType: typeof(GenericLeadGenPageTemplate))]

namespace Convenience.org.PageTemplates.GenericLeadGenPage
{
    public class GenericLeadGenPageTemplate : Controller
    {

        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public GenericLeadGenPageTemplate(IMediator mediator, IWebPageDataContextRetriever contextRetriever)
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

            var page = await mediator.Send(new GenericLeadGenPageQuery(data.WebPage));

            return new TemplateResult(page);
        }

    }
}
