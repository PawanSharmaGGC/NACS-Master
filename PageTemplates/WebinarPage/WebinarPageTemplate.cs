using Convenience;
using Convenience.org.PageTemplates.WebinarPage;
using Convenience.org.PageTemplates.WebinarPage.Operations;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine;
using System.Threading.Tasks;

[assembly: RegisterWebPageRoute(
    contentTypeName: Webinar.CONTENT_TYPE_NAME,
    controllerType: typeof(WebinarPageTemplateController))]

[assembly: RegisterPageTemplate(
    identifier: "Convenience.Pagetemplates.WebinarPage",
    name: "Webinar Page template",
    customViewName: "~/PageTemplates/WebinarPage/_WebinarPage.cshtml",
    ContentTypeNames = [Webinar.CONTENT_TYPE_NAME]
    )]

namespace Convenience.org.PageTemplates.WebinarPage
{
    public class WebinarPageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public WebinarPageTemplateController(IMediator mediator, IWebPageDataContextRetriever contextRetriever)
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

            var page = await mediator.Send(new WebinarPageQuery(data.WebPage));

           return new TemplateResult(page);
        }

    }
}
