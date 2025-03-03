using CMS.ContentEngine;
using CMS.Helpers;
using CMS.Websites;
using CMS.Websites.Routing;

namespace ConvenienceCares.Repository;

public class WebSiteSettingsRepository : ContentRepositoryBase
{

    public WebSiteSettingsRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor,
        IProgressiveCache cache)
        : base(websiteChannelContext, executor, cache)
    {
    }

	public async Task<WebSiteSettings> GetWebSiteSettingsAsync(string languageName, CancellationToken cancellationToken = default)
	{
		
		var queryBuilder = GetQueryBuilder(languageName);

		var cacheSettings = new CacheSettings(Constants.DEFAULT_CACHE_MINUTES, WebsiteChannelContext.WebsiteChannelName, nameof(GetWebSiteSettingsAsync), languageName);

		var queryOptions = new ContentQueryExecutionOptions(); // Create appropriate options if needed

		var result = await GetCachedQueryResult<WebSiteSettings>(queryBuilder, queryOptions, cacheSettings, GetDependencyCacheKeys, cancellationToken);

		return result?.FirstOrDefault() ?? new WebSiteSettings();
	}

    private ContentItemQueryBuilder GetQueryBuilder(string languageName)
    {
        return new ContentItemQueryBuilder()
                .ForContentType(WebSiteSettings.CONTENT_TYPE_NAME, config => config
                .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                .TopN(1))
                .InLanguage(languageName);
    }

    private static Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<WebSiteSettings> webPages, CancellationToken cancellationToken)
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
