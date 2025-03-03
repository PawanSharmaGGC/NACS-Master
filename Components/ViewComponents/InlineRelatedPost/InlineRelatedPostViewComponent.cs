using Convenience.org.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Convenience.org.Components.ViewComponents.InlineRelatedPost;

public class InlineRelatedPostViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var model = GetTestModel();

        return View("~/Components/ViewComponents/InlineRelatedPost/InlineRelatedPost.cshtml", model);
    }

    #region"Code Test"

    private IEnumerable<InlineRelatedPostViewModel> GetTestModel()
    {
        var caption = "Related Post";
        var imagePath = "/static/media/inline.8d74feea4de9a5cfa1e8.png";
        var title = "My Favorite Resturant Served Gas - Episode 434";
        var date = "24 March 2024";
        var listenTime = "10min Listen";

        var items = new List<InlineRelatedPostViewModel> {
        new InlineRelatedPostViewModel{PostCaption=caption,Image=imagePath,Title=title,Url = "#", Date=date,ListenTime=listenTime },
        new InlineRelatedPostViewModel{PostCaption=caption,Image=imagePath,Title=title,Url = "#", Date=date,ListenTime=listenTime },
        new InlineRelatedPostViewModel{PostCaption=caption,Image=imagePath,Title=title,Url = "#", Date=date,ListenTime=listenTime },
        new InlineRelatedPostViewModel{PostCaption=caption,Image=imagePath,Title=title,Url = "#", Date=date,ListenTime=listenTime },
        new InlineRelatedPostViewModel{PostCaption=caption,Image=imagePath,Title=title,Url = "#", Date=date,ListenTime=listenTime }
        };

        return items;
    }

    #endregion
}
