using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience;

namespace ConvenienceCares.Repository;

public class ConveniencePageRepository : ContentRepositoryBase
{
    public ConveniencePageRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor, IProgressiveCache cache)
        : base(websiteChannelContext, executor, cache)
    {
    }

    /// <summary>
    /// Returns <see cref="Page"/> content item.
    /// </summary>
    /// <param name="webPageItemId">Web page item ID.</param>
    /// <param name="languageName">Language name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<Page?> GetConveniencePageAsync(int webPageItemId, string languageName, CancellationToken cancellationToken = default)
    {
        var queryBuilder = GetQueryBuilder(webPageItemId, languageName);

        var cacheSettings = CreateCacheSettingsByKey<Page>(languageName, webPageItemId.ToString());

        var result = await GetCachedQueryResult<Page>(queryBuilder, new ContentQueryExecutionOptions(), cacheSettings, GetDependencyCacheKeys, cancellationToken);

        return result.FirstOrDefault();
    }


    private ContentItemQueryBuilder GetQueryBuilder(int webPageItemId, string languageName)
    {
        return new ContentItemQueryBuilder()
                .ForContentType(Page.CONTENT_TYPE_NAME, config => config
                    .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                    .Where(where => where
                        .WhereEquals(nameof(IWebPageContentQueryDataContainer.WebPageItemID), webPageItemId))
                    .TopN(1))
                .InLanguage(languageName);
    }


    private static Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<Page> webPages, CancellationToken cancellationToken)
    {
        var dependencyCacheKeys = new HashSet<string>();

        var webPage = webPages.FirstOrDefault();

        if (webPage != null)
        {
            dependencyCacheKeys.Add(CacheHelper.BuildCacheItemName(new[] { "webpageitem", "byid", webPage.SystemFields.WebPageItemID.ToString() }, false));
        }

        return Task.FromResult<ISet<string>>(dependencyCacheKeys);
    }

}
