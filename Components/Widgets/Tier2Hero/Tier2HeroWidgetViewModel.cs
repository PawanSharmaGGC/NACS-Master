using System.Collections.Generic;
using Convenience.org.Models;

namespace Convenience.org.Components.Widgets.Tier2Hero
{
    public class Tier2HeroWidgetViewModel
    {
        public string EyebrowTitle { get; set; }
        public string Title { get; set; }
        public string DateTime { get; set; }
        public string LocationOrReadMinutes { get; set; }
        public string MobileTitle { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }
        public string? CTAText { get; set; }
        public string? CTAUrl { get; set; }

        public static Tier2HeroWidgetViewModel GetViewModel(Tier2HeroWidgetProperties properties)
        {
            if (properties == null) { return null; }
            else
            {
                return new Tier2HeroWidgetViewModel()
                {
                    CTAText = properties.CTAText,
                    Title = properties.Title,
                    CTAUrl = properties.CTAUrl,
                    DateTime = properties.DateTime?.ToString("MMM dd yyyy"),
                    EyebrowTitle = properties.EyebrowTitle,
                    LocationOrReadMinutes = properties.LocationOrReadMinutes,
                    MobileTitle = properties.MobileTitle
                };
            }
        }
    }
}
