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

[assembly: RegisterModule(typeof(NodeBazInitializationModule))]
namespace Demo
{
    public class NodeBazInitializationModule : Module
    {
        public NodeBazInitializationModule() : base("NodeBazInitializationModule") { }
        protected override void OnInit()
        {
            base.OnInit();

            // Manually Trigger document update staging task.
            NodeBazInfo.TYPEINFO.Events.Insert.After += NodeBaz_Insert_Or_Delete_After;
            NodeBazInfo.TYPEINFO.Events.Delete.After += NodeBaz_Insert_Or_Delete_After;

            // Manually add items to Document Update task
            StagingEvents.LogTask.Before += LogTask_Before;

            // Manuall handle the Staging Task and processes our Node bound objects
            StagingEvents.ProcessTask.After += ProcessTask_After;
        }

        private void NodeBaz_Insert_Or_Delete_After(object sender, ObjectEventArgs e)
        {
            RelHelper.HandleNodeBindingInsertUpdateDeleteEvent(((NodeBazInfo)e.Object).NodeBazNodeID, NodeBazInfo.OBJECT_TYPE);
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
                        #region "Node Baz (Node object w/out Ordering)"
                        
                        // Get NodeBaz and Handle
                        List<int> BazIDs = RelHelper.NewBoundObjectIDs(e, NodeBazInfo.OBJECT_TYPE, nameof(NodeBazInfo.NodeBazNodeID), nameof(NodeBazInfo.NodeBazBazID), BazInfo.TYPEINFO);

                        // Delete Ones not found
                        NodeBazInfo.Provider.Get().WhereEquals("NodeID", NodeObj.NodeID).WhereNotIn("BazID", BazIDs).ForEachObject(x => x.Delete());

                        // Find ones that need to be added and add
                        List<int> CurrentBazIDs = NodeBazInfo.Provider.Get().WhereEquals(nameof(NodeBazInfo.NodeBazNodeID), NodeObj.NodeID).Select(x => x.NodeBazBazID).ToList();
                        foreach (int NewBazID in BazIDs.Except(CurrentBazIDs))
                        {
                            NodeBazInfo.Provider.Add(NodeObj.NodeID, NewBazID);
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
                new NodeBinding_DocumentLogTaskBefore_Configuration(new NodeBazInfo(), nameof(NodeBazInfo.NodeBazNodeID)+" = {0}")
            });
        }
    }

}
