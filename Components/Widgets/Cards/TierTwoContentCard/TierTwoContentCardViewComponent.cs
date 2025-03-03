using Microsoft.AspNetCore.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets.Cards.TierTwoContentCard;
using System.Threading.Tasks;
using Convenience.org.Helpers;
using System.Linq;

[assembly: RegisterWidget(identifier: TierTwoContentCardViewComponent.IDENTIFIER, name: "Tier 2 Content Card Widget", viewComponentType: typeof(TierTwoContentCardViewComponent), propertiesType: typeof(TierTwoContentCardProperties), Description = "Tier 1 Content Card Featured Widget", IconClass = "icon-box", AllowCache = true)]

namespace Convenience.org.Components.Widgets.Cards.TierTwoContentCard
{
    public class TierTwoContentCardViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.TierTwoContentCard";
        private readonly MediaLibraryHelpers mediaLibraryHelpers;

        public TierTwoContentCardViewComponent(MediaLibraryHelpers mediaLibraryHelpers)
        {
            this.mediaLibraryHelpers = mediaLibraryHelpers;
        }
        public async Task<IViewComponentResult> InvokeAsync(TierTwoContentCardProperties properties)
        {
            string imageAltText = string.Empty;

            var viewModel = new TierTwoContentCardViewModel
            {
                Title = properties.Title ?? string.Empty,
                ImagePath = mediaLibraryHelpers.GetImagePath(properties.Image.FirstOrDefault(), ref imageAltText),
                ImageAltText = imageAltText,
                PublishedDate = properties.PublishedDate.ToString("dd MMM yyyy"),
                ReadCaption = properties.ReadCaption ?? string.Empty,
            };

            return View($"~/Components/Widgets/Cards/TierTwoContentCard/_TierTwoContentCard.cshtml", viewModel);
        }
    }
}
