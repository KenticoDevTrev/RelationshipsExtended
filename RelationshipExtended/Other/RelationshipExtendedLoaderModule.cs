using CMS;
using CMS.Core;
using CMS.DataEngine;
using CMS.MacroEngine;
using CMS.Modules;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using RelationshipsExtended;
using RelationshipsExtended.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

[assembly: RegisterModule(typeof(RelationshipsExtendedLoaderModule))]
namespace RelationshipsExtended
{
    /// <summary>
    /// 
    /// </summary>
    public class RelationshipsExtendedLoaderModule : Module
    {
        /// <summary>
        /// 
        /// </summary>
        public RelationshipsExtendedLoaderModule()
                : base("RelationshipsExtendedLoaderModule")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreInit()
        {
            base.OnPreInit();
            Service.Use(typeof(IRelationshipExtendedHelper), typeof(RelationshipsExtendedHelper));
        }

        /// <summary>
        ///  Contains initialization code that is executed when the application starts
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();
            MacroContext.GlobalResolver.SetNamedSourceData("RelationshipsExtended", RelationshipsExtendedMacroNamespace.Instance);
            ModulePackagingEvents.Instance.BuildNuSpecManifest.After += BuildNuSpecManifest_After;
        }

        private void BuildNuSpecManifest_After(object sender, BuildNuSpecManifestEventArgs e)
        {
            if (e.ResourceName.Equals("RelationshipsExtended", StringComparison.InvariantCultureIgnoreCase))
            {
                e.Manifest.Metadata.SetIconUrl("https://www.kentico.com/icons/icon-48x48.png");
                e.Manifest.Metadata.SetProjectUrl("https://github.com/KenticoDevTrev/RelationshipsExtended");
                e.Manifest.Metadata.ReleaseNotes = "Fixed all queries to set published(false) and latest version true and combine with any culture to prevent pages not loading due to workflow/multi-culture, added recursive to all macros on UI templates to allow dynamic values.";
                e.Manifest.Metadata.Copyright = "Heartland Business Systems";
                
                // Add dependencies
                List<PackageDependency> NetStandardDependencies = new List<PackageDependency>()
                {
                    new PackageDependency("Kentico.Xperience.Libraries", new VersionRange(new NuGetVersion("13.0.13")), new string[] { }, new string[] {"Build","Analyzers"}),
                    new PackageDependency("RelationshipsExtended.Base", new VersionRange(new NuGetVersion("13.0.7")), new string[] { }, new string[] {"Build","Analyzers"})
                };
                PackageDependencyGroup PackageGroup = new PackageDependencyGroup(new NuGet.Frameworks.NuGetFramework(".NETStandard2.0"), NetStandardDependencies);
                e.Manifest.Metadata.DependencyGroups = new PackageDependencyGroup[] { PackageGroup };
            }
        }
    }
}