using CMS.Helpers;
using CMS.Websites.Routing;
using CMS.Websites;
using ConvenienceCares.Interface.Services;
using ConvenienceCares.Models;
using ConvenienceCares.Repository;

namespace ConvenienceCares.Services;

public class MenuItemService : IMenuItemService
{
    private readonly MenuItemRepository menuItemRepository;
    private readonly IWebPageUrlRetriever webPageUrlRetriever;
    private readonly IProgressiveCache progressiveCache;
    private readonly IWebsiteChannelContext websiteChannelContext;


    public MenuItemService(
        MenuItemRepository menuItemRepository,
        IWebPageUrlRetriever webPageUrlRetriever,
        IProgressiveCache progressiveCache,
        IWebsiteChannelContext websiteChannelContext)
    {
        this.menuItemRepository = menuItemRepository;
        this.webPageUrlRetriever = webPageUrlRetriever;
        this.progressiveCache = progressiveCache;
        this.websiteChannelContext = websiteChannelContext;
    }


    public async Task<List<MenuItemViewModel>> GetMenuItemViewModels(string languageName, string navigationMenuFolderPath, CancellationToken cancellationToken = default)
    {
        var menuItems = (await menuItemRepository.GetMenuItems(languageName, navigationMenuFolderPath, cancellationToken))
        .ToList();

        var menuItemGuids = menuItems.Where(x => x.MenuLink.Any())
            .Select(menuItem => menuItem.MenuLink.First().WebPageGuid)
            .ToList();

        var menuItemModel = await GetModelsCached(menuItems, menuItemGuids, languageName, cancellationToken);

        return menuItemModel;
    }


	private async Task<List<MenuItemViewModel>> GetModelsCached(List<MenuItem> menuItems, List<Guid> menuItemGuids, string languageName, CancellationToken cancellationToken)
	{
		var cacheSettings = new CacheSettings(Constants.DEFAULT_CACHE_MINUTES, websiteChannelContext.WebsiteChannelName, nameof(GetMenuItemViewModels), languageName);

		List<MenuItemViewModel>? menuItemsModel = await progressiveCache.LoadAsync(async (settings, cancellationToken) =>
		{
			var urls = await webPageUrlRetriever.Retrieve(menuItemGuids, websiteChannelContext.WebsiteChannelName, languageName, cancellationToken: cancellationToken);

			var menuItemModel = menuItems
					.Select(x =>
						new MenuItemViewModel(
							x.SystemFields.WebPageItemID,
							x.MenuItemName,
							x.MenuLink.Any() && urls.ContainsKey(x.MenuLink.First().WebPageGuid) ? urls[x.MenuLink.First().WebPageGuid].RelativePath : "#",
							x.OpenInNewTab,
							x.SystemFields.WebPageItemParentID,
							x.SystemFields.WebPageItemOrder,
							new List<MenuItemViewModel>()
						)).ToList();

			if (menuItemModel != null && menuItemModel.Count > 0)
			{
				var cacheKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

				foreach (var key in menuItemGuids)
				{
					cacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byguid", key.ToString() }, false));
				}

				cacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "webpageitem", "bychannel", websiteChannelContext.WebsiteChannelName, "childrenofpath", Constants.NAVIGATION_MENU_FOLDER_PATH }));

				cacheSettings.CacheDependency = CacheHelper.GetCacheDependency(cacheKeys);
			}

			if (menuItemModel != null && menuItemModel.Count > 0)
			{
				var menu = BuildMenuItemHierarchy(menuItemModel);
				return menu;
			}

			return menuItemModel ?? new List<MenuItemViewModel>();

		}, cacheSettings, cancellationToken);
		return menuItemsModel ?? new List<MenuItemViewModel>();
	}


    private List<MenuItemViewModel> BuildMenuItemHierarchy(List<MenuItemViewModel> allItems)
    {
        var menuItems = new List<MenuItemViewModel>();

        var itemsLookup = allItems.ToLookup(item => item.ParentId);
        var allItemsIds = new HashSet<int>(allItems.Select(item => item.ItemId));
        var topLevelItems = allItems.Where(item => !allItemsIds.Contains(item.ParentId)).ToList();

        foreach (var item in topLevelItems)
        {
            menuItems.Add(BuildMenuItem(item, itemsLookup));
        }
        return menuItems;
    }

    private MenuItemViewModel BuildMenuItem(MenuItemViewModel menuItem, ILookup<int, MenuItemViewModel> itemsLookup)
    {
        var children = itemsLookup[menuItem.ItemId].ToList();
        foreach (var child in children)
        {
            var childMenuItem = BuildMenuItem(child, itemsLookup);
            menuItem.Childern.Add(childMenuItem);
        }
        return menuItem;
    }

}
