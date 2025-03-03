using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Convenience.org.Components.Widgets
{
    public class CopyBlockViewComponent : ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<CopyBlockProperties> model)
        {
            string altText = string.Empty;
            var viewModel = new CopyBlockViewModel
            {
                CopyBlockContent = model.Properties.CopyBlockContent
            };
            return View("~/Components/Widgets/CopyBlock/CopyBlock.cshtml", viewModel);
        }
    }
}
