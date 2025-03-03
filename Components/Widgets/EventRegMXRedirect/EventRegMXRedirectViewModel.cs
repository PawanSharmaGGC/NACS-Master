namespace Convenience.org.Components.Widgets.EventRegMXRedirect
{
    public class EventRegMXRedirectViewModel
    {
        public string RegistrationSiteURL { get; set; }
        public string RegistrationSite_Production { get; set; }
        public string RegistrationSite_Staging { get; set; }
        public string AnonymousNavigateUrl { get; set; }
        public string AuthenticatedNavigateUrl { get; set; }
        public bool ShowAnonymousPanel { get; set; }
        public bool ShowAuthenticatedPanel { get; set; }
        public string RedirectURL { get; set; }
    }
}
