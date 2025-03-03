using Kentico.PageBuilder.Web.Mvc;

namespace NACSShow.Components.Widgets.LeftNavigationWidget;

public class LeftNavigationWidgetProperties : IWidgetProperties
{
    public bool ShowChildrenOfChildren { get; set; } = true;
}
