using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CMS.Core;
using Convenience.org.Components.Widgets.DOHAttendees;
using Convenience.org.Components.Widgets.EventCarousel;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

[assembly: RegisterWidget(identifier: DOHAttendeesWidget.IDENTIFIER,
    name: "DOH Attendees",
    viewComponentType: typeof(DOHAttendeesWidget),
    propertiesType: typeof(DOHAttendeesWidgetProperties),
    Description = "DOH Attendees",
    IconClass = "icon-users", AllowCache = true)]

namespace Convenience.org.Components.Widgets.DOHAttendees
{
    public class DOHAttendeesWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.DOHAttendees";
        private readonly IMapper _mapper;
        private readonly IEventLogService _eventLogService;
        private readonly IEventPageRepository _eventPageRepository;

        public DOHAttendeesWidget(IMapper mapper, IEventLogService eventLogService, IEventPageRepository eventPageRepository)
        {
            _mapper = mapper;
            _eventLogService = eventLogService;
            _eventPageRepository = eventPageRepository;
        }

        /// <summary>
        /// Get DOH widget details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IViewComponentResult Invoke(ComponentViewModel<DOHAttendeesWidgetProperties> model)
        {
            List<NACSAttendeeViewModel> attendees = _eventPageRepository.GetData();

            var attendeeList = new List<NACSAttendeeViewModel>();
            if (attendees != null && (attendees.Count > 19 || System.Configuration.ConfigurationManager.AppSettings["Environment"] == "DEV"))
            {
                attendees = attendees.OrderBy(a => a.CompanyName).ToList();
            }

            var vm = _mapper.Map<DOHAttendeesWidgetViewModel>(model.Properties);
            vm.Attendies = attendees == null ? null : new StaticPagedList<NACSAttendeeViewModel>(attendees, 1, 20, attendees?.Count ?? 0);
            vm.AttendiesCount = attendees?.Count??0;

            return View("~/Components/Widgets/DOHAttendees/_DOHAttendeesWidget.cshtml", vm);
        }

    }
}
