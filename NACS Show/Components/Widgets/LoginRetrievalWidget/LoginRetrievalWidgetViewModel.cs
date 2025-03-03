namespace NACSShow.Components.Widgets.LoginRetrievalWidget
{
    public class LoginRetrievalWidgetViewModel
    {
        public bool IsPostback { get; set; } = false;
        public string ConfirmationNumber { get; set; }  = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordResetUrl { get; set; } = string.Empty;
        public string ErrorMsg { get; set; } = string.Empty;
    }
}