using NACSShow;
using System.Collections;
using System.Collections.Generic;

namespace NACSShow.Components.Widgets.DailyNewsListing
{
    public class DailyNewsListingViewModel
    {
        public required string Title { get; set; }

        public IEnumerable<DailyNews> DailyNewsList { get; set; } = new List<DailyNews>();
    }
}
