using NACS.Portal.Core.Infrastructure.Search;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using NACS.Portal.Core.Components.Pagination;

namespace NACSMagazine.PageTemplates.MagazineArticlePage
{
    public class ArticleListWidgetViewModel : IPagedViewModel
    {
        public string? Title { get; } = "";
        public IReadOnlyList<Article> Articles { get; set; } = [];
        public string? Query { get; set; } = "";
        public string SortBy { get; set; } = "";
        [HiddenInput]
        public string Type { get; set; } = "";
        [HiddenInput]
        public int Page { get; set; } = 0;
        public List<FacetOption> Types { get; set; } = [];
        public int TotalPages { get; set; } = 0;

        public ArticleListWidgetViewModel() { }

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
