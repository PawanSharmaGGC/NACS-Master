using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;

namespace Convenience.org.Components.Widgets.AlumniProfile
{
    [Route("AlumniUserProfile")]
    public class AlumniProfileController : Controller
    {
        private readonly Dynamics365DataService _dataService;
        private readonly MarketingListIdsConfig _marketListIds;

        public AlumniProfileController(Dynamics365DataService dataService, IOptions<MarketingListIdsConfig> marketingListIds)
        {
            _dataService = dataService;
            _marketListIds = marketingListIds.Value;
        }

        [HttpPost("UpdatePreferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] AlumniProfileViewModel model)
        {
            try
            {
                if (model.IsSubscribedToNewsletter)
                {
                    await _dataService.AddUserToListAsync(_marketListIds.AlumniNetworkOptedIn, model.UserId);
                }
                else
                {
                    await _dataService.RemoveUserFromListAsync(_marketListIds.AlumniNetworkOptedIn, model.UserId);
                }

                if (model.IsListedInDirectory)
                {
                    await _dataService.AddUserToListAsync(_marketListIds.AlumniNetworkDirectoryOptIn, model.UserId);
                }
                else
                {
                    await _dataService.RemoveUserFromListAsync(_marketListIds.AlumniNetworkDirectoryOptIn, model.UserId);
                }

                return Ok(new { message = "Preferences updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
