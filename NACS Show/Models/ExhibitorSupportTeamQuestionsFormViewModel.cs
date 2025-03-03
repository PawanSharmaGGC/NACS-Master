using System.ComponentModel.DataAnnotations;

namespace NACSShow.Models;

public class ExhibitorSupportTeamQuestionsFormViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public string? Company { get; set; }
    public string? Email { get; set; }
    public string? Question { get; set; }

    [Required]
    public string RecaptchaResponse { get; set; } = string.Empty;

}
