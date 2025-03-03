using CMS.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace NACSShow.Components.ViewComponents;

public class ConveniencePageTitleViewComponent: ViewComponent
{
    public IViewComponentResult Invoke(string sectionHeader, string title)
    {
        var pageTitle = ValidationHelper.GetString(string.IsNullOrEmpty(sectionHeader) ? title : sectionHeader, "");
        return View("~/Components/ViewComponents/ConveniencePageTitle/ConveniencePageTitle.cshtml", pageTitle);
    }
}
