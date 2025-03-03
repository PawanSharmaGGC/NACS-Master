using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using Kentico.Content.Web.Mvc.Routing;
using NACSShow;
using NACSShow.Repositories.Pages.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using static HotChocolate.ErrorCodes;

namespace NACSShow.Repositories.Pages
{
    public class ContentRepository : IContentRepository
    {
        private readonly IContentQueryExecutor executor;
        private readonly IWebsiteChannelContext channelContext;

        public ContentRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext)
        {
            this.channelContext = channelContext;
            this.executor = executor;
        }

        // Gets all linked 'Daily News' from the 'Content Hub'
        public async Task<IEnumerable<DailyNews>> GetDailyNewContentItems()
        {

            var query = new ContentItemQueryBuilder()
                .ForContentType(
                        contentTypeName: DailyNews.CONTENT_TYPE_NAME,
                        configureQuery: config => config
                            .WithLinkedItems(1));

            // Materializes the query
            IEnumerable<DailyNews> dailyNewsList = await executor.GetMappedResult<DailyNews>(query);
            return dailyNewsList;
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticleContentItemsAsync(string title, DateTime date, string path)
        {
            var idsQuery = new ContentItemQueryBuilder().ForContentType(NewsArticle.CONTENT_TYPE_NAME, config => config.Columns(nameof(NewsArticle.SystemFields.ContentItemID)).OrderBy("Date DESC"));

            var contentItemIds = (await executor.GetResult(idsQuery, c => c.ContentItemID)).ToList();

            var query = new ContentItemQueryBuilder()
                            .ForContentType(
                                NewsArticlePage.CONTENT_TYPE_NAME,
                                config => config
                                .ForWebsite(channelContext.WebsiteChannelName, PathMatch.Children(path), includeUrlPath: true)
                                .Linking("NewsContent", contentItemIds)
                                .WithLinkedItems(2)
                                ).InLanguage("en");

            var alsoNewsArticles = await executor.GetMappedWebPageResult<NewsArticlePage>(query);

            var newsArticles = new List<NewsArticle>();
            foreach (var article in alsoNewsArticles.Where(w => !w.NewsContent.First().Title.Equals(title)))
            {
                var articleQuery = new ContentItemQueryBuilder()
                                        .ForContentType(
                                        NewsArticle.CONTENT_TYPE_NAME,
                                        config => config
                                        .WithLinkedItems(2)
                                        .Where(where => where.WhereNotEquals("Title", title)
                                        .And()
                                        .Where(where => where.WhereEquals("Date", date)))
                                        ).InLanguage("en");

                var articleItems = await executor.GetMappedResult<NewsArticle>(articleQuery);

                foreach (var a in articleItems)
                {
                    if (a.Title.Equals(article.NewsContent.First().Title))
                    {
                        newsArticles.Add(a);
                    }
                }
            }

            return newsArticles;
        }

    }
}