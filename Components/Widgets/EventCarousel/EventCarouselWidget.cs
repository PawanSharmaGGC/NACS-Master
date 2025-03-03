using System;
using System.Collections.Generic;
using System.Linq;
using Convenience.org.Components.Widgets.EventCarousel;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(identifier: EventCarouselWidget.IDENTIFIER,
    name: "Event Carousel", 
    viewComponentType: typeof(EventCarouselWidget), 
    propertiesType: typeof(EventCarouselWidgetProperties), 
    Description = "Event Carousel", 
    IconClass = "icon-navigation", AllowCache = true)]

namespace Convenience.org.Components.Widgets.EventCarousel
{
    public class EventCarouselWidget : ViewComponent
    {
        public const string IDENTIFIER = "EventCarousel";
        protected readonly IEventDetailsRepository _eventDetailsRepository;
     
        public EventCarouselWidget(IEventDetailsRepository eventDetailsRepository)
        {
            _eventDetailsRepository = eventDetailsRepository;
        }

        public IViewComponentResult Invoke(ComponentViewModel<EventCarouselWidgetProperties> model)
        {
            // Retrieves the GUIDs of the selected pages from the 'Pages' property
            List<Guid> pageGuids = model?.Properties?.EventItems?
                                                        .Select(i => i.WebPageGuid)
                                                        .ToList();
            var vm = EventCarouselViewModel.GetViewModel(model?.Properties);
            var events = _eventDetailsRepository.GetEventDetailsRepository(pageGuids);
            vm.EventItems = events;

            return View("~/Components/Widgets/EventCarousel/EventCarousel.cshtml", vm);
        }
    }
}
