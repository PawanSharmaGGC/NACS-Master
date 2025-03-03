using CMS.MediaLibrary;
using System.Collections.Generic;
using System.Linq;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets
{
    public class PullQuoteProperties: IWidgetProperties
    {
        [TextInputComponent(Order = 0, Label = "Quote", Tooltip = "Quote")]
        public string Quote { get; set; }
        [TextInputComponent(Order = 1, Label = "Author Name", Tooltip = "Author Name")]
        public string AuthorName { get; set; }
    }
}
