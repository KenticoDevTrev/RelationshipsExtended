using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;
using CMS.Helpers;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="NodeFooInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(INodeFooInfoProvider))]
    public partial class NodeFooInfoProvider : AbstractInfoProvider<NodeFooInfo, NodeFooInfoProvider>, INodeFooInfoProvider
    {
        /// <summary>
        /// Gets an instance of the <see cref="NodeFooInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <returns>Returns an instance of <see cref="NodeFooInfo"/> corresponding to given identifiers or null.</returns>
        public virtual NodeFooInfo Get(int nodeId, int fooId)
        {
            return GetObjectQuery().TopN(1)
                .WhereEquals("NodeFooNodeID", nodeId)
                .WhereEquals("NodeFooFooID", fooId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="NodeFooInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="NodeFooInfo"/> corresponding to given identifiers or null.</returns>
        public async virtual Task<NodeFooInfo> GetAsync(int nodeId, int fooId, CancellationToken? cancellationToken = null)
        {
            var query = await GetObjectQuery().TopN(1)
                .WhereEquals("NodeFooNodeID", nodeId)
                .WhereEquals("NodeFooFooID", fooId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Deletes <see cref="NodeFooInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        public virtual void Remove(int nodeId, int fooId)
        {
            var infoObj = Get(nodeId, fooId);
            if (infoObj != null)
            {
                Delete(infoObj);
            }
        }


        /// <summary>
        /// Creates <see cref="NodeFooInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        public virtual void Add(int nodeId, int fooId)
        {
            // Create new binding
            var infoObj = new NodeFooInfo();
            infoObj.NodeFooNodeID = nodeId;
            infoObj.NodeFooFooID = fooId;

            // Save to the database
            Set(infoObj);
        }

        protected override void SetInfo(NodeFooInfo info)
        {
            // Customization 1 - Call overwritten Set
            if (ValidationHelper.GetInteger(info.GetValue("NodeFooOrder"), -1) <= 0)
            {
                info.NodeFooOrder = Get().WhereEquals(nameof(NodeFooInfo.NodeFooNodeID), info.NodeFooNodeID).Count + 1;
            }
            base.SetInfo(info);
        }

        protected override void DeleteInfo(NodeFooInfo info)
        {
            base.DeleteInfo(info);
            // Customization 2, on deletion re-order
            // Initialize Order, the info should still exist in memory and only needed the Generalized portion
            info.Generalized.InitObjectsOrder(null);
        }
    }
}