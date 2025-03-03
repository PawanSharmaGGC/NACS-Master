using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Kentico.PageBuilder.Web.Mvc;
using Convenience.org.Components.Widgets.Statistics;

[assembly: RegisterWidget(
    identifier: StatisticsWidget.IDENTIFIER,
    name: "Statistics Widget",
    viewComponentType: typeof(StatisticsWidget),
    propertiesType: typeof(StatisticsWidgetProperties),
    Description = "Statistics Widget",
    IconClass = "icon-factory",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.Statistics
{
    public class StatisticsWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.Statistics";
        private readonly IMapper _mapper;

        public StatisticsWidget(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retrive Statistics widget properties and it's view based on CMS widget data.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(StatisticsWidgetProperties properties)
        {
            string imageAltText = string.Empty;

            var viewModel = _mapper.Map<StatisticsWidgetViewModel>(properties);

            return View($"~/Components/Widgets/Statistics/_StatisticsWidget.cshtml", viewModel);
        }
    }
}
