using CMS.Membership;
using Convenience.org.Components.Widgets.SubscriptionsFMN;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

[assembly: RegisterWidget("SubscriptionsFMN", typeof(SubscriptionsFMNViewComponent), "Fuels Market News E-Newsletter", Description = "Fuels Market News E-Newsletter", IconClass = "icon-box")]

namespace Convenience.org.Components.Widgets.SubscriptionsFMN
{
    public class SubscriptionsFMNViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;

        public SubscriptionsFMNViewComponent(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketListIds)
        {
            _dataService = dataService;
            _marketListIds = marketListIds.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = MembershipContext.AuthenticatedUser;
            var email = user?.Email ?? "thelali@convenience.org";
            bool isSubscribed = false;
            var fmnListId = _marketListIds.FuelsMarketNewsWeekly;
            var contactId = Guid.Empty;

            if (!string.IsNullOrEmpty(email))
            {
                contactId = (Guid) await _dataService.GetCurrentUserContactIdAsync(email);
                isSubscribed = await _dataService.IsUserInListAsync(fmnListId, contactId);

            }
            var viewModel = new SubscriptionFMNViewModel
            {
                UserId = contactId,
                FMNSubscriptionOption = isSubscribed
            };
            return View("~/Components/Widgets/SubscriptionsFMN/_SubscriptionsFMN.cshtml", viewModel);
        }
        
    }
}
