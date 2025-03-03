using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets
{
    public class TableProperties: IWidgetProperties
    {
        [RichTextEditorComponent(Order = 1, Label = "Table Content", Tooltip = "Table Content")]
        public string TableContent { get; set; }
    }
}
