using CMS.DataEngine;

namespace MyNACSSavedItems
{
    /// <summary>
    /// Declares members for <see cref="MemberItemInfo"/> management.
    /// </summary>
    public partial interface IMemberItemInfoProvider : IInfoProvider<MemberItemInfo>, IInfoByIdProvider<MemberItemInfo>
    {
    }
}