﻿using CMS.DataEngine;
using CMS.Helpers;
using CMS.MediaLibrary;
using ConvenienceCares.Interface.Services;
using ConvenienceCares.Models;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

namespace ConvenienceCares.Services;

public class AssetItemService(IInfoProvider<MediaFileInfo> mediaFileInfoProvider, IMediaFileUrlRetriever mediaFileUrlRetriever, IHttpContextAccessor contextAccessor, IProgressiveCache cache) : IAssetItemService
{
    private readonly IInfoProvider<MediaFileInfo> mediaFileInfoProvider = mediaFileInfoProvider;
    private readonly IMediaFileUrlRetriever mediaFileUrlRetriever = mediaFileUrlRetriever;
    private readonly IHttpContextAccessor contextAccessor = contextAccessor;
    private readonly IProgressiveCache cache = cache;

    public async Task<AssetViewModel?> RetrieveMediaFile(AssetRelatedItem? item)
    {
        if (item is null)
        {
            return null;
        }

        var mediaInfo = await mediaFileInfoProvider.GetAsync(item.Identifier);

        if (mediaInfo is null)
        {
            return null;
        }

        var url = mediaFileUrlRetriever.Retrieve(mediaInfo);

        return new(mediaInfo.FileGUID, mediaInfo.FileTitle, url, mediaInfo.FileDescription, mediaInfo.FileExtension);

    }

    public async Task<IReadOnlyList<ImageAssetViewModel?>> RetrieveMediaFileImages(IEnumerable<AssetRelatedItem> items)
    {
        var images = new List<ImageAssetViewModel>(items.Count());

        foreach (var item in items)
        {
            var image = await RetrieveMediaFileImage(item);

            if (image is not null)
            {
                images.Add(image);
            }
        }

        return images;
    }

    public async Task<ImageAssetViewModel?> RetrieveMediaFileImage(AssetRelatedItem? item)
    {
        if (item is null)
        {
            return null;
        }

        var assetItem = await cache.LoadAsync(async cs =>
        {
            var mediaInfo = await mediaFileInfoProvider.GetAsync(item.Identifier);

            if (mediaInfo is null)
            {
                return null;
            }

            cs.GetCacheDependency = () => CacheHelper.GetCacheDependency($"mediafile|{mediaInfo.FileGUID}");

            var url = mediaFileUrlRetriever.Retrieve(mediaInfo);

            if (!url.IsImage)
            {
                return null;
            }

            return new ImageAssetViewModel(mediaInfo.FileGUID, mediaInfo.FileTitle, url, mediaInfo.FileDescription, item.Dimensions, mediaInfo.FileExtension);
        }, new CacheSettings(Constants.DEFAULT_CACHE_MINUTES, $"{nameof(ImageAssetViewModel)}|{item.Identifier}"));

        return assetItem;

    }

    /// <summary>
    /// MediaFileURLProvider.GetMediaFileAbsoluteUrl does not work correctly as of v26.3.1 because it
    /// doesn't include the port or the file extension
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public string BuildFullFileUrl(IMediaFileUrl url)
    {
        var req = contextAccessor.HttpContext?.Request;

        if (req is null)
        {
            return "";
        }

        return BuildFullFileUrl(url, req);
    }

    public static string BuildFullFileUrl(IMediaFileUrl url, HttpRequest request)
    {
        var uriBuilder = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1, TrimLeadingTilde(url.RelativePath));
        if (uriBuilder.Uri.IsDefaultPort)
        {
            uriBuilder.Port = -1;
        }

        return uriBuilder.Uri.AbsoluteUri;
    }

    private static string TrimLeadingTilde(string input)
    {
        var span = input.AsSpan();
        if (span.Length > 0 && span[0] == '~')
        {
            span = span[1..];
        }

        return span.ToString();
    }
}

public static class MediaFileURLExtensions
{
    public static string AbsoluteURL(this IMediaFileUrl url, HttpRequest request) =>
        AssetItemService.BuildFullFileUrl(url, request);
}