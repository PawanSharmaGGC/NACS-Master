using Convenience;
﻿using ConvenienceCares.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceCares.Components.ViewComponents.PageTitle;

public class PageTitleViewComponent : ViewComponent
{
	public IViewComponentResult Invoke(ConveniencePageViewModel pageModel) =>
		View("~/Components/ViewComponents/Convenience/PageTitle/PageTitle.cshtml", pageModel);
}
