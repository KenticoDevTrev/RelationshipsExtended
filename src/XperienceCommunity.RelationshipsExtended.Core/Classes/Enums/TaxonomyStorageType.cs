namespace XperienceCommunity.RelationshipsExtended.Classes.Enums
{
    public enum TaxonomyStorageType
    {
        /// <summary>
        /// Field Type is Taxonomy, stored like [{"Identifier":"70a69181-1149-4402-b618-71b2c7ed2e73"},{"Identifier":"887156fb-f54d-4737-a5dc-8ee59fa7150c"},{"Identifier":"64cde581-57be-4776-826e-ea191dbf0dab"}]
        /// </summary>
        TaxonomyIdentifier,
        /// <summary>
        /// For Object GUID Identifiers, stored like ["887156fb-f54d-4737-a5dc-8ee59fa7150c","70a69181-1149-4402-b618-71b2c7ed2e73","1757559e-5cfc-4e55-bacb-5583661e643f"]
        /// </summary>
        GuidArray,
        /// <summary>
        /// For Object Code Name Identifiers, stored like ["Blue","Green","Red"]
        /// </summary>
        CodeNameArray
    }
}
