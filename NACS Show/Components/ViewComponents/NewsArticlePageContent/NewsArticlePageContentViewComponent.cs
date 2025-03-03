using Microsoft.AspNetCore.Mvc;

namespace NACSShow.Components.ViewComponents;

public class NewsArticlePageContentViewComponent :ViewComponent
{
    public IViewComponentResult Invoke(string pageContent) =>
        View("~/Components/ViewComponents/NewsArticlePageContent/NewsArticlePageContent.cshtml", pageContent);
}
