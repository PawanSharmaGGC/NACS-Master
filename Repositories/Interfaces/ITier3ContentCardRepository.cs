using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convenience.org.Components.Widgets;
using Convenience.org.Models;

namespace Convenience.org.Repositories.Interfaces
{
    public interface ITier3ContentCardRepository
    {
        Task<IEnumerable<CommunityNewsContent>> GetCommunityNewsAsync(IEnumerable<Guid> communityNewsGuids);
    }
}
