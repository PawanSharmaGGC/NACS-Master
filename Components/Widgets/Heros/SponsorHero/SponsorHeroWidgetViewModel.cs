namespace Convenience.org.Components.Widgets.Heros.SponsorHero
{
    public class SponsorHeroWidgetViewModel
    {
        public string EyebrowTitle { get; set; }
        public string DateMonth { get; set; }
        public string DateYear { get; set; }
        public string DateDay { get; set; }
        public string Title { get; set; }
        public string SponsorImageUrl { get; set; }
        public string SponsorImageUrlAlt { get; set; }
        public string VideoUrl { get; set; }
        public string CTAText { get; set; }
        public string CTALink { get; set; }
        public string WebPageGuid { get; set; }
        public bool HideAddToCalendarButton { get; set; }

        public static SponsorHeroWidgetViewModel GetViewModel(SponsorHeroWidgetProperties properties)
        {
            if (properties == null)
            {
                return null;
            }
            else
            {
                var vm = new SponsorHeroWidgetViewModel
                {
                    EyebrowTitle = properties.EyebrowTitle,
                    CTALink = properties.CTALink,
                    CTAText = properties.CTAText,
                    HideAddToCalendarButton = properties.HideAddToCalendarButton,
                };
                return vm;
            }
        }
    }
}
