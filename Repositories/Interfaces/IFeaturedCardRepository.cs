using Convenience.org.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Convenience.org.Repositories.Interfaces
{
    public interface IFeaturedCardRepository
    {
        Task<FeaturedContentCardViewModel> GetFeaturedCardRepositoryAsync(List<Guid> WebPageGuids);
    }
}
