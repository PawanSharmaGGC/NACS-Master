using System.ComponentModel.DataAnnotations;

namespace NACSShow.Models;

public class NACSShowNotifyMeFormViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Company { get; set; }
    public string Prospecting_SourceCode { get; set; } = "Web_2024NACSShow_NotifyMe";
    public bool SubmitToAMS { get; set; } = true;
    public string Prospecting_SourceOwnerEmail { get; set; } = "lbeck@convenience.org";
    public string Prospecting_SourceDescription { get; set; } = "Users who who want to be notified when registration opens.";

    [Required]
    public string RecaptchaResponse { get; set; } = string.Empty;

}
