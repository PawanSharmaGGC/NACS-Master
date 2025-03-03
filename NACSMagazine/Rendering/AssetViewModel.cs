using CMS.MediaLibrary;

using Kentico.Content.Web.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACSMagazine.Rendering
{
    public record AssetViewModel(Guid ID, string Title, IMediaFileUrl URLData, string AltText, string Extension);
    public record ImageAssetViewModel(Guid ID, string Title, IMediaFileUrl URLData, string AltText, AssetDimensions Dimensions, string Extension)
        : AssetViewModel(ID, Title, URLData, AltText, Extension);
}
