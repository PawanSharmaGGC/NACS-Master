using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Customtable;

[assembly: RegisterObjectType(typeof(AlumniDirectoryUpdatesTableInfo), AlumniDirectoryUpdatesTableInfo.OBJECT_TYPE)]

namespace Customtable
{
    /// <summary>
    /// Data container class for <see cref="AlumniDirectoryUpdatesTableInfo"/>.
    /// </summary>
    [Serializable]
    public partial class AlumniDirectoryUpdatesTableInfo : AbstractInfo<AlumniDirectoryUpdatesTableInfo, IAlumniDirectoryUpdatesTableInfoProvider>, IInfoWithId, IInfoWithGuid
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "customtable.alumnidirectoryupdatestable";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(AlumniDirectoryUpdatesTableInfoProvider), OBJECT_TYPE, "customtable.AlumniDirectoryUpdatesTable", "AlumniDirectoryUpdatesTableID", null, "ItemGUID", null, null, null, null, null)
        {
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Alumni directory updates table ID.
        /// </summary>
        [DatabaseField]
        public virtual int AlumniDirectoryUpdatesTableID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(AlumniDirectoryUpdatesTableID)), 0);
            set => SetValue(nameof(AlumniDirectoryUpdatesTableID), value);
        }


        /// <summary>
        /// Type.
        /// </summary>
        [DatabaseField]
        public virtual string Type
        {
            get => ValidationHelper.GetString(GetValue(nameof(Type)), String.Empty);
            set => SetValue(nameof(Type), value);
        }


        /// <summary>
        /// Location.
        /// </summary>
        [DatabaseField]
        public virtual string Location
        {
            get => ValidationHelper.GetString(GetValue(nameof(Location)), String.Empty);
            set => SetValue(nameof(Location), value, String.Empty);
        }


        /// <summary>
        /// Topic.
        /// </summary>
        [DatabaseField]
        public virtual string Topic
        {
            get => ValidationHelper.GetString(GetValue(nameof(Topic)), String.Empty);
            set => SetValue(nameof(Topic), value, String.Empty);
        }


        /// <summary>
        /// Start date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime StartDate
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(StartDate)), DateTimeHelper.ZERO_TIME);
            set => SetValue(nameof(StartDate), value, DateTimeHelper.ZERO_TIME);
        }


        /// <summary>
        /// Event start time.
        /// </summary>
        [DatabaseField]
        public virtual DateTime EventStartTime
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(EventStartTime)), DateTimeHelper.ZERO_TIME);
            set => SetValue(nameof(EventStartTime), value, DateTimeHelper.ZERO_TIME);
        }


        /// <summary>
        /// Event end date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime EventEndDate
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(EventEndDate)), DateTimeHelper.ZERO_TIME);
            set => SetValue(nameof(EventEndDate), value, DateTimeHelper.ZERO_TIME);
        }


        /// <summary>
        /// Event time zone.
        /// </summary>
        [DatabaseField]
        public virtual DateTime EventTimeZone
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(EventTimeZone)), DateTimeHelper.ZERO_TIME);
            set => SetValue(nameof(EventTimeZone), value, DateTimeHelper.ZERO_TIME);
        }


        /// <summary>
        /// End date.
        /// </summary>
        [DatabaseField]
        public virtual DateTime EndDate
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(EndDate)), DateTimeHelper.ZERO_TIME);
            set => SetValue(nameof(EndDate), value, DateTimeHelper.ZERO_TIME);
        }


        /// <summary>
        /// Title.
        /// </summary>
        [DatabaseField]
        public virtual string Title
        {
            get => ValidationHelper.GetString(GetValue(nameof(Title)), String.Empty);
            set => SetValue(nameof(Title), value);
        }


        /// <summary>
        /// Description.
        /// </summary>
        [DatabaseField]
        public virtual string Description
        {
            get => ValidationHelper.GetString(GetValue(nameof(Description)), String.Empty);
            set => SetValue(nameof(Description), value);
        }


        /// <summary>
        /// URL.
        /// </summary>
        [DatabaseField]
        public virtual string URL
        {
            get => ValidationHelper.GetString(GetValue(nameof(URL)), String.Empty);
            set => SetValue(nameof(URL), value, String.Empty);
        }


        /// <summary>
        /// URL button text.
        /// </summary>
        [DatabaseField]
        public virtual string URLButtonText
        {
            get => ValidationHelper.GetString(GetValue(nameof(URLButtonText)), "Register");
            set => SetValue(nameof(URLButtonText), value, String.Empty);
        }


        /// <summary>
        /// Item created by.
        /// </summary>
        [DatabaseField]
        public virtual int ItemCreatedBy
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(ItemCreatedBy)), 0);
            set => SetValue(nameof(ItemCreatedBy), value, 0);
        }


        /// <summary>
        /// Item created when.
        /// </summary>
        [DatabaseField]
        public virtual DateTime ItemCreatedWhen
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(ItemCreatedWhen)), DateTimeHelper.ZERO_TIME);
            set => SetValue(nameof(ItemCreatedWhen), value, DateTimeHelper.ZERO_TIME);
        }


        /// <summary>
        /// Item modified by.
        /// </summary>
        [DatabaseField]
        public virtual int ItemModifiedBy
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(ItemModifiedBy)), 0);
            set => SetValue(nameof(ItemModifiedBy), value, 0);
        }


        /// <summary>
        /// Item modified when.
        /// </summary>
        [DatabaseField]
        public virtual DateTime ItemModifiedWhen
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(ItemModifiedWhen)), DateTimeHelper.ZERO_TIME);
            set => SetValue(nameof(ItemModifiedWhen), value, DateTimeHelper.ZERO_TIME);
        }


        /// <summary>
        /// Item order.
        /// </summary>
        [DatabaseField]
        public virtual int ItemOrder
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(ItemOrder)), 0);
            set => SetValue(nameof(ItemOrder), value, 0);
        }


        /// <summary>
        /// Item GUID.
        /// </summary>
        [DatabaseField]
        public virtual Guid ItemGUID
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(ItemGUID)), Guid.Empty);
            set => SetValue(nameof(ItemGUID), value, Guid.Empty);
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
        protected AlumniDirectoryUpdatesTableInfo(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="AlumniDirectoryUpdatesTableInfo"/> class.
        /// </summary>
        public AlumniDirectoryUpdatesTableInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="AlumniDirectoryUpdatesTableInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public AlumniDirectoryUpdatesTableInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}