using System.Collections.Generic;
using Convenience.org.Components.Widgets.EventCarousel;

namespace Convenience.org.Models
{
    public class EventCarouselViewModel
    {
        public string Title { get; set; }
        public EyebrowTitleViewModel Eyebrow { get; set; }
        public string Status { get; set; }
        public List<EventCardItem> EventItems { get; set; }

        public static EventCarouselViewModel GetViewModel(EventCarouselWidgetProperties properties)
        {
            var viewModel = new EventCarouselViewModel();
            viewModel.Title = properties?.Title;
            viewModel.Eyebrow = new EyebrowTitleViewModel();
            viewModel.Eyebrow.Title = properties?.EyebrowTitle;
            viewModel.Status = properties?.Status;
            return viewModel;
        }
    }

    public class EventCardItem
    {

        public string Image { get; set; }
        public string ImageAlt { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LocationOrReadTime { get; set; }
    }
}
