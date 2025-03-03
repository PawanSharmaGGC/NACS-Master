using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Convenience.org.Components.Widgets.Cards.RelatedContentCard;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(
    identifier: RelatedContentCardWidget.IDENTIFIER,
    name: "Related Content Card",
    viewComponentType: typeof(RelatedContentCardWidget),
    propertiesType: typeof(RelatedContentCardWidgetProperties),
    Description = "Related Content Card",
    IconClass = "icon-scheme-connected-circles",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Cards.RelatedContentCard
{
    public class RelatedContentCardWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.RelatedContentCard";
        private readonly IMapper _mapper;
        private readonly IDeepDiveRepository _deepDiveRepository;

        public RelatedContentCardWidget(IMapper mapper, IDeepDiveRepository deepDiveRepository)
        {
            _mapper = mapper;
            _deepDiveRepository = deepDiveRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<RelatedContentCardWidgetProperties> model)
        {
            var vm = _mapper.Map<RelatedContentCardWidgetViewModel>(model?.Properties);
            if (model.Properties.SelectedArticle != null && model.Properties.SelectedArticle.Count() > 0)
            {
              var artilceItem = await _deepDiveRepository.GetArticleItemAsync(model.Properties.SelectedArticle.FirstOrDefault().WebPageGuid);
                if (artilceItem != null)
                {
                    vm.ArticleCardItem = artilceItem;
                }
            }
            else
            {
                vm = null;
            }

            return View("~/Components/Widgets/Cards/RelatedContentCard/_RelatedContentCardWidget.cshtml", vm);
        }

    }
}
