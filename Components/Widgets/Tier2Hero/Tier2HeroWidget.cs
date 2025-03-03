using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Convenience.org.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets.Tier2Hero;

[assembly: RegisterWidget(
    identifier: Tier2HeroWidget.IDENTIFIER,
    name: "Tier 2 Hero Widget",
    viewComponentType: typeof(Tier2HeroWidget),
    propertiesType: typeof(Tier2HeroWidgetProperties),
    Description = "Tier 2 Hero Widget",
    IconClass = "icon-outdent",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Tier2Hero
{
    public class Tier2HeroWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.Tier2Hero";
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;

        public Tier2HeroWidget(MediaLibraryHelpers mediaLibraryHelpers)
        {
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }

        public async Task<IViewComponentResult> InvokeAsync(Tier2HeroWidgetProperties properties)
        {
            Tier2HeroWidgetViewModel viewModel = Tier2HeroWidgetViewModel.GetViewModel(properties);
            string imageAltText = string.Empty;

            viewModel.ImageUrl = _mediaLibraryHelpers.GetImagePath(properties.Image.FirstOrDefault(), ref imageAltText);
            viewModel.ImageAltText = imageAltText;

            return View($"~/Components/Widgets/Tier2Hero/_Tier2HeroWidget.cshtml", viewModel);

        }
    }
}