using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc.Routing;
using NACSShow;
using NACSShow.Features.AttendeeDirectory;

[assembly: RegisterWebPageRoute(
    contentTypeName: AttendeeDirectoryPage.CONTENT_TYPE_NAME,
    controllerType: typeof(AttendeeDirectoryPageController),
    WebsiteChannelNames = new[] { "NACSShow" })]

namespace NACSShow.Features.AttendeeDirectory
{
    public class AttendeeDirectoryPageController : Controller
    {
        // Service for executing content item queries
        private readonly IContentQueryExecutor executor;

        public AttendeeDirectoryPageController(IContentQueryExecutor executor)
        {
            this.executor = executor;
        }

		public async Task<IActionResult> Index()
		{
			var query = new ContentItemQueryBuilder()
								.ForContentType(AttendeeDirectoryPage.CONTENT_TYPE_NAME,
								config => config
									.ForWebsite("NACSShow", PathMatch.Single("/Attendee-Directory")));

			AttendeeDirectoryPage? page = (await executor.GetMappedWebPageResult<AttendeeDirectoryPage>(query)).FirstOrDefault();

			if (page == null)
			{
				return NotFound();
			}

			return View("Views/ContentTypes/AttendeeDirectory.cshtml", page);
		}
    }
}