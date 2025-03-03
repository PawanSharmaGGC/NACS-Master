using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

using Microsoft.AspNetCore.Mvc;

using NACSShow;
using NACSShow.Features.SpeakerItem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: RegisterPageTemplate(Speaker.CONTENT_TYPE_NAME, "Speaker", customViewName: "Features/Shared/PageTemplates/_Speaker.cshtml", Description = "Speaker Page", ContentTypeNames = ["Speaker"])]

[assembly: RegisterWebPageRoute(
    contentTypeName: Speaker.CONTENT_TYPE_NAME,
    controllerType: typeof(SpeakerController))]

namespace NACSShow.Features.SpeakerItem
{
    public class SpeakerController(IContentQueryExecutor executor) : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {
            var query = new ContentItemQueryBuilder()
                .ForContentType(Speaker.CONTENT_TYPE_NAME,
                    config => config
                    .WithLinkedItems(3)
                    .ForWebsite("NACSShow", PathMatch.Children("/Speakers/")));
            IEnumerable<Speaker> page = await executor.GetMappedResult<Speaker>(query);
            var model = new SpeakerViewModel()
            {
                SpeakerItem = page.FirstOrDefault() ?? new Speaker()
            };

            return View("~/Features/Shared/SpeakerItem/_Speaker.cshtml", model);
        }
    }
}
