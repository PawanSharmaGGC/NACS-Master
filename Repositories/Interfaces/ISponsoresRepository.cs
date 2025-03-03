using System.Collections.Generic;
using System;
using Convenience.org.Models;

namespace Convenience.org.Repositories.Interfaces;

public interface ISponsoresRepository
{
    List<SponsorItem> GetSponsoresRepository(List<Guid> WebPageGuids);
}
