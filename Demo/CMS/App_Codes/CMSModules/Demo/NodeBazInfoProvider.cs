using System;
using System.Linq;

using CMS.DataEngine;

namespace Demo
{    
    /// <summary>
    /// Class providing NodeBazInfo management.
    /// </summary>
    public partial class NodeBazInfoProvider : AbstractInfoProvider<NodeBazInfo, NodeBazInfoProvider>
    {
        #region "Public methods"

		/// <summary>
        /// Returns all NodeBazInfo bindings.
        /// </summary>
        public static ObjectQuery<NodeBazInfo> GetNodeBazes()
        {
            return ProviderObject.GetObjectQuery();
        }


		/// <summary>
        /// Returns NodeBazInfo binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="bazId">ObjectType.demo_baz ID</param>  
        public static NodeBazInfo GetNodeBazInfo(int nodeId, int bazId)
        {
            return ProviderObject.GetNodeBazInfoInternal(nodeId, bazId);
        }


        /// <summary>
        /// Sets specified NodeBazInfo.
        /// </summary>
        /// <param name="infoObj">NodeBazInfo to set</param>
        public static void SetNodeBazInfo(NodeBazInfo infoObj)
        {
            ProviderObject.SetNodeBazInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified NodeBazInfo binding.
        /// </summary>
        /// <param name="infoObj">NodeBazInfo object</param>
        public static void DeleteNodeBazInfo(NodeBazInfo infoObj)
        {
            ProviderObject.DeleteNodeBazInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes NodeBazInfo binding.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="bazId">ObjectType.demo_baz ID</param>  
        public static void RemoveTreeFromBaz(int nodeId, int bazId)
        {
            ProviderObject.RemoveTreeFromBazInternal(nodeId, bazId);
        }


        /// <summary>
        /// Creates NodeBazInfo binding. 
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="bazId">ObjectType.demo_baz ID</param>   
        public static void AddTreeToBaz(int nodeId, int bazId)
        {
            ProviderObject.AddTreeToBazInternal(nodeId, bazId);
        }

        #endregion


        #region "Internal methods"
	
        /// <summary>
        /// Returns the NodeBazInfo structure.
        /// Null if binding doesn't exist.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="bazId">ObjectType.demo_baz ID</param>  
        protected virtual NodeBazInfo GetNodeBazInfoInternal(int nodeId, int bazId)
        {
            return GetSingleObject()
                .WhereEquals("NodeID", nodeId)
                .WhereEquals("BazID", bazId);
        }


        /// <summary>
        /// Sets specified NodeBazInfo binding.
        /// </summary>
        /// <param name="infoObj">NodeBazInfo object</param>
        protected virtual void SetNodeBazInfoInternal(NodeBazInfo infoObj)
        {
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified NodeBazInfo.
        /// </summary>
        /// <param name="infoObj">NodeBazInfo object</param>
        protected virtual void DeleteNodeBazInfoInternal(NodeBazInfo infoObj)
        {
            DeleteInfo(infoObj);
        }


		/// <summary>
        /// Deletes NodeBazInfo binding.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="bazId">ObjectType.demo_baz ID</param>  
        protected virtual void RemoveTreeFromBazInternal(int nodeId, int bazId)
        {
            var infoObj = GetNodeBazInfo(nodeId, bazId);
			if (infoObj != null) 
			{
				DeleteNodeBazInfo(infoObj);
			}
        }


        /// <summary>
        /// Creates NodeBazInfo binding. 
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="bazId">ObjectType.demo_baz ID</param>   
        protected virtual void AddTreeToBazInternal(int nodeId, int bazId)
        {
            // Create new binding
            var infoObj = new NodeBazInfo();
            infoObj.NodeID = nodeId;
			infoObj.BazID = bazId;

            // Save to the database
            SetNodeBazInfo(infoObj);
        }
       
        #endregion		
    }
}