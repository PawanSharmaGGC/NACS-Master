using System.Collections.Generic;
using Convenience.org.Components.Widgets.Heros.Tier1GlassSuperHeroCard;
using Convenience.org.Models;

namespace Convenience.org.Components.Widgets.Heros.Tier1SuperHero
{
    public class Tier1SuperHeroWidgetViewModel
    {
        public string EyebrowTitle { get; set; } = string.Empty;
        public string CardEyebrowTitle { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string DateTime { get; set; } = string.Empty;
        public string ReadTimeOrLocation { get; set; } = string.Empty;
        public string CTAText { get; set; } = string.Empty;
        public string CTALink { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageAltText { get; set; } = string.Empty;

        public List<CardItemViewModel> ArticleCards { get; set; }

        public static Tier1SuperHeroWidgetViewModel GetViewModel(Tier1SuperHeroWidgetProperties properties)
        {
            var vm = properties == null ? new Tier1SuperHeroWidgetViewModel() : new Tier1SuperHeroWidgetViewModel()
            {
                CTALink = properties.CTALink,
                CTAText = properties.CTAText,
                DateTime = properties.DateTime != null ? properties.DateTime?.ToString("dd MMM yyyy") : "",
                EyebrowTitle = properties.EyebrowTitle,
                CardEyebrowTitle = properties.CardEyebrowTitle,
                ReadTimeOrLocation = properties.ReadTimeOrLocation,
                Title = properties.Title,
            };

            return vm;
        }
    }

    public class CardItemViewModel
    {
        public string EyebrowTitle { get; set; }
        public string EyebrowStatus { get; set; }
        public string Title { get; set; }
        public string PageUrl { get; set; }
    }
}
