using CMS;
using CMS.DataEngine;
using CMS.MacroEngine;
using RelationshipsExtended;
using RelationshipsExtended.Enums;

[assembly: RegisterModule(typeof(RelationshipsExtendedLoaderModule))]
namespace RelationshipsExtended
{
    /// <summary>
    /// Adds Macros to engine
    /// </summary>
    public class RelationshipsExtendedLoaderModule : Module
    {
        /// <summary>
        /// Initializer
        /// </summary>
        public RelationshipsExtendedLoaderModule()
                : base("RelationshipsExtendedLoaderModule")
        {
        }

        /// <summary>
        /// Contains initialization code that is executed when the application starts
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();

            // Registers "CustomNamespace" into the macro engine
            MacroContext.GlobalResolver.SetNamedSourceData("RelHelper", RelHelperMacroNamespace.Instance);
            MacroContext.GlobalResolver.SetNamedSourceData("RelEnums", EnumMacroEvaluator.EnumMacroObjects());

        }
    }

}
