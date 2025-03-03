namespace NACSShow.Models;

public class LeftNavItems
{
    public string LeftNavTitle { get; set; }
    public List<NavItem> NavItems { get; set; }
}


public class NavItem
{
    public string Title { get; set; }
    public string URL { get; set; }
    public string Target { get; set; }
    public string CssClass { get; set; }
    public List<NavItem> ChildItems { get; set; }
}
