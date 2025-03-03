using Convenience.org.Components.Widgets.EventDetail;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterWidget(identifier: EventDetailWidget.IDENTIFIER,
    propertiesType: typeof(EventDetailWidgetProperties), viewComponentType: typeof(EventDetailWidget),
    name: "Event Detail", Description = "Event Detail",
    IconClass = "icon-event", AllowCache = true)]

namespace Convenience.org.Components.Widgets.EventDetail;

public class EventDetailWidget : ViewComponent
{
    public const string IDENTIFIER = "EventDetail";
    private readonly IEventPageRepository eventPageRepository;

    public EventDetailWidget(IEventPageRepository eventPageRepository)
    {
        this.eventPageRepository = eventPageRepository;
    }

    public IViewComponentResult Invoke(ComponentViewModel<EventDetailWidgetProperties> widgetProperties)
    {
        List<Guid> pageGuids = widgetProperties?.Properties?.EventDetail?
                                                    .Select(i => i.WebPageGuid)
                                                    .ToList();
        var model = EventDetailViewModel.GetViewModel(widgetProperties?.Properties);
        if (pageGuids != null && pageGuids.Any())
        {
            var events = eventPageRepository.GetEventsRepository(pageGuids);
            var eventDetail = events.FirstOrDefault();

            model.Eyebrow.Title = model.Eyebrow.Title ?? eventDetail.Title;

            var startTime = eventDetail.StartDate == default ? null : eventDetail.StartDate.ToShortTimeString();
            var endTime = eventDetail.EndDate == default ? null : eventDetail.EndDate.ToShortTimeString();

            model.EventTiming = string.Join(" - ", new[] { startTime, endTime }.Where(time => !string.IsNullOrEmpty(time)));

            model.EventDate = eventDetail.StartDate == default ? "" : eventDetail.StartDate.ToString("MMMM dd, yyyy");
            model.EventStartTime = eventDetail.StartDate.ToString();
            model.EventAddress = string.Join(" | ",
                            new[] { eventDetail.Address, eventDetail.City, eventDetail.State, eventDetail.Country }
                            .Where(part => !string.IsNullOrEmpty(part)));
            model.EventLocation = eventDetail.Location;
        }
        return View("~/Components/Widgets/EventDetail/EventDetail.cshtml", model);
    }
}
