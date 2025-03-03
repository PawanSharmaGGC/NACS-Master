using Convenience.org.Models;

namespace Convenience.org.Components.Widgets.ProductHero
{
    public class ProductHeroWidgetViewModel
    {

        public ProductViewModel ProductDetail { get; set; }
        
        public string CTA1Text { get; set; } = string.Empty;
        
        public string CTA1Link { get; set; } = string.Empty;
        
        public string CTA2Text { get; set; } = string.Empty;
        
        public string CTA2Link { get; set; } = string.Empty;

        public string AdditionalLicenses { get; set; } = string.Empty;
    }
}
