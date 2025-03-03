using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSMagazine.Components.ViewComponents.Breadcrumbs
{
    public class BreadcrumbsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View("/Components/ViewComponents/Breadcrumbs/Breadcrumbs.cshtml");
    }
}
