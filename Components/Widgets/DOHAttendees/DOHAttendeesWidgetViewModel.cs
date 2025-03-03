using System.Collections.Generic;
using Convenience.org.Models;
using X.PagedList;

namespace Convenience.org.Components.Widgets.DOHAttendees
{
    public class DOHAttendeesWidgetViewModel
    {
        public string EventKey { get; set; } = "6632678b-39fb-4c0e-a2fd-27226141c451";
        
        public string EventYear { get; set; }
        
        public int AttendiesCount { get; set; }

        //Pagination
        public int? pageSize;
        public int sortBy;
        public bool isAsc { get; set; }

        public StaticPagedList<NACSAttendeeViewModel> Attendies { get; set; }
    }
}
