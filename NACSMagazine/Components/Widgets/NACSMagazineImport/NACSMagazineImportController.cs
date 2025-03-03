using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace NACSMagazine.Components.Widgets.NACSMagazineImport
{
    [Route("NACSMagazineImport")]
    [Route("api/nacsmagazine")]
    [ApiController]
    public class NACSMagazineImportController : Controller
    {
        private readonly PageServices pageServices;

        public NACSMagazineImportController(PageServices pageServices)
        {
            this.pageServices = pageServices;
        }

        [HttpPost("UploadIssue")]
        public async Task<IActionResult> UploadIssue([FromForm] NACSMagazineImportViewModel model)
        {
            if (!ModelState.IsValid || model.IssueXmlFile == null)
            {
                return BadRequest(new { success = false, message = "Invalid input data or missing XML file." });
            }

            try
            {
                var xmlContent = await ReadXmlFileAsync(model.IssueXmlFile);

                // Validate XML structure before processing
                var validationMessage = ValidateXmlStructure(xmlContent);
                if (!string.IsNullOrEmpty(validationMessage))
                {
                    return BadRequest(new { success = false, message = $"Invalid XML format: {validationMessage}" });
                }

                // Create the Issue Page
                var issuePageId = await pageServices.CreateIssuePageAsync(model, model.MagazineCoverImage);
                if (issuePageId == 0)
                {
                    return BadRequest(new { success = false, message = "Failed to create issue page." });
                }

                // Create Articles (as content items and pages) and link to Authors
                await pageServices.CreateArticlesAsync(issuePageId, xmlContent, model.Images);

                //  Publish the Issue Page
                await pageServices.PublishPageAsync(issuePageId);

                return Ok(new { success = true, message = "Issue, articles, and authors created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred.", error = ex.Message });
            }
        }
        private string ValidateXmlStructure(string xmlContent)
        {
            try
            {
                XDocument xmlDoc = XDocument.Parse(xmlContent, LoadOptions.SetLineInfo); 

                // Check for required root elements
                var issueElement = xmlDoc.Element("Issue");
                if (issueElement == null)
                    return "Missing <Issue> root element.";

                var articles = issueElement.Elements("Article");
                if (!articles.Any())
                    return "No <Article> elements found in <Issue>.";

                foreach (var article in articles)
                {
                    IXmlLineInfo lineInfo = (IXmlLineInfo)article;

                    // Validate required article fields
                    if (article.Element("Title") == null)
                        return $"Missing <Title> in an <Article> (Line {lineInfo.LineNumber}).";

                    //if (article.Element("LedeText") == null)
                    //    return $"Missing <LedeText> in an <Article> (Line {lineInfo.LineNumber}).";

                    if (article.Element("IssueDate") == null)
                        return $"Missing <IssueDate> in an <Article> (Line {lineInfo.LineNumber}).";

                    if (article.Element("MagazineSection") == null)
                        return $"Missing <MagazineSection> in an <Article> (Line {lineInfo.LineNumber}).";

                    if (article.Element("PageContent") == null)
                        return $"Missing <PageContent> in an <Article> (Line {lineInfo.LineNumber}).";

                    // Validate author structure (optional)
                    var authorElement = article.Element("Author");
                    if (authorElement != null) 
                    {
                        IXmlLineInfo authorLineInfo = (IXmlLineInfo)authorElement;

                        if (authorElement.Element("Name") == null)
                            return $"Missing <Name> in <Author> (Line {authorLineInfo.LineNumber}).";

                        if (authorElement.Element("Bio") == null)
                            return $"Missing <Bio> in <Author> (Line {authorLineInfo.LineNumber}).";

                        var photoAttr = authorElement.Element("Photo")?.Attribute("href");
                        if (photoAttr == null || string.IsNullOrEmpty(photoAttr.Value))
                            return $"Missing or empty href in <Photo> in <Author> (Line {authorLineInfo.LineNumber}).";
                    }
                }

                return ""; // XML is valid
            }
            catch (Exception ex)
            {
                return $"XML parsing error: {ex.Message}";
            }
        }
        private async Task<string> ReadXmlFileAsync(IFormFile xmlFile)
        {
            using var stream = xmlFile.OpenReadStream();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
