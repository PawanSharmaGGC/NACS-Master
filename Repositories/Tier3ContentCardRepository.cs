using Convenience.org.Components.Widgets;
using System.Collections.Generic;
using System;
using Convenience.org.Repositories.Interfaces;
using CMS.ContentEngine;
using System.CodeDom.Compiler;
using System.Linq;
using System.Threading.Tasks;
using CMS.Websites;

namespace Convenience.org.Repositories
{
    public class Tier3ContentCardRepository : ITier3ContentCardRepository
    {
        private readonly IContentQueryExecutor executor;
        public Tier3ContentCardRepository(IContentQueryExecutor executor)
        {
            this.executor = executor;
        }

        public async Task<IEnumerable<CommunityNewsContent>> GetCommunityNewsAsync(IEnumerable<Guid> communityNewsGuids)
        {
            if (communityNewsGuids == null || !communityNewsGuids.Any())
                return Enumerable.Empty<CommunityNewsContent>();

            var query = new ContentItemQueryBuilder()
                .ForContentType(CommunityNewsContent.CONTENT_TYPE_NAME, config =>
                    config
                   .ForWebsite("Convenience.org").
                    Where(where => where
                        .WhereIn(nameof(IContentQueryDataContainer.ContentItemGUID), communityNewsGuids)));

            return await executor.GetMappedResult<CommunityNewsContent>(query);
        }
    }
}
