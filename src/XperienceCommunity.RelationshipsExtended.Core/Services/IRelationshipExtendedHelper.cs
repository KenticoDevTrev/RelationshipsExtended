using CMS.ContentEngine;
using CMS.DataEngine;
using RelationshipsExtended.Enums;
using RelationshipsExtended.Interfaces;
using XperienceCommunity.RelationshipsExtended.Classes.Enums;

namespace XperienceCommunity.RelationshipsExtended.Services
{
    public interface IRelationshipExtendedHelper
    {

        #region "Retrieve Items by Parent-Child Relationship with Ordering possible"

        // Use the binding information and type to generate the where condition
        ContentTypeQueryParameters InCustomRelationshipWithPossibleOrder(ContentTypeQueryParameters baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true);

        // Use the binding information and type to generate the where condition
        ObjectQuery InCustomRelationshipWithPossibleOrder(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true);

        // Use the binding information and type to generate the where condition
        ObjectQuery<TObject> InCustomRelationshipWithPossibleOrder<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true) where TObject : BaseInfo, new();

        // Use the binding information and type to generate the where condition
        MultiObjectQuery InCustomRelationshipWithPossibleOrder(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingQueryType bindingType, object inRelationshipWithValue, bool orderAsc = true);

        #endregion

        #region "Category Filtering"

        // Using the Relationships Extended ContentItemCategory table
        ContentTypeQueryParameters ContentItemCategoryCondition(ContentTypeQueryParameters baseQuery, IEnumerable<object> values, ConditionType condition = ConditionType.Any);

        // Use custom SQL parsing of the field values based on the storage type
        ContentTypeQueryParameters ContentItemLanguageCategoryCondition(ContentTypeQueryParameters baseQuery, string field, IEnumerable<object> values, TaxonomyStorageType taxonomyStorageType = TaxonomyStorageType.TaxonomyIdentifier, ConditionType condition = ConditionType.Any);

        // Use IBindingInfo's parent/child type to find the taxonomy type and it's storage type, then use the OTHER type (parent or child) and it's type)
        ContentTypeQueryParameters CategoryConditionCustomBinding(ContentTypeQueryParameters baseQuery, IBindingInfo bindingClass, IEnumerable<object> values, ConditionType condition = ConditionType.Any);

        // Use IBindingInfo's parent/child type to find the taxonomy type and it's storage type, then use the OTHER type (parent or child) and it's type)
        ObjectQuery BindingCategoryCondition(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);

        // Use IBindingInfo's parent/child type to find the taxonomy type and it's storage type, then use the OTHER type (parent or child) and it's type)
        ObjectQuery<TObject> BindingCategoryCondition<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any) where TObject : BaseInfo, new();

        // Use IBindingInfo's parent/child type to find the taxonomy type and it's storage type, then use the OTHER type (parent or child) and it's type)
        MultiObjectQuery BindingCategoryCondition(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);

        #endregion

        #region "Custom Object Filtering"

        // Use info on IBindingInfo to generate proper query, may have to translate values if not right type
        ContentTypeQueryParameters BindingCondition(ContentTypeQueryParameters baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);

        // Use info on IBindingInfo to generate proper query, may have to translate values if not right type
        ObjectQuery BindingCondition(ObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);

        // Use info on IBindingInfo to generate proper query, may have to translate values if not right type
        ObjectQuery<TObject> BindingCondition<TObject>(ObjectQuery<TObject> baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any) where TObject : BaseInfo, new();

        // Use info on IBindingInfo to generate proper query, may have to translate values if not right type
        MultiObjectQuery BindingCondition(MultiObjectQuery baseQuery, IBindingInfo bindingClass, BindingConditionType bindingCondition, IEnumerable<object> values, ConditionType condition = ConditionType.Any);

        #endregion

    }
}
