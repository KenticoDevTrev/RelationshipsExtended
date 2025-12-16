using CMS.ContentEngine;
using RelationshipsExtended.Enums;
using XperienceCommunity.RelationshipsExtended.Services;

namespace XperienceCommunity.RelationshipsExtended.Extensions
{
    public static class QueryExtensions
    {
        public static async Task<ContentTypeQueryParameters> ContentItemCategoryCondition(this ContentTypeQueryParameters baseQuery, IRelationshipsExtendedHelper relationshipExtendedHelper, IEnumerable<object> values, ContentItemConditionType condition = ContentItemConditionType.Any) => await relationshipExtendedHelper.BindingTagsCondition(baseQuery, values, condition);

    }
}
