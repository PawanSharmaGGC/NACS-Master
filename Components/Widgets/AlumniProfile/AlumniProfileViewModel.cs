using System;

namespace Convenience.org.Components.Widgets.AlumniProfile
{
    public class AlumniProfileViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string AccountName { get; set; }
        public string Address { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool IsSubscribedToNewsletter { get; set; }
        public bool IsListedInDirectory { get; set; }
    }
}
