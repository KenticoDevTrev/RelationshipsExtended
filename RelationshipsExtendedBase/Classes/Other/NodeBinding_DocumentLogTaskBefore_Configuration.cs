using CMS.DataEngine;

namespace RelationshipsExtended
{ 
    /// <summary>
    /// Used in the RelHelper.UpdateTaskDataWithNodeBinding, pass this to allow the method to automatically find and attach the Bound data to the TaskData
    /// </summary>
    public class NodeBinding_DocumentLogTaskBefore_Configuration
    {
        public BaseInfo EmptyNodeBindingObj;
        public string NodeMatchStringFormat;
        /// <summary>
        /// Provides the needed information for the Document.LogTask.Before to properly append Node-bound objects to the Document update task.
        /// </summary>
        /// <param name="EmptyNodeBindingObj">An empty instance of your Binding Class Info (ex new NodeItemsInfo())</param>
        /// <param name="NodeMatchStringFormat">ex "NodeID = {0}" if your binding table has 'NodeID' as it's reference field. Used in a String.Format() to create the Where condition to find the related objects. </param>
        public NodeBinding_DocumentLogTaskBefore_Configuration(BaseInfo EmptyNodeBindingObj, string NodeMatchStringFormat)
        {
            this.EmptyNodeBindingObj = EmptyNodeBindingObj;
            this.NodeMatchStringFormat = NodeMatchStringFormat;
        }
    }
}
