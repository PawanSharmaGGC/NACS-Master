using CMS.ContentEngine;
using CMS.Core;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using NACSShow.Models;

namespace NACSShow.Components.Widgets.LeftNavigationWidget;

public class LeftNavigationWidgetViewComponent : ViewComponent
{
    private readonly IWebsiteChannelContext channelContext;
    private readonly IContentQueryExecutor executor;
    private readonly IWebPageUrlRetriever webPageUrlRetriever;
    private readonly IPreferredLanguageRetriever currentLanguageRetriever;
    private readonly IEventLogService log;

    private bool showChildrenOfChildren = true;
    private bool ShowTitle = false;
    private string TitleOverride = "";
    private string header = "";
    private string languageName = "";
    private IEnumerable<IContentItemFieldsSource> siblingPageItems = new List<IContentItemFieldsSource>();

    public LeftNavigationWidgetViewComponent(IWebsiteChannelContext channelContext, IContentQueryExecutor executor,
        IPreferredLanguageRetriever currentLanguageRetriever,
        IWebPageUrlRetriever webPageUrlRetriever, IEventLogService log)
    {
        this.channelContext = channelContext;
        this.executor = executor;
        this.currentLanguageRetriever = currentLanguageRetriever;
        this.webPageUrlRetriever = webPageUrlRetriever;
        this.log = log;
        languageName = currentLanguageRetriever.Get();
    }

    public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<LeftNavigationWidgetProperties> vm/*,Page currentPage*/)
    {
        var leftNavItems = new LeftNavItems();

        try
        {
            showChildrenOfChildren = vm.Properties.ShowChildrenOfChildren;

            var currentPage = await GetPageItemAsync(vm.Page.WebPageItemID);
            if (currentPage != null)
            {
                ShowTitle = GetBoolPropertyValue(currentPage, "LeftNavShowTitle");
                TitleOverride = GetStringPropertyValue(currentPage, "LeftNavTitleOverride");

                var navItems = GetNavItemsAsync(currentPage, vm.Page.WebPageItemID, vm.Page.WebPageItemID).Result;

                leftNavItems = new LeftNavItems
                {
                    LeftNavTitle = header,
                    NavItems = navItems,
                };
            }
        }
        catch (Exception ex)
        {
            log.LogException(nameof(LeftNavigationWidgetViewComponent), nameof(InvokeAsync), ex);
        }

        return View("~/Components/Widgets/LeftNavigationWidget/_LeftNavigationWidget.cshtml", leftNavItems);
    }

    private string GetNavTitle(IContentItemFieldsSource pageContentItems)
    {
        var title = GetStringPropertyValue(pageContentItems, "Title");
        return !string.IsNullOrEmpty(title) ? title : GetSystemPropertyValue(pageContentItems.SystemFields, "WebPageItemName");
    }

    private bool UserHasPermissions(IContentItemFieldsSource pageContentItems)
    {
        try
        {
            var propertyInfo = pageContentItems.GetType().GetProperty("ContentItemIsSecured");
            var isSecuredNode = propertyInfo?.GetValue(pageContentItems);

            int webPageItemParentID = ValidationHelper.GetInteger(GetSystemPropertyValue(pageContentItems.SystemFields, "WebPageItemParentID"), 0);

            IContentItemFieldsSource currentItem = pageContentItems;

            while (isSecuredNode == null)
            {

                var parentItem = siblingPageItems.FirstOrDefault(item =>
                {
                    int pageItemId = ValidationHelper.GetInteger(GetSystemPropertyValue(item.SystemFields, "WebPageItemID"), 0);
                    return pageItemId == webPageItemParentID;
                });

                if (parentItem == null)
                {
                    break; // If no parent is found, exit the loop
                }

                currentItem = parentItem;
                var currentPagePropertyInfo = currentItem.GetType().GetProperty("ContentItemIsSecured");
                isSecuredNode = currentPagePropertyInfo?.GetValue(currentItem);

                webPageItemParentID = ValidationHelper.GetInteger(GetSystemPropertyValue(currentItem.SystemFields, "WebPageItemParentID"), 0);
            }

            if (!ValidationHelper.GetBoolean(isSecuredNode, false) || ValidationHelper.GetBoolean(User.Identity.IsAuthenticated, true))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            log.LogException(nameof(LeftNavigationWidgetViewComponent), nameof(UserHasPermissions), ex);
        }
        return false;
    }

    private string GetStringPropertyValue(IContentItemFieldsSource pageContentItems, string propertyName)
    {
        var propertyInfo = pageContentItems.GetType().GetProperty(propertyName);
        return ValidationHelper.GetString(propertyInfo?.GetValue(pageContentItems), "");
    }

    private bool GetBoolPropertyValue(IContentItemFieldsSource pageContentItems, string propertyName)
    {
        var propertyInfo = pageContentItems.GetType().GetProperty(propertyName);
        return ValidationHelper.GetBoolean(propertyInfo?.GetValue(pageContentItems), false);
    }

    private string GetSystemPropertyValue(ContentItemFields pageContentItems, string propertyName)
    {
        var propertyInfo = pageContentItems.GetType().GetProperty(propertyName);
        return ValidationHelper.GetString(propertyInfo?.GetValue(pageContentItems), "");
    }

    private async Task<List<NavItem>> GetNavItemsAsync(IContentItemFieldsSource pageContent, int currentPageItemId, int pageItemId)
    {
        var items = new List<NavItem>();

        try
        {
            header = GetStringPropertyValue(pageContent, "Title");
            var navType = GetStringPropertyValue(pageContent, "NavType");
            var webPageItemParentID = ValidationHelper.GetInteger(GetSystemPropertyValue(pageContent.SystemFields, "WebPageItemParentID"), 0);

            var webPageItemID = ValidationHelper.GetInteger(GetSystemPropertyValue(pageContent.SystemFields, "WebPageItemID"), 0);
            var webPageItemTreePath = GetSystemPropertyValue(pageContent.SystemFields, "WebPageItemTreePath");

            switch (navType)
            {
                case null:
                case "":
                case "Inherit":
                    if (webPageItemParentID > 0)
                    {
                        var parentPage = await GetPageItemAsync(webPageItemParentID);
                        items.AddRange(await GetNavItemsAsync(parentPage, currentPageItemId, webPageItemParentID));
                    }
                    break;

                case "Siblings":
                case "SiblingsAndChildren":
                    items.AddRange(await GetSiblingsAndChildrenNavItemsAsync(webPageItemParentID, webPageItemTreePath, navType, currentPageItemId, pageItemId));
                    break;

                case "Children":
                    items.AddRange(await GetChildrenNavItemsAsync(webPageItemID, webPageItemTreePath, navType, currentPageItemId, pageItemId, nodeLevel: 1));
                    break;

                case "Manual":
                case "None":
                    break;
            }
        }
        catch (Exception ex)
        {
            log.LogException(nameof(LeftNavigationWidgetViewComponent), nameof(GetNavItemsAsync), ex);
        }
        return items;
    }

    private async Task<IEnumerable<NavItem>> GetSiblingsAndChildrenNavItemsAsync(int parentWebPageItemID, string siblingPath, string navType, int currentPageItemId, int pageItemId)
    {
        var items = new List<NavItem>();

        try
        {
            siblingPageItems = await GetNavigationPagesAsync(parentWebPageItemID, siblingPath, navType);

            foreach (var sibling in siblingPageItems)
            {
                if (UserHasPermissions(sibling))
                {
                    var siblingWebPageItemID = ValidationHelper.GetInteger(GetSystemPropertyValue(sibling.SystemFields, "WebPageItemID"), 0);
                    var pageUrl = await webPageUrlRetriever.Retrieve(siblingWebPageItemID, languageName);
                    var item = CreateNavItem(sibling, pageUrl.RelativePath, siblingWebPageItemID, currentPageItemId, pageItemId);

                    // add child items
                    if (navType.Equals("SiblingsAndChildren", StringComparison.OrdinalIgnoreCase))
                    {
                        var siblingPageTreePath = GetSystemPropertyValue(sibling.SystemFields, "WebPageItemTreePath");
                        item.ChildItems.AddRange(await GetChildrenNavItemsAsync(siblingWebPageItemID, siblingPageTreePath, navType, currentPageItemId, pageItemId, nodeLevel: 2));
                    }

                    items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            log.LogException(nameof(LeftNavigationWidgetViewComponent), nameof(GetSiblingsAndChildrenNavItemsAsync), ex);
        }
        return items;
    }

    private async Task<IEnumerable<NavItem>> GetChildrenNavItemsAsync(int parentWebPageItemID, string parentPageUrl, string navType, int currentPageItemId, int pageItemId, int nodeLevel)
    {
        var items = new List<NavItem>();

        try
        {
            var siblingPageItems = await GetNavigationPagesAsync(parentWebPageItemID, parentPageUrl, "Children");

            foreach (var child in siblingPageItems)
            {
                if (UserHasPermissions(child))
                {
                    var childWebPageItemID = ValidationHelper.GetInteger(GetSystemPropertyValue(child.SystemFields, "WebPageItemID"), 0);
                    var pageUrl = await webPageUrlRetriever.Retrieve(childWebPageItemID, languageName);
                    var item = CreateNavItem(child, pageUrl.RelativePath, childWebPageItemID, currentPageItemId, pageItemId);

                    // add child items for "Children" nav items
                    if (navType.Equals("Children", StringComparison.OrdinalIgnoreCase) && showChildrenOfChildren && nodeLevel < 2)
                    {
                        var childPageTreePath = GetSystemPropertyValue(child.SystemFields, "WebPageItemTreePath");
                        item.ChildItems.AddRange(await GetChildrenNavItemsAsync(childWebPageItemID, childPageTreePath, navType, currentPageItemId, pageItemId, nodeLevel: nodeLevel + 1));
                    }

                    items.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            log.LogException(nameof(LeftNavigationWidgetViewComponent), nameof(GetChildrenNavItemsAsync), ex);
        }

        return items;
    }

    private NavItem CreateNavItem(IContentItemFieldsSource pageContent, string pageUrl, int siblingWebPageItemID, int currentPageItemId, int pageItemId)
    {

        var navItem = new NavItem
        {
            Title = GetNavTitle(pageContent),
            URL = pageUrl,
            Target = "_self",
            CssClass = (siblingWebPageItemID == currentPageItemId || siblingWebPageItemID == pageItemId) ? "active" : "",
            ChildItems = new List<NavItem>()
        };

        return navItem;
    }

    private async Task<IContentItemFieldsSource> GetPageItemAsync(int webPageItemId)
    {
            var builder = new ContentItemQueryBuilder()
                .ForContentTypes(parameters =>
                {
                    parameters.ForWebsite(channelContext.WebsiteChannelName)
                              .WithContentTypeFields();
                })
                .Parameters(parameters =>
                {
                    parameters.Where(w => w.WhereEquals("WebPageItemID", webPageItemId));
                });

            var pageContent = await executor.GetMappedResult<IContentItemFieldsSource>(builder);
            return pageContent.FirstOrDefault();
    }

    private async Task<IEnumerable<IContentItemFieldsSource>> GetNavigationPagesAsync(int webPageItemId, string queryBuilderPath, string pathMatchType = "")
    {
        if (!pathMatchType.Equals("Children", StringComparison.OrdinalIgnoreCase))
        {
            var parentWebPage = GetPageItemAsync(webPageItemId).Result;
            queryBuilderPath = GetSystemPropertyValue(parentWebPage.SystemFields, "WebPageItemTreePath");
        }

        var builder = new ContentItemQueryBuilder()
                    .ForContentTypes(parameters =>
                    {
                        parameters.ForWebsite(channelContext.WebsiteChannelName,
                           PathMatch.Children(queryBuilderPath, 1)
                            , includeUrlPath: false)
                                  .WithContentTypeFields();
                    })
                    .Parameters(parameter =>
                    {
                        parameter.Where(i => i.WhereTrue("DocumentMenuItemHideInNavigation"));
                    })
                    ;

        var pageContent = await executor.GetMappedResult<IContentItemFieldsSource>(builder);
        return pageContent;
    }

}
