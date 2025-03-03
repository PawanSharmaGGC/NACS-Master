using MediatR;

using Microsoft.AspNetCore.Mvc;

using NACS.Portal.Core.Infrastructure.Search;
using NACSMagazine.PageTemplates.SearchPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSMagazine.PageTemplates.MagazineArticlePage.Components
{
    public class ArticleSearchViewComponent(IMediator mediator, ArticleSearchService searchService) : ViewComponent
    {
        private readonly IMediator mediator = mediator;
        private readonly ArticleSearchService searchService = searchService;

        public IViewComponentResult Invoke()
        {
            var request = new ArticleSearchRequest(HttpContext.Request);

            //var result = await mediator.Send(new ArticleTaxonomiesQuery());
            //var taxonomies = result.Items;

            var searchResult = searchService.SearchArticle(request);
            //var chosenFacets = request.Type.ToLower().Split(";", StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? [];

            var model = new ArticleListWidgetViewModel()
            {
                Articles = BuildPostPageViewModels(searchResult?.Hits),
                Page = request.PageNumber,
                Query = request.SearchText,
                SortBy = request.SortBy,
                Type = request.Type,
                //Types = [.. taxonomies
                //    .Select(x => new FacetOption()
                //    {
                //        Label = x.DisplayName,
                //        Value = searchResult?.Facets?.FirstOrDefault(y => y.Label.Equals(x.DisplayName, StringComparison.InvariantCultureIgnoreCase))?.Value ?? 0,
                //        IsSelected = chosenFacets.Contains(x.DisplayName, StringComparer.OrdinalIgnoreCase)
                //    })
                //    .Where(x => x.Value != 0)
                //    .OrderBy(f => f.Label)],
                TotalPages = searchResult?.TotalPages ?? 0
            };

            return View("~/PageTemplates/MagazineArticlePage/Components/ArticleSearch.cshtml", model);
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
                    IssueDate = result.IssueDate,
                    ParentPageUrl = "~/" + result.ParentPageUrl,
                    
                    //Taxonomy = result.Taxonomy
                });
            }
            return vms;
        }
    }
}
