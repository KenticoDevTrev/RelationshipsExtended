using System;
using System.Linq;
using CMS.Helpers;
using CMS.DataEngine;

namespace Demo
{    
    /// <summary>
    /// Class providing NodeFooInfo management.
    /// </summary>
    public partial class NodeFooInfoProvider : AbstractInfoProvider<NodeFooInfo, NodeFooInfoProvider>
    {
        #region "Public methods"

		/// <summary>
        /// Returns all NodeFooInfo bindings.
        /// </summary>
        public static ObjectQuery<NodeFooInfo> GetNodeFoos()
        {
            return ProviderObject.GetObjectQuery();
        }


		/// <summary>
        /// Returns NodeFooInfo binding structure.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="fooId">ObjectType.demo_foo ID</param>  
        public static NodeFooInfo GetNodeFooInfo(int nodeId, int fooId)
        {
            return ProviderObject.GetNodeFooInfoInternal(nodeId, fooId);
        }


        /// <summary>
        /// Sets specified NodeFooInfo.
        /// </summary>
        /// <param name="infoObj">NodeFooInfo to set</param>
        public static void SetNodeFooInfo(NodeFooInfo infoObj)
        {
            ProviderObject.SetNodeFooInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified NodeFooInfo binding.
        /// </summary>
        /// <param name="infoObj">NodeFooInfo object</param>
        public static void DeleteNodeFooInfo(NodeFooInfo infoObj)
        {
            ProviderObject.DeleteNodeFooInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes NodeFooInfo binding.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="fooId">ObjectType.demo_foo ID</param>  
        public static void RemoveTreeFromFoo(int nodeId, int fooId)
        {
            ProviderObject.RemoveTreeFromFooInternal(nodeId, fooId);
        }


        /// <summary>
        /// Creates NodeFooInfo binding. 
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="fooId">ObjectType.demo_foo ID</param>   
        public static void AddTreeToFoo(int nodeId, int fooId)
        {
            ProviderObject.AddTreeToFooInternal(nodeId, fooId);
        }

        #endregion


        #region "Internal methods"
	
        /// <summary>
        /// Returns the NodeFooInfo structure.
        /// Null if binding doesn't exist.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="fooId">ObjectType.demo_foo ID</param>  
        protected virtual NodeFooInfo GetNodeFooInfoInternal(int nodeId, int fooId)
        {
            return GetSingleObject()
                .WhereEquals("NodeID", nodeId)
                .WhereEquals("FooID", fooId);
        }


        /// <summary>
        /// Sets specified NodeFooInfo binding.
        /// </summary>
        /// <param name="infoObj">NodeFooInfo object</param>
        protected virtual void SetNodeFooInfoInternal(NodeFooInfo inNodebj)
        {
            // Customization 1 - On Insert or update, check and set the Order
            if (ValidationHelper.GetInteger(inNodebj.GetValue("NodeFooOrder"), -1) <= 0)
            {
                inNodebj.NodeFooOrder = GetNodeFoos().WhereEquals("NodeID", inNodebj.NodeID).Count + 1;
            }
            SetInfo(inNodebj);
        }


        /// <summary>
        /// Deletes specified NodeFooInfo.
        /// </summary>
        /// <param name="infoObj">NodeFooInfo object</param>
        protected virtual void DeleteNodeFooInfoInternal(NodeFooInfo inNodebj)
        {
            DeleteInfo(inNodebj);

            // Customization 2, on deletion re-order
            // Initialize Order, the inNodebj should still exist in memory and only needed the Generalized portion
            inNodebj.Generalized.InitObjectsOrder(null);
        }


        /// <summary>
        /// Deletes NodeFooInfo binding.
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="fooId">ObjectType.demo_foo ID</param>  
        protected virtual void RemoveTreeFromFooInternal(int nodeId, int fooId)
        {
            var infoObj = GetNodeFooInfo(nodeId, fooId);
			if (infoObj != null) 
			{
				DeleteNodeFooInfo(infoObj);
			}
        }


        /// <summary>
        /// Creates NodeFooInfo binding. 
        /// </summary>
        /// <param name="nodeId">Node ID</param>
        /// <param name="fooId">ObjectType.demo_foo ID</param>   
        protected virtual void AddTreeToFooInternal(int nodeId, int fooId)
        {
            // Create new binding
            var infoObj = new NodeFooInfo();
            infoObj.NodeID = nodeId;
			infoObj.FooID = fooId;

            // Save to the database
            SetNodeFooInfo(infoObj);
        }
       
        #endregion		
    }
}