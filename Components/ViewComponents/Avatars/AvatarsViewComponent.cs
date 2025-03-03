using Convenience.org.Models;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.ViewComponents.Avatars
{
    public class AvatarsViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(AvatarsViewModel model)
        {
            return View("~/Components/ViewComponents/Avatars/Avatars.cshtml", model);
        }
    }
}
