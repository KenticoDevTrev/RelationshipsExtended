using System;
using System.Linq;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing <see cref="FooCategoryInfo"/> management.
    /// </summary>
    public partial class FooCategoryInfoProvider : AbstractInfoProvider<FooCategoryInfo, FooCategoryInfoProvider>
    {
        /// <summary>
        /// Returns all <see cref="FooCategoryInfo"/> bindings.
        /// </summary>
        public static ObjectQuery<FooCategoryInfo> GetFooCategories()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="FooCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        public static FooCategoryInfo GetFooCategoryInfo(int fooId, int categoryId)
        {
            return ProviderObject.GetObjectQuery().TopN(1)
                .WhereEquals("FooID", fooId)
                .WhereEquals("CategoryID", categoryId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Sets specified <see cref="FooCategoryInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="FooCategoryInfo"/> to set.</param>
        public static void SetFooCategoryInfo(FooCategoryInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="FooCategoryInfo"/> binding.
        /// </summary>
        /// <param name="infoObj"><see cref="FooCategoryInfo"/> object.</param>
        public static void DeleteFooCategoryInfo(FooCategoryInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="FooCategoryInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        public static void RemoveFooFromCategory(int fooId, int categoryId)
        {
            var infoObj = GetFooCategoryInfo(fooId, categoryId);
            if (infoObj != null)
            {
                DeleteFooCategoryInfo(infoObj);
            }
        }


        /// <summary>
        /// Creates <see cref="FooCategoryInfo"/> binding.
        /// </summary>
        /// <param name="fooId">ObjectType.demo_foo ID.</param>
        /// <param name="categoryId">Content category ID.</param>
        public static void AddFooToCategory(int fooId, int categoryId)
        {
            // Create new binding
            var infoObj = new FooCategoryInfo();
            infoObj.FooID = fooId;
            infoObj.CategoryID = categoryId;

            // Save to the database
            SetFooCategoryInfo(infoObj);
        }
    }
}