using System.ComponentModel.DataAnnotations;

namespace ConvenienceCares.Models;

public class AwarenessCampaignFormViewModel
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Company { get; set; }
	public string? Title { get; set; }
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public string? AdditionalNotes { get; set; }
	public List<string> Consent { get; set; } = new List<string>();

	public string Prospecting_SourceCode { get; set; } = "Foundation_AwarenessCampaign_Web";
	public string Prospecting_SourceOwnerEmail { get; set; } = "ssikorski@convenience.org";

	[Required]
	public string RecaptchaResponse { get; set; } = string.Empty;
}
