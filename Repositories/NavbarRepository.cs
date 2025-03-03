using System.Collections.Generic;
using CMS.ContentEngine;
using System.Linq;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using CMS.Websites;
using CMS.Websites.Routing;

namespace Convenience.org.Repositories
{
    public class NavbarRepository : INavbarRepository
    {
        private readonly IContentQueryExecutor executor;
        private readonly IWebsiteChannelContext channelContext;

        public NavbarRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext)
        {
            this.channelContext = channelContext;
            this.executor = executor;
        }

        /// <summary>
        /// Get navigation by parameter navigation path and nesting level
        /// </summary>
        /// <param name="navItemPath"></param>
        /// <param name="nestingLevel"></param>
        /// <returns></returns>
        public IEnumerable<NavBarMenuViewModel> GetNavItems(string navItemPath, int nestingLevel = 1)
        {
            IEnumerable<NavBarMenuViewModel> menuList = Enumerable.Empty<NavBarMenuViewModel>();
            var query = new ContentItemQueryBuilder()
                .ForContentType(
                        contentTypeName: NavTopLevel.CONTENT_TYPE_NAME,
                        configureQuery: config => config
                            .ForWebsite(channelContext.WebsiteChannelName, PathMatch.Children(navItemPath, nestingLevel))
                            .OrderBy("WebPageItemOrder"));


            // Materializes the query
            IEnumerable<NavTopLevel> menuItems = executor.GetMappedResult<NavTopLevel>(query)?.Result;

            foreach (var item in menuItems)
            {
                NavBarMenuViewModel navigationMenu = new NavBarMenuViewModel();
                navigationMenu.Menu = new NavbarItemViewModel() { Title = item.Title, Url = item.Url, IsLeftNavItem = item.IsLeftNavItem, IconClass = item.IconClass, IsChildInTwoColumn = item.IsChildInTwoColumn, OpenInNewTab = item.OpenInNewTab };
                navigationMenu.SubMenu = GetNavSecondLevelItems(item.SystemFields.WebPageItemTreePath);
                menuList = menuList.Append(navigationMenu);
            }

            return menuList;
        }


        /// <summary>
        /// Get only submenu based on parent menu path
        /// </summary>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public IEnumerable<NavbarItemViewModel> GetNavSecondLevelItems(string parentPath)
        {
            IEnumerable<NavbarItemViewModel> navigationItems = Enumerable.Empty<NavbarItemViewModel>();
            var query = new ContentItemQueryBuilder()
                .ForContentType(
                        contentTypeName: NavSecondLevel.CONTENT_TYPE_NAME,
                        configureQuery: config => config
                            .ForWebsite(channelContext.WebsiteChannelName, PathMatch.Children(parentPath))
                            .OrderBy("WebPageItemOrder"));

            // Materializes the query
            IEnumerable<NavSecondLevel> submenuItems = executor.GetMappedResult<NavSecondLevel>(query)?.Result;

            if (submenuItems.Count() > 0)
            {
                navigationItems = submenuItems.Select(item => new NavbarItemViewModel()
                {
                    Title = item.Title,
                    Url = item.Url
                });
            }

            return navigationItems;
        }

    }
}
