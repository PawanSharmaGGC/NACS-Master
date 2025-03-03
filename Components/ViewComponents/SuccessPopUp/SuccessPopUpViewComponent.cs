using Convenience.org.Models;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.ViewComponents.SuccessPopUp
{
    public class SuccessPopUpViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(SuccessPopUpViewModel model)
        {
            return View("~/Components/ViewComponents/SuccessPopUp/SuccessPopUp.cshtml", model);
        }
    }
}