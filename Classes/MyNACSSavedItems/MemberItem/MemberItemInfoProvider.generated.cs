using CMS.DataEngine;

namespace MyNACSSavedItems
{
    /// <summary>
    /// Class providing <see cref="MemberItemInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IMemberItemInfoProvider))]
    public partial class MemberItemInfoProvider : AbstractInfoProvider<MemberItemInfo, MemberItemInfoProvider>, IMemberItemInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberItemInfoProvider"/> class.
        /// </summary>
        public MemberItemInfoProvider()
            : base(MemberItemInfo.TYPEINFO)
        {
        }
    }
}