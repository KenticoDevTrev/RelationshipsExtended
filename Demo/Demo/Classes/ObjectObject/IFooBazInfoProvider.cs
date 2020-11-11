using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="FooBazInfo"/> management.
    /// </summary>
    public partial interface IFooBazInfoProvider : IInfoProvider<FooBazInfo>
    {
        /// <summary>
        /// Gets an instance of the <see cref="FooBazInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        /// <returns>Returns an instance of <see cref="FooBazInfo"/> corresponding to given identifiers or null.</returns>
        FooBazInfo Get(int fooId, int bazId);


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="FooBazInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="FooBazInfo"/> corresponding to given identifiers or null.</returns>
        Task<FooBazInfo> GetAsync(int fooId, int bazId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Deletes <see cref="FooBazInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        void Remove(int fooId, int bazId);


        /// <summary>
        /// Creates <see cref="FooBazInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        void Add(int fooId, int bazId);
    }
}