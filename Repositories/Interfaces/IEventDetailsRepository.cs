using System;
using System.Collections.Generic;
using Convenience.org.Models;

namespace Convenience.org.Repositories.Interfaces
{
    public interface IEventDetailsRepository
    {
        List<EventCardItem> GetEventDetailsRepository(List<Guid> WebPageGuid);
    }
}
