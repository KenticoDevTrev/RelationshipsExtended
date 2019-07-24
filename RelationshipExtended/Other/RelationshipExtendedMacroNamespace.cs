using CMS.Base;
using CMS.MacroEngine;

namespace RelationshipsExtended
{
    [Extension(typeof(RelationshipMacroMethods))]
    public class RelationshipsExtendedMacroNamespace : MacroNamespace<RelationshipsExtendedMacroNamespace>
    {

    }

    [Extension(typeof(RelHelperMacrosMethods))]
    public class RelHelperMacroNamespace : MacroNamespace<RelHelperMacroNamespace>
    {

    }
}