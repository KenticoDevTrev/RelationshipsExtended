using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="NodeRegionInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(INodeRegionInfoProvider))]
    public partial class NodeRegionInfoProvider : AbstractInfoProvider<NodeRegionInfo, NodeRegionInfoProvider>, INodeRegionInfoProvider
    {
        /// <summary>
        /// Gets an instance of the <see cref="NodeRegionInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <returns>Returns an instance of <see cref="NodeRegionInfo"/> corresponding to given identifiers or null.</returns>
        public virtual NodeRegionInfo Get(int nodeId, int categoryId)
        {
            return GetObjectQuery().TopN(1)
                .WhereEquals("NodeRegionNodeID", nodeId)
                .WhereEquals("NodeRegionCategoryID", categoryId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="NodeRegionInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="NodeRegionInfo"/> corresponding to given identifiers or null.</returns>
        public async virtual Task<NodeRegionInfo> GetAsync(int nodeId, int categoryId, CancellationToken? cancellationToken = null)
        {
            var query = await GetObjectQuery().TopN(1)
                .WhereEquals("NodeRegionNodeID", nodeId)
                .WhereEquals("NodeRegionCategoryID", categoryId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Deletes <see cref="NodeRegionInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        public virtual void Remove(int nodeId, int categoryId)
        {
            var infoObj = Get(nodeId, categoryId);
            if (infoObj != null)
            {
                Delete(infoObj);
            }
        }


        /// <summary>
        /// Creates <see cref="NodeRegionInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        public virtual void Add(int nodeId, int categoryId)
        {
            // Create new binding
            var infoObj = new NodeRegionInfo();
            infoObj.NodeRegionNodeID = nodeId;
            infoObj.NodeRegionCategoryID = categoryId;

            // Save to the database
            Set(infoObj);
        }
    }
}