using Convenience.org.Components.Widgets.AlumniContent;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


[assembly: RegisterWidget("AlumniContents", typeof(AlumniContentViewComponent), "Alumni Content Portal", Description = "Displays Alumni main contents.", IconClass = "icon-list")]
namespace Convenience.org.Components.Widgets.AlumniContent
{
    public class AlumniContentViewComponent : ViewComponent
    {
        private readonly ICustomTableService _customTableService;

        public AlumniContentViewComponent(ICustomTableService customTableService)
        {
            _customTableService = customTableService;
        }

        public IViewComponentResult Invoke(string filters = "", int limit = 5, int offset = 0)
        {
            var selectedFilters = string.IsNullOrEmpty(filters)
                ? new List<string>()
                : filters.Split(',').Select(f => f.Trim()).ToList();

            var data = _customTableService.GetData(selectedFilters, limit, offset);

            var hasMoreItems = data.Count == limit;

            var model = new AlumniContentViewModel
            {
                Filters = selectedFilters,
                Limit = limit,
                Items = data,
                CanShowMore = hasMoreItems 
            };

            return View("~/Components/Widgets/AlumniContent/_AlumniContent.cshtml", model);
        }

    }
}
