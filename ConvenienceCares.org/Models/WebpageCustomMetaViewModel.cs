namespace ConvenienceCares.Models;

public class WebpageCustomMetaViewModel
{
    public static WebpageCustomMetaViewModel Empty { get; } = new WebpageCustomMetaViewModel();

    public WebpageCustomMetaViewModel(WebpageMetaTagsViewModel meta)
    {
        SocialMediaHandler = meta.DefaultSocialMediaHandler;
        Title = meta.DefaultMetaTitle;
        Description = meta.DefaultMetaDescription;
        OGImageURL = meta.DefaultMetaImageUrl;
    }

    private WebpageCustomMetaViewModel()
    {
        SocialMediaHandler = "";
        Title = "";
        Description = "";
        OGImageURL = "";
    }

    public string SocialMediaHandler { get; set; } = "";
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string OGImageURL { get; init; } = "";
}

