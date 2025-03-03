using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Threading.Tasks;

namespace Convenience.org.Components.Widgets
{
    public class NacsCtaCardNoImageViewComponent:ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<NacsCtaCardNoImageProperties> model)
        {
            var viewModel = new NacsCtaCardNoImageViewModel
            {
                EyebrowTitle = model.Properties.EyebrowTitle,
                CTAText = model.Properties.CTAText,
                CTALink = model.Properties.CTALink,
                Title = model.Properties.Title
            };
            return View("~/Components/Widgets/NACSCTACardNoImage/NACSCTACardNoImage.cshtml", viewModel);
        }
    }
}
