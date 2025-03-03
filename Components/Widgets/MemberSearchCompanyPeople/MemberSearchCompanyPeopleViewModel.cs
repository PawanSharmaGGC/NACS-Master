using System;

namespace Convenience.org.Components.Widgets.MemberSearchCompanyPeople
{
    public class MemberSearchCompanyPeopleViewModel
    {
        public Guid ContactId {get; set;}
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
    }
}
