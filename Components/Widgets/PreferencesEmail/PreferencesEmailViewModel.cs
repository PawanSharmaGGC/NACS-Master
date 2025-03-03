using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.PreferencesEmail
{
    public class PreferencesEmailViewModel
    {
        public List<SelectListItem> Events { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> OtherTopics { get; set; } = new List<SelectListItem>();
        public bool UnsubscribeAll { get; set; }
        public List<string> SelectedEvents { get; set; } = new List<string>();
        public List<string> SelectedOtherTopics { get; set; } = new List<string>();
    }
}
