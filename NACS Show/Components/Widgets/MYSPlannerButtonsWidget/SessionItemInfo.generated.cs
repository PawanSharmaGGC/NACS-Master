using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using MyNACSSavedItems;

[assembly: RegisterObjectType(typeof(SessionItemInfo), SessionItemInfo.OBJECT_TYPE)]

namespace MyNACSSavedItems
{
    /// <summary>
    /// Data container class for <see cref="SessionItemInfo"/>.
    /// </summary>
    [Serializable]
    public partial class SessionItemInfo : AbstractInfo<SessionItemInfo, ISessionItemInfoProvider>, IInfoWithId, IInfoWithName
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "mynacssaveditems.sessionitem";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(SessionItemInfoProvider), OBJECT_TYPE, "MyNACSSavedItems.SessionItem", "SessionItemID", null, null, "KenticoClassName", "SavedItemDisplayName", null, null, null)
        {
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Session item ID.
        /// </summary>
        [DatabaseField]
        public virtual int SessionItemID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(SessionItemID)), 0);
            set => SetValue(nameof(SessionItemID), value);
        }


        /// <summary>
        /// Saved item display name.
        /// </summary>
        [DatabaseField]
        public virtual string SavedItemDisplayName
        {
            get => ValidationHelper.GetString(GetValue(nameof(SavedItemDisplayName)), String.Empty);
            set => SetValue(nameof(SavedItemDisplayName), value, String.Empty);
        }


        /// <summary>
        /// Saved item display description.
        /// </summary>
        [DatabaseField]
        public virtual string SavedItemDisplayDescription
        {
            get => ValidationHelper.GetString(GetValue(nameof(SavedItemDisplayDescription)), String.Empty);
            set => SetValue(nameof(SavedItemDisplayDescription), value, String.Empty);
        }


        /// <summary>
        /// Saved date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime SavedDate
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(SavedDate)), DateTime.Parse(DateTime.Now.ToString()));
            set => SetValue(nameof(SavedDate), value, DateTimeHelper.ZERO_TIME);
        }


        /// <summary>
        /// Saved type.
        /// </summary>
        [DatabaseField]
        public virtual string SavedType
        {
            get => ValidationHelper.GetString(GetValue(nameof(SavedType)), String.Empty);
            set => SetValue(nameof(SavedType), value, String.Empty);
        }


        /// <summary>
        /// Kentico user ID.
        /// </summary>
        [DatabaseField]
        public virtual int KenticoUserID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(KenticoUserID)), 0);
            set => SetValue(nameof(KenticoUserID), value, 0);
        }


        /// <summary>
        /// Kentico contact ID.
        /// </summary>
        [DatabaseField]
        public virtual int KenticoContactID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(KenticoContactID)), 0);
            set => SetValue(nameof(KenticoContactID), value, 0);
        }


        /// <summary>
        /// Kentico class name.
        /// </summary>
        [DatabaseField]
        public virtual string KenticoClassName
        {
            get => ValidationHelper.GetString(GetValue(nameof(KenticoClassName)), String.Empty);
            set => SetValue(nameof(KenticoClassName), value, String.Empty);
        }


        /// <summary>
        /// Kentico node GUID.
        /// </summary>
        [DatabaseField]
        public virtual string KenticoNodeGUID
        {
            get => ValidationHelper.GetString(GetValue(nameof(KenticoNodeGUID)), String.Empty);
            set => SetValue(nameof(KenticoNodeGUID), value, String.Empty);
        }


        /// <summary>
        /// Kentico node alias path.
        /// </summary>
        [DatabaseField]
        public virtual string KenticoNodeAliasPath
        {
            get => ValidationHelper.GetString(GetValue(nameof(KenticoNodeAliasPath)), String.Empty);
            set => SetValue(nameof(KenticoNodeAliasPath), value, String.Empty);
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            Provider.Delete(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            Provider.Set(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected SessionItemInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="SessionItemInfo"/> class.
        /// </summary>
        public SessionItemInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="SessionItemInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public SessionItemInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}