using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.Helpers;
using RelationshipsExtended;
using RelationshipsExtended.Enums;
using RelationshipsExtended.Helpers;
using RelationshipsExtended.Interfaces;
using System.Text.RegularExpressions;
using XperienceCommunity.RelationshipsExtended.Classes.Helpers;

namespace XperienceCommunity.RelationshipsExtended.Services.Implementations
{
    public class RelationshipExtendedHelper(IRelHelper relHelper, IProgressiveCache progressiveCache, RelationshipsExtendedOptions options,
        IInfoProvider<ContentItemCategoryInfo> contentItemCategoryInfo) : IRelationshipExtendedHelper
    {
        private readonly IRelHelper _relHelper = relHelper;
        private readonly IProgressiveCache _progressiveCache = progressiveCache;
        private readonly RelationshipsExtendedOptions _options = options;
        private readonly IInfoProvider<ContentItemCategoryInfo> _contentItemCategoryInfo = contentItemCategoryInfo;

        public async Task<ObjectQuery> BindingTagsCondition(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            baseQuery.Where(await GetBindingTagWhere(bindingClass, bindingCondition, values, condition));
            return baseQuery;
        }

        public async Task<ObjectQuery<TObject>> BindingTagsCondition<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any) where TObject : BaseInfo, new()
        {
            baseQuery.Where(await GetBindingTagWhere(bindingClass, bindingCondition, values, condition));
            return baseQuery;
        }

        public async Task<MultiObjectQuery> BindingTagsCondition(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            baseQuery.Where(await GetBindingTagWhere(bindingClass, bindingCondition, values, condition));
            return baseQuery;
        }

        public async Task<ContentTypeQueryParameters> BindingTagsCondition(ContentTypeQueryParameters baseQuery, IEnumerable<object> values, ContentItemConditionType condition = ContentItemConditionType.Any)
        {
            // Since Content Type Queries can't do a string Where Condition, must get all the ContentItemIDs in the given categories and then do the In or not in
            var bindingClass = new ContentItemCategoryInfo();
            var contentItemIds = await GetContentItemIdsForTags(values);
            baseQuery.Where(x => {
                if (condition == ContentItemConditionType.Any) {
                    x.WhereIn(nameof(ContentItemFields.ContentItemID), contentItemIds);
                } else {
                    x.WhereNotIn(nameof(ContentItemFields.ContentItemID), contentItemIds);
                }
            });
            return baseQuery;
        }

        public async Task<ObjectQuery> BindingCondition(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            baseQuery.Where(await GetBindingWhere(bindingClass, bindingCondition, values, condition));
            return baseQuery;
        }

        public async Task<ObjectQuery<TObject>> BindingCondition<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any) where TObject : BaseInfo, new()
        {
            baseQuery.Where(await GetBindingWhere(bindingClass, bindingCondition, values, condition));
            return baseQuery;
        }

        public async Task<MultiObjectQuery> BindingCondition(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            baseQuery.Where(await GetBindingWhere(bindingClass, bindingCondition, values, condition));
            return baseQuery;
        }


        public async Task<ObjectQuery> InCustomRelationshipWithPossibleOrder(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true)
        {
            var lookupVal = await GetLookupValue(bindingClass.ParentClassName(), inRelationshipWithValue, bindingClass.ChildReferenceType());
            baseQuery.Source((QuerySource s) =>
                s.InnerJoin(new QuerySourceTable(bindingClass.BindingTableName()),
                    new WhereCondition($"{_relHelper.GetBracketedColumnName(bindingClass.ParentClassReferenceColumn())} = {_relHelper.GetBracketedColumnName(bindingClass.ParentObjectReferenceColumnName())}")
                        .WhereEquals(bindingClass.ChildObjectReferenceColumnName(), lookupVal)
                        )
            );

            // add the order by
            if (!string.IsNullOrWhiteSpace(bindingClass.OrderColumn())) {
                if (orderAsc) {
                    baseQuery.OrderBy(bindingClass.OrderColumn());
                } else {
                    baseQuery.OrderByDescending(bindingClass.OrderColumn());
                }
            }
            return baseQuery;
        }

        public async Task<ObjectQuery<TObject>> InCustomRelationshipWithPossibleOrder<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true) where TObject : BaseInfo, new()
        {
            var lookupVal = await GetLookupValue(bindingClass.ParentClassName(), inRelationshipWithValue, bindingClass.ChildReferenceType());
            baseQuery.Source((QuerySource s) =>
                s.InnerJoin(new QuerySourceTable(bindingClass.BindingTableName()),
                    new WhereCondition($"{_relHelper.GetBracketedColumnName(bindingClass.ParentClassReferenceColumn())} = {_relHelper.GetBracketedColumnName(bindingClass.ParentObjectReferenceColumnName())}")
                        .WhereEquals(bindingClass.ChildObjectReferenceColumnName(), lookupVal)
                        )
            );

            // add the order by
            if (!string.IsNullOrWhiteSpace(bindingClass.OrderColumn())) {
                if (orderAsc) {
                    baseQuery.OrderBy(bindingClass.OrderColumn());
                } else {
                    baseQuery.OrderByDescending(bindingClass.OrderColumn());
                }
            }
            return baseQuery;
        }

        public async Task<MultiObjectQuery> InCustomRelationshipWithPossibleOrder(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true)
        {
            var lookupVal = await GetLookupValue(bindingClass.ParentClassName(), inRelationshipWithValue, bindingClass.ChildReferenceType());
            baseQuery.Source((QuerySource s) => 
                s.InnerJoin(new QuerySourceTable(bindingClass.BindingTableName()), 
                    new WhereCondition($"{ _relHelper.GetBracketedColumnName(bindingClass.ParentClassReferenceColumn())} = {_relHelper.GetBracketedColumnName(bindingClass.ParentObjectReferenceColumnName())}")
                        .WhereEquals(bindingClass.ChildObjectReferenceColumnName(), lookupVal)
                        )
            );

            // add the order by
            if (!string.IsNullOrWhiteSpace(bindingClass.OrderColumn())) {
                if (orderAsc) {
                    baseQuery.OrderBy(bindingClass.OrderColumn());
                } else {
                    baseQuery.OrderByDescending(bindingClass.OrderColumn());
                }
            }
            return baseQuery;
        }

        /// <summary>
        /// Gets the Lookup value for the class, converting whatever the value is passed to the ID type.
        /// </summary>
        /// <param name="primaryClass"></param>
        /// <param name="inRelationshipWithValue"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        private async Task<object> GetLookupValue(string primaryClass, object inRelationshipWithValue, IdentityType identity)
        {
            switch (identity) {
                default:
                case IdentityType.ID:
                    return (await _relHelper.ObjectIdentitiesToIDs(await _relHelper.GetClassObjSummary(primaryClass), [inRelationshipWithValue])).FirstOrDefault();
                case IdentityType.CodeName:
                    return (await _relHelper.ObjectIdentitiesToCodeNames(await _relHelper.GetClassObjSummary(primaryClass), [inRelationshipWithValue])).FirstOrDefault() ?? string.Empty;
                case IdentityType.Guid:
                    return (await _relHelper.ObjectIdentitiesToGUIDs(await _relHelper.GetClassObjSummary(primaryClass), [inRelationshipWithValue])).FirstOrDefault();
            }
        }


        #region "Where Condition Generators"

        private async Task<IEnumerable<int>> GetContentItemIdsForTags(IEnumerable<object> tagValues)
        {
            var intValues = (tagValues.Any(x => x is not int) ? await _relHelper.TagIdentitiesToIDs(tagValues) : tagValues.Select(x => (int)x));
            return await _progressiveCache.LoadAsync(async cs => {
                if (cs.Cached) {
                    cs.CacheDependency = CacheHelper.GetCacheDependency($"{ContentItemCategoryInfo.OBJECT_TYPE}|all");
                }
                return (await _contentItemCategoryInfo.Get().WhereIn(nameof(ContentItemCategoryInfo.ContentItemCategoryTagID), intValues).Columns(nameof(ContentItemCategoryInfo.ContentItemCategoryContentItemID)).GetEnumerableTypedResultAsync()).Select(x => x.ContentItemCategoryContentItemID);
            }, new CacheSettings(_options.CacheMinutes, "GetContentItemIdsFromTags", string.Join(",", intValues)));
        }

        /// <summary>
        /// Returns a full where condition (for Relationships Extended Content Item Category (Tag) Relationships) to be used in filtering (ex repeaters).  
        /// </summary>
        /// <param name="values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="NodeIDTableName">The Table Name/Alias where the NodeID belongs. Only needed for the 'All' Condition, defaults to CMS_ContentItem.</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        public async Task<string> GetContentItemCategoryWhere(IEnumerable<object> values, ConditionType condition = ConditionType.Any, string contentItemIDTableName = "CMS_ContentItem")
        {

            return await _progressiveCache.LoadAsync(async cs => {
                var tagIDs = await _relHelper.TagIdentitiesToIDs(values);
                if (!tagIDs.Any()) {
                    return "(1=1)";
                }
                switch (condition) {
                    case ConditionType.Any:
                    default:
                        return $"(ContentItemID in (Select ContentItemCategoryContentItemID from RelationshipsExtended_ContentItemCategory where ContentItemCategoryTagID in ({string.Join(",", tagIDs)})))";
                    case ConditionType.All:
                        contentItemIDTableName = new Regex("[^a-zA-Z0-9 _-]").Replace(contentItemIDTableName, "");
                        return $"(Select Count(*) from RelationshipsExtended_ContentItemCategory where ContentItemCategoryContentItemID = [{contentItemIDTableName}].[ContentItemID] and ContentItemCategoryTagID in ({string.Join(",", tagIDs)})) = {tagIDs.Count()}";
                    case ConditionType.None:
                        return $"(ContentItemID not in (Select ContentItemCategoryContentItemID from RelationshipsExtended_ContentItemCategory where ContentItemCategoryTagID in ({string.Join(",", tagIDs)})))";
                }
            }, new CacheSettings(_options.CacheMinutes, "GetNodeCategoryWhere", string.Join("|", values), condition, contentItemIDTableName));
        }

        /// <summary>
        /// Returns a full where condition (for Binding tables that bind an object to Categories) to be used in filtering (ex repeaters).  For property examples we will use Demo.Foo, CMS.Category, and Demo.FooCategory  
        /// </summary>
        /// <param name="bindingClass">The Binding Class Code Name</param>
        /// <param name="objectIDFieldName">The Field Name of this object that matches the binding table's Left Field value. Ex: FooID (From Demo.Foo)</param>
        /// <param name="leftFieldName">The Field Name of the binding class that contains this Object IDs value.  Ex: FooID (from Demo.FooCategory)</param>
        /// <param name="rightFieldName">The Field Name of the binding class that contains the Category's identy value.  Ex: CategoryID (from Demo.FooCategory) </param>
        /// <param name="values">list of category values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="objectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_Foo</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        public async Task<string> GetBindingCategoryWhere(string bindingClass, string objectIDFieldName, string leftFieldName, string rightFieldName, IEnumerable<object> values, IdentityType identity = IdentityType.ID, ConditionType condition = ConditionType.Any, string? objectIDTableName = null)
        {
            leftFieldName = _relHelper.GetBracketedColumnName(leftFieldName);
            rightFieldName = _relHelper.GetBracketedColumnName(rightFieldName);
            objectIDFieldName = _relHelper.GetBracketedColumnName(objectIDFieldName);
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
                        var tagIDs = await _relHelper.TagIdentitiesToIDs(values);
                        whereInValue = string.Join(",", tagIDs);
                        count = tagIDs.Count();
                        break;
                    case IdentityType.Guid:
                        var tagGUIDs = await _relHelper.TagIdentitiesToGUIDs(values);
                        whereInValue = "'" + string.Join("','", tagGUIDs) + "'";
                        count = tagGUIDs.Count();
                        break;
                    case IdentityType.CodeName:
                        var tagCodeNames = await _relHelper.TagIdentitiesToCodeNames(values);
                        whereInValue = "'" + string.Join("','", tagCodeNames) + "'";
                        count = tagCodeNames.Count();
                        break;
                }
                if (count == 0) {
                    return "(1=1)";
                }
                return condition switch {
                    ConditionType.All => $"(Select Count(*) from [{tableName}] where [{tableName}].[{leftFieldName}] = {(!string.IsNullOrWhiteSpace(objectIDTableName) ? objectIDTableName + "." : "")}{objectIDFieldName} and {rightFieldName} in ({whereInValue})) = {count}",
                    ConditionType.None => $"({objectIDFieldName} not in (Select {leftFieldName} from {tableName} where {rightFieldName} in ({whereInValue})))",
                    _ => $"({objectIDFieldName} in (Select {leftFieldName} from {tableName} where {rightFieldName} in ({whereInValue})))",
                };
            }, new CacheSettings(_options.CacheMinutes, "GetBindingCategoryWhere", bindingClass, objectIDFieldName, leftFieldName, rightFieldName, string.Join("|", values), identity, condition, objectIDTableName));
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
        /// <param name="identity">RelEnums.IdentityType of what value is stored in the binding table for the category, default is ID</param>
        /// <param name="condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <param name="objectIDTableName">The Table Name/Alias where the ObjectIDFieldName belongs. Only needed for the 'All' Condition and if the ObjectIDField and LeftFieldName are the same. Ex: Demo_FooBar</param>
        /// <returns>The Where Condition, If no object values provided or none found, returns 1=1</returns>
        public async Task<string> GetBindingWhere(string bindingClass, string objectClass, string objectIDFieldName, string leftFieldName, string rightFieldName, IEnumerable<object> values, IdentityType identity = IdentityType.ID, ConditionType condition = ConditionType.Any, string? objectIDTableName = null)
        {
            leftFieldName = _relHelper.GetBracketedColumnName(leftFieldName);
            rightFieldName = _relHelper.GetBracketedColumnName(rightFieldName);
            objectIDFieldName = _relHelper.GetBracketedColumnName(objectIDFieldName);
            return await _progressiveCache.LoadAsync(async cs => {
                // Find class table name
                var classObj = DataClassInfoProvider.GetDataClassInfo(bindingClass);
                var classObjSummary = await _relHelper.GetClassObjSummary(objectClass);

                var whereInValue = "";
                var tableName = classObj.ClassTableName;
                var count = 0;
                switch (identity) {
                    case IdentityType.ID:
                        var objectIDs = await _relHelper.ObjectIdentitiesToIDs(classObjSummary, values);
                        whereInValue = (objectIDs.Count() > 0 ? string.Join(",", objectIDs) : "''");
                        count = objectIDs.Count();
                        break;
                    case IdentityType.Guid:
                        var objectGUIDs = await _relHelper.ObjectIdentitiesToGUIDs(classObjSummary, values);
                        whereInValue = "'" + string.Join("','", objectGUIDs) + "'";
                        count = objectGUIDs.Count();
                        break;
                    case IdentityType.CodeName:
                        var objectCodeNames = await _relHelper.ObjectIdentitiesToCodeNames(classObjSummary, values);
                        whereInValue = "'" + string.Join("','", objectCodeNames) + "'";
                        count = objectCodeNames.Count();
                        break;
                }

                // If no related object IDs found, then completely ignore.
                if (count == 0) {
                    return "(1=1)";
                }

                switch (condition) {
                    case ConditionType.Any:
                    default:
                        return $"({objectIDFieldName} in (Select {leftFieldName} from [{tableName}] where {rightFieldName} in ({whereInValue})))";
                    case ConditionType.All:
                        if (!string.IsNullOrWhiteSpace(objectIDTableName)) {
                            objectIDTableName = new Regex("[^a-zA-Z0-9 _-]").Replace(objectIDTableName, "");
                        }
                        return $"(Select Count(*) from [{tableName}] where [{tableName}].{leftFieldName} = {(!string.IsNullOrWhiteSpace(objectIDTableName) ? "[" + objectIDTableName + "]." : "")}{objectIDFieldName} and {rightFieldName} in ({whereInValue})) = {count}";
                    case ConditionType.None:
                        return $"({objectIDFieldName} not in (Select {leftFieldName} from [{tableName}] where {rightFieldName} in ({whereInValue})))";
                }
            }, new CacheSettings(_options.CacheMinutes, "GetBindingWhere", bindingClass, objectClass, objectIDFieldName, leftFieldName, rightFieldName, string.Join("|", values), identity, condition, objectIDTableName));
        }

        /// <summary>
        /// Returns a full where condition (for Binding tables that bind an object to Categories) to be used in filtering (ex repeaters).  For property examples we will use Demo.Foo, CMS.Tag, and Demo.FooTag  
        /// </summary>
        /// <param name="bindingClass">The Binding Class (ex new FooTagInfo())</param>
        /// <param name="bindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="values">list of values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <returns>The Where Condition, If no categories provided or none found, returns 1=1</returns>
        public async Task<string> GetBindingTagWhere(IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            string leftFieldName;
            string rightFieldName;
            string objectIDFieldName;
            IdentityType identity;
            string objectClassName;
            switch (bindingCondition) {
                case BindingConditionType.FilterParentsByChildren:
                default:
                    leftFieldName = _relHelper.GetBracketedColumnName(bindingClass.ParentObjectReferenceColumnName());
                    rightFieldName = _relHelper.GetBracketedColumnName(bindingClass.ChildObjectReferenceColumnName());
                    objectIDFieldName = _relHelper.GetBracketedColumnName(bindingClass.ParentClassReferenceColumn());
                    objectClassName = bindingClass.ChildClassName();
                    identity = bindingClass.ChildReferenceType();
                    break;
                case BindingConditionType.FilterChildrenByParents:
                    leftFieldName = _relHelper.GetBracketedColumnName(bindingClass.ChildObjectReferenceColumnName());
                    rightFieldName = _relHelper.GetBracketedColumnName(bindingClass.ParentObjectReferenceColumnName()); ;
                    objectIDFieldName = _relHelper.GetBracketedColumnName(bindingClass.ChildClassReferenceColumn());
                    objectClassName = bindingClass.ParentClassName();
                    identity = bindingClass.ParentReferenceType();
                    break;
            }
            return await _progressiveCache.LoadAsync(async cs => {
                var whereInValue = "";
                var tableName = bindingClass.BindingTableName();
                var count = 0;
                // If reference field is 
                if (objectClassName.Equals(TagInfo.OBJECT_TYPE, StringComparison.InvariantCultureIgnoreCase)) {
                    switch (identity) {
                        case IdentityType.ID:
                            var tagIDs = await _relHelper.TagIdentitiesToIDs(values);
                            whereInValue = string.Join(",", tagIDs);
                            count = tagIDs.Count();
                            break;
                        case IdentityType.Guid:
                            var tagGUIDs = await _relHelper.TagIdentitiesToGUIDs(values);
                            whereInValue = "'" + string.Join("','", tagGUIDs) + "'";
                            count = tagGUIDs.Count();
                            break;
                        case IdentityType.CodeName:
                            var tagCodeNames = await _relHelper.TagIdentitiesToCodeNames(values);
                            whereInValue = "'" + string.Join("','", tagCodeNames) + "'";
                            count = tagCodeNames.Count();
                            break;
                    }
                } else {
                    var summary = new ClassObjSummary(objectClassName);
                    switch (identity) {
                        case IdentityType.ID:
                            var tagIDs = await _relHelper.ObjectIdentitiesToIDs(summary, values);
                            whereInValue = string.Join(",", tagIDs);
                            count = tagIDs.Count();
                            break;
                        case IdentityType.Guid:
                            var tagGUIDs = await _relHelper.ObjectIdentitiesToGUIDs(summary, values);
                            whereInValue = "'" + string.Join("','", tagGUIDs) + "'";
                            count = tagGUIDs.Count();
                            break;
                        case IdentityType.CodeName:
                            var tagCodeNames = await _relHelper.ObjectIdentitiesToCodeNames(summary, values);
                            whereInValue = "'" + string.Join("','", tagCodeNames) + "'";
                            count = tagCodeNames.Count();
                            break;
                    }
                }
                if (count == 0) {
                    return "(1=1)";
                }
                switch (condition) {
                    case ConditionType.Any:
                    default:
                        return $"({objectIDFieldName} in (Select REBT.{leftFieldName} from {tableName} REBT where REBT.{rightFieldName} in ({whereInValue})))";
                    case ConditionType.All:
                        var ObjectIDTableName = new ClassObjSummary(objectClassName).TableName;
                        return $"(Select Count(*) from [{tableName}] REBT where REBT.[{leftFieldName}] = {(!string.IsNullOrWhiteSpace(ObjectIDTableName) ? "[" + ObjectIDTableName + "]." : "")}{objectIDFieldName} and REBT.{rightFieldName} in ({whereInValue})) = {count}";
                    case ConditionType.None:
                        return $"({objectIDFieldName} not in (Select REBT.{leftFieldName} from {tableName} REBT where REBT.{rightFieldName} in ({whereInValue})))";
                }
            }, new CacheSettings(_options.CacheMinutes, "GetBindingTagWhere", bindingClass.GetType().FullName, objectIDFieldName, leftFieldName, rightFieldName, string.Join("|", values), identity, condition));
        }

        /// <summary>
        /// Returns a full where condition (for Binding Tables that bind on any object) to be used in filtering (ex repeaters).  For property exampples, we will assume Demo.Foo, Demo.Bar, and Demo.FooBar
        /// </summary>
        /// <param name="bindingClass">The Binding Class Code Name.  Ex: Demo.FooBar</param>
        /// <param name="bindingCondition">The type of Condition filtering to be done.  Use FilterParentsByChildren for Where conditions on the Parent, passing Child values, and FilterChildrenByParents on the Child type, passing Parent values </param>
        /// <param name="values">list of object values (int IDs, GUIDs, or string CodeNames)</param>
        /// <param name="condition">RelEnums.ConditionType of what type of condition to generate.</param>
        /// <returns>The Where Condition, If no object values provided or none found, returns 1=1</returns>
        public async Task<string> GetBindingWhere(IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            string leftFieldName;
            string rightFieldName;
            string objectIDFieldName;
            IdentityType identity;
            string objectClassName;
            switch (bindingCondition) {
                case BindingConditionType.FilterParentsByChildren:
                default:
                    leftFieldName = _relHelper.GetBracketedColumnName(bindingClass.ParentObjectReferenceColumnName());
                    rightFieldName = _relHelper.GetBracketedColumnName(bindingClass.ChildObjectReferenceColumnName());
                    objectIDFieldName = _relHelper.GetBracketedColumnName(bindingClass.ParentClassReferenceColumn());
                    objectClassName = bindingClass.ParentClassName();
                    identity = bindingClass.ChildReferenceType();
                    break;
                case BindingConditionType.FilterChildrenByParents:
                    leftFieldName = _relHelper.GetBracketedColumnName(bindingClass.ChildObjectReferenceColumnName());
                    rightFieldName = _relHelper.GetBracketedColumnName(bindingClass.ParentObjectReferenceColumnName()); ;
                    objectIDFieldName = _relHelper.GetBracketedColumnName(bindingClass.ChildClassReferenceColumn());
                    objectClassName = bindingClass.ChildClassName();
                    identity = bindingClass.ParentReferenceType();
                    break;
            }
            return await _progressiveCache.LoadAsync(async cs => {
                // Find class table name

                var classObjSummary = await _relHelper.GetClassObjSummary(objectClassName);

                var whereInValue = "";
                var tableName = bindingClass.BindingTableName();
                var count = 0;
                switch (identity) {
                    case IdentityType.ID:
                        var objectIDs = await _relHelper.ObjectIdentitiesToIDs(classObjSummary, values);
                        whereInValue = (objectIDs.Count() > 0 ? string.Join(",", objectIDs) : "''");
                        count = objectIDs.Count();
                        break;
                    case IdentityType.Guid:
                        var objectGUIDs = await _relHelper.ObjectIdentitiesToGUIDs(classObjSummary, values);
                        whereInValue = "'" + string.Join("','", objectGUIDs) + "'";
                        count = objectGUIDs.Count();
                        break;
                    case IdentityType.CodeName:
                        var objectCodeNames = await _relHelper.ObjectIdentitiesToCodeNames(classObjSummary, values);
                        whereInValue = "'" + string.Join("','", objectCodeNames) + "'";
                        count = objectCodeNames.Count();
                        break;
                }

                // If no related object IDs found, then completely ignore.
                if (count == 0) {
                    return "(1=1)";
                }

                switch (condition) {
                    case ConditionType.Any:
                    default:
                        return $"({objectIDFieldName} in (Select REBT.{leftFieldName} from {tableName} REBT where REBT.{rightFieldName} in ({whereInValue})))";
                    case ConditionType.All:
                        string ObjectIDTableName = classObjSummary.TableName;
                        return $"(Select Count(*) from [{tableName}] REBT where REBT.[{leftFieldName}] = {(!string.IsNullOrWhiteSpace(ObjectIDTableName) ? "[" + ObjectIDTableName + "]." : "")}{objectIDFieldName} and REBT.{rightFieldName} in ({whereInValue})) = {count}";
                    case ConditionType.None:
                        return $"({objectIDFieldName} not in (Select REBT.{leftFieldName} from {tableName} REBT where REBT.{rightFieldName} in ({whereInValue})))";
                }
            }, new CacheSettings(_options.CacheMinutes, "GetBindingWhere", bindingClass.GetType().FullName, objectClassName, objectIDFieldName, leftFieldName, rightFieldName, string.Join("|", values), identity, condition));
        }


        #endregion

    }
}
