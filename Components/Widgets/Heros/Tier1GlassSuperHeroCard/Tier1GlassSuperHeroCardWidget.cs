using Convenience.org.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets.Heros.Tier1GlassSuperHeroCard;
using Convenience.org.Repositories.Interfaces;
using System;
using System.Collections.Generic;

[assembly: RegisterWidget(identifier: Tier1GlassSuperHeroCardWidget.IDENTIFIER,
    name: "Tier 1 Glass Super Hero Widget",
    viewComponentType: typeof(Tier1GlassSuperHeroCardWidget),
    propertiesType: typeof(Tier1GlassSuperHeroCardWidgetProperties),
    Description = "Tier 1 Glass Super Hero Widget",
    IconClass = "icon-l-img-3-cols-3",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Heros.Tier1GlassSuperHeroCard
{
    public class Tier1GlassSuperHeroCardWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.Tier1GlassSuperHeroCard";
        private readonly MediaLibraryHelpers mediaLibraryHelpers;
        protected readonly IEventPageRepository _eventDetailsRepository;


        public Tier1GlassSuperHeroCardWidget(MediaLibraryHelpers mediaLibraryHelpers,
            IEventPageRepository eventDetailsRepository)
        {
            this.mediaLibraryHelpers = mediaLibraryHelpers;
            _eventDetailsRepository = eventDetailsRepository;
        }


        public async Task<IViewComponentResult> InvokeAsync(Tier1GlassSuperHeroCardWidgetProperties properties)
        {
            string imageAltText = string.Empty;
            Tier1GlassSuperHeroCardWidgetViewModel viewModel = Tier1GlassSuperHeroCardWidgetViewModel.GetViewModel(properties);

            if (properties.ArticleItems != null && properties.ArticleItems.Count() > 0)
            {
                // Retrieves the GUIDs of the selected pages from the 'Pages' property
                List<Guid> pageGuids = properties?.ArticleItems?
                                                            .Select(i => i.WebPageGuid)
                                                            .ToList();

                var selectedEvent = _eventDetailsRepository.GetEventsRepository(pageGuids)?.FirstOrDefault();
                if (selectedEvent != null)
                {
                    viewModel.ImageUrl = mediaLibraryHelpers.GetImagePath(selectedEvent.Image.FirstOrDefault(), ref imageAltText);
                    viewModel.ImageAltText = !string.IsNullOrEmpty(selectedEvent.ImageAltText) ? selectedEvent.ImageAltText : imageAltText;

                    viewModel.Title = selectedEvent.Title;
                    viewModel.EyebrowTitle = selectedEvent.Title ?? string.Empty;
                    viewModel.DateTime = selectedEvent.StartDate.ToString("MMM dd yyyy");
                    viewModel.ReadTimeOrLocation = selectedEvent.Location;
                }
            }
            else
            {
                viewModel.ImageUrl = mediaLibraryHelpers.GetImagePath(properties.Image.FirstOrDefault(), ref imageAltText);
                viewModel.ImageAltText = imageAltText;
            }

            return View($"~/Components/Widgets/Heros/Tier1GlassSuperHeroCard/_Tier1GlassSuperHeroCardWidget.cshtml", viewModel);
        }
    }
}
