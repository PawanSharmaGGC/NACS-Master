using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.ViewComponents.Navbar
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IContentQueryExecutor executor;
        private readonly IWebsiteChannelContext channelContext;
        private readonly INavbarRepository navBarRepository;

        public NavbarViewComponent(IContentQueryExecutor executor, IWebsiteChannelContext channelContext, 
            INavbarRepository navBarRepository)
        {
            this.channelContext = channelContext;
            this.executor = executor;
            this.navBarRepository = navBarRepository;
        }

        /// <summary>
        /// Get navigation items
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            GlobalNavbarViewModel globalNavbar = new GlobalNavbarViewModel();
            globalNavbar.TopNavBarMenu = navBarRepository.GetNavItems("/Global-Navigation/Top-Navigation");
            globalNavbar.MainNavBarMenu = navBarRepository.GetNavItems("/Global-Navigation/Main-Navigation");
            return View($"~/Components/ViewComponents/Navbar/Navbar.cshtml", globalNavbar);
        }
    }
}
