using Convenience.org.Components.Widgets.SponsoresWidget;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterWidget(identifier: SponsoresWidget.IDENTIFIER, name: "Sponsores",
    viewComponentType: typeof(SponsoresWidget),
    propertiesType: typeof(SponsoresWidgetProperties), Description = "Sponsores",
    IconClass = "icon-heartshake", AllowCache = true)]

namespace Convenience.org.Components.Widgets.SponsoresWidget
{
    public class SponsoresWidget : ViewComponent
    {
        public const string IDENTIFIER = "Sponsores";
        private readonly ISponsoresRepository sponsoresRepository;

        public SponsoresWidget(ISponsoresRepository sponsoresRepository)
        {
            this.sponsoresRepository = sponsoresRepository;
        }

        public IViewComponentResult Invoke(ComponentViewModel<SponsoresWidgetProperties> widgetProperties)
        {

            // Retrieves the GUIDs of the selected pages from the 'Pages' property
            List<Guid> pageGuids = widgetProperties?.Properties?.Sponsores?
                                                        .Select(i => i.WebPageGuid)
                                                        .ToList();
            var model = SponsoresViewModel.GetViewModel(widgetProperties?.Properties);
            if (pageGuids != null && pageGuids.Any()){
                var sponsores = sponsoresRepository.GetSponsoresRepository(pageGuids);
                model.SponsorItems = sponsores;
            }

            return View("~/Components/Widgets/SponsoresWidget/Sponsores.cshtml", model);
        }
    }
}
