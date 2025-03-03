using CMS.Websites;
using Convenience.org.Components.Widgets.EventSpeakerCard;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

[assembly: RegisterWidget(identifier: EventSpeakerCardWidget.IDENTIFIER,
    propertiesType: typeof(EventSpeakerCardProperties), viewComponentType: typeof(EventSpeakerCardWidget),
    name: "Event Speaker Card", Description = "Event Speaker Card",
    IconClass = "icon-engage-users", AllowCache = true)]

namespace Convenience.org.Components.Widgets.EventSpeakerCard;

public class EventSpeakerCardWidget : ViewComponent
{
    public const string IDENTIFIER = "EventSpeakerCard";
    private readonly IPersonBioRepository personBioRepository;
    private readonly IWebPageUrlRetriever urlRetriever;
    private readonly IPreferredLanguageRetriever languageRetriever;

    public EventSpeakerCardWidget(IPersonBioRepository personBioRepository, IWebPageUrlRetriever urlRetriever, IPreferredLanguageRetriever languageRetriever)
    {
        this.personBioRepository = personBioRepository;
        this.urlRetriever = urlRetriever;
        this.languageRetriever = languageRetriever;
    }

    public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<EventSpeakerCardProperties> widgetProperties)
    {
        var model = PersonBioViewModel.GetViewModel();

        if (widgetProperties != null)
        {
            var pageGuids = widgetProperties?.Properties?.SelectedSpeaker.Select(i => i.WebPageGuid).ToList();

            if (pageGuids != null && pageGuids.Any())
            {
                var languageName = languageRetriever.Get();
                var pageUrl = await urlRetriever.Retrieve(pageGuids.FirstOrDefault(), languageName);

                var speaker = personBioRepository.GetPersonBioRepository(pageGuids);
                model.PersonBioItem = speaker;
            }
        }

        return View("~/Components/Widgets/EventSpeakerCard/EventSpeakerCard.cshtml", model);
    }
}
