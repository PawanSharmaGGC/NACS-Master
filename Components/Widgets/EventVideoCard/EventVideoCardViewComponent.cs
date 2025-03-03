using System.Threading.Tasks;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Convenience.org.Helpers;
using System.Linq;
namespace Convenience.org.Components.Widgets
{
    public class EventVideoCardViewComponent : ViewComponent
    {
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;
        public EventVideoCardViewComponent(MediaLibraryHelpers mediaLibraryHelpers)
        {
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<EventVideoCardProperties> model)
        {
            string altText = string.Empty;
            var viewModel = new EventVideoCardViewModel
            {
                EyebrowTitle = model.Properties.EyebrowTitle,
                TagName = model.Properties.TagName,
                Title = model.Properties.Title,
                IsOverlayVisible = model.Properties.IsOverlayVisible,
                VideoPoster = _mediaLibraryHelpers.GetImagePath(model.Properties.VideoPoster?.FirstOrDefault() ?? null, ref altText),
                VideoURL = _mediaLibraryHelpers.GetVideoPath(model.Properties.VideoURL?.FirstOrDefault() ?? null),
            };
            return View("~/Components/Widgets/EventVideoCard/EventVideoCard.cshtml", viewModel);
        }
    }
}
