using CMS.MediaLibrary;
using NACS.Portal.Core.Models;
using Kentico.Content.Web.Mvc;

namespace NACS.Portal.Core.Services;

public interface IAssetItemService
{
    Task<AssetViewModel?> RetrieveMediaFile(AssetRelatedItem? item);
    Task<IReadOnlyList<ImageAssetViewModel?>> RetrieveMediaFileImages(IEnumerable<AssetRelatedItem> items);
    Task<ImageAssetViewModel?> RetrieveMediaFileImage(AssetRelatedItem? item);
    string BuildFullFileUrl(IMediaFileUrl url);
}
