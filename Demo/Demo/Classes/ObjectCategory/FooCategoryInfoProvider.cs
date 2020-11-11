using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="FooCategoryInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IFooCategoryInfoProvider))]
    public partial class FooCategoryInfoProvider : AbstractInfoProvider<FooCategoryInfo, FooCategoryInfoProvider>, IFooCategoryInfoProvider
    {
        /// <summary>
        /// Gets an instance of the <see cref="FooCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <returns>Returns an instance of <see cref="FooCategoryInfo"/> corresponding to given identifiers or null.</returns>
        public virtual FooCategoryInfo Get(int fooId, int categoryId)
        {
            return GetObjectQuery().TopN(1)
                .WhereEquals("FooCategoryFooID", fooId)
                .WhereEquals("FooCategoryCategoryID", categoryId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="FooCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="FooCategoryInfo"/> corresponding to given identifiers or null.</returns>
        public async virtual Task<FooCategoryInfo> GetAsync(int fooId, int categoryId, CancellationToken? cancellationToken = null)
        {
            var query = await GetObjectQuery().TopN(1)
                .WhereEquals("FooCategoryFooID", fooId)
                .WhereEquals("FooCategoryCategoryID", categoryId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Deletes <see cref="FooCategoryInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        public virtual void Remove(int fooId, int categoryId)
        {
            var infoObj = Get(fooId, categoryId);
            if (infoObj != null)
            {
                Delete(infoObj);
            }
        }


        /// <summary>
        /// Creates <see cref="FooCategoryInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        public virtual void Add(int fooId, int categoryId)
        {
            // Create new binding
            var infoObj = new FooCategoryInfo();
            infoObj.FooCategoryFooID = fooId;
            infoObj.FooCategoryCategoryID = categoryId;

            // Save to the database
            Set(infoObj);
        }
    }
}