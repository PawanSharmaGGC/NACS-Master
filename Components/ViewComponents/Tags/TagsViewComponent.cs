using Convenience.org.Models;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.ViewComponents.Tags
{
    public class TagsViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(TagsViewModel model)
        {
            return View("~/Components/ViewComponents/Tags/Tags.cshtml", model);
        }
    }
}
