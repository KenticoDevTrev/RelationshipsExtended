namespace RelationshipsExtended.Interfaces
{
    /// <summary>
    /// Binding Interface used for the Edit Bindings with Node + Orderable Support, ____Info class implement these for the UI to work.
    /// </summary>
    public interface IBindingBaseInfo
    {
        /// <summary>
        /// This should return the column name of the parent object id (ex "NodeID")
        /// </summary>
        /// <returns>The Column name that contains the Parent ID reference</returns>
        string ParentObjectReferenceColumnName();

        /// <summary>
        /// This should return the column name of the bound object's id (ex "BannerID")
        /// </summary>
        /// <returns>The column name that contains the bound ID reference</returns>
        string BoundObjectReferenceColumnName();
    }
}
