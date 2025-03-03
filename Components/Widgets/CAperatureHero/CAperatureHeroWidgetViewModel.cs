using Convenience.org.Helpers;
using System.Linq;

namespace Convenience.org.Components.Widgets.CAperatureHero
{
    public class CAperatureHeroWidgetViewModel
    {
        public string EyebrowTitle { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string VideoUrl { get; set; }
        public string VideoUrlAltText { get; set; }
        public string ClipImageUrl { get; set; }
        public string ClipImageAltText { get; set; }
        public string MobileImageUrl { get; set; }
        public string MobileImageAltText { get; set; }
        public string Date { get; set; }
        public string LocationOrReadMinutes { get; set; }
        public string CTAText { get; set; }
        public string CTALink { get; set; }

        public static CAperatureHeroWidgetViewModel GetViewModel(CAperatureHeroWidgetProperties properties)
        {
            CAperatureHeroWidgetViewModel cAperatureHero = new CAperatureHeroWidgetViewModel();
            if (properties != null)
            {
                cAperatureHero.EyebrowTitle = properties.EyebrowTitle ?? string.Empty;
                cAperatureHero.Title = properties.Title ?? string.Empty;
                cAperatureHero.Date = properties.DateTime?.ToString("MMM dd yyyy") ?? string.Empty;
                cAperatureHero.CTALink = properties.CTALink ?? string.Empty;
                cAperatureHero.CTAText = properties.CTAText ?? string.Empty;
                cAperatureHero.VideoUrl = properties.VideoUrl ?? string.Empty;
                cAperatureHero.LocationOrReadMinutes = string.IsNullOrEmpty(properties.LocationOrReadMinutes) ? properties.DateTime?.ToString("HH:mm") : properties.LocationOrReadMinutes;
            }

            return cAperatureHero;
        }
    }
}
