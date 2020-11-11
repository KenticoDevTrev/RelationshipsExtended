using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="FooBazInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IFooBazInfoProvider))]
    public partial class FooBazInfoProvider : AbstractInfoProvider<FooBazInfo, FooBazInfoProvider>, IFooBazInfoProvider
    {
        /// <summary>
        /// Gets an instance of the <see cref="FooBazInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        /// <returns>Returns an instance of <see cref="FooBazInfo"/> corresponding to given identifiers or null.</returns>
        public virtual FooBazInfo Get(int fooId, int bazId)
        {
            return GetObjectQuery().TopN(1)
                .WhereEquals("FooBazFooID", fooId)
                .WhereEquals("FooBazBazID", bazId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="FooBazInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="FooBazInfo"/> corresponding to given identifiers or null.</returns>
        public async virtual Task<FooBazInfo> GetAsync(int fooId, int bazId, CancellationToken? cancellationToken = null)
        {
            var query = await GetObjectQuery().TopN(1)
                .WhereEquals("FooBazFooID", fooId)
                .WhereEquals("FooBazBazID", bazId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Deletes <see cref="FooBazInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        public virtual void Remove(int fooId, int bazId)
        {
            var infoObj = Get(fooId, bazId);
            if (infoObj != null)
            {
                Delete(infoObj);
            }
        }


        /// <summary>
        /// Creates <see cref="FooBazInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="bazId">ObjectType.demo_baz ID.</param>
        public virtual void Add(int fooId, int bazId)
        {
            // Create new binding
            var infoObj = new FooBazInfo();
            infoObj.FooBazFooID = fooId;
            infoObj.FooBazBazID = bazId;

            // Save to the database
            Set(infoObj);
        }
    }
}