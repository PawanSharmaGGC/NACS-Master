using CMS.ContentEngine;
using CMS.Core;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.Websites;
using CMS.Websites.Routing;
using ConvenienceCares.Operations;
using Kentico.Content.Web.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NACSShow.Models;

namespace NACSShow.Components.ViewComponents
{
    /// <summary>
    /// Current page data view component.
    /// </summary>
    public class CurrentPageDataViewComponent : ViewComponent
    {
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IContentQueryExecutor executor;
        private readonly IEventLogService log;
        private readonly IMediator mediator;

        public CurrentPageDataViewComponent(IWebPageDataContextRetriever webPageDataContextRetriever, 
            IContentQueryExecutor executor, IWebsiteChannelContext channelContext, IEventLogService log, IMediator mediator)
        {
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.channelContext = channelContext;
            this.executor = executor;
            this.log = log;
            this.mediator = mediator;
        }
        public ViewViewComponentResult Invoke()
        {


            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            if (webPage != null)
            {
                var builder = new ContentItemQueryBuilder()
                        .ForContentType(webPage.ContentTypeName,
                        config => config
                        .ForWebsite(channelContext.WebsiteChannelName)
                        .WithLinkedItems(2)
                        .Where(w => w.WhereEquals("WebPageItemID", webPage.WebPageItemID))
                        .OrderBy("WebPageItemOrder"));

                TempData["PageContentTypeName"] = webPage.ContentTypeName;

                switch (webPage.ContentTypeName)
                {
                    case Page.CONTENT_TYPE_NAME:

                        var pageContent = executor.GetMappedResult<Page>(builder).Result.FirstOrDefault();
                        TempData["CurrentPageData"] = pageContent;
                        break;
                    case Speaker.CONTENT_TYPE_NAME:

                        var speakerPageContent = executor.GetMappedResult<Speaker>(builder).Result.FirstOrDefault();
                        TempData["CurrentPageData"] = speakerPageContent;
                        break;
                    case Video.CONTENT_TYPE_NAME:

                        var videoPageContent = executor.GetMappedResult<Video>(builder).Result.FirstOrDefault();
                        TempData["CurrentPageData"] = videoPageContent;
                        break;
                    case DailyNewsPage.CONTENT_TYPE_NAME:

                        TempData["CurrentPageData"] = MapNewsContentToViewModel<DailyNewsPage>(builder);
                        break;
                    case NewsArticlePage.CONTENT_TYPE_NAME:

                        TempData["CurrentPageData"] = MapNewsContentToViewModel<NewsArticlePage>(builder);
                        break;
                    case Convenience.Page.CONTENT_TYPE_NAME:

                        var conveniencePageContent = mediator.Send(new NACSFoundationPageQuery(webPage)).Result;
                        TempData["CurrentPageData"] = conveniencePageContent;
                        break;
                    case Convenience.EventPage.CONTENT_TYPE_NAME:

                        var convenienceEventPageContent = executor.GetMappedResult<Convenience.EventPage>(builder).Result.FirstOrDefault();
                        TempData["CurrentPageData"] = convenienceEventPageContent;
                        break;
                    
                }
            }
            return View("~/Components/ViewComponents/CurrentPageData/Default.cshtml");
        }

        private DailyNewsArticleViewModel MapNewsContentToViewModel<T>(ContentItemQueryBuilder builder) where T : class
        {
            try
            {
                var content = executor.GetMappedResult<T>(builder).Result.FirstOrDefault();
                if (content == null)
                {
                    return new DailyNewsArticleViewModel();
                }

                var newsContent = content.GetType().GetProperty("NewsContent")?.GetValue(content) as IEnumerable<object>;

                if (newsContent?.FirstOrDefault() is var firstContent && firstContent != null)
                {
                    var title = ValidationHelper.GetString(firstContent.GetType().GetProperty("Title")?.GetValue(firstContent), string.Empty);
                    var description = ValidationHelper.GetString(firstContent.GetType().GetProperty("Description")?.GetValue(firstContent), string.Empty);
                    var headerImage = firstContent.GetType().GetProperty("HeaderImage")?.GetValue(firstContent) as IEnumerable<AssetRelatedItem>;
                    var pageContent = ValidationHelper.GetString(firstContent.GetType().GetProperty("PageContent")?.GetValue(firstContent), string.Empty);
                    var date = ValidationHelper.GetDateTime(firstContent.GetType().GetProperty("Date")?.GetValue(firstContent), DateTime.MinValue);

                    return new DailyNewsArticleViewModel
                    {
                        Title = title,
                        Description = description,
                        HeaderImage = headerImage ?? [],
                        PageContent = pageContent,
                        Date = date
                    };
                   
                }
                return new DailyNewsArticleViewModel();

            }
            catch (Exception ex)
            {
                log.LogException(nameof(CurrentPageDataViewComponent), nameof(MapNewsContentToViewModel), ex);
                return new DailyNewsArticleViewModel();
            }

        }
    }
}
