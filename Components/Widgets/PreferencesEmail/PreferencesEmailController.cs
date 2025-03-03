using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Convenience.org.Components.Widgets.PreferencesEmail
{
    [Route("PreferencesEmail")]
    public class PreferencesEmailController : Controller
    {
        private readonly Dynamics365DataService _dataService;
        private readonly Dynamics365Connectivity _connectivity;

        public PreferencesEmailController(Dynamics365Connectivity connectivity, Dynamics365DataService dataService)
        {
            _connectivity = connectivity;
            _dataService = dataService;

        }

        [HttpPost("SaveEmailPreferences")]
        public async Task<IActionResult> SaveEmailPreferences([FromBody, Bind("UnsubscribeAll, SelectedEvents, SelectedOtherTopics")] PreferencesEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid data provided." });
            }

            try
            {
                //var user = MembershipContext.AuthenticatedUser;
                //var email = user?.Email; 
                var email = "thelali@convenience.org";


                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new { message = "User email is not available." });
                }

                var selectedTopicIds = model.SelectedEvents
                .Concat(model.SelectedOtherTopics)
                .Select(Guid.Parse)
                .ToList();

                await SavePreferencesAsync(email, selectedTopicIds, model.UnsubscribeAll);

                return Ok(new { message = "Preferences saved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving preferences." });
            }
        }

        public async Task SavePreferencesAsync(string email, List<Guid> selectedTopicIds, bool unsubscribeAll)
        {
            var client = _connectivity.GetServiceClient();
            var existingPreferences = await _dataService.GetPreferencesAsync(email);
            var existingTopicIds = existingPreferences.Entities.Select(e => e.GetAttributeValue<EntityReference>("msdynmkt_topicid").Id).ToList();

            if (unsubscribeAll || selectedTopicIds.Count == 0)
            {
                foreach (var consentEntity in existingPreferences.Entities)
                {
                    consentEntity["msdynmkt_value"] = new OptionSetValue(534120002); // Unsubscribed
                    await client.UpdateAsync(consentEntity);
                }
            }
            else
            {
                foreach (var topicId in selectedTopicIds)
                {
                    if (!existingTopicIds.Contains(topicId))
                    {
                        var newConsent = new Entity("msdynmkt_contactpointconsent4")
                        {
                            ["msdynmkt_contactpointtype"] = new OptionSetValue(534120000),
                            ["msdynmkt_value"] = new OptionSetValue(534120001),
                            ["msdynmkt_contactpointvalue"] = email,
                            ["msdynmkt_topicid"] = new EntityReference("msdynmkt_topic", topicId),
                            ["msdynmkt_purposeid"] = new EntityReference("msdynmkt_purpose", Guid.Parse("1e6a4204-3056-47a6-8350-750fc17c0fdf")),
                            ["msdynmkt_logicalreason"] = new OptionSetValue(534119999),
                            ["msdynmkt_source"] = new OptionSetValue(534120001),
                            ["msdynmkt_contactpointconsenttype"] = new OptionSetValue(534120001),
                        };
                        await client.CreateAsync(newConsent);
                    }
                    else
                    {
                        foreach (var entity in existingPreferences.Entities)
                        {
                            var existingTopic = entity.GetAttributeValue<EntityReference>("msdynmkt_topicid");
                            if (existingTopic.Id == topicId)
                            {
                                entity["msdynmkt_value"] = new OptionSetValue(534120001); // Subscribed
                                await client.UpdateAsync(entity);
                            }
                        }
                    }
                }
                // Unsubscribe topics that are not selected
                var topicsToUnsubscribe = existingTopicIds.Except(selectedTopicIds);
                foreach (var topicId in topicsToUnsubscribe)
                {
                    var entity = existingPreferences.Entities.FirstOrDefault(e => e.GetAttributeValue<EntityReference>("msdynmkt_topicid").Id == topicId);

                    if (entity != null)
                    {
                        entity["msdynmkt_value"] = new OptionSetValue(534120002); // Unsubscribed
                        await client.UpdateAsync(entity);
                    }
                }
            }
        }

    }
}
