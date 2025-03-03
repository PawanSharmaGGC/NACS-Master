using CMS.MediaLibrary;
using ConvenienceCares.Models;
using Kentico.Content.Web.Mvc;

namespace ConvenienceCares.Interface.Services;

public interface IAssetItemService
{
    Task<AssetViewModel?> RetrieveMediaFile(AssetRelatedItem? item);
    Task<IReadOnlyList<ImageAssetViewModel?>> RetrieveMediaFileImages(IEnumerable<AssetRelatedItem> items);
    Task<ImageAssetViewModel?> RetrieveMediaFileImage(AssetRelatedItem? item);
    string BuildFullFileUrl(IMediaFileUrl url);
}
