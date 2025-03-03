using Convenience.org.Models;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.ViewComponents.CopyBlock
{
    public class CopyBlockViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CopyBlockViewModel model)
        {
            return View("~/Components/ViewComponents/CopyBlock/CopyBlock.cshtml", model);
        }
    }
}
