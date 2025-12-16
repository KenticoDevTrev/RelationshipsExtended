using XperienceCommunity.RelationshipsExtended.Models;

namespace XperienceCommunity.RelationshipsExtended.Services.Implementations
{
    public class DefaultContentItemCategoryUIService(RelationshipsExtendedOptions options) : IContentItemCategoryUIService
    {
        public RelationshipsExtendedOptions Options { get; } = options;

        public Task<ContentItemCategoryUIOptions> GetCategoryUIOptions(int contentItemID)
        {
            return Task.FromResult(new ContentItemCategoryUIOptions() {
                TaxonomyNames = [],
                Enabled = Options.AllowContentItemCategories
            });
        }
    }
}
