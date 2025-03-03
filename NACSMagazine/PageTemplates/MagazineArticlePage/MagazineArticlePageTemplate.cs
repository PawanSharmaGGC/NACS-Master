using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACSMagazine;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine.PageTemplates.MagazineArticlePage;
using Kentico.Content.Web.Mvc;
using MediatR;
using CMS.ContentEngine;
using CMS.Websites;

[assembly: RegisterPageTemplate(
    identifier: ArticlePage.CONTENT_TYPE_NAME,
    name: "Magazine Article Page",
    propertiesType: null,
    customViewName: "~/PageTemplates/MagazineArticlePage/_MagazineArticlePage.cshtml",
    ContentTypeNames = [ArticlePage.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: ArticlePage.CONTENT_TYPE_NAME,
    controllerType: typeof(MagazineArticlePageTemplateController))]

namespace NACSMagazine.PageTemplates.MagazineArticlePage
{
    public class MagazineArticlePageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor executor;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly ITaxonomyRetriever taxonomyRetriever;

        public MagazineArticlePageTemplateController(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever, IContentQueryExecutor _executor, IWebPageUrlRetriever _urlRetriever, ITaxonomyRetriever _taxonomyRetriever)
        {
            mediator = _mediator;
            contextRetriever = _contextRetriever;
            executor = _executor;
            urlRetriever = _urlRetriever;
            taxonomyRetriever = _taxonomyRetriever;
        }

        public async Task<IActionResult> Index()
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }

            var page = await mediator.Send(new MagazineArticleQuery(data.WebPage));

            var editorsPicksArticles = await GetEditorsPicksAsync(page);
            page.EditorsPicks = editorsPicksArticles;

            var alsoArticles = await GetYouMayAlsoLikeArticlesAsync(page);
            page.YouMayAlsoLikeArticles = alsoArticles;

            return new TemplateResult(page);
        }

        public async Task<IEnumerable<ArticlePage>> GetCurrentArticleAsync(bool includeSecuredItems)
        {
            var idsQuery = new ContentItemQueryBuilder().ForContentType(Article.CONTENT_TYPE_NAME, config => config.Columns(nameof(Article.SystemFields.ContentItemID)));

            var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                                ArticlePage.CONTENT_TYPE_NAME,
                                config => config
                                .ForWebsite("NACSMagazine")
                                .TopN(1)
                                .Linking("ArticleContent", contentItemIds)
                                .WithLinkedItems(1)
                                ).InLanguage("en");

            var queryOptions = new ContentQueryExecutionOptions()
            {
                IncludeSecuredItems = includeSecuredItems
            };

            IEnumerable<ArticlePage> articles = await executor.GetMappedResult<ArticlePage>(query, queryOptions);

            
            return articles;                
        }

        public async Task<IEnumerable<Article>> GetEditorsPicksAsync(ArticlePage page)
        {
            var query = new ContentItemQueryBuilder()
                        .ForContentType(
                                Article.CONTENT_TYPE_NAME,
                                config => config
								.TopN(4)
								.WithLinkedItems(3)
                                .Where(where => where.WhereTrue("EditorsPick")
                                .And()
                                .Where(where => where.WhereNotEquals("Title", page.ArticleContent.First().Title)))
                                ).InLanguage("en");

            IEnumerable<Article> editorsPicks = await executor.GetMappedWebPageResult<Article>(query);

            return editorsPicks;
        }

        public async Task<IEnumerable<Article>> GetYouMayAlsoLikeArticlesAsync(ArticlePage page)
        {
            

            var idsQuery = new ContentItemQueryBuilder().ForContentType(Article.CONTENT_TYPE_NAME, config => config.Columns(nameof(Article.SystemFields.ContentItemID)).OrderBy("IssueDate DESC"));

            var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                                ArticlePage.CONTENT_TYPE_NAME,
                                config => config
                                .ForWebsite("NACSMagazine")
                                .Linking("ArticleContent", contentItemIds)
                                .WithLinkedItems(2)
                                ).InLanguage("en");

            var alsoArticles = await executor.GetMappedWebPageResult<ArticlePage>(query);

            var articles = new List<Article>();
            foreach(var article in alsoArticles.Where(w => !w.ArticleContent.First().Title.Equals(page.ArticleContent.First().Title)))
            {
                IEnumerable<Guid> tagIdentifiers = article.ArticleContent.First().ContentCategory.Select(item => item.Identifier);
                IEnumerable<CMS.ContentEngine.Tag> tags = await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en");

                foreach (CMS.ContentEngine.Tag tag in tags)
                {
                    page.ArticleContent.First().CategoryTags = tag.Title;
                }

                var articleQuery = new ContentItemQueryBuilder()
                                        .ForContentType(
                                        Article.CONTENT_TYPE_NAME,
                                        config => config
                                        .TopN(3)
                                        .WithLinkedItems(2)
                                        .Where(where => where.WhereNotEquals("Title", page.ArticleContent.First().Title)
                                        .And()
                                        .Where(where => where.WhereContainsTags("ContentCategory", tagIdentifiers)))
                                        ).InLanguage("en");

                var articleItems = await executor.GetMappedResult<Article>(articleQuery);

                foreach (var a in articleItems)
                {
                    if (a.Title.Equals(article.ArticleContent.First().Title))
                    {
                        articles.Add(a);
                        a.ParentPageUrl = "~/" + article.SystemFields.WebPageUrlPath;
                    }
                }
            }

            return articles;
        }
    }
}
