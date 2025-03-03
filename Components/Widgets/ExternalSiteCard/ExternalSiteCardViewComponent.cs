using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Convenience.org.Helpers;
using System.Linq;

namespace Convenience.org.Components.Widgets
{
    public class ExternalSiteCardViewComponent:ViewComponent
    {
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;
        public ExternalSiteCardViewComponent(MediaLibraryHelpers mediaLibraryHelpers)
        {
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<ExternalSiteCardProperties> model)
        {
            string altText = string.Empty;
            var viewModel = new ExternalSiteCardViewModel
            {
                EyebrowTitle = model.Properties.EyebrowTitle,
                CTAText = model.Properties.CTAText,
                CTALink = model.Properties.CTALink,
                IsTagVisible = model.Properties.IsTagVisible,
                Title = model.Properties.Title,
                TagName = model.Properties.TagName,
                Image = _mediaLibraryHelpers.GetImagePath(model.Properties.Image?.FirstOrDefault() ?? null, ref altText),
            };
            return View("~/Components/Widgets/ExternalSiteCard/ExternalSiteCard.cshtml", viewModel);
        }
    }
}
