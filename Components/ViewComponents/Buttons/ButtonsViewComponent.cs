using Microsoft.AspNetCore.Mvc;
using Convenience.org.Models;
namespace Convenience.org.Components.ViewComponents.Buttons
{
    public class ButtonsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ButtonsViewModel model)
        {
            return View("~/Components/ViewComponents/Buttons/Buttons.cshtml", model);
        }
    }
}
