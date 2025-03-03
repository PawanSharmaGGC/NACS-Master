using System.Threading.Tasks;
using Convenience.org.Components.Widgets.RecommendedCardCarouselWidget;
using Convenience.org.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CMS.ContentEngine;
using System.CodeDom.Compiler;
using System.Linq;
using Convenience.org.Models;
using System;
using CMS.Websites;

[assembly: RegisterWidget(identifier: RecommendedCardCarouselWidget.IDENTIFIER,
    name: "Recommended Card Carousel Widget",
    viewComponentType: typeof(RecommendedCardCarouselWidget),
    propertiesType: typeof(RecommendedCardCarouselWidgetProperties),
    Description = "Recommended Card Carousel Widget",
    IconClass = "icon-navigation", AllowCache = true)]

namespace Convenience.org.Components.Widgets.RecommendedCardCarouselWidget
{
    public class RecommendedCardCarouselWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.RecommendedCardCarousel";
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;
        private readonly IContentQueryExecutor _executor;

        public RecommendedCardCarouselWidget(MediaLibraryHelpers mediaLibraryHelpers,
         IContentQueryExecutor executor)
        {
            _mediaLibraryHelpers = mediaLibraryHelpers;
            _executor = executor;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<RecommendedCardCarouselWidgetProperties> viewModel)
        {
            string imageAltText = string.Empty;

            var model = RecommendedCardCarouselWidgetViewModel.GetViewModel(viewModel.Properties);
            if (model == null)
            {
                model.ArticleItems = new List<ArticleCardViewModel>();
            }

            // get all articles
            var articleCardViewModel = new List<ArticleCardViewModel>();
            List<Guid> pageGuids = viewModel?.Properties?.ArticleItems?
                                                    .Select(i => i.WebPageGuid)
                                                    .ToList();
            if (pageGuids != null && pageGuids.Any())
            {
                // Prepares a query that retrieves article pages matching the selected GUIDs
                var pageQuery = new ContentItemQueryBuilder()
                    .ForContentTypes(parameters =>
                        parameters.ForWebsite(pageGuids).OfContentType(News.CONTENT_TYPE_NAME).WithContentTypeFields());
                IEnumerable<News> articles =
                         _executor.GetMappedWebPageResult<News>(pageQuery)?.Result;

                articleCardViewModel = articles.Select(a => new ArticleCardViewModel()
                {
                    ImageAlttext = a.ArticleImageAltText,
                    ImageUrl = _mediaLibraryHelpers.GetImagePath(a.ArticleImage.FirstOrDefault(), ref imageAltText),
                    Status = a.ArticleTitle,
                    Title = a.ArticleTitle
                }).ToList();
            }
            else
            {
                articleCardViewModel = null;
            }
            model.ArticleItems = articleCardViewModel;

            return View($"~/Components/Widgets/RecommendedCardCarouselWidget/_RecommendedCardCarouselWidget.cshtml", model);
        }
    }
}
