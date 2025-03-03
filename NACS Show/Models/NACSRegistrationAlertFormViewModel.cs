using System.ComponentModel.DataAnnotations;

namespace NACSShow.Models;

public class NACSRegistrationAlertFormViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }

    [Required]
    public string RecaptchaResponse { get; set; } = string.Empty;
}
