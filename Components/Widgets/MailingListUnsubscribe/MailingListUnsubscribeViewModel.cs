namespace Convenience.org.Components.Widgets.MailingListUnsubscribe
{
    public class MailingListUnsubscribeViewModel
    {
        public bool ShowErrorPanel { get; set; }
        public bool ShowUnsubscribedPanel { get; set; }
        public bool ShowPromptPanel { get; set; }
        public string ErrorMessage { get; set; }
        public string NACSKey { get; set; }
        public string MailingListKey { get; set; }
        public string MLDisplayName1 { get; set; }
        public string MLDisplayName2 { get; set; }
        public string ListName { get; set; }
    }
}
