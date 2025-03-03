using System.Threading.Tasks;
using System;
using Convenience.org.Components.Widgets.Heros.SponsorHero;
using Convenience.org.Components.Widgets.Heros.Tier1GlassSuperHeroCard;
using Convenience.org.Helpers;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

[assembly: RegisterWidget(identifier: SponsorHeroWidget.IDENTIFIER,
    name: "Sponsor Hero Widget",
    viewComponentType: typeof(SponsorHeroWidget),
    propertiesType: typeof(SponsorHeroWidgetProperties),
    Description = "Sponsor Hero Widget",
    IconClass = "icon-dollar-sign",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Heros.SponsorHero
{
    public class SponsorHeroWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.SponsorHero";
        private readonly MediaLibraryHelpers mediaLibraryHelpers;
        protected readonly IEventPageRepository _eventDetailsRepository;
        protected readonly ISponsoresRepository _sponsoresRepository;
        //Sponsor
        public SponsorHeroWidget(MediaLibraryHelpers mediaLibraryHelpers, IEventPageRepository eventDetailsRepository, ISponsoresRepository sponsoresRepository)
        {
            this.mediaLibraryHelpers = mediaLibraryHelpers;
            _eventDetailsRepository = eventDetailsRepository;
            _sponsoresRepository = sponsoresRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(SponsorHeroWidgetProperties properties)
        {
            string imageAltText = string.Empty;
            SponsorHeroWidgetViewModel viewModel = SponsorHeroWidgetViewModel.GetViewModel(properties);
            viewModel.VideoUrl = mediaLibraryHelpers.GetVideoPath(properties.VideoURL.FirstOrDefault());

            if (properties.ArticleItems != null && properties.ArticleItems.Count() > 0)
            {
                // Retrieves the GUIDs of the selected pages from the 'Pages' property
                List<Guid> pageGuids = properties?.ArticleItems?
                                                            .Select(i => i.WebPageGuid)
                                                            .ToList();

                var selectedEvent = _eventDetailsRepository.GetEventsRepository(pageGuids)?.FirstOrDefault();
                if (selectedEvent != null)
                {
                    viewModel.WebPageGuid = selectedEvent.SystemFields.WebPageItemGUID.ToString();
                    viewModel.Title = selectedEvent.Title;
                    viewModel.EyebrowTitle = selectedEvent.Title ?? string.Empty;
                    viewModel.DateMonth = selectedEvent.StartDate.ToString("MMM");
                    viewModel.DateDay = selectedEvent.StartDate.ToString("dd");
                    viewModel.DateYear = selectedEvent.StartDate.ToString("yyyy");

                    var sponsor = selectedEvent.Sponsor?.FirstOrDefault();
                    if (sponsor != null && sponsor.SystemFields.ContentItemID > 0)
                    {
                        viewModel.SponsorImageUrl = mediaLibraryHelpers.GetImagePath(sponsor.Image.FirstOrDefault(),ref imageAltText);
                        viewModel.SponsorImageUrlAlt = sponsor.ImageAlt;
                    }
                }
            }
            else
            {
                viewModel.VideoUrl = mediaLibraryHelpers.GetVideoPath(properties.VideoURL.FirstOrDefault());
            }

            return View($"~/Components/Widgets/Heros/SponsorHero/_SponsorHeroWidget.cshtml", viewModel);
        }
    }
}
