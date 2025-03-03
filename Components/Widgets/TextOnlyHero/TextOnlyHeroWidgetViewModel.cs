namespace Convenience.org.Components.Widgets.TextOnlyHero
{
    public class TextOnlyHeroWidgetViewModel
    {
        public string EyebrowTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string LocationOrReadTime { get; set; }
        public string TimeWithZone { get; set; }
        public string CTAText { get; set; } = string.Empty;
        public string CTALink { get; set; } = string.Empty;


        public static TextOnlyHeroWidgetViewModel GetViewModel(TextOnlyHeroWidgetProperties properties)
        {

            var viewModel = properties==null ?new TextOnlyHeroWidgetViewModel(): new TextOnlyHeroWidgetViewModel
            {

                EyebrowTitle = properties.EyebrowTitle ?? string.Empty,
                Title = properties.Title ?? string.Empty,
                Description = properties.Description ?? string.Empty,
                Date = properties.Date?.ToString("dd MMM yyyy"),
                LocationOrReadTime = properties.LocationOrReadTime,
                TimeWithZone = properties.Date?.ToString("HH:mm"),
                CTALink = properties.CTALink,
                CTAText = properties.CTAText
            };

            return viewModel;
        }
    }
}
