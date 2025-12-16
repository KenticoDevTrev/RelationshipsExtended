using RelationshipsExtended.Enums;

namespace RelationshipsExtended.Interfaces
{
    /// <summary>
    /// Contains reference methods for Binding info objects, when defined allows for simpler execution of the Query Extensions
    /// </summary>
    public interface IBindingInfo : IBindingBaseInfo
    {
        /// <summary>
        /// This should return the column name found in the Parent class that is referenced by the this binding class's parent object id column.  (ex "NodeID")
        /// </summary>
        /// <returns></returns>
        string ParentClassReferenceColumn();

        /// <summary>
        /// The column name found in the Child class that is referenced by this binding class's child object id column. (ex "FooID")
        /// </summary>
        /// <returns></returns>
        string ChildClassReferenceColumn();

        /// <summary>
        /// The type of reference identity that is stored for the Parent object. (ex IdentityType.ID)
        /// </summary>
        /// <returns></returns>
        IdentityType ParentReferenceType();

        /// <summary>
        /// The type of reference identity that is stored for the Bound object. (ex IdentityType.ID)
        /// </summary>
        /// <returns></returns>
        IdentityType ChildReferenceType();

        /// <summary>
        /// The parent's class name. (ex "cms.tree")
        /// </summary>
        /// <returns></returns>
        string ParentClassName();

        /// <summary>
        /// The child's class name. (ex "demo.foo")
        /// </summary>
        /// <returns></returns>
        string ChildClassName();

        /// <summary>
        /// The table name of the this class
        /// </summary>
        /// <returns></returns>
        string BindingTableName();

        /// <summary>
        /// The order column for the child references, return empty or null if it is not ordable.
        /// </summary>
        /// <returns></returns>
        string OrderColumn();
    }
}
