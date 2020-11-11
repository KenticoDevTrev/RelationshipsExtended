using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="BarNodeInfo"/> management.
    /// </summary>
    public partial interface IBarNodeInfoProvider : IInfoProvider<BarNodeInfo>
    {
        /// <summary>
        /// Gets an instance of the <see cref="BarNodeInfo"/> binding structure.
        /// </summary>
        /// <param name="barId">Bar ID.</param>
        /// <param name="nodeId">Node ID.</param>
        /// <returns>Returns an instance of <see cref="BarNodeInfo"/> corresponding to given identifiers or null.</returns>
        BarNodeInfo Get(int barId, int nodeId);


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="BarNodeInfo"/> binding structure.
        /// </summary>
        /// <param name="barId">Bar ID.</param>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="BarNodeInfo"/> corresponding to given identifiers or null.</returns>
        Task<BarNodeInfo> GetAsync(int barId, int nodeId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Deletes <see cref="BarNodeInfo"/> binding.
        /// </summary>
        /// <param name="barId">Bar ID.</param>
        /// <param name="nodeId">Node ID.</param>
        void Remove(int barId, int nodeId);


        /// <summary>
        /// Creates <see cref="BarNodeInfo"/> binding.
        /// </summary>
        /// <param name="barId">Bar ID.</param>
        /// <param name="nodeId">Node ID.</param>
        void Add(int barId, int nodeId);
    }
}