using CMS.ContentEngine;
using CMS.Core;
using CMS.MediaLibrary;
using CMS.Websites.Routing;
using CMS.Websites;
using Convenience.org.Models;
using Kentico.Content.Web.Mvc.Routing;
using NACS.Portal.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Convenience.org.Repositories.Interfaces;
using System.Linq;
using Microsoft.CodeAnalysis;
using CMS.Helpers;

namespace Convenience.org.Repositories;

public class DeepDiveRepository : IDeepDiveRepository
{
    private readonly IContentQueryExecutor _executor;
    private readonly IWebsiteChannelContext _channelContext;
    private readonly IWebPageUrlRetriever _urlRetriever;
    private readonly IPreferredLanguageRetriever _languageRetriever;
    private readonly IAssetItemService _itemService;
    private readonly IEventLogService _eventLogService;
    private readonly ITaxonomyRetriever _taxonomyRetriever;

    public DeepDiveRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext,
        IWebPageUrlRetriever urlRetriever, IPreferredLanguageRetriever languageRetriever,
        IAssetItemService itemService, IEventLogService eventLogService, ITaxonomyRetriever taxonomyRetriever)
    {
        _executor = executor;
        _channelContext = channelContext;
        _urlRetriever = urlRetriever;
        _languageRetriever = languageRetriever;
        _itemService = itemService;
        _eventLogService = eventLogService;
        _taxonomyRetriever = taxonomyRetriever;
    }

    public async Task<DeepDiveCardViewModel> GetArticleItemAsync(Guid WebPageGuid)
    {
        var deepDiveItems = new DeepDiveCardViewModel();
        try
        {
            // fetch data from content hub for article
            var hubItemBuilder =
            new ContentItemQueryBuilder()
                        .ForContentTypes(parameters =>
                        {
                            parameters
                            //.ForWebsite(_channelContext.WebsiteChannelName)
                            .OfContentType(Article.CONTENT_TYPE_NAME)
                            .WithContentTypeFields().WithLinkedItems(1);
                        })
                        .Parameters(parameter =>
                        {
                            parameter.Where(i => i.WhereEquals("ContentItemGUID", WebPageGuid));
                        });
            var hubContentItems = await _executor.GetMappedResult<IContentItemFieldsSource>(hubItemBuilder);

            // fetch data from pages for eventpage
            var pageItembuilder = new ContentItemQueryBuilder()
                                .ForContentTypes(parameters =>
                                {
                                    parameters.ForWebsite(_channelContext.WebsiteChannelName).WithContentTypeFields().WithLinkedItems(1);
                                })
                                .Parameters(parameter =>
                                {
                                    parameter.Where(i => i.WhereEquals("WebPageItemGuid", WebPageGuid));
                                });
            //new ContentItemQueryBuilder()
            //            .ForContentTypes(parameters =>
            //            {
            //                parameters
            //                .ForWebsite(_channelContext.WebsiteChannelName)
            //                .OfContentType(EventPage.CONTENT_TYPE_NAME)
            //                .WithContentTypeFields().WithLinkedItems(1);
            //            })
            //            .Parameters(parameter =>
            //            {
            //                parameter.Where(i => i.WhereEquals("WebPageItemGUID", WebPageGuid));
            //            });

            var pagesContentItems = await _executor.GetMappedResult<IContentItemFieldsSource>(pageItembuilder);
            var contentItems = pagesContentItems.Concat(hubContentItems).Take(1);

            if (contentItems == null && contentItems.FirstOrDefault() == null)
            {
                return deepDiveItems;
            }

            var languageName = _languageRetriever.Get();

            foreach (var contentItem in contentItems)
            {
                var itemImageProperty = GetPropertyValue<IEnumerable<AssetRelatedItem>>(contentItem, "Image");
                var systemFields = GetPropertyValue<ContentItemFields>(contentItem, "SystemFields");

                var itemId = ValidationHelper.GetInteger(GetPropertyValue<int>(systemFields, "WebPageItemID"), 0);

                var itemImages = itemImageProperty != null ? _itemService.RetrieveMediaFileImages(itemImageProperty)?.Result : null;
                var pageUrl = itemId == 0 ? "#" : _urlRetriever.Retrieve(itemId, languageName).GetAwaiter().GetResult().RelativePath;

                deepDiveItems = new DeepDiveCardViewModel
                {
                    Title = ValidationHelper.GetString(GetPropertyValue<string>(contentItem, "Title"), ""),
                    Description = ValidationHelper.GetString(GetPropertyValue<string>(contentItem, "Description"), ""),
                    Image = itemImages != null ? itemImages.Select(s => _itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty,
                    ItemPageUrl = ValidationHelper.GetString(pageUrl, ""),
                    ImageAltText = ValidationHelper.GetString(GetPropertyValue<string>(contentItem, "ImageAltText"), "")
                };
            }
        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(DeepDiveRepository), nameof(GetArticleItemAsync), ex);
        }

        return deepDiveItems;
    }

    public async Task<List<DeepDiveCardViewModel>> GetDeepDiveContentRepositoryAsync(IEnumerable<Guid> tagIdentifiers, int topN = 10)
    {
        var deepDiveItems = new List<DeepDiveCardViewModel>();
        try
        {
            // fetch data from content hub for article
            var hubItemBuilder =
            new ContentItemQueryBuilder()
                        .ForContentTypes(parameters =>
                        {
                            parameters
                            //.ForWebsite(_channelContext.WebsiteChannelName)
                            .OfContentType(Article.CONTENT_TYPE_NAME)
                            .WithContentTypeFields().WithLinkedItems(1);
                        })
                        .Parameters(parameter =>
                        {
                            parameter.Where(i => i.WhereContainsTags("ContentCategory", tagIdentifiers)).TopN(topN);
                        });
            var hubContentItems = await _executor.GetMappedResult<IContentItemFieldsSource>(hubItemBuilder);

            // fetch data from pages for eventpage
            var pageItembuilder =
            new ContentItemQueryBuilder()
                        .ForContentTypes(parameters =>
                        {
                            parameters
                            .ForWebsite(_channelContext.WebsiteChannelName)
                            .OfContentType(EventPage.CONTENT_TYPE_NAME)
                            .WithContentTypeFields().WithLinkedItems(1);
                        })
                        .Parameters(parameter =>
                        {
                            parameter.Where(i => i.WhereContainsTags("ContentCategory", tagIdentifiers)).TopN(topN);
                        });

            var pagesContentItems = await _executor.GetMappedResult<IContentItemFieldsSource>(pageItembuilder);
            var contentItems = pagesContentItems.Concat(hubContentItems).Take(topN);

            if (contentItems == null && contentItems.FirstOrDefault() == null)
            {
                return deepDiveItems;
            }

            var languageName = _languageRetriever.Get();

            foreach (var contentItem in contentItems)
            {
                var itemImageProperty = GetPropertyValue<IEnumerable<AssetRelatedItem>>(contentItem, "Image");
                var systemFields = GetPropertyValue<ContentItemFields>(contentItem, "SystemFields");

                var itemId = ValidationHelper.GetInteger(GetPropertyValue<int>(systemFields, "WebPageItemID"), 0);

                var itemImages = itemImageProperty != null ? _itemService.RetrieveMediaFileImages(itemImageProperty)?.Result : null;
                var pageUrl = itemId == 0 ? "#" : _urlRetriever.Retrieve(itemId, languageName).GetAwaiter().GetResult().RelativePath;

                deepDiveItems.Add(new DeepDiveCardViewModel
                {
                    Title = ValidationHelper.GetString(GetPropertyValue<string>(contentItem, "Title"), ""),
                    Description = ValidationHelper.GetString(GetPropertyValue<string>(contentItem, "Description"), ""),
                    Image = itemImages != null ? itemImages.Select(s => _itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty,
                    ItemPageUrl = ValidationHelper.GetString(pageUrl, ""),
                    ImageAltText = ValidationHelper.GetString(GetPropertyValue<string>(contentItem, "ImageAltText"), "")
                });
            }
        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(FeaturedCardRepository), nameof(GetDeepDiveContentRepositoryAsync), ex);
        }

        return deepDiveItems;
    }


    private T GetPropertyValue<T>(object obj, string propertyName)
    {
        var propertyInfo = obj.GetType().GetProperty(propertyName);
        if (propertyInfo == null)
        {
            return default(T);
        }

        var value = propertyInfo.GetValue(obj);
        if (value is T result)
        {
            return result;
        }

        return default(T);
    }
}
