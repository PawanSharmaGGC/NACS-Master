namespace Convenience.org.Components.Widgets.EventRegMXRedirectSessionSignup
{
    public class EventRegMXRedirectSessionSignupViewModel
    {
        public string RegistrationSiteURL { get; set; }
        public bool ShowRegisterButton { get; set; }
        public string EventCode { get; set; }
        public string ReturnURL { get; set; }
        public string AnonymousNavigateUrl { get; set; }
        public string AuthenticatedNavigateUrl { get; set; }
        public bool ShowAnonymousPanel { get; set; }
        public bool ShowAuthenticatedPanel { get; set; }
        public string RedirectURL { get; set; }
        public string InformationMessage { get; set; }
    }
}
