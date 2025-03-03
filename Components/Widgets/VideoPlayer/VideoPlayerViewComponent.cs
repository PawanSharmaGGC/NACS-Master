using System.Threading.Tasks;
using Convenience.org.Models;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Convenience.org.Helpers;
using System.Linq;
namespace Convenience.org.Components.Widgets
{
    public class VideoPlayerViewComponent : ViewComponent
    {
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;
        public VideoPlayerViewComponent(MediaLibraryHelpers mediaLibraryHelpers)
        {
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<VideoPlayerProperties> model)
        {
            string altText = string.Empty;
            var viewModel = new VideoPlayerViewModel
            {
                EyebrowTitle = model.Properties.EyebrowTitle,
                CTAText = model.Properties.CTAText,
                CTALink = model.Properties.CTALink,
                Title = model.Properties.Title,
                IsOverlayVisible = model.Properties.IsOverlayVisible,
                VideoPoster = _mediaLibraryHelpers.GetImagePath(model.Properties.VideoPoster?.FirstOrDefault() ?? null, ref altText),
                VideoURL = _mediaLibraryHelpers.GetVideoPath(model.Properties.VideoURL?.FirstOrDefault() ?? null),
            };
            return View("~/Components/Widgets/VideoPlayer/VideoPlayer.cshtml", viewModel);
        }
    }
}
