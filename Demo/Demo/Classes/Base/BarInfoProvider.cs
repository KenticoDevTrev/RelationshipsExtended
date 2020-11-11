using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="BarInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IBarInfoProvider))]
    public partial class BarInfoProvider : AbstractInfoProvider<BarInfo, BarInfoProvider>, IBarInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarInfoProvider"/> class.
        /// </summary>
        public BarInfoProvider()
            : base(BarInfo.TYPEINFO)
        {
        }
    }
}