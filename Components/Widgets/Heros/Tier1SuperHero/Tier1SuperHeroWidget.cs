using Convenience.org.Components.Widgets.Heros.Tier1GlassSuperHeroCard;
using System.Threading.Tasks;
using Convenience.org.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Convenience.org.Components.Widgets.Heros.Tier1SuperHero;
using Kentico.PageBuilder.Web.Mvc;
using System;
using System.Collections.Generic;
using Convenience.org.Repositories.Interfaces;

[assembly: RegisterWidget(identifier: Tier1SuperHeroWidget.IDENTIFIER,
    name: "Tier 1 Super Hero Widget",
    viewComponentType: typeof(Tier1SuperHeroWidget),
    propertiesType: typeof(Tier1SuperHeroWidgetProperties),
    Description = "Tier 1 Super Hero Widget",
    IconClass = "icon-l-img-3-cols-3",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Heros.Tier1SuperHero
{
    public class Tier1SuperHeroWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.Tier1SuperHero";
        private readonly MediaLibraryHelpers mediaLibraryHelpers;
        private readonly IEventPageRepository eventPageRepository;


        public Tier1SuperHeroWidget(MediaLibraryHelpers _mediaLibraryHelpers,
            IEventPageRepository _eventPageRepository)
        {
            mediaLibraryHelpers = _mediaLibraryHelpers;
            eventPageRepository = _eventPageRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync(Tier1SuperHeroWidgetProperties properties)
        {
            string imageAltText = string.Empty;
            Tier1SuperHeroWidgetViewModel viewModel = Tier1SuperHeroWidgetViewModel.GetViewModel(properties);

            if (properties.ArticleItems != null && properties.ArticleItems.Count() > 0)
            {
                //need to implement the code to get the evnt details from event page once "Event carousel" branch will be merged. 
                //For now copying the same else part code to handle the output in result.
                // Retrieves the GUIDs of the selected pages from the 'Pages' property
                List<Guid> pageGuids = properties?.ArticleItems?
                                                            .Select(i => i.WebPageGuid)
                                                            .ToList();

                var selectedEvent = eventPageRepository.GetEventsRepository(pageGuids);
                if (selectedEvent != null)
                {
                    viewModel.ArticleCards = selectedEvent.Select(e => new CardItemViewModel()
                    {
                        EyebrowStatus = e.EventStatus,
                        EyebrowTitle = e.Title,
                        Title = e.Title,
                        PageUrl = e.SystemFields.WebPageUrlPath,
                    }).ToList();
                }

                viewModel.ImageUrl = mediaLibraryHelpers.GetImagePath(properties.Image.FirstOrDefault(), ref imageAltText);
                viewModel.ImageAltText = imageAltText;
            }
            else
            {

                viewModel.ImageUrl = mediaLibraryHelpers.GetImagePath(properties.Image.FirstOrDefault(), ref imageAltText);
                viewModel.ImageAltText = imageAltText;
            }

            return View($"~/Components/Widgets/Heros/Tier1SuperHero/_Tier1SuperHeroWidget.cshtml", viewModel);
        }
    }
}
