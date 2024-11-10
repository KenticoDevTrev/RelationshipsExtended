using CMS.ContentEngine;
using CMS.DataEngine;
using RelationshipsExtended.Enums;
using RelationshipsExtended.Interfaces;
using XperienceCommunity.RelationshipsExtended.Classes.Enums;

namespace XperienceCommunity.RelationshipsExtended.Services.Implementations
{
    public class RelationshipExtendedHelper : IRelationshipExtendedHelper
    {
        public ObjectQuery BindingCategoryCondition(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            throw new NotImplementedException();
        }

        public ObjectQuery<TObject> BindingCategoryCondition<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any) where TObject : BaseInfo, new()
        {
            throw new NotImplementedException();
        }

        public MultiObjectQuery BindingCategoryCondition(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            throw new NotImplementedException();
        }

        public ContentTypeQueryParameters BindingCondition(ContentTypeQueryParameters baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            throw new NotImplementedException();
        }

        public ObjectQuery BindingCondition(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            throw new NotImplementedException();
        }

        public ObjectQuery<TObject> BindingCondition<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any) where TObject : BaseInfo, new()
        {
            throw new NotImplementedException();
        }

        public MultiObjectQuery BindingCondition(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            throw new NotImplementedException();
        }

        public ContentTypeQueryParameters CategoryConditionCustomBinding(ContentTypeQueryParameters baseQuery, IBindingInfo bindingClass, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            throw new NotImplementedException();
        }

        public ContentTypeQueryParameters ContentItemCategoryCondition(ContentTypeQueryParameters baseQuery, IEnumerable<object> values, ConditionType condition = ConditionType.Any)
        {
            throw new NotImplementedException();
        }

        public ContentTypeQueryParameters ContentItemLanguageCategoryCondition(ContentTypeQueryParameters baseQuery, string field, IEnumerable<object> values, TaxonomyStorageType taxonomyStorageType = TaxonomyStorageType.TaxonomyIdentifier, ConditionType condition = ConditionType.Any)
        {
            throw new NotImplementedException();
        }

        public ContentTypeQueryParameters InCustomRelationshipWithPossibleOrder(ContentTypeQueryParameters baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true)
        {
            throw new NotImplementedException();
        }

        public ObjectQuery InCustomRelationshipWithPossibleOrder(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true)
        {
            throw new NotImplementedException();
        }

        public ObjectQuery<TObject> InCustomRelationshipWithPossibleOrder<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true) where TObject : BaseInfo, new()
        {
            throw new NotImplementedException();
        }

        public MultiObjectQuery InCustomRelationshipWithPossibleOrder(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true)
        {
            throw new NotImplementedException();
        }
    }
}
