using System.Linq;
using CMS.DataEngine;

namespace CMS
{
    /// <summary>
    /// Class providing <see cref="TreeCategoryInfo"/> management.
    /// </summary>
    public partial class TreeCategoryInfoProvider : AbstractInfoProvider<TreeCategoryInfo, TreeCategoryInfoProvider>
    {
        /// <summary>
        /// Returns all <see cref="TreeCategoryInfo"/> bindings.
        /// </summary>
        public static ObjectQuery<TreeCategoryInfo> GetTreeCategories()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="TreeCategoryInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>  
        public static TreeCategoryInfo GetTreeCategoryInfo(int nodeId, int categoryId)
        {
            return ProviderObject.GetObjectQuery().TopN(1)
                .WhereEquals("NodeID", nodeId)
                .WhereEquals("CategoryID", categoryId)
                .FirstOrDefault();
        }


        /// <summary>
        /// Sets specified <see cref="TreeCategoryInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="TreeCategoryInfo"/> to set.</param>
        public static void SetTreeCategoryInfo(TreeCategoryInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="TreeCategoryInfo"/> binding.
        /// </summary>
        /// <param name="infoObj"><see cref="TreeCategoryInfo"/> object.</param>
        public static void DeleteTreeCategoryInfo(TreeCategoryInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="TreeCategoryInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>  
        public static void RemoveTreeFromCategory(int nodeId, int categoryId)
        {
            var infoObj = GetTreeCategoryInfo(nodeId, categoryId);
            if (infoObj != null)
            {
                DeleteTreeCategoryInfo(infoObj);
            }
        }


        /// <summary>
        /// Creates <see cref="TreeCategoryInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>   
        public static void AddTreeToCategory(int nodeId, int categoryId)
        {
            // Create new binding
            var infoObj = new TreeCategoryInfo();
            infoObj.NodeID = nodeId;
            infoObj.CategoryID = categoryId;

            // Save to the database
            SetTreeCategoryInfo(infoObj);
        }
    }
}