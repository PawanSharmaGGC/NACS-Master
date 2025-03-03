using Microsoft.AspNetCore.Mvc;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Convenience.org.Helpers;
using Convenience.org.Components.Widgets.Cards.Tier1ContentCard;

[assembly: RegisterWidget(
    identifier: Tier1ContentCardWidget.IDENTIFIER,
    name: "Tier 1 Content Card",
    viewComponentType: typeof(Tier1ContentCardWidget),
    propertiesType: typeof(Tier1ContentCardWidgetProperties),
    Description = "Tier 1 Content Card",
    IconClass = "icon-l-img-3-cols-3",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Cards.Tier1ContentCard
{
    public class Tier1ContentCardWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.Tier1ContentCard";
        protected readonly IEventDetailsRepository _eventDetailsRepository;
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;


        public Tier1ContentCardWidget(IEventDetailsRepository eventDetailsRepository,
            MediaLibraryHelpers mediaLibraryHelpers)
        {
            _eventDetailsRepository = eventDetailsRepository;
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }

        public IViewComponentResult Invoke(ComponentViewModel<Tier1ContentCardWidgetProperties> model)
        {
            string imageAltText = string.Empty;
            var vm =new  Tier1ContentCardWidgetViewModel();

            if (model?.Properties?.EventItems == null || model?.Properties?.EventItems.Count() == 0)
            {
                vm = Tier1ContentCardWidgetViewModel.GetViewModel(model?.Properties);

                // Retive image using media helper
                vm.ImageUrl = _mediaLibraryHelpers.GetImagePath(model?.Properties?.Image?.FirstOrDefault(), ref imageAltText);
                vm.ImageAltText = imageAltText;
            }
            else
            {
                // Retrieves the GUIDs of the selected pages from the 'Pages' property
                List<Guid> pageGuids = model?.Properties?.EventItems?
                                                            .Select(i => i.WebPageGuid)
                                                            .ToList();
                var events = _eventDetailsRepository.GetEventDetailsRepository(pageGuids);
                var aerticleItem = events?.FirstOrDefault();
                if (aerticleItem != null)
                {
                    vm.ImageUrl = aerticleItem.Image;
                    vm.Title = aerticleItem.Title;
                    vm.Description = aerticleItem.ShortDescription;
                    vm.DateTime = aerticleItem?.StartDate;
                    vm.LocationOrReadTime = aerticleItem?.LocationOrReadTime;
                    vm.EyebrowTitle = model.Properties.EyebrowTitle;

                    DateTime startDate;
                    DateTime.TryParse(aerticleItem?.StartDate, out startDate);

                    DateTime endDate;
                    DateTime.TryParse(aerticleItem?.EndDate, out endDate);

                    if (DateTime.Now < startDate)
                    {
                        vm.EyebrowStatus = "Upcomming";
                    }
                    else if (startDate > DateTime.Now && DateTime.Now > endDate)
                    {
                        vm.EyebrowStatus = "New";
                    }
                    else
                    {
                        vm.EyebrowStatus = "Ended";
                    }
                }
            }

            return View("~/Components/Widgets/Cards/Tier1ContentCard/Tier1ContentCard.cshtml", vm);
        }
    }
}
