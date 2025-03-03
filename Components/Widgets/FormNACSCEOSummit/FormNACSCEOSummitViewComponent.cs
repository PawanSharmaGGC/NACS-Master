using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Convenience.org.Components.Widgets.FormNACSCEOSummit
{
    public class FormNACSCEOSummitViewComponent : ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var model = new FormNACSCEOSummitViewModel();
            return View("~/Components/Widgets/FormNACSCEOSummit/FormNACSCEOSummit.cshtml", model);
        }
    }
}
