using CMS.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace NACSShow.Components.ViewComponents.FullWidthTextPageTitle;

public class FullWidthPageTitleViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string sectionHeader, string title)
    {
        var pageTitle = ValidationHelper.GetString(string.IsNullOrEmpty(sectionHeader) ? title : sectionHeader, "");
        return View("~/Components/ViewComponents/FullWidthTextPageTitle/FullWidthPageTitle.cshtml", pageTitle);
    }
}
