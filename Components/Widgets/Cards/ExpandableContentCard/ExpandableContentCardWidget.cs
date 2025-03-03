using System.Linq;
using AutoMapper;
using Convenience.org.Components.Widgets.Cards.ExpandableContentCard;
using Convenience.org.Components.Widgets.Cards.Tier1ContentCard;
using Convenience.org.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(
    identifier: ExpandableContentCardWidget.IDENTIFIER,
    name: "Expandable Content Card",
    viewComponentType: typeof(ExpandableContentCardWidget),
    propertiesType: typeof(ExpandableContentCardWidgetProperties),
    Description = "Expandable Content Card",
    IconClass = "icon-edge",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Cards.ExpandableContentCard
{
    public class ExpandableContentCardWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.ExpandableContentCard";
        private readonly IMapper _mapper;
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;

        public ExpandableContentCardWidget(IMapper mapper, MediaLibraryHelpers mediaLibraryHelpers)
        {
            _mapper = mapper;
            _mediaLibraryHelpers = mediaLibraryHelpers;
        }

        /// <summary>
        /// Retrive the widget data from the admin configuration for the Expandable Content Card Widget.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IViewComponentResult Invoke(ComponentViewModel<ExpandableContentCardWidgetProperties> model)
        {
            string imageAltText = string.Empty;
            var vm = _mapper.Map<ExpandableContentCardWidgetViewModel>(model?.Properties);

            vm.ImageUrl = _mediaLibraryHelpers.GetImagePath(model.Properties.Image.FirstOrDefault(), ref imageAltText);
            vm.ImageAltText = imageAltText;

            return View("~/Components/Widgets/Cards/ExpandableContentCard/_ExpandableContentCardWidget.cshtml", vm);
        }

    }
}
