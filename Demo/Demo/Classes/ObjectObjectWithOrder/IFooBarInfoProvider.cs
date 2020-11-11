using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Declares members for <see cref="FooBarInfo"/> management.
    /// </summary>
    public partial interface IFooBarInfoProvider : IInfoProvider<FooBarInfo>
    {
        /// <summary>
        /// Gets an instance of the <see cref="FooBarInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <param name="barId">ObjectType.test_bar ID.</param>
        /// <returns>Returns an instance of <see cref="FooBarInfo"/> corresponding to given identifiers or null.</returns>
        FooBarInfo Get(int fooId, int barId);


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="FooBarInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <param name="barId">ObjectType.test_bar ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="FooBarInfo"/> corresponding to given identifiers or null.</returns>
        Task<FooBarInfo> GetAsync(int fooId, int barId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Deletes <see cref="FooBarInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <param name="barId">ObjectType.test_bar ID.</param>
        void Remove(int fooId, int barId);
    }
}