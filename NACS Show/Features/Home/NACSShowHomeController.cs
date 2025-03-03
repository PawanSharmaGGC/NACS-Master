using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

using NACSShow.Features.Home;


[assembly: RegisterWebPageRoute(
    contentTypeName: NACSShow.Home.CONTENT_TYPE_NAME,
    controllerType: typeof(NACSShowHomeController),
    ActionName = "Index",
    Path = "/Home",
    WebsiteChannelNames = ["NACSShow"])]

namespace NACSShow.Features.Home
{
    public class NACSShowHomeController : Controller
    {
        // Service for executing content item queries
        private readonly IContentQueryExecutor executor;
        public NACSShowHomeController(IContentQueryExecutor executor)
        {
            this.executor = executor;
        }

        public async Task<IActionResult> Index()
        {
            var query = new ContentItemQueryBuilder()
                                // Scopes the query to pages of the MEDLAB.Home content type
                                .ForContentType(NACSShow.Home.CONTENT_TYPE_NAME,
                                config => config
                                    // Retrieves the page with the /Home tree path under the Refined Element website channel
                                    .ForWebsite("NACSShow", PathMatch.Single("/Home")));

            // Executes the query and stores the data in the generated 'Home' class
            NACSShow.Home? page = (await executor.GetMappedWebPageResult<NACSShow.Home>(query)).FirstOrDefault();

            // Passes the home page content to the view using HomePageViewModel

			return View("Features/Home/NACSShow/Home.cshtml", new HomePageViewModel(page!));
        }
    }
}