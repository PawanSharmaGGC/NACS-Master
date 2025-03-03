using Convenience.org.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Convenience.org.Repositories.Interfaces
{
    public interface IDeepDiveRepository
    {
        Task<List<DeepDiveCardViewModel>> GetDeepDiveContentRepositoryAsync(IEnumerable<Guid> tagIdentifiers, int topN = 10);
        Task<DeepDiveCardViewModel> GetArticleItemAsync(Guid WebPageGuid);
    }
}
