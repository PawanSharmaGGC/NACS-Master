using System.Threading.Tasks;
using Convenience;
using Convenience.org.PageTemplates.ArticlesDetailPage;
using Convenience.org.PageTemplates.ArticlesDetailPage.Operations;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterPageTemplate(
    identifier: "Convenience.Pagetemplates.ArticlesDetailPage",
    name: "Articles Detail Page template",
    propertiesType: null,
    customViewName: "~/PageTemplates/ArticlesDetailPage/_ArticlesDetailPage.cshtml",
    ContentTypeNames = [Article.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: Article.CONTENT_TYPE_NAME,
    controllerType: typeof(ArticlesDetailPageTemplateController))]

namespace Convenience.org.PageTemplates.ArticlesDetailPage
{
    public class ArticlesDetailPageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;

        public ArticlesDetailPageTemplateController(IMediator mediator, IWebPageDataContextRetriever contextRetriever)
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

            var page = await mediator.Send(new ArticlesDetailPageQuery(data.WebPage));
            
            return new TemplateResult(page);
        }

    }
}
