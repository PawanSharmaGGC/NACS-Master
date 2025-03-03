using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.Admin.Base.FormAnnotations;

namespace Convenience.org.Components.Widgets.PriceCard;

public class PriceCardWidgetProperties: IWidgetProperties
{
    [TextInputComponent(Label = "Eyebrow Title", Order = 1)]
    public string EyebrowTitle { get; set; } = string.Empty;

    [TextInputComponent(Label = "Title", Order = 2)]
    public string Title { get; set; }

    [RichTextEditorComponent(Label = "Description", Order = 3)]
    public string Description { get; set; }

    [RichTextEditorComponent(Label = "Retail Members Price", Order = 4)]
    public string RetailMembersPrice { get; set; }

    [RichTextEditorComponent(Label = "Others", Order = 5)]
    public string Others { get; set; }

    [TextInputComponent(Label = "CTA Text", Order = 6)]
    public string CTAText { get; set; }

    [TextInputComponent(Label = "CTA Url", Order = 7)]
    public string CTAUrl { get; set; }

}
