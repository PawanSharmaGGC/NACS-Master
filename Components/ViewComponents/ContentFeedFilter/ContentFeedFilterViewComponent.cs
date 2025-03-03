using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.MediaLibrary;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Helpers;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;


namespace Convenience.org.Components.ViewComponents.ContentFeedFilter
{
    public class ContentFeedFilterViewComponent : ViewComponent
    {
        private readonly IContentQueryExecutor executor;
        private readonly IWebsiteChannelContext channelContext;
        private readonly ITaxonomyRetriever taxonomyRetriever;
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;

        public ContentFeedFilterViewComponent(
            IContentQueryExecutor executor,
            ITaxonomyRetriever taxonomyRetriever,
            IWebsiteChannelContext channelContext,
            MediaLibraryHelpers mediaLibraryHelpers)
        {
            this.channelContext = channelContext;
            this.executor = executor;
            this.taxonomyRetriever = taxonomyRetriever;
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ContentFeedFilterViewModel viewModel = await GetVideoData(_mediaLibraryHelpers);
            return View("~/Components/ViewComponents/ContentFeedFilter/ContentFeedFilter.cshtml", viewModel);
        }

        public async Task<ContentFeedFilterViewModel> GetVideoData(MediaLibraryHelpers mediaLibraryHelpers)
        {
            ContentFeedFilterViewModel viewModel = new ContentFeedFilterViewModel
            {
                WebinarType = new List<Tag>(),
                Videos = new List<VideoExtended>()
            };

            var query = new ContentItemQueryBuilder()
                .ForContentType(Video.CONTENT_TYPE_NAME, config => config.OrderBy("Date"));

            IEnumerable<Video> videoData = await executor.GetMappedResult<Video>(query);

            if (videoData != null && videoData.Any())
            {
                foreach (var video in videoData)
                {
                    string altText = string.Empty;

                    var extendedVideo = new VideoExtended
                    {
                        Title = video.Title,
                        Description = video.Description,
                        Location = video.Location,
                        Date = video.Date,
                        vzaarPostingID = video.vzaarPostingID,
                        Duration = video.Duration,
                        SectionHeader = video.SectionHeader,
                        FeatureType = video.FeatureType,
                        Icon = video.Icon,
                        WebinarType = video.WebinarType,
                        Categories = video.Categories,

                        HeaderImagePath = video.HeaderImage?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(video.HeaderImage.First(), ref altText)
                            : string.Empty,
                        RollupImagePath = video.RollupImage?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(video.RollupImage.First(), ref altText)
                            : string.Empty,
                        RollupImageURLPath = video.RollupImageURL?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(video.RollupImageURL.First(), ref altText)
                            : string.Empty,
                        ImagePath = video.Image?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(video.Image.First(), ref altText)
                            : string.Empty
                    };

                    viewModel.Videos.Add(extendedVideo);

                    IEnumerable<Guid> tagIdentifiers = video.WebinarType?
                        .Select(tr => tr.Identifier)
                        .Where(id => id != Guid.Empty) ?? new List<Guid>();

                    if (tagIdentifiers.Any())
                    {
                        IEnumerable<Tag> tags = (await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en")).ToList();
                        var uniqueTags = tags.Where(tag => !viewModel.WebinarType.Any(existing => existing.Identifier == tag.Identifier));
                        viewModel.WebinarType.AddRange(uniqueTags);
                    }
                }
            }

            return viewModel;
        }



    }
}
