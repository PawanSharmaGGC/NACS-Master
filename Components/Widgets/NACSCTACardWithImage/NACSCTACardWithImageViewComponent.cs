using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Convenience.org.Helpers;
using System.Linq;

namespace Convenience.org.Components.Widgets
{
    public class NACSCTACardWithImageViewComponent : ViewComponent
    {
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;
        public NACSCTACardWithImageViewComponent(MediaLibraryHelpers mediaLibraryHelpers)
        {
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<NACSCTACardWithImageProperties> model)
        {
            string altText = string.Empty;
            var viewModel = new NACSCTACardWithImageViewModel
            {
                EyebrowTitle = model.Properties.EyebrowTitle,
                CTAText = model.Properties.CTAText,
                CTALink = model.Properties.CTALink,
                Title = model.Properties.Title,
                IsTagVisible = model.Properties.IsTagVisible,
                TagName = model.Properties.TagName,
                Image = _mediaLibraryHelpers.GetImagePath(model.Properties.Image?.FirstOrDefault() ?? null, ref altText),
            };
            return View("~/Components/Widgets/NACSCTACardWithImage/NACSCTACardWithImage.cshtml", viewModel);
        }
    }
}
