using System;

namespace Convenience.org.Models
{
    public class SocialShareViewModel
    {
        private const string DefaultPageUrl = "#";
        public string LinkedinUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string MailUrl { get; set; }
        public string LinkUrl { get; set; }

        public static SocialShareViewModel GetViewModel(string pageUrl)
        {
            string encodedPageUrl = Uri.EscapeDataString(string.IsNullOrEmpty(pageUrl) ? DefaultPageUrl : pageUrl);

            return new SocialShareViewModel()
            {
                LinkedinUrl = $"https://www.linkedin.com/shareArticle?mini=true&url={encodedPageUrl}",
                TwitterUrl = $"https://twitter.com/intent/tweet?url={encodedPageUrl}",
                FacebookUrl = $"https://www.facebook.com/sharer/sharer.php?u={encodedPageUrl}",
                InstagramUrl = "https://www.instagram.com/",
                MailUrl = $"mailto:?subject=Check this out&body={encodedPageUrl}",
                LinkUrl = encodedPageUrl
            };
        }
    }

}
