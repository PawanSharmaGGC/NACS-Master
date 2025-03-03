using System.Threading.Tasks;
using Convenience.org.Models;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Convenience.org.Helpers;
using System.Linq;
using CMS.MediaLibrary;

namespace Convenience.org.Components.Widgets
{
    public class PullQuoteViewComponent: ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<PullQuoteProperties> model)
        {
            string altText = string.Empty;
            var viewModel = new PullQuoteViewModel
            {
                Quote =  model.Properties.Quote,
                AuthorName = model.Properties.AuthorName
            };
            return View("~/Components/Widgets/PullQuote/PullQuote.cshtml", viewModel);
        }
    }
}
