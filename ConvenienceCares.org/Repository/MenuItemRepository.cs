using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

namespace ConvenienceCares.Repository;

/// <summary>
/// Represents a collection of menu items.
/// </summary>
public class MenuItemRepository : ContentRepositoryBase
{

    public MenuItemRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor,
        IProgressiveCache cache)
        : base(websiteChannelContext, executor, cache)
    {
    }

    /// <summary>
    /// Returns list of <see cref="MenuItem"/> content items representing navigation menu.
    /// </summary>
    public async Task<IEnumerable<MenuItem>> GetMenuItems(string languageName, string navigationMenuFolderPath, CancellationToken cancellationToken)
    {
        var queryBuilder = GetQueryBuilder(navigationMenuFolderPath, languageName);

        var cacheSettings = new CacheSettings(Constants.DEFAULT_CACHE_MINUTES, WebsiteChannelContext.WebsiteChannelName, nameof(GetMenuItems), languageName);
        //var cacheSettings = CreateCacheSettings<MenuItem>(nameof(MenuItemRepository), nameof(GetMenuItems), languageName);

        return await GetCachedQueryResult<MenuItem>(queryBuilder, new ContentQueryExecutionOptions(), cacheSettings, GetDependencyCacheKeys, cancellationToken);
    }

    private ContentItemQueryBuilder GetQueryBuilder(string navigationMenuFolderPath, string languageName)
    {
        return new ContentItemQueryBuilder()
            .ForContentType(MenuItem.CONTENT_TYPE_NAME, config => config
                .ForWebsite(WebsiteChannelContext.WebsiteChannelName, PathMatch.Children(navigationMenuFolderPath), includeUrlPath: false)
                .OrderBy(nameof(IWebPageContentQueryDataContainer.WebPageItemOrder)))
            .InLanguage(languageName);
    }

    private Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<MenuItem> menuItems, CancellationToken cancellationToken)
    {
        if (menuItems == null)
        {
            return Task.FromResult<ISet<string>>(new HashSet<string>());
        }

        var dependencyCacheKeys = CreateCacheKeys(menuItems.Select(navItem => navItem.SystemFields.WebPageItemID), "webpageitem")
            .Append(CacheHelper.BuildCacheItemName(new[] { "webpageitem", "bychannel", WebsiteChannelContext.WebsiteChannelName, "childrenofpath", Constants.NAVIGATION_MENU_FOLDER_PATH }))
            .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        return Task.FromResult<ISet<string>>(dependencyCacheKeys);
    }

}
