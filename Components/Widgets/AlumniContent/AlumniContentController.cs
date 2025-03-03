using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Linq;

namespace Convenience.org.Components.Widgets.AlumniContent
{
    [Route("AlumniContent")]
    public class AlumniContentController : Controller
    {
        private readonly ICustomTableService _customTableService;

        public AlumniContentController(ICustomTableService customTableService)
        {
            _customTableService = customTableService;
        }

        [HttpPost("ShowItems")]
        public IActionResult ShowItems(string filters, int limit, int offset)
        {
            var selectedFilters = string.IsNullOrEmpty(filters)
        ? null
        : filters.Split(',').Select(f => f.Trim()).ToList();

            var data = _customTableService.GetData(selectedFilters, limit, offset);

            return PartialView("~/Components/Widgets/AlumniContent/_AlumniContentList.cshtml", data);
        }
       
    }
}
