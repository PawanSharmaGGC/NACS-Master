using Microsoft.AspNetCore.Mvc;

namespace Convenience.org.Components.Widgets.MemberSearchMyDirectory
{
    public class MemberSearchMyDirectoryController : Controller
    {
        public IActionResult Index()
        {
            return ViewComponent("MemberSearchMyDirectory");
        }
    }
}
