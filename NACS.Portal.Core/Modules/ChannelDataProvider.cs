using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Websites;

using NACS.Portal.Core.Operations;

namespace NACS.Portal.Core;

public interface IChannelDataProvider
{
    Task<string?> GetChannelNameByWebsiteChannelID(int websiteChannelID);
}

public class ChannelDataProvider(
    IInfoProvider<ChannelInfo> channelProvider,
    IProgressiveCache cache, 
    ICacheDependencyKeysBuilder keysBuilder)
    : IChannelDataProvider
{
    private readonly IInfoProvider<ChannelInfo> channelProvider = channelProvider;
    private readonly IProgressiveCache cache = cache;
    private readonly ICacheDependencyKeysBuilder keysBuilder = keysBuilder;

    public Task<string?> GetChannelNameByWebsiteChannelID(int websiteChannelID) =>
        cache.LoadAsync(cs => channelProvider.Get()
            .Source(s => s.Join<WebsiteChannelInfo>(nameof(ChannelInfo.ChannelID), nameof(WebsiteChannelInfo.WebsiteChannelChannelID)))
            .WhereEquals(nameof(WebsiteChannelInfo.WebsiteChannelID), websiteChannelID)
            .Columns(nameof(ChannelInfo.ChannelName))
            .GetScalarResultAsync<string?>(), new(30, [nameof(ChannelDataProvider), nameof(GetChannelNameByWebsiteChannelID)])
            {
                CacheDependency = CacheHelper.GetCacheDependency($"{ChannelInfo.OBJECT_TYPE}|all")
            });
}
