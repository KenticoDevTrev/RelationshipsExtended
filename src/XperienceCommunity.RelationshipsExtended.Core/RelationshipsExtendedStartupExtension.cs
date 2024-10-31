using XperienceCommunity.RelationshipsExtended;
using XperienceCommunity.RelationshipsExtended.Services;
using XperienceCommunity.RelationshipsExtended.Services.Implementations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RelationshipsExtendedStartupExtension
    {
        public static IServiceCollection AddRelationshipsExtended(this IServiceCollection serviceCollection)
        {
            var options = new RelationshipsExtendedOptions(serviceCollection);
            serviceCollection.AddRelationshipsExtendedInternal(options);
            return serviceCollection;
        }

        public static IServiceCollection AddRelationshipsExtended(this IServiceCollection serviceCollection, Action<RelationshipsExtendedOptions> configure)
        {
            var options = new RelationshipsExtendedOptions(serviceCollection);
            configure(options);

            serviceCollection.AddRelationshipsExtendedInternal(options);

            return serviceCollection;
        }


        private static IServiceCollection AddRelationshipsExtendedInternal(this IServiceCollection services, RelationshipsExtendedOptions options) =>
            services
               .AddSingleton(options)
               .AddSingleton<ILanguageSyncService, LanguageSyncService>()
               .AddSingleton<RelationshipsExtendedModuleInstaller>()
               .AddScoped<IContentItemCategoryUIService, DefaultContentItemCategoryUIService>();
    }

    
}
