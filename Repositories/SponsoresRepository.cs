using CMS.ContentEngine;
using CMS.Websites.Routing;
using NACS.Portal.Core.Services;
using System.Collections.Generic;
using System;
using Convenience.org.Models;
using CMS.Websites;
using Convenience.org.Repositories.Interfaces;
using System.Linq;
using CMS.Core;

namespace Convenience.org.Repositories;

public class SponsoresRepository : ISponsoresRepository
{
    private readonly IContentQueryExecutor _executor;
    private readonly IWebsiteChannelContext _channelContext;
    private readonly IAssetItemService _itemService;
    private readonly IEventLogService _eventLogService;

    public SponsoresRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext,
            IAssetItemService itemService, IEventLogService eventLogService)
    {
        _executor = executor;
        _channelContext = channelContext;
        _itemService = itemService;
        _eventLogService = eventLogService;
    }

    public List<SponsorItem> GetSponsoresRepository(List<Guid> webPageGuids)
    {
        List<SponsorItem> model = new List<SponsorItem>();

        try
        {
            var pageQuery = new ContentItemQueryBuilder()
                    .ForContentTypes(parameters =>
                        parameters.ForWebsite(webPageGuids).OfContentType(Sponsores.CONTENT_TYPE_NAME).WithContentTypeFields());

            IEnumerable<Sponsores> events =
                     _executor.GetMappedWebPageResult<Sponsores>(pageQuery)?.Result;

            foreach (var item in events)
            {
                var images = item.Image != null ? _itemService.RetrieveMediaFileImages(item.Image)?.Result : null;

                model.Add(new SponsorItem
                {
                    Image = images != null ? images.Select(s => _itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty,
                    ImageAlt = item.ImageAlt,
                    SponsorName = item.SponsorName,
                }
                    );
            }
        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(SponsoresRepository), nameof(GetSponsoresRepository), ex);
        }
        return model;
    }
}
