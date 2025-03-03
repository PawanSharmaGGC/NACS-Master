using Convenience.org.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Convenience.org.Components.ViewComponents
{
	public class SocialShareViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync()
		{
			string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

			if (currentUrl != null)
			{
				var model = SocialShareViewModel.GetViewModel(currentUrl);
				return View("~/Components/ViewComponents/SocialShare/SocialShare.cshtml", model);
			}

			return View("~/Components/ViewComponents/SocialShare/SocialShare.cshtml", new SocialShareViewModel());
		}
	}
}
