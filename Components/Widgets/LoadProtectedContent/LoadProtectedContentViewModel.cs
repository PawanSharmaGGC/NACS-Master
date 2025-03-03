namespace Convenience.org.Components.Widgets.LoadProtectedContent
{
    public class LoadProtectedContentViewModel
    {
        public int TimeToExpire { get; set; }
        public string ContactEmail { get; set; } 
        public string? RedirectURL { get; set; } 
        public string? ErrorMessage { get; set; } 
        public bool ShowContactEmailPanel { get; set; } = false;
    }
}
