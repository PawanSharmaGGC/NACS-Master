using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Components.Pagination;
using NACS.Portal.Core.Infrastructure.Search;

using NACSShow.Services.Search.SiteSearch;

namespace NACSShow.Features.SearchPage
{
    public class SearchViewModel : IPagedViewModel
    {
        public IReadOnlyList<SiteSearchResultViewModel> SearchResults { get;} = [];
        public string? Query { get; set; } = "";
        public string SortBy { get; } = "";
        [HiddenInput]
        public int Page { get; set; } = 0;
        public IReadOnlyList<FacetOption> Types { get; }
        public int TypesSelected { get; }
        public int TotalAppliedFilters { get; }
        public int TotalPages { get; set; } = 0;


       public Dictionary<string, string?> GetRouteData(int page)
        {
            var routeData = new Dictionary<string, string?>
            {
                ["page"] = page.ToString()
            };

            if(!string.IsNullOrWhiteSpace(Query))
            {
                routeData["query"] = Query; 
            };

            if(!string.IsNullOrWhiteSpace(SortBy))
            {
                routeData["sortBy"] = SortBy;
            };

            if (Types.Any(t => t.IsSelected)) 
            {
            routeData["types"] = string.Join("&types=", Types.Where(t => t.IsSelected).Select(t => t.Value)).Trim('&');
            };

            return routeData;                
        }

        public SearchViewModel(SiteSearchRequest request, SiteSearchResults searchResult, List<object> objectTypes)
        {
            SearchResults = searchResult
                            .Hits
                            .Select(searchResult => new SiteSearchResultViewModel(searchResult))
                            .ToList();
            Page = request.PageNumber;
            Query = request.SearchText;
            //SortBy = request.SortBy;
            Types = [.. objectTypes
                    .Select(x => new FacetOption()
                    {
                        Label = x.GetType().Name.ToString(), 
                        Value = x.GetType().Name.ToString(),
                        Count = (int)Math.Round(searchResult
                            .Types
                            .FirstOrDefault(y => y.Label.Equals(x.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            ?.Value ?? 0),
                        IsSelected = request
                                    .Types
                                    .Contains(x.ToString(), StringComparer.OrdinalIgnoreCase)
                    })
                    .OrderBy(f => f.Label)];
            TypesSelected = Types.Count(t => t.IsSelected);
            TotalAppliedFilters = Types.Count(t => t.IsSelected);
            //QueryStringValues = qs;
            TotalPages = searchResult?.TotalPages ?? 0;
        }

        public class SiteSearchResultViewModel(SiteSearchIndexModel model)
        {
            public string Title { get; } = model.Title ?? "";
            public string LinkPath { get; } = model.Url ?? "";
            public string Types { get; } = model.Types ?? "";
            public string Content { get; } = model.Content ?? "";
            public DateTime Date { get; } = model.PublishedDate;
            //TODO: Image will go here too if Speaker type
            public string Image { get; } = model.AuthorImage ?? "";
        }
    }
}
