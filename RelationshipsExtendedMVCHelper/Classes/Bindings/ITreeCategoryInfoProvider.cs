using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace CMS
{
    /// <summary>
    /// Declares members for <see cref="TreeCategoryInfo"/> management.
    /// </summary>
    public partial interface ITreeCategoryInfoProvider : IInfoProvider<TreeCategoryInfo>
    {
        /// <summary>
        /// Gets an instance of the <see cref="TreeCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <returns>Returns an instance of <see cref="TreeCategoryInfo"/> corresponding to given identifiers or null.</returns>
        TreeCategoryInfo Get(int nodeId, int categoryId);


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="TreeCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="TreeCategoryInfo"/> corresponding to given identifiers or null.</returns>
        Task<TreeCategoryInfo> GetAsync(int nodeId, int categoryId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Deletes <see cref="TreeCategoryInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        void Remove(int nodeId, int categoryId);


        /// <summary>
        /// Creates <see cref="TreeCategoryInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        void Add(int nodeId, int categoryId);
    }
}