using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.OnlineForms;
using CMS.SiteProvider;
using CMS.Taxonomy;
using RelationshipsExtended.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace RelationshipsExtended
{
    /// <summary>
    /// Helper methods for generating Where Conditions for Relationships
    /// </summary>
    public static class RelHelper
    {

        #region "MVC Helpers"

        /// <summary>
        /// Gets the Current Site Name based on the request's Host (HttpContext.Current.Request.Host) and a match on the Presentation Url for the Sites Object in Kentico.  Returns an empty or null string if not found.
        /// </summary>
        public static string CurrentSiteName
        {
            get
            {
                return SiteContext.CurrentSiteName;
            }
        }

        /// <summary>
        /// Gets the Current Site ID based on the request's Host (HttpContext.Current.Request.Host) and a match on the Presentation Url for the Sites Object in Kentico.  -1 if not found.
        /// </summary>
        public static int CurrentSiteID
        {
            get
            {
                return SiteContext.CurrentSiteID;
            }
        }

        /// <summary>
        /// Cache minutes, tries to use the current site, otherwise if it can't find the current site then 30
        /// </summary>
        public static int CacheMinutes
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(CurrentSiteName))
                {
                    return CacheHelper.CacheMinutes(CurrentSiteName);
                }
                else
                {
                    return 30;
                }
            }
        }

        #endregion

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
            }, new CacheSettings(CacheMinutes, "GetDocumentCategoryWhere", string.Join("|", Values), Condition, DocumentIDTableName));
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
            }, new CacheSettings(CacheMinutes, "GetNodeCategoryWhere", string.Join("|", Values), Condition, NodeIDTableName));
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
            }, new CacheSettings(CacheMinutes, "GetBindingCategoryWhere", BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, string.Join("|", Values), Identity, Condition, ObjectIDTableName));
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
            }, new CacheSettings(CacheMinutes, "GetBindingWhere", BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, string.Join("|", Values), Identity, Condition, ObjectIDTableName));
        }

        #endregion

        #region "Internal Helpers"

        /// <summary>
        /// Returns the proper Node ID, if the Node is a Linked Node, it will cycle through the Nodes it's lined to until it finds a Non-lined node.
        /// </summary>
        /// <param name="NodeID">The NodeID</param>
        /// <returns>The Non-Linked Node ID, -1 if it can't find the main Node</returns>
        public static int GetPrimaryNodeID(int NodeID)
        {
            return CacheHelper.Cache<int>(cs =>
            {
                TreeNode NodeObj = new DocumentQuery().WhereEquals("NodeID", NodeID).FirstOrDefault();
                while (NodeObj != null && NodeObj.NodeLinkedNodeID > 0)
                {
                    NodeObj = new DocumentQuery().WhereEquals("NodeID", NodeObj.NodeLinkedNodeID).FirstOrDefault();
                }
                int PrimaryNodeID = (NodeObj != null ? NodeObj.NodeID : -1);
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency(new string[] { "nodeid|" + NodeID, "nodeid|" + PrimaryNodeID });
                }
                return PrimaryNodeID;
            }, new CacheSettings(CacheMinutes, "GetPrimaryNodeID", NodeID));
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
                WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, string.Format("(CategoryName in ('{0}') and (CategorySiteID is null or CategorySiteID = {1}))", string.Join("','", Strings.ToArray()), CurrentSiteID), "OR");
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
            }, new CacheSettings(CacheMinutes, "GetClassObjSummary", ClassName));
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
                WhereCondition = SqlHelper.AddWhereCondition(WhereCondition, string.Format("({0} in ('{1}'))", classObjSummary.CodeNameColumn, string.Join("','", Strings.ToArray()), CurrentSiteID), "OR");
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
}
