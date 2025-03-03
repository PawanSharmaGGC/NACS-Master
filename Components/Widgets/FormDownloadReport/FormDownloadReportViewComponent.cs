using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;

namespace Convenience.org.Components.Widgets.FormDownloadReport
{
    public class FormDownloadReportViewComponent : ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var model = new FormDownloadReportViewModel();
            return View("~/Components/Widgets/FormDownloadReport/FormDownloadReport.cshtml", model);
        }
    }
}
