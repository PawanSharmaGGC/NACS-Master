using System.Threading.Tasks;
using Convenience;
using Convenience.org.PageTemplates.HomePage;
using Convenience.org.PageTemplates.HomePage.Operations;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageTemplate(
    identifier: Home.CONTENT_TYPE_NAME,
    name: "Home Page template",
    propertiesType: null,
    customViewName: "~/PageTemplates/HomePage/_HomePage.cshtml",
    ContentTypeNames = [Home.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: Home.CONTENT_TYPE_NAME,
    controllerType: typeof(HomePageTemplate))]

namespace Convenience.org.PageTemplates.HomePage
{
    public class HomePageTemplate : Controller
    {

        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public HomePageTemplate(IMediator mediator, IWebPageDataContextRetriever contextRetriever)
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

            var page = await mediator.Send(new HomePageQuery(data.WebPage));

            return new TemplateResult(page);
        }

    }
}
