using CMS.ContentEngine;
using RelationshipsExtended.Enums;
using RelationshipsExtended.Interfaces;
using XperienceCommunity.RelationshipsExtended.Classes.Enums;
using XperienceCommunity.RelationshipsExtended.Services;

namespace XperienceCommunity.RelationshipsExtended.Extensions
{
    public static class QueryExtensions
    {
        public static ContentTypeQueryParameters ContentItemCategoryCondition(this ContentTypeQueryParameters baseQuery, IRelationshipExtendedHelper relationshipExtendedHelper, IEnumerable<object> values, ConditionType condition = ConditionType.Any) => relationshipExtendedHelper.ContentItemCategoryCondition(baseQuery, values, condition);

        public static ContentTypeQueryParameters ContentItemLanguageCategoryCondition(this ContentTypeQueryParameters baseQuery, IRelationshipExtendedHelper relationshipExtendedHelper, string field, IEnumerable<object> values, TaxonomyStorageType taxonomyStorageType = TaxonomyStorageType.TaxonomyIdentifier) => relationshipExtendedHelper.ContentItemLanguageCategoryCondition(baseQuery, field, values, taxonomyStorageType);

        public static ContentTypeQueryParameters ContentCategoryConditionCustomBinding(this ContentTypeQueryParameters baseQuery, IRelationshipExtendedHelper relationshipExtendedHelper, IBindingInfo bindingClass, IEnumerable<object> values, ConditionType condition = ConditionType.Any) => relationshipExtendedHelper.CategoryConditionCustomBinding(baseQuery, bindingClass, values, condition);

        /* var tableName = "@tagIdentifiers";

            var parameters = new QueryDataParameters
            {
                { tableName , SqlHelper.BuildGuidTable(tagIdentifiers.ToGuidItems()), typeof(IEnumerable<GuidItem>)}
            };


            var where = new WhereCondition
            {
                Parameters = parameters,
                WhereCondition = $@"EXISTS (
    SELECT 1 
    FROM OPENJSON([{taxonomyColumnName}]) WITH (identifier UNIQUEIDENTIFIER '$.Identifier') 
    WHERE identifier IN (
        SELECT [Value] 
        FROM {tableName}
    )
    )"
            };*/
    }
}
