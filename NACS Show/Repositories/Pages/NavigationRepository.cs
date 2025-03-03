using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using Kentico.Content.Web.Mvc.Routing;
using NACSShow;
using NACSShow.Models;
using NACSShow.Repositories.Pages.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using static HotChocolate.ErrorCodes;

namespace NACSShow.Repositories.Pages
{
    public class NavigationRepository : INavigationRepository
    {
        private readonly IContentQueryExecutor executor;
        private readonly IWebsiteChannelContext channelContext;

        public NavigationRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext)
        {
            this.channelContext = channelContext;
            this.executor = executor;
        }

        /// <summary>
        /// Get all level menus
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NavigationMenu>> GetMenuItems()
        {
            IEnumerable<NavigationMenu> menuList = Enumerable.Empty<NavigationMenu>();
            var query = new ContentItemQueryBuilder()
                .ForContentType(
                        contentTypeName: NavTopLevel.CONTENT_TYPE_NAME,
                        configureQuery: config => config
                            .ForWebsite(channelContext.WebsiteChannelName)
                            .OrderBy("WebPageItemOrder"));

            // Materializes the query
            IEnumerable<NavTopLevel> menuItems = await executor.GetMappedResult<NavTopLevel>(query);

            foreach (var item in menuItems)
            {
                NavigationMenu navigationMenu = new NavigationMenu();
                navigationMenu.Menu = new NavigationItem() { MenuName = item.Title, MenuURL = item.Url };
                navigationMenu.SubMenu = await GetNavSecondLevelItems(item.SystemFields.WebPageItemTreePath);
                menuList = menuList.Append(navigationMenu);
            }

            return menuList;
        }

        /// <summary>
        /// Get only submenu based on parent menu path
        /// </summary>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public async Task<IEnumerable<NavigationItem>> GetNavSecondLevelItems(string parentPath)
        {
            IEnumerable<NavigationItem> navigationItems = Enumerable.Empty<NavigationItem>();
            var query = new ContentItemQueryBuilder()
                .ForContentType(
                        contentTypeName: NavSecondLevel.CONTENT_TYPE_NAME,
                        configureQuery: config => config
                            .ForWebsite(channelContext.WebsiteChannelName, PathMatch.Children(parentPath))
                            .OrderBy("WebPageItemOrder"));

            // Materializes the query
            IEnumerable<NavSecondLevel> submenuItems = await executor.GetMappedResult<NavSecondLevel>(query);

            if (submenuItems.Count() > 0)
            {
                navigationItems = submenuItems.Select(item => new NavigationItem()
                {
                    MenuName = item.Title,
                    MenuURL = item.Url
                });
            }

            return navigationItems;
        }

       
        /// <summary>
        /// Get social navigation menu
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NavigationItem>> GetSocialMenuItems()
        {
            IEnumerable<NavigationItem> navigationItems = Enumerable.Empty<NavigationItem>();
            var query = new ContentItemQueryBuilder()
                .ForContentType(
                        contentTypeName: SocialMenu.CONTENT_TYPE_NAME,
                        configureQuery: config => config
                            .ForWebsite(channelContext.WebsiteChannelName)
                            .OrderBy("WebPageItemOrder"));

            // Materializes the query
            IEnumerable<SocialMenu> menuItems = await executor.GetMappedResult<SocialMenu>(query);
            
            if (menuItems.Count() > 0)
            {
                navigationItems = menuItems.Select(item => new NavigationItem()
                {
                    MenuName = item.MenuName,
                    MenuURL = item.Url,
                    Icon = item.Icon
                });
            }
            return navigationItems;
        }
    }
}