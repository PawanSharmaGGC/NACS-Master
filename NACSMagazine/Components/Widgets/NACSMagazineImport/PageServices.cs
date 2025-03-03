using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.MediaLibrary;
using CMS.Membership;
using CMS.Websites;
using CMS.Websites.Routing;
using Microsoft.AspNetCore.Http;
using NACSMagazine;
using NACSMagazine.Components.Widgets.NACSMagazineImport;
using System.Text.RegularExpressions;
using System.Xml.Linq;

public class PageServices
{
    private readonly IWebPageManager webPageManager;
    private readonly IContentQueryExecutor contentQueryExecutor;
    private readonly IContentItemManager contentItemManager;
    private readonly IContentFolderManager contentFolderManager;
    private readonly IInfoProvider<MediaLibraryInfo> mediaLibraryInfoProvider;
    private readonly IInfoProvider<MediaFileInfo> mediaFileInfoProvider;

    public PageServices(
        IContentQueryExecutor contentQueryExecutor,
        IWebPageManagerFactory webPageManagerFactory,
        IContentItemManagerFactory contentItemManagerFactory,
        IContentFolderManagerFactory contentFolderManagerFactory,
        IUserInfoProvider userInfoProvider,
        IWebsiteChannelContext websiteChannelContext, 
        IInfoProvider<MediaFileInfo> mediaFileInfoProvider,
        IInfoProvider<MediaLibraryInfo> mediaLibraryInfoProvider)
    {
        UserInfo user = userInfoProvider.Get("administrator");
        this.webPageManager = webPageManagerFactory.Create(websiteChannelContext.WebsiteChannelID, user.UserID);
        this.contentQueryExecutor = contentQueryExecutor;
        this.contentItemManager = contentItemManagerFactory.Create(user.UserID);
        this.contentFolderManager = contentFolderManagerFactory.Create(user.UserID);
        this.mediaFileInfoProvider = mediaFileInfoProvider;
        this.mediaLibraryInfoProvider = mediaLibraryInfoProvider;
    }

    public async Task<int> CreateIssuePageAsync(NACSMagazineImportViewModel model, IFormFile coverImage)
    {
        // Upload Cover Image and get GUID
        Guid coverImageGuid = Guid.Empty;
        if (coverImage != null)
        {
            var imageGuids = await UploadImagesAsync(new FormFileCollection { coverImage });
            if (imageGuids.TryGetValue(coverImage.FileName, out var guid))
            {
                coverImageGuid = guid;
            }
        }

        // Create Issue as reusable content item
        var issueItemData = new ContentItemData(new Dictionary<string, object>
        {
            { "Title", model.IssueName },
            { "IssueDate", model.IssueDate },
            { "MagazineCover1", new List<ContentItemReference> { new ContentItemReference { Identifier = coverImageGuid } } },
            { "IssueIntro", model.IssueDescription },
            { "DigitalIssueLink", model.IssuuLink }
        });

        var createIssueParams = new CreateContentItemParameters("NACSMagazine.Issue", model.IssueName, "en");
        var createdIssueId = await contentItemManager.Create(createIssueParams, issueItemData);

        // Move Issue to 'Issues' folder
        var issueFolderId = await GetContentFolderIdAsync("Issues-plapu8ia");
        await contentFolderManager.MoveItems(issueFolderId, new List<int> { createdIssueId });

        // Publish Issue reusable content
        await contentItemManager.TryPublish(createdIssueId, "en");

        // Retrieve the created Issue to get its GUID
        ContentItemQueryBuilder getCreatedIssueBuilder = new ContentItemQueryBuilder()
            .ForContentType("NACSMagazine.Issue", q =>
            {
                q.TopN(1)
                 .Where(w => w.WhereEquals("ContentItemID", createdIssueId));
            })
            .InLanguage("en");

        var createdIssues = await contentQueryExecutor.GetMappedResult<Issue>(getCreatedIssueBuilder);
        var createdIssue = createdIssues.FirstOrDefault();

        if (createdIssue == null)
        {
            throw new Exception("Failed to retrieve the created issue content item.");
        }

        // Create Issue Page and link the reusable Issue content
        var parentPageId = await GetIssuesParentPageIdAsync();
        if (parentPageId == 0)
        {
            throw new Exception("Parent page 'Issues' not found.");
        }

        var issuePageItemData = new ContentItemData(new Dictionary<string, object>
        {
            { "Title", model.IssueName },
            { "Issue", new List<ContentItemReference> { new ContentItemReference { Identifier = createdIssue.SystemFields.ContentItemGUID } } }
        });

        var createIssuePageParams = new ContentItemParameters("NACSMagazine.IssuePage", issuePageItemData);
        var createPageParameters = new CreateWebPageParameters(model.IssueName, "en", createIssuePageParams)
        {
            ParentWebPageItemID = parentPageId
        };

        var issuePageId = await webPageManager.Create(createPageParameters);
        await webPageManager.TryPublish(issuePageId, "en");

        return issuePageId;
    }
    public async Task<bool> PublishPageAsync(int pageId)
    {
        return await webPageManager.TryPublish(pageId, "en");
    }
    public async Task CreateArticlesAsync(int issuePageId, string xmlContent, IFormFileCollection images)
    {
        XDocument xmlDoc = XDocument.Parse(xmlContent);
        var articles = xmlDoc.Descendants("Article");

        // Upload all images and get their GUIDs
        var imageGuids = await UploadImagesAsync(images);

        // Get Content Folder IDs dynamically
        var articleFolderId = await GetContentFolderIdAsync("Articles-ordaiv8x");
        var authorFolderId = await GetContentFolderIdAsync("Authors-eo6pugw8");

        foreach (var article in articles)
        {
            var title = article.Element("Title")?.Value;
            var ledeText = article.Element("LedeText")?.Value;
            var issueDate = article.Element("IssueDate")?.Value;
            var magazineSection = article.Element("MagazineSection")?.Value;
            var pageContent = article.Element("PageContent")?.Value;
            var teaserText = ExtractTeaser(pageContent);
            var rollupImageFilename = article.Element("RollupImage")?.Attribute("href")?.Value;
            Guid rollupImageGuid = imageGuids.TryGetValue(rollupImageFilename, out var guid) ? guid : Guid.Empty;

            pageContent = ReplaceImageHrefsWithUrls(pageContent, imageGuids);

            // Create Authors as reusable content items (if present)
            var authorElements = article.Elements("Author").ToList();
            List<ContentItemReference> authorReferences = new List<ContentItemReference>();

            if (authorElements.Any()) 
            {
                foreach (var authorElement in authorElements)
                {
                    var authorName = authorElement.Element("Name")?.Value;
                    var authorBio = authorElement.Element("Bio")?.Value;
                    var authorPhotoFilename = authorElement.Element("Photo")?.Attribute("href")?.Value;

                    if (!string.IsNullOrWhiteSpace(authorName))
                    {
                        var authorGuid = await CreateOrRetrieveAuthorAsync(authorName, authorBio, authorPhotoFilename, authorFolderId, imageGuids);
                        if (authorGuid != Guid.Empty) 
                        {
                            authorReferences.Add(new ContentItemReference { Identifier = authorGuid });
                        }
                    }
                }
            }

            // Create Article as a reusable content item
            var articleItemData = new ContentItemData(new Dictionary<string, object>
            {
                { "Title", title },
                { "LedeText", ledeText },
                { "IssueDate", issueDate },
                { "PageContent", pageContent },
                { "PageContentTeaser", teaserText },
                { "MagazineSection", magazineSection },
                { "RollupImage1", new List<ContentItemReference> { new ContentItemReference { Identifier = rollupImageGuid } } },
                { "Authors", authorReferences } 
            });

            var createArticleParams = new CreateContentItemParameters("NACSMagazine.Article", title, "en");

            var createdArticleId = await contentItemManager.Create(createArticleParams, articleItemData);
            await contentFolderManager.MoveItems(articleFolderId, new List<int> { createdArticleId });
            await contentItemManager.TryPublish(createdArticleId, "en");

            // Retrieve the created article to get its GUID
            ContentItemQueryBuilder getCreatedArticleBuilder = new ContentItemQueryBuilder()
                .ForContentType("NACSMagazine.Article", q =>
                {
                    q.TopN(1)
                     .Where(w => w.WhereEquals("ContentItemID", createdArticleId));
                })
                .InLanguage("en");

            var createdArticles = await contentQueryExecutor.GetMappedResult<Article>(getCreatedArticleBuilder);
            var createdArticle = createdArticles.FirstOrDefault();

            if (createdArticle == null)
            {
                throw new Exception("Failed to retrieve the created article content item.");
            }

            // Create Article Page under Issue Page
            var articlePageItemData = new ContentItemData(new Dictionary<string, object>
            {
                { "ArticleContent", new List<ContentItemReference> { new ContentItemReference { Identifier = createdArticle.SystemFields.ContentItemGUID } } }
            });

            var createArticlePageParams = new ContentItemParameters("NACSMagazine.ArticlePage", articlePageItemData);
            var createPageParameters = new CreateWebPageParameters(title, "en", createArticlePageParams)
            {
                ParentWebPageItemID = issuePageId
            };

            var articlePageId = await webPageManager.Create(createPageParameters);
            await webPageManager.TryPublish(articlePageId, "en");
        }

    }
    private async Task<Guid> CreateOrRetrieveAuthorAsync(string name, string bio, string photoFilename, int authorFolderId, Dictionary<string, Guid> imageGuids)
    {
        string AUTHOR_CONTENT_TYPE = "NACSMagazine.Author";
        string languageName = "en";

        ContentItemQueryBuilder builder = new ContentItemQueryBuilder()
            .ForContentType(AUTHOR_CONTENT_TYPE, q =>
            {
                q.TopN(1)
                 .Where(w => w.WhereEquals("FullName", name));
            })
            .InLanguage(languageName);

        var authors = await contentQueryExecutor.GetMappedResult<Author>(builder);
        var existingAuthor = authors.FirstOrDefault();

        if (existingAuthor != null)
        {
            return existingAuthor.SystemFields.ContentItemGUID;
        }

        // Retrieve image GUID for Headshot
        Guid headshotGuid = imageGuids.TryGetValue(photoFilename, out var guid) ? guid : Guid.Empty;


        // Create Author
        var authorData = new ContentItemData(new Dictionary<string, object>
        {
            { "FullName", name },
            { "Bio", bio },
            { "Headshot", new List<ContentItemReference> { new ContentItemReference { Identifier = headshotGuid } } }
        });

        var createAuthorParams = new CreateContentItemParameters(AUTHOR_CONTENT_TYPE, name, languageName);
        var createdAuthorId = await contentItemManager.Create(createAuthorParams, authorData);

        await contentFolderManager.MoveItems(authorFolderId, new List<int> { createdAuthorId });

        await contentItemManager.TryPublish(createdAuthorId, "en");

        ContentItemQueryBuilder getCreatedAuthorBuilder = new ContentItemQueryBuilder()
            .ForContentType(AUTHOR_CONTENT_TYPE, q =>
            {
                q.TopN(1)
                 .Where(w => w.WhereEquals("ContentItemID", createdAuthorId));
            })
            .InLanguage(languageName);

        var createdAuthors = await contentQueryExecutor.GetMappedResult<Author>(getCreatedAuthorBuilder);
        var createdAuthor = createdAuthors.FirstOrDefault();

        if (createdAuthor != null)
        {
            return createdAuthor.SystemFields.ContentItemGUID;
        }

        throw new Exception("Failed to retrieve created author content item.");
    }
    private string ExtractTeaser(string content)
    {
        var match = Regex.Match(content, @"<p>(.*?)</p>");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
    private async Task<int> GetIssuesParentPageIdAsync()
    {
        string ISSUES_CONTENT_TYPE = "NACSMagazine.ArchivePage";
        string languageName = "en";

        ContentItemQueryBuilder builder =
            new ContentItemQueryBuilder()
                .ForContentType(ISSUES_CONTENT_TYPE, subqueryParameters =>
                {
                    subqueryParameters.ForWebsite("NACSMagazine", PathMatch.Children("/"));
                    subqueryParameters.TopN(1);
                })
                .InLanguage(languageName);

        var pages = await contentQueryExecutor.GetWebPageResult(builder, rowData => rowData.WebPageItemID);
        return pages.FirstOrDefault();
    }
    private async Task<int> GetContentFolderIdAsync(string folderCodeName)
    {
        var folder = await contentFolderManager.Get(folderCodeName);
        if (folder == null)
        {
            throw new Exception($"Content folder '{folderCodeName}' not found.");
        }
        return folder.ContentFolderID;
    }
    private async Task<Dictionary<string, Guid>> UploadImagesAsync(IFormFileCollection images)
    {
        var imageGuids = new Dictionary<string, Guid>();

        if (images == null || images.Count == 0)
            return imageGuids;

        // Get the target media library
        var library = await mediaLibraryInfoProvider.GetAsync("NACSMagazine");
        if (library == null)
            throw new Exception("Media library 'NACSMagazine' not found.");

        foreach (var image in images)
        {
            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(image.FileName);
            var fileExtension = System.IO.Path.GetExtension(image.FileName);
            var filePath = $"{fileNameWithoutExtension}{fileExtension}";

            // Check if the file already exists in the media library
            var existingFile = MediaFileInfoProvider.GetMediaFileInfo(library.LibraryID, filePath);

            if (existingFile != null)
            {
                existingFile.FileDescription = $"Updated on {DateTime.UtcNow}"; // Optional metadata update
                mediaFileInfoProvider.Set(existingFile);

                // Store the existing file GUID
                imageGuids[image.FileName] = existingFile.FileGUID;
                continue; 
            }

            // Save the uploaded file to a temporary location
            var tempFilePath = System.IO.Path.GetTempFileName();
            try
            {
                using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }


                var mediaFile = new MediaFileInfo(tempFilePath, library.LibraryID)
                {
                    FileName = fileNameWithoutExtension,
                    FileTitle = fileNameWithoutExtension,
                    FilePath = filePath,
                    FileExtension = fileExtension,
                    FileMimeType = image.ContentType,
                    FileLibraryID = library.LibraryID,
                    FileSize = image.Length
                };

                mediaFileInfoProvider.Set(mediaFile);

                // Store the newly uploaded file GUID
                imageGuids[image.FileName] = mediaFile.FileGUID;
            }
            finally
            {
                // Ensure the temporary file is deleted
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }
            }
        }

        return imageGuids;
    }
    private string ReplaceImageHrefsWithUrls(string pageContent, Dictionary<string, Guid> imageGuids)
    {
        if (string.IsNullOrWhiteSpace(pageContent))
        {
            return pageContent;
        }

        return Regex.Replace(pageContent, @"<img\s+href=""([^""]+)""\s*/?>", match =>
        {
            string filename = match.Groups[1].Value;
            if (imageGuids.TryGetValue(filename, out var guid))
            {
                // Generate the correct image URL
                string imageUrl = $"/getmedia/{guid}/{filename}"; 
                return $"<img src=\"{imageUrl}\" />";
            }
            return match.Value;
        });
    }
}
