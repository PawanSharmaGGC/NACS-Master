using Convenience.org.Components.Widgets.MemberSearchCompanyPeople;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;

[assembly: RegisterWidget("MemberSearchCompanyPeople", typeof(MemberSearchCompanyPeopleViewComponent), "Contact Details", Description = "Widget to display contact details of an account", IconClass = "icon-user")]
namespace Convenience.org.Components.Widgets.MemberSearchCompanyPeople
{
    public class MemberSearchCompanyPeopleViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;

        public MemberSearchCompanyPeopleViewComponent(Dynamics365DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid accountId)
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
