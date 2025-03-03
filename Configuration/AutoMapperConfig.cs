using AutoMapper;
using Convenience.org.Components.Widgets.Cards.ExpandableContentCard;
using Convenience.org.Components.Widgets.Cards.RelatedContentCard;
using Convenience.org.Components.Widgets.DOHAttendees;
using Convenience.org.Components.Widgets.ProductHero;
using Convenience.org.Components.Widgets.Statistics;
using Convenience.org.Models;

namespace Convenience.org.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //Page mapping
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductHeroWidgetProperties, ProductHeroWidgetViewModel>();

            //Page widgt mapping
            ConfigWidgetModels();
        }

        /// <summary>
        /// Map widget properties with widget view model
        /// </summary>
        private void ConfigWidgetModels()
        {
            CreateMap<RelatedContentCardWidgetProperties, RelatedContentCardWidgetViewModel>();
            CreateMap<ExpandableContentCardWidgetProperties, ExpandableContentCardWidgetViewModel>();
            CreateMap<StatisticsWidgetProperties, StatisticsWidgetViewModel>();
            CreateMap<DOHAttendeesWidgetProperties, DOHAttendeesWidgetViewModel>();
        }
    }
}
