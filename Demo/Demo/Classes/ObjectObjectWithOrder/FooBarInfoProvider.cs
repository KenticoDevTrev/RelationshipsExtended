using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.DataEngine;
using CMS.Helpers;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="FooBarInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IFooBarInfoProvider))]
    public partial class FooBarInfoProvider : AbstractInfoProvider<FooBarInfo, FooBarInfoProvider>, IFooBarInfoProvider
    {
        /// <summary>
        /// Gets an instance of the <see cref="FooBarInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <param name="barId">ObjectType.test_bar ID.</param>
        /// <returns>Returns an instance of <see cref="FooBarInfo"/> corresponding to given identifiers or null.</returns>
        public virtual FooBarInfo Get(int fooId, int barId)
        {
            return GetObjectQuery().TopN(1)
                .WhereEquals("FooBarFooID", fooId)
                .WhereEquals("FooBarBarID", barId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Asynchronously gets an instance of the <see cref="FooBarInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <param name="barId">ObjectType.test_bar ID.</param>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>Returns a task returning either an instance of <see cref="FooBarInfo"/> corresponding to given identifiers or null.</returns>
        public async virtual Task<FooBarInfo> GetAsync(int fooId, int barId, CancellationToken? cancellationToken = null)
        {
            var query = await GetObjectQuery().TopN(1)
                .WhereEquals("FooBarFooID", fooId)
                .WhereEquals("FooBarBarID", barId)
                .GetEnumerableTypedResultAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return query.FirstOrDefault();
        }


        /// <summary>
        /// Deletes <see cref="FooBarInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.test_foo ID.</param>
        /// <param name="barId">ObjectType.test_bar ID.</param>
        public virtual void Remove(int fooId, int barId)
        {
            var infoObj = Get(fooId, barId);
            if (infoObj != null)
            {
                Delete(infoObj);
            }
        }

        protected override void SetInfo(FooBarInfo info)
        {
            // Customization 1 - On Insert or update, check and set the Order
            if (ValidationHelper.GetInteger(info.GetValue(nameof(info.FooBarOrder)), -1) <= 0)
            {
                info.FooBarOrder = Get().WhereEquals(nameof(info.FooBarFooID), info.FooBarFooID).Count + 1;
            }
            base.SetInfo(info);
        }

        protected override void DeleteInfo(FooBarInfo info)
        {
            base.DeleteInfo(info);

            // Customization 2, on deletion re-order
            // Initialize Order, the info should still exist in memory and only needed the Generalized portion
            info.Generalized.InitObjectsOrder(null);
        }
    }
}