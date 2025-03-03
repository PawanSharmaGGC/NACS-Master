//using NACS.Helper.CustomerService;

//namespace Convenience.org.Components.Widgets.CommitteeListing
//{
//    public class CommitteeListingViewModel
//    {
//        public NACSCommitteeMember[] CMTEMembers { get; set; }
//    }
//}

using System.Collections.Generic;

namespace Convenience.org.Components.Widgets.CommitteeListing
{
    public class CommitteeMemberViewModel
    {
        public string Position { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string CommitteeName { get; set; }
    }

    public class CommitteeListingViewModel
    {
        public List<CommitteeMemberViewModel> Members { get; set; } = new List<CommitteeMemberViewModel>();
    }
}
