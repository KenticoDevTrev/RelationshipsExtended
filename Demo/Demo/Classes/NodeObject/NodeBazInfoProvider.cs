using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="NodeBazInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(INodeBazInfoProvider))]
    public partial class NodeBazInfoProvider : AbstractInfoProvider<NodeBazInfo, NodeBazInfoProvider>, INodeBazInfoProvider
    {
        /// <summary>
        /// Gets an instance of the <see cref="NodeBazInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        /// <returns>Returns an instance of <see cref="NodeBazInfo"/> corresponding to given identifiers or null.</returns>
        public virtual NodeBazInfo Get(int nodeId, int bazId)
        {
            return GetObjectQuery().TopN(1)
                .WhereEquals("NodeBazNodeID", nodeId)
                .WhereEquals("NodeBazBazID", bazId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="NodeBazInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="NodeBazInfo"/> corresponding to given identifiers or null.</returns>
        public async virtual Task<NodeBazInfo> GetAsync(int nodeId, int bazId, CancellationToken? cancellationToken = null)
        {
            var query = await GetObjectQuery().TopN(1)
                .WhereEquals("NodeBazNodeID", nodeId)
                .WhereEquals("NodeBazBazID", bazId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Deletes <see cref="NodeBazInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        public virtual void Remove(int nodeId, int bazId)
        {
            var infoObj = Get(nodeId, bazId);
            if (infoObj != null)
            {
                Delete(infoObj);
            }
        }


        /// <summary>
        /// Creates <see cref="NodeBazInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        public virtual void Add(int nodeId, int bazId)
        {
            // Create new binding
            var infoObj = new NodeBazInfo();
            infoObj.NodeBazNodeID = nodeId;
            infoObj.NodeBazBazID = bazId;

            // Save to the database
            Set(infoObj);
        }
    }
}