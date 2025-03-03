using CMS.DataEngine;

namespace MyNACSSavedItems
{
    /// <summary>
    /// Class providing <see cref="SessionItemInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(ISessionItemInfoProvider))]
    public partial class SessionItemInfoProvider : AbstractInfoProvider<SessionItemInfo, SessionItemInfoProvider>, ISessionItemInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionItemInfoProvider"/> class.
        /// </summary>
        public SessionItemInfoProvider()
            : base(SessionItemInfo.TYPEINFO)
        {
        }

        /// <summary>
        /// Returns a query for all the <see cref="SessionItemInfo"/> objects.
        /// </summary>
        public static ObjectQuery<SessionItemInfo> GetSessionItems()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="SessionItemInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="SessionItemInfo"/> ID.</param>
        public static SessionItemInfo GetSessionItemInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="SessionItemInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="SessionItemInfo"/> to be set.</param>
        public static void SetSessionItemInfo(SessionItemInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="SessionItemInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="SessionItemInfo"/> to be deleted.</param>
        public static void DeleteSessionItemInfo(SessionItemInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="SessionItemInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="SessionItemInfo"/> ID.</param>
        public static void DeleteSessionItemInfo(int id)
        {
            SessionItemInfo infoObj = GetSessionItemInfo(id);
            DeleteSessionItemInfo(infoObj);
        }
    }
}
