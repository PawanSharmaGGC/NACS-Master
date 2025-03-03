using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

using NACS.Portal.Core.Infrastructure.Search;
using NACS.Portal.Core.Components.Pagination;

namespace NACSMagazine.PageTemplates.SearchPage
{
    public class SearchViewModel : IPagedViewModel
    {
        public string? Title { get; set; } = "";
        public IReadOnlyList<Article> Articles { get; set; } = [];
        public string? Query { get; set; } = "";
        public string SortBy { get; set; } = "";
        [HiddenInput]
        public string Type { get; set; } = "";
        [HiddenInput]
        public int Page { get; set; } = 0;
        public List<FacetOption> Types { get; set; } = [];
        public int TotalPages { get; set; } = 0;


        public SearchViewModel() { }

        public Dictionary<string, string?> GetRouteData(int page) =>
            new()
            {
                { "query", Query },
                { "Type", Type },
                { "page", page.ToString() },
                { "sortBy", SortBy }
            };
    }
}
