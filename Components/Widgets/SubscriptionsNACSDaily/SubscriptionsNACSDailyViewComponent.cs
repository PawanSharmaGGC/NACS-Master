using CMS.Membership;
using Convenience.org.Components.Widgets.SubscriptionsNACSDaily;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Threading.Tasks;

[assembly: RegisterWidget("SubscriptionsNACSDaily", typeof(SubscriptionsNACSDailyViewComponent), "NACS Daily Widget", Description = "Subscriptions NACS Daily", IconClass = "icon-box")]

namespace Convenience.org.Components.Widgets.SubscriptionsNACSDaily
{
    public class SubscriptionsNACSDailyViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;
        private readonly Dynamics365Connectivity _connectivity;
        private readonly IMemoryCache _memoryCache;

        public SubscriptionsNACSDailyViewComponent(Dynamics365DataService dataService, Dynamics365Connectivity connectivity, IMemoryCache memoryCache)
        {
            _dataService = dataService;
            _connectivity = connectivity;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = MembershipContext.AuthenticatedUser;
            var email = user?.Email ?? "thelali@convenience.org"; 
            var nacsNacsdaily = false;
            var cacheKey = $"ContactData_{email}";
            Guid? contactId;

            if (!_memoryCache.TryGetValue(cacheKey, out contactId))
            {
                // Fetch from Dynamics 365 if not in cache
                contactId = await _dataService.GetCurrentUserContactIdAsync(email);
                if (contactId != null)
                {
                    // Store in cache with an expiration time
                    _memoryCache.Set(cacheKey, contactId, TimeSpan.FromMinutes(30));
                }
            }

            if (contactId != null)
            {
                nacsNacsdaily = await GetNacsNacsdailyAsync(contactId.Value);
            }

            var model = new SubscriptionNACSDailyViewModel
            {
                NacsNacsdaily = nacsNacsdaily
            };

            return View("~/Components/Widgets/SubscriptionsNACSDaily/_SubscriptionsNACSDaily.cshtml", model);
        }
        private async Task<bool> GetNacsNacsdailyAsync(Guid contactId)
        {
            var client = _connectivity.GetServiceClient();
            var contact = await client.RetrieveAsync("contact", contactId, new ColumnSet("nacs_nacsdaily"));
            return contact.Contains("nacs_nacsdaily") && (bool)contact["nacs_nacsdaily"];
        }
    }
}
