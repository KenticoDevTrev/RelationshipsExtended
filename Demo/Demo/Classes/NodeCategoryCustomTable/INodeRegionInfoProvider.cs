using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="NodeRegionInfo"/> management.
    /// </summary>
    public partial interface INodeRegionInfoProvider : IInfoProvider<NodeRegionInfo>
    {
        /// <summary>
        /// Gets an instance of the <see cref="NodeRegionInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <returns>Returns an instance of <see cref="NodeRegionInfo"/> corresponding to given identifiers or null.</returns>
        NodeRegionInfo Get(int nodeId, int categoryId);


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="NodeRegionInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="NodeRegionInfo"/> corresponding to given identifiers or null.</returns>
        Task<NodeRegionInfo> GetAsync(int nodeId, int categoryId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Deletes <see cref="NodeRegionInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        void Remove(int nodeId, int categoryId);


        /// <summary>
        /// Creates <see cref="NodeRegionInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        void Add(int nodeId, int categoryId);
    }
}