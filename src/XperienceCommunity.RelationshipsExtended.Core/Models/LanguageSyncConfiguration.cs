using CMS.ContentEngine;

namespace XperienceCommunity.RelationshipsExtended.Models
{
    public record LanguageSyncConfiguration(IEnumerable<LanguageSyncClassConfiguration> ContentItemConfigurations, IEnumerable<string> ReusableFields, string? SyncConfigReusableFieldName = null);

    public record LanguageSyncClassConfiguration(string ContentItemType, IEnumerable<string> LanguageSyncFields);

}
