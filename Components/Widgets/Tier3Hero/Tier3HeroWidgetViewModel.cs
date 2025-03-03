namespace Convenience.org.Components.Widgets.Tier3Hero
{
    public class Tier3HeroWidgetViewModel
    {
        public string EyebrowTitle { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string LocationOrWatchTime { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Date { get; set; }
        public string ImageAltText { get; set; } = string.Empty;
        public string CTAText { get; set; } = string.Empty;
        public string CTALink { get; set; } = string.Empty;

        public static Tier3HeroWidgetViewModel GetViewModel(Tier3HeroWidgetProperties properties)
        {
            if (properties != null)
            {
                return new Tier3HeroWidgetViewModel()
                {
                    Title = properties.Title,
                    CTALink = properties.CTALink,
                    CTAText = properties.CTAText,
                    EyebrowTitle = properties.EyebrowTitle,
                    LocationOrWatchTime = !string.IsNullOrEmpty(properties.LocationOrWatchTime) ? properties.LocationOrWatchTime : properties.DateTime?.ToString("mm:ss zz"),
                    Date = properties.DateTime?.ToString("MMM dd yyyy"),
                };
            }
            else
            {
                return null;
            }
        }
    }
}
