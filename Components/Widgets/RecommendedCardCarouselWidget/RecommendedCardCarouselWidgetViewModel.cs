using Convenience.org.Models;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.RecommendedCardCarouselWidget
{
    public class RecommendedCardCarouselWidgetViewModel
    {
        public string LeftTitle { get; set; }
        
        public string CTAText { get; set; }
           
        public string CTALink { get; set; }

        public List<ArticleCardViewModel> ArticleItems { get; set; }

        public static RecommendedCardCarouselWidgetViewModel GetViewModel(RecommendedCardCarouselWidgetProperties properties)
        {
            if (properties == null)
            {
                return null;
            }
            else
            {
                var model = new RecommendedCardCarouselWidgetViewModel()
                {
                    LeftTitle = properties.LeftTitle,
                    CTAText = properties.CTAText,
                    CTALink = properties.CTALink,
                };
                return model;
            }
            
        }
    }
}