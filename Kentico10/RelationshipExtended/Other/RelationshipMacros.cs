using CMS;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.Relationships;
using RelationshipsExtended;
using System;
using CMS.DataEngine;
using CMS.SiteProvider;
using RelationshipsExtended.Enums;
using CMS.EventLog;

[assembly: RegisterExtension(typeof(RelationshipMacroMethods), typeof(RelationshipsExtendedMacroNamespace))]
[assembly: RegisterExtension(typeof(RelHelperMacrosMethods), typeof(RelHelperMacroNamespace))]
namespace RelationshipsExtended
{
    public class RelationshipMacroMethods : MacroMethodContainer
    {

        [MacroMethod(typeof(string), "Returns the URL to create a new page of the specified type at the specified location.", 2)]
        [MacroMethodParam(0, "ClassName", typeof(string), "The class name of the page type that will be created.")]
        [MacroMethodParam(1, "ParentNodeAlias", typeof(string), "The parent node alias that the page will be inserted at.")]
        [MacroMethodParam(2, "CurrentCulture", typeof(string), "The document culture, will default to en-US if not provided.")]
        public static object GetNewPageLink(EvaluationContext context, params object[] parameters)
        {
            try
            {
                if (parameters.Length >= 2)
                {
                    string ClassName = ValidationHelper.GetString(parameters[0], "");
                    string ParentNodeAlias = ValidationHelper.GetString(parameters[1], "");
                    string Culture = ValidationHelper.GetString(parameters.Length > 2 ? parameters[2] : "en-US", "en-US");
                    if (!string.IsNullOrWhiteSpace(ClassName) && !string.IsNullOrWhiteSpace(ParentNodeAlias))
                    {
                        return CacheHelper.Cache<string>(cs =>
                        {
                            int ClassID = DataClassInfoProvider.GetDataClassInfo(ClassName).ClassID;
                            int NodeID = new DocumentQuery().Path(ParentNodeAlias, PathTypeEnum.Single).FirstObject.NodeID;
                            return URLHelper.ResolveUrl(string.Format("~/CMSModules/Content/CMSDesk/Edit/Edit.aspx?action=new&classid={0}&parentnodeid={1}&parentculture={2}", ClassID, NodeID, Culture));
                        }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), ClassName, ParentNodeAlias, Culture));
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("RelationshipMacros", "GetNewPageLinkError", ex);
            }
            return "#";
        }

        [MacroMethod(typeof(bool), "Determines if the current relationship tab should be visible (if the current document is either on the left or right side of this relationship).", 0)]
        public static object RelationshipTabIsVisible(EvaluationContext context, params object[] parameters)
        {
            string LeftSideMacro = ValidationHelper.GetString(UIContext.Current.Data.GetValue("IsLeftSideMacro"), "");
            string RightSideMacro = ValidationHelper.GetString(UIContext.Current.Data.GetValue("IsRightSideMacro"), "");
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                var NewResolver = MacroContext.CurrentResolver.CreateChild();
                NewResolver.SetNamedSourceData("CurrentDocument", currentNode);
                bool IsLeft = ValidationHelper.GetBoolean(NewResolver.ResolveMacros(LeftSideMacro), false);
                bool IsRight = ValidationHelper.GetBoolean(NewResolver.ResolveMacros(RightSideMacro), false);
                return IsLeft || IsRight;
            }
            else
            {
                return false;
            }
        }

        [MacroMethod(typeof(bool), "Determines if the selector should be visible (if AdHoc relationship, the document is left and not right, if not adhoc relationship then document is left or is right and allowed to switch sides).", 0)]
        public static object RelationshipSelectorIsVisible(EvaluationContext context, params object[] parameters)
        {
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                bool AllowSwitchSides = ValidationHelper.GetBoolean(UIContext.Current.Data.GetValue("AllowSwitchSides"), false);
                return (IsAdHocRelationship() && IsLeft() && !IsRight()) || (!IsAdHocRelationship() && (IsLeft() || (IsRight() && AllowSwitchSides)));
            }
            else
            {
                return false;
            }
        }

        [MacroMethod(typeof(bool), "Determines if the Relationship Listing (Editable) should be visible (if the current document is a Left side, or is Right Side and Is not an ad-hoc relationship).", 0)]
        public static object RelationshipListingEditableIsVisible(EvaluationContext context, params object[] parameters)
        {
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                return (IsLeft() || (IsRight() && !IsAdHocRelationship()));
            }
            else
            {
                return false;
            }
        }

        [MacroMethod(typeof(bool), "Determines if the Relationship Listing (View Only) should be visible (if the current document is a Left side, or is Right Side and Is not an ad-hoc relationship).", 0)]
        public static object RelationshipListingReadOnlyIsVisible(EvaluationContext context, params object[] parameters)
        {
            if (CurrentNode() != null)
            {
                bool AllowSwitchSides = ValidationHelper.GetBoolean(UIContext.Current.Data.GetValue("AllowSwitchSides"), false);
                return !((IsLeft() || (IsRight() && !IsAdHocRelationship())));
            }
            else
            {
                return false;
            }
        }

        [MacroMethod(typeof(bool), "Returns if the current document is allowed on the Left side of the relationship (using the Left Side Macro).", 0)]
        public static object CurrentNodeIsLeft(EvaluationContext context, params object[] parameters)
        {
            return IsLeft();
        }

        [MacroMethod(typeof(bool), "Returns if the current document is allowed on the Right side of the relationship (using the Right Side Macro).", 0)]
        public static object CurrentNodeIsRight(EvaluationContext context, params object[] parameters)
        {
            return IsRight();
        }

        [MacroMethod(typeof(bool), "Returns if the current relationship (RelationshipName) is an AdHoc Relationship", 0)]
        public static object IsAdHocRelationship(EvaluationContext context, params object[] parameters)
        {
            return IsAdHocRelationship();
        }

        /// <summary>
        /// Determines if the current page fits the IsLeftSideMacro for the UI Page.
        /// </summary>
        /// <returns>True if it is a Left side relationship</returns>
        private static bool IsLeft()
        {
            string LeftSideMacro = ValidationHelper.GetString(UIContext.Current.Data.GetValue("IsLeftSideMacro"), "");
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                var NewResolver = MacroContext.CurrentResolver.CreateChild();
                NewResolver.SetNamedSourceData("CurrentDocument", currentNode);
                return ValidationHelper.GetBoolean(NewResolver.ResolveMacros(LeftSideMacro), false);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the current page fits the IsRightSideMacro for the UI Page.
        /// </summary>
        /// <returns>True if it is a Right side relationship</returns>
        private static bool IsRight()
        {
            string LeftRightMacro = ValidationHelper.GetString(UIContext.Current.Data.GetValue("IsRightSideMacro"), "");
            TreeNode currentNode = CurrentNode();
            if (currentNode != null)
            {
                var NewResolver = MacroContext.CurrentResolver.CreateChild();
                NewResolver.SetNamedSourceData("CurrentDocument", currentNode);
                return ValidationHelper.GetBoolean(NewResolver.ResolveMacros(LeftRightMacro), false);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the current Tree Node based on the NodeID parameter passed to UI elements
        /// </summary>
        /// <returns>The Tree Node that the current page belongs to</returns>
        private static TreeNode CurrentNode()
        {
            int NodeID = QueryHelper.GetInteger("NodeID", -1);
            string Culture = QueryHelper.GetString("culture", "en-US");
            if (NodeID > 0)
            {
                return CacheHelper.Cache<TreeNode>(cs =>
                {
                    TreeNode currentNode = new DocumentQuery().WhereEquals("NodeID", NodeID).Culture(Culture).CombineWithDefaultCulture(true).FirstObject;
                    if (currentNode != null && cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency("nodeid|" + NodeID);
                    }
                    return currentNode;
                }, new CacheSettings(10, "RelationshipMacro", "CurrentNode", NodeID, Culture));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines if the current relationship is an AdHoc relationship based on the UI Property RelationshipName
        /// </summary>
        /// <returns>True if the current relationship is an ad hoc relationship</returns>
        private static bool IsAdHocRelationship()
        {
            string RelationshipName = ValidationHelper.GetString(UIContext.Current.Data.GetValue("RelationshipName"), "");
            return CacheHelper.Cache<bool>(cs =>
            {
                RelationshipNameInfo relationshipObj = RelationshipNameInfoProvider.GetRelationshipNameInfo(RelationshipName);

                if (relationshipObj != null && cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency("cms.relationshipname|byid|" + relationshipObj.RelationshipNameId);
                }
                return relationshipObj != null ? relationshipObj.RelationshipNameIsAdHoc : false;
            }, new CacheSettings(10, "RelationshipMacro", "IsAdHocRelationship", RelationshipName));
        }

    }

    public class RelHelperMacrosMethods : MacroMethodContainer
    {

        [MacroMethod(typeof(string), "Returns a full where condition (for Document Category Relationships) to be used in filtering (ex repeaters).  If no categories provided or none found, returns 1=1", 1)]
        [MacroMethodParam(0, "Values", typeof(string), "| , or ; seperated list of category values (IDs, GUIDs, or CodeNames)")]
        [MacroMethodParam(1, "ConditionType", typeof(Enums.ConditionType), "RelEnums.ConditionType of what type of condition to generate.  Default is 'Any'")]
        [MacroMethodParam(2, "DocumentIDTableName", typeof(string), "The Table Name/Alias where the DocumentID belongs. Only needed for the 'All' Condition, defaults to CMS_Document.")]
        public static object GetDocumentCategoryWhere(EvaluationContext context, params object[] parameters)
        {
            ConditionType CondType = ConditionType.Any;
            string DocumentIDTableName = "CMS_Document";
            string[] Values = new string[] { };
            if (parameters.Length > 0)
            {
                Values = ValidationHelper.GetString(parameters[0], "").Split("|;,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            if (parameters.Length > 1)
            {
                CondType = (ConditionType)ValidationHelper.GetInteger(parameters[1], 0);
            }
            if (parameters.Length > 2)
            {
                DocumentIDTableName = SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[2], "CMS_Document")).Trim('.');
            }
            return RelHelper.GetDocumentCategoryWhere(Values, CondType, DocumentIDTableName);
        }

        [MacroMethod(typeof(string), "Returns a full where condition (for Node Category Relationships) to be used in filtering (ex repeaters).  If no categories provided or none found, returns 1=1", 1)]
        [MacroMethodParam(0, "Values", typeof(string), "| , or ; seperated list of category values (IDs, GUIDs, or CodeNames)")]
        [MacroMethodParam(1, "ConditionType", typeof(Enums.ConditionType), "RelEnums.ConditionType of what type of condition to generate.  Default is 'Any'")]
        [MacroMethodParam(2, "NodeIDTableName", typeof(string), "The Table Name/Alias where the NodeID belongs. Only needed for the 'All' Condition, defaults to CMS_Tree.")]
        public static object GetNodeCategoryWhere(EvaluationContext context, params object[] parameters)
        {
            int[] CategoryIDs = { };
            ConditionType CondType = ConditionType.Any;
            string NodeIDTable = "CMS_Tree";
            string[] Values = new string[] { };
            if (parameters.Length > 0)
            {
                Values = ValidationHelper.GetString(parameters[0], "").Split("|;,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            if (parameters.Length > 1)
            {
                CondType = (ConditionType)ValidationHelper.GetInteger(parameters[1], 0);
            }
            if (parameters.Length > 2)
            {
                NodeIDTable = SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[2], "CMS_Tree")).Trim('.');
            }
            return RelHelper.GetNodeCategoryWhere(Values, CondType, NodeIDTable);
        }

        [MacroMethod(typeof(string), "Returns a full where condition (for Binding tables that bind an object to Categories) to be used in filtering (ex repeaters).  If no categories provided or none found, returns 1=1", 5)]
        [MacroMethodParam(0, "BindingClass", typeof(string), "The Binding Class Code Name")]
        [MacroMethodParam(1, "ObjectIDFieldName", typeof(string), "The Field Name of this object that matches the binding table's Left Field value")]
        [MacroMethodParam(2, "LeftFieldName", typeof(string), "The Field Name of the binding class that contains this Object IDs value")]
        [MacroMethodParam(3, "RightFieldName", typeof(string), "The Field Name of the binding class that contains the Category's identy value")]
        [MacroMethodParam(4, "Values", typeof(string), "| , or ; seperated list of category values (IDs, GUIDs, or CodeNames)")]
        [MacroMethodParam(5, "IdentityType", typeof(IdentityType), "RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID")]
        [MacroMethodParam(6, "ConditionType", typeof(ConditionType), "RelEnums.ConditionType of what type of condition to generate.")]
        [MacroMethodParam(7, "ObjectIDTableName", typeof(string), "The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same.")]
        public static object GetBindingCategoryWhere(EvaluationContext context, params object[] parameters)
        {
            string BindingClass = ValidationHelper.GetString(parameters[0], "");
            string ObjectIDFieldName = "[" + SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[1], "")) + "]";
            string LeftFieldName = "[" + SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[2], "")) + "]";
            string RightFieldName = "[" + SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[3], "")) + "]";
            string[] Values = ValidationHelper.GetString(parameters[4], "").Split("|;,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string ObjIDTableName = null;
            IdentityType IdentType = IdentityType.ID;
            ConditionType CondType = ConditionType.Any;
            if (parameters.Length > 5)
            {
                IdentType = (IdentityType)ValidationHelper.GetInteger(parameters[5], 0);
            }
            if (parameters.Length > 6)
            {
                CondType = (ConditionType)ValidationHelper.GetInteger(parameters[6], 0);
            }
            if (parameters.Length > 7)
            {
                ObjIDTableName = SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[7], "").Trim('.'));
            }

            return RelHelper.GetBindingCategoryWhere(BindingClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, IdentType, CondType, ObjIDTableName);
        }

        [MacroMethod(typeof(string), "Returns a full where condition (for Binding Tables that bind on any object) to be used in filtering (ex repeaters).  If no object values provided or none found, returns 1=1", 6)]
        [MacroMethodParam(0, "BindingClass", typeof(string), "The Binding Class Code Name")]
        [MacroMethodParam(1, "ObjectClass", typeof(string), "The Object Class Code Name (the thing that is bound to the current object through the binding table)")]
        [MacroMethodParam(2, "ObjectIDFieldName", typeof(string), "The Field Name of this object that matches the binding table's Left Field value")]
        [MacroMethodParam(3, "LeftFieldName", typeof(string), "The Field Name of the binding class that contains this Object IDs value")]
        [MacroMethodParam(4, "RightFieldName", typeof(string), "The Field Name of the binding class that contains the related objects's identy value")]
        [MacroMethodParam(5, "Values", typeof(string), "| , or ; seperated list of object values (IDs, GUIDs, or CodeNames)")]
        [MacroMethodParam(6, "IdentityType", typeof(IdentityType), "RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID")]
        [MacroMethodParam(7, "ConditionType", typeof(ConditionType), "RelEnums.ConditionType of what type of condition to generate.")]
        [MacroMethodParam(8, "ObjectIDTableName", typeof(string), "The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same.")]
        public static object GetBindingWhere(EvaluationContext context, params object[] parameters)
        {
            string BindingClass = ValidationHelper.GetString(parameters[0], "");
            string ObjectClass = ValidationHelper.GetString(parameters[1], "");
            string ObjectIDFieldName = "[" + SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[2], "")) + "]";
            string LeftFieldName = "[" + SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[3], "")) + "]";
            string RightFieldName = "[" + SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[4], "")) + "]";
            string[] Values = ValidationHelper.GetString(parameters[5], "").Split("|;,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string ObjIDTableName = null;
            IdentityType IdentType = IdentityType.ID;
            ConditionType CondType = ConditionType.Any;
            if (parameters.Length > 6)
            {
                IdentType = (IdentityType)ValidationHelper.GetInteger(parameters[6], 0);
            }
            if (parameters.Length > 7)
            {
                CondType = (ConditionType)ValidationHelper.GetInteger(parameters[7], 0);
            }
            if (parameters.Length > 8)
            {
                ObjIDTableName = SqlHelper.EscapeQuotes(ValidationHelper.GetString(parameters[8], "").Trim('.'));
            }
            return RelHelper.GetBindingWhere(BindingClass, ObjectClass, ObjectIDFieldName, LeftFieldName, RightFieldName, Values, IdentType, CondType, ObjIDTableName);
        }
        
    }

}
