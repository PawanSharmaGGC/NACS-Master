using CMS.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSShow.Components.Widgets.ExhibitorApplication
{
    public class ExhibitorApplicationProperties : IWidgetProperties
    {

        [TextInputComponent(Label = "Show Year", Order = 0)]
        [RequiredValidationRule]
        public string ShowYear { get; set; }

        [TextInputComponent(Label = "Location Map URL", Order = 1)]
        [RequiredValidationRule]
        public string LocationMapURL { get; set; } = "/Exhibit/Portal/Documents/AmbassadorLocations.pdf";

        [TextInputComponent(Label = "Show Hours Offset", Order = 2)]
        [RequiredValidationRule]
        public int ShowHoursOffset { get; set; } = 1;

        [TextInputComponent(Label = "Show Time Zone", Order = 3)]
        [RequiredValidationRule]
        public string ShowTimeZone { get; set; } = "Central Standard Time";

        [TextInputComponent(Label = "Time Zone Text for ICAL", Order = 4)]
        [RequiredValidationRule]
        public string TimeZoneTextForICAL { get; set; } = "Chicago local (Central) time";

        [TextInputComponent(Label = "Contact Name", Order = 5)]
        [RequiredValidationRule]
        public string ContactName { get; set; } = "Kym Selph";

        [TextInputComponent(Label = "Contact Phone", Order = 6)]
        [RequiredValidationRule]
        public string ContactPhone { get; set; } = "(703) 518-4267";

        [TextInputComponent(Label = "Contact Email", Order = 7)]
        [RequiredValidationRule]
        public string ContactEmail { get; set; } = "kselph@convenience.org";

        [TextInputComponent(Label = "Local Path", Order = 8)]
        [RequiredValidationRule]
        public string LocalPath { get; set; } = "C:\\inetpub\\wwwroot\\apps.nacsonline.com\\ExhibitorPortal\\Ambassadors\\";

    }
}