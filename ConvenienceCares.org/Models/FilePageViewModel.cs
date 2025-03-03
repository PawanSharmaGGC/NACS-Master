namespace ConvenienceCares.Models;

public record FilePageViewModel(string FileName, string Description)
{
    public FilePageViewModel(ConvenienceCare.File file) : this(file.FileName, file.FileDescription) { }

    public string? FileURL { get; init; }
    public string? FileDirectPath { get; init; }
    public string? FileSizeBytes { get; set; }
    public string? FileMimeType { get; set; }
};
