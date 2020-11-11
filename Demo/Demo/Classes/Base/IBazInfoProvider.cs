using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="BazInfo"/> management.
    /// </summary>
    public partial interface IBazInfoProvider : IInfoProvider<BazInfo>, IInfoByIdProvider<BazInfo>, IInfoByNameProvider<BazInfo>
    {
    }
}