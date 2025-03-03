using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.MediaLibrary;
using CMS.Websites;
using CMS.Websites.Routing;
using Convenience.org.Helpers;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;


namespace Convenience.org.Components.ViewComponents.TagsCluster
{
    public class TagsClusterViewComponent : ViewComponent
    {
        private readonly IContentQueryExecutor executor;
        private readonly IWebsiteChannelContext channelContext;
        private readonly ITaxonomyRetriever taxonomyRetriever;
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;

        public TagsClusterViewComponent(
            IContentQueryExecutor executor,
            ITaxonomyRetriever taxonomyRetriever,
            IWebsiteChannelContext channelContext,
            MediaLibraryHelpers mediaLibraryHelpers)
        {
            this.channelContext = channelContext;
            this.executor = executor;
            this.taxonomyRetriever = taxonomyRetriever;
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            TagsClusterViewModel viewModel = await GetTagData(_mediaLibraryHelpers);
            return View("~/Components/ViewComponents/TagsCluster/TagsCluster.cshtml", viewModel);
        }

        public async Task<TagsClusterViewModel> GetTagData(MediaLibraryHelpers mediaLibraryHelpers)
        {
            TagsClusterViewModel viewModel = new TagsClusterViewModel
            {
                TagCategory = new List<Tag>(),
                Articles = new List<ArticleExtended>()
            };

            var query = new ContentItemQueryBuilder()
                .ForContentType(Article.CONTENT_TYPE_NAME, config => config.OrderBy("Date"));

            IEnumerable<Article> TagData = await executor.GetMappedResult<Article>(query);

            if (TagData != null && TagData.Any())
            {
                foreach (var Article in TagData)
                {
                    string altText = string.Empty;

                    var extendedArticle = new ArticleExtended
                    {
                        Title = Article.Title,
                        Description = Article.Description,
                        SectionHeader = Article.SectionHeader,
                        Comments = Article.Comments,
                        Date = Article.Date,
                        NavType = Article.NavType,
                        NavShowChildren = Article.NavShowChildren,  
                        LeftNavShowTitle = Article.LeftNavShowTitle,
                        LeftNavTitleOverride = Article.LeftNavTitleOverride,    
                        SendOwnerReminderEmails = Article.SendOwnerReminderEmails,  
                        PageContent = Article.PageContent,
                        ImageAltText = Article.ImageAltText,
                        Author = Article.Author,
                        ContentCategory = Article.ContentCategory,

                        HeaderImagePath = Article.HeaderImage?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(Article.HeaderImage.First(), ref altText)
                            : string.Empty,
                        RollupImagePath = Article.RollupImage?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(Article.RollupImage.First(), ref altText)
                            : string.Empty,
                        RollupImageURLPath = Article.RollupImageURL?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(Article.RollupImageURL.First(), ref altText)
                            : string.Empty,
                        ImagePath = Article.Image?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(Article.Image.First(), ref altText)
                            : string.Empty,
                        AuthorImagePath = Article.AuthorImage?.Any() == true
                            ? mediaLibraryHelpers.GetImagePath(Article.AuthorImage.First(), ref altText)
                            : string.Empty
                    };

                    viewModel.Articles.Add(extendedArticle);

                    IEnumerable<Guid> tagIdentifiers = Article.ContentCategory?
                        .Select(tr => tr.Identifier)
                        .Where(id => id != Guid.Empty) ?? new List<Guid>();

                    if (tagIdentifiers.Any())
                    {
                        IEnumerable<Tag> tags = (await taxonomyRetriever.RetrieveTags(tagIdentifiers, "en")).ToList();
                        var uniqueTags = tags.Where(tag => !viewModel.TagCategory.Any(existing => existing.Identifier == tag.Identifier));
                        viewModel.TagCategory.AddRange(uniqueTags);
                    }
                }
            }
            return viewModel;
        }



    }
}
