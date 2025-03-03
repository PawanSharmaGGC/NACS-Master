using System.ComponentModel.DataAnnotations;

namespace NACSShow.Models;

public class MobileAppNotificationFormViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string Prospecting_SourceCode { get; set; } = "ShowMobileApp_NotifyMe_Web";
    public string Prospecting_SourceOwnerEmail { get; set; } = "lbuchanan@convenience.org";

    [Required]
    public string RecaptchaResponse { get; set; } = string.Empty;
}
