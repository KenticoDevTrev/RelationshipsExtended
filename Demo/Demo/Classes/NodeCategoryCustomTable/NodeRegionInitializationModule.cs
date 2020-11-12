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
using RelationshipsExtended.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;

[assembly: RegisterModule(typeof(NodeRegionInitializationModule))]
namespace Demo
{
    public class NodeRegionInitializationModule : Module
    {
        public NodeRegionInitializationModule() : base("NodeRegionInitializationModule") { }
        protected override void OnInit()
        {
            base.OnInit();

            // Manually Trigger document update staging task.
            NodeRegionInfo.TYPEINFO.Events.Insert.After += NodeRegion_Insert_Or_Delete_After;
            NodeRegionInfo.TYPEINFO.Events.Delete.After += NodeRegion_Insert_Or_Delete_After;

            // Manually add items to Document Update task
            StagingEvents.LogTask.Before += LogTask_Before;

            // Manuall handle the Staging Task and processes our Node bound objects
            StagingEvents.ProcessTask.After += ProcessTask_After;
        }

        private void NodeRegion_Insert_Or_Delete_After(object sender, ObjectEventArgs e)
        {
            RelHelper.HandleNodeBindingInsertUpdateDeleteEvent(((NodeRegionInfo)e.Object).NodeRegionNodeID, NodeRegionInfo.OBJECT_TYPE);
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
                        #region "Node Region (Node object w/out Ordering)"
                        // Get Region Categories
                        List<int> RegionCategoryIDs = RelHelper.NewBoundObjectIDs(e, NodeRegionInfo.OBJECT_TYPE, nameof(NodeRegionInfo.NodeRegionNodeID), nameof(NodeRegionInfo.NodeRegionCategoryID), CategoryInfo.TYPEINFO);

                        // Delete Ones not found
                        NodeRegionInfo.Provider.Get().WhereEquals(nameof(NodeRegionInfo.NodeRegionNodeID), NodeObj.NodeID).WhereNotIn(nameof(NodeRegionInfo.NodeRegionCategoryID), RegionCategoryIDs).ForEachObject(x => x.Delete());

                        // Find ones that need to be added and add
                        List<int> CurrentRegionCategoryIDs = NodeRegionInfo.Provider.Get().WhereEquals(nameof(NodeRegionInfo.NodeRegionNodeID), NodeObj.NodeID).Select(x => x.NodeRegionCategoryID).ToList();
                        foreach (int NewRegionCategoryID in RegionCategoryIDs.Except(CurrentRegionCategoryIDs))
                        {
                            NodeRegionInfo.Provider.Add(NodeObj.NodeID, NewRegionCategoryID);
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
                new NodeBinding_DocumentLogTaskBefore_Configuration(new NodeRegionInfo(), nameof(NodeRegionInfo.NodeRegionNodeID)+" = {0}")
            });
        }
    }
    
}
