using XperienceCommunity.RelationshipsExtended.Web.Admin;

using Kentico.Xperience.Admin.Base;
using CMS.Core;
using Microsoft.Extensions.DependencyInjection;
using CMS.Base;
using System;

 [assembly: CMS.RegisterModule(typeof(RelationshipsExtendedWebAdminModule))]

// Adds a new application category 
[assembly: UICategory(RelationshipsExtendedWebAdminModule.CUSTOM_CATEGORY, "Custom", Icons.CustomElement, 100)]

namespace XperienceCommunity.RelationshipsExtended.Web.Admin
{
    internal class RelationshipsExtendedWebAdminModule : AdminModule
    {
        private RelationshipsExtendedModuleInstaller installer = null!;


        public const string CUSTOM_CATEGORY = "xperiencecommunity.relationshipsextended.web.admin.category";

        public RelationshipsExtendedWebAdminModule()
            : base("RelationshipsExtendedWebAdminModule")
        {
        }

        protected override void OnInit(ModuleInitParameters parameters)
        {
            base.OnInit();

            // Makes the module accessible to the admin UI
            RegisterClientModule("xperiencecommunity.relationshipsextended", "web-admin");
        }
    }
}
