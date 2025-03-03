using System.Threading.Tasks;
using Convenience;
using Convenience.org.PageTemplates.FormPage.Operations;
using Convenience.org.PageTemplates.HomePage;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageTemplate(
    identifier: Home.CONTENT_TYPE_NAME,
    name: "Form Page template",
    propertiesType: null,
    customViewName: "~/PageTemplates/FormPage/_FormPage.cshtml",
    ContentTypeNames = [Home.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: Form.CONTENT_TYPE_NAME,
    controllerType: typeof(FormPageTemplate))]

namespace Convenience.org.PageTemplates.HomePage
{
    public class FormPageTemplate : Controller
    {

        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public FormPageTemplate(IMediator mediator, IWebPageDataContextRetriever contextRetriever)
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

            var page = await mediator.Send(new FormPageQuery(data.WebPage));

            return new TemplateResult(page);
        }

    }
}
