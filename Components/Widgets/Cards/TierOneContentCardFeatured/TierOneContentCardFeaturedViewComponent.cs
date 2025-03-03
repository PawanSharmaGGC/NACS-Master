using Microsoft.AspNetCore.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets.Cards.TierOneContentCardFeatured;
using System.Threading.Tasks;
using Convenience.org.Helpers;
using System.Linq;

[assembly: RegisterWidget(identifier: TierOneContentCardFeaturedViewComponent.IDENTIFIER, name: "Tier 1 Content Card Featured Widget", viewComponentType: typeof(TierOneContentCardFeaturedViewComponent), propertiesType: typeof(TierOneContentCardFeaturedProperties), Description = "Tier 1 Content Card Featured Widget", IconClass = "icon-box", AllowCache = true)]

namespace Convenience.org.Components.Widgets.Cards.TierOneContentCardFeatured
{
    public class TierOneContentCardFeaturedViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.TierOneContentCardFeatured";
        private readonly MediaLibraryHelpers mediaLibraryHelpers;

        public TierOneContentCardFeaturedViewComponent(MediaLibraryHelpers mediaLibraryHelpers)
        {
            this.mediaLibraryHelpers = mediaLibraryHelpers;
        }
        public async Task<IViewComponentResult> InvokeAsync(TierOneContentCardFeaturedProperties properties)
        {
            string imageAltText = string.Empty;

            var viewModel = new TierOneContentCardFeaturedViewModel
            {
                
                EyebrowTitle = properties.EyebrowTitle ?? string.Empty,
                Title = properties.Title ?? string.Empty,
                Description = properties.Description ?? string.Empty,
                CTALink = properties.CTALink ?? string.Empty,
                CTAText = properties.CTAText ?? string.Empty,
                ImagePath = mediaLibraryHelpers.GetImagePath(properties.Image.FirstOrDefault(), ref imageAltText),
                ImageAltText = imageAltText,
                ImagePosition = properties.ImagePosition ?? string.Empty,
            };

            return View($"~/Components/Widgets/Cards/TierOneContentCardFeatured/_TierOneContentCardFeatured.cshtml", viewModel);
        }
    }
}
