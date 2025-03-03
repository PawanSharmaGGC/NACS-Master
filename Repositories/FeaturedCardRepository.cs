using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.Core;
using CMS.MediaLibrary;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.Content.Web.Mvc.Routing;
using NACS.Portal.Core.Services;

namespace Convenience.org.Repositories
{
    public class FeaturedCardRepository : IFeaturedCardRepository
    {
        private readonly IContentQueryExecutor _executor;
        private readonly IWebsiteChannelContext _channelContext;
        private readonly IWebPageUrlRetriever _urlRetriever;
        private readonly IPreferredLanguageRetriever _languageRetriever;
        private readonly IAssetItemService _itemService;
        private readonly IEventLogService _eventLogService;

        public FeaturedCardRepository(IContentQueryExecutor executor, IWebsiteChannelContext channelContext,
            IWebPageUrlRetriever urlRetriever, IPreferredLanguageRetriever languageRetriever,
            IAssetItemService itemService, IEventLogService eventLogService)
        {
            _executor = executor;
            _channelContext = channelContext;
            _urlRetriever = urlRetriever;
            _languageRetriever = languageRetriever;
            _itemService = itemService;
            _eventLogService = eventLogService;
        }

        public async Task<FeaturedContentCardViewModel> GetFeaturedCardRepositoryAsync(List<Guid> webPageGuids)
        {
            var model = FeaturedContentCardViewModel.GetViewModel();
            try
            {
                var builder = new ContentItemQueryBuilder()
                                .ForContentTypes(parameters =>
                                {
                                    parameters.ForWebsite(_channelContext.WebsiteChannelName).WithContentTypeFields().WithLinkedItems(1);
                                })
                                .Parameters(parameter =>
                                {
                                    parameter.Where(i => i.WhereEquals("WebPageItemGuid", webPageGuids.FirstOrDefault()));
                                });

                var contentItems = await _executor.GetMappedResult<IContentItemFieldsSource>(builder);
                var pageContent = contentItems.FirstOrDefault();


                if (pageContent == null)
                {
                    return model;
                }

                var languageName = _languageRetriever.Get();
                var pageUrl = await _urlRetriever.Retrieve(webPageGuids.FirstOrDefault(), languageName);

                var contentType = pageContent.GetType().FullName;

                if (contentType == "Convenience.ArticlePage")
                {
                    var articles = GetPropertyValue<IEnumerable<Article>>(pageContent, "ArticleContentItem");

                    if (articles != null)
                    {
                        foreach (var article in articles)
                        {
                            await ProcessArticle(model, article, pageUrl);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _eventLogService.LogException(nameof(FeaturedCardRepository), nameof(GetFeaturedCardRepositoryAsync), ex);
            }

            return model;
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

        private async Task ProcessArticle(FeaturedContentCardViewModel model, object article, WebPageUrl pageUrl)
        {
            try
            {
                model.Title = GetPropertyValue<string>(article, "Title");
                model.AuthorName = GetPropertyValue<string>(article, "Author");
                var date = GetPropertyValue<DateTime>(article, "Date");
                model.PublishedDate = date == default ? "" : date.ToString("MMMM dd, yyyy");
                model.PageContent = GetPropertyValue<string>(article, "PageContent");
                model.ImageAltText = GetPropertyValue<string>(article, "ImageAltText");

                model.CTA.ButtonURL = pageUrl.RelativePath;

                var authorImageProperty = GetPropertyValue<IEnumerable<AssetRelatedItem>>(article, "AuthorImage");
                var authorImages = authorImageProperty != null ? _itemService.RetrieveMediaFileImages(authorImageProperty)?.Result : null;
                model.Author.ImgSrc = authorImages != null ? authorImages.Select(s => _itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty;
                model.Author.Name = model.AuthorName;

                var imageProperty = GetPropertyValue<IEnumerable<AssetRelatedItem>>(article, "Image");
                var images = imageProperty != null ? _itemService.RetrieveMediaFileImages(imageProperty)?.Result : null;
                model.Image = images != null ? images.Select(s => _itemService.BuildFullFileUrl(s.URLData)).FirstOrDefault() : string.Empty;
            }
            catch (Exception ex)
            {
                _eventLogService.LogException(nameof(FeaturedCardRepository), nameof(ProcessArticle), ex);
            }
        }


    }
}
