using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.CommitteeMemberInfo
{
    public class CommitteeMemberInfoModel
    {
        public List<string> Committees { get; set; }
        public string NACSID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Stores { get; set; }
        public string Website { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityStateZip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}
