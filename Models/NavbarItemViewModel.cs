namespace Convenience.org.Models
{
    public class NavbarItemViewModel
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public bool? IsLeftNavItem { get; set; }
        public bool? IsChildInTwoColumn { get; set; } = false;
        public string? IconClass { get; set; }
        public bool? OpenInNewTab { get; set; }
    }
}
