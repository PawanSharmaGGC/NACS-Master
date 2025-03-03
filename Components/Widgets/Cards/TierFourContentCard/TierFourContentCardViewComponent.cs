using Microsoft.AspNetCore.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets.Cards.TierFourContentCard;
using System.Threading.Tasks;
using Convenience.org.Helpers;
using System.Linq;
using CMS.Base;

[assembly: RegisterWidget(identifier: TierFourContentCardViewComponent.IDENTIFIER, name: "Tier 4 Content Card Featured Widget", viewComponentType: typeof(TierFourContentCardViewComponent), propertiesType: typeof(TierFourContentCardProperties), Description = "Tier 4 Content Card Featured Widget", IconClass = "icon-box", AllowCache = true)]

namespace Convenience.org.Components.Widgets.Cards.TierFourContentCard
{
    public class TierFourContentCardViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.TierFourContentCard";
        public async Task<IViewComponentResult> InvokeAsync(TierFourContentCardProperties properties)
        {
            string imageAltText = string.Empty;

            var viewModel = new TierFourContentCardViewModel
            {
                
                Title = properties.Title ?? string.Empty,
                PublishedDate = properties.PublishedDate.ToString("dd MMM yyyy"),
                Caption = properties.Caption ?? string.Empty,
            };

            return View($"~/Components/Widgets/Cards/TierFourContentCard/_TierFourContentCard.cshtml", viewModel);
        }
    }
}
