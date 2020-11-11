using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="NodeBazInfo"/> management.
    /// </summary>
    public partial interface INodeBazInfoProvider : IInfoProvider<NodeBazInfo>
    {
        /// <summary>
        /// Gets an instance of the <see cref="NodeBazInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        /// <returns>Returns an instance of <see cref="NodeBazInfo"/> corresponding to given identifiers or null.</returns>
        NodeBazInfo Get(int nodeId, int bazId);


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="NodeBazInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="NodeBazInfo"/> corresponding to given identifiers or null.</returns>
        Task<NodeBazInfo> GetAsync(int nodeId, int bazId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Deletes <see cref="NodeBazInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        void Remove(int nodeId, int bazId);


        /// <summary>
        /// Creates <see cref="NodeBazInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        void Add(int nodeId, int bazId);
    }
}