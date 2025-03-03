using System.Threading.Tasks;
using Convenience.org.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets.Tier3Hero;

[assembly: RegisterWidget(
    identifier: Tier3HeroWidget.IDENTIFIER, 
    name: "Tier 3 Hero Widget", 
    viewComponentType: typeof(Tier3HeroWidget), 
    propertiesType: typeof(Tier3HeroWidgetProperties), 
    Description = "Tier 3 Hero Widget", 
    IconClass = "icon-box", 
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Tier3Hero
{
    public class Tier3HeroWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.Tier3Hero";
        private readonly MediaLibraryHelpers mediaLibraryHelpers;
        public Tier3HeroWidget(MediaLibraryHelpers mediaLibraryHelpers)
        {
            this.mediaLibraryHelpers = mediaLibraryHelpers;
        }
        public async Task<IViewComponentResult> InvokeAsync(Tier3HeroWidgetProperties properties)
        {
            string imageAltText = string.Empty;

            var viewModel = Tier3HeroWidgetViewModel.GetViewModel(properties);

            viewModel.ImageUrl = mediaLibraryHelpers.GetImagePath(properties.Image.FirstOrDefault(), ref imageAltText);
            viewModel.ImageAltText = imageAltText;

            return View($"~/Components/Widgets/Tier3Hero/_Tier3Hero.cshtml", viewModel);
        }
    }
}
