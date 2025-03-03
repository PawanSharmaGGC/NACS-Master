using CMS.ContentEngine;
using System.Threading.Tasks;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using CMS.Websites;
using CMS.Websites.Routing;
using CMS.Core;
using Kentico.Content.Web.Mvc.Routing;
using System.Linq;
using CMS.Helpers;
using System;
using System.Collections.Generic;

namespace Convenience.org.Components.ViewComponents.Breadcrumb
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        private readonly IWebsiteChannelContext channelContext;
        private readonly IContentQueryExecutor executor;
        private readonly IWebPageUrlRetriever webPageUrlRetriever;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private readonly IEventLogService log;

        public BreadcrumbViewComponent(IWebsiteChannelContext channelContext,
            IContentQueryExecutor executor,
        IPreferredLanguageRetriever currentLanguageRetriever,
        IWebPageUrlRetriever webPageUrlRetriever, IEventLogService log)
        {
            this.channelContext = channelContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Components/ViewComponents/Breadcrumb/Breadcrumbs.cshtml");
        }
    }
}
