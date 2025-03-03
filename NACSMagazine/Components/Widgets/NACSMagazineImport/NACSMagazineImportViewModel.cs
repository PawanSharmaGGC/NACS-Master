using Microsoft.AspNetCore.Http;

namespace NACSMagazine.Components.Widgets.NACSMagazineImport
{
    public class NACSMagazineImportViewModel
    {
        public string IssueName { get; set; }
        public DateTime IssueDate { get; set; }
        public IFormFile MagazineCoverImage { get; set; }
        public string IssueDescription { get; set; }
        public string IssuuLink { get; set; }
        public IFormFile IssueXmlFile { get; set; }
        public IFormFileCollection Images { get; set; }
    }
}
