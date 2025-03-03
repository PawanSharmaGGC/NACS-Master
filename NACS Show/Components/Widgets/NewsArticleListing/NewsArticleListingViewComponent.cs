using Microsoft.AspNetCore.Mvc;
using NACSShow.Repositories.Pages.Interfaces;
using Kentico.Content.Web.Mvc;
using NACS.Portal.Core.Services;
using CMS.Websites.Routing;
using CMS.Websites;
using Kentico.PageBuilder.Web.Mvc;
using CMS.ContentEngine;

namespace NACSShow.Components.Widgets;

public class NewsArticleListingViewComponent : ViewComponent
{
    public const string IDENTIFIER = "NACSShow.NewsArticleListing";
    private readonly IWebPageDataContextRetriever contextRetriever;
    private readonly IContentRepository contentRepository;
    private readonly IAssetItemService assetItemService;
    private readonly IWebPageUrlRetriever webPageUrlRetriever;
    private readonly IWebsiteChannelContext websiteChannelContext;
    private readonly IContentQueryExecutor executor;


    public NewsArticleListingViewComponent(IWebPageDataContextRetriever contextRetriever,
    IContentRepository contentRepository, IAssetItemService assetItemService,
    IWebPageUrlRetriever webPageUrlRetriever, IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor)
    {
        this.contextRetriever = contextRetriever;
        this.contentRepository = contentRepository;
        this.assetItemService = assetItemService;
        this.webPageUrlRetriever = webPageUrlRetriever;
        this.websiteChannelContext = websiteChannelContext;
        this.executor = executor;
    }

    public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<NewsArticleListingProperties> widgetProperties)
    {
        if (widgetProperties?.Properties?.Path == null || !contextRetriever.TryRetrieve(out var data))
        {
            return View("~/Components/Widgets/NewsArticleListing/_NewsArticleListing.cshtml", new NewsArticleListingViewModel());
        }

        var webPage = data.WebPage;
        if (webPage == null)
        {
            return View("~/Components/Widgets/NewsArticleListing/_NewsArticleListing.cshtml", new NewsArticleListingViewModel());
        }

        string title = string.Empty;
        DateTime date = DateTime.MinValue;

        // Set up query builder
        var builder = new ContentItemQueryBuilder()
            .ForContentType(webPage.ContentTypeName, config => config
                .ForWebsite(websiteChannelContext.WebsiteChannelName)
                .WithLinkedItems(2)
                .Where(w => w.WhereEquals("WebPageItemID", webPage.WebPageItemID))
                .OrderBy("WebPageItemOrder"));

        // Get content 
        object content = null;
        if (webPage.ContentTypeName.Contains(NewsArticlePage.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
        {
            content = executor.GetMappedResult<NewsArticlePage>(builder).Result.FirstOrDefault();
        }
        else if (webPage.ContentTypeName.Contains(DailyNewsPage.CONTENT_TYPE_NAME, StringComparison.OrdinalIgnoreCase))
        {
            content = executor.GetMappedResult<DailyNewsPage>(builder).Result.FirstOrDefault();
        }

        if (content != null)
        {
            var pageContent = content.GetType().GetProperty("NewsContent")?.GetValue(content) as IEnumerable<object>;
            var firstPageContent = pageContent?.FirstOrDefault();
            if (firstPageContent != null)
            {
                title = firstPageContent.GetType().GetProperty("Title")?.GetValue(firstPageContent) as string;
                date = (DateTime?)firstPageContent.GetType().GetProperty("Date")?.GetValue(firstPageContent) ?? DateTime.MinValue;
            }
        }

        // Get news article items
        var newsArticleItems = await contentRepository.GetNewsArticleContentItemsAsync(title ?? string.Empty, date, widgetProperties.Properties.Path);
        if (newsArticleItems == null || !newsArticleItems.Any())
        {
            return View("~/Components/Widgets/NewsArticleListing/_NewsArticleListing.cshtml", new NewsArticleListingViewModel());
        }

        // Retrieve additional data for the news articles
        var newsItemGuids = newsArticleItems.Select(i => i.SystemFields.ContentItemGUID).ToList();
        var webPageUrls = await webPageUrlRetriever.Retrieve(newsItemGuids, websiteChannelContext.WebsiteChannelName, "en");
        var newsRollupImages = await assetItemService.RetrieveMediaFileImages(newsArticleItems.SelectMany(x => x.RollupImage).Where(icon => icon != null));

        // Map news articles to view model
        var newsArticleList = newsArticleItems.Select(s =>
        {
            var webPagePath = webPageUrls.TryGetValue(s.SystemFields.ContentItemGUID, out var url) ? url.RelativePath : "#";
            var rollupImage = newsRollupImages.FirstOrDefault(i => s.RollupImage.Any(sImg => sImg.Identifier == i.ID))?.URLData?.RelativePath ?? "";

            return new NewsArticleViewModel
            {
                Title = s.Title,
                Description = s.Description,
                ImageAltText = s.ImageAltText,
                WebPagePath = webPagePath,
                Date = s.Date.ToString("MMMM dd, yyyy") ?? string.Empty,
                RollupImage = rollupImage
            };
        }).ToList();

        // Prepare the view model
        var viewModel = new NewsArticleListingViewModel
        {
            Heading = widgetProperties.Properties.Heading,
            NewsArticleList = newsArticleList
        };

        return View("~/Components/Widgets/NewsArticleListing/_NewsArticleListing.cshtml", viewModel);
    }
}
