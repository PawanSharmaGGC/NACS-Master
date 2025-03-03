using Microsoft.AspNetCore.Mvc;

namespace NACSShow.Components.ViewComponents.VideoPageContents;

public class VideoPageContentsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Video pageModel) =>
        View("~/Components/ViewComponents/VideoPageContents/VideoPageContents.cshtml", pageModel);
}
