using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Components.Widgets.SocialMetaTags;
using Convenience.org.Repositories.Interfaces;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;


[assembly: RegisterWidget(identifier: SocialMetaTagsViewComponent.IDENTIFIER, name: "SocialMetaTags",
    viewComponentType: typeof(SocialMetaTagsViewComponent),
    propertiesType: typeof(SocialMetaTagsProperties), Description = "SocialMetaTags",
    IconClass = "icon-list", AllowCache = true)]

namespace Convenience.org.Components.Widgets.SocialMetaTags
{
    public class SocialMetaTagsViewComponent : ViewComponent
    {
        public const string IDENTIFIER = "SocialMetaTags";
        private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IContentQueryExecutor contentQueryExecutor;

        public SocialMetaTagsViewComponent(IWebPageDataContextRetriever webPageDataContextRetriever, IWebsiteChannelContext channelContext, IContentQueryExecutor contentQueryExecutor)
        {
            this.webPageDataContextRetriever = webPageDataContextRetriever;
            this.channelContext = channelContext;
            this.contentQueryExecutor = contentQueryExecutor;
        }

        public IViewComponentResult Invoke(ComponentViewModel<SocialMetaTagsProperties> widgetProperties)
        {

            var vm = new SocialMetaTagsViewModel();
            var webPage = webPageDataContextRetriever.Retrieve().WebPage;
            var builder = new ContentItemQueryBuilder().ForContentType(webPage.ContentTypeName, config => config
            .ForWebsite(channelContext.WebsiteChannelName).Where(w => w.WhereEquals("WebPageItemID", webPage.WebPageItemID)).OrderBy("WebPageItemOrder"));
            
            //var pageMetaDetails = contentQueryExecutor.GetWebPageResult(builder:builder, resultSelector:container =>)

            //TBD update the logic's for default social meta tags for current page data item
            if (webPage != null)
            {
                vm.Title = ValidationHelper.GetString(widgetProperties.Properties.DefaultTitle, widgetProperties.Properties.DefaultTitle);
                vm.Description = ValidationHelper.GetString(widgetProperties.Properties.DefaultDescription, widgetProperties.Properties.DefaultDescription);
                vm.Image = ValidationHelper.GetString(widgetProperties.Properties.DefaultImage, widgetProperties.Properties.DefaultImage);
                vm.SocialMediaHandler = ValidationHelper.GetString(widgetProperties.Properties.SocialMediaHandler, widgetProperties.Properties.SocialMediaHandler);

            }
            return View("~/Components/Widgets/SocialMetaTags/_SocialMetaTags.cshtml", vm);
        }
    }
}