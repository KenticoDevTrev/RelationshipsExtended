using CMS;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Synchronization;
using CMS.Taxonomy;
using Demo;
using RelationshipsExtended;
using System.Collections.Generic;
using System.Data;
using System.Linq;

[assembly: RegisterModule(typeof(NodeFooInitializationModule))]
namespace Demo
{
    public class NodeFooInitializationModule : Module
    {
        public NodeFooInitializationModule() : base("NodeFooInitializationModule") { }
        protected override void OnInit()
        {
            base.OnInit();

            // Manually Trigger document update staging task.
            NodeFooInfo.TYPEINFO.Events.Insert.After += NodeFoo_Insert_Or_Delete_After;
            NodeFooInfo.TYPEINFO.Events.Delete.After += NodeFoo_Insert_Or_Delete_After;

            // Manually add items to Document Update task
            StagingEvents.LogTask.Before += LogTask_Before;

            // Manuall handle the Staging Task and processes our Node bound objects
            StagingEvents.ProcessTask.After += ProcessTask_After;
        }

        private void NodeFoo_Insert_Or_Delete_After(object sender, ObjectEventArgs e)
        {
            RelHelper.HandleNodeBindingInsertUpdateDeleteEvent(((NodeFooInfo)e.Object).NodeFooNodeID, NodeFooInfo.OBJECT_TYPE);
        }

        private void ProcessTask_After(object sender, StagingSynchronizationEventArgs e)
        {
            List<TaskTypeEnum> DocumentTaskTypes = new List<TaskTypeEnum>()
        {
            TaskTypeEnum.CreateDocument,
            TaskTypeEnum.UpdateDocument,
            TaskTypeEnum.PublishDocument
        };

            if (DocumentTaskTypes.Contains(e.TaskType))
            {
                // First table is the Node Data
                DataTable NodeTable = e.TaskData.Tables[0];

                if (NodeTable != null && NodeTable.Columns.Contains("NodeGuid"))
                {
                    // Get node ID
                    TreeNode NodeObj = new DocumentQuery().WhereEquals("NodeGUID", NodeTable.Rows[0]["NodeGuid"]).FirstOrDefault();

                    // Don't want to trigger updates as we set the data in the database, so we won't log synchronziations
                    using (new CMSActionContext()
                    {
                        LogSynchronization = false,
                        LogIntegration = false
                    })
                    {
                        #region "Node Foo  (Node object with Ordering)"

                        // Get NodeFoo and Handle
                        List<int> FooIDInOrders = RelHelper.NewOrderedBoundObjectIDs(e, NodeFooInfo.OBJECT_TYPE, nameof(NodeFooInfo.NodeFooNodeID), nameof(NodeFooInfo.NodeFooFooID), nameof(NodeFooInfo.NodeFooOrder), FooInfo.TYPEINFO);

                        // Delete those not found
                        NodeFooInfo.Provider.Get().WhereEquals(nameof(NodeFooInfo.NodeFooNodeID), NodeObj.NodeID).WhereNotIn(nameof(NodeFooInfo.NodeFooFooID), FooIDInOrders).ForEachObject(x => x.Delete());

                        // Get a list of the Current Foos, add missing
                        List<int> CurrentFooIDs = NodeFooInfo.Provider.Get().WhereEquals(nameof(NodeFooInfo.NodeFooNodeID), NodeObj.NodeID).Select(x => x.NodeFooFooID).ToList();
                        foreach (int NewFooID in FooIDInOrders.Except(CurrentFooIDs))
                        {
                            NodeFooInfo.Provider.Add(NodeObj.NodeID, NewFooID);
                        }
                        // Now handle the ordering
                        for (int FooIndex = 0; FooIndex < FooIDInOrders.Count; FooIndex++)
                        {
                            int FooID = FooIDInOrders[FooIndex];
                            NodeFooInfo CurrentObj = NodeFooInfo.Provider.Get(NodeObj.NodeID, FooID);
                            if (CurrentObj != null && CurrentObj.NodeFooOrder != (FooIndex + 1))
                            {
                                CurrentObj.SetObjectOrder(FooIndex + 1);
                            }
                        }

                        #endregion
                    }
                    if (RelHelper.IsStagingEnabled(NodeObj.NodeSiteID))
                    {
                        // Check if we need to generate a task for a server that isn't the origin server
                        RelHelper.CheckIfTaskCreationShouldOccur(NodeObj.NodeGUID);
                    }
                }
                else if (NodeTable == null || !NodeTable.Columns.Contains("NodeGuid"))
                {
                    Service.Resolve<IEventLogService>().LogEvent(EventTypeEnum.Error, "DemoProcessTask", "No Node Table Found", eventDescription: "First Table in the incoming Staging Task did not contain the Node GUID, could not processes.");
                }
            }
        }
        private void LogTask_Before(object sender, StagingLogTaskEventArgs e)
        {
            RelHelper.UpdateTaskDataWithNodeBinding(e, new NodeBinding_DocumentLogTaskBefore_Configuration[]
            {
                new NodeBinding_DocumentLogTaskBefore_Configuration(new NodeFooInfo(), nameof(NodeFooInfo.NodeFooNodeID)+" = {0}")
            });
        }
    }

}
