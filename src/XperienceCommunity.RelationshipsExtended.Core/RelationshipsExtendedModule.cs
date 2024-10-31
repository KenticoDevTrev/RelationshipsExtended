using XperienceCommunity.RelationshipsExtended.Web.Admin;
using CMS.Core;
using Microsoft.Extensions.DependencyInjection;
using CMS.Base;
using CMS.DataEngine;
using CMS.ContentEngine;
using XperienceCommunity.RelationshipsExtended.Services;

[assembly: CMS.RegisterModule(typeof(RelationshipsExtendedModule))]

namespace XperienceCommunity.RelationshipsExtended.Web.Admin
{
    internal class RelationshipsExtendedModule : Module
    {
        private RelationshipsExtendedModuleInstaller? _installer;
        private RelationshipsExtendedOptions? _options;
        private IServiceProvider? _services;
        public const string CUSTOM_CATEGORY = "xperiencecommunity.relationshipsextended.category";

        public RelationshipsExtendedModule()
            : base("RelationshipsExtendedModule")
        {
        }

        protected override void OnInit(ModuleInitParameters parameters)
        {
            base.OnInit();

            _services = parameters.Services;
            _installer = _services.GetRequiredService<RelationshipsExtendedModuleInstaller>();
            _options = _services.GetRequiredService<RelationshipsExtendedOptions>();
            ApplicationEvents.Initialized.Execute += InitializeModule;
            if (_options.AllowLanguageSyncConfiguration) {
                ContentItemEvents.Publish.Execute += LanguageSync_Publish_Execute;
            }
        }

        private void LanguageSync_Publish_Execute(object? sender, PublishContentItemEventArgs e)
        {
            // TODO: use information and options to execute
            if(_services == null) {
                return;
            }
            var languageSyncService = _services.GetRequiredService<ILanguageSyncService>();
        }

        private void InitializeModule(object? sender, EventArgs e)
        {
            _installer?.Install();
        }
    }
}
