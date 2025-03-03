
using Microsoft.Extensions.Configuration;

using NACS.Portal.Core.Infrastructure.Search;
using NACSMagazine.PageTemplates.SearchPage;
using NACSShow.Services.Search.SessionSearch;
using NACSShow.Services.Search.SiteSearch;
using NACSShow.Services.Search.SpeakerSearch;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionLuceneSearchExtensions
{
    public static IServiceCollection AddAppLuceneSearch(this IServiceCollection services, IConfiguration config) =>
        services
            .AddKenticoLucene(builder =>
            {
                _ = builder
                    .RegisterStrategy<ArticleSearchIndexModel.ArticleSearchIndexingStrategy>(ArticleSearchIndexModel.ArticleSearchIndexingStrategy.IDENTIFIER);


                _ = builder
                    .RegisterStrategy<SessionSearchIndexModel.SessionSearchIndexingStrategy>(SessionSearchIndexModel.SessionSearchIndexingStrategy.IDENTIFIER);

                _ = builder
                    .RegisterStrategy<SiteSearchIndexModel.SiteSearchIndexingStrategy>(SiteSearchIndexModel.SiteSearchIndexingStrategy.IDENTIFIER);

                _ = builder
                    .RegisterStrategy<SpeakerSearchIndexModel.SpeakerSearchIndexingStrategy>(SpeakerSearchIndexModel.SpeakerSearchIndexingStrategy.IDENTIFIER);
            })
            .AddSingleton<WebScraperHtmlSanitizer>()
            .AddHttpClient<WebCrawlerService>()
            .Services
            .AddSingleton<ArticleSearchService>()
            .AddSingleton<SessionSearchService>()
           .AddSingleton<SiteSearchService>()
           .AddSingleton<SpeakerSearchService>()
            .Configure<NACSLuceneSearchOptions>(config.GetSection("Kentico.Xperience.Lucene.Custom"));
}
