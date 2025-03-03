namespace ConvenienceCares.Helpers;

public class Helpers
{
    public static string GetPagePathLastSegment(string pageUrlPath)
    {
        if (string.IsNullOrEmpty(pageUrlPath)) return string.Empty;
        int lastSlashIndex = pageUrlPath.LastIndexOf('/');
        var pageLastSegment = lastSlashIndex >= 0 ? pageUrlPath.Substring(lastSlashIndex + 1) : pageUrlPath;
        return pageLastSegment.ToLowerInvariant();
    }

    public static string GetMimeType(string fileExtension)
    {
        return fileExtension.ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".bmp" => "image/bmp",
            ".svg" => "image/svg+xml",
            ".txt" => "text/plain",
            ".pdf" => "application/pdf",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => "application/octet-stream",
        };
    }

}
