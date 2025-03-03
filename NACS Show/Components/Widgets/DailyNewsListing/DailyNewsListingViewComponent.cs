using CMS.ContentEngine;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using NACSShow.Components.Widgets.DailyNewsListing;
using NACSShow.Repositories.Pages.Interfaces;
using Kentico.Content.Web.Mvc.Routing;
using CMS.Websites.Routing;
using CMS.Websites;

[assembly: RegisterWidget(DailyNewsListingViewComponent.IDENTIFIER, typeof(DailyNewsListingViewComponent), "Daily News Listing", typeof(DailyNewsListingProperties), Description = "Display a list of daily news.")]

namespace NACSShow.Components.Widgets.DailyNewsListing
{
    public class DailyNewsListingViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "NACSShow.DailyNewsListing";
        private readonly IContentRepository contentRepository;
        private readonly IContentQueryExecutor executor;
        private readonly IWebPageUrlRetriever urlRetriever;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IPreferredLanguageRetriever preferredLanguageRetriever;

        public DailyNewsListingViewComponent(
        IContentQueryExecutor contentQueryExecutor,
        IWebPageUrlRetriever pageUrlRetriever,
        IWebsiteChannelContext websiteChannelContext,
        IPreferredLanguageRetriever preferredLanguageRetriever, IContentRepository contentRepository)
        {
            // Initializes instances of required services using dependency injection
            this.executor = contentQueryExecutor;
            this.urlRetriever = pageUrlRetriever;
            this.channelContext = websiteChannelContext;
            this.preferredLanguageRetriever = preferredLanguageRetriever;
            this.contentRepository = contentRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(DailyNewsListingProperties properties)
        {
            var dailyNewsItems = await contentRepository.GetDailyNewContentItems();

            var viewModel = new DailyNewsListingViewModel
            {
                Title = properties.Title ?? string.Empty, // Fix for CS8601
                DailyNewsList = dailyNewsItems ?? Enumerable.Empty<DailyNews>()
            };

            return View("~/Components/Widgets/DailyNewsListing/_DailyNewsListing.cshtml", viewModel);
        }
    }
}
