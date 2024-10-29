using XperienceCommunity.RelationshipsExtended.Models;

namespace XperienceCommunity.RelationshipsExtended.Services
{
    public interface IContentItemCategoryUIService
    {
        /// <summary>
        /// Gets the ContentItemCategoryUIOptions for the given contentItem.  Can implement your own to customize.
        /// </summary>
        /// <param name="contentItemID">The Content Item ID</param>
        /// <returns>The Configuration Options</returns>
        Task<ContentItemCategoryUIOptions> GetCategoryUIOptions(int contentItemID);
    }
}
