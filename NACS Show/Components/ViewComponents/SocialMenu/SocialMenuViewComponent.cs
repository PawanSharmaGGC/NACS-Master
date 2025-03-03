using System.Threading.Tasks;

using Kentico.Content.Web.Mvc.Routing;
using Kentico.Xperience.Admin.Base;
using Microsoft.AspNetCore.Mvc;
using NACSShow.Repositories.Pages;
using NACSShow.Repositories.Pages.Interfaces;

namespace NACSShow.Components.ViewComponents
{
	public class SocialMenuViewComponent : ViewComponent
	{
		private readonly INavigationRepository navigationRepository;

		public SocialMenuViewComponent(INavigationRepository navigationRepository, IPreferredLanguageRetriever currentLanguageRetriever)
		{
			this.navigationRepository = navigationRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var navigationViewModel = await navigationRepository.GetSocialMenuItems();

			return View($"~/Components/ViewComponents/SocialMenu/Default.cshtml", navigationViewModel);
		}
	}
}
