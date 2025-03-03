using CMS.ContentEngine;

using Kentico.Content.Web.Mvc;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Components.Pagination;

using NACSShow.Services.Search.SessionSearch;

namespace NACSShow.Components.ViewComponents.SessionSearch
{
    public class SessionSearchResultsViewComponent(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever, IContentQueryExecutor _executor, SessionSearchService _searchService) : ViewComponent
    {
        private readonly IMediator mediator = _mediator;
        private readonly IWebPageDataContextRetriever contextRetriever = _contextRetriever;
        private readonly IContentQueryExecutor executor = _executor;
        private readonly SessionSearchService searchService = _searchService;

        public IViewComponentResult Invoke()
        {
            //if (!contextRetriever.TryRetrieve(out var data))
            //{
            //    return Content("");
            //}

            //var page = await mediator.Send(new SearchQuery(data.WebPage));

            var request = new SessionSearchRequest(HttpContext.Request);

            var searchResult = searchService.SearchSessions(request);

            var model = new SessionSearchViewModel(request, searchResult);

            return View("~/Components/ViewComponents/SessionSearch/_SearchResults.cshtml", model);
        }

        public class SessionSearchViewModel : IPagedViewModel
        {
            public string? Title { get; } = "";
            public IReadOnlyList<SessionSearchResultViewModel> Sessions { get; } = [];
            public List<Page> Documents { get; } = [];
            public List<Speaker> Speakers { get; } = [];
            public string DayFilter { get; } = string.Empty;
            public string KeywordFilter { get; } = string.Empty;
            public string? Query { get; } = "";
            public IQueryCollection? QueryStringValues { get; set; }
            //public string SortBy { get; set; } = "";
            //[HiddenInput]
            //public string Type { get; set; } = "";
            [HiddenInput]
            public int Page { get; set; } = 0;
            //public List<FacetOption> Types { get; set; } = [];
            public int TotalPages { get; set; } = 0;

            public Dictionary<string, string?> GetRouteData(int page) =>
                new()
                {
                { "day", DayFilter },
                { "track", KeywordFilter },
                { "query", Query },
                //{ "Type", Type },
                { "page", page.ToString() },
                //{ "sortBy", SortBy }
                };

            public SessionSearchViewModel(SessionSearchRequest request, SessionSearchResultsViewModel result)
            {
                Sessions = result.Hits.Select(result => new SessionSearchResultViewModel(result)).ToList();
                Page = request.PageNumber;
                DayFilter = request.DayFilter;
                KeywordFilter = request.KeywordFilter;
                Query = request.SearchText;
                TotalPages = result?.TotalPages ?? 0;
            }
        }

        public class SessionSearchResultViewModel
        {
            public string? Title { get; } = "";
            public string StartTime { get; }
            public string EndTime { get; }
            public string? PageContent { get; }
            public string? Keyword { get; }
            public string? Format { get; }
            public string? Translation { get; }


            public SessionSearchResultViewModel(SessionSearchIndexModel model)
            {
                Title = model.Title;
                StartTime = model.StartTime;
                EndTime = model.EndTime;
                PageContent = model.PageContent;
                Keyword = model.Keyword;
                Format = model.Format;
                Translation = model.Translation;
            }
        }

        //private static List<Workshop> BuildPostPageViewModels(IEnumerable<Workshop>? results)
        //{
        //    if (results is null)
        //    {
        //        return [];
        //    }

        //    var vms = new List<Workshop>();

        //    foreach (var result in results)
        //    {
        //        vms.Add(new Workshop()
        //        {
        //            Title = result.Title,
        //            StartTime = result.StartTime,
        //            EndTime = result.EndTime,
        //            PageContent = result.PageContent,
        //            Keyword = result.Keyword,
        //            Format = result.Format,
        //            Translation = result.Translation,
        //        });
        //    }
        //    return vms;
        //}
    }
}
