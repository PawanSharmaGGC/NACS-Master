using CMS.ContentEngine;
using CMS.Websites.Routing;
using NACS.Portal.Core.Services;
using System.Collections.Generic;
using Convenience.org.Repositories.Interfaces;
using Convenience.org.Models;
using CMS.Websites;
using System.Linq;
using System;
using CMS.Core;

namespace Convenience.org.Repositories;

public class TestimonialsRepository : ITestimonialsRepository
{
    private readonly IContentQueryExecutor _executor;
    private readonly IWebsiteChannelContext _channelContext;
    private readonly IEventLogService _eventLogService;

    public TestimonialsRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext,
            IAssetItemService itemService)
    {
        _executor = executor;
        _channelContext = channelContext;
    }

    public IEnumerable<Testimonial> GetTestimonialsRepository(List<Guid> webPageGuids)
    {
        try
        {
            var query = new ContentItemQueryBuilder()
                        .ForContentTypes(parameters =>
                            parameters.ForWebsite(webPageGuids).OfContentType(Testimonials.CONTENT_TYPE_NAME).WithContentTypeFields());

            var testimonials = _executor.GetMappedWebPageResult<Testimonials>(query)?.Result;

            IEnumerable<Testimonial> testimonialItems = testimonials.Select(i => new Testimonial
            {
                Text = i.Text,
                Author = i.Author
            }).ToList();

            return testimonialItems ?? Enumerable.Empty<Testimonial>();
        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(TestimonialsRepository), nameof(GetTestimonialsRepository), ex);
            return Enumerable.Empty<Testimonial>();
        }
    }
}
