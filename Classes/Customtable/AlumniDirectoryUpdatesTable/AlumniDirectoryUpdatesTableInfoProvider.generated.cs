using CMS.DataEngine;

namespace Customtable
{
    /// <summary>
    /// Class providing <see cref="AlumniDirectoryUpdatesTableInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IAlumniDirectoryUpdatesTableInfoProvider))]
    public partial class AlumniDirectoryUpdatesTableInfoProvider : AbstractInfoProvider<AlumniDirectoryUpdatesTableInfo, AlumniDirectoryUpdatesTableInfoProvider>, IAlumniDirectoryUpdatesTableInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlumniDirectoryUpdatesTableInfoProvider"/> class.
        /// </summary>
        public AlumniDirectoryUpdatesTableInfoProvider()
            : base(AlumniDirectoryUpdatesTableInfo.TYPEINFO)
        {
        }
    }
}