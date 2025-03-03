using CMS.ContentEngine;
using CMS.Core;
using CMS.MediaLibrary;
using CMS.Websites;

using Kentico.Xperience.Lucene.Core.Indexing;

using Lucene.Net.Documents;
using Lucene.Net.Documents.Extensions;
using Lucene.Net.Facet;

using MediatR;

using Microsoft.AspNetCore.Components.Forms;

using MimeKit;

using NACS.Portal.Core;
using NACS.Portal.Core.Infrastructure.Search;

using System.Text.Json;

namespace NACSShow.Services.Search.SiteSearch
{
    public class SiteSearchIndexModel
    {
        public const string IndexName = "NS_SITE_SEARCH";

        public string? Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string? Content { get; set; } = "";
        public string Types { get; set; } = "";
        public string TypeFacet { get; set; } = "";
        public DateTime PublishedDate { get; set; }
        //public IReadOnlyList<DateTime> PublishedDateFacet { get; set; } = [];
        public string AuthorImage { get; set; } = string.Empty;

        public Document ToDocument()
        {

            var indexDocument = new Document()
            {
                new TextField(nameof(Title), Title, Field.Store.YES),
                new TextField(nameof(Content), Content, Field.Store.YES),
                new TextField(nameof(Types), Types, Field.Store.YES),
                new Int64Field(nameof(PublishedDate), DateTools.TicksToUnixTimeMilliseconds(PublishedDate.Ticks), Field.Store.YES),
                new TextField(nameof(AuthorImage), AuthorImage, Field.Store.YES),
            };

            //if(PublishedDateFacet.Count > 0)
            //{
            //    _ = indexDocument.AddFacetField(nameof(PublishedDateFacet), PublishedDateFacet.ToString()); //"MMM d, yyyy"
            //}

            if(!string.IsNullOrWhiteSpace(TypeFacet))
            {
                _ = indexDocument.AddFacetField(nameof(TypeFacet), TypeFacet);
            }

            return indexDocument;
        }

        public static SiteSearchIndexModel FromDocument(Document document)
        {
            var model = new SiteSearchIndexModel()
            {
                Title = document.Get(nameof(Title)),
                Url = document.Get(nameof(Url)),
                Content = document.Get(nameof(Content)),
                Types = document.Get(nameof(Types)),
                AuthorImage = document.Get(nameof(AuthorImage)),
                PublishedDate = new DateTime(
                    DateTools.UnixTimeMillisecondsToTicks(
                        long.Parse(document.Get(nameof(PublishedDate)))
                        ))
            };

            return model;
        }

        public class SiteSearchIndexingStrategy(
            IContentQueryExecutor executor,
            WebScraperHtmlSanitizer htmlSanitizer,
            WebCrawlerService webCrawler,
            IChannelDataProvider channelDataProvider,
            ITaxonomyRetriever taxonomyRetriever,
            IEventLogService log) : DefaultLuceneIndexingStrategy
        {
            public const string IDENTIFIER = "SITE_SEARCH";

            private readonly IContentQueryExecutor executor = executor;
            private readonly WebScraperHtmlSanitizer htmlSanitizer = htmlSanitizer;
            private readonly WebCrawlerService webCrawler = webCrawler;
            private readonly IChannelDataProvider channelDataProvider = channelDataProvider;
            private readonly ITaxonomyRetriever taxonomyRetriever = taxonomyRetriever;
            private readonly IEventLogService log = log;

            public async Task<IEnumerable<IIndexEventItemModel>> FindItemsToReIndex(IndexEventWebPageItemModel changedItem) => await Task.FromResult<List<IIndexEventItemModel>>([changedItem]);

            public async Task<IEnumerable<IIndexEventItemModel>> FindItemsToReIndex(IndexEventReusableItemModel changedItem)
            {
                if (string.Equals(changedItem.ContentTypeName, Workshop.CONTENT_TYPE_NAME) ||
                    string.Equals(changedItem.ContentTypeName, Page.CONTENT_TYPE_NAME) ||
                    string.Equals(changedItem.ContentTypeName, Speaker.CONTENT_TYPE_NAME))
                {
                    var reindexible = new List<IIndexEventItemModel>();

                    var b = new ContentItemQueryBuilder()
                            .ForContentTypes(q =>
                            q.OfContentType([Workshop.CONTENT_TYPE_NAME, Page.CONTENT_TYPE_NAME, Speaker.CONTENT_TYPE_NAME])
                            //.ForWebsite(true)
                            .WithLinkedItems(2));
                            //.Linking(Workshop.CONTENT_TYPE_NAME, nameof(Workshop.PageContent), [changedItem.ItemID]));

                    var page = (await executor.GetMappedWebPageResult<IWebPageFieldsSource>(b)).FirstOrDefault();
                    if (page is null)
                    {
                        log.LogWarning(
                            source: nameof(FindItemsToReIndex),
                            eventCode: "MISSING_PAGE",
                            eventDescription: $"Could not find Page for content [{changedItem.ItemID}].{Environment.NewLine}Skipping search indexing.");

                        return reindexible;
                    }

                    string channelName = await channelDataProvider.GetChannelNameByWebsiteChannelID(page.SystemFields.WebPageItemWebsiteChannelId) ?? "";

                    reindexible.Add(new IndexEventWebPageItemModel(
                        page.SystemFields.WebPageItemID,
                        page.SystemFields.WebPageItemGUID,
                        changedItem.LanguageName,
                        changedItem.ContentTypeName,
                        page.SystemFields.WebPageItemName,
                        page.SystemFields.ContentItemIsSecured,
                        page.SystemFields.ContentItemContentTypeID,
                        page.SystemFields.ContentItemCommonDataContentLanguageID,
                        channelName,
                        page.SystemFields.WebPageItemTreePath,
                        page.SystemFields.WebPageItemParentID,
                        page.SystemFields.WebPageItemOrder));

                    reindexible.Add(new IndexEventReusableItemModel(
                        page.SystemFields.ContentItemID,
                        page.SystemFields.ContentItemGUID,
                        changedItem.LanguageName,
                        changedItem.ContentTypeName,
                        page.SystemFields.ContentItemName,
                        page.SystemFields.ContentItemIsSecured,
                        page.SystemFields.ContentItemContentTypeID,
                        page.SystemFields.ContentItemCommonDataContentLanguageID));

                    return reindexible;
                }
                return [];
            }

            public override async Task<Document?> MapToLuceneDocumentOrNull(IIndexEventItemModel item)
            {
                var indexModel = new SiteSearchIndexModel();
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
                        //var tag = (await taxonomyRetriever.RetrieveTags(articleContent.ContentCategory.Select(t => t.Identifier), item.LanguageName)).FirstOrDefault();

                        var type = item.ContentTypeName;
                        indexModel.Types = type.ToString();
                        indexModel.TypeFacet = type.ToString();

                        string content = await webCrawler.CrawlWebPage(page);
                        indexModel.Content = content;
                        indexModel.Title = page.Title;
                        //TODO: how to map Created Date to PublishedDate?

                    }
                    else if (string.Equals(item.ContentTypeName, Page.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    {
                        var b = new ContentItemQueryBuilder()
                        .ForWebPage(Page.CONTENT_TYPE_NAME, webpageItem.ItemGuid, queryParameters => queryParameters.WithLinkedItems(2));

                        var page = (await executor.GetMappedWebPageResult<Page>(b)).FirstOrDefault();

                        if (page is null)
                        {
                            return null;
                        }

                        var type = item.ContentTypeName;
                        indexModel.Types = type.ToString();
                        indexModel.TypeFacet = type.ToString();

                        string content = await webCrawler.CrawlWebPage(page);
                        indexModel.Title = page.Title;
                        indexModel.Content = content; //page.PageContent;

                    }
                }
                else if(item is IndexEventReusableItemModel)
                {
                    if (string.Equals(item.ContentTypeName, Speaker.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
                    {
                        var b = new ContentItemQueryBuilder()
                        .ForContentType(Speaker.CONTENT_TYPE_NAME);

                        var page = (await executor.GetMappedResult<Speaker>(b)).FirstOrDefault();

                        if (page is null)
                        {
                            return null;
                        }

                        var type = item.ContentTypeName;

                        indexModel.Types = type.ToString();
                        indexModel.TypeFacet = type.ToString();
                        
                        //string content = await webCrawler.CrawlWebPage(page);
                        indexModel.Title = page.Title;
                        indexModel.Content = page.PageContent;
                        indexModel.AuthorImage = page.Image.Url;


                    }
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

            public override FacetsConfig? FacetsConfigFactory()
            {
                var facetConfig = new FacetsConfig();

                facetConfig.SetMultiValued(nameof(TypeFacet), false);

                return facetConfig;
            }
        }

        //public class ImageAssetViewModelSerializable
        //{
        //    public ImageAssetViewModelSerializable(ImageContent image)
        //    {
        //        ID = image.SystemFields.ContentItemGUID;
        //        Title = image.MediaItemTitle;
        //        URL = image.ImageContentAsset.Url;
        //        AltText = image.MediaItemShortDescription;
        //        Dimensions = new() { Width = image.MediaItemAssetWidth, Height = image.MediaItemAssetHeight };
        //    }

        //    public ImageAssetViewModelSerializable() { }

        //    public Guid ID { get; set; }
        //    public string Title { get; set; } = "";
        //    public string URL { get; set; } = "";
        //    public string AltText { get; set; } = "";
        //    public AssetDimensions Dimensions { get; set; } = new();

        //    public ImageViewModel ToImageViewModel() => new(Title, AltText, Dimensions.Width, Dimensions.Height, URL) { ID = ID };
        //}
    }
}
