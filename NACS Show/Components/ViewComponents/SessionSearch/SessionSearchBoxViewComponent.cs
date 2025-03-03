using CMS.ContentEngine;

using Kentico.Content.Web.Mvc;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Components.Pagination;
using NACSShow.Services.Search.SessionSearch;

namespace NACSShow.Components.ViewComponents.SessionSearch
{
    public class SessionSearchBoxViewComponent(IMediator mediator, IWebPageDataContextRetriever contextRetriever, IContentQueryExecutor executor, SessionSearchService searchService) : ViewComponent
    {
        private readonly IMediator mediator = mediator;
        private readonly IWebPageDataContextRetriever contextRetriever = contextRetriever;
        private readonly IContentQueryExecutor executor = executor;
        private readonly SessionSearchService searchService = searchService;

        public IViewComponentResult Invoke()
        {
            //if (!contextRetriever.TryRetrieve(out var data))
            //{
            //    return Content("");
            //}

            var request = new SessionSearchRequest(HttpContext.Request);
            var qs = HttpContext.Request.Query;
            var searchResult = searchService.SearchSessions(request);
            var model = new SessionSearchViewModel(request, qs, searchResult);

            return View("~/Components/ViewComponents/SessionSearch/_SearchBox.cshtml", model);
        }

        public class SessionSearchViewModel : IPagedViewModel
        {
            public string? Title { get; } = "";
            public IReadOnlyList<WorkshopSearchResultViewModel> Sessions { get; } = [];
            public List<Page> Documents { get; } = [];
            public List<Speaker> Speakers { get; } = [];
            public string DayFilter { get; } = string.Empty;
            public string KeywordFilter { get; } = string.Empty;
            public string? Query { get; } = "";
            public IQueryCollection? QueryStringValues { get; set; }
            [HiddenInput]
            public int Page { get; set; } = 0;
            public int TotalPages { get; set; } = 0;

            public Dictionary<string, string?> GetRouteData(int page) =>
                new()
                {
                        { "day", DayFilter },
                        { "track", KeywordFilter },
                        { "query", Query },
                        { "page", page.ToString() },
                };

            public SessionSearchViewModel(SessionSearchRequest request, IQueryCollection qs, SessionSearchResultsViewModel result)
            {
                Sessions = result.Hits.Select(result => new WorkshopSearchResultViewModel(result)).ToList();
                Page = request.PageNumber;
                DayFilter = request.DayFilter;
                KeywordFilter = request.KeywordFilter;
                Query = request.SearchText;
                QueryStringValues = qs;
                TotalPages = result?.TotalPages ?? 0;
            }
        }

        public class WorkshopSearchResultViewModel
        {
            public string? Title { get; } = "";
            public string StartTime { get; }
            public string EndTime { get; }
            public string? PageContent { get; }
            public string? Keyword { get; }
            public string? Format { get; }
            public string? Translation { get; }
            public string Segment { get; }

            public WorkshopSearchResultViewModel(SessionSearchIndexModel model)
            {
                Title = model.Title;
                StartTime = model.StartTime;
                EndTime = model.EndTime;
                PageContent = model.PageContent;
                Keyword = model.Keyword;
                Format = model.Format;
                Translation = model.Translation;
                Segment = model.Segment;
            }
        }
    }
}
