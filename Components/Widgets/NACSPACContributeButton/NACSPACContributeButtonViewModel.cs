namespace Convenience.org.Components.Widgets.NACSPACContributeButton
{
    public class NACSPACContributeButtonViewModel
    {
        public bool ShowAuthorizedPanel { get; set; } = false;
        public bool ShowNotAuthorizedPanel { get; set; } = false;
        public string NavigateUrl { get; set; } = "/Advocacy/NACSPACContributions";
    }
}
