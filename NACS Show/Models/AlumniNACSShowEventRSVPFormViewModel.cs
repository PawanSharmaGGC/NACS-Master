using System.ComponentModel.DataAnnotations;

namespace NACSShow.Models;

public class AlumniNACSShowEventRSVPFormViewModel 
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CompanyName { get; set; }
    public string? Email { get; set; }
    public string? RSVP { get; set; }

    [Required]
    public string RecaptchaResponse { get; set; } = string.Empty;
}
