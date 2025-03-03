namespace NACSMagazine.Features.Home
{
    public class HomePageViewModel
    {
        public string Title { get; set; }
        public IEnumerable<Article>? HomeHero { get; set; }
        public IEnumerable<Article>? HeroFeaturedArticles { get; set; }
        public IEnumerable<Article>? EditorPicksArticles { get; set; }
        public IEnumerable<Article>? DepartmentArticles { get; set; }
		//This should be set to Convenience.NewsArticle page type which only exists on Convenience.org site (but that site hasn't been built as of writing this). We will need to change this for the functionality to work as expected.
		//public IEnumerable<Article> NACSDailyArticles { get; set; }
        

        public HomePageViewModel(NACSMagazine.Home home)
        {
            Title = home.Title;
        }
    }
}
