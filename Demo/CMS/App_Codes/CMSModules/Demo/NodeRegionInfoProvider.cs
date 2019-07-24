using System;
using System.Linq;

using CMS.DataEngine;

namespace Demo
{    
    /// <summary>
    /// Class providing <see cref="NodeRegionInfo"/> management.
    /// </summary>
    public partial class NodeRegionInfoProvider : AbstractInfoProvider<NodeRegionInfo, NodeRegionInfoProvider>
    {
        /// <summary>
        /// Returns all <see cref="NodeRegionInfo"/> bindings.
        /// </summary>
        public static ObjectQuery<NodeRegionInfo> GetNodeRegions()
        {
            return ProviderObject.GetObjectQuery();
        }


		/// <summary>
        /// Returns <see cref="NodeRegionInfo"/> binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>  
        public static NodeRegionInfo GetNodeRegionInfo(int nodeId, int categoryId)
        {
            return ProviderObject.GetObjectQuery().TopN(1)
                .WhereEquals("NodeID", nodeId)
                .WhereEquals("RegionCategoryID", categoryId)
				.FirstOrDefault();
        }


        /// <summary>
        /// Sets specified <see cref="NodeRegionInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="NodeRegionInfo"/> to set.</param>
        public static void SetNodeRegionInfo(NodeRegionInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="NodeRegionInfo"/> binding.
        /// </summary>
        /// <param name="infoObj"><see cref="NodeRegionInfo"/> object.</param>
        public static void DeleteNodeRegionInfo(NodeRegionInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="NodeRegionInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>  
        public static void RemoveTreeFromCategory(int nodeId, int categoryId)
        {
            var infoObj = GetNodeRegionInfo(nodeId, categoryId);
			if (infoObj != null) 
			{
				DeleteNodeRegionInfo(infoObj);
			}
        }


        /// <summary>
        /// Creates <see cref="NodeRegionInfo"/> binding.
        /// </summary>
        /// <param name="nodeId">Node ID.</param>
        /// <param name="categoryId">Content category ID.</param>   
        public static void AddTreeToCategory(int nodeId, int categoryId)
        {
            // Create new binding
            var infoObj = new NodeRegionInfo();
            infoObj.NodeID = nodeId;
			infoObj.RegionCategoryID = categoryId;

            // Save to the database
            SetNodeRegionInfo(infoObj);
        }
    }
}