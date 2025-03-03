using Kentico.PageBuilder.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Convenience.org.Components.Widgets
{
    public class TableViewComponent: ViewComponent
    {
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<TableProperties> model)
        {
            string altText = string.Empty;
            var viewModel = new TableViewModel
            {
                TableContent = model.Properties.TableContent
            };
            return View("~/Components/Widgets/Table/Table.cshtml", viewModel);
        }
    }
}
