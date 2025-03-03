using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Convenience.org.Components.Widgets.MemberSearchCompanyPeople
{
    [Route("MemberSearchCompanyPeople")]
    public class MemberSearchCompanyPeopleController : Controller
    {
        private readonly Dynamics365DataService _dataService;

        public MemberSearchCompanyPeopleController(Dynamics365DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index(Guid accountId)
        {
            var contactDetails = await _dataService.GetContactsByAccountIdAsync(accountId);

            var model = contactDetails.Entities.Select(e => new MemberSearchCompanyPeopleViewModel
            {
                ContactId = e.GetAttributeValue<Guid>("contactid"),
                FullName = e.GetAttributeValue<string>("fullname"),
                JobTitle = e.GetAttributeValue<string>("jobtitle"),
                City = e.GetAttributeValue<string>("address1_city"),
                StateOrProvince = e.GetAttributeValue<string>("address1_stateorprovince")
            }).ToList();

            return View("~/Components/Widgets/MemberSearchCompanyPeople/_MemberSearchCompanyPeople.cshtml", model);
        }
    }
}
