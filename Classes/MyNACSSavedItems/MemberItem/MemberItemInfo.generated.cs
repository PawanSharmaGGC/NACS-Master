using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using MyNACSSavedItems;

[assembly: RegisterObjectType(typeof(MemberItemInfo), MemberItemInfo.OBJECT_TYPE)]

namespace MyNACSSavedItems
{
    /// <summary>
    /// Data container class for <see cref="MemberItemInfo"/>.
    /// </summary>
    [Serializable]
    public partial class MemberItemInfo : AbstractInfo<MemberItemInfo, IMemberItemInfoProvider>, IInfoWithId
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "mynacssaveditems.memberitem";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(MemberItemInfoProvider), OBJECT_TYPE, "MyNACSSavedItems.MemberItem", "MemberItemID", null, null, null, "SavedItemDisplayName", null, null, null)
        {
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Member item ID.
        /// </summary>
        [DatabaseField]
        public virtual int MemberItemID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(MemberItemID)), 0);
            set => SetValue(nameof(MemberItemID), value);
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
            get => ValidationHelper.GetDateTime(GetValue(nameof(SavedDate)), DateTimeHelper.ZERO_TIME);
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
        /// NACS organization key.
        /// </summary>
        [DatabaseField]
        public virtual string NACSOrganizationKey
        {
            get => ValidationHelper.GetString(GetValue(nameof(NACSOrganizationKey)), String.Empty);
            set => SetValue(nameof(NACSOrganizationKey), value, String.Empty);
        }


        /// <summary>
        /// NACS individual key.
        /// </summary>
        [DatabaseField]
        public virtual string NACSIndividualKey
        {
            get => ValidationHelper.GetString(GetValue(nameof(NACSIndividualKey)), String.Empty);
            set => SetValue(nameof(NACSIndividualKey), value, String.Empty);
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
        protected MemberItemInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="MemberItemInfo"/> class.
        /// </summary>
        public MemberItemInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="MemberItemInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public MemberItemInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}