using RelationshipsExtended;
using System;
using RelationshipsExtended.Enums;
using CMS.MacroEngine;
using CMS;
using CMS.Helpers;
using CMS.DataEngine;

[assembly: RegisterExtension(typeof(RelHelperMacrosMethods), typeof(RelHelperMacroNamespace))]
namespace RelationshipsExtended
{
    /// <summary>
    /// Macro methods for RelHelper
    /// </summary>
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
