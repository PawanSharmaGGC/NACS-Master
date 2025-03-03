using System.ComponentModel.DataAnnotations;

namespace NACSShow.Models;

public class NSVirtualExperienceAlertFormViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string Prospecting_SourceCode { get; set; } = "NACSShowVirtual_NotifyMe_Web";
    public string Prospecting_SourceOwnerEmail { get; set; } = "lbeck@convenience.org";
    public string Prospecting_SourceDescription { get; set; } = "Users who who want to be notified when registration opens.";

    [Required]
    public string RecaptchaResponse { get; set; } = string.Empty;

}
