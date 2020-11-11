using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="NodeFooInfo"/> management.
    /// </summary>
    public partial interface INodeFooInfoProvider : IInfoProvider<NodeFooInfo>
    {
        /// <summary>
        /// Gets an instance of the <see cref="NodeFooInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <returns>Returns an instance of <see cref="NodeFooInfo"/> corresponding to given identifiers or null.</returns>
        NodeFooInfo Get(int nodeId, int fooId);


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="NodeFooInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="NodeFooInfo"/> corresponding to given identifiers or null.</returns>
        Task<NodeFooInfo> GetAsync(int nodeId, int fooId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Deletes <see cref="NodeFooInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        void Remove(int nodeId, int fooId);


        /// <summary>
        /// Creates <see cref="NodeFooInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        void Add(int nodeId, int fooId);
    }
}