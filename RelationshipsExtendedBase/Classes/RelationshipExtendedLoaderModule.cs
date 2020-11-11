using CMS;
using CMS.Base;
using CMS.Core;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.Relationships;
using CMS.SiteProvider;
using CMS.Synchronization;
using CMS.Taxonomy;
using RelationshipsExtended;
using RelationshipsExtended.Enums;
using RelationshipsExtended.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

[assembly: RegisterModule(typeof(RelationshipsExtendedLoaderModuleBase))]
namespace RelationshipsExtended
{

    public class RelationshipsExtendedLoaderModuleBase : Module
    {
        public RelationshipsExtendedLoaderModuleBase()
                : base("RelationshipsExtendedLoaderModuleBase")
        {
        }


        // Contains initialization code that is executed when the application starts
        protected override void OnInit()
        {
            base.OnInit();

            // Custom Relationship Name logging since adhoc is disabled in staging by default (since usually tied to page type)
            RelationshipNameInfo.TYPEINFO.Events.Insert.After += RelationshipName_Insert_After;
            RelationshipNameInfo.TYPEINFO.Events.Update.After += RelationshipName_Update_After;
            RelationshipNameInfo.TYPEINFO.Events.Delete.After += RelationshipName_Delete_After;
            RelationshipNameSiteInfo.TYPEINFO.Events.Insert.After += RelationshipNameSiteInfo_Insert_After;
            RelationshipNameSiteInfo.TYPEINFO.Events.Delete.After += RelationshipNameSiteInfo_Delete_After;

            // Since normally a page is "Saved" (changed) when you handle ad-hoc relationships, must also handle triggering the update on the document
            RelationshipInfo.TYPEINFO.Events.Insert.After += Relationship_Insert_Or_Delete_After;
            RelationshipInfo.TYPEINFO.Events.Delete.After += Relationship_Insert_Or_Delete_After;

            // Add in events to handle Document-bound node categories, or adjust to synchronize manually
            if (DataHelper.GetNotEmpty(SettingsKeyInfoProvider.GetValue(new SettingsKeyName("NodeCategoryStagingMode")), "WithDocument") == "WithDocument")
            {
                // Similar to Relationships, a Node Category needs to touch the Node, however this really is touching the 'document' not the node, so must manually trigger
                TreeCategoryInfo.TYPEINFO.Events.Insert.After += TreeCategory_Insert_Or_Delete_After;
                TreeCategoryInfo.TYPEINFO.Events.Delete.After += TreeCategory_Insert_Or_Delete_After;

                // Need to add TreeCategories to document data set and then processes it since sadly isn't doing it automatically :(
                StagingEvents.LogTask.Before += LogTask_Before;
                StagingEvents.ProcessTask.After += ProcessTask_After;
            }
            else
            {
                // Add some custom logic to create a more readable Task Title
                StagingEvents.LogTask.Before += NonBindingLogTask_Before;
                // Handle object deletions, additions work but removals don't for node object relationships
                StagingEvents.ProcessTask.After += NonBindingNodeDocument_ProcessTask_After;
            }

            // Handle any tasks that need to be deleted due to originating from another server
            StagingEvents.LogTask.After += LogTask_After;

            // Also make sure that the foreign key exists for the class
            try
            {
                ConnectionHelper.ExecuteQuery("CMS.TreeCategory.EnsureForeignKeys", null);
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("RelationshipsExtended", "ErrorSettingForeignKeys", ex, additionalMessage: "Make sure the Query CMS.TreeCategory.EnsureForeignKey exists.  IGNORE if you just installed the module as this will run before the class installs on the first application start after installation.");
            }

            // Registers "CustomNamespace" into the macro engine
            MacroContext.GlobalResolver.SetNamedSourceData("RelHelper", RelHelperMacroNamespace.Instance);
            MacroContext.GlobalResolver.SetNamedSourceData("RelEnums", EnumMacroEvaluator.EnumMacroObjects());

            /* Check to make sure the 2 forms exist */
            if (AlternativeFormInfoProvider.GetAlternativeFormInfo("cms.relationshipname.NewForm") == null)
            {
                int ClassID = DataClassInfoProvider.GetDataClassInfo("cms.relationshipname").ClassID;
                AlternativeFormInfo RelationshipNewForm = new AlternativeFormInfo()
                {
                    FormClassID = ClassID,
                    FormName = "NewForm",
                    FormDisplayName = "New Form",
                    FormDefinition = "<form version=\"2\"><field column=\"RelationshipNameID\" guid=\"47839bd6-f19c-4cfd-b67f-1ca754694d46\" /><field column=\"RelationshipDisplayName\" guid=\"6515b190-003a-44b6-b541-8814760de218\" /><field column=\"RelationshipName\" guid=\"42221f4a-30fa-47a6-bc80-3f99ee81f8a5\" /><field column=\"RelationshipAllowedObjects\" guid=\"2a02c9d5-f0f9-4a19-be8d-9a007f4464ac\" /><field column=\"RelationshipNameIsAdHoc\" guid=\"f1d3667d-77eb-47de-9ad0-5f22ad63e082\" visible=\"true\"><settings><controlname>CheckBoxControl</controlname></settings><properties><fieldcaption>Relationship Is AdHoc (Sortable)</fieldcaption><fielddescription>Must be true if you wish to use sorting.</fielddescription></properties></field><field column=\"RelationshipGUID\" guid=\"03ad948a-2bb7-44b2-b580-b05abf3a2a8b\" /><field column=\"RelationshipLastModified\" guid=\"ea7edf35-ed86-4cef-91c5-7bfdde27c389\" /><field column=\"ReltionshipSite\" guid=\"a733ba02-3675-481a-b586-b87c49e23268\" /></form>",
                    FormHideNewParentFields = false,
                    FormIsCustom = true
                };
                AlternativeFormInfoProvider.SetAlternativeFormInfo(RelationshipNewForm);
            }
            if (AlternativeFormInfoProvider.GetAlternativeFormInfo("cms.relationshipname.EditForm") == null)
            {
                int ClassID = DataClassInfoProvider.GetDataClassInfo("cms.relationshipname").ClassID;
                AlternativeFormInfo RelationshipNewForm = new AlternativeFormInfo()
                {
                    FormClassID = ClassID,
                    FormName = "EditForm",
                    FormDisplayName = "Edit Form",
                    FormDefinition = "<form version=\"2\"><field column=\"RelationshipNameID\" guid=\"47839bd6-f19c-4cfd-b67f-1ca754694d46\" /><field column=\"RelationshipDisplayName\" guid=\"6515b190-003a-44b6-b541-8814760de218\" /><field column=\"RelationshipName\" guid=\"42221f4a-30fa-47a6-bc80-3f99ee81f8a5\" /><field column=\"RelationshipAllowedObjects\" guid=\"2a02c9d5-f0f9-4a19-be8d-9a007f4464ac\" /><field column=\"RelationshipNameIsAdHoc\" guid=\"f1d3667d-77eb-47de-9ad0-5f22ad63e082\" visible=\"true\"><settings><controlname>CheckBoxControl</controlname></settings><properties><fieldcaption>Relationship Is AdHoc (Sortable)</fieldcaption><fielddescription>Must be true if you wish to use sorting.</fielddescription></properties></field><field column=\"RelationshipGUID\" guid=\"03ad948a-2bb7-44b2-b580-b05abf3a2a8b\" /><field column=\"RelationshipLastModified\" guid=\"ea7edf35-ed86-4cef-91c5-7bfdde27c389\" /><field column=\"ReltionshipSite\" guid=\"a733ba02-3675-481a-b586-b87c49e23268\" /></form>",
                    FormHideNewParentFields = false,
                    FormIsCustom = true
                };
                AlternativeFormInfoProvider.SetAlternativeFormInfo(RelationshipNewForm);
            }
        }

        private void LogTask_After(object sender, StagingLogTaskEventArgs e)
        {
            try
            {
                if (e.Task.TaskDocumentID > 0 && CallContext.GetData("DeleteTasks") != null)
                {
                    TreeNode Node = DocumentHelper.GetDocument(e.Task.TaskDocumentID, null);
                    if (((List<int>)CallContext.GetData("DeleteTasks")).Contains(Node.NodeID) && (DateTime.Now - e.Task.TaskTime).TotalSeconds < 10)
                    {
                        e.Task.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                Service.Resolve<IEventLogService>().LogException("RelExtended", "LogTaskAfterError", ex, additionalMessage: "For task " + e.Task.TaskDocumentID);
            }
        }

        /// <summary>
        /// Handle the Removal of Node Categories when not bound to the document, since binding to the Nodes don't seem to operate right.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NonBindingNodeDocument_ProcessTask_After(object sender, StagingSynchronizationEventArgs e)
        {
            if (e.TaskType == TaskTypeEnum.DeleteObject)
            {
                // Don't want to trigger updates as we set the data in the database, so we won't log synchronziations
                using (new CMSActionContext()
                {
                    LogSynchronization = false,
                    LogIntegration = false
                })
                {
                    if (e.ObjectType.ToLower() == "cms.treecategory")
                    {
                        DataTable NodeCategoryTable = e.TaskData.Tables[0];
                        // Translate tables
                        int NodeID = RelHelper.TranslateBindingTranslateID((int)NodeCategoryTable.Rows[0]["NodeID"], e.TaskData, "cms.node");
                        int CategoryID = RelHelper.TranslateBindingTranslateID((int)NodeCategoryTable.Rows[0]["CategoryID"], e.TaskData, "cms.category");
                        if (NodeID > 0 && CategoryID > 0)
                        {
                            TreeCategoryInfo.Provider.Remove(NodeID, CategoryID);
                        }
                    }
                }
            }
        }

        private void NonBindingLogTask_Before(object sender, StagingLogTaskEventArgs e)
        {
            RelHelper.SetBetterBindingTaskTitle(e, TreeCategoryInfo.OBJECT_TYPE, "NodeID", "CategoryID", "Category", CategoryInfo.TYPEINFO);
        }

        private void ProcessTask_After(object sender, StagingSynchronizationEventArgs e)
        {
            if (e.TaskType == TaskTypeEnum.UpdateDocument || e.TaskType == TaskTypeEnum.CreateDocument || e.TaskType == TaskTypeEnum.MoveDocument || e.TaskType == TaskTypeEnum.PublishDocument || e.TaskType == TaskTypeEnum.ArchiveDocument)
            {
                // Seems the first table is always the node's table, the table name dose change by the document page type.
                DataTable NodeTable = e.TaskData.Tables[0];
                if (NodeTable != null && NodeTable.Columns.Contains("NodeGuid"))
                {
                    // Don't want to trigger updates as we set the data in the database, so we won't log synchronziations
                    TreeNode NodeObj = new DocumentQuery().WhereEquals("NodeGUID", NodeTable.Rows[0]["NodeGuid"]).FirstOrDefault();

                    using (new CMSActionContext()
                    {
                        LogSynchronization = false,
                        LogIntegration = false
                    })
                    {
                        List<int> NewNodeCategoryIDs = RelHelper.NewBoundObjectIDs(e, TreeCategoryInfo.OBJECT_TYPE, "NodeID", "CategoryID", CategoryInfo.TYPEINFO);

                        // Now handle categories, deleting categories not found, and adding ones that are not set yet.
                        TreeCategoryInfo.Provider.Get().WhereEquals("NodeID", NodeObj.NodeID).WhereNotIn("CategoryID", NewNodeCategoryIDs).ForEachObject(x => x.Delete());
                        List<int> CurrentCategories = TreeCategoryInfo.Provider.Get().WhereEquals("NodeID", NodeObj.NodeID).Select(x => x.CategoryID).ToList();
                        foreach (int NewCategoryID in NewNodeCategoryIDs.Except(CurrentCategories))
                        {
                            TreeCategoryInfo.Provider.Add(NodeObj.NodeID, NewCategoryID);
                        }
                    }
                    if (RelHelper.IsStagingEnabled(NodeObj.NodeSiteID))
                    {
                        TaskTypeEnum TaskTypeToUse = e.TaskType;
                        switch (e.TaskType)
                        {
                            case TaskTypeEnum.MoveDocument:
                                TaskTypeToUse = TaskTypeEnum.UpdateDocument;
                                break;
                        }
                        // Check if we need to generate a task for a server that isn't the origin server
                        RelHelper.CheckIfTaskCreationShouldOccur(NodeObj.NodeGUID, TaskTypeToUse);
                    }
                }
                else
                {
                    Service.Resolve<IEventLogService>().LogEvent(EventTypeEnum.Error, "RelationshipExended", "No Node Table Found", eventDescription: "First Table in the incoming Staging Task did not contain the Node GUID, could not processes.");
                }
            }
        }

        private void LogTask_Before(object sender, StagingLogTaskEventArgs e)
        {
            RelHelper.UpdateTaskDataWithNodeBinding(e, new NodeBinding_DocumentLogTaskBefore_Configuration[]
            {
                    new NodeBinding_DocumentLogTaskBefore_Configuration(
                        new TreeCategoryInfo(),
                        "NodeID = {0}")
            });
        }

        private void TreeCategory_Insert_Or_Delete_After(object sender, ObjectEventArgs e)
        {
            RelHelper.HandleNodeBindingInsertUpdateDeleteEvent(((TreeCategoryInfo)e.Object).NodeID, TreeCategoryInfo.OBJECT_TYPE);
        }

        private void Relationship_Insert_Or_Delete_After(object sender, ObjectEventArgs e)
        {
            RelationshipInfo RelationshipObj = (RelationshipInfo)e.Object;
            RelationshipNameInfo RelationshipNameObj = RelationshipNameInfo.Provider.Get(RelationshipObj.RelationshipNameId);

            if (IsCustomAdhocRelationshipName(RelationshipNameObj))
            {
                TreeNode LeftNode = new DocumentQuery().WhereEquals("NodeID", RelationshipObj.LeftNodeId).FirstOrDefault();
                if (RelHelper.IsStagingEnabled(LeftNode.NodeSiteID))
                {
                    DocumentSynchronizationHelper.LogDocumentChange(LeftNode.NodeSiteName, LeftNode.NodeAliasPath, TaskTypeEnum.UpdateDocument, LeftNode.TreeProvider);
                }
            }
        }

        private void RelationshipName_Insert_After(object sender, ObjectEventArgs e)
        {
            if (RelHelper.IsStagingEnabled())
            {
                RelationshipName_CreateStagingTask((RelationshipNameInfo)e.Object, TaskTypeEnum.CreateObject);
            }
        }

        private void RelationshipName_Update_After(object sender, ObjectEventArgs e)
        {
            if (RelHelper.IsStagingEnabled())
            {
                RelationshipName_CreateStagingTask((RelationshipNameInfo)e.Object, TaskTypeEnum.UpdateObject);
            }
        }

        private void RelationshipName_Delete_After(object sender, ObjectEventArgs e)
        {
            if (RelHelper.IsStagingEnabled())
            {
                RelationshipName_CreateStagingTask((RelationshipNameInfo)e.Object, TaskTypeEnum.DeleteObject);
            }
        }

        private void RelationshipNameSiteInfo_Insert_After(object sender, ObjectEventArgs e)
        {
            if (RelHelper.IsStagingEnabled())
            {
                RelationshipNameSite_CreateStagingTask((RelationshipNameSiteInfo)e.Object, TaskTypeEnum.AddToSite);
            }
        }

        private void RelationshipNameSiteInfo_Delete_After(object sender, ObjectEventArgs e)
        {
            if (RelHelper.IsStagingEnabled())
            {
                RelationshipNameSite_CreateStagingTask((RelationshipNameSiteInfo)e.Object, TaskTypeEnum.RemoveFromSite);
            }
        }

        /// <summary>
        /// Custom Staging Task generation
        /// </summary>
        /// <param name="RelationshipSiteObj"></param>
        /// <param name="TaskType"></param>
        private void RelationshipNameSite_CreateStagingTask(RelationshipNameSiteInfo RelationshipSiteObj, TaskTypeEnum TaskType)
        {
            List<ServerInfo> ActiveServers = ServerInfo.Provider.Get().WhereEquals("ServerSiteID", SiteContext.CurrentSiteID).WhereEquals("ServerEnabled", true).ToList();
            RelationshipNameInfo RelationshipObj = RelationshipNameInfo.Provider.Get(RelationshipSiteObj.RelationshipNameID);
            // If relationship obj is already gone, then the Site deletion thing is already handled with the deletion of the relationship name.
            if (RelationshipObj == null)
            {
                return;
            }

            if (IsCustomAdhocRelationshipName(RelationshipObj) && ActiveServers.Count > 0)
            {
                string Data = "<NewDataSet>" + RelationshipObj.ToXML("CMS_RelationshipName", false) + "</NewDataSet>";
                string TaskTitle = "";
                string TaskTitleEnd = "";
                switch (TaskType)
                {
                    case TaskTypeEnum.AddToSite:
                        TaskTitle = "Add";
                        TaskTitleEnd = "to";
                        break;
                    case TaskTypeEnum.RemoveFromSite:
                        TaskTitle = "Remove";
                        TaskTitleEnd = "from";
                        break;
                }
                StagingTaskInfo SiteTask = new CMS.Synchronization.StagingTaskInfo()
                {
                    TaskTitle = string.Format("{0} Relationship name '{1}' {2} site", TaskTitle, RelationshipObj.RelationshipDisplayName, TaskTitleEnd),
                    TaskType = TaskType,
                    TaskObjectType = RelationshipNameInfo.OBJECT_TYPE,
                    TaskObjectID = RelationshipObj.RelationshipNameId,
                    TaskData = Data,
                    TaskTime = DateTime.Now,
                    TaskSiteID = SiteContext.CurrentSiteID
                };
                StagingTaskInfo.Provider.Set(SiteTask);

                foreach (ServerInfo ServerObj in ActiveServers)
                {

                    // Create synchronization
                    SynchronizationInfo SyncSiteInfo = new SynchronizationInfo()
                    {
                        SynchronizationTaskID = SiteTask.TaskID,
                        SynchronizationServerID = ServerObj.ServerID
                    };
                    SynchronizationInfo.Provider.Set(SyncSiteInfo);
                }

                TaskGroupInfo TaskGroup = TaskGroupInfoProvider.GetUserTaskGroupInfo(MembershipContext.AuthenticatedUser.UserID);
                if (TaskGroup != null)
                {
                    TaskGroupTaskInfo.Provider.Add(TaskGroup.TaskGroupID, SiteTask.TaskID);
                }
            }
        }

        /// <summary>
        /// Creates the Staging Task manually
        /// </summary>
        /// <param name="RelationshipObj"></param>
        /// <param name="TaskType"></param>
        private void RelationshipName_CreateStagingTask(RelationshipNameInfo RelationshipObj, TaskTypeEnum TaskType)
        {
            List<ServerInfo> ActiveServers = ServerInfo.Provider.Get().WhereEquals("ServerSiteID", SiteContext.CurrentSiteID).WhereEquals("ServerEnabled", true).ToList();

            if (IsCustomAdhocRelationshipName(RelationshipObj) && ActiveServers.Count > 0)
            {

                string Data = "<NewDataSet>" + RelationshipObj.ToXML("CMS_RelationshipName", false) + "</NewDataSet>";
                string TaskTitle = "";
                switch (TaskType)
                {
                    case TaskTypeEnum.CreateObject:
                        TaskTitle = "Create";
                        break;
                    case TaskTypeEnum.UpdateObject:
                        TaskTitle = "Update";
                        break;
                    case TaskTypeEnum.DeleteObject:
                        TaskTitle = "Delete";
                        break;
                }
                StagingTaskInfo Task = new StagingTaskInfo()
                {
                    TaskTitle = string.Format("{0} Relationship name '{1}'", TaskTitle, RelationshipObj.RelationshipDisplayName),
                    TaskType = TaskType,
                    TaskObjectType = RelationshipNameInfo.OBJECT_TYPE,
                    TaskObjectID = RelationshipObj.RelationshipNameId,
                    TaskData = Data,
                    TaskTime = DateTime.Now
                };
                StagingTaskInfo.Provider.Set(Task);

                foreach (ServerInfo ServerObj in ActiveServers)
                {
                    // Create synchronization
                    SynchronizationInfo SyncInfo = new SynchronizationInfo()
                    {
                        SynchronizationTaskID = Task.TaskID,
                        SynchronizationServerID = ServerObj.ServerID
                    };
                    SynchronizationInfo.Provider.Set(SyncInfo);
                }

                TaskGroupInfo TaskGroup = TaskGroupInfoProvider.GetUserTaskGroupInfo(MembershipContext.AuthenticatedUser.UserID);
                if (TaskGroup != null)
                {
                    TaskGroupTaskInfo.Provider.Add(TaskGroup.TaskGroupID, Task.TaskID);
                }
            }
        }

        /// <summary>
        /// Determines if the RelationshipName is a custom AdHoc relationship or if it's a PageType generated adhoc one based on the Guid at the end of the code name
        /// </summary>
        /// <param name="RelationshipNameObj">The Relationship Name Info Obj</param>
        /// <returns>If it's a custom Ad Hoc or not</returns>
        private bool IsCustomAdhocRelationshipName(RelationshipNameInfo RelationshipNameObj)
        {
            if (!RelationshipNameObj.RelationshipNameIsAdHoc)
            {
                return false;
            }
            if (!RelationshipNameObj.RelationshipName.Contains("_"))
            {
                return true;
            }
            return ValidationHelper.GetGuid(RelationshipNameObj.RelationshipName.Split('_')[1], Guid.Empty) == Guid.Empty;
        }
    }

}