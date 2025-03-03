using CMS.ContentEngine;
using CMS.Core;
using Convenience.org.Components.Widgets.DeepDive;
using Convenience.org.Models;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Kentico.Content.Web.Mvc.Routing;
using NACS.Portal.Core.Services;
using Convenience.org.Repositories.Interfaces;
using CMS.Helpers;

[assembly: RegisterWidget(identifier: DeepDiveWidget.IDENTIFIER,
    propertiesType: typeof(DeepDiveWidgetProperties), viewComponentType: typeof(DeepDiveWidget),
    name: "Deep Dive", Description = "Deep Dive",
    IconClass = "icon-deep", AllowCache = true)]

namespace Convenience.org.Components.Widgets.DeepDive;

public class DeepDiveWidget : ViewComponent
{
    public const string IDENTIFIER = "DeepDiveWidget";
    private readonly IEventLogService _eventLogService;
    private readonly ITaxonomyRetriever _taxonomyRetriever;
    private readonly IPreferredLanguageRetriever _languageRetriever;
    private readonly IAssetItemService _itemService;
    private readonly IDeepDiveRepository _deepDiveRepository;

    public DeepDiveWidget(IEventLogService eventLogService, ITaxonomyRetriever taxonomyRetriever,
        IPreferredLanguageRetriever languageRetriever, IAssetItemService itemService, IDeepDiveRepository deepDiveRepository)
    {
        _eventLogService = eventLogService;
        _taxonomyRetriever = taxonomyRetriever;
        _languageRetriever = languageRetriever;
        _itemService = itemService;
        _deepDiveRepository = deepDiveRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<DeepDiveWidgetProperties> widgetProperties)
    {
        var model = DeepDiveViewModel.GetViewModel();

        try
        {
            model.TopN = ValidationHelper.GetInteger(widgetProperties.Properties.TopN, 10);
            var languageName = _languageRetriever.Get();
            // Gets identifiers of the selected tags from the annotated property
            IEnumerable<Guid> tagIdentifiers = widgetProperties?.Properties?.SelectedTags.Select(item => item.Identifier);

            // Retrieves a collection of Tag object corresponding to the selected tags.
            IEnumerable<Tag> tags = await _taxonomyRetriever.RetrieveTags(tagIdentifiers, languageName);
            model.Tags = tags.OrderBy(tag => tag.Title);

            model.Title = widgetProperties.Properties.Title;
            model.CardCTAText = widgetProperties.Properties.CardCTAText;

            var bannerImage = _itemService.RetrieveMediaFileImage(widgetProperties?.Properties?.BannerImage?.FirstOrDefault()).Result;
            model.BannerImage = bannerImage?.URLData?.RelativePath;

            var lightImage = _itemService.RetrieveMediaFileImage(widgetProperties?.Properties?.LightImage?.FirstOrDefault()).Result;
            model.LightImage = lightImage?.URLData?.RelativePath;

            var defaultSelectedTag = tags.OrderBy(tag => tag.Title).FirstOrDefault().Identifier;
            var defaultTagIdentifier = new List<Guid> { defaultSelectedTag };
            model.DeepDiveCardItems = await _deepDiveRepository.GetDeepDiveContentRepositoryAsync(defaultTagIdentifier, widgetProperties.Properties.TopN);


        }
        catch (Exception ex)
        {
            _eventLogService.LogException(nameof(DeepDiveWidget), nameof(InvokeAsync), ex);
        }
        return View("~/Components/Widgets/DeepDive/DeepDive.cshtml", model);
    }
}
