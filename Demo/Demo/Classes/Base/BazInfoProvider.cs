using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="BazInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IBazInfoProvider))]
    public partial class BazInfoProvider : AbstractInfoProvider<BazInfo, BazInfoProvider>, IBazInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BazInfoProvider"/> class.
        /// </summary>
        public BazInfoProvider()
            : base(BazInfo.TYPEINFO)
        {
        }
    }
}