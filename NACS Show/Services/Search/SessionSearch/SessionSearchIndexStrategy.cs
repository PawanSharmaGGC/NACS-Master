using CMS.ContentEngine;
using CMS.Core;
using CMS.Websites;

using Kentico.Xperience.Lucene.Core.Indexing;

using Lucene.Net.Documents;

using NACS.Portal.Core;
using NACS.Portal.Core.Infrastructure.Search;
using Newtonsoft.Json;

using System.Globalization;

namespace NACSShow.Services.Search.SessionSearch
{
    public class SessionSearchIndexModel
    {
        public const string IndexName = "NS_SESSION_SEARCH";

        public string Title { get; set; } = "";
        //public string Icon { get; set; } = "";
        public string StartTime { get; set; } = DateTime.Now.ToString("MMddyyyy");
        public string EndTime { get; set; } = DateTime.Now.ToString("MMddyyyy");
        public string PageContent { get; set; } = "";
        public string Keyword { get; set; } = "";
        public string Format { get; set; } = "";
        public string Translation { get; set; } = "";
        public string Segment { get; set; } = "";

        public Document ToDocument()
        {

            var indexDocument = new Document()
            {
                new TextField(nameof(Title), Title, Field.Store.YES),
                //new TextField(nameof(Icon), Icon, Field.Store.NO),
                new TextField(nameof(StartTime), StartTime, Field.Store.YES),
                //new TextField(nameof(EndTime), EndTime, Field.Store.YES),
                new TextField(nameof(PageContent), PageContent, Field.Store.YES),
                new TextField(nameof(Keyword), Keyword, Field.Store.YES),
                new TextField(nameof(Format), Format, Field.Store.YES),
                new TextField(nameof(Translation), Translation, Field.Store.YES),
                new TextField(nameof(Segment), Segment, Field.Store.YES)
            };

            return indexDocument;
        }

        public static SessionSearchIndexModel FromDocument(Document document)
        {

            var dt = DateTime.ParseExact(document.Get(nameof(StartTime)), "MMddyyyy", CultureInfo.InvariantCulture);//DateTools.StringToDate(document.Get(nameof(StartTime))); DateTime.Parse(document.Get(nameof(StartTime)));

            var model = new SessionSearchIndexModel
            {
                Title = document.Get(nameof(Title)),
                //Icon = document.Get(nameof(Icon)),
                StartTime = string.IsNullOrEmpty(document.Get(nameof(StartTime))) ? DateTime.Now.ToString() : dt.ToString("MMddyyyy"),
                //EndTime = string.IsNullOrEmpty(document.Get(nameof(EndTime))) ? DateTime.Now : DateTime.Parse(document.Get(nameof(EndTime))),
                PageContent = document.Get(nameof(PageContent)),
                Keyword = document.Get(nameof(Keyword)),
                Format = document.Get(nameof(Format)),
                Translation = document.Get(nameof(Translation)),
                Segment = document.Get(nameof(Segment))
            };

            return model;
        }

        public class SessionSearchIndexingStrategy(
            IContentQueryExecutor executor,
            //ITaxonomyRetriever taxonomyRetriever,
            WebScraperHtmlSanitizer htmlSanitizer,
            WebCrawlerService webCrawler,
            IChannelDataProvider channelDataProvider,
            IEventLogService log) : DefaultLuceneIndexingStrategy
        {
            public const string IDENTIFIER = "SESSION_SEARCH";

            private readonly IContentQueryExecutor executor = executor;
            //private readonly ITaxonomyRetriever taxonomyRetriever = taxonomyRetriever;
            private readonly WebScraperHtmlSanitizer htmlSanitizer = htmlSanitizer;
            private readonly WebCrawlerService webCrawler = webCrawler;
            private readonly IChannelDataProvider channelDataProvider = channelDataProvider;
            private readonly IEventLogService log = log;

            public async Task<IEnumerable<IIndexEventItemModel>> FindItemsToReIndex(IndexEventWebPageItemModel changedItem) => await Task.FromResult<List<IIndexEventItemModel>>([changedItem]);

            public async Task<IEnumerable<IIndexEventItemModel>> FindItemsToReIndex(IndexEventReusableItemModel changedItem)
            {
                if (string.Equals(changedItem.ContentTypeName, Workshop.CONTENT_TYPE_NAME))
                {
                    var reindexible = new List<IIndexEventItemModel>();

                    var b = new ContentItemQueryBuilder()
                            .ForContentTypes(q =>
                            q.OfContentType(Workshop.CONTENT_TYPE_NAME)
                            .ForWebsite(true)
                            .WithLinkedItems(2));
                    //.Linking(Workshop.CONTENT_TYPE_NAME, nameof(Workshop.PageContent), [changedItem.ItemID]));

                    var page = (await executor.GetMappedWebPageResult<IWebPageFieldsSource>(b)).FirstOrDefault();
                    if (page is null)
                    {
                        log.LogWarning(
                            source: nameof(FindItemsToReIndex),
                            eventCode: "MISSING_SESSIONPAGE",
                            eventDescription: $"Could not find Session Page for content [{changedItem.ItemID}].{Environment.NewLine}Skipping search indexing.");

                        return reindexible;
                    }

                    string channelName = await channelDataProvider.GetChannelNameByWebsiteChannelID(page.SystemFields.WebPageItemWebsiteChannelId) ?? "";

                    reindexible.Add(new IndexEventWebPageItemModel(
                        page.SystemFields.WebPageItemID,
                        page.SystemFields.WebPageItemGUID,
                        changedItem.LanguageName,
                        Workshop.CONTENT_TYPE_NAME,
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
                var indexModel = new SessionSearchIndexModel();
                if (item is IndexEventWebPageItemModel webpageItem)
                {
                    if (string.Equals(item.ContentTypeName, Workshop.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    {
                        var b = new ContentItemQueryBuilder()
                        .ForWebPage(Workshop.CONTENT_TYPE_NAME, webpageItem.ItemGuid, queryParameters => queryParameters.WithLinkedItems(3));

                        var page = (await executor.GetMappedWebPageResult<Workshop>(b)).FirstOrDefault();

                        if (page is null)
                        {
                            return null;
                        }
                        //if (page.PageContent.FirstOrDefault() is not Workshop articleContent)
                        //{
                        //    return null;
                        //}



                        //var tag = (await taxonomyRetriever.RetrieveTags(articleContent.ContentCategory.Select(t => t.Identifier), item.LanguageName)).FirstOrDefault();

                        string content = await webCrawler.CrawlWebPage(page);
                        indexModel.PageContent = page.PageContent; //htmlSanitizer.SanitizeHtmlDocument(content); 
                        indexModel.StartTime = page.StartTime.ToString("MMddyyyy");
                        //indexModel.EndTime = page.EndTime.ToString("MMddyyyy");
                        indexModel.Title = page.Title;
                        indexModel.Keyword = page.Keyword;
                        indexModel.Format = page.Format;
                        indexModel.Translation = page.Translation;
                        indexModel.Segment = page.Segment;
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
