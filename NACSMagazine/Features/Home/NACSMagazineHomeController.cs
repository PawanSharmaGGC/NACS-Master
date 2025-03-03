using CMS.ContentEngine;
using CMS.Websites;

using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

using NACSMagazine;
using NACSMagazine.Features.Home;

using System.Data;

using Tag = CMS.ContentEngine.Tag;


[assembly: RegisterWebPageRoute(
    contentTypeName: Home.CONTENT_TYPE_NAME,
    controllerType: typeof(NACSMagazineHomeController),
    ActionName = "Index",
    Path = "/Home",
    WebsiteChannelNames = ["NACSMagazine"])]

namespace NACSMagazine.Features.Home
{
    public class NACSMagazineHomeController : Controller
    {
        // Service for executing content item queries
        private readonly IContentQueryExecutor executor;
        private readonly ITaxonomyRetriever taxonomyRetriever;

        public NACSMagazineHomeController(IContentQueryExecutor executor, ITaxonomyRetriever taxonomyRetriever)
        {
            this.executor = executor;
            this.taxonomyRetriever = taxonomyRetriever;
        }
        public async Task<IActionResult> Index()
        
        {
            var query = new ContentItemQueryBuilder()
                                // Scopes the query to pages of the NACSMagazine.Home content type
                                .ForContentType(NACSMagazine.Home.CONTENT_TYPE_NAME,
                                config => config
                                    // Retrieves the page with the /Home tree path under the NACSMagazine website channel
                                    .ForWebsite("NACSMagazine", PathMatch.Single("/Home")));

            // Executes the query and stores the data in the generated 'Home' class
            NACSMagazine.Home page = (await executor.GetMappedWebPageResult<NACSMagazine.Home>(query)).FirstOrDefault()!;

			var homeHeroArticle = await GetHomePageHero();
			//var homeHeroUrl = GetParentPageUrl(homeHeroArticle.SystemFields.ContentItemID);
			var heroFeaturedArticles = await GetHeroFeaturedArticles();
			var editorPicks = await GetEditorPicksArticles();
			var departmentArticles = await GetDepartmentArticles();
			//var NACSDailyArticles = await GetNACSDailyNewsArticles();

			HomePageViewModel model = new HomePageViewModel(page) { 
                Title = page.Title,
                HomeHero = homeHeroArticle,
                HeroFeaturedArticles = heroFeaturedArticles, 
                EditorPicksArticles = editorPicks, 
                DepartmentArticles = departmentArticles,
                //NACSDailyArticles = NACSDailyArticles
                };
                

            // Passes the home page content to the view using HomePageViewModel
            return View("Features/Home/NACSMagazine/Home.cshtml", model);
        }

		public async Task<IEnumerable<Article>> GetHomePageHero()
		{
			var idsQuery = new ContentItemQueryBuilder().ForContentType(Article.CONTENT_TYPE_NAME, config => config.Columns(nameof(Article.SystemFields.ContentItemID)).OrderBy("IssueDate DESC").TopN(1).Where(where => where.WhereEquals("CoverStory", 1)));

			var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

			var contentQuery = new ContentItemQueryBuilder()
								.ForContentType(
									ArticlePage.CONTENT_TYPE_NAME,
									config => config
									.ForWebsite("NACSMagazine")
									.Linking("ArticleContent", contentItemIds)
									.WithLinkedItems(3)
									).InLanguage("en");

			var articlePages = (await executor.GetMappedWebPageResult<ArticlePage>(contentQuery));

			var articleItem = new List<Article>();

			if (articlePages != null && articlePages.Any())
			{
				foreach (var article in articlePages.Where(w => w.ArticleContent.First().CoverStory.Equals(true)))
				{

					IEnumerable<Guid> tagIdentifiers = article.ArticleContent.First().ContentCategory.Select(item => item.Identifier);
					IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

					foreach (var tag in tags)
					{
						article.ArticleContent.First().CategoryTags = tag.Title;
					}

					articleItem.Add(article.ArticleContent.First());
					article.ArticleContent.First().ParentPageUrl = articlePages.First().SystemFields.WebPageUrlPath;
				}
			}

			if (!articleItem.Any())
			{
				return Enumerable.Empty<Article>();
			}

			return articleItem;
		}

        public async Task<IEnumerable<Article>> GetHeroFeaturedArticles()
        {
			var idsQuery = new ContentItemQueryBuilder().ForContentType(Article.CONTENT_TYPE_NAME, config => config.Columns(nameof(Article.SystemFields.ContentItemID)).OrderBy("IssueDate DESC").TopN(2).Where(where => where.WhereEquals("CurrentIssue", 1)));

			var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

			var contentQuery = new ContentItemQueryBuilder()
                                    .ForContentType(
                                    ArticlePage.CONTENT_TYPE_NAME,
                                    config => config
                                    .ForWebsite("NACSMagazine")
                                    .Linking("ArticleContent", contentItemIds)
                                    .WithLinkedItems(3)
                                    ).InLanguage("en");

            var featuredArticles = await executor.GetMappedWebPageResult<ArticlePage>(contentQuery);

            var articles = new List<Article>();
            foreach (var article in featuredArticles.Where(w => w.ArticleContent.First().CurrentIssue.Equals(true)).OrderByDescending(i => i.ArticleContent.First().IssueDate))
            {
				IEnumerable<Guid> tagIdentifiers = article.ArticleContent.First().ContentCategory.Select(item => item.Identifier);
				IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

                foreach (var tag in tags)
                {
                    article.ArticleContent.First().CategoryTags = tag.Title;
                }

                articles.Add(article.ArticleContent.First());
                article.ArticleContent.First().ParentPageUrl = article.SystemFields.WebPageUrlPath;
            }

            if(!articles.Any())
			{
				return Enumerable.Empty<Article>();
			}

			return articles;
        }

        public async Task<IEnumerable<Article>> GetEditorPicksArticles()
        {
			var idsQuery = new ContentItemQueryBuilder().ForContentType(Article.CONTENT_TYPE_NAME, config => config.Columns(nameof(Article.SystemFields.ContentItemID)).OrderBy("IssueDate DESC").TopN(3).Where(where => where.WhereEquals("EditorsPick", 1)));

			var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

			var contentQuery = new ContentItemQueryBuilder()
                                    .ForContentType(
                                    ArticlePage.CONTENT_TYPE_NAME,
                                    config => config
                                    .ForWebsite("NACSMagazine")
                                    .Linking("ArticleContent", contentItemIds)
                                    .WithLinkedItems(3)
                                    ).InLanguage("en");

            var editorPicksArticles = await executor.GetMappedWebPageResult<ArticlePage> (contentQuery);

            var articles = new List<Article>();
            if (editorPicksArticles.Any())
            {
                foreach (var article in editorPicksArticles)
                {
                    IEnumerable<Guid> tagIdentifiers = article.ArticleContent.First().ContentCategory.Select(item => item.Identifier);
                    IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

                    foreach (var tag in tags)
                    {
                        article.ArticleContent.First().CategoryTags = tag.Title;
                    }

                    articles.Add(article.ArticleContent.First());
                    article.ArticleContent.First().ParentPageUrl = article.SystemFields.WebPageUrlPath;
                }
            }

            if(!articles.Any())
            {
				return Enumerable.Empty<Article>();
			
            }
				return articles;
        }

        public async Task<IEnumerable<Article>> GetDepartmentArticles()
        {
            var articleList = new List<Article>();

            var sections = new List<string>();
            var sectionString = "From the Editor,The Big Question,NACS News,Convenience Cares,Inside Washington,Ideas 2 Go,Cool New Products,Gas Station Gourmet,Category Close-Up";

            foreach (string section in sectionString.Split(','))
            {
                sections.Add(section);
            }

            foreach (string section in sections)
            {
                await GetDepartmentArticle(section, articleList);
            }

            var departmentArticles = new List<Article>();
            if (articleList.Any())
            {
                departmentArticles = articleList;

                foreach (Article article in departmentArticles)
                {
                    IEnumerable<Guid> tagIdentifiers = article.ContentCategory.Select(item => item.Identifier);
                    IEnumerable<Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

                    foreach (Tag tag in tags)
                    {
                        article.CategoryTags = tag.Title;
                    }
                }
            }

            if (!departmentArticles.Any())
            {
                return Enumerable.Empty<Article>();
            }

				return departmentArticles;
        }

        public async Task GetDepartmentArticle(string magSection, List<Article> articleList)
        {
			var idsQuery = new ContentItemQueryBuilder().ForContentType(Article.CONTENT_TYPE_NAME, config => config.Columns(nameof(Article.SystemFields.ContentItemID)).OrderBy("IssueDate DESC").TopN(1).Where(where => where.WhereEquals("MagazineSection", magSection)));

			var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

			var contentQuery = new ContentItemQueryBuilder()
									.ForContentType(
									ArticlePage.CONTENT_TYPE_NAME,
									config => config
                                    .ForWebsite("NACSMagazine")
									.Linking("ArticleContent", contentItemIds)
									.WithLinkedItems(3)
									).InLanguage("en");

			var article = (await executor.GetMappedWebPageResult<ArticlePage>(contentQuery));

            if (article != null)
            {
                foreach (var articleItem in article.Where(w => w.ArticleContent.First().MagazineSection.Equals(magSection)))
                {
                    if (articleItem.ArticleContent.First().MagazineSection.Equals(magSection))
                    {

                        articleList.Add(articleItem.ArticleContent.First());
                    }
                }
            }
		}

        //This is getting articles from Convenience.org website and won't work until that site is built and the pagetype exists. For now I have it mapped to a NACSMagazine.Article page type, but this is wrong.
        //public async Task<IEnumerable<Article>> GetNACSDailyNewsArticles()
        //{
        //    var contentQuery = new ContentItemQueryBuilder()
        //                            .ForContentType(
        //                            "NACS.NewsArticle",
        //                            config => config
        //                            .TopN(6)
        //                            .WithLinkedItems(1)
        //                            .OrderBy("Date DESC, SortOrder ASC")
        //                            .ForWebsite("NACS", PathMatch.Single("/Media/Daily/%"))
        //                            ).InLanguage("en");

        //    var articles = await executor.GetMappedResult<Article>(contentQuery);

        //    return articles;
        //}
    }
}