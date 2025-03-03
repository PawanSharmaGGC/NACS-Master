using System.ComponentModel.DataAnnotations;

namespace ConvenienceCares.Models;

public class ScholarshipUpdatesFormViewModel
{
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? Email { get; set; }
}
