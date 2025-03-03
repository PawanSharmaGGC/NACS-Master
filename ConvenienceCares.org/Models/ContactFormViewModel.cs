using System.ComponentModel.DataAnnotations;

namespace ConvenienceCares.Models;

public class ContactFormViewModel
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public string? Channel { get; set; }
	public string? Comments { get; set; }

	[Required]
	public string RecaptchaResponse { get; set; } = string.Empty;
}

