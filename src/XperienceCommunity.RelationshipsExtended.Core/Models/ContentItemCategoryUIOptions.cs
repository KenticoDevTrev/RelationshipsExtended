namespace XperienceCommunity.RelationshipsExtended.Models
{
    public record ContentItemCategoryUIOptions
    {
        /// <summary>
        /// If provided, will only show Categories (Tags) in the given Taxonomy Groups by Code Name
        /// </summary>
        public string[] TaxonomyNames { get; set; } = [];

        /// <summary>
        /// If the UI Should display, will always be false if Content Categories are not enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}
