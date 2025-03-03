using System.Collections.Generic;
using System.Linq;
using Convenience.org.Models;

namespace Convenience.org.Components.Widgets.Heros.Tier1GlassSuperHeroCard
{
    public class Tier1GlassSuperHeroCardWidgetViewModel
    {
        public string EyebrowTitle { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string DateTime { get; set; } = string.Empty;
        public string ReadTimeOrLocation { get; set; } = string.Empty;
        public string CTAText { get; set; } = string.Empty;
        public string CTALink { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageAltText { get; set; } = string.Empty;
        public string ArticleItemdata { get; set; } = string.Empty;
        public List<ArticleCardItem> ArticleCardItem { get; set; }

        public static Tier1GlassSuperHeroCardWidgetViewModel GetViewModel(Tier1GlassSuperHeroCardWidgetProperties properties)
        {
            var vm = properties == null ? new Tier1GlassSuperHeroCardWidgetViewModel() : new Tier1GlassSuperHeroCardWidgetViewModel()
            {
                CTALink = properties.CTALink,
                CTAText = properties.CTAText,
                DateTime = properties.DateTime != null ? properties.DateTime?.ToString("dd MMM yyyy") : "",
                EyebrowTitle = properties.EyebrowTitle,
                ReadTimeOrLocation = properties.ReadTimeOrLocation,
                Title = properties.Title,
            };

            return vm;
        }
    }
}
