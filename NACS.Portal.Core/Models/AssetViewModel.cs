using CMS.MediaLibrary;
using Kentico.Content.Web.Mvc;

namespace NACS.Portal.Core.Models;

public record AssetViewModel(Guid ID, string Title, IMediaFileUrl URLData, string AltText, string Extension);
public record ImageAssetViewModel(Guid ID, string Title, IMediaFileUrl URLData, string AltText, AssetDimensions Dimensions, string Extension)
   : AssetViewModel(ID, Title, URLData, AltText, Extension)
{
	public ImageAssetViewModel() : this(Guid.Empty, string.Empty, default!, string.Empty, new AssetDimensions(), string.Empty)
	{
	}
}
