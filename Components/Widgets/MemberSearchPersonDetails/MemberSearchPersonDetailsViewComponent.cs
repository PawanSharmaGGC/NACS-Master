using Convenience.org.Components.Widgets.MemberSearchPersonDetails;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Xrm.Sdk;
using MyNACSSavedItems;
using System.Linq;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using CMS.Membership;

[assembly: RegisterWidget("PersonDetails", typeof(MemberSearchPersonDetailsViewComponent), "Person Details", Description = "Widget to display details of a person", IconClass = "icon-user")]
namespace Convenience.org.Components.Widgets.MemberSearchPersonDetails
{
    public class MemberSearchPersonDetailsViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;

        public MemberSearchPersonDetailsViewComponent(Dynamics365DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid contactId)
        {
            var personDetails = await _dataService.GetPersonDetailsByIdAsync(contactId);
            var parentCustomerId = personDetails.GetAttributeValue<EntityReference>("parentcustomerid");
            var userId = MembershipContext.AuthenticatedUser.UserID;
            var isSaved = IsPersonSaved(contactId, userId);

            var model = new MemberSearchPersonDetailsViewModel
            {
                ContactId = personDetails.GetAttributeValue<Guid>("contactid"),
                AccountId = parentCustomerId?.Id ?? Guid.Empty,
                PaLabelName = personDetails.GetAttributeValue<string>("pa_labelname"),
                JobTitle = personDetails.GetAttributeValue<string>("jobtitle"),
                AccountName = parentCustomerId?.Name,
                AddressComposite = personDetails.GetAttributeValue<string>("address1_composite"),
                City = personDetails.GetAttributeValue<string>("address1_city"),
                StateOrProvince = personDetails.GetAttributeValue<string>("address1_stateorprovince"),
                Telephone = personDetails.GetAttributeValue<string>("address1_telephone1"),
                Email = personDetails.GetAttributeValue<string>("emailaddress1"),
                IsSaved = isSaved
            };

            return View("~/Components/Widgets/MemberSearchPersonDetails/_MemberSearchPersonDetails.cshtml", model);
        }
        public bool IsPersonSaved(Guid contactId, int userId)
        {
            return MemberItemInfo.Provider.Get().Any(item => item.NACSIndividualKey == contactId.ToString() && item.KenticoUserID == userId);
        }

    }
}
