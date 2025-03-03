using CMS.Helpers;
using ConvenienceCares.Interface.Services;
using Kentico.Content.Web.Mvc.Routing;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NACS.Portal.Core.Services;
using ConvenienceCares.Repository;

namespace ConvenienceCares.Components.ViewComponents.MenuItem;

public class MenuItemViewComponent : ViewComponent
{
    private readonly IMenuItemService menuItemService;
    private readonly IPreferredLanguageRetriever currentLanguageRetriever;
    private readonly IMediator mediator;
    private readonly WebSiteSettingsRepository webSiteSettingsRepository;
    private readonly IAssetItemService itemService;

   public MenuItemViewComponent(IMenuItemService menuItemService, 
        IPreferredLanguageRetriever currentLanguageRetriever, IMediator mediator, WebSiteSettingsRepository webSiteSettingsRepository,
        IAssetItemService itemService)
    {
        this.menuItemService = menuItemService;
        this.currentLanguageRetriever = currentLanguageRetriever;
        this.mediator = mediator;
        this.webSiteSettingsRepository = webSiteSettingsRepository;
        this.itemService = itemService;
    }

    /// <summary>
    /// Return menu items
    /// </summary>
    /// <returns></returns>
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var languageName = currentLanguageRetriever.Get();

        //var websiteSettings = await mediator.Send(new WebsiteSettingsQuery());

        var websiteSettings = await webSiteSettingsRepository.GetWebSiteSettingsAsync(languageName, cancellationToken: HttpContext.RequestAborted);
        
        var logo = itemService.RetrieveMediaFileImage(websiteSettings?.HeaderLogo?.FirstOrDefault()).GetAwaiter().GetResult();
        ViewBag.HeaderLogo = logo?.URLData?.RelativePath;

        var menuFolderPath = ValidationHelper.GetString(websiteSettings?.NavigationMenuFolderPath, Constants.NAVIGATION_MENU_FOLDER_PATH);

        var menuItems = await menuItemService.GetMenuItemViewModels(languageName, menuFolderPath, HttpContext.RequestAborted);

        return View("~/Components/ViewComponents/MenuItem/MenuItem.cshtml", menuItems);
    }
}


