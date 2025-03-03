using Microsoft.AspNetCore.Mvc;

namespace NACSShow.Components.ViewComponents.ShawPageDescriptionText
{
    public class ShawPageDescriptionTextViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string description)
        {
            return View("~/Components/ViewComponents/ShawPageDescriptionText/ShawPageDescriptionText.cshtml", description);
        }
    }
}
