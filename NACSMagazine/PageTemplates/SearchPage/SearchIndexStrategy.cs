using CMS.ContentEngine;
using CMS.Core;
using CMS.Websites;

using Kentico.Xperience.Lucene.Core.Indexing;

using Lucene.Net.Documents;

using NACS.Portal.Core;
using NACS.Portal.Core.Infrastructure.Search;
using Newtonsoft.Json;

namespace NACSMagazine.PageTemplates.SearchPage
{
    public class ArticleSearchIndexModel
    {
        public const string IndexName = "ARTICLE_SEARCH";

        public string Title { get; set; } = "";
        public string? RollupImage { get; set; } = null;
        public string RollupImageURL { get; set; } = "";
        public string LedeText { get; set; } = "";
        public string PageContent { get; set; } = "";
        public string PageContentTeaser { get; set; } = "";
        public DateTime IssueDate { get; set; }
        public string ParentPageUrl { get; set; } = "";
        //public const string TaxonomyFacetField = $"{nameof(Taxonomy)}_Facet";
        //public string Taxonomy { get; set; } = "";

        public Document ToDocument()
        {
            var indexDocument = new Document()
            {
                new TextField(nameof(Title), Title, Field.Store.YES),
                new TextField(nameof(RollupImage), JsonConvert.SerializeObject(RollupImage), Field.Store.NO),
                new TextField(nameof(RollupImageURL), RollupImageURL, Field.Store.YES),
                new TextField(nameof(LedeText), LedeText, Field.Store.YES),
                new TextField(nameof(PageContent), PageContent, Field.Store.YES),
                new TextField(nameof(PageContentTeaser), PageContentTeaser, Field.Store.YES),
                new Int64Field(nameof(IssueDate), DateTools.TicksToUnixTimeMilliseconds(IssueDate.Ticks), Field.Store.YES),
                new TextField(nameof(ParentPageUrl), ParentPageUrl, Field.Store.YES),
                //new TextField(nameof(Type), (string.IsNullOrWhiteSpace(Type) ? "untaxonomized" : Type).ToLowerInvariant(), Field.Store.YES),
                //new TextField(nameof(Taxonomy), (string.IsNullOrWhiteSpace(Taxonomy) ? "untaxonomized" : Taxonomy).ToLowerInvariant(), Field.Store.YES),
            };

            //_ = indexDocument.AddFacetField(nameof(TaxonomyFacetField), indexDocument.Get(nameof(Taxonomy)));

            return indexDocument;
        }

        public static Article FromDocument(Document document)
        {
            var rollupImage = JsonConvert.DeserializeObject<ContentItemAsset>(document.Get(nameof(RollupImage)) ?? "{ }");

            var model = new Article
            {

                Title = document.Get(nameof(Title)),
                RollupImage = rollupImage,
                RollupImageURL = document.Get(nameof(RollupImageURL)),
                LedeText = document.Get(nameof(LedeText)),
                PageContent = document.Get(nameof(PageContent)),
                PageContentTeaser = document.Get(nameof(PageContentTeaser)),
                IssueDate = new DateTime(DateTools.UnixTimeMillisecondsToTicks(long.Parse(document.Get(nameof(IssueDate))))),
                ParentPageUrl = document.Get(nameof(ParentPageUrl)),
                //Type = document.Get(nameof(Type)),
            };

            return model;
        }

        public class ArticleSearchIndexingStrategy(
            IContentQueryExecutor executor,
            ITaxonomyRetriever taxonomyRetriever,
            WebScraperHtmlSanitizer htmlSanitizer,
            WebCrawlerService webCrawler,
            IChannelDataProvider channelDataProvider,
            IEventLogService log) : DefaultLuceneIndexingStrategy
        {
            public const string IDENTIFIER = "ARTICLE_SEARCH";

            private readonly IContentQueryExecutor executor = executor;
            private readonly ITaxonomyRetriever taxonomyRetriever = taxonomyRetriever;
            private readonly WebScraperHtmlSanitizer htmlSanitizer = htmlSanitizer;
            private readonly WebCrawlerService webCrawler = webCrawler;
            private readonly IChannelDataProvider channelDataProvider = channelDataProvider;
            private readonly IEventLogService log = log;

            public async Task<IEnumerable<IIndexEventItemModel>> FindItemsToReIndex(IndexEventWebPageItemModel changedItem) => await Task.FromResult<List<IIndexEventItemModel>>([changedItem]);

            public async Task<IEnumerable<IIndexEventItemModel>> FindItemsToReIndex(IndexEventReusableItemModel changedItem)
            {
                if (string.Equals(changedItem.ContentTypeName, Article.CONTENT_TYPE_NAME))
                {
                    var reindexible = new List<IIndexEventItemModel>();

                    var b = new ContentItemQueryBuilder()
                            .ForContentTypes(q =>
                            q.OfContentType(ArticlePage.CONTENT_TYPE_NAME)
                            .ForWebsite(true)
                            .WithLinkedItems(2)
                            .Linking(ArticlePage.CONTENT_TYPE_NAME, nameof(ArticlePage.ArticleContent), [changedItem.ItemID]));

                    var page = (await executor.GetMappedWebPageResult<IWebPageFieldsSource>(b)).FirstOrDefault();
                    if (page is null)
                    {
                        log.LogWarning(
                            source: nameof(FindItemsToReIndex),
                            eventCode: "MISSING_ARTICLEPAGE",
                            eventDescription: $"Could not find ArticlePage for article content [{changedItem.ItemID}].{Environment.NewLine}Skipping search indexing.");

                        return reindexible;
                    }

                    string channelName = await channelDataProvider.GetChannelNameByWebsiteChannelID(page.SystemFields.WebPageItemWebsiteChannelId) ?? "";

                    reindexible.Add(new IndexEventWebPageItemModel(
                        page.SystemFields.WebPageItemID,
                        page.SystemFields.WebPageItemGUID,
                        changedItem.LanguageName,
                        ArticlePage.CONTENT_TYPE_NAME,
                        page.SystemFields.WebPageItemName,
                        page.SystemFields.ContentItemIsSecured,
                        page.SystemFields.ContentItemContentTypeID,
                        page.SystemFields.ContentItemCommonDataContentLanguageID,
                        channelName,
                        page.SystemFields.WebPageItemTreePath,
                        page.SystemFields.WebPageItemParentID,
                        page.SystemFields.WebPageItemOrder));

                    return reindexible;
                }
                return [];
            }

            public override async Task<Document?> MapToLuceneDocumentOrNull(IIndexEventItemModel item)
            {
                var indexModel = new ArticleSearchIndexModel();
                if (item is IndexEventWebPageItemModel webpageItem)
                {
                    if (string.Equals(item.ContentTypeName, ArticlePage.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    {
                        var b = new ContentItemQueryBuilder()
                        .ForWebPage(ArticlePage.CONTENT_TYPE_NAME, webpageItem.ItemGuid, queryParameters => queryParameters.WithLinkedItems(3));

                        var page = (await executor.GetMappedWebPageResult<ArticlePage>(b)).FirstOrDefault();

                        if (page is null)
                        {
                            return null;
                        }
                        if (page.ArticleContent.FirstOrDefault() is not Article articleContent)
                        {
                            return null;
                        }



                        var tag = (await taxonomyRetriever.RetrieveTags(articleContent.ContentCategory.Select(t => t.Identifier), item.LanguageName)).FirstOrDefault();

                        string content = await webCrawler.CrawlWebPage(page);
                        indexModel.PageContent = htmlSanitizer.SanitizeHtmlDocument(content);
                        indexModel.PageContentTeaser = articleContent.PageContentTeaser;
                        indexModel.Title = articleContent.Title;
                        indexModel.IssueDate = articleContent.IssueDate;
                        indexModel.ParentPageUrl = "~/" + page.SystemFields.WebPageUrlPath;
                        indexModel.RollupImage = articleContent.RollupImage == null ? articleContent.RollupImageURL : articleContent.RollupImage.Url;
                        indexModel.RollupImageURL = articleContent.RollupImage?.Url ?? "";
                        indexModel.LedeText = articleContent.LedeText;
                        //indexModel.Taxonomy = articleContent.
                        //if (tag?.Title is string tagTitle)
                        //{
                        //    indexModel.Type = tagTitle;
                        //}
                        //indexModel.PublishedDate = articleContent.
                    }

                    //These would be used if you were doing search for all page types in the site, however for this implementation we are only searching Articles, so just the above code is required.

                    //else if (string.Equals(item.ContentTypeName, IssuePage.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var b = new ContentItemQueryBuilder()
                    //    .ForWebPage(IssuePage.CONTENT_TYPE_NAME, webpageItem.ItemGuid, queryParameters => queryParameters.WithLinkedItems(2));

                    //    var page = (await executor.GetMappedWebPageResult<IssuePage>(b)).FirstOrDefault();

                    //    if (page is null)
                    //    {
                    //        return null;
                    //    }
                    //    if (page.Issue.FirstOrDefault() is not Issue issueContent)
                    //    {
                    //        return null;
                    //    }

                    //    string content = await webCrawler.CrawlWebPage(page);
                    //    indexModel.PageContentTeaser = htmlSanitizer.SanitizeHtmlDocument(content);
                    //    indexModel.Title = issueContent.Title;
                    //    indexModel.IssueDate = issueContent.IssueDate;
                    //    indexModel.ParentPageUrl = "~/" + page.SystemFields.WebPageUrlPath;

                    //}
                    //else if (string.Equals(item.ContentTypeName, ArchivePage.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var b = new ContentItemQueryBuilder()
                    //    .ForWebPage(ArchivePage.CONTENT_TYPE_NAME, webpageItem.ItemGuid, queryParameters => queryParameters.WithLinkedItems(2));

                    //    var page = (await executor.GetMappedWebPageResult<ArchivePage>(b)).FirstOrDefault();

                    //    if (page is null)
                    //    {
                    //        return null;
                    //    }

                    //    string content = await webCrawler.CrawlWebPage(page);
                    //    indexModel.PageContentTeaser = htmlSanitizer.SanitizeHtmlDocument(content);
                    //    indexModel.Title = page.Title;

                    //}
                    //else if (string.Equals(item.ContentTypeName, NACSMagazine.CategoryPage.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var b = new ContentItemQueryBuilder()
                    //    .ForWebPage(NACSMagazine.CategoryPage.CONTENT_TYPE_NAME, webpageItem.ItemGuid, queryParameters => queryParameters.WithLinkedItems(2));

                    //    var page = (await executor.GetMappedWebPageResult<NACSMagazine.CategoryPage>(b)).FirstOrDefault();

                    //    if (page is null)
                    //    {
                    //        return null;
                    //    }

                    //    string content = await webCrawler.CrawlWebPage(page);
                    //    indexModel.PageContentTeaser = htmlSanitizer.SanitizeHtmlDocument(content);
                    //    indexModel.Title = page.Title;
                    //}
                    //else if (string.Equals(item.ContentTypeName, Home.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var b = new ContentItemQueryBuilder()
                    //    .ForWebPage(Home.CONTENT_TYPE_NAME, webpageItem.ItemGuid, queryParameters => queryParameters.WithLinkedItems(2));

                    //    var page = (await executor.GetMappedWebPageResult<Home>(b)).FirstOrDefault();

                    //    if (page is null)
                    //    {
                    //        return null;
                    //    }

                    //    string content = await webCrawler.CrawlWebPage(page);
                    //    indexModel.PageContentTeaser = htmlSanitizer.SanitizeHtmlDocument(content);
                    //    indexModel.Title = page.Title;
                    //}
                    //else if (string.Equals(item.ContentTypeName, NACSMagazine.LandingPage.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var b = new ContentItemQueryBuilder()
                    //    .ForWebPage(NACSMagazine.LandingPage.CONTENT_TYPE_NAME, webpageItem.ItemGuid, queryParameters => queryParameters.WithLinkedItems(2));

                    //    var page = (await executor.GetMappedWebPageResult<NACSMagazine.LandingPage>(b)).FirstOrDefault();

                    //    if (page is null)
                    //    {
                    //        return null;
                    //    }

                    //    string content = await webCrawler.CrawlWebPage(page);
                    //    indexModel.PageContentTeaser = htmlSanitizer.SanitizeHtmlDocument(content);
                    //    indexModel.Title = page.Title;
                    //}
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

                return indexModel.ToDocument();
            }

            //public override FacetsConfig? FacetsConfigFactory()
            //{
            //    var facetConfig = new FacetsConfig();

            //    facetConfig.SetMultiValued(nameof(TaxonomyFacetField), false);

            //    return facetConfig;
            //}
        }
    }
}
