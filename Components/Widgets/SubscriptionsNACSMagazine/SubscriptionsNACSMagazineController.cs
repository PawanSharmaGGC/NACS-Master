using Convenience.org.Components.Widgets.SubscriptionsFMN;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;
using CMS.Membership;
using Microsoft.Xrm.Sdk;

namespace Convenience.org.Components.Widgets.SubscriptionsNACSMagazine
{
    [Route("SubscriptionsNACSMagazine")]
    public class SubscriptionsNACSMagazineController : Controller
    {
        private readonly Dynamics365Connectivity _connectivity;
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;
        private readonly IWebPageDataContextInitializer _pageDataContextInitializer;
        private readonly IWebPageDataContextRetriever _pageDataContextRetriever;
        public SubscriptionsNACSMagazineController(Dynamics365Connectivity dynamics365Connectivity, Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketingListIds, IWebPageDataContextRetriever pageDataContextRetriever, IWebPageDataContextInitializer pageDataContextInitializer)
        {
            _connectivity = dynamics365Connectivity;
            _dataService = dataService;
            _marketListIds = marketingListIds.Value;
            _pageDataContextRetriever = pageDataContextRetriever;
            _pageDataContextInitializer = pageDataContextInitializer;
        }
        [HttpPost("UpdateNACSMagazineLists")]
        public async Task<IActionResult> UpdateNACSMagazineLists([FromBody] SubscriptionNACSMagazineViewModel model)
        {
            try
            {
                switch (model.SelectedOption)
                {
                    case "PrintDigital":
                        await _dataService.RemoveUserFromListAsync(_marketListIds.NACSMagazinePrintUnsubscribe, model.UserId);
                        await _dataService.RemoveUserFromListAsync(_marketListIds.NACSMagazineDigitalUnsubscribe, model.UserId);
                        await _dataService.AddUserToListAsync(_marketListIds.NACSMagazinePrint, model.UserId);
                        await _dataService.AddUserToListAsync(_marketListIds.NACSMagazineDigital, model.UserId);
                        break;

                    case "PrintOnly":
                        await _dataService.RemoveUserFromListAsync(_marketListIds.NACSMagazinePrintUnsubscribe, model.UserId);
                        await _dataService.RemoveUserFromListAsync(_marketListIds.NACSMagazineDigital, model.UserId);
                        await _dataService.AddUserToListAsync(_marketListIds.NACSMagazinePrint, model.UserId);
                        await _dataService.AddUserToListAsync(_marketListIds.NACSMagazineDigitalUnsubscribe, model.UserId);
                        break;

                    case "DigitalOnly":
                        await _dataService.RemoveUserFromListAsync(_marketListIds.NACSMagazinePrint, model.UserId);
                        await _dataService.RemoveUserFromListAsync(_marketListIds.NACSMagazineDigitalUnsubscribe, model.UserId);
                        await _dataService.AddUserToListAsync(_marketListIds.NACSMagazinePrintUnsubscribe, model.UserId);
                        await _dataService.AddUserToListAsync(_marketListIds.NACSMagazineDigital, model.UserId);
                        break;

                    case "Unsubscribe":
                        await _dataService.RemoveUserFromListAsync(_marketListIds.NACSMagazinePrint, model.UserId);
                        await _dataService.RemoveUserFromListAsync(_marketListIds.NACSMagazineDigital, model.UserId);
                        await _dataService.AddUserToListAsync(_marketListIds.NACSMagazinePrintUnsubscribe, model.UserId);
                        await _dataService.AddUserToListAsync(_marketListIds.NACSMagazineDigitalUnsubscribe, model.UserId);
                        break;
                    default:
                        return BadRequest(new { message = "Invalid subscription option selected." });
                }

                return Ok(new { message = "Subscription updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
        [HttpGet("GetAddress")]
        public async Task<IActionResult> GetAddress()
        {
            try
            {
                //var user = MembershipContext.AuthenticatedUser;
                //if (user == null) return Unauthorized();

                var email = "thelali@convenience.org";
                var userId = await _dataService.GetCurrentUserContactIdAsync(email);
                if (userId == Guid.Empty) return NotFound();

                var contact = await _dataService.GetContactDetailsAsync((Guid)userId);

                if (contact == null) return NotFound();

                return Ok(new
                {
                    street = contact.GetAttributeValue<string>("address1_line1"),
                    city = contact.GetAttributeValue<string>("address1_city"),
                    state = contact.GetAttributeValue<string>("address1_stateorprovince"),
                    postalCode = contact.GetAttributeValue<string>("address1_postalcode"),
                    country = contact.GetAttributeValue<string>("address1_country")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the address.", details = ex.Message });
            }
        }
        [HttpPost("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress([FromBody] SubscriptionNACSMagazineViewModel address)
        {
            try
            {
                //var user = MembershipContext.AuthenticatedUser;
                //if (user == null) return Unauthorized();

                string email = "thelali@convenience.org";
                Guid userId = (Guid) await _dataService.GetCurrentUserContactIdAsync(email);
                if (userId == Guid.Empty) return NotFound();

                // Update address logic
                await UpdateUserAddressAsync(userId, address);

                return Ok(new { message = "Address updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the address.", details = ex.Message });
            }
        }
        public async Task UpdateUserAddressAsync(Guid userId, SubscriptionNACSMagazineViewModel address)
        {
            try
            {
                var client = _connectivity.GetServiceClient();
                var contact = new Entity("contact", userId)
                {
                    ["address1_line1"] = address.Street,
                    ["address1_city"] = address.City,
                    ["address1_stateorprovince"] = address.State,
                    ["address1_postalcode"] = address.PostalCode,
                    ["address1_country"] = address.Country
                };
                await client.UpdateAsync(contact);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to update address for user ID {userId}.", ex);
            }
        }
    }
}
