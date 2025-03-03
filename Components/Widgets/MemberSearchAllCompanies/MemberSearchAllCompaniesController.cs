using Microsoft.AspNetCore.Mvc;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convenience.org.Components.Widgets.MemberSearchAllCompanies
{
    [Route("MemberSearchCompany")]
    public class MemberSearchAllCompaniesController : Controller
    {
        private readonly Dynamics365DataService _dataService;

        public MemberSearchAllCompaniesController(Dynamics365DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("GetMemberCompanies")]
        public async Task<IActionResult> GetMemberCompanies()
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

            return Ok(memberCompanies);
        }
    }
}
