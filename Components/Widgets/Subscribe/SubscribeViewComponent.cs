using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Convenience.org.Components.Widgets.Subscribe
{
    public class SubscribeViewComponent: ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var model = new SubscribeViewModel();
            return View("~/Components/Widgets/Subscribe/Subscribe.cshtml", model);
        }
    }
}
