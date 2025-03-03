using System;
using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.MemberSearchMyDirectory
{
    public class MemberSearchMyDirectoryViewModel
    {
        public List<SavedItemViewModel> Persons { get; set; }
        public List<SavedItemViewModel> Companies { get; set; }
        public List<RecentViewModel> RecentViews { get; set; }
    }
    public class SavedItemViewModel
    {
        public Guid ItemId { get; set; }
        public string SavedItemDisplayName { get; set; }
        public string SavedItemDisplayDescription { get; set; }
        public string SavedType { get; set; } // "Person" or "Company"
    }
    public class RecentViewModel
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public string Type { get; set; } // "Person" or "Company"
        public Guid ItemGUID { get; set; }
        public int Ordinal { get; set; }
    }

}
