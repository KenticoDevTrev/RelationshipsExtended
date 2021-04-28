using CMS.Base;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Helpers;
using CMS.LicenseProvider;
using CMS.Membership;
using CMS.OnlineForms;
using CMS.SiteProvider;
using CMS.Synchronization;
using CMS.Taxonomy;
using RelationshipsExtended.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Timers;
using System.Web;
using System.Xml;

namespace RelationshipsExtended
{
    public static class RelHelper
    {
        #region "Where Condition Generators"

        /// <summary>
        /// Returns a full where condition (for Document Category Relationships) to be used in filtering (ex repeaters).  
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="DocumentIDTableName">The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        public static string GetDocumentCategoryWhere(IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string DocumentIDTableName = "CMS_Document")
        {
            IEnumerable<int> CategoryIDs = null;
            bool CacheWhere = SettingsKeyInfoProvider.GetBoolValue("CacheRelationshipWhereGeneration", new SiteInfoIdentifier(SiteContext.CurrentSiteID));
            return CacheHelper.Cache<string>(cs =>
            {
                CategoryIDs = CategoryIdentitiesToIDs(Values);
                if (CategoryIDs.Count() == 0)
                {
                    return "(1=1)";
                }
                switch (Condition)
                {
                    case ConditionType.Any:
                    default:
                        return string.Format("(DocumentID in (Select DocumentID from CMS_DocumentCategory where CategoryID in ({0})))", string.Join(",", CategoryIDs));
                    case ConditionType.All:
                        return string.Format("(Select Count(*) from CMS_DocumentCategory where CMS_DocumentCategory.DocumentID = {0}.[DocumentID] and CategoryID in ({1})) = {2}", DocumentIDTableName, string.Join(",", CategoryIDs), CategoryIDs.Count());
                    case ConditionType.None:
                        return string.Format("(DocumentID not in (Select DocumentID from CMS_DocumentCategory where CategoryID in ({0})))", string.Join(",", CategoryIDs), CategoryIDs.Count());
                }
            }, new CacheSettings((CacheWhere ? CacheHelper.CacheMinutes(SiteContext.CurrentSiteName) : 0), "GetDocumentCategoryWhere", string.Join("|", Values), Condition, DocumentIDTableName));
        }

        /// <summary>
        /// Returns a full where condition (for Node Category Relationships) to be used in filtering (ex repeaters).  
        /// </summary>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="NodeIDTableName">The Table Name/Alias where the NodeID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        public static string GetNodeCategoryWhere(IEnumerable<object> Values, ConditionType Condition = ConditionType.Any, string NodeIDTableName = "CMS_Tree")
        {
            IEnumerable<int> CategoryIDs = null;
            bool CacheWhere = SettingsKeyInfoProvider.GetBoolValue("CacheRelationshipWhereGeneration", new SiteInfoIdentifier(SiteContext.CurrentSiteID));
            return CacheHelper.Cache<string>(cs =>
            {
                CategoryIDs = CategoryIdentitiesToIDs(Values);
                if (CategoryIDs.Count() == 0)
                {
                    return "(1=1)";
                }
                switch (Condition)
                {
                    case ConditionType.Any:
                    default:
                        return string.Format("(NodeID in (Select NodeID from CMS_TreeCategory where CategoryID in ({0})))", string.Join(",", CategoryIDs));
                    case ConditionType.All:
                        return string.Format("(Select Count(*) from CMS_TreeCategory where CMS_TreeCategory.NodeID = {0}.[NodeID] and CategoryID in ({1})) = {2}", NodeIDTableName, string.Join(",", CategoryIDs), CategoryIDs.Count());
                    case ConditionType.None:
                        return string.Format("(NodeID not in (Select NodeID from CMS_TreeCategory where CategoryID in ({0})))", string.Join(",", CategoryIDs), CategoryIDs.Count());
                }
            }, new CacheSettings((CacheWhere ? CacheHelper.CacheMinutes(SiteContext.CurrentSiteName) : 0), "GetNodeCategoryWhere", string.Join("|", Values), Condition, NodeIDTableName));
        }

        /// <summary>
        /// Returns a full where condition (for Binding tables that bind an object to Categories) to be used in filtering (ex repeaters).  For property examples we will use Demo.Foo, CMS.Category, and Demo.FooCategory  
        /// </summary>
        /// <param name="BindingClass">The Binding Class Code Name</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID (From Demo.Foo)</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: FooID (from Demo.FooCategory)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the Category's identy value.  Ex: CategoryID (from Demo.FooCategory) </param>
        /// <param name="Values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_Foo</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        public static string GetBindingCategoryWhere(string BindingClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<object> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            LeftFieldName = GetBracketedColumnName(LeftFieldName);
            RightFieldName = GetBracketedColumnName(RightFieldName);
            ObjectIDFieldName = GetBracketedColumnName(ObjectIDFieldName);
            bool CacheWhere = SettingsKeyInfoProvider.GetBoolValue("CacheRelationshipWhereGeneration", new SiteInfoIdentifier(SiteContext.CurrentSiteID));
            return CacheHelper.Cache<string>(cs =>
            {
                // Find class table name
                DataClassInfo ClassObj = DataClassInfoProvider.GetDataClassInfo(BindingClass);
                if (ClassObj == null || string.IsNullOrEmpty(ObjectIDFieldName) || string.IsNullOrEmpty(LeftFieldName) || string.IsNullOrEmpty(RightFieldName))
                {
                    throw new Exception("Class or fields not provided/found.  Please ensure your macro is set up properly.");
                }

                string WhereInValue = "";
                string TableName = ClassObj.ClassTableName;
                int Count = 0;
                switch (Identity)
                {
                    case IdentityType.ID:
                        IEnumerable<int> CategoryIDs = CategoryIdentitiesToIDs(Values);
                        WhereInValue = string.Join(",", CategoryIDs);
                        Count = CategoryIDs.Count();
                        break;
                    case IdentityType.Guid:
                        IEnumerable<Guid> CategoryGUIDs = CategoryIdentitiesToGUIDs(Values);
                        WhereInValue = "'" + string.Join("','", CategoryGUIDs) + "'";
                        Count = CategoryGUIDs.Count();
                        break;
                    case IdentityType.CodeName:
                        IEnumerable<string> CategoryCodeNames = CategoryIdentitiesToCodeNames(Values);
                        WhereInValue = "'" + string.Join("','", CategoryCodeNames) + "'";
                        Count = CategoryCodeNames.Count();
                        break;
                }
                if (Count == 0)
                {
                    return "(1=1)";
                }
                switch (Condition)
                {
                    case ConditionType.Any:
                    default:
                        return string.Format("({0} in (Select {1} from {2} where {3} in ({4})))", ObjectIDFieldName, LeftFieldName, TableName, RightFieldName, WhereInValue);
                    case ConditionType.All:
                        return string.Format("(Select Count(*) from {0} where {0}.{1} = {2}{3} and {4} in ({5})) = {6}", TableName, LeftFieldName, (!string.IsNullOrWhiteSpace(ObjectIDTableName) ? ObjectIDTableName + "." : ""), ObjectIDFieldName, RightFieldName, WhereInValue, Count);
                    case ConditionType.None:
                        return string.Format("({0} not in (Select {1} from {2} where {3} in ({4})))", ObjectIDFieldName, LeftFieldName, TableName, RightFieldName, WhereInValue);
                }
            }, new CacheSettings((CacheWhere ? CacheHelper.CacheMinutes(SiteContext.CurrentSiteName) : 0), "GetBindingCategoryWhere", BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, string.Join("|", Values), Identity, Condition, ObjectIDTableName));
        }

        /// <summary>
        /// Returns a full where condition (for Binding Tables that bind on any object) to be used in filtering (ex repeaters).  For property exampples, we will assume Demo.Foo, Demo.Bar, and Demo.FooBar
        /// </summary>
        /// <param name="BindingClass">The Binding Class Code Name.  Ex: Demo.FooBar</param>
        /// <param name="ObjectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Bar</param>
        /// <param name="ObjectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID (from Demo.Foo)</param>
        /// <param name="LeftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: FooID (from Demo.FooBar)</param>
        /// <param name="RightFieldName">The Field Name of the binding class that contains the related objects's identy value.  Ex: BarID (from Demo.FooBar)</param>
        /// <param name="Values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="Identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="Condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="ObjectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBar</param>
        /// <returns>The Where Condition, If no object values provided or none found, returns 1=1</returns>
        public static string GetBindingWhere(string BindingClass, string ObjectClass, string ObjectIDFieldName, string LeftFieldName, string RightFieldName, IEnumerable<string> Values, IdentityType Identity = IdentityType.ID, ConditionType Condition = ConditionType.Any, string ObjectIDTableName = null)
        {
            LeftFieldName = GetBracketedColumnName(LeftFieldName);
            RightFieldName = GetBracketedColumnName(RightFieldName);
            ObjectIDFieldName = GetBracketedColumnName(ObjectIDFieldName);
            bool CacheWhere = SettingsKeyInfoProvider.GetBoolValue("CacheRelationshipWhereGeneration", new SiteInfoIdentifier(SiteContext.CurrentSiteID));
            return CacheHelper.Cache<string>(cs =>
            {
                // Find class table name
                DataClassInfo ClassObj = DataClassInfoProvider.GetDataClassInfo(BindingClass);
                ClassObjSummary classObjSummary = GetClassObjSummary(ObjectClass);

                string WhereInValue = "";
                string TableName = ClassObj.ClassTableName;
                int Count = 0;
                switch (Identity)
                {
                    case IdentityType.ID:
                        IEnumerable<int> ObjectIDs = ObjectIdentitiesToIDs(classObjSummary, Values);
                        WhereInValue = (ObjectIDs.Count() > 0 ? string.Join(",", ObjectIDs) : "''");
                        Count = ObjectIDs.Count();
                        break;
                    case IdentityType.Guid:
                        IEnumerable<Guid> ObjectGUIDs = ObjectIdentitiesToGUIDs(classObjSummary, Values);
                        WhereInValue = "'" + string.Join("','", ObjectGUIDs) + "'";
                        Count = ObjectGUIDs.Count();
                        break;
                    case IdentityType.CodeName:
                        IEnumerable<string> ObjectCodeNames = ObjectIdentitiesToCodeNames(classObjSummary, Values);
                        WhereInValue = "'" + string.Join("','", ObjectCodeNames) + "'";
                        Count = ObjectCodeNames.Count();
                        break;
                }

                // If no related object IDs found, then completely ignore.
                if (Count == 0)
                {
                    return "(1=1)";
                }

                switch (Condition)
                {
                    case ConditionType.Any:
                    default:
                        return string.Format("({0} in (Select {1} from {2} where {3} in ({4})))", ObjectIDFieldName, LeftFieldName, TableName, RightFieldName, WhereInValue);
                    case ConditionType.All:
                        return string.Format("(Select Count(*) from {0} where {0}.{1} = {2}{3} and {4} in ({5})) = {6}", TableName, LeftFieldName, (!string.IsNullOrWhiteSpace(ObjectIDTableName) ? ObjectIDTableName + "." : ""), ObjectIDFieldName, RightFieldName, WhereInValue, Count);
                    case ConditionType.None:
                        return string.Format("({0} not in (Select {1} from {2} where {3} in ({4})))", ObjectIDFieldName, LeftFieldName, TableName, RightFieldName, WhereInValue);
                }
            }, new CacheSettings((CacheWhere ? CacheHelper.CacheMinutes(SiteContext.CurrentSiteName) : 0), "GetBindingWhere", BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, string.Join("|", Values), Identity, Condition, ObjectIDTableName));
        }

        /// <summary>
        /// Checks for the Application Key for the given Guid which if present will indicate that a task needs to be generated for additional servers
        /// </summary>
        /// <param name="nodeGUID">The Node Guid</param>
        /// <param name="TaskEnumToUse">The task type that should be processed</param>
        public static void CheckIfTaskCreationShouldOccur(Guid nodeGUID, TaskTypeEnum TaskEnumToUse = TaskTypeEnum.UpdateDocument)
        {
            try
            {
                // Check to see if there are any staging servers that are different from originator and log staging task
                if (CallContext.GetData("UpdateAfterProcesses_" + nodeGUID) != null)
                {

                    TreeNode Node = new DocumentQuery().WhereEquals("NodeGUID", nodeGUID).FirstOrDefault();
                    // Destroy any delete task items
                    if (CallContext.GetData("DeleteTasks") != null)
                    {
                        ((List<int>)CallContext.GetData("DeleteTasks")).Remove(Node.NodeID);
                    }
                    string[] ServersToSendTo = (string[])CallContext.GetData("UpdateAfterProcesses_" + nodeGUID);

                    // Now set this to null so the task doesn't cause a loop
                    CallContext.SetData("UpdateAfterProcesses_" + nodeGUID, null);

                    // Add a catcher so it doesn't cause a loop in thinking this event is from another source and trigger the same logic of this thing.
                    CallContext.SetData("UpdateAfterProcessesProcessed_" + nodeGUID, true);

                    foreach (ServerInfo Server in ServerInfoProvider.GetServers().WhereEquals("ServerSiteID", Node.NodeSiteID).WhereIn("ServerName", ServersToSendTo))
                    {
                        DocumentSynchronizationHelper.LogDocumentChange(new LogMultipleDocumentChangeSettings()
                        {
                            NodeAliasPath = Node.NodeAliasPath,
                            CultureCode = Node.DocumentCulture,
                            TaskType = TaskEnumToUse,
                            Tree = Node.TreeProvider,
                            SiteName = Node.NodeSiteName,
                            RunAsynchronously = false,
                            User = MembershipContext.AuthenticatedUser,
                            ServerID = Server.ServerID
                        });
                    }
                    CallContext.SetData("UpdateAfterProcessesProcessed_" + nodeGUID, null);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("RelHelper", "CheckIfTaskCreationShouldOccurr", ex);
            }
        }

        #endregion

        #region "Staging Task Helpers"

        /// <summary>
        /// Returns true if Staging is eanbled for the current request, this uses the LicenseHelper.CurrentEdition (Ultimate or EMS = Staging enabled)
        /// </summary>
        /// <param name="SiteID">The SiteID of the task, if the SiteContext.CurrentSite is null, it will use this site</param>
        /// <returns>True if staging is enabled</returns>
        public static bool IsStagingEnabled(int SiteID = -1)
        {
            try
            {
                // Various checkts to make sure we have the SiteInfo, it is missing in context randomly it seems so this should allow us to do multiple checks with a fallback
                // of just the first site, since rare that a multisite has staging on some but not all.
                if (SiteContext.CurrentSite == null)
                {
                    SiteInfo Site = SiteInfoProvider.GetSiteInfo(SiteID);
                    SiteContext.CurrentSite = Site ?? SiteInfoProvider.GetSites().FirstOrDefault();
                }
                if (LicenseHelper.CheckFeature(SiteContext.CurrentSite.DomainName, FeatureEnum.Staging))
                {
                    return true;
                }
                else
                {
                    //EventLogProvider.LogEvent("W", "RelHelper", "LicenseBase", eventDescription: "Site: " + SiteContext.CurrentSiteName + ", License " + LicenseHelper.CurrentEdition + ", feature: " + LicenseHelper.CheckFeature(SiteContext.CurrentSite.DomainName, FeatureEnum.Staging).ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("RelationshipsExtended", "CannotDetectStagingEnabled", ex, additionalMessage: "Could not detect the LicenseHelper.CurrentEdition to see if Staging is enabled, related staging tasks will not run. Please contact tfayas@hbs.net if you see this error.");
                return false;
            }
        }

        /// <summary>
        /// Called on the Document.LogTask.Before, updates the Task's TaskData to include the provided Node-Bound objects
        /// </summary>
        /// <param name="e">The StagingLogTaskEventArgs from the Document.LogTask.Before</param>
        /// <param name="Configurations">The Configurations, one for each object binding you are including.</param>
        public static void UpdateTaskDataWithNodeBinding(StagingLogTaskEventArgs e, NodeBinding_DocumentLogTaskBefore_Configuration[] Configurations)
        {
            //EventLogProvider.LogEvent("W", "RelHelper", "UpdateTask1", eventDescription: "NodeID: " + e.Task.TaskNodeID);

            //EventLogProvider.LogEvent("W", "RelHelper", "UpdateTask2", eventDescription: "NodeID: " + e.Task.TaskNodeID);
            if (ValidationHelper.GetInteger(e.Task.TaskDocumentID, 0) > 1 && (e.Task.TaskType == TaskTypeEnum.UpdateDocument || e.Task.TaskType == TaskTypeEnum.CreateDocument))
            {
                //EventLogProvider.LogEvent("W", "RelHelper", "UpdateTask3", eventDescription: "NodeID: " + e.Task.TaskNodeID);
                TreeNode Node = new DocumentQuery().WhereEquals("DocumentID", e.Task.TaskDocumentID).FirstOrDefault();
                if (IsStagingEnabled(Node.NodeSiteID))
                {

                    // Get all staging servers
                    string[] StagingServers = CacheHelper.Cache<string[]>(cs =>
                    {
                        if (cs.Cached)
                        {
                            cs.CacheDependency = CacheHelper.GetCacheDependency("staging.server|all");
                        }
                        return ServerInfoProvider.GetServers().WhereEquals("ServerSiteID", Node.NodeSiteID).Select(x => x.ServerName.ToLower()).ToArray();
                    }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSite.SiteName), "GetServers"));


                    // This check will detect if this event was generated from another server and the task is being created for 
                    // another server besides it's origin.  In these cases the task should be removed and will be triggered once
                    // all of the Processesing is done of the binding items.
                    string[] TaskServers = e.Task.TaskServers.ToLower().Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    try
                    {
                        if (TaskServers.Length > 1 && StagingServers.Except(TaskServers).Count() > 0 && CallContext.GetData("UpdateAfterProcessesProcessed_" + Node.NodeGUID) == null)
                        {
                            CallContext.SetData("UpdateAfterProcesses_" + Node.NodeGUID, StagingServers.Except(TaskServers).ToArray());
                            if (CallContext.GetData("DeleteTasks") == null)
                            {
                                CallContext.SetData("DeleteTasks", new List<int>());
                            }
                            ((List<int>)CallContext.GetData("DeleteTasks")).Add(Node.NodeID);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogProvider.LogException("RelHelper", "CheckIfTaskCreationShouldOccurr", ex);
                    }

                    // The Task Data is an XML version of a DataSet, so convert to DataSet, then we can add our table data.
                    string DataSetXML = e.Task.TaskData;
                    DataSet DocumentDataSet = new DataSet();
                    DocumentDataSet.ReadXml(new StringReader(DataSetXML));

                    foreach (NodeBinding_DocumentLogTaskBefore_Configuration Configuration in Configurations)
                    {
                        TranslationHelper NodeBoundObjectTableHelper = new TranslationHelper();
                        DataSet NodeBoundObjectData = SynchronizationHelper.GetObjectsData(OperationTypeEnum.Synchronization, Configuration.EmptyNodeBindingObj, string.Format(Configuration.NodeMatchStringFormat, Node.NodeID), null, true, false, NodeBoundObjectTableHelper);
                        //EventLogProvider.LogEvent("W", "RelHelper", "UpdateTask3.5", eventDescription: "NodeID: " + e.Task.TaskNodeID);

                        if (NodeBoundObjectTableHelper.TranslationTable != null && NodeBoundObjectTableHelper.TranslationTable.Rows.Count > 0)
                        {
                            //EventLogProvider.LogEvent("W", "RelHelper", "UpdateTask4", eventDescription: "NodeID: " + e.Task.TaskNodeID);
                            NodeBoundObjectData.Tables.Add(NodeBoundObjectTableHelper.TranslationTable);
                        }

                        // Convert to XML and Back, this makes the Columns all type string so the transfer table works
                        DataSet NodeRegionObjectDataHolder = new DataSet();
                        NodeRegionObjectDataHolder.ReadXml(new StringReader(NodeBoundObjectData.GetXml()));
                        if (!DataHelper.DataSourceIsEmpty(NodeRegionObjectDataHolder) && NodeRegionObjectDataHolder.Tables.Count > 0)
                        {
                            //EventLogProvider.LogEvent("W", "RelHelper", "UpdateTask5", eventDescription: "NodeID: " + e.Task.TaskNodeID);
                            DataHelper.TransferTables(DocumentDataSet, NodeRegionObjectDataHolder);
                        }
                    }
                    //EventLogProvider.LogEvent("W", "RelHelper", "UpdateTask6", eventDescription: "NodeID: " + e.Task.TaskNodeID);
                    // Convert it back to XML
                    DataSetXML = DocumentDataSet.GetXml();
                    e.Task.TaskData = DataSetXML;
                }
            }
        }

        /// <summary>
        /// Handles the NodeBinding's TypeInfo.Events.Insert, Update, and Delete events (update only needed for binding objects with more than just the NodeID and other ObjectRef, such as Binding with Ordering)
        /// </summary>
        /// <param name="NodeID">The NodeID that the Bound Object is referencing</param>
        /// <param name="BindingObjectClassName">The Bound Object's Class Name</param>
        /// <param name="TaskOriginatorUserID">The User ID who originated the Insert/Update/Delete event, used in Task assignment.  If unset, will use the Current Authenticated User</param>
        /// <param name="TimerMS">Delay between when the last Event triggers and when the Log Document staging task is generated (so if you add 100 bound objects, 100 log document updates aren't triggered).  Default is 2000 ms (2 seconds)</param>
        public static void HandleNodeBindingInsertUpdateDeleteEvent(int NodeID, string BindingObjectClassName, int TaskOriginatorUserID = -1, int TimerMS = 2000)
        {
            // If synchronization is off, don't continue
            if (!CMSActionContext.CurrentLogSynchronization)
            {
                return;
            }
            // If no ID given, use the Current User, when the Log Document Task is called it's in a separate thread so there is no Membership Context
            if (TaskOriginatorUserID <= 0)
            {
                TaskOriginatorUserID = MembershipContext.AuthenticatedUser.UserID;
            }
            SiteInfo Site = SiteContext.CurrentSite;
            // Trigger update on document, uses a Timer so if you insert multiple items, it will simply reset the timer so there will only be 1 or so calls total.
            TreeNode Node = CacheHelper.Cache<TreeNode>(cs =>
            {
                TreeNode FoundNode = new DocumentQuery().WhereEquals("NodeID", NodeID).FirstOrDefault();
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency("NodeID|" + FoundNode.NodeID);
                }
                return FoundNode;
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "HandleBindingInsertUpdateDeleteEvent", NodeID, BindingObjectClassName));

            if (IsStagingEnabled(Node.NodeSiteID))
            {
                string TimerKey = string.Format("{0}Trigger_{1}", BindingObjectClassName, NodeID);
                bool CreateTimer = CallContext.GetData(TimerKey) == null;
                if (!CreateTimer)
                {
                    try
                    {
                        // Reset the existing timer
                        Timer TheTimer = (Timer)CallContext.GetData(TimerKey);
                        TheTimer.Stop();
                        TheTimer.Start();
                    }
                    catch (ObjectDisposedException)
                    {
                        // It was disposed mid way, recrate
                        CreateTimer = true;
                    }
                }
                if (CreateTimer)
                {
                    // Create a new Timer, if it finishes, then log document change
                    var TheTimer = new Timer(TimerMS)
                    {
                        AutoReset = false
                    };
                    TheTimer.Elapsed += (object TimerObj, ElapsedEventArgs ElapsedEvent) =>
                    {
                        ((Timer)TimerObj).Dispose();

                        // Reset the Membership Context and Site so the task is logged by the user who initiated it and what license is available
                        SiteContext.CurrentSite = Site;
                        MembershipContext.AuthenticatedUser = CacheHelper.Cache<CurrentUserInfo>(cs =>
                        {
                            UserInfo User = UserInfoProvider.GetUserInfo(TaskOriginatorUserID);
                            CurrentUserInfo CurrentUserObj = null;
                            if (User != null)
                            {
                                CurrentUserObj = new CurrentUserInfo(UserInfoProvider.GetUserInfo(TaskOriginatorUserID), true);
                            }
                            else
                            {
                                CurrentUserObj = new CurrentUserInfo(MembershipContext.AuthenticatedUser, true);
                            }
                            if (cs.Cached)
                            {
                                cs.CacheDependency = CacheHelper.GetCacheDependency("cms.user|byid|" + CurrentUserObj.UserID);
                            }
                            return CurrentUserObj;
                        }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "HandleNodeBindingInsertUpdateDeleteEvent_User", TaskOriginatorUserID));
                        DocumentSynchronizationHelper.LogDocumentChange(new LogMultipleDocumentChangeSettings()
                        {
                            NodeAliasPath = Node.NodeAliasPath,
                            CultureCode = Node.DocumentCulture,
                            TaskType = TaskTypeEnum.UpdateDocument,
                            Tree = Node.TreeProvider,
                            SiteName = Node.NodeSiteName,
                            RunAsynchronously = false,
                            User = MembershipContext.AuthenticatedUser
                        });
                    };
                    TheTimer.Start();
                    CallContext.SetData(TimerKey, TheTimer);
                }
            }
        }

        /// <summary>
        /// Handles the Translation of the Bound ObjectIDs and returns a list of all the ObjectIDs that the Node should be bound to.
        /// </summary>
        /// <param name="e">The StagingSynchronizationEventArgs</param>
        /// <param name="NodeBindingObjectClassName">The ClassName of the Node-Binding object (ex "cms.TreeCategory"</param>
        /// <param name="NodeIDReferenceField">The NodeID reference field for your Node-Binding object (ex "NodeID")</param>
        /// <param name="BoundObjectIDReferenceField">The Bound ObjectID reference field for your Node-Binding Object (ex "CategoryID")</param>
        /// <param name="BoundObjectTypeInfo">The TypeInfo of the object that the Node is bound to (ex new CategoryInfo().TypeInfo)</param>
        /// <returns>The List of all the New Object IDs that the node should be bound to.</returns>
        public static List<int> NewBoundObjectIDs(StagingSynchronizationEventArgs e, string NodeBindingObjectClassName, string NodeIDReferenceField, string BoundObjectIDReferenceField, ObjectTypeInfo BoundObjectTypeInfo)
        {
            string NodeBindingObjectClassNameTableName = NodeBindingObjectClassName.ToLower().Replace(".", "_");
            string BoundObjectClassNameTableName = BoundObjectTypeInfo.ObjectType.ToLower().Replace(".", "_");
            // Get the Tree Category table.
            DataTable NodeBoundTable = e.TaskData.Tables.Cast<DataTable>().Where(x => x.TableName.ToLower() == NodeBindingObjectClassNameTableName).FirstOrDefault();

            // Node has no Bindings, return empty list.
            if (NodeBoundTable == null)
            {
                return new List<int>();
            }

            // Build translation from Category data
            List<int> NewBoundIDs = new List<int>();
            List<int> TaskBoundIDs = NodeBoundTable.Rows.Cast<DataRow>().Select(x => ValidationHelper.GetInteger(x[BoundObjectIDReferenceField], 0)).ToList();

            TaskBoundIDs.RemoveAll(x => x <= 0);

            // Go through the Bound object tables which we'll use to gather the fields to translate the IDs from old env to new.
            foreach (DataTable BoundObjectTable in e.TaskData.Tables.Cast<DataTable>().Where(x => x.TableName.ToLower() == BoundObjectClassNameTableName))
            {
                bool ContainsGuidColumn = BoundObjectTable.Columns.Contains(BoundObjectTypeInfo.GUIDColumn);
                bool ContainsCodeNameColumn = BoundObjectTable.Columns.Contains(BoundObjectTypeInfo.CodeNameColumn);
                bool ContainsSiteIDColumn = BoundObjectTable.Columns.Contains(BoundObjectTypeInfo.SiteIDColumn);
                foreach (DataRow BoundObjectDR in BoundObjectTable.Rows)
                {
                    int ObjectID = ValidationHelper.GetInteger(BoundObjectDR[BoundObjectTypeInfo.IDColumn], 0);
                    if (TaskBoundIDs.Contains(ObjectID))
                    {
                        GetIDParameters ObjectParams = new GetIDParameters();
                        if (ContainsGuidColumn && !string.IsNullOrWhiteSpace(BoundObjectTypeInfo.GUIDColumn.Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "")))
                        {
                            ObjectParams.Guid = ValidationHelper.GetGuid(BoundObjectDR[BoundObjectTypeInfo.GUIDColumn], Guid.Empty);
                        }
                        if (ContainsCodeNameColumn && !string.IsNullOrWhiteSpace(BoundObjectTypeInfo.CodeColumn.Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "")))
                        {
                            ObjectParams.CodeName = ValidationHelper.GetString(BoundObjectDR[BoundObjectTypeInfo.CodeColumn], "");
                        }
                        if (ContainsSiteIDColumn && !string.IsNullOrWhiteSpace(BoundObjectTypeInfo.SiteIDColumn.Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "")))
                        {
                            int SiteID = ValidationHelper.GetInteger(BoundObjectDR[BoundObjectTypeInfo.SiteIDColumn], -1);
                            if (SiteID > 0)
                            {
                                ObjectParams.SiteId = SiteID;
                            }
                        }
                        try
                        {
                            int NewID = TranslationHelper.GetIDFromDB(ObjectParams, BoundObjectTypeInfo.ObjectType);
                            if (NewID > 0)
                            {
                                NewBoundIDs.Add(NewID);
                            }
                        }
                        catch (Exception ex)
                        {
                            EventLogProvider.LogException("RelationshipExended", "No Bound Object Found", ex, additionalMessage: string.Format("No Bound object of type [{0}] could be found in the new system that matched the incoming [{1}].", BoundObjectTypeInfo.ObjectType, NodeBindingObjectClassName));
                        }
                    }
                }
            }
            // Also check ObjectTranslations
            foreach (DataTable ObjectTranslationTable in e.TaskData.Tables.Cast<DataTable>().Where(x => x.TableName.ToLower() == "objecttranslation" && x.Columns.Contains("ClassName")))
            {
                foreach (DataRow ObjectTranslationDR in ObjectTranslationTable.Rows.Cast<DataRow>().Where(x => ((string)x["ClassName"]).ToLower() == BoundObjectClassNameTableName))
                {
                    int ID = ValidationHelper.GetInteger(ObjectTranslationDR["ID"], 0);
                    if (TaskBoundIDs.Contains(ID))
                    {
                        GetIDParameters GetIDParams = new GetIDParameters();
                        Guid GuidVal = ValidationHelper.GetGuid(ObjectTranslationDR["GUID"], Guid.Empty);
                        string CodeName = ValidationHelper.GetString(ObjectTranslationDR["CodeName"], "");
                        string SiteName = ValidationHelper.GetString(ObjectTranslationDR["SiteName"], "");
                        if (GuidVal != Guid.Empty)
                        {
                            GetIDParams.Guid = GuidVal;
                        }
                        if (!string.IsNullOrWhiteSpace(CodeName))
                        {
                            GetIDParams.CodeName = CodeName;
                        }
                        if (!string.IsNullOrWhiteSpace(SiteName))
                        {
                            GetIDParams.SiteId = CacheHelper.Cache<int>(cs =>
                            {
                                return SiteInfoProvider.GetSiteID(SiteName);
                            }, new CacheSettings(CacheHelper.CacheMinutes(SiteName), "NewBoundObjectIDsSiteName", SiteName));
                        }
                        try
                        {
                            int NewID = TranslationHelper.GetIDFromDB(GetIDParams, BoundObjectTypeInfo.ObjectType);
                            if (NewID > 0)
                            {
                                NewBoundIDs.Add(NewID);
                            }
                        }
                        catch (Exception ex)
                        {
                            EventLogProvider.LogException("RelationshipExended", "No Bound Object Found", ex, additionalMessage: string.Format("No Bound object of type [{0}] could be found in the new system that matched the incoming [{1}].", BoundObjectTypeInfo.ObjectType, NodeBindingObjectClassName));
                        }
                    }
                }
            }
            return NewBoundIDs;
        }

        /// <summary>
        /// Handles the Translation of the Bound ObjectIDs and returns an ordered list of all the ObjectIDs that the Node should be bound to.  Once Additions/Deletions made, then bound object's order should be set to the order in the list.
        /// </summary>
        /// <param name="e">The StagingSynchronizationEventArgs</param>
        /// <param name="NodeBindingObjectClassName">The ClassName of the Node-Binding object (ex "cms.TreeCategory"</param>
        /// <param name="NodeIDReferenceField">The NodeID reference field for your Node-Binding object (ex "NodeID")</param>
        /// <param name="BoundObjectIDReferenceField">The Bound ObjectID reference field for your Node-Binding Object (ex "CategoryID")</param>
        /// <param name="BoundObjectOrderField">The Bound Object's Order Field (ex "NodeCategoryOrder")</param>
        /// <param name="BoundObjectTypeInfo">The TypeInfo of the object that the Node is bound to (ex new CategoryInfo().TypeInfo)</param>
        /// <returns>The List of all the New Object IDs that the node should be bound to.</returns>
        public static List<int> NewOrderedBoundObjectIDs(StagingSynchronizationEventArgs e, string NodeBindingObjectClassName, string NodeIDReferenceField, string BoundObjectIDReferenceField, string BoundObjectOrderField, ObjectTypeInfo BoundObjectTypeInfo)
        {
            string NodeBindingObjectClassNameTableName = NodeBindingObjectClassName.ToLower().Replace(".", "_");
            string BoundObjectClassNameTableName = BoundObjectTypeInfo.ObjectType.ToLower().Replace(".", "_");
            // Get the Tree Category table.
            DataTable NodeBoundTable = e.TaskData.Tables.Cast<DataTable>().Where(x => x.TableName.ToLower() == NodeBindingObjectClassNameTableName).FirstOrDefault();

            // Node has no Bindings, return empty list.
            if (NodeBoundTable == null)
            {
                return new List<int>();
            }

            // Build translation from Category data
            List<int> NewBoundIDs = new List<int>();
            List<int> TaskBoundIDs = new List<int>();
            Dictionary<int, int> OldBoundIDToOrder = new Dictionary<int, int>();
            Dictionary<int, int> NewBoundIDToOrder = new Dictionary<int, int>();
            foreach (DataRow NodeBoundDR in NodeBoundTable.Rows)
            {
                NodeBoundTable.Rows.Cast<DataRow>().Select(x => ValidationHelper.GetInteger(x[BoundObjectIDReferenceField], 0)).ToList();
                int BoundObjectID = ValidationHelper.GetInteger(NodeBoundDR[BoundObjectIDReferenceField], 0);
                if (BoundObjectID > 0)
                {
                    TaskBoundIDs.Add(BoundObjectID);
                    if (!OldBoundIDToOrder.ContainsKey(BoundObjectID))
                    {
                        OldBoundIDToOrder.Add(BoundObjectID, ValidationHelper.GetInteger(NodeBoundDR[BoundObjectOrderField], 1));
                    }
                }
            }

            TaskBoundIDs = TaskBoundIDs.Distinct().ToList();

            // Go through the Bound object tables which we'll use to gather the fields to translate the IDs from old env to new.
            foreach (DataTable BoundObjectTable in e.TaskData.Tables.Cast<DataTable>().Where(x => x.TableName.ToLower() == BoundObjectClassNameTableName))
            {
                bool ContainsGuidColumn = BoundObjectTable.Columns.Contains(BoundObjectTypeInfo.GUIDColumn);
                bool ContainsCodeNameColumn = BoundObjectTable.Columns.Contains(BoundObjectTypeInfo.CodeNameColumn);
                bool ContainsSiteIDColumn = BoundObjectTable.Columns.Contains(BoundObjectTypeInfo.SiteIDColumn);
                foreach (DataRow BoundObjectDR in BoundObjectTable.Rows)
                {
                    int ObjectID = ValidationHelper.GetInteger(BoundObjectDR[BoundObjectTypeInfo.IDColumn], 0);
                    if (TaskBoundIDs.Contains(ObjectID))
                    {
                        GetIDParameters ObjectParams = new GetIDParameters();
                        if (ContainsGuidColumn && !string.IsNullOrWhiteSpace(BoundObjectTypeInfo.GUIDColumn.Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "")))
                        {
                            ObjectParams.Guid = ValidationHelper.GetGuid(BoundObjectDR[BoundObjectTypeInfo.GUIDColumn], Guid.Empty);
                        }
                        if (ContainsCodeNameColumn && !string.IsNullOrWhiteSpace(BoundObjectTypeInfo.CodeColumn.Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "")))
                        {
                            ObjectParams.CodeName = ValidationHelper.GetString(BoundObjectDR[BoundObjectTypeInfo.CodeColumn], "");
                        }
                        if (ContainsSiteIDColumn && !string.IsNullOrWhiteSpace(BoundObjectTypeInfo.SiteIDColumn.Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "")))
                        {
                            int SiteID = ValidationHelper.GetInteger(BoundObjectDR[BoundObjectTypeInfo.SiteIDColumn], -1);
                            if (SiteID > 0)
                            {
                                ObjectParams.SiteId = SiteID;
                            }
                        }
                        try
                        {
                            int NewID = TranslationHelper.GetIDFromDB(ObjectParams, BoundObjectTypeInfo.ObjectType);
                            if (NewID > 0)
                            {
                                NewBoundIDs.Add(NewID);
                                if (!NewBoundIDToOrder.ContainsKey(NewID))
                                {
                                    NewBoundIDToOrder.Add(NewID, OldBoundIDToOrder[ObjectID]);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EventLogProvider.LogException("RelationshipExended", "No Bound Object Found", ex, additionalMessage: string.Format("No Bound object of type [{0}] could be found in the new system that matched the incoming [{1}].", BoundObjectTypeInfo.ObjectType, NodeBindingObjectClassName));
                        }
                    }
                }
            }
            // Also check ObjectTranslations
            foreach (DataTable ObjectTranslationTable in e.TaskData.Tables.Cast<DataTable>().Where(x => x.TableName.ToLower() == "objecttranslation" && x.Columns.Contains("ClassName")))
            {
                foreach (DataRow ObjectTranslationDR in ObjectTranslationTable.Rows.Cast<DataRow>().Where(x => ((string)x["ClassName"]).ToLower() == BoundObjectClassNameTableName))
                {
                    int ID = ValidationHelper.GetInteger(ObjectTranslationDR["ID"], 0);
                    if (TaskBoundIDs.Contains(ID))
                    {
                        GetIDParameters GetIDParams = new GetIDParameters();
                        Guid GuidVal = ValidationHelper.GetGuid(ObjectTranslationDR["GUID"], Guid.Empty);
                        string CodeName = ValidationHelper.GetString(ObjectTranslationDR["CodeName"], "");
                        string SiteName = ValidationHelper.GetString(ObjectTranslationDR["SiteName"], "");
                        if (GuidVal != Guid.Empty)
                        {
                            GetIDParams.Guid = GuidVal;
                        }
                        if (!string.IsNullOrWhiteSpace(CodeName))
                        {
                            GetIDParams.CodeName = CodeName;
                        }
                        if (!string.IsNullOrWhiteSpace(SiteName))
                        {
                            GetIDParams.SiteId = CacheHelper.Cache<int>(cs =>
                            {
                                return SiteInfoProvider.GetSiteID(SiteName);
                            }, new CacheSettings(CacheHelper.CacheMinutes(SiteName), "NewBoundObjectIDsSiteName", SiteName));
                        }
                        try
                        {
                            int NewID = TranslationHelper.GetIDFromDB(GetIDParams, BoundObjectTypeInfo.ObjectType);
                            if (NewID > 0)
                            {
                                NewBoundIDs.Add(NewID);
                                if (!NewBoundIDToOrder.ContainsKey(NewID))
                                {
                                    NewBoundIDToOrder.Add(NewID, OldBoundIDToOrder[ID]);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EventLogProvider.LogException("RelationshipExended", "No Bound Object Found", ex, additionalMessage: string.Format("No Bound object of type [{0}] could be found in the new system that matched the incoming [{1}].", BoundObjectTypeInfo.ObjectType, NodeBindingObjectClassName));
                        }
                    }
                }
            }

            // Return the list ordered by the Proper Order
            return NewBoundIDs.OrderBy(x => NewBoundIDToOrder[x]).ToList();
        }

        /// <summary>
        /// Used to convert the less-than-readable staging task into a more readable format (ex: Adding Foo "Foo1" to Node "/Home/MyPage"). Use this on the StagingEvents.LogTask.Before event
        /// </summary>
        /// <param name="e">The Staging Log Task Event Args that is passed during the StagingEvents.LogTask.Before event</param>
        /// <param name="NodeBindingObjectType">The Node-Binding Class Name, ex: Demo.NodeFoo</param>
        /// <param name="NodeBindingNodeIDField">The Column name of the Binding class that contains the NodeID, ex: "NodeID"</param>
        /// <param name="NodeBindingObjectIDField">The column name of the Binding class that contains the other object's ID, ex: "FooID"</param>
        /// <param name="BoundObjectDescription">Description of what the Bound object is.  ex: "Foo" or "Banner"</param>
        /// <param name="BoundObjectTypeInfo">The TypeInfo of the Object that is bound to the Node, ex: FooInfo.TypeInfo</param>
        public static void SetBetterBindingTaskTitle(StagingLogTaskEventArgs e, string NodeBindingObjectType, string NodeBindingNodeIDField, string NodeBindingObjectIDField, string BoundObjectDescription, ObjectTypeInfo BoundObjectTypeInfo)
        {
            if (e.Task.TaskObjectType.ToLower() == NodeBindingObjectType.ToLower())
            {
                try
                {
                    // The Task Data is an XML version of a DataSet, so convert to DataSet, then we can add our table data.
                    string DataSetXML = e.Task.TaskData;
                    DataSet DocumentDataSet = new DataSet();
                    DocumentDataSet.ReadXml(new StringReader(DataSetXML));
                    DataTable BoundObjectTable = DocumentDataSet.Tables[0];

                    TreeNode Node = new DocumentQuery().WhereEquals("NodeID", ValidationHelper.GetInteger(BoundObjectTable.Rows[0][NodeBindingNodeIDField], -1)).Columns("NodeAliasPath").FirstOrDefault();

                    string ColumnToGet = BoundObjectTypeInfo.DisplayNameColumn != ObjectTypeInfo.COLUMN_NAME_UNKNOWN ? BoundObjectTypeInfo.DisplayNameColumn : "";
                    ColumnToGet = string.IsNullOrWhiteSpace(ColumnToGet) && BoundObjectTypeInfo.CodeNameColumn != ObjectTypeInfo.COLUMN_NAME_UNKNOWN ? BoundObjectTypeInfo.CodeNameColumn : ColumnToGet;
                    ColumnToGet = string.IsNullOrWhiteSpace(ColumnToGet) && BoundObjectTypeInfo.GUIDColumn != ObjectTypeInfo.COLUMN_NAME_UNKNOWN ? BoundObjectTypeInfo.GUIDColumn : ColumnToGet;
                    ColumnToGet = string.IsNullOrWhiteSpace(ColumnToGet) ? BoundObjectTypeInfo.IDColumn : ColumnToGet;

                    DataRow BoundObjectDR = ConnectionHelper.ExecuteQuery(BoundObjectTypeInfo.ObjectType + ".selectall", null, where: string.Format("{0} = {1}", BoundObjectTypeInfo.IDColumn, ValidationHelper.GetInteger(BoundObjectTable.Rows[0][NodeBindingObjectIDField], -1)), columns: ColumnToGet).Tables[0].Rows[0];
                    e.Task.TaskTitle = string.Format("{0} {1} \"{2}\" {3} Node \"{4}\"", (e.Task.TaskType == TaskTypeEnum.CreateObject ? "Adding" : "Removing"), BoundObjectDescription, BoundObjectDR["BoundObjectDR"].ToString(), (e.Task.TaskType == TaskTypeEnum.CreateObject ? "to" : "from"), Node.NodeAliasPath);
                }
                catch (Exception ex)
                {
                    EventLogProvider.LogException("RelationshipHelper", "SetBetterBindingTaskTitleError", ex, additionalMessage: string.Format("For NodeBindingObjectType {0} with Object Reference Field {1} and Node Field {2}", NodeBindingObjectType, NodeBindingObjectIDField, NodeBindingNodeIDField));
                }
            }
        }

        #endregion

        #region "Internal Helpers"

        /// <summary>
        /// Helper to take IDs coming in and translate them for the new system.
        /// </summary>
        /// <param name="ItemID">The original Item ID</param>
        /// <param name="TaskData">The Task's Data, must contain an ObjectTranslation table</param>
        /// <param name="classname">The Item's Classname</param>
        /// <returns>The proper ID value</returns>
        public static int TranslateBindingTranslateID(int ItemID, DataSet TaskData, string classname)
        {
            DataTable ObjectTranslationTable = TaskData.Tables.Cast<DataTable>().Where(x => x.TableName.ToLower() == "objecttranslation").FirstOrDefault();
            if (ObjectTranslationTable == null)
            {
                EventLogProvider.LogEvent("E", "RelHelper.TranslateBindingTranslateID", "NoObjectTranslationTable", "Could not find an ObjectTranslation table in the Task Data, please make sure to only call this with a task that has an ObjectTranslation table");
                return -1;
            }
            foreach (DataRow ItemDR in ObjectTranslationTable.Rows.Cast<DataRow>()
                .Where(x => ValidationHelper.GetString(x["ObjectType"], "").ToLower() == classname.ToLower() && ValidationHelper.GetInteger(x["ID"], -1) == ItemID))
            {
                int TranslationID = ValidationHelper.GetInteger(ItemDR["ID"], 0);
                if (ItemID == TranslationID)
                {
                    GetIDParameters ItemParams = new GetIDParameters();
                    if (ValidationHelper.GetGuid(ItemDR["GUID"], Guid.Empty) != Guid.Empty)
                    {
                        ItemParams.Guid = (Guid)ItemDR["GUID"];
                    }
                    if (!string.IsNullOrWhiteSpace(ValidationHelper.GetString(ItemDR["CodeName"], "")))
                    {
                        ItemParams.CodeName = (string)ItemDR["CodeName"];
                    }
                    if (ObjectTranslationTable.Columns.Contains("SiteName") && !string.IsNullOrWhiteSpace(ValidationHelper.GetString(ItemDR["SiteName"], "")))
                    {
                        int SiteID = SiteInfoProvider.GetSiteID((string)ItemDR["SiteName"]);
                        if (SiteID > 0)
                        {
                            ItemParams.SiteId = SiteID;
                        }
                    }
                    try
                    {
                        int NewID = TranslationHelper.GetIDFromDB(ItemParams, classname);
                        if (NewID > 0)
                        {
                            ItemID = NewID;
                        }
                    }
                    catch (Exception ex)
                    {
                        EventLogProvider.LogException("RelHelper.TranslateBindingTranslateID", "No Translation Found", ex, additionalMessage: "No Translation found.");
                        return -1;
                    }
                }
            }
            return ItemID;
        }

        /// <summary>
        /// Returns the proper Node ID, if the Node is a Linked Node, it will cycle through the Nodes it's lined to until it finds a Non-lined node.
        /// </summary>
        /// <param name="NodeID">The NodeID</param>
        /// <returns>The Non-Linked Node ID, -1 if it can't find the main Node</returns>
        public static int GetPrimaryNodeID(int NodeID)
        {
            return CacheHelper.Cache<int>(cs =>
            {
                TreeNode NodeObj = new DocumentQuery().WhereEquals("NodeID", NodeID).FirstObject;
                while (NodeObj != null && NodeObj.NodeLinkedNodeID > 0)
                {
                    NodeObj = new DocumentQuery().WhereEquals("NodeID", NodeObj.NodeLinkedNodeID).FirstObject;
                }
                int PrimaryNodeID = (NodeObj != null ? NodeObj.NodeID : -1);
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency(new string[] { "nodeid|" + NodeID, "nodeid|" + PrimaryNodeID });
                }
                return PrimaryNodeID;
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "GetPrimaryNodeID", NodeID));
        }

        /// <summary>
        /// Gets the Category identities where condition (ex (CategoryID in (1,2,3) )
        /// </summary>
        /// <param name="CategoryIdentifications">List of Ints, Guids, or CodeNames of the Categories</param>
        /// <returns>the Category identity where condition</returns>
        private static string CategoryIdentitiesWhere(IEnumerable<object> CategoryIdentifications)
        {
            List<Guid> Guids = new List<Guid>();
            List<int> Ints = new List<int>();
            List<string> Strings = new List<string>();

            foreach (object CategoryIdentification in CategoryIdentifications)
            {
                Guid GuidVal = ValidationHelper.GetGuid(CategoryIdentification, Guid.Empty);
                int IntVal = ValidationHelper.GetInteger(CategoryIdentification, -1);
                string StringVal = ValidationHelper.GetString(CategoryIdentification, "");
                if (GuidVal != Guid.Empty)
                {
                    Guids.Add(GuidVal);
                }
                else if (IntVal > 0)
                {
                    Ints.Add(IntVal);
                }
                else if (!string.IsNullOrWhiteSpace(StringVal))
                {
                    Strings.Add(SqlHelper.EscapeQuotes(StringVal));
                }
            }
            string WhereCondition = "";
            if (Guids.Count > 0)
            {
                WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, string.Format("(CategoryGUID in ('{0}'))", string.Join("','", Guids.Select(x => x.ToString()).ToArray())), "OR");
            }
            if (Ints.Count > 0)
            {
                WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, string.Format("(CategoryID in ('{0}'))", string.Join("','", Ints.Select(x => x.ToString()).ToArray())), "OR");
            }
            if (Strings.Count > 0)
            {
                WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, string.Format("(CategoryName in ('{0}') and (CategorySiteID is null or CategorySiteID = {1}))", string.Join("','", Strings.ToArray()), SiteContext.CurrentSiteID), "OR");
            }
            return (!string.IsNullOrWhiteSpace(WhereCondition) ? WhereCondition : "(1=0)");

        }

        /// <summary>
        /// Converts Category IDs, Guids, or CodeNames to CategoryIDs
        /// </summary>
        /// <param name="CategoryIdentifications">List of Category IDs, Guids, or CodeNames</param>
        /// <returns>List of Category IDs</returns>
        private static IEnumerable<int> CategoryIdentitiesToIDs(IEnumerable<object> CategoryIdentifications)
        {
            return CategoryInfoProvider.GetCategories().Where(CategoryIdentitiesWhere(CategoryIdentifications)).Columns("CategoryID").Select(x => x.CategoryID).ToArray();
        }

        /// <summary>
        /// Converts Category IDs, Guids, or CodeNames to CategoryGUIDs
        /// </summary>
        /// <param name="CategoryIdentifications">List of Category IDs, Guids, or CodeNames</param>
        /// <returns>List of Category GUIDs</returns>
        private static IEnumerable<Guid> CategoryIdentitiesToGUIDs(IEnumerable<object> CategoryIdentifications)
        {
            return CategoryInfoProvider.GetCategories().Where(CategoryIdentitiesWhere(CategoryIdentifications)).Columns("CategoryGUID").Select(x => x.CategoryGUID).ToArray();
        }

        /// <summary>
        /// Converts Category IDs, Guids, or CodeNames to Category CodeNames
        /// </summary>
        /// <param name="CategoryIdentifications">List of Category IDs, Guids, or CodeNames</param>
        /// <returns>List of Category Code Names</returns>
        private static IEnumerable<string> CategoryIdentitiesToCodeNames(IEnumerable<object> CategoryIdentifications)
        {
            return CategoryInfoProvider.GetCategories().Where(CategoryIdentitiesWhere(CategoryIdentifications)).Columns("CategoryName").Select(x => x.CategoryName).ToArray();
        }

        /// <summary>
        /// Gets a Class Object Summary based on the class name.
        /// </summary>
        /// <param name="ClassName">The Class Name</param>
        /// <returns>The Class Object Summary</returns>
        private static ClassObjSummary GetClassObjSummary(string ClassName)
        {
            return CacheHelper.Cache<ClassObjSummary>(cs =>
            {
                ClassObjSummary summaryObj = new ClassObjSummary(ClassName);
                DataClassInfo ClassObj = DataClassInfoProvider.GetDataClassInfo(ClassName);
                if (ClassObj != null)
                {
                    summaryObj.ClassIsCustomTable = ClassObj.ClassIsCustomTable;
                    summaryObj.ClassIsDocumentType = ClassObj.ClassIsDocumentType;
                    summaryObj.ClassIsForm = ClassObj.ClassIsForm;
                }
                else
                {
                    summaryObj.ClassIsCustomTable = false;
                    summaryObj.ClassIsDocumentType = false;
                    summaryObj.ClassIsForm = false;
                }
                // now get GUID and Code Name if possible.
                var ObjectClassFactoryObj = new InfoObjectFactory(ClassName);
                if (ObjectClassFactoryObj != null && ObjectClassFactoryObj.Singleton != null)
                {
                    ObjectTypeInfo typeInfoObj = ((BaseInfo)ObjectClassFactoryObj.Singleton).TypeInfo;
                    summaryObj.IDColumn = ValidationHelper.GetString(typeInfoObj.IDColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "");
                    summaryObj.GUIDColumn = ValidationHelper.GetString(typeInfoObj.GUIDColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "");
                    summaryObj.CodeNameColumn = ValidationHelper.GetString(typeInfoObj.CodeNameColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "");
                }
                else
                {
                    // handle unique cases
                    switch (ClassName.ToLower())
                    {
                        case "cms.tree":
                        case "cms.node":
                        case "cms.root":
                            summaryObj.IDColumn = "NodeID";
                            summaryObj.CodeNameColumn = "NodeAliasPath";
                            summaryObj.GUIDColumn = "NodeGUID";
                            break;
                        case "cms.document":
                            summaryObj.IDColumn = "DocumentID";
                            summaryObj.GUIDColumn = "DocumentGUID";
                            break;
                        case "om.contactgroupmember":
                            summaryObj.IDColumn = "ContactGroupMemberID";
                            break;
                        case "om.membership":
                            summaryObj.IDColumn = "MembershipID";
                            summaryObj.GUIDColumn = "MembershipGUID";
                            break;
                    }

                    // if still missing fields, try parsing XML
                    if (string.IsNullOrWhiteSpace(summaryObj.CodeNameColumn) || string.IsNullOrWhiteSpace(summaryObj.GUIDColumn) || string.IsNullOrWhiteSpace(summaryObj.IDColumn))
                    {
                        XmlDocument classXML = new XmlDocument();
                        classXML.LoadXml(ClassObj.ClassFormDefinition);
                        if (string.IsNullOrWhiteSpace(summaryObj.IDColumn))
                        {
                            try
                            {
                                summaryObj.IDColumn = classXML.SelectNodes("/form/field[@columntype='integer' and @isPK='true']").Item(0).Attributes["column"].Value;
                            }
                            catch (Exception)
                            {
                                // can't figure out that code name
                            }
                        }
                        if (string.IsNullOrWhiteSpace(summaryObj.CodeNameColumn))
                        {
                            try
                            {
                                summaryObj.CodeNameColumn = classXML.SelectNodes("/form/field[@columntype='text' and contains(@column, 'CodeName')]").Item(0).Attributes["column"].Value;
                            }
                            catch (Exception)
                            {
                                // can't figure out that code name
                            }
                        }
                        if (string.IsNullOrWhiteSpace(summaryObj.GUIDColumn))
                        {
                            try
                            {
                                summaryObj.GUIDColumn = classXML.SelectNodes("/form/field[@publicfield='false' and @columntype='guid' and system='true']").Item(0).Attributes["column"].Value;
                            }
                            catch (Exception)
                            {
                                // Can't figure out GUID
                            }
                        }
                    }
                }
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency("cms.class|byname|" + ClassName);
                }
                return summaryObj;
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "GetClassObjSummary", ClassName));
        }

        /// <summary>
        /// Converts an Object's IDs, Guids, or CodeNames to the Object IDs
        /// </summary>
        /// <param name="classObjSummary">The Class Object Summary</param>
        /// <param name="ObjectIdentifications">List of Object IDs, Guids, or CodeNames</param>
        /// <returns>A list of the Object's IDs</returns>
        private static IEnumerable<int> ObjectIdentitiesToIDs(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications)
        {

            switch (classObjSummary.ClassName.ToLower())
            {
                case "cms.tree":
                case "cms.document":
                    return new DocumentQuery().Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.IDColumn).Select(x => (int)x.GetValue(classObjSummary.IDColumn)).ToArray();
                default:
                    if (classObjSummary.ClassIsDocumentType)
                    {
                        return new DocumentQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.IDColumn).Select(x => (int)x.GetValue(classObjSummary.IDColumn)).ToArray();
                    }
                    else if (classObjSummary.ClassIsCustomTable)
                    {
                        return CustomTableItemProvider.GetItems(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.IDColumn).Select(x => (int)x.GetValue(classObjSummary.IDColumn)).ToArray();
                    }
                    else if (classObjSummary.ClassIsForm)
                    {
                        return BizFormItemProvider.GetItems(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.IDColumn).Select(x => (int)x.GetValue(classObjSummary.IDColumn)).ToArray();
                    }
                    else
                    {
                        return new ObjectQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.IDColumn).Select(x => (int)x.GetValue(classObjSummary.IDColumn)).ToArray();
                    }
            }
        }

        /// <summary>
        /// Converts an Object's IDs, Guids, or CodeNames to the Objects GUID
        /// </summary>
        /// <param name="classObjSummary">The Class Object Summary</param>
        /// <param name="ObjectIdentifications">List of Object IDs, Guids, or CodeNames</param>
        /// <returns>A list of the Object's GUIDs</returns>
        private static IEnumerable<Guid> ObjectIdentitiesToGUIDs(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications)
        {

            switch (classObjSummary.ClassName.ToLower())
            {
                case "cms.tree":
                case "cms.document":
                    return new DocumentQuery().Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.GUIDColumn).Select(x => (Guid)x.GetValue(classObjSummary.GUIDColumn)).ToArray();
                default:
                    if (classObjSummary.ClassIsDocumentType)
                    {
                        return new DocumentQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.GUIDColumn).Select(x => (Guid)x.GetValue(classObjSummary.GUIDColumn)).ToArray();
                    }
                    else if (classObjSummary.ClassIsCustomTable)
                    {
                        return CustomTableItemProvider.GetItems(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.GUIDColumn).Select(x => (Guid)x.GetValue(classObjSummary.GUIDColumn)).ToArray();
                    }
                    else if (classObjSummary.ClassIsForm)
                    {
                        return BizFormItemProvider.GetItems(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.GUIDColumn).Select(x => (Guid)x.GetValue(classObjSummary.GUIDColumn)).ToArray();
                    }
                    else
                    {
                        return new ObjectQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.GUIDColumn).Select(x => (Guid)x.GetValue(classObjSummary.GUIDColumn)).ToArray();
                    }
            }
        }

        /// <summary>
        /// Converts an Object's IDs, Guids, or CodeNames to the Objects CodeNames
        /// </summary>
        /// <param name="classObjSummary">The Class Object Summary</param>
        /// <param name="ObjectIdentifications">List of Object IDs, Guids, or CodeNames</param>
        /// <returns>A list of the Object's Code Names</returns>
        private static IEnumerable<string> ObjectIdentitiesToCodeNames(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications)
        {
            switch (classObjSummary.ClassName.ToLower())
            {
                case "cms.tree":
                case "cms.document":
                    return new DocumentQuery().Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.CodeNameColumn).Select(x => (string)x.GetValue(classObjSummary.CodeNameColumn)).ToArray();
                default:
                    if (classObjSummary.ClassIsDocumentType)
                    {
                        return new DocumentQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.CodeNameColumn).Select(x => (string)x.GetValue(classObjSummary.CodeNameColumn)).ToArray();
                    }
                    else if (classObjSummary.ClassIsCustomTable)
                    {
                        return CustomTableItemProvider.GetItems(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.CodeNameColumn).Select(x => (string)x.GetValue(classObjSummary.CodeNameColumn)).ToArray();
                    }
                    else if (classObjSummary.ClassIsForm)
                    {
                        return BizFormItemProvider.GetItems(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.CodeNameColumn).Select(x => (string)x.GetValue(classObjSummary.CodeNameColumn)).ToArray();
                    }
                    else
                    {
                        return new ObjectQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.CodeNameColumn).Select(x => (string)x.GetValue(classObjSummary.CodeNameColumn)).ToArray();
                    }
            }

        }

        /// <summary>
        /// Gets the Object WHERE condition based on the given identities
        /// </summary>
        /// <param name="classObjSummary">The Class Object Summary</param>
        /// <param name="ObjectIdentifications">List of IDs, Guids, or CodeNames</param>
        /// <returns>The WHERE condition to select the objects (ex MyObjectID in (1,2,3) )</returns>
        private static string ObjectIdentitiesWhere(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications)
        {
            List<Guid> Guids = new List<Guid>();
            List<int> Ints = new List<int>();
            List<string> Strings = new List<string>();

            foreach (object ObjectIdentification in ObjectIdentifications)
            {
                Guid GuidVal = ValidationHelper.GetGuid(ObjectIdentification, Guid.Empty);
                int IntVal = ValidationHelper.GetInteger(ObjectIdentification, -1);
                string StringVal = ValidationHelper.GetString(ObjectIdentification, "");
                if (GuidVal != Guid.Empty)
                {
                    Guids.Add(GuidVal);
                }
                else if (IntVal > 0)
                {
                    Ints.Add(IntVal);
                }
                else if (!string.IsNullOrWhiteSpace(StringVal))
                {
                    Strings.Add(SqlHelper.EscapeQuotes(StringVal));
                }
            }

            string WhereCondition = "";
            if (Guids.Count > 0 && !string.IsNullOrWhiteSpace(classObjSummary.GUIDColumn))
            {
                WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, string.Format("({0} in ('{1}'))", classObjSummary.GUIDColumn, string.Join("','", Guids.Select(x => x.ToString()).ToArray())), "OR");
            }
            if (Ints.Count > 0 && !string.IsNullOrWhiteSpace(classObjSummary.IDColumn))
            {
                WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, string.Format("({0} in ({1}))", classObjSummary.IDColumn, string.Join(",", Ints.Select(x => x.ToString()).ToArray())), "OR");
            }
            if (Strings.Count > 0 && !string.IsNullOrWhiteSpace(classObjSummary.CodeNameColumn))
            {
                WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, string.Format("({0} in ('{1}'))", classObjSummary.CodeNameColumn, string.Join("','", Strings.ToArray()), SiteContext.CurrentSiteID), "OR");
            }
            return (!string.IsNullOrWhiteSpace(WhereCondition) ? WhereCondition : "(1=0)");
        }

        /// <summary>
        /// Makes sure to wrap the field in []'s, along with handling full-pathed fields such as My_Table.MyField
        /// </summary>
        /// <param name="Field">The Field Name (ex MyField, or My_Table.MyField)</param>
        /// <returns>The properly formatted FieldName</returns>
        private static string GetBracketedColumnName(string Field)
        {
            string[] FieldSplit = Field.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < FieldSplit.Length; i++)
            {
                FieldSplit[i] = string.Format("[{0}]", FieldSplit[i].Trim("[]".ToCharArray()));
            }
            return string.Join(".", FieldSplit);
        }

        #endregion
    }

    /// <summary>
    /// Internal use only, creates a summary of a Class for processing
    /// </summary>
    public class ClassObjSummary
    {
        public string ClassName;
        public string TableName;
        public string IDColumn;
        public string GUIDColumn;
        public string CodeNameColumn;
        public bool ClassIsDocumentType;
        public bool ClassIsCustomTable;
        public bool ClassIsForm;

        public ClassObjSummary(string ClassName)
        {
            this.ClassName = ClassName;
        }
    }

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
