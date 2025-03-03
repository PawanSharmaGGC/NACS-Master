using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Components.Pagination;

using NACSShow.Services.Search.SessionSearch;

using SessionSearchResultViewModel = NACSShow.Components.ViewComponents.SessionSearch.SessionSearchResultsViewComponent.SessionSearchResultViewModel;


namespace NACSShow.Components.ViewComponents.SessionSearch
{
    public class SessionSearchViewModel : IPagedViewModel
    {
        public string? Title { get; set; } = "";
        public IReadOnlyList<SessionSearchResultViewModel> Sessions { get; set; } = [];
        //public IReadOnlyList<DocumentSearchResultViewModel> Documents { get; set; } = [];
        //public IReadOnlyList<SpeakerSearchResultViewModel> Speakers { get; set; } = [];
        public string DayFilter { get; set; } = string.Empty;
        public string KeywordFilter { get; set; } = string.Empty;
        public string? Query { get; set; } = "";
        public IQueryCollection? QueryStringValues { get; set; }
        //public string SortBy { get; set; } = "";
        //[HiddenInput]
        //public string Type { get; set; } = "";
        [HiddenInput]
        public int Page { get; set; } = 0;
        //public List<FacetOption> Types { get; set; } = [];
        public int TotalPages { get; set; } = 0;


        public SessionSearchViewModel() { }

        public Dictionary<string, string?> GetRouteData(int page) =>
            new()
            {
                //{ "day", DayFilter },
                //{ "track", KeywordFilter },
                { "query", Query },
                //{ "Type", Type },
                { "page", page.ToString() },
                //{ "sortBy", SortBy }
            };

        //public SessionSearchViewModel(SessionSearchRequest request, SessionSearchResultsViewModel searchResult)
        //{
        //    Sessions = searchResult.Hits.Select(result => new SessionSearchResultViewModel(result)).ToList();
        //    //Documents = searchResult.Hits.Select(result => new DocumentSearchResultViewModel(result)).ToList();
        //    //Speakers = searchResult.Hits.Select(result => new SpeakerSearchResultViewModel(result)).ToList();
        //    Page = request.PageNumber;
        //    //DayFilter = request.DayFilter;
        //    //KeywordFilter = request.KeywordFilter;
        //    Query = request.SearchText;
        //    TotalPages = searchResult?.TotalPages ?? 0;
        //}
    }
}
