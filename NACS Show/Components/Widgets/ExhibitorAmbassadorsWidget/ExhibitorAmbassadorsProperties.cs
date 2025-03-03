using CMS.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSShow.Components.Widgets.ExhibitorAmbassadorsWidget
{
    public class ExhibitorAmbassadorsProperties : IWidgetProperties
    {
        [TextInputComponent(Label = "Show Year", Order = 0)]
        [RequiredValidationRule]
        public string ShowYear { get; set; } = "2023";

        [TextInputComponent(Label = "Location Map URL", Order = 1)]
        [RequiredValidationRule]
        public string LocationMapURL { get; set; } = "/NACSShow/media/Exhibit/Portal/Documents/AmbassadorLocations.pdf";

        [TextInputComponent(Label = "Show Hours Offset", Order = 2)]
        [RequiredValidationRule]
        public string ShowHoursOffset { get; set; } = "3";

        [TextInputComponent(Label = "Show Time Zone", Order = 3)]
        [RequiredValidationRule]
        public string ShowTimeZone { get; set; } = "Pacific Standard Time";

        [TextInputComponent(Label = "Time Zone Text for ICAL", Order = 4)]
        public string TimeZoneTextForICAL { get; set; } = "Atlanta local (Eastern) time";

        [TextInputComponent(Label = "Contact Name", Order = 5)]
        [RequiredValidationRule]
        public string ContactName { get; set; } = "Erin Garay";

        [TextInputComponent(Label = "Contact Phone", Order = 6)]
        [RequiredValidationRule]
        public string ContactPhone { get; set; } = "(703) 518-4244";

        [TextInputComponent(Label = "Contact Email", Order = 7)]
        [RequiredValidationRule]
        public string ContactEmail { get; set; } = "egaray@convenience.org";

        [TextInputComponent(Label = "Local Path", Order = 8)]
        [RequiredValidationRule]
        public string LocalPath { get; set; } = "C:\\inetpub\\wwwroot\\apps.nacsonline.com\\ExhibitorPortal\\Ambassadors\\";
    }
}
