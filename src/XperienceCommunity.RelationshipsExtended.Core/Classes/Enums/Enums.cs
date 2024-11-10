namespace RelationshipsExtended.Enums
{
    public enum SelectorFieldSaveType { ID, GUID, String, CategoryName };
    public enum DisplayType { Tree, List };
    public enum SaveType { ToField, ToCategory, Both, BothNode, ToJoinTable };
    public enum CategoryFieldSaveType { ID, GUID, CategoryName };

    public enum BindingQueryType { GetChildrenByParent, GetChildrenByParentOrdered, GetParentsByChild}

    public enum BindingConditionType { FilterParentsByChildren, FilterChildrenByParents }

    /// <summary>
    /// The Binding Condition
    /// </summary>
    public enum ConditionType
    {
        /// <summary>
        /// Any of the given values match
        /// </summary>
        Any,
        /// <summary>
        /// All the binding values must be found (if 5 passed, it must have those 5)
        /// </summary>
        All,
        /// <summary>
        /// That none of the given values are found in the binding.
        /// </summary>
        None
    }

    /// <summary>
    /// The identity field type
    /// </summary>
    public enum IdentityType
    {
        /// <summary>
        /// Integar ID
        /// </summary>
        ID,
        /// <summary>
        /// Unique Identifier
        /// </summary>
        Guid,
        /// <summary>
        /// Kentico CodeName
        /// </summary>
        CodeName
    }
}
