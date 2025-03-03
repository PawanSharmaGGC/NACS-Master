using System.Threading.Tasks;

using Kentico.Content.Web.Mvc.Routing;
using Kentico.Xperience.Admin.Base;
using Microsoft.AspNetCore.Mvc;
using NACSShow.Repositories.Pages;
using NACSShow.Repositories.Pages.Interfaces;

namespace NACSShow.Components.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly INavigationRepository navigationRepository;

        public NavigationMenuViewComponent(INavigationRepository navigationRepository)
        {
            this.navigationRepository = navigationRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var navigationViewModels = await navigationRepository.GetMenuItems();

            return View($"~/Components/ViewComponents/NavigationMenu/Default.cshtml", navigationViewModels);
        }
    }
}
