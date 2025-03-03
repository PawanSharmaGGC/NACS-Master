namespace ConvenienceCares.Models;

public class NACS_24_7DayFormViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Title { get; set; }
    public string? Company { get; set; }

    public string? Email { get; set; }
    public string? Phone { get; set; }
    public List<string> GetInvolved { get; set; } = new List<string>();
    public string? AnthingElse { get; set; }
    public List<string> Intesrests { get; set; } = new List<string>();

    public string Prospecting_SourceCode { get; set; } = "Foundation_247Day_Web";
    public string Prospecting_SourceOwnerEmail { get; set; } = "ssikorski@convenience.org";
}
