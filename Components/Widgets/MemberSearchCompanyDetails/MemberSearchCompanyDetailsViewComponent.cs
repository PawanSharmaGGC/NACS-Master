using Convenience.org.Components.Widgets.MemberSearchCompanyDetails;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Xrm.Sdk;
using System.Linq;

[assembly: RegisterWidget("CompanyDetails", typeof(MemberSearchCompanyDetailsViewComponent), "Company Details", Description = "Widget to display details of a company", IconClass = "icon-info")]
namespace Convenience.org.Components.Widgets.MemberSearchCompanyDetails
{
    public class MemberSearchCompanyDetailsViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;

        public MemberSearchCompanyDetailsViewComponent(Dynamics365DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid accountId)
        {
            // Fetch company details including processed option set labels
            var companyDetails = await _dataService.GetCompanyDetailsAsync(accountId);

            // Prepare the view model
            var model = new MemberSearchCompanyDetailsViewModel
            {
                AccountId = accountId,
                Name = companyDetails.GetAttributeValue<string>("name"),
                AccountTypeName = companyDetails.GetAttributeValue<string>("nacs_accounttype"),
                SupplierTypeName = companyDetails.GetAttributeValue<string>("nacs_suppliertype"),
                Address = companyDetails.GetAttributeValue<string>("address1_composite"),
                WebsiteUrl = companyDetails.GetAttributeValue<string>("websiteurl"),
                Telephone = companyDetails.GetAttributeValue<string>("telephone1"),
                TotalStores = companyDetails.GetAttributeValue<int>("nacs_totalstores")
            };

            return View("~/Components/Widgets/MemberSearchCompanyDetails/_MemberSearchCompanyDetails.cshtml", model);
        }

    }
}
