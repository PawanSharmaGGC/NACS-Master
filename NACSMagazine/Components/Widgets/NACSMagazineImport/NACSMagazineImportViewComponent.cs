using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using NACSMagazine.Components.Widgets.NACSMagazineImport;

[assembly: RegisterWidget("NACSMagazineImport", typeof(NACSMagazineImportViewComponent), "Magazine Import Widget", Description = "Uploads an issue and creates articles.", IconClass = "icon-upload")]

namespace NACSMagazine.Components.Widgets.NACSMagazineImport
{
    public class NACSMagazineImportViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Components/Widgets/NACSMagazineImport/_NACSMagazineImport.cshtml", new NACSMagazineImportViewModel());
        }
    }
}
