using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using NACSMagazine;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine.PageTemplates.MagazineIssuePage;
using Kentico.Content.Web.Mvc;
using MediatR;
using NACSMagazine.PageTemplates.MagazineArchivePage;
using NACS.Portal.Core.Rendering;
using CMS.ContentEngine;
using CMS.Websites;

[assembly: RegisterPageTemplate(
    identifier: "NACSMagazine.IssuePage",
    name: "Magazine Issue Page",
    propertiesType: null,
    customViewName: "~/PageTemplates/MagazineIssuePage/_MagazineIssuePage.cshtml",
    ContentTypeNames = [IssuePage.CONTENT_TYPE_NAME]
    )]

[assembly: RegisterWebPageRoute(
    contentTypeName: IssuePage.CONTENT_TYPE_NAME,
    controllerType: typeof(MagazineIssuePageTemplateController))]

namespace NACSMagazine.PageTemplates.MagazineIssuePage
{
    public class MagazineIssuePageTemplateController : Controller
    {
        private readonly IMediator mediator;
        private readonly IWebPageDataContextRetriever contextRetriever;
        private readonly IContentQueryExecutor executor;

        public MagazineIssuePageTemplateController(IMediator _mediator, IWebPageDataContextRetriever _contextRetriever, IContentQueryExecutor _executor)
        {
            mediator = _mediator;
            contextRetriever = _contextRetriever;
            executor = _executor;
        }

        public async Task<IActionResult> Index()
        {
            if (!contextRetriever.TryRetrieve(out var data))
            {
                return NotFound();
            }
            
            var page = await mediator.Send(new MagazineIssueQuery(data.WebPage));

            var issue = await GetIssuesAsync(page);
            page.Issue = issue;

            var currentFeaturedArticleList = await GetCurrentFeaturedArticlesAsync(page);
            page.CurrentFeaturedArticleList = currentFeaturedArticleList;

            var articleList = await GetArticlesAsync(page);
            page.ArticleList = articleList.Where(where => where.MagazineSection != "Feature");

            var otherIssuesList = await GetOtherIssuesAsync(page);
            page.OtherIssuesList = otherIssuesList;

            return new TemplateResult(page);
        }

        public async Task<IEnumerable<Issue>> GetIssuesAsync(IssuePage page)
        {
            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                                Issue.CONTENT_TYPE_NAME,
                                config => config
                                .TopN(1)
                                .WithLinkedItems(1)
                                .OrderBy("IssueDate DESC")
                                .Where(where => where.WhereEquals("Title", page.Title))
                                ).InLanguage("en");

            var issues = await executor.GetMappedResult<Issue>(query);

            var issueList = new List<Issue>();
            foreach(var issue in issues.Where(w=>w.Title == page.Title))
            {
                issueList.Add(issue);
            }

            return issueList;
        }

        public async Task<IEnumerable<Article>> GetCurrentFeaturedArticlesAsync(IssuePage page)
        {
            var idsQuery = new ContentItemQueryBuilder().ForContentType(Article.CONTENT_TYPE_NAME, config => config.Columns(nameof(Article.SystemFields.ContentItemID)).OrderBy("IssueDate DESC").Where(where => where.WhereEquals("IssueDate", page.Issue.First().IssueDate).Where(w=>w.WhereEquals("MagazineSection", "Feature"))));

            var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

            var query = new ContentItemQueryBuilder()
                        .ForContentType(
                                ArticlePage.CONTENT_TYPE_NAME,
                                config => config
                                .ForWebsite("NACSMagazine")
                                .Linking("ArticleContent", contentItemIds)
                                .WithLinkedItems(3)
                                ).InLanguage("en");

            var currentFeaturedArticleList = await executor.GetMappedResult<ArticlePage>(query);

            var articleList = new List<Article>();
            foreach(var article in currentFeaturedArticleList.Where(w=>w.ArticleContent.First().IssueDate.Equals(page.Issue.First().IssueDate)).Where(w=>w.ArticleContent.First().MagazineSection.Equals("Feature")))
            {
                articleList.Add(article.ArticleContent.First());
                article.ArticleContent.First().ParentPageUrl = "~/" + article.SystemFields.WebPageUrlPath;
            }

            return articleList;
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync(IssuePage page)
        {
            var idsQuery = new ContentItemQueryBuilder().ForContentType(Article.CONTENT_TYPE_NAME, config => config.Columns(nameof(Article.SystemFields.ContentItemID)).OrderBy("IssueDate DESC").Where(where => where.WhereEquals("IssueDate", page.Issue.First().IssueDate)));

            var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

            var query = new ContentItemQueryBuilder()
                        .ForContentType(
                                ArticlePage.CONTENT_TYPE_NAME,
                                config => config
                                .ForWebsite("NACSMagazine")
                                .WithLinkedItems(3)
                                ).InLanguage("en");

            var articleList = await executor.GetMappedResult<ArticlePage>(query);

            var articles = new List<Article>();
            foreach (var article in articleList.Where(w=>w.ArticleContent.First().IssueDate.Equals(page.Issue.First().IssueDate)))
            {
                articles.Add(article.ArticleContent.First());
                article.ArticleContent.First().ParentPageUrl = "~/" + article.SystemFields.WebPageUrlPath;
            }

            return articles;
        }

        public async Task<IEnumerable<Issue>> GetOtherIssuesAsync(IssuePage page)
        {
            var idsQuery = new ContentItemQueryBuilder().ForContentType(Issue.CONTENT_TYPE_NAME, config => config.Columns(nameof(Issue.SystemFields.ContentItemID)).TopN(10).Where(where => where.WhereNotEquals("Title", page.Title)));

            var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                                IssuePage.CONTENT_TYPE_NAME,
                                config => config
                                .ForWebsite("NACSMagazine")
                                .Linking("Issue", contentItemIds)
                                .WithLinkedItems(3)
                                .OrderBy("Title DESC")
                                .Where(where => where.WhereNotEquals("Title", page.Title))
                                ).InLanguage("en");

            IEnumerable<IssuePage> otherIssuesList = await executor.GetMappedResult<IssuePage>(query);

            var issues = new List<Issue>();
            foreach(var issue in otherIssuesList.Where(w=> w.Title != page.Title))
            {
                issues.Add(issue.Issue.First());
                issues.First().ParentPageUrl = "~/" + issue.SystemFields.WebPageUrlPath;
            }

            return issues;
        }
    }
}
