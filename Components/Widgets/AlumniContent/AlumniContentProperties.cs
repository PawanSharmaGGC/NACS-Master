using Kentico.PageBuilder.Web.Mvc;

namespace Convenience.org.Components.Widgets.AlumniContent
{
    public class AlumniContentProperties : IWidgetProperties
    {
        public int Limit { get; set; } = 10;

        public string Topics { get; set; }
    }
}
