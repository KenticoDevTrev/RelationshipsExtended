using CMS;
using CMS.DataEngine;
using CMS.MacroEngine;
using RelationshipsExtended;

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
        ///  Contains initialization code that is executed when the application starts
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();
            MacroContext.GlobalResolver.SetNamedSourceData("RelationshipsExtended", RelationshipsExtendedMacroNamespace.Instance);

        }
    }
}