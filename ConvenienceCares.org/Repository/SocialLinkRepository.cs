using CMS.ContentEngine;
using CMS.Helpers;
using CMS.MediaLibrary;
using CMS.Websites;
using CMS.Websites.Routing;

using ConvenienceCares.Models;

using Nacs;

using NACS.Portal.Core.Models;
using NACS.Portal.Core.Services;


namespace ConvenienceCares.Repository;

public class SocialLinkRepository : ContentRepositoryBase
{
    private readonly ILinkedItemsDependencyRetriever linkedItemsDependencyRetriever;
    private readonly IAssetItemService itemService;

    public SocialLinkRepository(IWebsiteChannelContext websiteChannelContext, IContentQueryExecutor executor,
        IProgressiveCache cache, ILinkedItemsDependencyRetriever linkedItemsDependencyRetriever, IAssetItemService itemService)
        : base(websiteChannelContext, executor, cache)
    {
        this.linkedItemsDependencyRetriever = linkedItemsDependencyRetriever;
        this.itemService = itemService;
    }


    /// <summary>
    /// Returns list of <see cref="SocialLinkViewModel"/> content items.
    /// </summary>
    public async Task<List<SocialLinkViewModel>> GetSocialLinks(string languageName, CancellationToken cancellationToken = default)
    {
        var socialLinkItems = new List<SocialLinkViewModel>();
        var queryBuilder = GetQueryBuilder(languageName);

        var cacheSettings = CreateCacheSettings<SocialLink>(nameof(SocialLinkRepository), nameof(GetSocialLinks), languageName);

        var socialLinks = await GetCachedQueryResult<SocialLink>(queryBuilder, new ContentQueryExecutionOptions(), cacheSettings, GetDependencyCacheKeys, cancellationToken);

        if (socialLinks != null && socialLinks.Any())
        {
            var icons = await itemService.RetrieveMediaFileImages(socialLinks.SelectMany(x => x.SocialLinkIcon ?? []).Where(icon => icon != null));
            socialLinkItems = socialLinks.Select(x => new SocialLinkViewModel(
                x.SocialLinkTitle ?? string.Empty,
                x.SocialLinkUrl ?? string.Empty,
                icons.FirstOrDefault(i => x.SocialLinkIcon?.Any(s => s.Identifier == i?.ID) == true) ?? new ImageAssetViewModel()
            )).ToList();
        }

        return socialLinkItems ?? new List<SocialLinkViewModel>();
    }


    private ContentItemQueryBuilder GetQueryBuilder(string languageName)
    {
        return new ContentItemQueryBuilder()
                .ForContentType(SocialLink.CONTENT_TYPE_NAME, config => config
                 .ForWebsite(WebsiteChannelContext.WebsiteChannelName)
                 .WithLinkedItems(1))
                .InLanguage(languageName);
    }


    private Task<ISet<string>> GetDependencyCacheKeys(IEnumerable<SocialLink> socialLinks, CancellationToken cancellationToken)
    {
        var dependencyCacheKeys = CreateCacheKeys(socialLinks.Select(socialLink => socialLink.SystemFields.ContentItemID), "contentitem")
            .Concat(linkedItemsDependencyRetriever.Get(socialLinks.Select(link => link.SystemFields.ContentItemID), 1))
            .Append(CacheHelper.GetCacheItemName(null, ContentLanguageInfo.OBJECT_TYPE, "all"))
            .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

        return Task.FromResult<ISet<string>>(dependencyCacheKeys);
    }

}
