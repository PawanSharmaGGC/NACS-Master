using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACSMagazine;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine.PageTemplates.SearchPage;
using MediatR;
using Kentico.Content.Web.Mvc;
using CMS.ContentEngine;

[assembly: RegisterPageTemplate(
    identifier: "NACSMagazine.Search",
    name: "Search Results",
    customViewName: "~/PageTemplates/SearchPage/NACSMagazine/_SearchPage.cshtml",
    ContentTypeNames = [Search.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: Search.CONTENT_TYPE_NAME,
    controllerType: typeof(NMSearchPageTemplateController))]

namespace NACSMagazine.PageTemplates.SearchPage
{
    public class NMSearchPageTemplateController: Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor executor;
        private readonly ArticleSearchService searchService;

        public NMSearchPageTemplateController(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever, IContentQueryExecutor _executor, ArticleSearchService _searchService)
        {
            this.mediator = _mediator;
            this.contextRetriever = _contextRetriever;
            this.executor = _executor;
            this.searchService = _searchService;
        }

        public async Task<IActionResult> Index()
        {
            if(!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            var page = await mediator.Send(new SearchQuery(data.WebPage));

            var request = new ArticleSearchRequest(HttpContext.Request);

            var searchResult = searchService.SearchArticle(request);

            var model = new SearchViewModel()
            {
                Title = page.Title,
                Articles = BuildPostPageViewModels(searchResult?.Hits),
                Page = request.PageNumber,
                Query = request.SearchText,
                SortBy = request.SortBy,
                Type = request.Type,
                TotalPages = searchResult?.TotalPages ?? 0
            };

            return View("~/PageTemplates/SearchPage/NACSMagazine/_SearchPage.cshtml", model);
        }

        private static List<Article> BuildPostPageViewModels(IEnumerable<Article>? results)
        {
            if (results is null)
            {
                return [];
            }

            var vms = new List<Article>();

            foreach (var result in results)
            {
                vms.Add(new Article()
                {
                    Title = result.Title,
                    //RollupImage = result.RollupImage,
                    RollupImageURL = result.RollupImageURL,
                    LedeText = result.LedeText,
                    PageContentTeaser = result.PageContentTeaser,
                    IssueDate = result.IssueDate,
                    ParentPageUrl = "~/" + result.ParentPageUrl
                });
            }
            return vms;
        }
    }
}
