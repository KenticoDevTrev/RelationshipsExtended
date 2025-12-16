using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using RelationshipsExtended.Enums;
using System.Data;
using System.Xml;
using XperienceCommunity.RelationshipsExtended;
using XperienceCommunity.RelationshipsExtended.Classes.Helpers;

namespace RelationshipsExtended.Helpers
{
    public class RelHelper(IInfoProvider<TagInfo> tagInfoProvider, IProgressiveCache progressiveCache, RelationshipsExtendedOptions options) : IRelHelper
    {
        private readonly IInfoProvider<TagInfo> _tagInfoProvider = tagInfoProvider;
        private readonly IProgressiveCache _progressiveCache = progressiveCache;
        private readonly RelationshipsExtendedOptions _options = options;


        #region "Where Condition Generators"

        /// <summary>
        /// Returns a full where condition (for Content Item Category (Tag) Relationships) to be used in filtering (ex repeaters).  
        /// </summary>
        /// <param name="values">list of tag values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="contentIDTableName">The Table Name/Alias where the NodeID belongs. Only needed for the 'All' Condition, defaults to CMS_ContentItem.</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        public async Task<string> GetContentTagsWhere(IEnumerable<object> values, ConditionType condition = ConditionType.Any, string contentIDTableName = "CMS_ContentItem")
        {
            IEnumerable<int> categoryIDs = [];
            return await _progressiveCache.LoadAsync(async cs => {
                categoryIDs = await TagIdentitiesToIDs(values);
                if (!categoryIDs.Any()) {
                    return "(1=1)";
                }
                return condition switch {
                    ConditionType.All => $"(Select Count(*) from RelationshipsExtended_ContentItemCategory where ContentItemCategoryContentItemID = {contentIDTableName}.[ContentItemID] and ContentItemCategoryTagID in ({string.Join(",", categoryIDs)})) = {categoryIDs.Count()}",
                    ConditionType.None => $"(ContentItemID not in (Select ContentItemCategoryContentItemID from RelationshipsExtended_ContentItemCategory where ContentItemCategoryTagID in ({string.Join(",", categoryIDs)})))",
                    _ => $"(ContentItemID in (Select ContentItemCategoryContentItemID from RelationshipsExtended_ContentItemCategory where ContentItemCategoryTagID in ({string.Join(",", categoryIDs)})))",
                };
            }, new CacheSettings(_options.CacheWhereConditions ? _options.CacheMinutes : 0, "GetContentCategoryWhere", string.Join("|", values), condition, contentIDTableName));
        }

        /// <summary>
        /// Returns a full where condition (for Binding tables that bind an object to Categories) to be used in filtering (ex repeaters).  For property examples we will use Demo.Foo, CMS.Tag, and Demo.FooTag  
        /// </summary>
        /// <param name="bindingClass">The Binding Class Code Name</param>
        /// <param name="objectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID (From Demo.Foo)</param>
        /// <param name="leftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: FooID (from Demo.FooTag)</param>
        /// <param name="rightFieldName">The Field Name of the binding class that contains the Tag's identy value.  Ex: TagID (from Demo.FooTag) </param>
        /// <param name="values">list of tag values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="identity">RelEnums.IdentityType of what value is stored in the binding table for the tag, default is ID</param>
        /// <param name="condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="objectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_Foo</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        public async Task<string> GetBindingTagsWhere(string bindingClass, string objectIDFieldName, string leftFieldName, string rightFieldName, IEnumerable<object> values, IdentityType identity = IdentityType.ID, ConditionType condition = ConditionType.Any, string? objectIDTableName = null)
        {
            leftFieldName = GetBracketedColumnName(leftFieldName);
            rightFieldName = GetBracketedColumnName(rightFieldName);
            objectIDFieldName = GetBracketedColumnName(objectIDFieldName);
            return await _progressiveCache.LoadAsync(async cs => {
                // Find class table name
                var classObj = DataClassInfoProvider.GetDataClassInfo(bindingClass);
                if (classObj == null || string.IsNullOrEmpty(objectIDFieldName) || string.IsNullOrEmpty(leftFieldName) || string.IsNullOrEmpty(rightFieldName)) {
                    throw new Exception("Class or fields not provided/found.  Please ensure your macro is set up properly.");
                }

                var whereInValue = "";
                var tableName = classObj.ClassTableName;
                var count = 0;
                switch (identity) {
                    case IdentityType.ID:
                        var tagIDs = await TagIdentitiesToIDs(values);
                        whereInValue = string.Join(",", tagIDs);
                        count = tagIDs.Count();
                        break;
                    case IdentityType.Guid:
                        var tagGUIDs = await TagIdentitiesToGUIDs(values);
                        whereInValue = "'" + string.Join("','", tagGUIDs) + "'";
                        count = tagGUIDs.Count();
                        break;
                    case IdentityType.CodeName:
                        var tagCodeNames = await TagIdentitiesToCodeNames(values);
                        whereInValue = "'" + string.Join("','", tagCodeNames) + "'";
                        count = tagCodeNames.Count();
                        break;
                }
                if (count == 0) {
                    return "(1=1)";
                }
                return condition switch {
                    ConditionType.All => $"(Select Count(*) from {tableName} where {tableName}.{leftFieldName} = {(!string.IsNullOrWhiteSpace(objectIDTableName) ? objectIDTableName + "." : "")}{objectIDFieldName} and {rightFieldName} in ({whereInValue})) = {count}",
                    ConditionType.None => $"({objectIDFieldName} not in (Select {leftFieldName} from {tableName} where {rightFieldName} in ({whereInValue})))",
                    _ => $"({objectIDFieldName} in (Select {leftFieldName} from {tableName} where {rightFieldName} in ({whereInValue})))",
                };
            }, new CacheSettings(_options.CacheWhereConditions ? _options.CacheMinutes : 0, "GetBindingTagsWhere", bindingClass, objectIDFieldName, leftFieldName, rightFieldName, string.Join("|", values), identity, condition, objectIDTableName));
        }

        /// <summary>
        /// Returns a full where condition (for Binding Tables that bind on any object) to be used in filtering (ex repeaters).  For property exampples, we will assume Demo.Foo, Demo.Bar, and Demo.FooBar
        /// </summary>
        /// <param name="bindingClass">The Binding Class Code Name.  Ex: Demo.FooBar</param>
        /// <param name="objectClass">The Object Class Code Name (the thing that is bound to the current object through the binding table).  Ex: Demo.Bar</param>
        /// <param name="objectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID (from Demo.Foo)</param>
        /// <param name="leftFieldName">The Field Name of the binding class that contains this Object IDs value. Ex: FooID (from Demo.FooBar)</param>
        /// <param name="rightFieldName">The Field Name of the binding class that contains the related objects's identy value.  Ex: BarID (from Demo.FooBar)</param>
        /// <param name="values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="identity">RelEnums.IdentityType of what value is stored in the binding table for the tag, default is ID</param>
        /// <param name="condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="objectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBar</param>
        /// <returns>The Where Condition, If no object values provided or none found, returns 1=1</returns>
        public async Task<string> GetBindingWhere(string bindingClass, string objectClass, string objectIDFieldName, string leftFieldName, string rightFieldName, IEnumerable<string> values, IdentityType identity = IdentityType.ID, ConditionType condition = ConditionType.Any, string? objectIDTableName = null)
        {
            leftFieldName = GetBracketedColumnName(leftFieldName);
            rightFieldName = GetBracketedColumnName(rightFieldName);
            objectIDFieldName = GetBracketedColumnName(objectIDFieldName);
            return await _progressiveCache.LoadAsync(async cs => {
                // Find class table name
                var classObj = DataClassInfoProvider.GetDataClassInfo(bindingClass);
                var classObjSummary = await GetClassObjSummary(objectClass);

                var whereInValue = "";
                var tableName = classObj.ClassTableName;
                var count = 0;
                switch (identity) {
                    case IdentityType.ID:
                        var objectIDs = await ObjectIdentitiesToIDs(classObjSummary, values);
                        whereInValue = (objectIDs.Any() ? string.Join(",", objectIDs) : "''");
                        count = objectIDs.Count();
                        break;
                    case IdentityType.Guid:
                        var objectGUIDs = await ObjectIdentitiesToGUIDs(classObjSummary, values);
                        whereInValue = "'" + string.Join("','", objectGUIDs) + "'";
                        count = objectGUIDs.Count();
                        break;
                    case IdentityType.CodeName:
                        var objectCodeNames = await ObjectIdentitiesToCodeNames(classObjSummary, values);
                        whereInValue = "'" + string.Join("','", objectCodeNames) + "'";
                        count = objectCodeNames.Count();
                        break;
                }

                // If no related object IDs found, then completely ignore.
                if (count == 0) {
                    return "(1=1)";
                }

                return condition switch {
                    ConditionType.All => $"(Select Count(*) from {tableName} where {tableName}.{leftFieldName} = {(!string.IsNullOrWhiteSpace(objectIDTableName) ? objectIDTableName + "." : "")}{objectIDFieldName} and {rightFieldName} in ({whereInValue})) = {count}",
                    ConditionType.None => $"({objectIDFieldName} not in (Select {leftFieldName} from {tableName} where {rightFieldName} in ({whereInValue})))",
                    _ => $"({objectIDFieldName} in (Select {leftFieldName} from {tableName} where {rightFieldName} in ({whereInValue})))",
                };
            }, new CacheSettings(_options.CacheWhereConditions ? _options.CacheMinutes : 0, "GetBindingWhere", bindingClass, objectClass, objectIDFieldName, leftFieldName, rightFieldName, string.Join("|", values), identity, condition, objectIDTableName));
        }
        #endregion


        #region "public Helpers"


        /// <summary>
        /// Gets the Tag identities where condition (ex (TagId in (1,2,3) )
        /// </summary>
        /// <param name="tagIdentifications">List of Ints, Guids, or CodeNames of the Tags</param>
        /// <returns>the Tag identity where condition</returns>
        public static string TagIdentitiesWhere(IEnumerable<object> tagIdentifications)
        {
            List<Guid> guids = [];
            List<int> ints = [];
            List<string> strings = [];

            foreach (object tagIdentification in tagIdentifications) {
                var guidVal = ValidationHelper.GetGuid(tagIdentification, Guid.Empty);
                var intVal = ValidationHelper.GetInteger(tagIdentification, -1);
                var stringVal = ValidationHelper.GetString(tagIdentification, "");
                if (guidVal != Guid.Empty) {
                    guids.Add(guidVal);
                } else if (intVal > 0) {
                    ints.Add(intVal);
                } else if (!string.IsNullOrWhiteSpace(stringVal)) {
                    strings.Add(SqlHelper.EscapeQuotes(stringVal));
                }
            }
            var whereCondition = "";
            if (guids.Count > 0) {
                whereCondition = SqlHelper.AddWhereCondition(whereCondition, $"(TagGuid in ('{string.Join("','", guids.Select(x => x.ToString()))}'))", "OR");
            }
            if (ints.Count > 0) {
                whereCondition = SqlHelper.AddWhereCondition(whereCondition, $"(TagID in ('{string.Join("','", ints.Select(x => x.ToString()))}'))", "OR");
            }
            if (strings.Count > 0) {
                whereCondition = SqlHelper.AddWhereCondition(whereCondition, $"(TagName in ('{string.Join("','", strings)}'))", "OR");
            }
            return (!string.IsNullOrWhiteSpace(whereCondition) ? whereCondition : "(1=0)");

        }

        /// <summary>
        /// Converts Tag IDs, Guids, or CodeNames to TagIDs
        /// </summary>
        /// <param name="CategoryIdentifications">List of Tag IDs, Guids, or CodeNames</param>
        /// <returns>List of Tag IDs</returns>
        public async Task<IEnumerable<int>> TagIdentitiesToIDs(IEnumerable<object> tagIdentifications)
        {
            return (await _tagInfoProvider.Get().Where(TagIdentitiesWhere(tagIdentifications)).Columns(nameof(TagInfo.TagID)).GetEnumerableTypedResultAsync()).Select(x => x.TagID);
        }

        /// <summary>
        /// Converts Tag IDs, Guids, or CodeNames to TagGUIDs
        /// </summary>
        /// <param name="tagIdentifications">List of Tag IDs, Guids, or CodeNames</param>
        /// <returns>List of Tag GUIDs</returns>
        public async Task<IEnumerable<Guid>> TagIdentitiesToGUIDs(IEnumerable<object> tagIdentifications)
        {
            return (await _tagInfoProvider.Get().Where(TagIdentitiesWhere(tagIdentifications)).Columns(nameof(TagInfo.TagGUID)).GetEnumerableTypedResultAsync()).Select(x => x.TagGUID);
        }

        /// <summary>
        /// Converts Tag IDs, Guids, or CodeNames to Tag CodeNames
        /// </summary>
        /// <param name="tagIdentifications">List of Tag IDs, Guids, or CodeNames</param>
        /// <returns>List of Tag Code Names</returns>
        public async Task<IEnumerable<string>> TagIdentitiesToCodeNames(IEnumerable<object> tagIdentifications)
        {
            return (await _tagInfoProvider.Get().Where(TagIdentitiesWhere(tagIdentifications)).Columns(nameof(TagInfo.TagName)).GetEnumerableTypedResultAsync()).Select(x => x.TagName);
        }

        /// <summary>
        /// Gets a Class Object Summary based on the class name.
        /// </summary>
        /// <param name="ClassName">The Class Name</param>
        /// <returns>The Class Object Summary</returns>
        public async Task<ClassObjSummary> GetClassObjSummary(string ClassName)
        {
            return await _progressiveCache.LoadAsync(async cs => {
                var summaryObj = new ClassObjSummary(ClassName);
                var ClassObj = DataClassInfoProvider.GetDataClassInfo(ClassName);
                if (ClassObj != null) {
                    summaryObj.ClassIsContentType = ClassObj.ClassType == "Content";
                    summaryObj.TableName = ClassObj.ClassTableName;
                } else {
                    summaryObj.ClassIsContentType = false;
                }
                // now get GUID and Code Name if possible.
                var ObjectClassFactoryObj = new InfoObjectFactory(ClassName);
                if (ObjectClassFactoryObj != null && ObjectClassFactoryObj.Singleton != null) {
                    ObjectTypeInfo typeInfoObj = ((BaseInfo)ObjectClassFactoryObj.Singleton).TypeInfo;
                    summaryObj.IDColumn = ValidationHelper.GetString(typeInfoObj.IDColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "");
                    summaryObj.GUIDColumn = ValidationHelper.GetString(typeInfoObj.GUIDColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "");
                    summaryObj.CodeNameColumn = ValidationHelper.GetString(typeInfoObj.CodeNameColumn, "").Replace(ObjectTypeInfo.COLUMN_NAME_UNKNOWN, "");
                } else {
                    // handle unique cases
                    switch (ClassName.ToLower()) {
                        case "cms.contentitem":
                            summaryObj.IDColumn = "ContentItemID";
                            summaryObj.CodeNameColumn = "ContentItemName";
                            summaryObj.GUIDColumn = "ContentItemGuid";
                            break;
                        case "cms.contentitemcommondata":
                            summaryObj.IDColumn = "ContentItemCommonDataID";
                            summaryObj.GUIDColumn = "ContentItemCommonDataGUID";
                            break;
                        case "cms.contentitemlanguagemetadata":
                            summaryObj.IDColumn = "ContentItemLanguageMetadataID";
                            summaryObj.GUIDColumn = "ContentItemLanguageMetadataGUID";
                            break;
                    }

                    if (ClassObj != null) {
                        // if still missing fields, try parsing XML
                        if (string.IsNullOrWhiteSpace(summaryObj.CodeNameColumn) || string.IsNullOrWhiteSpace(summaryObj.GUIDColumn) || string.IsNullOrWhiteSpace(summaryObj.IDColumn)) {
                            XmlDocument classXML = new XmlDocument();
                            classXML.LoadXml(ClassObj.ClassFormDefinition);
                            if (string.IsNullOrWhiteSpace(summaryObj.IDColumn)) {
                                try {
                                    var idNode = classXML.SelectNodes("/form/field[@columntype='integer' and @isPK='true']");
                                    if (idNode != null && idNode.Item(0) != null) {
                                        summaryObj.IDColumn = idNode.Item(0)?.Attributes?["column"]?.Value ?? string.Empty;
                                    }
                                } catch (Exception) {
                                    // can't figure out that code name
                                }
                            }
                        }
                    }
                }
                if (cs.Cached) {
                    cs.CacheDependency = CacheHelper.GetCacheDependency("cms.class|byname|" + ClassName);
                }
                return summaryObj;
            }, new CacheSettings(_options.CacheMinutes, "GetClassObjSummary", ClassName));
        }

        /// <summary>
        /// Converts an Object's IDs, Guids, or CodeNames to the Object IDs
        /// </summary>
        /// <param name="classObjSummary">The Class Object Summary</param>
        /// <param name="ObjectIdentifications">List of Object IDs, Guids, or CodeNames</param>
        /// <returns>A list of the Object's IDs</returns>
        public async Task<IEnumerable<int>> ObjectIdentitiesToIDs(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications)
        {
            if (classObjSummary.ClassIsContentType) {
                throw new NotImplementedException("Can't convert content item identities yet");
            } else {
                return (await new ObjectQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.IDColumn).GetEnumerableTypedResultAsync()).Select(x => (int)x.GetValue(classObjSummary.IDColumn));
            }
        }

        /// <summary>
        /// Converts an Object's IDs, Guids, or CodeNames to the Objects GUID
        /// </summary>
        /// <param name="classObjSummary">The Class Object Summary</param>
        /// <param name="ObjectIdentifications">List of Object IDs, Guids, or CodeNames</param>
        /// <returns>A list of the Object's GUIDs</returns>
        public async Task<IEnumerable<Guid>> ObjectIdentitiesToGUIDs(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications)
        {
            if (classObjSummary.ClassIsContentType) {
                throw new NotImplementedException("Can't convert content item identities yet");
            } else {
                return (await new ObjectQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.GUIDColumn).GetEnumerableTypedResultAsync()).Select(x => (Guid)x.GetValue(classObjSummary.GUIDColumn));
            }
        }

        /// <summary>
        /// Converts an Object's IDs, Guids, or CodeNames to the Objects CodeNames
        /// </summary>
        /// <param name="classObjSummary">The Class Object Summary</param>
        /// <param name="ObjectIdentifications">List of Object IDs, Guids, or CodeNames</param>
        /// <returns>A list of the Object's Code Names</returns>
        public async Task<IEnumerable<string>> ObjectIdentitiesToCodeNames(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications)
        {
            if (classObjSummary.ClassIsContentType) {
                throw new NotImplementedException("Can't convert content item identities yet");
            } else {
                return (await new ObjectQuery(classObjSummary.ClassName).Where(ObjectIdentitiesWhere(classObjSummary, ObjectIdentifications)).Columns(classObjSummary.CodeNameColumn).GetEnumerableTypedResultAsync()).Select(x => (string)x.GetValue(classObjSummary.CodeNameColumn));
            }
        }

        /// <summary>
        /// Gets the Object WHERE condition based on the given identities
        /// </summary>
        /// <param name="classObjSummary">The Class Object Summary</param>
        /// <param name="ObjectIdentifications">List of IDs, Guids, or CodeNames</param>
        /// <returns>The WHERE condition to select the objects (ex MyObjectID in (1,2,3) )</returns>
        public string ObjectIdentitiesWhere(ClassObjSummary classObjSummary, IEnumerable<object> ObjectIdentifications)
        {
            List<Guid> guids = [];
            List<int> ints = [];
            List<string> strings = [];

            foreach (object ObjectIdentification in ObjectIdentifications) {
                var guidVal = ValidationHelper.GetGuid(ObjectIdentification, Guid.Empty);
                var intVal = ValidationHelper.GetInteger(ObjectIdentification, -1);
                var stringVal = ValidationHelper.GetString(ObjectIdentification, "");
                if (guidVal != Guid.Empty) {
                    guids.Add(guidVal);
                } else if (intVal > 0) {
                    ints.Add(intVal);
                } else if (!string.IsNullOrWhiteSpace(stringVal)) {
                    strings.Add(SqlHelper.EscapeQuotes(stringVal));
                }
            }

            var whereCondition = "";
            if (guids.Count > 0 && !string.IsNullOrWhiteSpace(classObjSummary.GUIDColumn)) {
                whereCondition = SqlHelper.AddWhereCondition(whereCondition, $"({classObjSummary.GUIDColumn} in ('{string.Join("','", guids.Select(x => x.ToString()))}'))", "OR");
            }
            if (ints.Count > 0 && !string.IsNullOrWhiteSpace(classObjSummary.IDColumn)) {
                whereCondition = SqlHelper.AddWhereCondition(whereCondition, $"({classObjSummary.IDColumn} in ({string.Join(",", ints.Select(x => x.ToString()))}))", "OR");
            }
            if (strings.Count > 0 && !string.IsNullOrWhiteSpace(classObjSummary.CodeNameColumn)) {
                whereCondition = SqlHelper.AddWhereCondition(whereCondition, $"({classObjSummary.CodeNameColumn} in ('{string.Join("','", strings)}'))", "OR");
            }
            return (!string.IsNullOrWhiteSpace(whereCondition) ? whereCondition : "(1=0)");
        }

        /// <summary>
        /// Makes sure to wrap the field in []'s, along with handling full-pathed fields such as My_Table.MyField
        /// </summary>
        /// <param name="Field">The Field Name (ex MyField, or My_Table.MyField)</param>
        /// <returns>The properly formatted FieldName</returns>
        public string GetBracketedColumnName(string Field)
        {
            string[] FieldSplit = Field.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < FieldSplit.Length; i++) {
                FieldSplit[i] = string.Format("[{0}]", FieldSplit[i].Trim("[]".ToCharArray()));
            }
            return string.Join(".", FieldSplit);
        }

        #endregion
    }
}