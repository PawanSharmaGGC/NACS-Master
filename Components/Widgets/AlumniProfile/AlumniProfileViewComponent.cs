using Convenience.org.Components.Widgets.AlumniProfile;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CMS.Membership;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;

[assembly: RegisterWidget("AlumniUserProfileWidget", typeof(AlumniProfileViewComponent), "Alumni Profile", Description = "Displays user profile with preferences", IconClass = "icon-user")]

namespace Convenience.org.Components.Widgets.AlumniProfile
{
    public class AlumniProfileViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;

        public AlumniProfileViewComponent(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketListIds)
        {
            _dataService = dataService;
            _marketListIds = marketListIds.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = MembershipContext.AuthenticatedUser;
            var email = user?.Email ?? "thelali@convenience.org";
            var alumniNetworkOptedInListId = _marketListIds.AlumniNetworkOptedIn;
            var alumniNetworkDirectoryOptInListId = _marketListIds.AlumniNetworkDirectoryOptIn;
            var contactId = Guid.Empty;

            contactId = (Guid)await _dataService.GetCurrentUserContactIdAsync(email);
            var contactDetails = await _dataService.GetPersonDetailsByIdAsync(contactId);
            var parentCustomerId = contactDetails.GetAttributeValue<EntityReference>("parentcustomerid");
            var isSubscribedToNewsletter = await _dataService.IsUserInListAsync(alumniNetworkOptedInListId, contactId);
            var isListedInDirectory = await _dataService.IsUserInListAsync(alumniNetworkDirectoryOptInListId, contactId);

            var viewModel = new AlumniProfileViewModel
            {
                UserId = contactId,
                FullName = contactDetails.GetAttributeValue<string>("pa_labelname"),
                JobTitle = contactDetails.GetAttributeValue<string>("jobtitle"),
                AccountName = parentCustomerId?.Name,
                Address = contactDetails.GetAttributeValue<string>("address1_composite"),
                ProfilePictureUrl = "path/to/default/profile-pic.jpg", 
                IsSubscribedToNewsletter = isSubscribedToNewsletter,
                IsListedInDirectory = isListedInDirectory
            };

            return View("~/Components/Widgets/AlumniProfile/_AlumniProfile.cshtml", viewModel);
        }
    }
}
