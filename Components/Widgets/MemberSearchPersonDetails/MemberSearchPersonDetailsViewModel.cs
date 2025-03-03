using System;

namespace Convenience.org.Components.Widgets.MemberSearchPersonDetails
{
    public class MemberSearchPersonDetailsViewModel
    {
        public Guid ContactId { get; set; }
        public Guid? AccountId { get; set; }
        public string PaLabelName { get; set; }
        public string JobTitle { get; set; }
        public string AccountName { get; set; }
        public string AddressComposite { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool IsSaved { get; set; }
        public string Command { get; set; }
    }
}
