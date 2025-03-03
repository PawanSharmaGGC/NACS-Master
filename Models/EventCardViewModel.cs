using System.Collections.Generic;

namespace Convenience.org.Models
{
    public class EventCardViewModel
    {
        public string? Title { get; set; }
        public string? Datetime { get; set; }
        public string? Location { get; set; }
        public string CTAText { get; set; }
        public string CTAUrl { get; set; }

        public List<ImageViewModel> Images { get; set; }
    }

    public class ImageViewModel
    {
        public string Url { get; set; }
        public string AltText { get; set; }
    }
}
