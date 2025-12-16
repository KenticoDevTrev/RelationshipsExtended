using CMS.ContentEngine;
using CMS.DataEngine;
using RelationshipsExtended.Enums;
using RelationshipsExtended.Interfaces;

namespace XperienceCommunity.RelationshipsExtended.Services
{
    public interface IRelationshipExtendedHelper
    {

        #region "Retrieve Items by Parent-Child Relationship with Ordering possible"

        Task<ObjectQuery> InCustomRelationshipWithPossibleOrder(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true);
        Task<ObjectQuery<TObject>> InCustomRelationshipWithPossibleOrder<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true) where TObject : BaseInfo, new();
        Task<MultiObjectQuery> InCustomRelationshipWithPossibleOrder(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true);

        #endregion

        #region "Category Filtering"

        Task<ObjectQuery> BindingTagsCondition(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);
        Task<ObjectQuery<TObject>> BindingTagsCondition<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any) where TObject : BaseInfo, new();
        Task<MultiObjectQuery> BindingTagsCondition(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);
        Task<ContentTypeQueryParameters> BindingTagsCondition(ContentTypeQueryParameters baseQuery, IEnumerable<object> values, ContentItemConditionType condition = ContentItemConditionType.Any);

        #endregion

        #region "Custom Object Filtering"

        Task<ObjectQuery> BindingCondition(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);
        Task<ObjectQuery<TObject>> BindingCondition<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any) where TObject : BaseInfo, new();
        Task<MultiObjectQuery> BindingCondition(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);
  
        #endregion

    }
}
