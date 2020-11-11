using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="FooInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IFooInfoProvider))]
    public partial class FooInfoProvider : AbstractInfoProvider<FooInfo, FooInfoProvider>, IFooInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FooInfoProvider"/> class.
        /// </summary>
        public FooInfoProvider()
            : base(FooInfo.TYPEINFO)
        {
        }
    }
}