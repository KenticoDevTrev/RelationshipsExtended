using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="BarInfo"/> management.
    /// </summary>
    public partial interface IBarInfoProvider : IInfoProvider<BarInfo>, IInfoByIdProvider<BarInfo>, IInfoByNameProvider<BarInfo>
    {
    }
}