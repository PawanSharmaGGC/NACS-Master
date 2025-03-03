using BalluunApi;

using CMS.ContentEngine;
using CMS.Core;
using CMS.Websites;

using Kentico.Xperience.Lucene.Core.Indexing;

using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;

using MediatR;

using NACS.Portal.Core;
using NACS.Portal.Core.Infrastructure.Search;
using NACS.Portal.Core.Models;

using NACSShow.Modules;
using NACSShow.Services.Search.Operations;

using Newtonsoft.Json;

using System.Globalization;

namespace NACSShow.Services.Search.SpeakerSearch
{
    public class SpeakerSearchIndexModel
    {
        public const string IndexName = "NS_SPEAKER_SEARCH";

        public string Title { get; set; } = "";
        //public ImageAssetViewModel Image { get; set; } = new ImageAssetViewModel();
        public string PageContent { get; set; } = "";
        public string Company { get; set; } = "";
        public string JobTitle { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Track { get; set; } = "";
        public IReadOnlyList<TaxonomyTag> TrackFacet { get; set; } = [];

        public Document ToDocument()
        {

            var indexDocument = new Document()
            {
                new TextField(nameof(Title), Title, Field.Store.YES),
                //new TextField(nameof(Image), JsonConvert.SerializeObject(Image), Field.Store.YES),
                new TextField(nameof(PageContent), PageContent, Field.Store.YES),
                new TextField(nameof(Company), Company, Field.Store.YES),
                new TextField(nameof(JobTitle), JobTitle, Field.Store.YES),
                new TextField(nameof(FirstName), FirstName, Field.Store.YES),
                new TextField(nameof(LastName), LastName, Field.Store.YES),
                new TextField(nameof(Description), Description, Field.Store.YES),
                new TextField(nameof(Track), Track, Field.Store.YES)
            };

            if(TrackFacet.Count > 0)
            {
                _ = indexDocument.AddFacetField(nameof(TrackFacet), TrackFacet.ToString());
            }

            return indexDocument;
        }

        public static SpeakerSearchIndexModel FromDocument(Document document)
        {
            var model = new SpeakerSearchIndexModel
            {
                Title = document.Get(nameof(Title)),
                //Image = JsonConvert.DeserializeObject<ImageAssetViewModel>(document.Get(nameof(Image))),
                PageContent = document.Get(nameof(PageContent)),
                Company = document.Get(nameof(Company)),
                JobTitle = document.Get(nameof(JobTitle)),
                FirstName = document.Get(nameof(FirstName)),
                LastName = document.Get(nameof(LastName)),
                Description = document.Get(nameof(Description)),
                Track = document.Get(nameof(Track))
            };

            return model;
        }

        public class SpeakerSearchIndexingStrategy(
            IContentQueryExecutor executor,
            //ITaxonomyRetriever taxonomyRetriever,
            WebScraperHtmlSanitizer htmlSanitizer,
            WebCrawlerService webCrawler,
            IChannelDataProvider channelDataProvider,
            IMediator mediator,
            IEventLogService log) : DefaultLuceneIndexingStrategy
        {
            public const string IDENTIFIER = "SPEAKER_SEARCH";

            private readonly IContentQueryExecutor executor = executor;
            //private readonly ITaxonomyRetriever taxonomyRetriever = taxonomyRetriever;
            private readonly WebScraperHtmlSanitizer htmlSanitizer = htmlSanitizer;
            private readonly WebCrawlerService webCrawler = webCrawler;
            private readonly IChannelDataProvider channelDataProvider = channelDataProvider;
            private readonly IMediator mediator = mediator;
            private readonly IEventLogService log = log;

            public async Task<IEnumerable<IIndexEventItemModel>> FindItemsToReIndex(IndexEventWebPageItemModel changedItem) => await Task.FromResult<List<IIndexEventItemModel>>([changedItem]);

            public async Task<IEnumerable<IIndexEventItemModel>> FindItemsToReIndex(IndexEventReusableItemModel changedItem)
            {
                if (string.Equals(changedItem.ContentTypeName, Speaker.CONTENT_TYPE_NAME))
                {
                    var reindexible = new List<IIndexEventItemModel>();

                    var b = new ContentItemQueryBuilder()
                            .ForContentTypes(q =>
                            q.OfContentType(Speaker.CONTENT_TYPE_NAME)
                            .ForWebsite(true)
                            .WithLinkedItems(2));
                    
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
                        Speaker.CONTENT_TYPE_NAME,
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
                var indexModel = new SpeakerSearchIndexModel();
                //if (item is IndexEventWebPageItemModel webpageItem)
                //{
                    if (string.Equals(item.ContentTypeName, Speaker.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    {
                        var b = new ContentItemQueryBuilder()
                        .ForContentType(Speaker.CONTENT_TYPE_NAME, queryParameters => queryParameters.WithLinkedItems(3));

                        var page = (await executor.GetMappedResult<Speaker>(b)).FirstOrDefault();

                        if (page is null)
                        {
                            return null;
                        }
                        //if (page.Track.FirstOrDefault() is not Workshop articleContent)
                        //{
                        //    return null;
                        //}



                    //var tag = (await taxonomyRetriever.RetrieveTags(articleContent.ContentCategory.Select(t => t.Identifier), item.LanguageName)).FirstOrDefault();

                    //string content = await webCrawler.CrawlWebPage(page);
                    indexModel.PageContent = page.PageContent; //htmlSanitizer.SanitizeHtmlDocument(content); 
                        //indexModel.Image = page.Image;
                        indexModel.Title = page.Title;
                        indexModel.Company = page.Company;
                        indexModel.JobTitle = page.JobTitle;
                        indexModel.FirstName = page.FirstName;
                        indexModel.LastName = page.LastName;
                        indexModel.Description = page.Description;

                    }
                    else
                    {
                        return null;
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

                //}
                //else
                //{
                //    return null;
                //}

                var taxonomies = await mediator.Send(new SpeakerTaxonomiesQuery());

                if (taxonomies is not null)
                {
                    indexModel.TrackFacet = taxonomies.Tracks;
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
