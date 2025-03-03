using CMS.ContentEngine;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Convenience.org.Repositories;

public class FAQRepository : IFAQRepository
{
    private readonly IContentQueryExecutor _contentQueryExecutor;

    public FAQRepository(IContentQueryExecutor contentQueryExecutor)
    {
        _contentQueryExecutor = contentQueryExecutor;
    }


    public List<FAQItem> GetFAQRepository(List<Guid> webPageGuids)
    {
        var query = new ContentItemQueryBuilder()
                        .ForContentType(FAQ.CONTENT_TYPE_NAME,
                              config => config
                                .WithLinkedItems(1)
                                .Where(where => where
                                        .WhereIn(nameof(IContentQueryDataContainer.ContentItemGUID), webPageGuids)));

        var content = _contentQueryExecutor.GetMappedResult<FAQ>(query).Result;

        IEnumerable<FAQItem> faqItems = content.Select(i => new FAQItem
        {
            Question =i.Question,
            Answer = i.Answer,
        }).ToList();

        return faqItems.ToList() ?? new List<FAQItem>();
    }
}
