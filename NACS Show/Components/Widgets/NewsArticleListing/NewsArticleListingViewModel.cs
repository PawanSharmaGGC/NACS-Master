namespace NACSShow.Components.Widgets
{
    public class NewsArticleListingViewModel
    {
        public string Heading { get; set; } = string.Empty;

        public IEnumerable<NewsArticleViewModel> NewsArticleList { get; set; } = new List<NewsArticleViewModel>();
    }

    public class NewsArticleViewModel
    {
        public string WebPagePath { get; set; } = string.Empty;
        public string RollupImage { get; set; } = string.Empty;
        public string ImageAltText { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;

    }
}
