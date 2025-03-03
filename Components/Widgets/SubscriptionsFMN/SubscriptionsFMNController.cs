using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Convenience.org.Components.Widgets.SubscriptionsFMN
{
    [Route("SubscriptionsFMN")]
    public class SubscriptionsFMNController : Controller
    {
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;
        private readonly IWebPageDataContextInitializer _pageDataContextInitializer;
        private readonly IWebPageDataContextRetriever _pageDataContextRetriever;
        public SubscriptionsFMNController(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketingListIds, IWebPageDataContextRetriever pageDataContextRetriever, IWebPageDataContextInitializer pageDataContextInitializer)
        {
            _dataService = dataService;
            _marketListIds = marketingListIds.Value;
            _pageDataContextRetriever = pageDataContextRetriever;
            _pageDataContextInitializer = pageDataContextInitializer;
        }

        [HttpPost("UpdateFMNList")]
        public async Task<IActionResult> UpdateFMNList([FromBody] SubscriptionFMNViewModel model)
        {
            try
            {
                if (model.FMNSubscriptionOption)
                {
                    await _dataService.AddUserToListAsync(_marketListIds.FuelsMarketNewsWeekly, model.UserId);
                }
                else
                {
                    await _dataService.RemoveUserFromListAsync(_marketListIds.FuelsMarketNewsWeekly, model.UserId);
                }

                return Ok(new { message = "FMN List updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
