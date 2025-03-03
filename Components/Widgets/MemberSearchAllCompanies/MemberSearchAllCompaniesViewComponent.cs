using Convenience.org.Components.Widgets.MemberSearchAllCompanies;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

[assembly: RegisterWidget("MemberSearchAllCompanies", typeof(MemberSearchAllCompaniesViewComponent), "Member Search All Companies", Description = "Widget for searching member companies", IconClass = "icon-box")]
namespace Convenience.org.Components.Widgets.MemberSearchAllCompanies
{
    public class MemberSearchAllCompaniesViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;

        public MemberSearchAllCompaniesViewComponent(Dynamics365DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var memberCompanies = await _dataService.GetMemberCompaniesAsync();

            //var model = memberCompanies.Entities.Select(e => new MemberSearchAllCompaniesViewModel
            //{
            //    AccountId = e.GetAttributeValue<Guid>("accountid"),
            //    Name = e.GetAttributeValue<string>("name"),
            //    AccountTypeName = e.GetAttributeValue<string>("nacs_accounttypename"),
            //    SupplierTypeName = e.GetAttributeValue<string>("nacs_suppliertypename"),
            //    City = e.GetAttributeValue<string>("address1_city"),
            //    StateOrProvince = e.GetAttributeValue<string>("address1_stateorprovince"),
            //    Country = e.GetAttributeValue<string>("address1_country"),
            //    Region = e.GetAttributeValue<string>("nacs_region")
            //}).ToList();

            return View("~/Components/Widgets/MemberSearchAllCompanies/_MemberSearchAllCompanies.cshtml", memberCompanies);
        }
    }
}
