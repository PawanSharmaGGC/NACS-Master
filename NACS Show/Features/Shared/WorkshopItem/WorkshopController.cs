using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using Microsoft.AspNetCore.Mvc;

using NACSShow;
using NACSShow.Features.Shared.WorkshopItem;

[assembly: RegisterPageTemplate(Workshop.CONTENT_TYPE_NAME, "Workshop", customViewName: "Features/Shared/PageTemplates/_Workshop.cshtml", Description = "Workshop Page", ContentTypeNames = ["Workshop"])]

[assembly: RegisterWebPageRoute(
    contentTypeName: Workshop.CONTENT_TYPE_NAME,
    controllerType: typeof(WorkshopController))]

namespace NACSShow.Features.Shared.WorkshopItem
{
    public class WorkshopController(IContentQueryExecutor executor) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var query = new ContentItemQueryBuilder()
                .ForContentType(Workshop.CONTENT_TYPE_NAME,
                    config => config
                    .WithLinkedItems(3)
                    .ForWebsite("NACSShow", PathMatch.Children("/Sessions/Education-Sessions/")));

            IEnumerable<Workshop> page = await executor.GetMappedResult<Workshop>(query);

            var model = new WorkshopViewModel()
            {
                WorkshopItem = page.FirstOrDefault() ?? new Workshop()
            };

            return View("~/Features/Shared/WorkshopItem/_Workshop.cshtml", model);
        }
    }
}
