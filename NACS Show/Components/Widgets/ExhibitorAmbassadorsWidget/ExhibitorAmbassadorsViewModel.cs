using Kentico.Xperience.Admin.Base.FormAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSShow.Components.Widgets.ExhibitorAmbassadorsWidget
{
    public class ExhibitorAmbassadorsViewModel
    {
        public string? ShowYear { get; set; }
        public string? LocationMapURL { get; set; }
        public int ShowHoursOffset { get; set; }
        public string? ShowTimeZone { get; set; }
        public string? TimeZoneTextForICAL { get; set; }
        public string? ContactName { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? LocalPath { get; set; }
    }
}
