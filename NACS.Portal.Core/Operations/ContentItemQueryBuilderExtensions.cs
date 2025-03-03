﻿using CMS.Websites.Routing;
using CMS.Websites;
using Kentico.Content.Web.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.ContentEngine
{
    public static class ContentItemQueryBuilderExtensions
    {
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, IWebsiteChannelContext channelContext, RoutedWebPage page) =>
        builder
            .ForContentType(page.ContentTypeName, queryParameters =>
            {
                _ = queryParameters
                    .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemID), page.WebPageItemID))
                    .ForWebsite(channelContext.WebsiteChannelName)
                    .TopN(1);
            })
            .InLanguage(page.LanguageName);
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, IWebsiteChannelContext channelContext, RoutedWebPage page, Action<ContentTypeQueryParameters> configureParameters) =>
            builder
                .ForContentType(page.ContentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemID), page.WebPageItemID))
                        .ForWebsite(channelContext.WebsiteChannelName)
                        .TopN(1);

                    configureParameters(queryParameters);
                })
                .InLanguage(page.LanguageName);
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, string channelName, RoutedWebPage page) =>
            builder
                .ForContentType(page.ContentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemID), page.WebPageItemID))
                        .ForWebsite(channelName)
                        .TopN(1);
                })
                .InLanguage(page.LanguageName);
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, string channelName, RoutedWebPage page, Action<ContentTypeQueryParameters> configureParameters) =>
            builder
                .ForContentType(page.ContentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemID), page.WebPageItemID))
                        .ForWebsite(channelName)
                        //.WithLinkedItems(2)
                        .TopN(1);

                    configureParameters(queryParameters);
                })
                .InLanguage(page.LanguageName);
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, IWebsiteChannelContext channelContext, string contentTypeName, int webPageID) =>
            builder
                .ForContentType(contentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemID), webPageID))
                        .ForWebsite(channelContext.WebsiteChannelName);
                });
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, IWebsiteChannelContext channelContext, string contentTypeName, int webPageID, Action<ContentTypeQueryParameters> configureParameters) =>
            builder
                .ForContentType(contentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemID), webPageID))
                        .ForWebsite(channelContext.WebsiteChannelName);

                    configureParameters(queryParameters);
                });
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, IWebsiteChannelContext channelContext, string contentTypeName, Guid webPageGUID) =>
            builder
                .ForContentType(contentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemGUID), webPageGUID))
                        .ForWebsite(channelContext.WebsiteChannelName)
                        .TopN(1);
                });
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, IWebsiteChannelContext channelContext, string contentTypeName, Guid webPageGUID, Action<ContentTypeQueryParameters> configureParameters) =>
            builder
                .ForContentType(contentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemGUID), webPageGUID))
                        .ForWebsite(channelContext.WebsiteChannelName)
                        .TopN(1);

                    configureParameters(queryParameters);
                });
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, string channelName, string contentTypeName, Guid webPageGUID) =>
            builder
                .ForContentType(contentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemGUID), webPageGUID))
                        .ForWebsite(channelName)
                        .TopN(1);
                });
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, string channelName, string contentTypeName, Guid webPageGUID, Action<ContentTypeQueryParameters> configureParameters) =>
            builder
                .ForContentType(contentTypeName, queryParameters =>
                {
                    _ = queryParameters
                        .Where(w => w.WhereEquals(nameof(WebPageFields.WebPageItemGUID), webPageGUID))
                        .TopN(1)
                        .ForWebsite(channelName);

                    configureParameters(queryParameters);
                });
        public static ContentItemQueryBuilder ForWebPage(this ContentItemQueryBuilder builder, string contentTypeName, Guid webPageGUID, Action<ContentTypesQueryParameters> configureParameters) =>
            builder
                .ForContentTypes(p =>
                configureParameters(p
                    .OfContentType(contentTypeName)
                    .WithContentTypeFields()
                    .ForWebsite()))
            .Parameters(p => p
                .Where(w => w.WhereEquals(nameof(IWebPageFieldsSource.SystemFields.WebPageItemGUID), webPageGUID)));

    }
}