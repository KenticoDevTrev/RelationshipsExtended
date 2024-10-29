using CMS.ContentEngine;

namespace XperienceCommunity.RelationshipsExtended.Services
{
    public interface ILanguageSyncService
    {
        Task HandleLanguageSync(ContentItemDataEventContainer originContentItem, ContentItemDataEventContainer destinationContentItem, IReadOnlyList<string> fieldsToSync);
    }
}
