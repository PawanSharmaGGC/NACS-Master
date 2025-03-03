using CMS.MediaLibrary;
using System.Collections.Generic;
using Convenience.org.Models.Cards;
using System.Linq;
using Convenience.org.Helpers;


namespace Convenience.org.Components.Widgets.Cards
{
    public class TierThreeContentCardViewModel
    {
        public string EyebrowTitle { get; set; }
        public string CTAText { get; set; }
        public string PageUrl { get; set; }
        public string ImageAltText { get; set; }
        public bool CTALeftIconVisible { get; set; }
        public IEnumerable<CommunityNewsViewModel> CommunityNews { get; set; }

    }
}
