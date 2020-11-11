using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="BarNodeInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IBarNodeInfoProvider))]
    public partial class BarNodeInfoProvider : AbstractInfoProvider<BarNodeInfo, BarNodeInfoProvider>, IBarNodeInfoProvider
    {
        /// <summary>
        /// Gets an instance of the <see cref="BarNodeInfo"/> binding structure.
        /// </summary>
        /// <param name="barId">Bar ID.</param>
        /// <param name="nodeId">Node ID.</param>
        /// <returns>Returns an instance of <see cref="BarNodeInfo"/> corresponding to given identifiers or null.</returns>
        public virtual BarNodeInfo Get(int barId, int nodeId)
        {
            return GetObjectQuery().TopN(1)
                .WhereEquals("BarNodeBarID", barId)
                .WhereEquals("BarNodeNodeID", nodeId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="BarNodeInfo"/> binding structure.
        /// </summary>
        /// <param name="barId">Bar ID.</param>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="BarNodeInfo"/> corresponding to given identifiers or null.</returns>
        public async virtual Task<BarNodeInfo> GetAsync(int barId, int nodeId, CancellationToken? cancellationToken = null)
        {
            var query = await GetObjectQuery().TopN(1)
                .WhereEquals("BarNodeBarID", barId)
                .WhereEquals("BarNodeNodeID", nodeId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Deletes <see cref="BarNodeInfo"/> binding.
        /// </summary>
        /// <param name="barId">Bar ID.</param>
        /// <param name="nodeId">Node ID.</param>
        public virtual void Remove(int barId, int nodeId)
        {
            var infoObj = Get(barId, nodeId);
            if (infoObj != null)
            {
                Delete(infoObj);
            }
        }


        /// <summary>
        /// Creates <see cref="BarNodeInfo"/> binding.
        /// </summary>
        /// <param name="barId">Bar ID.</param>
        /// <param name="nodeId">Node ID.</param>
        public virtual void Add(int barId, int nodeId)
        {
            // Create new binding
            var infoObj = new BarNodeInfo();
            infoObj.BarNodeBarID = barId;
            infoObj.BarNodeNodeID = nodeId;

            // Save to the database
            Set(infoObj);
        }
    }
}