using CMS.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Xrm.Sdk;
using System;
using System.Threading.Tasks;

namespace Convenience.org.Components.Widgets.SubscriptionsNACSDaily
{
    [Route("SubscriptionsNACSDaily")]
    public class SubscriptionsNACSDailyController : Controller
    {
        private readonly Dynamics365DataService _dataService;
        private readonly Dynamics365Connectivity _connectivity;
        private readonly IEventLogService _eventLogService;

        public SubscriptionsNACSDailyController(Dynamics365Connectivity connectivity,Dynamics365DataService dataService, IEventLogService eventLog)
        {
            _connectivity = connectivity;
            _dataService = dataService;
            _eventLogService = eventLog;
        }

        [HttpPost("UpdateNacsDaily")]
        public async Task<IActionResult> UpdateNacsDaily([FromBody] SubscriptionNACSDailyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data received." });
            }
            //var email = MembershipContext.AuthenticatedUser?.Email;
            var email =  "thelali@convenience.org";


            if (string.IsNullOrEmpty(email))
            {
                //return BadRequest(new { message = "User email is not available." });
                return Unauthorized(new { message = "User is not authenticated." });
            }

            try
            {
                var contactId = await _dataService.GetCurrentUserContactIdAsync(email);
                if (contactId == null)
                {
                    return NotFound(new { message = "Contact not found in Dynamics 365." });
                }

                await UpdateNacsNacsdailyAsync(contactId.Value, model.NacsNacsdaily);

                return Ok(new { message = "NACS Daily updated successfully!" });
            }
            catch (Exception ex)
            {
                _eventLogService.LogError("NACSDailySubscription", "NACSDailySubscription", $"{ex} Error updating subscription for user {email}");
                return StatusCode(500, new { message = "An error occurred. Please try again later." });
            }
        }
        public async Task UpdateNacsNacsdailyAsync(Guid contactId, bool nacsNacsdaily)
        {
            var client = _connectivity.GetServiceClient();
            var contact = new Entity("contact", contactId) { ["nacs_nacsdaily"] = nacsNacsdaily };
            await client.UpdateAsync(contact);
        }
    }
}
