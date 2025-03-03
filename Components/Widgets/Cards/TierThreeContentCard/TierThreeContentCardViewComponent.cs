using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CMS.ContentEngine;
using CMS.MediaLibrary;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Convenience.org.Helpers;
using CMS.Websites;
using NACSMagazine;
using CMS.Base;
using Convenience.org.Models.Cards;

namespace Convenience.org.Components.Widgets.Cards
{
    public class TierThreeContentCardViewComponent : ViewComponent
    {
        private readonly ITier3ContentCardRepository _repository;
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;
        private readonly IContentQueryExecutor _executor;
        private readonly IWebPageUrlRetriever urlRetriever;

        private readonly IComponentPropertiesRetriever componentPropertiesRetriever;
        public TierThreeContentCardViewComponent(ITier3ContentCardRepository repository, MediaLibraryHelpers mediaLibraryHelpers, IContentQueryExecutor executor, IWebPageUrlRetriever urlRetriever)
        {
            _repository = repository;
            _mediaLibraryHelpers = mediaLibraryHelpers;
            _executor = executor;
            this.urlRetriever = urlRetriever;
        }
        public async Task<ViewViewComponentResult> InvokeAsync(ComponentViewModel<TierThreeContentCardProperties> model)
        {
            var builder = new ContentItemQueryBuilder()
                        .ForContentType(
                CommunityNewsPage.CONTENT_TYPE_NAME,
                            config => config
                                .ForWebsite(
                                    "Convenience",
                                    PathMatch.Children("/Community_News"))).InLanguage("en");

            IEnumerable<CommunityNewsPage> pages = await _executor.GetMappedWebPageResult<CommunityNewsPage>(builder);

            List<Guid> pageGuids = model?.Properties?.SelectedCommunityNews?
                                           .Select(i => i.WebPageGuid)
                                           .ToList();

            var pageQuery = new ContentItemQueryBuilder()
                    .ForContentType(CommunityNewsPage.CONTENT_TYPE_NAME, config => config
                        .WithLinkedItems(1)
                        .ForWebsite("Convenience")
                        .Where(where => where.WhereIn(nameof(IWebPageContentQueryDataContainer.WebPageItemGUID), pageGuids)));

            var communityNews = await _executor.GetMappedResult<CommunityNewsPage>(pageQuery);
            var viewModel = await GetCommunityNews(communityNews, model.Properties, _mediaLibraryHelpers);
            return View("~/Components/Widgets/Cards/TierThreeContentCard/TierThreeContentCard.cshtml", viewModel);
        }
        public async Task<TierThreeContentCardViewModel> GetCommunityNews(IEnumerable<CommunityNewsPage> communityNews, TierThreeContentCardProperties model, MediaLibraryHelpers mediaLibraryHelpers)
        {
            var vms = new List<CommunityNewsViewModel>();
            foreach (var page in communityNews)
            {
                var post = page.SelectCommunityNews.FirstOrDefault();
                var pageUrl = await urlRetriever.Retrieve(page);
                string altText = string.Empty;
                var authorImagePath = string.Empty;

                if (post.AuthorImage != null && post.AuthorImage.Any())
                {
                    authorImagePath = mediaLibraryHelpers.GetImagePath(post.AuthorImage.First(), ref altText);
                }
                vms.Add(new CommunityNewsViewModel
                {
                    Title = post.Title,
                    Description = post.Description,
                    SubTitle = post.SubTitle,
                    AuthorName = post.AuthorName,
                    AuthorImage = post.AuthorImage,
                    PublishDate = post.PublishDate,
                    AuthorImagePath = authorImagePath,
                    ImageAltText = altText = post.AuthorName,
                    PageUrl = pageUrl.RelativePath
                });
            }
            return new TierThreeContentCardViewModel
            {
                CommunityNews = vms ?? Enumerable.Empty<CommunityNewsViewModel>(),
                CTAText = model?.CTAText,
                CTALeftIconVisible = model?.CTALeftIconVisible ?? false,
                EyebrowTitle = model?.EyebrowTitle,
            };
        }
    }
}
