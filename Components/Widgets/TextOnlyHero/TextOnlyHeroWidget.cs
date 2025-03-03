using System.Threading.Tasks;
using Convenience.org.Components.Widgets.Cards.TierOneContentCardFeatured;
using Convenience.org.Components.Widgets.TextOnlyHero;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(
    identifier: TextOnlyHeroWidget.IDENTIFIER, 
    name: "Text Only Hero Widget", 
    viewComponentType: typeof(TextOnlyHeroWidget), 
    propertiesType: typeof(TextOnlyHeroWidgetProperties), 
    Description = "Text Only Hero Widget", 
    IconClass = "icon-l-text", 
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.TextOnlyHero
{
    public class TextOnlyHeroWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.TextOnlyHero";

        public async Task<IViewComponentResult> InvokeAsync(TextOnlyHeroWidgetProperties properties)
        {
            string imageAltText = string.Empty;

            var viewModel = TextOnlyHeroWidgetViewModel.GetViewModel(properties);

            return View($"~/Components/Widgets/TextOnlyHero/_TextOnlyHeroWidget.cshtml", viewModel);
        }

    }
}
