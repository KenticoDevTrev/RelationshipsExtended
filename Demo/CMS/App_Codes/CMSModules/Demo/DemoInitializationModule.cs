using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Synchronization;
using Demo;
using System;
using System.Data;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Timers;
using System.Linq;
using CMS.EventLog;
using System.Collections.Generic;
using RelationshipsExtended;
using CMS.Taxonomy;
using CMS.Membership;

[assembly: RegisterModule(typeof(DemoInitializationModule))]

/// <summary>
/// Summary description for DemoInitializationModule
/// </summary>
public class DemoInitializationModule : Module
{
    public DemoInitializationModule() : base("DemoInitializationModule")
    {
    }
    // This is done in your OnInit for a Module class
    protected override void OnInit()
    {
        base.OnInit();

        // Manually Trigger document update staging task.
        NodeBazInfo.TYPEINFO.Events.Insert.After += NodeBaz_Insert_Or_Delete_After;
        NodeBazInfo.TYPEINFO.Events.Delete.After += NodeBaz_Insert_Or_Delete_After;

        // Manually Trigger document update staging task.
        NodeFooInfo.TYPEINFO.Events.Insert.After += NodeFoo_Insert_Or_Update_Or_Delete_After;
        NodeFooInfo.TYPEINFO.Events.Update.After += NodeFoo_Insert_Or_Update_Or_Delete_After;
        NodeFooInfo.TYPEINFO.Events.Delete.After += NodeFoo_Insert_Or_Update_Or_Delete_After;

        // Manually Trigger document update staging task.
        NodeRegionInfo.TYPEINFO.Events.Insert.After += NodeRegion_Insert_Or_Delete_After;
        NodeRegionInfo.TYPEINFO.Events.Delete.After += NodeRegion_Insert_Or_Delete_After;

        // Manually add items to Document Update task
        StagingEvents.LogTask.Before += LogTask_Before;

        // Manuall handle the Staging Task and processes our Node bound objects
        StagingEvents.ProcessTask.After += ProcessTask_After;
    }

    private void ProcessTask_After(object sender, StagingSynchronizationEventArgs e)
    {
        if (e.TaskType == TaskTypeEnum.UpdateDocument)
        {
            // First table is the Node Data
            DataTable NodeTable = e.TaskData.Tables[0];

            if (NodeTable != null && NodeTable.Columns.Contains("NodeGuid"))
            {
                // Get node ID
                TreeNode NodeObj = new DocumentQuery().WhereEquals("NodeGUID", NodeTable.Rows[0]["NodeGuid"]).FirstObject;

                // Don't want to trigger updates as we set the data in the database, so we won't log synchronziations
                using (new CMSActionContext()
                {
                    LogSynchronization = false,
                    LogIntegration = false
                })
                {
                    #region "Node Baz"
                    // Get NodeBaz and Handle
                    List<int> BazIDs = RelHelper.NewBoundObjectIDs(e, "demo.nodebaz", "NodeID", "BazID", BazInfo.TYPEINFO);
                    NodeBazInfoProvider.GetNodeBazes().WhereEquals("NodeID", NodeObj.NodeID).WhereNotIn("BazID", BazIDs).ForEachObject(x => x.Delete());
                    List<int> CurrentBazIDs = NodeBazInfoProvider.GetNodeBazes().WhereEquals("NodeID", NodeObj.NodeID).Select(x => x.BazID).ToList();
                    foreach (int NewBazID in BazIDs.Except(CurrentBazIDs))
                    {
                        NodeBazInfoProvider.AddTreeToBaz(NodeObj.NodeID, NewBazID);
                    }
                    #endregion

                    #region "Node Foo (Ordered)"
                    // Get NodeFoo and Handle
                    List<int> FooIDInOrders = RelHelper.NewOrderedBoundObjectIDs(e, "demo.nodeFoo", "NodeID", "FooID", "NodeFooOrder", FooInfo.TYPEINFO);
                    NodeFooInfoProvider.GetNodeFoos().WhereEquals("NodeID", NodeObj.NodeID).WhereNotIn("FooID", FooIDInOrders).ForEachObject(x => x.Delete());
                    List<int> CurrentFooIDs = NodeFooInfoProvider.GetNodeFoos().WhereEquals("NodeID", NodeObj.NodeID).Select(x => x.FooID).ToList();
                    foreach (int NewFooID in FooIDInOrders.Except(CurrentFooIDs))
                    {
                        NodeFooInfoProvider.AddTreeToFoo(NodeObj.NodeID, NewFooID);
                    }
                    // Now handle the ordering
                    for (int FooIndex = 0; FooIndex < FooIDInOrders.Count; FooIndex++)
                    {
                        int FooID = FooIDInOrders[FooIndex];
                        NodeFooInfo CurrentObj = NodeFooInfoProvider.GetNodeFooInfo(NodeObj.NodeID, FooID);
                        if (CurrentObj != null && CurrentObj.NodeFooOrder != (FooIndex + 1))
                        {
                            CurrentObj.SetObjectOrder(FooIndex + 1);
                        }
                    }
                    #endregion

                    #region "Node Region"
                    // Get NodeRegion and Handle
                    List<int> RegionCategoryIDs = RelHelper.NewBoundObjectIDs(e, "demo.nodeRegion", "NodeID", "RegionCategoryID", CategoryInfo.TYPEINFO);
                    NodeRegionInfoProvider.GetNodeRegions().WhereEquals("NodeID", NodeObj.NodeID).WhereNotIn("RegionCategoryID", RegionCategoryIDs).ForEachObject(x => x.Delete());
                    List<int> CurrentRegionCategoryIDs = NodeRegionInfoProvider.GetNodeRegions().WhereEquals("NodeID", NodeObj.NodeID).Select(x => x.RegionCategoryID).ToList();
                    foreach (int NewRegionCategoryID in RegionCategoryIDs.Except(CurrentRegionCategoryIDs))
                    {
                        NodeRegionInfoProvider.AddTreeToCategory(NodeObj.NodeID, NewRegionCategoryID);
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
                EventLogProvider.LogEvent("E", "DemoProcessTask", "No Node Table Found", eventDescription: "First Table in the incoming Staging Task did not contain the Node GUID, could not processes.");
            }
        }
    }

    private void NodeBaz_Insert_Or_Delete_After(object sender, ObjectEventArgs e)
    {
        RelHelper.HandleNodeBindingInsertUpdateDeleteEvent(((NodeBazInfo)e.Object).NodeID, "demo.nodebaz", MembershipContext.AuthenticatedUser.UserID);
    }
    private void NodeFoo_Insert_Or_Update_Or_Delete_After(object sender, ObjectEventArgs e)
    {
        RelHelper.HandleNodeBindingInsertUpdateDeleteEvent(((NodeFooInfo)e.Object).NodeID, "demo.nodefoo", MembershipContext.AuthenticatedUser.UserID);
    }
    private void NodeRegion_Insert_Or_Delete_After(object sender, ObjectEventArgs e)
    {
        RelHelper.HandleNodeBindingInsertUpdateDeleteEvent(((NodeRegionInfo)e.Object).NodeID, "demo.noderegion", MembershipContext.AuthenticatedUser.UserID);
    }

    private void LogTask_Before(object sender, StagingLogTaskEventArgs e)
    {
        RelHelper.UpdateTaskDataWithNodeBinding(e, new NodeBinding_DocumentLogTaskBefore_Configuration[]
        {
                new NodeBinding_DocumentLogTaskBefore_Configuration(new NodeBazInfo(), "NodeID = {0}"),
                new NodeBinding_DocumentLogTaskBefore_Configuration(new NodeFooInfo(), "NodeID = {0}"),
                new NodeBinding_DocumentLogTaskBefore_Configuration(new NodeRegionInfo(), "NodeID = {0}")
        });
    }
}