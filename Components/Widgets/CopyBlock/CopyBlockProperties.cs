using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets
{
    public class CopyBlockProperties : IWidgetProperties
    {
        [RichTextEditorComponent(Order = 1, Label = "Copy Block Content", Tooltip = "Copy Block Content")]
        public string CopyBlockContent { get; set; }
    }
}
