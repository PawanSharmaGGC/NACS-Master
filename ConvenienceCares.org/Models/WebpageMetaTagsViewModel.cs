using ConvenienceCares.org;

namespace ConvenienceCares.Models;

public record WebpageMetaTagsViewModel(string DefaultSocialMediaHandler, string DefaultMetaTitle, string DefaultMetaDescription, string DefaultMetaImageUrl)
{
    public WebpageMetaTagsViewModel(ISocialMetaTags meta) : this(meta.SocialMediaHandler, meta.DefaultTitle, meta.DefaultDescription, meta.DefaultImage) { }
};