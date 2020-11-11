using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace CMS
{
    /// <summary>
    /// Class providing <see cref="TreeCategoryInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(ITreeCategoryInfoProvider))]
    public partial class TreeCategoryInfoProvider : AbstractInfoProvider<TreeCategoryInfo, TreeCategoryInfoProvider>, ITreeCategoryInfoProvider
    {
        /// <summary>
        /// Gets an instance of the <see cref="TreeCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <returns>Returns an instance of <see cref="TreeCategoryInfo"/> corresponding to given identifiers or null.</returns>
        public virtual TreeCategoryInfo Get(int nodeId, int categoryId)
        {
            return GetObjectQuery().TopN(1)
                .WhereEquals("NodeID", nodeId)
                .WhereEquals("CategoryID", categoryId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="TreeCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="TreeCategoryInfo"/> corresponding to given identifiers or null.</returns>
        public async virtual Task<TreeCategoryInfo> GetAsync(int nodeId, int categoryId, CancellationToken? cancellationToken = null)
        {
            var query = await GetObjectQuery().TopN(1)
                .WhereEquals("NodeID", nodeId)
                .WhereEquals("CategoryID", categoryId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Deletes <see cref="TreeCategoryInfo"/> binding.
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
        /// Creates <see cref="TreeCategoryInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        public virtual void Add(int nodeId, int categoryId)
        {
            // Create new binding
            var infoObj = new TreeCategoryInfo();
            infoObj.NodeID = nodeId;
            infoObj.CategoryID = categoryId;

            // Save to the database
            Set(infoObj);
        }
    }
}