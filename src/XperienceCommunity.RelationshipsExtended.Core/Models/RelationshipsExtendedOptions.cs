using Microsoft.Extensions.DependencyInjection;
using XperienceCommunity.RelationshipsExtended.Models;

namespace XperienceCommunity.RelationshipsExtended
{
    public class RelationshipsExtendedOptions
    {
        private readonly IServiceCollection _serviceCollection;

        public RelationshipsExtendedOptions(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        /// <summary>
        /// This enables "Content Item Categories" which are language agnostic (and are not part of workflow)
        /// 
        /// This is an optional strategy for content tagging, vs. a synced field or a child content item that has taxonomy and is not translated
        /// </summary>
        public bool AllowContentItemCategories { get; set; } = false;

        /// <summary>
        /// Allows language syncing of specified fields (LanguageSyncConfiguration) and/or through custom implementation of ILanguageSyncService
        /// </summary>
        public bool AllowLanguageSyncConfiguration { get; set; } = false;

        /// <summary>
        /// The Language Sync Configuration.
        /// </summary>
        public LanguageSyncConfiguration? LanguageSyncConfiguration { get; set; } = null;

        
    }
}
