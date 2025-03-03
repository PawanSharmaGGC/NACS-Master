using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACSShow;
using Microsoft.AspNetCore.Mvc;
using NACSShow.PageTemplates.SearchPage;
using MediatR;
using Kentico.Content.Web.Mvc;
using CMS.ContentEngine;
using NACSShow.Features.SearchPage;
using NACSShow.Services.Search.Operations;
using NACSShow.Services.Search.SessionSearch;
using NACSShow.Services.Search.SiteSearch;
using Microsoft.AspNetCore.WebUtilities;

[assembly: RegisterPageTemplate(
    identifier: "NACSShow.SessionSearch",
    name: "Search Results",
    customViewName: "~/Features/SearchPage/NACSShow/_SearchPage.cshtml",
    ContentTypeNames = [SessionSearch.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: SessionSearch.CONTENT_TYPE_NAME,
    controllerType: typeof(NSSearchPageTemplateController))]

namespace NACSShow.PageTemplates.SearchPage
{
    public class NSSearchPageTemplateController(SiteSearchService _searchService) : Controller
    {

        private readonly SiteSearchService searchService = _searchService;

        public IActionResult Index()
        {
           var request = new SiteSearchRequest(HttpContext.Request);
            var objectTypes = new List<object> { new Workshop(), new Page(), new Speaker() };

            var searchResult = searchService.SearchSite(request);

            

            var model = new SearchViewModel(request, searchResult, objectTypes);
            

            return View("~/Features/SearchPage/NACSShow/_SearchPage.cshtml", model);
        }
    }
}
    

