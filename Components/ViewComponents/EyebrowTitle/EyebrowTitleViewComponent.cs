using Convenience.org.Models;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.ViewComponents.EyebrowTitle
{
    public class EyebrowTitleViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EyebrowTitleViewModel model)
        {
            return View("~/Components/ViewComponents/EyebrowTitle/EyebrowTitle.cshtml", model);
        }
    }
}
