using CMS.ContentEngine;
using System.Collections.Generic;
using System.Linq;

using Tag = CMS.ContentEngine.Tag;

namespace Convenience.org.Models
{
    public class DeepDiveViewModel
    {
        public string BannerImage { get; set; }
        public string LightImage { get; set; }
        public string Title { get; set; }
        public IEnumerable<Tag> Tags { get; set; } = Enumerable.Empty<Tag>();
        public int TopN { get; set; }

        public string CardCTAText { get; set; }
        public List<DeepDiveCardViewModel> DeepDiveCardItems { get; set; } = new List<DeepDiveCardViewModel>();

        public static DeepDiveViewModel GetViewModel()
        {
            var viewModel = new DeepDiveViewModel();
            return viewModel;
        }
    }

    public class DeepDiveCardViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
        public string ItemPageUrl { get; set; }
    }
}
