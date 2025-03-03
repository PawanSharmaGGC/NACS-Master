using CMS.Membership;
using Convenience.org.Components.Widgets.PreferencesEmail;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Threading.Tasks;
using System.Linq;

[assembly: RegisterWidget("PreferencesEmail", typeof(PreferencesEmailViewComponent), "Email Preferences", Description = "Email Preferences Widget", IconClass = "icon-box")]

namespace Convenience.org.Components.Widgets.PreferencesEmail
{
    public class PreferencesEmailViewComponent : ViewComponent
    {
        private readonly Dynamics365DataService _dataService;

        public PreferencesEmailViewComponent(Dynamics365DataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await LoadPreferencesAsync();

            return View("~/Components/Widgets/PreferencesEmail/_PreferencesEmail.cshtml", model);
        }
        private async Task<PreferencesEmailViewModel> LoadPreferencesAsync()
        {
            var model = new PreferencesEmailViewModel();
            var user = MembershipContext.AuthenticatedUser;
            var email = user?.Email ?? "thelali@convenience.org";

            if (!string.IsNullOrEmpty(email))
            {
                var topicsTask = _dataService.GetTopicsAsync();
                var preferencesTask = _dataService.GetPreferencesAsync(email);

                await Task.WhenAll(topicsTask, preferencesTask);

                var topics = await topicsTask;
                var preferences = await preferencesTask;

                foreach (var topic in topics.Entities)
                {
                    var name = topic.GetAttributeValue<string>("msdynmkt_name");
                    var id = topic.GetAttributeValue<Guid>("msdynmkt_topicid");
                    var grouping = topic.GetAttributeValue<string>("nacs_webgrouping");

                    var isSelected = preferences.Entities.Any(pref =>
                        pref.GetAttributeValue<EntityReference>("msdynmkt_topicid").Id == id &&
                        pref.GetAttributeValue<OptionSetValue>("msdynmkt_value").Value == 534120001 // Subscribed
                    );

                    var item = new SelectListItem
                    {
                        Text = name,
                        Value = id.ToString(),
                        Selected = isSelected
                    };

                    if (grouping == "Events")
                        model.Events.Add(item);
                    else
                        model.OtherTopics.Add(item);
                }
            }

            return model;
        }
        
    }
}
