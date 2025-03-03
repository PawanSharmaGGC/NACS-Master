using Convenience.Features.Home;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using NACSShow.Features.Home;

[assembly: RegisterWebPageRoute(
    contentTypeName: Convenience.Home.CONTENT_TYPE_NAME,
    controllerType: typeof(ConvenienceHomeController),
    ActionName = "Index",
    Path = "/Home",
    WebsiteChannelNames = ["Convenience.org"])]
namespace Convenience.Features.Home
{
    public class ConvenienceHomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Features/Home/Convenience/Home.cshtml");
        }
    }
}
