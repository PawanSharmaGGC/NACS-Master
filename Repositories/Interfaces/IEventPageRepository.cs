using System;
using System.Collections.Generic;
using Convenience.org.Models;

namespace Convenience.org.Repositories.Interfaces
{
    public interface IEventPageRepository
    {
        List<EventPage> GetEventsRepository(List<Guid> WebPageGuids);

        List<NACSAttendeeViewModel> GetData();
        List<NACSAttendeeViewModel> GetFilteredData(string sortby, out int totalItems, int pageNumber = 1, int pageSize = 20);
    }
}
