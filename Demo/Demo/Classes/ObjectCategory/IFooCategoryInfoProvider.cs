using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="FooCategoryInfo"/> management.
    /// </summary>
    public partial interface IFooCategoryInfoProvider : IInfoProvider<FooCategoryInfo>
    {
        /// <summary>
        /// Gets an instance of the <see cref="FooCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <returns>Returns an instance of <see cref="FooCategoryInfo"/> corresponding to given identifiers or null.</returns>
        FooCategoryInfo Get(int fooId, int categoryId);


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="FooCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="FooCategoryInfo"/> corresponding to given identifiers or null.</returns>
        Task<FooCategoryInfo> GetAsync(int fooId, int categoryId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Deletes <see cref="FooCategoryInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        void Remove(int fooId, int categoryId);


        /// <summary>
        /// Creates <see cref="FooCategoryInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        void Add(int fooId, int categoryId);
    }
}