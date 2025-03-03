using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Convenience.org.Components.Widgets.FormSignUp
{
    public class FormSignUpViewComponent : ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var model = new FormSignUpViewModel();
            return View("~/Components/Widgets/FormSignUp/FormSignUp.cshtml", model);
        }
    }
}
