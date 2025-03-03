using Convenience.org.Models;
using System;
using System.Collections.Generic;

namespace Convenience.org.Repositories.Interfaces;

public interface IFAQRepository
{
    List<FAQItem> GetFAQRepository(List<Guid> webPageGuids);
}
