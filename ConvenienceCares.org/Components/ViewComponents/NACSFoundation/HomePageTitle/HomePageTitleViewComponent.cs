using Convenience;
﻿using ConvenienceCares.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceCares.Components.ViewComponents.HomePageTitle;

public class HomePageTitleViewComponent : ViewComponent
{
	//public IViewComponentResult Invoke(Page pageModel) =>
	public IViewComponentResult Invoke(ConveniencePageViewModel pageModel) =>
		View("~/Components/ViewComponents/NACSFoundation/HomePageTitle/HomePageTitle.cshtml", pageModel);
}
