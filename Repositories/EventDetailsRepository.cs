using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.MediaLibrary;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using NACS.Portal.Core.Services;
using NACSMagazine;

namespace Convenience.org.Repositories
{
    public class EventDetailsRepository : IEventDetailsRepository
    {
        private readonly IContentQueryExecutor _executor;
        private readonly IWebsiteChannelContext _channelContext;
        private readonly IAssetItemService _itemService;

        public EventDetailsRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext,
                IAssetItemService itemService)
        {
            _executor = executor;
            _channelContext = channelContext;
            _itemService = itemService;
        }

        public List<EventCardItem> GetEventDetailsRepository(List<Guid> WebPageGuids)
        {
            List<EventCardItem> model = new List<EventCardItem>();

            // Prepares a query that retrieves article pages matching the selected GUIDs
            var pageQuery = new ContentItemQueryBuilder()
                    .ForContentTypes(parameters =>
                        parameters.ForWebsite(WebPageGuids).OfContentType(EventDetails.CONTENT_TYPE_NAME).WithContentTypeFields());

            // Retrieves the data of the selected article pages
            IEnumerable<EventDetails> events =
                     _executor.GetMappedWebPageResult<EventDetails>(pageQuery)?.Result;

            foreach (var item in events)
            {
                var images = item.EventImage != null ? _itemService.RetrieveMediaFileImages(item.EventImage)?.Result : null;

                //var asseetItem = MediaFileInfoObjectQueryExtensions.ForAssets();
                model.Add(new EventCardItem
                {
                    Image = images != null ? images.Select(s => _itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty,
                    ImageAlt = images.FirstOrDefault()?.AltText,
                    Title = item.EventTitle,
                    ShortDescription = item.EventSummary,
                    StartDate = item.EventStartDate.ToString(),
                    EndDate = item.EventEndDate.ToString(),
                }
                    );
            }

            return model;

        }
    }
}
