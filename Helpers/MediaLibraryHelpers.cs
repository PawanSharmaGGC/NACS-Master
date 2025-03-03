using CMS.DataEngine;
using CMS.MediaLibrary;
using CMS.Websites.Routing;
using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Content.Web.Mvc;
using System;

namespace Convenience.org.Helpers
{
    public class MediaLibraryHelpers
    {
        private readonly IInfoProvider<MediaLibraryInfo> mediaLibraryInfoProvider;
        private readonly IInfoProvider<MediaFileInfo> mediaFileInfoProvider;
        private readonly IWebsiteChannelContext channelContext;
        private readonly IMediaFileUrlRetriever mediaFileURLRetriever;

        public MediaLibraryHelpers(IInfoProvider<MediaLibraryInfo> mediaLibraryInfoProvider, IInfoProvider<MediaFileInfo> mediaFileInfoProvider, IWebsiteChannelContext channelContext, IMediaFileUrlRetriever mediaFileURLRetriever)
        {
            this.mediaLibraryInfoProvider = mediaLibraryInfoProvider;
            this.mediaFileInfoProvider = mediaFileInfoProvider;
            this.channelContext = channelContext;
            this.mediaFileURLRetriever = mediaFileURLRetriever;
        }

        /// <summary>
        /// Get media file from media library using file guid
        /// </summary>
        /// <param name="mediaFileGUID"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public MediaFileInfo GetMediaFile(Guid mediaFileGUID, int siteID)
        {
            return mediaFileInfoProvider.Get(mediaFileGUID);
        }


        /// <summary>
        /// Get media file path
        /// </summary>
        /// <param name="image"></param>
        /// <param name="altText"></param>
        /// <returns></returns>
        public string GetImagePath(AssetRelatedItem image, ref string altText)
        {
            var imageGUID = image?.Identifier ?? Guid.Empty;
            if (imageGUID == Guid.Empty)
            {
                altText = string.Empty;
            }

            var objImage = GetMediaFile(imageGUID, channelContext.WebsiteChannelID);
            if (objImage != null)
            {
                altText = objImage.FileTitle;
                return mediaFileURLRetriever.Retrieve(objImage).RelativePath;
            }

            else
                return string.Empty;
        }

        /// <summary>
        /// Get media file path
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public string GetVideoPath(AssetRelatedItem video)
        {
            var imageGUID = video?.Identifier ?? Guid.Empty;
            var objImage = GetMediaFile(imageGUID, channelContext.WebsiteChannelID);
            if (objImage != null)
            {
                return mediaFileURLRetriever.Retrieve(objImage).RelativePath;
            }

            else
                return string.Empty;
        }
    }
}
