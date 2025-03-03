using System.ComponentModel.DataAnnotations;

namespace ConvenienceCares.Models;

public class CorporateInvolvementFormViewModel
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Title { get; set; }
	public string? Company { get; set; }
	public string? Phone { get; set; }
	public string? Email { get; set; }
	public string? MoreInfo { get; set; }
	public string Prospecting_SourceCode { get; set; } = "Foundation_247Day_Corp_Web";
	public string Prospecting_SourceOwnerEmail { get; set; } = "ssikorski@convenience.org";

	[Required]
	public string RecaptchaResponse { get; set; } = string.Empty;
}
