using CMS.Membership;
using Convenience.org.Components.Widgets.SubscriptionsNACSMagazine;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

[assembly: RegisterWidget("SubscriptionsNACSMagazine", typeof(SubscriptionNACSMagazineViewComponent), "NACS Magazine + Fuels Market News Magazine", Description = "NACS Magazine + Fuels Market News Magazine", IconClass = "icon-box")]

namespace Convenience.org.Components.Widgets.SubscriptionsNACSMagazine
{
    public class SubscriptionNACSMagazineViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;

        public SubscriptionNACSMagazineViewComponent(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketListIds)
        {
            _dataService = dataService;
            _marketListIds = marketListIds.Value;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = MembershipContext.AuthenticatedUser;
            var email = user?.Email ?? "thelali@convenience.org";
            var userId = Guid.Empty;
            string country = string.Empty;

            var printListId = _marketListIds.NACSMagazinePrint;
            var digitalListId = _marketListIds.NACSMagazineDigital;
            var printUnsubscribedListId = _marketListIds.NACSMagazinePrintUnsubscribe;
            var digitalUnsubscribedListId = _marketListIds.NACSMagazineDigitalUnsubscribe;

            if (!string.IsNullOrEmpty(email))
            {
                var contactId = await _dataService.GetCurrentUserContactIdAsync(email);
                if (contactId.HasValue)
                {
                    userId = contactId.Value;
                    var contact = await _dataService.GetContactDetailsAsync(userId);
                    country = contact?.GetAttributeValue<string>("address1_country") ?? string.Empty;
                }
            }

            var isInPrintList = await _dataService.IsUserInListAsync(printListId, userId);
            var isInDigitalList = await _dataService.IsUserInListAsync(digitalListId, userId);
            var isInPrintUnsubscribeList = await _dataService.IsUserInListAsync(printUnsubscribedListId, userId);
            var isInDigitalUnsubscribeList = await _dataService.IsUserInListAsync(digitalUnsubscribedListId, userId);

            var viewModel = new SubscriptionNACSMagazineViewModel
            {
                UserId = userId,
                Country = country,
                IsPrintDigital = isInPrintList && isInDigitalList,
                IsPrintOnly = isInPrintList && !isInDigitalList,
                IsDigitalOnly = isInDigitalList && !isInPrintList,
                IsUnsubscribeBoth = isInPrintUnsubscribeList && isInDigitalUnsubscribeList
            };

            return View("~/Components/Widgets/SubscriptionsNACSMagazine/_SubscriptionsNACSMagazine.cshtml", viewModel);
        }
        
    }
}
