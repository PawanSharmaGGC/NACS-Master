using CMS.DataEngine;

namespace NACS.MyNACSSavedItems
{
    /// <summary>
    /// Declares members for <see cref="SessionItemInfo"/> management.
    /// </summary>
    public partial interface ISessionItemInfoProvider : IInfoProvider<SessionItemInfo>, IInfoByIdProvider<SessionItemInfo>, IInfoByNameProvider<SessionItemInfo>
    {
    }
}