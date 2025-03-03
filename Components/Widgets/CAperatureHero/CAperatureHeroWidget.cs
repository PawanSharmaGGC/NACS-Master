using Convenience.org.Components.Widgets.CAperatureHero;
using Convenience.org.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

[assembly: RegisterWidget(
    identifier: CAperatureHeroWidget.IDENTIFIER,
    name: "C Aperature Hero Widget",
    viewComponentType: typeof(CAperatureHeroWidget),
    propertiesType: typeof(CAperatureHeroWidgetProperties),
    Description = "C Aperature Hero Widget",
    IconClass = "icon-cookie",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.CAperatureHero
{
    public class CAperatureHeroWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.CAperatureHero";
        private readonly MediaLibraryHelpers mediaLibraryHelpers;

        public CAperatureHeroWidget(MediaLibraryHelpers mediaLibraryHelpers)
        {
            this.mediaLibraryHelpers = mediaLibraryHelpers;
        }
        public async Task<IViewComponentResult> InvokeAsync(CAperatureHeroWidgetProperties properties)
        {
            string clipImageAltText = string.Empty;
            string videoUrlAltText = string.Empty;
            string mobileImageAltText = string.Empty;

            var viewModel = CAperatureHeroWidgetViewModel.GetViewModel(properties);
            viewModel.ClipImageUrl = mediaLibraryHelpers.GetImagePath(properties.ClipImage.FirstOrDefault(), ref clipImageAltText);
            viewModel.ClipImageAltText = clipImageAltText;
            viewModel.MobileImageUrl = mediaLibraryHelpers.GetImagePath(properties.MobileImage.FirstOrDefault(), ref mobileImageAltText);
            viewModel.MobileImageAltText = mobileImageAltText;
            if (string.IsNullOrEmpty(viewModel.VideoUrl))
            {
                viewModel.VideoUrl = !string.IsNullOrEmpty(properties.VideoUrl) ? properties.VideoUrl : mediaLibraryHelpers.GetImagePath(properties.Video.FirstOrDefault(), ref videoUrlAltText);
                viewModel.VideoUrlAltText = videoUrlAltText;
            }

            return View($"~/Components/Widgets/CAperatureHero/_CAperatureHeroWidget.cshtml", viewModel);
        }

    }
}
