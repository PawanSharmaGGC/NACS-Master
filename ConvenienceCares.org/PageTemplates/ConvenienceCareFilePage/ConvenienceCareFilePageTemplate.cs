using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ConvenienceCares.Operations;
using Kentico.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using ConvenienceCares.Models;
using ConvenienceCares.PageTemplates;
using Kentico.Content.Web.Mvc.Routing;
using CMS.MediaLibrary;
using Microsoft.AspNetCore.Hosting;
using CMS.DataEngine;
using CMS.Core;
using Path = System.IO.Path;
using NACS.Portal.Core.Services;
using NACS.Portal.Core.Models;

[assembly: RegisterWebPageRoute(ConvenienceCare.File.CONTENT_TYPE_NAME, typeof(ConvenienceCareFilePageTemplateController), WebsiteChannelNames = new[] { Constants.WEBSITE_CHANNEL_NAME })]

namespace ConvenienceCares.PageTemplates;

public class ConvenienceCareFilePageTemplateController : Controller
{
    private readonly IMediator mediator;
    private readonly IWebPageDataContextRetriever webPageDataContextRetriever;
    private readonly IAssetItemService itemService;
    private readonly IHttpContextAccessor contextAccessor;
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IInfoProvider<MediaFileInfo> mediaFileInfoProvider;
    private readonly IEventLogService log;


    public ConvenienceCareFilePageTemplateController(IMediator mediator, IWebPageDataContextRetriever webPageDataContextRetriever,
        IAssetItemService itemService,
        IHttpContextAccessor contextAccessor,
        IWebHostEnvironment webHostEnvironment,
        IInfoProvider<MediaFileInfo> mediaFileInfoProvider,
        IEventLogService log)
    {
        this.mediator = mediator;
        this.webPageDataContextRetriever = webPageDataContextRetriever;
        this.itemService = itemService;
        this.contextAccessor = contextAccessor;
        this.webHostEnvironment = webHostEnvironment;
        this.mediaFileInfoProvider = mediaFileInfoProvider;
        this.log = log;
    }


    public async Task<IActionResult> Index()
    {
        if (!webPageDataContextRetriever.TryRetrieve(out var data))
        {
            return NotFound();
        }

        var filePage = await mediator.Send(new FilePageQuery(data.WebPage));
        var file = new FilePageViewModel(filePage);

        try
        {
            if (filePage.FileAttachment?.Any() == true)
            {
                var mediaFile = filePage.FileAttachment.ToList().FirstOrDefault();
                if (mediaFile != null)
                {
                    var asset = await itemService.RetrieveMediaFile(mediaFile);

                    if (asset != null)
                    {
                        string fileUrlDirectPath = GetFileUrlDirectPath(asset);
                        byte[] fileBytes = await LoadFileBytesAsync(fileUrlDirectPath);
                        string fileUrl = itemService.BuildFullFileUrl(asset.URLData);
                        string fileExtension = Path.GetExtension(fileUrl);
                        string mimeType = Helpers.Helpers.GetMimeType(fileExtension);

                        file = file with
                        {
                            FileURL = fileUrl,
                            FileSizeBytes = fileBytes?.LongLength.ToString(),
                            FileDirectPath = fileUrlDirectPath.TrimStart('~', '/'),
                            FileMimeType = mimeType
                        };

                        if (!contextAccessor.HttpContext.Kentico().PageBuilder().EditMode && !contextAccessor.HttpContext.Kentico().Preview().Enabled)
                        {
                            return File(fileBytes ?? Array.Empty<byte>(), file.FileMimeType);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.LogException(nameof(ConvenienceCareFilePageTemplateController), nameof(Index), ex, $"Tree Path: {filePage.SystemFields.WebPageItemTreePath}");
        }
        return new TemplateResult(file);
    }

    private string GetFileUrlDirectPath(AssetViewModel asset)
    {
        try
        {
            var (fileDirectoryContainer, fileStoragePath) = GetStoragePaths();

            string fileUrlDirectPath = Path.Combine(fileDirectoryContainer, asset.URLData.DirectPath.TrimStart('~', '/'));

            if (asset.URLData.DirectPath.Contains("getmedia", StringComparison.OrdinalIgnoreCase))
            {
                var mediaInfo = mediaFileInfoProvider.GetAsync(asset.ID).Result;
                return $"{fileDirectoryContainer}/{fileStoragePath}/{mediaInfo.Parent.GetProperty("CodeName")}/{mediaInfo.FilePath}";
            }

            return fileUrlDirectPath;
        }
        catch (Exception ex)
        {
            log.LogException(nameof(ConvenienceCareFilePageTemplateController), nameof(GetFileUrlDirectPath), ex);
        }
        return "";
    }

    private (string fileDirectoryContainer, string fileStoragePath) GetStoragePaths()
    {

        const string containerName = Constants.CONTAINER_NAME;
        const string azureStorageAssetsPath = Constants.AZURE_STORAGE_ASSETS_PATH;

        try
        {
            if (webHostEnvironment.EnvironmentName.Contains("development", StringComparison.OrdinalIgnoreCase))
            {
                return ($"{Constants.LOCAL_STORAGE_ASSETS_DIRECTORY_NAME}/{containerName}", Constants.LOCAL_STORAGE_ASSETS_PATH);
            }

            return (containerName, azureStorageAssetsPath);
        }
        catch (Exception ex)
        {
            log.LogException(nameof(ConvenienceCareFilePageTemplateController), nameof(GetStoragePaths), ex);
        }
        return (containerName, azureStorageAssetsPath);
    }

    private async Task<byte[]> LoadFileBytesAsync(string filePath)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath.TrimStart('~', '/'));
        return await System.IO.File.ReadAllBytesAsync(fullPath);
    }



    [HttpGet]
    [Route("/ConvenienceCareFileTemplate/OpenFile")]
    public IActionResult OpenFile(string fileUrl, string fileMimeType)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return BadRequest("File URL cannot be null or empty.");
        }
        byte[] fileBytes = System.IO.File.ReadAllBytes(fileUrl);
        return File(fileBytes, fileMimeType);
    }

}

