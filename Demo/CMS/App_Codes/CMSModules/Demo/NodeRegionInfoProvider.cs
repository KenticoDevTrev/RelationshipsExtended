using System;
using System.Linq;

using CMS.DataEngine;

namespace Demo
{
    /// <summary>
    /// Class providing NodeRegionInfo management.
    /// </summary>
    public partial class NodeRegionInfoProvider : AbstractInfoProvider<NodeRegionInfo, NodeRegionInfoProvider>
    {
        #region "Public methods"

        /// <summary>
        /// Returns all NodeRegionInfo bindings.
        /// </summary>
        public static ObjectQuery<NodeRegionInfo> GetNodeRegions()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns NodeRegionInfo binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="categoryId">Content category ID</param>  
        public static NodeRegionInfo GetNodeRegionInfo(int nodeId, int categoryId)
        {
            return ProviderObject.GetNodeRegionInfoInternal(nodeId, categoryId);
        }


        /// <summary>
        /// Sets specified NodeRegionInfo.
        /// </summary>
        /// <param name="infoObj">NodeRegionInfo to set</param>
        public static void SetNodeRegionInfo(NodeRegionInfo infoObj)
        {
            ProviderObject.SetNodeRegionInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified NodeRegionInfo binding.
        /// </summary>
        /// <param name="infoObj">NodeRegionInfo object</param>
        public static void DeleteNodeRegionInfo(NodeRegionInfo infoObj)
        {
            ProviderObject.DeleteNodeRegionInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes NodeRegionInfo binding.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="categoryId">Content category ID</param>  
        public static void RemoveTreeFromCategory(int nodeId, int categoryId)
        {
            ProviderObject.RemoveTreeFromCategoryInternal(nodeId, categoryId);
        }


        /// <summary>
        /// Creates NodeRegionInfo binding. 
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="categoryId">Content category ID</param>   
        public static void AddTreeToCategory(int nodeId, int categoryId)
        {
            ProviderObject.AddTreeToCategoryInternal(nodeId, categoryId);
        }

        #endregion


        #region "Internal methods"

        /// <summary>
        /// Returns the NodeRegionInfo structure.
        /// Null if binding doesn't exist.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="categoryId">Content category ID</param>  
        protected virtual NodeRegionInfo GetNodeRegionInfoInternal(int nodeId, int categoryId)
        {
            return GetSingleObject()
                .WhereEquals("NodeID", nodeId)
                .WhereEquals("RegionCategoryID", categoryId);
        }


        /// <summary>
        /// Sets specified NodeRegionInfo binding.
        /// </summary>
        /// <param name="infoObj">NodeRegionInfo object</param>
        protected virtual void SetNodeRegionInfoInternal(NodeRegionInfo infoObj)
        {
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified NodeRegionInfo.
        /// </summary>
        /// <param name="infoObj">NodeRegionInfo object</param>
        protected virtual void DeleteNodeRegionInfoInternal(NodeRegionInfo infoObj)
        {
            DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes NodeRegionInfo binding.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="categoryId">Content category ID</param>  
        protected virtual void RemoveTreeFromCategoryInternal(int nodeId, int categoryId)
        {
            var infoObj = GetNodeRegionInfo(nodeId, categoryId);
            if (infoObj != null)
            {
                DeleteNodeRegionInfo(infoObj);
            }
        }


        /// <summary>
        /// Creates NodeRegionInfo binding. 
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="categoryId">Content category ID</param>   
        protected virtual void AddTreeToCategoryInternal(int nodeId, int categoryId)
        {
            // Create new binding
            var infoObj = new NodeRegionInfo();
            infoObj.NodeID = nodeId;
            infoObj.RegionCategoryID = categoryId;

            // Save to the database
            SetNodeRegionInfo(infoObj);
        }

        #endregion
    }
}